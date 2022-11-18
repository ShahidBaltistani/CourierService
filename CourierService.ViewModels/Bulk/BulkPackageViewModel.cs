using CourierService.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.ViewModels.Bulk
{
   public class BulkPackageViewModel
    {
        public DateTime? OrderDate { get; set; }
        public string CustomerReference { get; set; }
        public string ShortSerial { get; set; }
        public string Practice { get; set; }
        public int? SenderId { get; set; }
        public virtual SenderViewModel Sender { get; set; }
        public int? ReceiverId { get; set; }
        public virtual ReceiverViewModel Receiver { get; set; }
        public int? OrderStatusId { get; set; }
        public virtual OrderStatusViewModel OrderStatus { get; set; }
        public int? ProductId { get; set; }
        public virtual ProductViewModel Product { get; set; }
        public int? SourceId { get; set; }
        public virtual SourceViewModel Source { get; set; }
        public int? DeviceTypeId { get; set; }
        public virtual DeviceTypeViewModel DeviceType { get; set; }
    }
}
