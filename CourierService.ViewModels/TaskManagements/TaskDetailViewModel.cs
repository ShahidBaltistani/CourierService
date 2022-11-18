using CourierService.ViewModels.NewPracticeF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.ViewModels.TaskManagements
{
   public class TaskDetailViewModel
    {
       
            public int Id { get; set; }
            public string Remarks { get; set; }
            public string PatientName { get; set; }
            public string PatientAddress { get; set; }
            public string PatientClinic { get; set; }
            public bool Status { get; set; }
            public string SendleRefrence { get; set; }
            public DateTime? CreatedOn { get; set; }
            public int? CreatedBy { get; set; }
            public DateTime? ModifiedOn { get; set; }
            public int? ModifiedBy { get; set; }
            public bool IsDeleted { get; set; }
            public bool IsActive { get; set; }
            public bool Replacement { get; set; }
            public int TaskMailId { get; set; }
            public virtual TaskMailViewModel TaskMail { get; set; }


        // For Practice
        //public int? NewPracticeId { get; set; }
        //public NewPracticeViewModel NewPractice { get; set; }
    }
}
