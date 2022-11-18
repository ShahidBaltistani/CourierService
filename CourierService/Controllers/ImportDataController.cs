using CourierService.Entities.Orders;
using CourierService.Services;
using CourierService.Services.NewPracticeF;
using CourierService.Services.Orders;
using CourierService.ViewModels.Orders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Excel = Microsoft.Office.Interop.Excel;

namespace CourierService.Controllers
{
    [Authorize]
    public class ImportDataController : Controller
    {
        OrderService Service = new OrderService();
        NewPracticeService practiceService = new NewPracticeService();
        public ActionResult DownloadFileFromFolder()
        {
            string fileName = "Import_Patients.xlsx";
            string fullName = Server.MapPath("~/ExcelTemplate/" + fileName);
            byte[] fileBytes = GetFile(fullName);
            return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
        }
        byte[] GetFile(string s)
        {
            FileStream fs = System.IO.File.OpenRead(s);
            byte[] data = new byte[fs.Length];
            int br = fs.Read(data, 0, data.Length);
            if (br != fs.Length)
                throw new System.IO.IOException(s);
            return data;
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
                var data = practiceService.GetIdByPracticeName(item.PracticeId);
                if (data!=null)
                {
                    model.NewPracticeId = data.Id;
                }
                else
                {
                    return Json(new { result=false,text= "Please Use Correct Practice Name..." });
                }
                


                model.Receiver.EHR = item.Receiver.EHR;


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
        [HttpPost]
        public ActionResult IsExist(string name, string address)
        {
            var data = Service.CheckExistForRechecking(name, address);
            return Json(data, JsonRequestBehavior.AllowGet);
        }
    }
}