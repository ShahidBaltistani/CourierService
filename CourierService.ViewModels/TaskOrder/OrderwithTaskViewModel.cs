using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.ViewModels.TaskOrder
{
    public class OrderwithTaskViewModel
    {
        public bool? AddToTaskManagement { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Description { get; set; }
        public DateTime? ReceivedDate { get; set; }
    }
}
