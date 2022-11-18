using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.ViewModels.DTOForTaskManagements
{
    public class TaskMailDTOViewModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? ReceivedDate { get; set; }

        public bool IsCompleted { get; set; }

        public virtual List<TaskDetailDTOViewModel> TaskDetails { get; set; }



        public string Date
        {
            get
            {
                return ((DateTime)this.ReceivedDate).ToString("dd-MMM-yyyy");
            }
        }
    }
}
