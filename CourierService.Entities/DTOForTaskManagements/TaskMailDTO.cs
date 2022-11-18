using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Entities.DTOForTaskManagements
{
    public class TaskMailDTO
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public bool IsCompleted { get; set; }

        public DateTime? ReceivedDate { get; set; }
        public virtual List<TaskDetailDTO> TaskDetails { get; set; }
    }
}
