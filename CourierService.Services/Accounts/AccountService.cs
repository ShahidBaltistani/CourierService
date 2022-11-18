using CourierService.Repositories.Accounts;
using CourierService.ViewModels.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Services.Accounts
{
    
   public class AccountService : AutoMapperProfile
    {
        AccountRepository _repo = new AccountRepository();

        public UserViewModel Login(string userName, string password)
        {
            var data = _repo.Login(userName, password);
            var res = _mapper.Map<UserViewModel>(data);
            return res;

        }
    }
}
