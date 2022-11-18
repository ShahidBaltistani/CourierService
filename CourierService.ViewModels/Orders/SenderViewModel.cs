using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.ViewModels.Orders
{
   public class SenderViewModel : BaseEntityViewModel
    {
        public string Instructions { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Company { get; set; }
        public string Address_Line1 { get; set; }
        public string Address_Line2 { get; set; }
        public string Suburb { get; set; }
        public string Postcode { get; set; }
        public string State_Name { get; set; }
        public string Country { get; set; }

    }
}
