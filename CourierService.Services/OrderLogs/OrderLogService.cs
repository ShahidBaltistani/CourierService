using CourierService.Entities.Orders;
using CourierService.Repositories.OrderLogs;
using CourierService.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Services.OrderLogs
{
   public class OrderLogService : AutoMapperProfile
    {
        OrderLogRepository _repo = new OrderLogRepository();
        public IEnumerable<OrderLogViewModel> GetAll()
        {
            var data = _repo.GetAll();
            var res = _mapper.Map<IEnumerable<OrderLogViewModel>>(data);
            return res;
        }
        public OrderLogViewModel GetById(int id)
        {
            var data = _repo.GetById(id);
            var res = _mapper.Map<OrderLogViewModel>(data);
            return res;
        }

        public OrderLog Add(OrderLogViewModel model)
        {
            var res = _mapper.Map<OrderLog>(model);
            var data = _repo.Add(res);
            return data;
        }

        public OrderLogViewModel GetOrderLogOrderId(int id)
        {
            var data = _repo.GetOrderLogOrderId(id);
            var res = _mapper.Map<OrderLogViewModel>(data);
            return res;
        }

        public OrderLog UpdateCancelRemarks(int OrderId, string remarks)
        {
            var data = _repo.UpdateCancelRemarks(OrderId, remarks);
            return data;
        }
    }
}
