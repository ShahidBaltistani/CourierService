using CourierService.Services;
using CourierService.Services.Orders;
using CourierService.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CourierService.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // Adding Service
        OrderService orderService = new OrderService();
        public ActionResult Dashboard()
        {

            var data = orderService.Dashboard();
            return View(data);
        }
        [HttpPost]
        public ActionResult PracticeSelectionOnSelection()
        {
            var list = orderService.GetAllUniquePracticeId();
            List<OrderViewModel> refineorders = new List<OrderViewModel>();
            BadgeData badgeData = new BadgeData();
            if (list != null)
            {
                foreach (var item in list)
                {
                    List<OrderViewModel> orders = orderService.DashboardDataByPractice(item).ToList();
                    refineorders.AddRange(orders);

                }
                var today = DateTime.Today;
                var yesterday = DateTime.Today.AddDays(-1);

                badgeData = new BadgeData
                {
                    DeliveredPatients = refineorders.Where(x => x.DeliveredDate.Date == yesterday.Date).ToList().Count(),
                    newDeviceOrders = refineorders.Where(x => x.CreatedOn.Value.Date == today.Date && x.DeviceTypeId == 1 && (x.OrderStatusId == 2)).ToList().Count(),
                    asessoriesOrders = refineorders.Where(x => x.CreatedOn.Value.Date == today.Date && x.DeviceType.MainDeviceTypeId == 2 && (x.OrderStatusId == 2)).ToList().Count(),
                    returnOrders = refineorders.Where(x => x.CreatedOn.Value.Date == today.Date && x.DeviceTypeId == 6 && (x.OrderStatusId == 2)).ToList().Count(),
                    replacementOrders = refineorders.Where(x => x.CreatedOn.Value.Date == today.Date && x.DeviceTypeId == 8 && x.SenderId != null && (x.OrderStatusId == 2)).ToList().Count(),
                    cancelOrders = refineorders.Where(x => x.CreatedOn.Value.Date == today.Date && x.OrderStatusId == 4).ToList().Count()
                };
            }
            return Json(badgeData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult PracticeSelection(DateTime fromdate, DateTime todate, string[] list)
        {
            List<OrderViewModel> refineorders = new List<OrderViewModel>();
            BadgeData badgeData = new BadgeData();
            if (list != null)
            {
                foreach (var item in list)
                {
                    List<OrderViewModel> orders = orderService.DashboardDataByPractice(item).ToList();
                    refineorders.AddRange(orders);

                }
                badgeData = new BadgeData
                {
                    DeliveredPatients = refineorders.Where(x => x.DeliveredDate.Date >= fromdate.Date && x.DeliveredDate.Date <= todate.Date).ToList().Count(),
                    newDeviceOrders = refineorders.Where(x => x.CreatedOn.Value.Date >= fromdate.Date && x.CreatedOn.Value.Date <= todate.Date && (x.DeviceTypeId == 1 || x.OrderStatusId == 2)).ToList().Count(),
                    asessoriesOrders = refineorders.Where(x => x.CreatedOn.Value.Date >= fromdate.Date && x.CreatedOn.Value.Date <= todate.Date && (x.DeviceType.MainDeviceTypeId == 2 && x.OrderStatusId == 2)).ToList().Count(),
                    returnOrders = refineorders.Where(x => x.CreatedOn.Value.Date >= fromdate.Date && x.CreatedOn.Value.Date <= todate.Date && (x.DeviceTypeId == 6 || x.OrderStatusId == 2)).ToList().Count(),
                    replacementOrders = refineorders.Where(x => x.CreatedOn.Value.Date >= fromdate.Date && x.CreatedOn.Value.Date <= todate.Date && x.SenderId != null && (x.DeviceTypeId == 8 || x.OrderStatusId == 2)).ToList().Count(),
                    cancelOrders = refineorders.Where(x => x.CreatedOn.Value.Date >= fromdate.Date && x.CreatedOn.Value.Date <= todate.Date && (x.OrderStatusId == 4)).ToList().Count()
                };
            }
            return Json(badgeData, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult DashboardDetails(string Id, DateTime fromdate, DateTime todate, string[] list)
        {



            List<OrderViewModel> result = new List<OrderViewModel>();
            switch (Id)
            {
                case "delivered":
                    ViewBag.PartialHeading = "Delivered Patients";
                    result = DeliveredDetails(fromdate, todate, list);
                    break;
                case "rpdDevice":
                    ViewBag.PartialHeading = "New Device Orders";
                    result = RPDDeviceDetails(fromdate, todate, list);
                    break;
                case "asessoriesDetails":
                    ViewBag.PartialHeading = "Asessories Orders";
                    result = AsessoriesDetails(fromdate, todate, list);
                    break;
                case "returnDetails":
                    ViewBag.PartialHeading = "Return Orders";
                    result = ReturnDetails(fromdate, todate, list);
                    break;
                case "replacementDetails":
                    ViewBag.PartialHeading = "Replacement Orders";
                    result = ReplacementDetails(fromdate, todate, list);
                    break;
                case "canceledDetails":
                    ViewBag.PartialHeading = "Cancel Orders";
                    result = CanceledDetails(fromdate, todate, list);
                    break;
                case "pendingSerial":
                    ViewBag.PartialHeading = "Pending Serial Assigning";
                    result = PendingSerialDetails();
                    break;
                case "pendingTasks":
                    ViewBag.PartialHeading = "Pending Task";
                    result = PendingTaskDetails();
                    break;
                case "inTransits":
                    ViewBag.PartialHeading = "In-Transit";
                    result = InTransitDetails();
                    break;
            }
            return PartialView("_OverallDashboardOrders", result);
        }
        private List<OrderViewModel> DeliveredDetails(DateTime fromdate, DateTime todate, string[] list)
        {
            List<OrderViewModel> refineorders = new List<OrderViewModel>();
            List<OrderViewModel> data = new List<OrderViewModel>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    List<OrderViewModel> orders = orderService.DashboardDataByPractice(item).ToList();
                    refineorders.AddRange(orders);

                }
                data = refineorders.Where(x => x.DeliveredDate.Date >= fromdate.Date && x.DeliveredDate.Date <= todate.Date).ToList();
            }
            return data;
        }

        private List<OrderViewModel> RPDDeviceDetails(DateTime fromdate, DateTime todate, string[] list)
        {
            List<OrderViewModel> refineorders = new List<OrderViewModel>();
            List<OrderViewModel> data = new List<OrderViewModel>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    List<OrderViewModel> orders = orderService.DashboardDataByPractice(item).ToList();
                    refineorders.AddRange(orders);

                }
                data = refineorders.Where(x => x.CreatedOn.Value.Date >= fromdate.Date && x.CreatedOn.Value.Date <= todate.Date && x.DeviceTypeId == 1 && (x.OrderStatusId == 2)).ToList();
            }
            return data;
        }

        private List<OrderViewModel> AsessoriesDetails(DateTime fromdate, DateTime todate, string[] list)
        {
            List<OrderViewModel> refineorders = new List<OrderViewModel>();
            List<OrderViewModel> data = new List<OrderViewModel>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    List<OrderViewModel> orders = orderService.DashboardDataByPractice(item).ToList();
                    refineorders.AddRange(orders);

                }
                data = refineorders.Where(x => x.CreatedOn.Value.Date >= fromdate.Date && x.CreatedOn.Value.Date <= todate.Date && (x.DeviceType.MainDeviceTypeId == 2 && x.OrderStatusId == 2)).ToList();
            }
            return data;
        }

        private List<OrderViewModel> ReturnDetails(DateTime fromdate, DateTime todate, string[] list)
        {
            List<OrderViewModel> refineorders = new List<OrderViewModel>();
            List<OrderViewModel> data = new List<OrderViewModel>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    List<OrderViewModel> orders = orderService.DashboardDataByPractice(item).ToList();
                    refineorders.AddRange(orders);

                }
                data = refineorders.Where(x => x.CreatedOn.Value.Date >= fromdate.Date && x.CreatedOn.Value.Date <= todate.Date && (x.DeviceTypeId == 6 || x.OrderStatusId == 2)).ToList();
            }
            return data;
        }

        private List<OrderViewModel> ReplacementDetails(DateTime fromdate, DateTime todate, string[] list)
        {
            List<OrderViewModel> refineorders = new List<OrderViewModel>();
            List<OrderViewModel> data = new List<OrderViewModel>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    List<OrderViewModel> orders = orderService.DashboardDataByPractice(item).ToList();
                    refineorders.AddRange(orders);

                }
                data = refineorders.Where(x => x.CreatedOn.Value.Date >= fromdate.Date && x.CreatedOn.Value.Date <= todate.Date && x.SenderId != null && (x.DeviceTypeId == 8 || x.OrderStatusId == 2)).ToList();
            }
            return data;
        }

        private List<OrderViewModel> CanceledDetails(DateTime fromdate, DateTime todate, string[] list)
        {
            List<OrderViewModel> refineorders = new List<OrderViewModel>();
            List<OrderViewModel> data = new List<OrderViewModel>();
            if (list != null)
            {
                foreach (var item in list)
                {
                    List<OrderViewModel> orders = orderService.DashboardDataByPractice(item).ToList();
                    refineorders.AddRange(orders);

                }
                data = refineorders.Where(x => x.CreatedOn.Value.Date >= fromdate.Date && x.CreatedOn.Value.Date <= todate.Date && (x.OrderStatusId == 4)).ToList();
            }
            return data;
        }

        private List<OrderViewModel> PendingSerialDetails()
        {
            var data = orderService.PendingSerialDetails();
            return data;
        }

        private List<OrderViewModel> PendingTaskDetails()
        {
            List<OrderViewModel> refineorders = new List<OrderViewModel>();
            var data = orderService.PendingTaskDetails();
            foreach (var item in data)
            {
                var order = new OrderViewModel()
                {
                    CreatedOn = item.CreatedOn == null ? null : item.CreatedOn,
                    Receiver = new ReceiverViewModel()
                    {
                        Name = item.PatientName,
                        Address_Line2 = item.PatientAddress

                    },
                    PracticeId = item.PatientClinic
                };
                refineorders.Add(order);

            }
            return refineorders;
        }

        private List<OrderViewModel> InTransitDetails()
        {
            var data = orderService.InTransitDetails();
            return data;
        }
        class BadgeData
        {
            public int DeliveredPatients { get; set; }
            public int newDeviceOrders { get; set; }
            public int asessoriesOrders { get; set; }
            public int returnOrders { get; set; }
            public int replacementOrders { get; set; }
            public int cancelOrders { get; set; }
        }
    }
}