using CourierService.Repositories.StreamVitals;
using CourierService.ViewModels.Orders;
using CourierService.ViewModels.StreamVitalsIntegration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Services.StreamVitals
{
   public class StreamVitalService : AutoMapperProfile
    {
        StreamVitalRepository _repo = new StreamVitalRepository();
        public IEnumerable<OrderViewModel> AssignDevice()
        {
            var data = _repo.AssignDevice();
            var res = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return res;
        }
        public IEnumerable<OrderViewModel> GetPairList(int Id, DateTime CreatedOn)
        {
            var data = _repo.GetPairList(Id, CreatedOn);
            var res = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return res;
        }

        public OrderViewModel GetReplacementReturn(int Id)
        {
            var data = _repo.GetReplacementReturn(Id);
            var res = _mapper.Map<OrderViewModel>(data);
            return res;
        }

        public bool UpdateIsCompleted(int Id)
        {
            var data = _repo.UpdateIsCompleted(Id);
            return data;
        }
        public bool UpdateIntegrationStatus(int Id)
        {
            var data = _repo.UpdateIntegrationStatus(Id);
            return data;
        }
        public bool UpdateInegrationFields(ResponceViewModel responce)
        {
            var data = _repo.UpdateInegrationFields(responce);
            return data;
        }
    }
}
