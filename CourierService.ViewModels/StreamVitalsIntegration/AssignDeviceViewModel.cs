using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.ViewModels.StreamVitalsIntegration
{
    public class AssignDeviceViewModel
    {
        public Guid PatientId { get; set; }
        public string EHR { get; set; }
        public string IMEI { get; set; }
        public int RefrenceId { get; set; }
        public string RequestType { get; set; }
        public string Notes { get; set; }
        public DateTime DeliveredOn { get; set; }
    }
}
