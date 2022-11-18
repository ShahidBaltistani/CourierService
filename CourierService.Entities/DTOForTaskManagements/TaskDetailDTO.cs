using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Entities.DTOForTaskManagements
{
    public class TaskDetailDTO
    {
        public int Id { get; set; }
        public string Remarks { get; set; }
        public string PatientName { get; set; }
        public string PatientAddress { get; set; }
        public string PatientClinic { get; set; }
        public bool Status { get; set; }
        public string SendleRefrence { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsActive { get; set; }
        public int TaskMailId { get; set; }
        public virtual TaskMailDTO TaskMail { get; set; }
    }
}
