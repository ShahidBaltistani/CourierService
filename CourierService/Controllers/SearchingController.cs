using CourierService.Services;
using CourierService.Services.Orders;
using CourierService.ViewModels.Orders;
using CourierService.ViewModels.SearchingDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CourierService.Controllers
{
    [Authorize]
    public class SearchingController : Controller
    {
        // GET: Searching

        // Adding Service
        OrderService orderService = new OrderService();
        APIResponseService aPIResponseService = new APIResponseService();
        public ActionResult Search(string Search)
        {
            var data = orderService.SearchLikeSendle(Search);
            return PartialView("_SearchingPartial",data);
        }
        //public ActionResult Search(string Search, int page = 1)
        //{
        //    if (page < 0 || page == 0)
        //    {
        //        page = 1;
        //    }
        //    int totalPage = 0;
        //    int pageSize = 5;
        //    int totalRecord = 0;
        //    var data = orderService.SearchLikeSendle(Search, page, pageSize, out totalPage, out totalRecord);


        //    ViewBag.dbCount = totalPage;
        //    ViewBag.currentPage = page;
        //    ViewBag.SearchTerm = Search;


        //    return PartialView("_SearchingPartial", data);
        //}

        public async Task<ActionResult> PatientDetail(int Id)
        {
            var data = new SearchingDetailViewModel
            {
                OrderViewModel= orderService.PatientDetail(Id),
                OrderTrackingViewModel=await TrackOrder(Id)
            };
            return View(data);
        }
        public async Task<OrderTrackingViewModel> TrackOrder(int Id)
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
            return data;
        }
        public ActionResult CheckHistory(int Id)
        {
            var data = orderService.CheckHistory(Id).Select(
                (OL)=>new
                {
                    Name=OL.Receiver.Name,
                    Device=OL.DeviceType.Name,
                    Remarks=OL.CancelRemarks,
                    CreatedDate=OL.Order.CreatedOn.ToString(),
                    OrderStatus=OL.Order.OrderStatus.Name,
                    Date=OL.Order.DeliveredDate.ToString()
                }             
                );
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult DupPatientswithReturnedStatus()
        {
            var data = orderService.GetDuplicatePatients();
            return View(data);
        }
    }
}