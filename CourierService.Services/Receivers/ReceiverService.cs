using CourierService.Entities.Orders;
using CourierService.Repositories.Receivers;
using CourierService.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Services.Receivers
{
   public class ReceiverService : AutoMapperProfile
    {
         ReceiverRepository _repo = new ReceiverRepository();
        

        public ReceiverViewModel Add(ReceiverViewModel model)
        {
            var res = _mapper.Map<Receiver>(model);
            var data = _repo.Add(res);
            var item = _mapper.Map<ReceiverViewModel>(data);
            return item;
        }

        public OrderViewModel AddOrder(OrderViewModel model)
        {
            var res = _mapper.Map<Order>(model);
            var data = _repo.AddOrder(res);
            var item = _mapper.Map<OrderViewModel>(data);
            return item;
        }
    }
}
