using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.ViewModels.Orders
{
    public class DeviceTypeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // For MainDeviceType
        public int? MainDeviceTypeId { get; set; }
        public MainDeviceTypeViewModel MainDeviceType { get; set; }
    }
}
