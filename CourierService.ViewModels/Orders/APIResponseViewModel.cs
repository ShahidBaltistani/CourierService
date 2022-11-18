using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.ViewModels.Orders
{
   public class APIResponseViewModel : BaseEntityViewModel
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public OrderViewModel Order { get; set; }
        public string ResponseText { get; set; }
        public string SendleOrderId { get; set; }
        public string SendleOrderUrl { get; set; }
        public string SendleRefrence { get; set; }
        public string SendleTrackingUrl { get; set; }


    }
}
