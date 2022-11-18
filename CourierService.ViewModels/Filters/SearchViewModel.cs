using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.ViewModels.Filters
{
  public class SearchViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address_Line2 { get; set; }
        public string PracticeId { get; set; }
        public string Customer_Refrence { get; set; }
        public bool Practice { get; set; }


    }
}
