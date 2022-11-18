using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Entities.Orders
{
   public class OrderLabel
    {
        public int Id { get; set; }
        public int? OrderId { get; set; }
        public Order Order { get; set; }
        public string Formate { get; set; }
        public string Size { get; set; }
        public string Url { get; set; }


    }
}
