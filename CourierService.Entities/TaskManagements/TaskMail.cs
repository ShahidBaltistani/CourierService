using CourierService.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Entities.TaskManagements
{
   public class TaskMail : BaseEntity
    {
        public TaskMail()
        {
            this.TaskDetails = new HashSet<TaskDetail>();
        }
        public string Description { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public virtual ICollection<TaskDetail> TaskDetails { get; set; }


    }
}
