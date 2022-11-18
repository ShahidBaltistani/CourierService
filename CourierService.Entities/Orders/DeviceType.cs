using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Entities.Orders
{
    public class DeviceType
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // For MainDeviceType
        public int? MainDeviceTypeId { get; set; }
        public MainDeviceType MainDeviceType { get; set; }
    }
}
