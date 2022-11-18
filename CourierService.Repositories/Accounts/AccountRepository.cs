using CourierService.Database;
using CourierService.Entities.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Repositories.Accounts
{
   public class AccountRepository
    {
        CourierServiceContext _context = new CourierServiceContext();

        public User Login(string userName, string password)
        {
            var data = _context.Users.Where(x => x.Username == userName && x.Password == password).FirstOrDefault();
            return data;
        }

    }
}
