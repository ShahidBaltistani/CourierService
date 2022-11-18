using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Entities.Orders
{
   public class APIResponse : BaseEntity
    {
        public int? OrderId { get; set; }
        public Order Order { get; set; }
        public string ResponseText { get; set; }
        public string SendleOrderId { get; set; }
        public string SendleOrderUrl { get; set; }
        public string SendleRefrence { get; set; }
        public string SendleTrackingUrl { get; set; }

    }
}
