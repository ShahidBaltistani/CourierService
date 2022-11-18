using System;
using System.Collections.Generic;
using System.Text;

namespace CourierService.ViewModels.Orders
{
   public class BaseEntityViewModel
    {
        public int Id { get; set; }
        public DateTime? CreatedOn { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int? ModifiedBy { get; set; }
        public bool IsDeleted { get; set; } 
        public bool IsActive { get; set; }
    }
}
