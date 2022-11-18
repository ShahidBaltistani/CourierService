using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Entities.Orders
{
    public class OrderDeviceType
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public Order Order { get; set; }
        public int? DeviceTypeId { get; set; }
        public DeviceType DeviceType { get; set; }
    }
}
