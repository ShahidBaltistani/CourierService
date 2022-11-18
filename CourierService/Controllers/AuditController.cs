using CourierService.Entities.Orders;
using CourierService.Services;
using CourierService.Services.Orders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CourierService.Controllers
{
    [Authorize]
    public class AuditController : Controller
    {
        // GET: Audit
        class DeliveredPatientsModel
        {
            public int OI { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public string Practice { get; set; }
            public string Customer_Reference { get; set; }
            public DateTime? Delivery_Status { get; set; }
            public string Phone { get; set; }
            public string EHR { get; set; }
            public DateTime? Order_Status { get; set; }
            public string State { get; set; }
        }
        public class OrderStatus
        {
            public int Booking { get; set; } = 0;
            public int Pickup { get; set; } = 0;
            public int Pickup_Attempted { get; set; } = 0;
            public int Transit { get; set; } = 0;
            public int Delivered { get; set; } = 0;
            public int Cancelled { get; set; } = 0;
            public int Unable_to_Book { get; set; } = 0;
            public int Lost { get; set; } = 0;
            public int Return_to_Sender { get; set; } = 0;

            public int Total
            {
                get
                {
                    return Booking + Pickup + Pickup_Attempted + Transit + Delivered + Cancelled + Unable_to_Book + Lost + Return_to_Sender;
                }
            }

        }
        // Adding Service
        OrderService orderService = new OrderService();
        APIResponseService aPIResponseService = new APIResponseService();
        public async Task<ActionResult> GetDataByPracticeId(string PracticeId)
        {
            var data = orderService.GetDataByPracticeId(PracticeId);
            string JSONresult = JsonConvert.SerializeObject(data);
            var model = JsonConvert.DeserializeObject<List<DeliveredPatientsModel>>(JSONresult);
            var Patientswithdeliverystatusnotnull = model.Where(x => x.Delivery_Status != null).ToList();

            var Patientswithdeliverystatusnull = model.Where(x=>x.Delivery_Status==null).ToList();
            foreach (var item in Patientswithdeliverystatusnull)
            {
                var SendleReference = aPIResponseService.GetByOrderId(item.OI);
                if (SendleReference != null)
                {
                    string Sendle_OrderId = SendleReference.SendleOrderId;
                    using (var client = Utility.GetHttpClient())
                    {
                        var responseTask = client.GetAsync("https://api.sendle.com/api/orders/" + Sendle_OrderId + "");
                        responseTask.Wait();
                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var response = await result.Content.ReadAsStringAsync();
                            dynamic data1 = JObject.Parse(response);
                            string state = data1.state;
                            item.State = state;
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Server error try after some time.");
                        }
                    }
                }
            }

            var LatestList = Patientswithdeliverystatusnotnull.Concat(Patientswithdeliverystatusnull);

            var gv = new GridView();
            gv.DataSource = LatestList;
            gv.DataBind();
            Response.ClearContent();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment; filename=Audit.xls");
            Response.ContentType = "application/ms-excel";
            Response.Charset = "";
            StringWriter objStringWriter = new StringWriter();
            HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
            gv.RenderControl(objHtmlTextWriter);
            Response.Output.Write(objStringWriter.ToString());
            Response.Flush();
            Response.End();


            return View();
        }
        public async Task<ActionResult> PracticeSummary(string PracticeId)
        {
            OrderStatus statusObj = new OrderStatus();
            var PracticeId1 = PracticeId.Replace("_", " ");
            var data = orderService.GetDataByPracticeId(PracticeId1);
            string JSONresult = JsonConvert.SerializeObject(data);
            var model = JsonConvert.DeserializeObject<List<DeliveredPatientsModel>>(JSONresult);
            var Patientswithdeliverystatusnotnull = model.Where(x => x.Delivery_Status != null).ToList();

            var Patientswithdeliverystatusnull = model.Where(x => x.Delivery_Status == null).ToList();
            foreach (var item in Patientswithdeliverystatusnull)
            {
                var SendleReference = aPIResponseService.GetByOrderId(item.OI);
                if (SendleReference != null)
                {
                    string Sendle_OrderId = SendleReference.SendleOrderId;
                    using (var client = Utility.GetHttpClient())
                    {
                        var responseTask = client.GetAsync("https://api.sendle.com/api/orders/" + Sendle_OrderId + "");
                        responseTask.Wait();
                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var response = await result.Content.ReadAsStringAsync();
                            dynamic data1 = JObject.Parse(response);
                            string state = data1.state;
                            item.State = state;
                            if (state == "Booking")
                            {
                                statusObj.Booking++;
                            }
                            else if (state == "Pickup")
                            {
                                statusObj.Pickup++;
                            }
                            else if (state == "Pickup Attempted")
                            {
                                statusObj.Pickup_Attempted++;
                            }
                            else if (state == "Transit")
                            {
                                statusObj.Transit++;
                            }
                            else if (state == "Cancelled")
                            {
                                statusObj.Cancelled++;
                            }
                            else if (state == "Unable to Book")
                            {
                                statusObj.Unable_to_Book++;
                            }
                            else if (state == "Lost")
                            {
                                statusObj.Lost++;
                            }
                            else if (state == "Return to Sender")
                            {
                                statusObj.Return_to_Sender++;
                            }
                        }
                    }
                }

            }

            statusObj.Delivered = Patientswithdeliverystatusnotnull.Count();
            return Json(statusObj, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ViewSummary()
        {
            return View();
        }
    }
}