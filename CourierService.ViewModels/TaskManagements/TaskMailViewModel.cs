using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CourierService.ViewModels.TaskManagements
{
    public class TaskMailViewModel
    {
        public TaskMailViewModel()
        {
            this.TaskDetails = new HashSet<TaskDetailViewModel>();
        }
        public int Id { get; set; }
        public string Description { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? ReceivedDate { get; set; }
        public virtual ICollection<TaskDetailViewModel> TaskDetails { get; set; }

        public string Date
        {
            get
            {
                return ((DateTime)this.ReceivedDate).ToString("dd-MMM-yyyy");
            }
        }
    }
}