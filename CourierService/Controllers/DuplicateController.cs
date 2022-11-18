using CourierService.Services.Orders;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourierService.Controllers
{
    [Authorize]
    public class DuplicateController : Controller
    {
      public class DuplicatePatientVM
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public int Returned { get; set; }
            public int Counter { get; set; }
            public string Address { get; set; }
            public DateTime? DeliveredDate { get; set; }
        }
        // GET: Duplicate
        OrderService _orderService = new OrderService();
        public ActionResult DuplicatePatientList()
        {
            var data = _orderService.GetDuplicatePatients();
            string JSONresult = JsonConvert.SerializeObject(data);
            var model = JsonConvert.DeserializeObject<List<DuplicatePatientVM>>(JSONresult);
            ViewBag.DuplicatePatientCount = model.Count;
            return View(model);
        }

        [HttpPost]
        public ActionResult DuplicatePatientDetail(string name)
        {
            var data = _orderService.DuplicatePatientDetail(name);
            string JSONresult = JsonConvert.SerializeObject(data);
            var model = JsonConvert.DeserializeObject<List<DuplicatePatientVM>>(JSONresult);
            var t = model.Select(
                (OL) => new
                {
                    Name = OL.Name,
                    Address = OL.Address,
                    DeliveredDate = OL.DeliveredDate == null ? "" : OL.DeliveredDate.Value.ToString("dd-MMM-yyyy"),
                    Returned = OL.Returned == 6 ? true : false
                }
                );
            return Json(t, JsonRequestBehavior.AllowGet);
        }
    }
}