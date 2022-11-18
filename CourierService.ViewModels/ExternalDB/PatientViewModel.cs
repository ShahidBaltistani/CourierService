using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.ViewModels.ExternalDB
{
    public class PatientViewModel
    {
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        public string EHRNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ContactNo { get; set; }
        public string AlternateNumber { get; set; }
        public string Practice { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
    }
}
