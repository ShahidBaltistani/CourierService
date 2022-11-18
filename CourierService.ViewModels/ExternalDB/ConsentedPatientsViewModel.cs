using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.ViewModels.ExternalDB
{
    public class ConsentedPatientsViewModel
    {
        public string FullName
        {
            get { return firstName + " " + lastName; }
        }
        public Guid patientId { get; set; }
        public Guid practiceId { get; set; }
        public string ehrNumber { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string landlineNumber { get; set; }
        public string mobilePhoneNumber { get; set; }
        public string practiceName { get; set; }
        public string status { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string country { get; set; }
        public string postalCode { get; set; }
        public bool IsExist { get; set; } = false;
    }
}
