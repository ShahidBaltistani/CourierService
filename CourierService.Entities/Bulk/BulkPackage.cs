using CourierService.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Entities.Bulk
{
    public class BulkPackage : BaseEntity
    {
        public DateTime? OrderDate { get; set; }
        public string CustomerReference { get; set; }
        public string ShortSerial { get; set; }
        public string Practice { get; set; }
        public int? SenderId { get; set; }
        public virtual Sender Sender { get; set; }
        public int? ReceiverId { get; set; }
        public virtual Receiver Receiver { get; set; }
        public int? OrderStatusId { get; set; }
        public virtual OrderStatus OrderStatus { get; set; }
        public int? ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int? SourceId { get; set; }
        public virtual Source Source { get; set; }
        public int? DeviceTypeId { get; set; }
        public virtual DeviceType DeviceType { get; set; }

    }
}
