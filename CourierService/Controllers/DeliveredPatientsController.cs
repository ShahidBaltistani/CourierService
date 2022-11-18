using CourierService.Services.Orders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace CourierService.Controllers
{
    [Authorize]
    public class DeliveredPatientsController : Controller
    {
        class DeliveredPatientsModel
        {
            public string Name { get; set; }
            public string Address { get; set; }
            public string Practice { get; set; }
            public string Customer_Reference { get; set; }
            public DateTime Delivery_Status { get; set; }
            public string Phone { get; set; }
            public string EHR { get; set; }
            public DateTime Order_Status { get; set; }
        }

        // GET: DeliveredPatients

        // Adding Service
        //OrderService orderService = new OrderService();
        //public void DeliveredPatientsMailing()
        //{
        //    var data = orderService.DeliveredPatients();
        //    string JSONresult = JsonConvert.SerializeObject(data);
        //    var model = JsonConvert.DeserializeObject<List<DeliveredPatientsModel>>(JSONresult);

        //    var products = new DataTable("teste");


        //    products.Columns.Add("Name", typeof(string));

        //    products.Columns.Add("Address", typeof(string));

        //    products.Columns.Add("Practice", typeof(string));

        //    products.Columns.Add("Customer_Reference", typeof(string));

        //    products.Columns.Add("Delivery_Status", typeof(string));

        //    products.Columns.Add("Phone", typeof(string));

        //    products.Columns.Add("EHR", typeof(string));

        //    products.Columns.Add("Order_Status", typeof(string));

        //    foreach (var item in model)
        //    {
        //        products.Rows.Add(item.Name, item.Address, item.Practice, item.Customer_Reference, item.Delivery_Status.ToShortDateString(), item.Phone, item.EHR, item.Order_Status.ToShortDateString());
        //    }
        //    var gv = new GridView();
        //    gv.DataSource = products;
        //    gv.DataBind();
        //    StringWriter objStringWriter = new StringWriter();
        //    HtmlTextWriter objHtmlTextWriter = new HtmlTextWriter(objStringWriter);
        //    gv.RenderControl(objHtmlTextWriter);



        //    string filename = "DeliveredPatients.xls";
        //    string[] exts = filename.Split('.');
        //    string name = exts[0].ToString() + Utility.getDefaultTime().ToString("yyyyMMddHHmmss");//DateTime.Now.ToString("yyyyMMddHHmmss")
        //    string ext = exts[1].ToString();
        //    string FinalResult = name + "." + ext;
        //    string path = HostingEnvironment.MapPath("~/ExcelTemplate/" + FinalResult);


        //    System.IO.File.WriteAllText(path, objStringWriter.ToString());

        //    // Sending Email
        //    string ToEmail = "shahidhussain1397@gmail.com";
        //    string cc = "kashifshahzadid@gmail.com";


        //    var message = new MailMessage();
        //    message.From = new MailAddress("streamvitalsllc@gmail.com");
        //    message.Subject = "Delivered Patients";
        //    message.Body = "Hi,<br>The latest delivered patients list is attached below.<br>Regards,<br>Shahid Hussain";
        //    string[] ToMuliId = ToEmail.Split(',');
        //    foreach (string ToEMailId in ToMuliId)
        //    {
        //        message.To.Add(new MailAddress(ToEMailId)); //adding multiple TO Email Id
        //    }
        //    string[] CCId = cc.Split(',');
        //    foreach (string CCEmail in CCId)
        //    {
        //        message.CC.Add(new MailAddress(CCEmail)); //Adding Multiple CC email Id
        //    }


        //    message.IsBodyHtml = true;
        //    message.Attachments.Add(new Attachment(path));

        //    using (var smtp = new System.Net.Mail.SmtpClient())
        //    {
        //        smtp.UseDefaultCredentials = true;
        //        var credential = new NetworkCredential
        //        {
        //            UserName = "streamvitalsllc@gmail.com",
        //            Password = "VuSolution360.info@StreamVitals"
        //        };
        //        smtp.Credentials = credential;
        //        smtp.Host = "smtp.gmail.com";
        //        smtp.Port = 587;
        //        smtp.EnableSsl = true;
        //        smtp.Send(message);
        //    }
        //}
    }
}