using CourierService.Entities.Orders;
using CourierService.Repositories.OrderAssociationDeviceType;
using CourierService.ViewModels.OrderAssociationDeviceType;
using CourierService.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Services.OrderAssociationDeviceType
{
  public  class OrderDeviceTypeService : AutoMapperProfile
    {
        OrderDeviceTypeRepository _repo = new OrderDeviceTypeRepository();

        public IEnumerable<OrderDeviceTypeViewModel> GetAll_RPD_Device()
        {
            var data = _repo.GetAll_RPD_Device();
            var res = _mapper.Map<IEnumerable<OrderDeviceTypeViewModel>>(data);
            return res;
        }

        public IEnumerable<OrderDeviceTypeViewModel> GetAll_Accessories()
        {
            var data = _repo.GetAll_Accessories();
            var res = _mapper.Map<IEnumerable<OrderDeviceTypeViewModel>>(data);
            return res;
        }

        public IEnumerable<OrderDeviceTypeViewModel> GetAll_BulkPackagings()
        {
            var data = _repo.GetAll_BulkPackagings();
            var res = _mapper.Map<IEnumerable<OrderDeviceTypeViewModel>>(data);
            return res;
        }

        public IEnumerable<OrderDeviceTypeViewModel> FilterProcessedOrders(OrderViewModel model)
        {
            var res = _mapper.Map<Order>(model);
            var data = _repo.FilterProcessedOrders(res);
            var list = _mapper.Map<IEnumerable<OrderDeviceTypeViewModel>>(data);
            return list;
        }
        public OrderDeviceTypeViewModel AddOrderAssociation(int orderId, int deviceTypeId)
        {
            var data = _repo.AddOrderAssociation(orderId, deviceTypeId);
            var res = _mapper.Map<OrderDeviceTypeViewModel>(data);
            return res;
        }

    }
}
