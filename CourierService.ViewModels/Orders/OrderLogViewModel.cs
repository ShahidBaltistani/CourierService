using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.ViewModels.Orders
{
   public class OrderLogViewModel
    {

        public int? OrderId { get; set; }
        public OrderViewModel Order { get; set; }
        public int? ReceiverId { get; set; }
        public ReceiverViewModel Receiver { get; set; }
        public int? DeviceTypeId { get; set; }
        public DeviceTypeViewModel DeviceType { get; set; }
        public int? ReferenceOrderId { get; set; }
        public string SendleRefrence { get; set; }
        public string CancelRemarks { get; set; }
    }
}
