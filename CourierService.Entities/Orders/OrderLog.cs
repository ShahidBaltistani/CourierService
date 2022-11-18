using CourierService.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Entities.Orders
{
   public class OrderLog
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public Order Order { get; set; }
        public int? ReceiverId { get; set; }
        public Receiver Receiver { get; set; }
        public int? DeviceTypeId { get; set; }
        public DeviceType DeviceType { get; set; }
        public int? ReferenceOrderId { get; set; }
        public string SendleRefrence { get; set; }
        public string CancelRemarks { get; set; }
    }
}
