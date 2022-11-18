using CourierService.Services;
using CourierService.Services.Bulk;
using CourierService.Services.OrderLogs;
using CourierService.Services.Orders;
using CourierService.ViewModels.Filters;
using CourierService.ViewModels.Orders;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection.Emit;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Script.Serialization;

namespace CourierService.Controllers
{
    [Authorize]
    public class ReceiverListController : Controller
    {
        // Adding Service
        OrderService orderService = new OrderService();
        APIResponseService aPIResponseService = new APIResponseService();
        OrderLabelService orderLabelService = new OrderLabelService();
        OrderLogService _orderLogService = new OrderLogService();
        BulkService _Bulk = new BulkService();
        //
        public ActionResult ReceiverList()
        {
            return View(orderService.GetAll());
        }

        public ActionResult EditReceiversDetail()
        {
            return View();
        }

        public ActionResult GetOrderName(int id)
        {
            var getdata = orderService.GetById(id);
            return Json(getdata, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UpdateCRSingle(string Customer_Reference,int Id)
        {
            var data=orderService.UpdateCustomerRefrenceSingle(Customer_Reference,Id);
            return RedirectToAction("ReceiverList");
        }


        public ActionResult UpdateCustomerRefrenceInProcess(List<OrderViewModel> model)
        {
            
            foreach (var item in model)
            {
                var data = orderService.UpdateCustomerRefrence(item.Customer_Reference, item.Id);
            }
                return RedirectToAction("GetInprocessOrder");
           
        
        }

        public ActionResult UpdateCustomerRefrenceProcessed(List<OrderViewModel> model)
        {
            foreach (var item in model)
            {
                var data = orderService.UpdateCustomerRefrence(item.Customer_Reference, item.Id);
            }
            return RedirectToAction("GetAllProcessedOrders");
        }

        public ActionResult GetAllCanceledOrders()
        {
            var data = orderService.GetAllCanceledOrders();
            return PartialView("_Cancelled", data);
        }

        public ActionResult GetAllAccessories()
        {
            var data = orderService.GetAllAccessories();
            return PartialView("_Accessories", data);
        }

        public ActionResult GetAllBulkPackaging()
        {
            //var data = orderService.GetAllBulkPackaging();

            var data = _Bulk.GetAll();
            return PartialView("_BulkPackaging", data);
        }

        public ActionResult GetAllReturns()
        {
            var data = orderService.GetAllReturns();
            return PartialView("_Returns", data);
        }

        public ActionResult GetAllReplacements()
        {
            var data = orderService.GetReplacementOrders();
            return PartialView("_Replacement", data);
        }

        public ActionResult GetAllMiscellaneousOrders()
        {
            var data = orderService.GetAllMiscellaneousOrders();
            return PartialView("_MiscellaneousOrders", data);
        }
        public ActionResult GetAllSubMiscellaneousOrders()
        {
            var data = orderService.GetAllMiscellaneousOrders();
            return PartialView("_MiscellaneousSubOrders", data);
        }

        public ActionResult GetAllExpireOrders()
        {
            var data = orderService.GetAllExpireOrders();
            return PartialView("_ExpiresSubOrders", data);
        }

        public /*async Task<ActionResult>*/ ActionResult  GetAllProcessedOrders()
        {
            //var orders = orderService.GetAllProcessedOrders().Where(x => x.IsDelivered == false).ToList();
            //foreach (var item in orders)
            //{
            //    var SendleReference = aPIResponseService.GetByOrderId(item.Id);

            //    if (SendleReference != null)
            //    {
            //        string Sendle_Reference = SendleReference.SendleRefrence;

            //        using (var client = Utility.GetHttpClient())
            //        {
            //            var responseTask = client.GetAsync("https://api.sendle.com/api/tracking/" + Sendle_Reference + "");
            //            responseTask.Wait();
            //            var result = responseTask.Result;
            //            if (result.IsSuccessStatusCode)
            //            {
            //                var response = await result.Content.ReadAsStringAsync();
            //                dynamic data1 = JObject.Parse(response);
            //                var state = data1.state;
            //                string ddate = data1.status.last_changed_at;
            //                var newstring = ddate.Remove(ddate.Length - 3, 3) + "Z";
            //                var dddate = Convert.ToDateTime(newstring);
            //                if (state == "Delivered")
            //                {
            //                    orderService.UpdateIsDelivered(item.Id, dddate);


            //                    orderService.UpdateOrderStatus(item.Id,6);
            //                }
            //                else if (state == "Cancelled")
            //                {
            //                    orderService.UpdateOrderStatus(item.Id, 4);
            //                }
            //                else if (state == "Return to Sender")
            //                {
            //                    orderService.UpdateOrderStatus(item.Id, 4);
            //                }
            //            }
            //            else
            //            {
            //                ModelState.AddModelError(string.Empty, "Server error try after some time.");
            //            }
            //        }
            //    }
            //}
            // Load all process orders
            var data = orderService.GetAllProcessedOrders();
            return PartialView("_Processed", data);

        }

        public ActionResult OrderDetailPage(int Id)
        {
            var data = orderService.OrderDetailPage(Id);
            return View(data);
        }

        public ActionResult ProcessedOrdersSearchWithStatus(int StatusId)
        {
            List<OrderViewModel> data = new List<OrderViewModel>();
            if (StatusId==1)
            {
                data = orderService.DeliveredOrders();
            }
            return PartialView("_Processed", data);
        }


        public ActionResult ResetProcessedOrdersSearch()
        {
            var data = orderService.GetAllProcessedOrders();
            return PartialView("_Processed", data);
        }

        public ActionResult ProcessedOrdersSearch(string Search)
        {

            var data = orderService.GetAllProcessedOrders(Search);
            return PartialView("_SearchProcessed", data);

        }

        public ActionResult GetInprocessOrder()
        {
            var data = orderService.GetInprocessOrder();
            return PartialView("_InProcess", data);

        }

        public ActionResult GetSelectedOrders(int id)
        {

            List<OrderViewModel> list = new List<OrderViewModel>();
            if (id == 4)
            {
                list = orderService.GetInprocessOrderforAllSelected();
            }
            else if (id == 1)
            {
                list = orderService.GetPendingInprocessOrder();
            }
            else
            {
                list = orderService.GetUnsuccessfullInprocessOrder();
            }

             return PartialView("_SearchInProcess", list);

        }

        public ActionResult InprocessOrderSearch(string Search)
        {
            var data = orderService.GetInprocessOrder(Search);
            return PartialView("_SearchInProcess", data);

        }

       
        public ActionResult LabelPrinting(int Id)
        {
            List<OrderLabelViewModel> label = new List<OrderLabelViewModel>();
            var jsonString = aPIResponseService.GetByOrderId(Id);
            dynamic data = JObject.Parse(jsonString.ResponseText);
            JArray array= data.labels;
            foreach (var item in array)
            {
                OrderLabelViewModel labels1 = new OrderLabelViewModel();
                labels1.Formate = item["format"].ToString();
                labels1.Size = item["size"].ToString();
                labels1.Url = item["url"].ToString();

                labels1.OrderId = jsonString.OrderId;

                label.Add(labels1);
            }
            return View(label.Where(x=>x.Size== "cropped"));
        }

        public JsonResult DownloadPdf(int[] Id)
        {
            try
            {
                List<string> imagesforpdf = new List<string>();
                foreach (var item in Id)
                {
                    var jsonString = aPIResponseService.GetByOrderId(item);
                    dynamic data = JObject.Parse(jsonString.ResponseText);
                    JArray array = data.labels;
                    string Url = array[1]["url"].ToString();

                    string filename = "labelpdf.pdf";
                    string[] exts = filename.Split('.');
                    string name = exts[0].ToString() + Utility.getDefaultTime().ToString("yyyyMMddHHmmss");//DateTime.Now.ToString("yyyyMMddHHmmss")
                    string ext = exts[1].ToString();
                    string FinalResult = name + "." + ext;
                    string path = Server.MapPath("~/Pdf's/" + FinalResult);


                    NetworkCredential myCreds = new NetworkCredential(Utility.UserName, Utility.Password);
                    using (var client = new WebClient())
                    {
                        client.Credentials = myCreds;
                        client.DownloadFile(Url, path);
                    }


                    // Images Making

                    Spire.Pdf.PdfDocument doc = new Spire.Pdf.PdfDocument();
                    doc.LoadFromFile(path);
                    Image bmp = doc.SaveAsImage(0);
                    Image emf = doc.SaveAsImage(0, Spire.Pdf.Graphics.PdfImageType.Metafile);
                    Image zoomImg = new Bitmap((int)(emf.Size.Width * 2), (int)(emf.Size.Height * 2));
                    using (Graphics g = Graphics.FromImage(zoomImg))
                    {
                        g.ScaleTransform(2.0f, 2.0f);
                        g.DrawImage(emf, new Rectangle(new Point(0, 0), emf.Size), new Rectangle(new Point(0, 0), emf.Size), GraphicsUnit.Pixel);
                    }
                    emf.Save(path, ImageFormat.Png);
                    imagesforpdf.Add(path);
                }
                using (var doc = new iTextSharp.text.Document())
                {
                    iTextSharp.text.pdf.PdfWriter.GetInstance(doc, new FileStream(Server.MapPath("~/Pdf's/labelpdf.pdf"), FileMode.Create));
                    doc.Open();
                    foreach (var item in imagesforpdf)
                    {
                        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(item);
                        image.ScaleAbsolute(380f, 380f);
                        doc.Add(image);
                    }
                }
                foreach (var item in imagesforpdf)
                {
                    if ((System.IO.File.Exists(item)))
                    {
                        System.IO.File.Delete(item);
                    }
                }
                return Json("labelpdf.pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpGet]
        public ActionResult Download(string file)
        {

            string fullPath = Path.Combine(Server.MapPath("~/Pdf's/"), file);
            if (System.IO.File.Exists(fullPath))
            {
                return File(fullPath, "application/pdf", file);
            }
            return null;
        }

        public JsonResult OnePerPdfDownload(int[] Id)
        {
            try
            {
                List<string> pdfs = new List<string>();
                foreach (var item in Id)
                {
                    var jsonString = aPIResponseService.GetByOrderId(item);
                    dynamic data = JObject.Parse(jsonString.ResponseText);
                    JArray array = data.labels;
                    string Url = array[1]["url"].ToString();

                    string filename = "labelpdf.pdf";
                    string[] exts = filename.Split('.');
                    string name = exts[0].ToString() + Utility.getDefaultTime().ToString("yyyyMMddHHmmss");//DateTime.Now.ToString("yyyyMMddHHmmss")
                    string ext = exts[1].ToString();
                    string FinalResult = name + "." + ext;
                    string path = Server.MapPath("~/Pdf's/" + FinalResult);


                    pdfs.Add(path);


                    NetworkCredential myCreds = new NetworkCredential(Utility.UserName, Utility.Password);
                    using (var client = new WebClient())
                    {
                        client.Credentials = myCreds;
                        client.DownloadFile(Url, path);
                    }


                    // Pdf Making


                }

                using (PdfSharp.Pdf.PdfDocument targetDoc = new PdfSharp.Pdf.PdfDocument())
                {
                    foreach (string pdf in pdfs)
                    {
                        using (PdfSharp.Pdf.PdfDocument pdfDoc = PdfReader.Open(pdf, PdfDocumentOpenMode.Import))
                        {
                            for (int i = 0; i < pdfDoc.PageCount; i++)
                            {
                                targetDoc.AddPage(pdfDoc.Pages[i]);
                            }
                        }
                    }
                    targetDoc.Save(Server.MapPath("~/Pdf's/labelpdf.pdf"));
                }
                foreach (var item in pdfs)
                {
                    if ((System.IO.File.Exists(item)))
                    {
                        System.IO.File.Delete(item);
                    }
                }
                return Json("labelpdf.pdf");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<ActionResult> DownloadPdfPortrait(int Id)
        {

            var jsonString = aPIResponseService.GetByOrderId(Id);
            dynamic data = JObject.Parse(jsonString.ResponseText);
            JArray array = data.labels;
            if (array != null)
            {
                string Url = array[0]["url"].ToString();
                using (var client = Utility.GetHttpClient())
                {
                    var responseTask = client.GetAsync(Url);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var contentStream = await result.Content.ReadAsStreamAsync();
                        return File(contentStream, "Label.pdf");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                }
            }
            else
            {
                string order_id = data.order_id;
                using (var client = Utility.GetHttpClient())
                {
                    var responseTask = client.GetAsync("https://api.sendle.com/api/orders/" + order_id + "");
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var response = await result.Content.ReadAsStringAsync();
                        dynamic data1 = JObject.Parse(response);
                        JArray array1 = data1.labels;
                        string Url = array1[0]["url"].ToString();

                        var responseTask1 = client.GetAsync(Url);
                        responseTask1.Wait();
                        var result1 = responseTask1.Result;
                        if (result1.IsSuccessStatusCode)
                        {
                            var contentStream1 = await result1.Content.ReadAsStreamAsync();
                            return File(contentStream1, "Label.pdf");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Server error try after some time.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                }
            }
            return View();
        }


        public async Task<ActionResult> DownloadPdfLandScape(int Id)
        {

            var jsonString = aPIResponseService.GetByOrderId(Id);
            if (jsonString.ResponseText!=null)
            {
                dynamic data = JObject.Parse(jsonString.ResponseText);
                JArray array = data.labels;
                string Url = array[1]["url"].ToString();
                using (var client = Utility.GetHttpClient())
                {
                    var responseTask = client.GetAsync(Url);
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        orderService.UpdateIsPrinted(Id);
                        var contentStream = await result.Content.ReadAsStreamAsync();
                        return File(contentStream, "Label.pdf");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                }
            }
            else
            {
                string order_id = jsonString.SendleOrderId;
                using (var client = Utility.GetHttpClient())
                {
                    var responseTask = client.GetAsync("https://api.sendle.com/api/orders/" + order_id + "");
                    responseTask.Wait();
                    var result = responseTask.Result;
                    if (result.IsSuccessStatusCode)
                    {
                        var response = await result.Content.ReadAsStringAsync();
                        dynamic data1 = JObject.Parse(response);
                        JArray array1 = data1.labels;
                        if (array1==null)
                        {
                            return Json("Sorry ! Don't Print Label due to missing response text.",JsonRequestBehavior.AllowGet);
                        }
                        string Url = array1[1]["url"].ToString();

                        var responseTask1 = client.GetAsync(Url);
                        responseTask1.Wait();
                        var result1 = responseTask1.Result;
                        if (result1.IsSuccessStatusCode)
                        {
                            orderService.UpdateIsPrinted(Id);
                            var contentStream1 = await result1.Content.ReadAsStreamAsync();
                            return File(contentStream1, "Label.pdf");
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, "Server error try after some time.");
                        }
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                }
            }
            return View();
        }

        public ActionResult CancelOrder(int Id, string CancelRemarks)
        {
            var jsonString = aPIResponseService.GetByOrderId(Id);
            dynamic data = jsonString.SendleOrderId;
            string order_id = data;
            using (var client = Utility.GetHttpClient())
            {
                var responseTask = client.DeleteAsync("https://api.sendle.com/api/orders/" + order_id + "");
                responseTask.Wait();
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    orderService.UpdateOrderStatus(Id,4);
                   _orderLogService.UpdateCancelRemarks(Id, CancelRemarks);

                    return Json(true,JsonRequestBehavior.AllowGet);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");

                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public async Task<ActionResult> TrackOrder(int Id)
        {
            var jsonString = aPIResponseService.GetByOrderId(Id);
            string sendle_reference = jsonString.SendleRefrence;
            var data = await new SendleAPI().TrackOrder(sendle_reference);


            var state = data.State;
            string ddate = data.Status.last_changed_at;
            var newstring = ddate.Remove(ddate.Length - 3, 3) + "Z";
            var dddate = Convert.ToDateTime(newstring);
            if (state == "Delivered")
            {
                orderService.UpdateIsDelivered(Id, dddate);

                orderService.UpdateOrderStatus(Id, 6);
            }
            return View(data);
        }


        #region DownloadPdf
        //public ActionResult DownloadPdf(string Url)
        //{
        //    //create new excel file with other name 
        //    string filename = "labelpdf.pdf";
        //    string[] exts = filename.Split('.');
        //    string name = exts[0].ToString() + Utility.getDefaultTime().ToString("yyyyMMddHHmmss");//DateTime.Now.ToString("yyyyMMddHHmmss")
        //    string ext = exts[1].ToString();
        //    string FinalResult = name + "." + ext;

        //    //save file at path
        //    string path = Server.MapPath("~/Pdf's/" + FinalResult);

        //    NetworkCredential myCreds = new NetworkCredential(Utility.UserName, Utility.Password);
        //    using (var client = new WebClient())
        //    {
        //        client.Credentials = myCreds;
        //        client.DownloadFile(Url, path);
        //    }

        //    byte[] fileBytes = GetFile(path);
        //    return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, FinalResult);
        //}
        //byte[] GetFile(string s)
        //{
        //    FileStream fs = System.IO.File.OpenRead(s);
        //    byte[] data = new byte[fs.Length];
        //    int br = fs.Read(data, 0, data.Length);
        //    if (br != fs.Length)
        //        throw new System.IO.IOException(s);
        //    return data;
        //}
        #endregion



        [HttpGet]
        public ActionResult UpdateProcessedOrders(int id)
        {
            var order = orderService.GetById(id);
            return View(order);

        }

        [HttpPost]
        public ActionResult UpdateProcessedOrders(OrderViewModel model)
        {
            model.ModifiedBy =Convert.ToInt32(Session["UserId"]) ;
            model.Receiver.ModifiedBy = Convert.ToInt32(Session["UserId"]);
            orderService.Update(model);
            return RedirectToAction("ReceiverList");

        }


        public ActionResult GetAllUniquePracticeId()
        {
            var data = orderService.GetAllUniquePracticeId();
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult FilterProcessedOrder(OrderViewModel model)
        {
            var data = orderService.FilterProcessedOrders(model);
            return PartialView("_Processed", data);

        }


        public ActionResult GetAllUniqueInProcessPracticeId()
        {
            var data = orderService.GetAllUniqueInProcessPracticeId();
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult FilterInProcessOrder(OrderViewModel model)
        {
            var data = orderService.FilterInProcessOrders(model);
            return PartialView("_InProcess", data);

        }

        public ActionResult GetAllUniqueCanceledPracticeId()
        {
            var data = orderService.GetAllUniqueCanceledPracticeId();
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult FilterCanceledOrders(OrderViewModel model)
        {
            var data = orderService.FilterCanceledOrders(model);
            return PartialView("_CancelledOrders", data);

        }

        public ActionResult GetAllUniqueDeliveredPracticeId()
        {
            var data = orderService.GetAllUniqueDeliveredPracticeId();
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult FilterDeliveredOrders(OrderViewModel model)
        {
            var data = orderService.FilterDeliveredOrders(model);
            return PartialView("_DeliveredOrders", data);

        }

        [HttpGet]
        public ActionResult PreviewOrder(int id)
        {
            var data = orderService.GetById(id);
            return Json(data, JsonRequestBehavior.AllowGet);

        }

        [HttpGet]
        public async Task<ActionResult> ReturnOrder(int id, DateTime PickUpDate,string ReturnRemarks,bool IsduplicateReturn)
        {
            var item = orderService.GetById(id);
            string date = PickUpDate.ToString("yyyy-MM-dd");
            var order = new
            {

                pickup_date = date,
                first_mile_option = "pickup",
                description = item.Description,
                weight = new
                {
                    value = "2",
                    units = "lb"
                },
                //customer_reference = item.Customer_Reference,
                sender = new
                {
                    instructions = "Please Call Support Number - R",
                    contact = new
                    {
                        name = item.Receiver.Name,
                        phone = item.Receiver.Phone,
                        company = "-"
                    },
                    address = new
                    {
                        address_line1 = item.Receiver.Address_Line1,
                        suburb = item.Receiver.Suburb,
                        postcode = item.Receiver.Postcode,
                        state_name = item.Receiver.State_Name,
                        country = item.Receiver.Country
                    }
                },
                receiver = new
                {
                    instructions = "Please Call Support Number - R",
                    contact = new
                    {
                        name = "Rizwan Khan",
                        phone =item.Sender.Phone,
                        email = item.Sender.Email,
                        company = item.Sender.Company
                    },
                    address = new
                    {
                        address_line1 = item.Sender.Address_Line1,
                        suburb = item.Sender.Suburb,
                        postcode = item.Sender.Postcode,
                        state_name = item.Sender.State_Name,
                        country = item.Sender.Country
                    }
                }
            };
            using (var client = Utility.GetHttpClient())
            {
                var myContent = JsonConvert.SerializeObject(order);
                var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/Json");
                var responseTask = client.PostAsync("https://api.sendle.com/api/orders", byteContent);
                responseTask.Wait();
                var result = responseTask.Result;
                var responsetext = await result.Content.ReadAsStringAsync();
                if (result.IsSuccessStatusCode)
                {
                    dynamic data = JObject.Parse(responsetext);
                    JArray array = data.labels;


                    var newModel = new OrderViewModel()
                    {
                        ReceiverId = item.ReceiverId,
                        SourceId = item.SourceId,
                        DeviceTypeId =IsduplicateReturn==true ? 12 : 6,
                        PracticeId = item.PracticeId,
                        CreatedBy = Convert.ToInt32(Session["UserId"]),
                        CreatedOn = DateTime.Now,
                        //IsReturned =true,
                        IsDeleted = false,
                        IsActive = true,
                        //EHR=item.EHR,

                        
                        SenderId=null,
                        NewPracticeId = item.NewPracticeId,

                        PatientId=item.ReceiverId.Value
                    };

                    var returnOrder = orderService.AddSingleOrder(newModel);

                    var apiResp = new APIResponseViewModel
                    {
                        OrderId = returnOrder.Id,
                        ResponseText = responsetext,
                        SendleOrderId = data.order_id,
                        SendleOrderUrl = data.order_url,
                        SendleRefrence = data.sendle_reference,
                        SendleTrackingUrl = data.tracking_url,
                        CreatedBy = Convert.ToInt32(Session["UserId"]),
                        CreatedOn = DateTime.Now,
                        IsDeleted = false,
                        IsActive = true
                    };
                    aPIResponseService.Add(apiResp);
                    var orderLabel = new OrderLabelViewModel
                    {
                        OrderId = returnOrder.Id,
                        Formate = array[0]["format"].ToString(),
                        Size = array[0]["size"].ToString(),
                        Url = array[0]["url"].ToString()
                    };

                    orderLabelService.Add(orderLabel);
                    orderService.UpdateOrderStatus(returnOrder.Id, 2);

                    // Adding Remarks
                    _orderLogService.UpdateCancelRemarks(returnOrder.Id,ReturnRemarks);

                    //Order Log
                    var orderLog = new OrderLogViewModel
                    {
                        DeviceTypeId = returnOrder.DeviceTypeId,
                        OrderId = returnOrder.Id,
                        ReceiverId = returnOrder.ReceiverId,
                        ReferenceOrderId = id,
                        SendleRefrence= data.sendle_reference
                    };
                    _orderLogService.Add(orderLog);
                }
            }
            return RedirectToAction("ReceiverList");
        }


        [HttpGet]
        public ActionResult DeleteOrder(int id)
        {
            var data = orderService.DeleteOrder(id);
            return Json(true, JsonRequestBehavior.AllowGet);

        }

        #region Searching

        [HttpPost]
        public ActionResult FilterAccessoriesOrder(OrderViewModel model)
        {
            var data = orderService.FilterAccessoriesOrder(model);
            return PartialView("_Accessories", data);

        }
        public ActionResult ResetAllAccessories()
        {
            var data = orderService.GetAllAccessories();
            return PartialView("_Accessories", data);
        }
        [HttpPost]
        public ActionResult FilterReturnOrder(OrderViewModel model)
        {
            var data = orderService.FilterReturnOrder(model);
            return PartialView("_Returns", data);

        }
        public ActionResult ResetReturnOrdersSearch()
        {
            var data = orderService.GetAllReturns();
            return PartialView("_Returns", data);
        }
        [HttpPost]
        public ActionResult FilterReplacementOrder(OrderViewModel model)
        {
            var data = orderService.FilterReplacementOrder(model);
            return PartialView("_Replacement", data);

        }
        public ActionResult ResetReplacementOrdersSearch()
        {
            var data = orderService.GetReplacementOrders();
            return PartialView("_Replacement", data);
        }
        [HttpPost]
        public ActionResult FilterCanceledOrder(OrderViewModel model)
        {
            var data = orderService.FilterCanceledOrder(model);
            return PartialView("_Cancelled", data);

        }
        public ActionResult ResetCanceledOrdersSearch()
        {
            var data = orderService.GetAllCanceledOrders();
            return PartialView("_Cancelled", data);
        }

        #endregion




    }
}