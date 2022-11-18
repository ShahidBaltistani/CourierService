using CourierService.Services;
using CourierService.Services.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace CourierService.Controllers
{
    [Authorize]
    public class InProcessController : Controller
    {
        // GET: InProcess

        // Adding Service
        OrderService orderService = new OrderService();
        APIResponseService aPIResponseService = new APIResponseService();
        OrderLabelService orderLabelService = new OrderLabelService();

        public ActionResult InProcessOrders()
        {
            return View();
        }
    }
}