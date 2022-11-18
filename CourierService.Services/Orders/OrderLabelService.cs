using CourierService.Entities.Orders;
using CourierService.Repositories.Orders;
using CourierService.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Services.Orders
{
   public class OrderLabelService : AutoMapperProfile
    {
        OrderLabelRepository _repo = new OrderLabelRepository();
        public IEnumerable<OrderLabelViewModel> GetAll()
        {
            var data = _repo.GetAll();
            var res = _mapper.Map<IEnumerable<OrderLabelViewModel>>(data);
            return res;
        }
        public OrderLabelViewModel GetById(int id)
        {
            var data = _repo.GetById(id);
            var res = _mapper.Map<OrderLabelViewModel>(data);
            return res;
        }

        public OrderLabel Add(OrderLabelViewModel model)
        {
            var res = _mapper.Map<OrderLabel>(model);
            var data = _repo.Add(res);
            return data;
        }

        public bool Update(OrderLabelViewModel model)
        {
            var res = _mapper.Map<OrderLabel>(model);
            var data = _repo.Update(res);
            return data;
        }

        public OrderLabelViewModel Delete(int id)
        {
            var data = _repo.Delete(id);
            var res = _mapper.Map<OrderLabelViewModel>(data);
            return res;
        }

        public List<OrderLabelViewModel> LabelByOrderId(int orderId)
        {
            var data = _repo.LabelByOrderId(orderId);
            var res = _mapper.Map<List<OrderLabelViewModel>>(data);
            return res;
        }
    }
}
