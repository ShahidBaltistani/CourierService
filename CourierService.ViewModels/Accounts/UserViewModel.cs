using CourierService.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.ViewModels.Accounts
{
   public class UserViewModel :BaseEntityViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int RoleId { get; set; }
        public RoleViewModel Role { get; set; }

    }
}
