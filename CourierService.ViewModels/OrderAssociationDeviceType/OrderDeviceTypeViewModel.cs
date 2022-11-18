using CourierService.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.ViewModels.OrderAssociationDeviceType
{
   public class OrderDeviceTypeViewModel
    {
        public int? OrderId { get; set; }
        public OrderViewModel Order { get; set; }
        public int? DeviceTypeId { get; set; }
        public DeviceTypeViewModel DeviceType { get; set; }
    }
}
