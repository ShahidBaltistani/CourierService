using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Entities.TaskManagements
{
   public class TaskLog
    {
        public int Id { get; set; }
        public bool TaskStatus { get; set; }
        
        public int? CreatedBy { get; set; }
        public DateTime? CreatedOn { get; set; }

        public int? TaskDetailId { get; set; }
        public TaskDetail TaskDetail { get; set; }


    }
}
