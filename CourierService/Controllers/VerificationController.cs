using ClosedXML.Excel;
using CourierService.Services.Orders;
using CourierService.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CourierService.Controllers
{
    [Authorize]
    public class VerificationController : Controller
    {
        OrderService Service = new OrderService();
        public ActionResult UploadExcel()
        {
            return View();
        }
        [HttpPost]
        public ActionResult UploadExcel(HttpPostedFileBase file)
        {
            string path = Path.Combine(Server.MapPath("~/ExcelFiles"), Path.GetFileName(file.FileName));
            file.SaveAs(path);
            using (var workBook = new XLWorkbook(path))
            {
                var workSheet = workBook.Worksheet(1);
                var firstRowUsed = workSheet.FirstRowUsed();
                var firstPossibleAddress = workSheet.Row(firstRowUsed.RowNumber()).FirstCell().Address;
                var lastPossibleAddress = workSheet.LastCellUsed().Address;
                var range = workSheet.Range(firstPossibleAddress, lastPossibleAddress).AsRange();
                var table = range.AsTable();
                var dataList = new List<string[]>
                {
                table.DataRange.Rows()
                .Select(tableRow =>
                tableRow.Field("First")
                .GetString())
                .ToArray(),
                table.DataRange.Rows()
                .Select(tableRow => tableRow.Field("Last").GetString())
                .ToArray(),
                table.DataRange.Rows()
                .Select(tableRow => tableRow.Field("Landline#").GetString())
                .ToArray(),
                table.DataRange.Rows()
                .Select(tableRow => tableRow.Field("Mobile#").GetString())
                .ToArray(),
                table.DataRange.Rows()
                .Select(tableRow => tableRow.Field("Address").GetString())
                .ToArray(),
                table.DataRange.Rows()
                .Select(tableRow => tableRow.Field("EHR#").GetString())
                .ToArray(),
                };
                var orders = new List<OrderViewModel>();
                var rows = dataList.Select(array => array.Length).Concat(new[] { 0 }).Max();

                for (var j = 0; j < rows; j++)
                {
                    OrderViewModel order = new OrderViewModel();
                    order.Receiver = new ReceiverViewModel();

                    order.Receiver.Name = dataList[0][j] +" "+dataList[1][j];
                    order.Receiver.Phone= dataList[2][j];
                    if (order.Receiver.Phone == "null")
                    {
                        order.Receiver.Phone = dataList[3][j];
                    }
                    order.Receiver.Address_Line2 = dataList[4][j];
                    order.Receiver.EHR = dataList[5][j];
                    orders.Add(order);
                }
                List<OrderViewModel> neworders = new List<OrderViewModel>();
                foreach (var item in orders)
                {
                    var data = Service.GetDuplicateDate(item.Receiver.Name, item.Receiver.Address_Line2);

                    if (data == false)
                    {
                        neworders.Add(item);

                    }
                    Service.UpdateEHR(item.Receiver.Name, item.Receiver.Address_Line2, item.Receiver.EHR);
                }
                var products = new DataTable("teste");

                products.Columns.Add("instructions", typeof(string));

                products.Columns.Add("Name", typeof(string));

                products.Columns.Add("email", typeof(string));

                products.Columns.Add("Phone", typeof(string));

                products.Columns.Add("address_line1", typeof(string));

                products.Columns.Add("address_line2", typeof(string));

                products.Columns.Add("suburb", typeof(string));

                products.Columns.Add("postcode", typeof(string));

                products.Columns.Add("state", typeof(string));

                products.Columns.Add("country", typeof(string));

                products.Columns.Add("customer_reference", typeof(string));

                products.Columns.Add("PracticeId", typeof(string));

                products.Columns.Add("EHR#", typeof(string));

                foreach (var item in neworders)
                {
                    products.Rows.Add("Please Call Support Number", item.Receiver.Name,"",item.Receiver.Phone,"", item.Receiver.Address_Line2,"",item.Receiver.Postcode,item.Receiver.State_Name,"","","",item.Receiver.EHR);
                }
                var gv = new GridView();
                gv.DataSource = products;
                gv.DataBind();

                Response.ClearContent();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment; filename=Refined_Patients.xls");
                Response.ContentType = "application/ms-excel";

                Response.Charset = "";
                StringWriter objStringWriter = new StringWriter();
                HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);

                gv.RenderControl(objHtmlTextWriter);

                Response.Output.Write(objStringWriter.ToString());
                Response.Flush();
                Response.End();


                ViewBag.orders = orders;
            }
            return View();
        }
    }
}