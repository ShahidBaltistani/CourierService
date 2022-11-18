using CourierService.Entities.NewPracticeF;
using CourierService.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Entities.TaskManagements
{
   public class TaskDetail : BaseEntity
    {
       
        public string Remarks { get; set; }
        public string PatientName { get; set; }
        public string PatientAddress { get; set; }
        public string PatientClinic { get; set; }
        public bool Status { get; set; }
        public string SendleRefrence { get; set; }
        public int TaskMailId { get; set; }
        public virtual TaskMail TaskMail { get; set; }

        // For Practice
        //public int? NewPracticeId { get; set; }
        //public NewPractice NewPractice { get; set; }
    }
}
