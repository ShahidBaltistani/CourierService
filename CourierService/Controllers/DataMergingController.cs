using CourierService.Services.NewPracticeF;
using CourierService.Services.Orders;
using CourierService.ViewModels.ExternalDB;
using CourierService.ViewModels.Orders;
using Newtonsoft.Json;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CourierService.Controllers
{
    [Authorize]
    // GET: DataMerging
    public class DataMergingController : Controller
    {
        NewPracticeService newPractice = new NewPracticeService();
        OrderService Service = new OrderService();
        #region
        public ActionResult ConnectionClass()
        {
            DataTable dt = new DataTable();
            try
            {
                SqlCommand command = new SqlCommand();
                SqlConnection connection = new SqlConnection();

                connection.ConnectionString = "data source=34.73.221.148;initial catalog=RPD_Courier_New; user=sa;password=360VUSolutions;Persist Security Info=True";
                connection.Open();
                //Console.WriteLine("Connection Open  !");
                command.Connection = connection;
                command.CommandType = CommandType.Text;
                command.CommandText = "select * from Orders";

                var data1 = command.ExecuteReader();
                dt.Load(data1);
                string JSONresult = JsonConvert.SerializeObject(dt);
                var model = JsonConvert.DeserializeObject<List<OrderForEDB>>(JSONresult);
                connection.Close();

                var res = Insertion(model);
                if (res == true)
                {
                    return Json("Success.....", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json("Please try again or no new records found.....", JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public bool Insertion(List<OrderForEDB> GetAllFromRPD_Courier)
        {
            if (GetAllFromRPD_Courier.Count > 0)
            {
                SqlCommand command = new SqlCommand();
                SqlConnection connection = new SqlConnection();
                connection.ConnectionString = "data source=awsrpd.cuacmx9wkp9o.us-east-2.rds.amazonaws.com; initial catalog = StreamVitalspk; user=Haroon;password=haroon@123;";
                connection.Open();
                command.Connection = connection;
                command.CommandType = CommandType.Text;


                DataTable TableForAllOrders = new DataTable();
                command.CommandText = "select * from Order_Latest";
                command.ExecuteNonQuery();
                var AllOrders = command.ExecuteReader();
                TableForAllOrders.Load(AllOrders);
                string JSONresult = JsonConvert.SerializeObject(TableForAllOrders);
                var GetAllFromStreamvitalspk = JsonConvert.DeserializeObject<List<OrderForSVPKDB>>(JSONresult);

                var filtered = GetAllFromRPD_Courier.Where(x => !GetAllFromStreamvitalspk.Any(y => y.CourierOrderId == x.Id));
                if (filtered.Count() > 0)
                {
                    // Insert Query
                    foreach (var item in filtered)
                    {
                        command.CommandText =
                        "insert into Order_Latest (CourierOrderId,Customer_Reference,ReceiverId,CreatedOn,PracticeId,IsDelivered,DeliveredDate,EHR,SourceId,DeviceTypeId) " +
                        "values ('" + item.Id + "','" + item.Customer_Reference + "','" + item.ReceiverId + "','" + item.CreatedOn + "','" + item.PracticeId + "','" + item.IsDelivered + "','" + item.DeliveredDate + "','" + item.EHR + "','" + item.SourceId + "','" + item.DeviceTypeId + "')";
                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    return false;
                }
                // Insert Single Column Just like EHR# Query

                //foreach (var rpd in GetAllFromRPD_Courier)
                //{
                //    command.CommandText = " UPDATE Orders SET EHRNumber='" + rpd.EHR + "' where ReceiverId ='" + rpd.ReceiverId + "'";
                //    command.ExecuteNonQuery();
                //}
                connection.Close();
                return true;
            }
            else
            {
                return false;
            }
        }

        //public ActionResult DistinctConsentedPatients()
        //{
        //    DataTable dt = new DataTable();
        //    try
        //    {
        //        SqlCommand command = new SqlCommand();
        //        SqlConnection connection = new SqlConnection();
        //        SqlParameter param;


        //        connection.ConnectionString = "data source=awsrpd.cuacmx9wkp9o.us-east-2.rds.amazonaws.com; initial catalog = StreamVitalspk; user=Haroon;password=haroon@123;";
        //        connection.Open();
        //        command.Connection = connection;
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = "RPDCourier.RPDCourier_GetPaitents";

        //        param = new SqlParameter("@Show", "1");
        //        param.Direction = ParameterDirection.Input;
        //        param.DbType = DbType.String;
        //        command.Parameters.Add(param);

        //        var data1 = command.ExecuteReader();
        //        dt.Load(data1);
        //        string JSONresult = JsonConvert.SerializeObject(dt);
        //        var model = JsonConvert.DeserializeObject<List<PatientViewModel>>(JSONresult);
        //        connection.Close();


        //        //var products = new DataTable("teste");

        //        //products.Columns.Add("instructions", typeof(string));

        //        //products.Columns.Add("Name", typeof(string));

        //        //products.Columns.Add("email", typeof(string));

        //        //products.Columns.Add("Phone", typeof(string));

        //        //products.Columns.Add("address_line1", typeof(string));

        //        //products.Columns.Add("address_line2", typeof(string));

        //        //products.Columns.Add("suburb", typeof(string));

        //        //products.Columns.Add("postcode", typeof(string));

        //        //products.Columns.Add("state", typeof(string));

        //        //products.Columns.Add("country", typeof(string));

        //        //products.Columns.Add("customer_reference", typeof(string));

        //        //products.Columns.Add("PracticeId", typeof(string));

        //        //products.Columns.Add("EHR#", typeof(string));

        //        //foreach (var item in model)
        //        //{
        //        //    products.Rows.Add("", item.FirstName +" " + item.LastName, "", item.ContactNo, "", item.Address, "", "", "", "", "", "", item.EHRNumber);
        //        //} 




        //        //var gv = new GridView();
        //        //gv.DataSource = products;
        //        //gv.DataBind();

        //        //Response.ClearContent();
        //        //Response.Buffer = true;
        //        //Response.AddHeader("content-disposition", "attachment; filename=DemoExcel.xls");
        //        //Response.ContentType = "application/ms-excel";

        //        //Response.Charset = "";
        //        //StringWriter objStringWriter = new StringWriter();
        //        //HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

        //        //gv.RenderControl(objHtmlTextWriter);

        //        //Response.Output.Write(objStringWriter.ToString());
        //        //Response.Flush();
        //        //Response.End();

        //        return View(model);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        //[HttpPost]
        //public ActionResult OrdersListSave(List<OrderViewModel> orders)
        //{
        //    var OrdersList = new List<OrderViewModel>();
        //    foreach (var item in orders)
        //    {
        //        OrderViewModel model = new OrderViewModel();
        //        model.Receiver = new ReceiverViewModel();


        //        model.Receiver.Instructions = item.Receiver.Instructions;
        //        model.Receiver.Name = item.Receiver.Name;
        //        model.Receiver.Phone = item.Receiver.Phone;
        //        model.Receiver.Email = item.Receiver.Email;
        //        model.Receiver.Address_Line1 = item.Receiver.Address_Line1;
        //        model.Receiver.Address_Line2 = item.Receiver.Address_Line2;
        //        model.Receiver.Suburb = item.Receiver.Suburb;
        //        model.Receiver.Postcode = item.Receiver.Postcode;
        //        model.Receiver.State_Name = item.Receiver.State_Name;
        //        model.Receiver.Country = item.Receiver.Country;
        //        model.Customer_Reference = item.Customer_Reference;
        //        model.PracticeId = item.PracticeId;
        //        model.EHR = item.EHR;

        //        //BaseEntity
        //        model.CreatedBy = Convert.ToInt32(Session["UserId"]);
        //        model.CreatedOn = DateTime.Now;
        //        model.Receiver.CreatedBy = Convert.ToInt32(Session["UserId"]);
        //        model.Receiver.CreatedOn = DateTime.Now;

        //        OrdersList.Add(model);
        //    }
        //    var responce = Service.Add(OrdersList);
        //    if (!responce)
        //    {
        //        ModelState.AddModelError(string.Empty, "Server error try after some time.");
        //    }
        //    return Json(responce);
        //}

        //public ActionResult PdfMerging()
        //{
        //    using (PdfDocument one = PdfReader.Open("file1.pdf", PdfDocumentOpenMode.Import))
        //    using (PdfDocument two = PdfReader.Open("file2.pdf", PdfDocumentOpenMode.Import))
        //    using (PdfDocument outPdf = new PdfDocument())
        //    {
        //        CopyPages(one, outPdf);
        //        CopyPages(two, outPdf);

        //        outPdf.Save("file1and2.pdf");
        //    }
        //    void CopyPages(PdfDocument from, PdfDocument to)
        //    {
        //        for (int i = 0; i < from.PageCount; i++)
        //        {
        //            to.AddPage(from.Pages[i]);
        //        }
        //    }
        //    return View();
        //}
        #endregion Old Code For Merging

        public ActionResult DistinctConsentedPatients()
        {
            try
            {
                IEnumerable<ConsentedPatientsViewModel> consenteds;
                // For View
                List<ConsentedPatientsViewModel> ConsentedsPatients=new List<ConsentedPatientsViewModel>();

                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    client.DefaultRequestHeaders.Add("CourierKey", "c8c6742e-5487-45df-9acf-1e723b1386d1");
                    var responseTask = client.GetAsync("http://svrpdapi.vu360solutions.org/courierservice/getallconsentedpaitents");
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var readTask = result.Content.ReadAsAsync<IList<ConsentedPatientsViewModel>>();
                        readTask.Wait();
                        consenteds = readTask.Result;
                    }
                    else
                    {
                        //Error response received   
                        consenteds = Enumerable.Empty<ConsentedPatientsViewModel>();
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                    
                }
                foreach (var item in consenteds)
                {
                    var data = Service.CheckDuplicateWithEHR(item.ehrNumber);

                    if (data == null)
                    {
                        ConsentedsPatients.Add(item);
                    }
                }
                return View(ConsentedsPatients);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult OrdersListSave(List<OrderViewModel> orders)
        {
            var OrdersList = new List<OrderViewModel>();
            foreach (var item in orders)
            {
                OrderViewModel model = new OrderViewModel();
                model.Receiver = new ReceiverViewModel();


                model.Receiver.Instructions = item.Receiver.Instructions;
                model.Receiver.Name = item.Receiver.Name;
                model.Receiver.Phone = item.Receiver.Phone;
                model.Receiver.Email = item.Receiver.Email;
                model.Receiver.Address_Line1 = item.Receiver.Address_Line1;
                model.Receiver.Address_Line2 = item.Receiver.Address_Line2;
                model.Receiver.Suburb = item.Receiver.Suburb;
                model.Receiver.Postcode = item.Receiver.Postcode;
                model.Receiver.State_Name = item.Receiver.State_Name;
                model.Receiver.Country = item.Receiver.Country;
                model.Customer_Reference = item.Customer_Reference;
                // Save Practice Name
                model.PracticeId = item.PracticeId;
                // Save Practice Id
                var data = newPractice.GetIdByPracticeName(item.PracticeId);
                if (data != null)
                {
                    model.NewPracticeId = data.Id;
                }
                else
                {
                    return Json(new { result = false, text = "Please Use Correct Practice Name..." });
                }


                model.Receiver.EHR = item.Receiver.EHR;

                model.Receiver.SVPatientId = item.Receiver.SVPatientId;

                model.DeviceTypeId = item.DeviceTypeId;
                model.SourceId = item.SourceId;

                //BaseEntity
                model.CreatedBy = Convert.ToInt32(Session["UserId"]);
                model.CreatedOn = DateTime.Now;
                model.Receiver.CreatedBy = Convert.ToInt32(Session["UserId"]);
                model.Receiver.CreatedOn = DateTime.Now;

                OrdersList.Add(model);
            }
            var responce = Service.Add(OrdersList);
            if (!responce)
            {
                return Json(new { result = false, text = "Please Use Correct Practice Name..." });
            }
            return Json(responce);
        }

        public ActionResult NewPracticeDropdown()
        {
            var data = newPractice.GetAll();
            return Json(data, JsonRequestBehavior.AllowGet);

        }
    }
}