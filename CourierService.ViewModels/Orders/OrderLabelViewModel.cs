using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.ViewModels.Orders
{
   public class OrderLabelViewModel
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public OrderViewModel Order { get; set; }
        public string Formate { get; set; }
        public string Size { get; set; }
        public string Url { get; set; }
    }
}
