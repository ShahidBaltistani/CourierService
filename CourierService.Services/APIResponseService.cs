using CourierService.Entities.Orders;
using CourierService.Repositories;
using CourierService.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Services
{
   public class APIResponseService :AutoMapperProfile
    {

        APIResponseRepository _repo = new APIResponseRepository();
        public IEnumerable<APIResponseViewModel> GetAll()
        {
            var data = _repo.GetAll();
            var res = _mapper.Map<IEnumerable<APIResponseViewModel>>(data);
            return res;
        }
        public APIResponseViewModel GetById(int id)
        {
            var data = _repo.GetById(id);
            var res = _mapper.Map<APIResponseViewModel>(data);
            return res;
        }

        public APIResponseViewModel GetByOrderId(int id)
        {
            var data = _repo.GetByOrderId(id);
            var res = _mapper.Map<APIResponseViewModel>(data);
            return res;
        }

        public APIResponse Add(APIResponseViewModel model)
        {
            var res = _mapper.Map<APIResponse>(model);
            var data = _repo.Add(res);
            return data;
        }

        public bool CheckIsExist(string SendleReference)
        {
            var res = _repo.CheckIsExist(SendleReference);
            return res;
        }
    }
}
