using ClosedXML.Excel;
using CourierService.Services.Orders;
using CourierService.ViewModels.Orders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourierService.Controllers
{
    [Authorize]
    public class ExcelClosedXMLController : Controller
    {
        OrderService Service = new OrderService();
        // GET: ExcelClosedXML
        public ActionResult ExcelFileData()
        {
            return View();
        }
        public ActionResult UploadExcel()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadExcel(HttpPostedFileBase file)
        {
            DataTable dt = new DataTable();
            if (file != null && file.ContentLength > 0 && System.IO.Path.GetExtension(file.FileName).ToLower() == ".xlsx")
            {
                string path = Path.Combine(Server.MapPath("~/ExcelFiles"), Path.GetFileName(file.FileName));
                file.SaveAs(path);
                using (XLWorkbook workbook = new XLWorkbook(path))
                {
                    IXLWorksheet worksheet = workbook.Worksheet(1);
                    bool FirstRow = true;
                    string readRange = "1:1";
                    foreach (IXLRow row in worksheet.RowsUsed())
                    {
                        if (FirstRow)
                        {
                            readRange = string.Format("{0}:{1}", 1, row.LastCellUsed().Address.ColumnNumber);
                            foreach (IXLCell cell in row.Cells(readRange))
                            {
                                dt.Columns.Add(cell.Value.ToString());
                            }
                            FirstRow = false;
                        }
                        else
                        {
                            dt.Rows.Add();
                            int cellIndex = 0;
                            foreach (IXLCell cell in row.Cells(readRange))
                            {
                                dt.Rows[dt.Rows.Count - 1][cellIndex] = cell.Value.ToString();
                                cellIndex++;
                            }
                        }
                    }
                    if (FirstRow)
                    {
                        ViewBag.Message = "Empty Excel File!";
                    }
                }
            }
            else
            {
                ViewBag.Message = "Please select file with .xlsx extension!";
            }
            List<OrderViewModel> orders = new List<OrderViewModel>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                OrderViewModel model = new OrderViewModel();
                model.Receiver = new ReceiverViewModel();


                model.Receiver.Instructions = dt.Rows[i]["instructions"].ToString();
                model.Receiver.Name= dt.Rows[i]["name"].ToString();
                model.Receiver.Email = dt.Rows[i]["email"].ToString();
                model.Receiver.Phone = dt.Rows[i]["Phone"].ToString();
                model.Receiver.Address_Line1 = dt.Rows[i]["address_line1"].ToString();
                model.Receiver.Address_Line2 = dt.Rows[i]["Address_Line2"].ToString();
                model.Receiver.Suburb = dt.Rows[i]["suburb"].ToString();
                model.Receiver.Postcode = dt.Rows[i]["postcode"].ToString();
                model.Receiver.State_Name = dt.Rows[i]["state"].ToString();
                model.Receiver.Country = dt.Rows[i]["country"].ToString();
                model.Customer_Reference = dt.Rows[i]["customer_reference"].ToString();
                model.PracticeId = dt.Rows[i]["PracticeId"].ToString();

                model.Receiver.EHR= dt.Rows[i]["EHR#"].ToString();

                orders.Add(model);
            }
            #pragma warning disable CS0219
            bool message;
            #pragma warning restore CS0219
            foreach (var item in orders)
            {

                if (item.Receiver.EHR=="")
                {

                    var data = Service.GetDuplicateDate(item.Receiver.Name,item.Receiver.Address_Line2);
                    item.IsExist = data;
                    if (data == true)
                    {
                        message = true;
                    }
                }
                else
                {
                    var data = Service.CheckExist(item.Receiver.EHR);
                    item.IsExist = data;
                    if (data == true)
                    {
                        message = true;
                    }
                }
            }
            ViewBag.orders = orders;
            return View("ExcelFileData");
        }
        
        public ActionResult SourceDropdown()
        {
            var data = Service.SourceDropdown();
            return Json(data, JsonRequestBehavior.AllowGet);

        }
        public ActionResult DeviceTypeDropdown()
        {
            var data = Service.DeviceTypesDropdown();
            var dd = data.Where(x => x.Id != 5 && x.Id != 6 && x.Id != 7 && x.Id != 8).ToList();
            return Json(dd, JsonRequestBehavior.AllowGet);
        }
    }
}