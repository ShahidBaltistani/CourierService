using CourierService.Services;
using CourierService.Services.OrderLogs;
using CourierService.Services.Orders;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CourierService
{
    public class HangFireService
    {
        OrderService orderService = new OrderService();
        APIResponseService aPIResponseService = new APIResponseService();
        OrderLabelService orderLabelService = new OrderLabelService();
        OrderLogService _orderLogService = new OrderLogService();
        public async Task GetAllProcessedOrders()
        {
            var orders = orderService.GetAllProcessedOrdersHangfire().ToList();
            foreach (var item in orders)
            {
                var SendleReference = aPIResponseService.GetByOrderId(item.Id);

                if (SendleReference != null)
                {
                    string Sendle_Reference = SendleReference.SendleRefrence;

                    using (var client = Utility.GetHttpClient())
                    {
                        var responseTask = client.GetAsync("https://api.sendle.com/api/tracking/" + Sendle_Reference + "");
                        responseTask.Wait();
                        var result = responseTask.Result;
                        if (result.IsSuccessStatusCode)
                        {
                            var response = await result.Content.ReadAsStringAsync();
                            dynamic data1 = JObject.Parse(response);
                            var state = data1.state;
                            string ddate = data1.status.last_changed_at;
                            var newstring = ddate.Remove(ddate.Length - 3, 3) + "Z";
                            var dddate = Convert.ToDateTime(newstring);
                            if (state == "Delivered")
                            {
                                orderService.UpdateIsDelivered(item.Id, dddate);

                                orderService.UpdateOrderStatus(item.Id,6);
                            }
                            else if (state == "Cancelled")
                            {
                                orderService.UpdateOrderStatus(item.Id, 4);
                            }
                            else if (state == "Return to Sender")
                            {
                                orderService.UpdateOrderStatus(item.Id, 5);
                            }

                            string CurrentStatus = data1.status.description;
                            orderService.UpdateCurrentStatus(item.Id, CurrentStatus);
                        }
                       
                    }
                }
            }
           
        }
    }
}