using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.ViewModels.StreamVitalsIntegration
{
    public class ResponceViewModel
    {
        public int RefrenceId { get; set; }
        public bool IsCompleted { get; set; }
        public string NotCompletedRemarks { get; set; }
    }
}
