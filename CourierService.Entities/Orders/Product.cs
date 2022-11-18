using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Entities.Orders
{
   public class Product : BaseEntity
    {
        public string VolumeValue { get; set; }
        public string VolumeUnits { get; set; }
        public string WeightValue { get; set; }
        public string WeightUnits { get; set; }
        public string Height { get; set; }
        public string Width { get; set; }
        public string Length { get; set; }


    }
}
