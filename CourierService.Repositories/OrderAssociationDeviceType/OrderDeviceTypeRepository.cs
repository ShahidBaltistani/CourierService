using CourierService.Database;
using CourierService.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Repositories.OrderAssociationDeviceType
{
   public class OrderDeviceTypeRepository
    {
        CourierServiceContext _context = new CourierServiceContext();

        public IEnumerable<OrderDeviceType> GetAll_RPD_Device()
        {
            var data = _context.OrderDeviceTypes.Where(x=>x.DeviceTypeId == 1)
                .Include(x => x.Order.Receiver)
                .Include(x => x.Order.Sender)
                .Include(x => x.Order.OrderStatus)
                .Include(x => x.Order.Product)
                .Include(x => x.DeviceType)
                       .ToList();
            return data;

        }

        public IEnumerable<OrderDeviceType> GetAll_BulkPackagings()
        {
            var data = _context.OrderDeviceTypes.Where(x => x.DeviceTypeId == 4)
                .Include(x => x.Order.Receiver)
                .Include(x => x.Order.Sender)
                .Include(x => x.Order.OrderStatus)
                .Include(x => x.Order.Product)
                .Include(x => x.DeviceType)
                       .ToList();
            return data;

        }

        public IEnumerable<OrderDeviceType> FilterProcessedOrders(Order model)
        {


            if (model.IsDelivered == true)
            {
                var data = _context.OrderDeviceTypes.Where(x => (x.DeviceTypeId == 1) &&
                            (x.Order.IsDelivered == model.IsDelivered))
            .Include(x => x.Order.Product)
            .Include(x => x.Order.Receiver)
            .Include(x => x.Order.Sender)
            .Include(x => x.Order.OrderStatus)
            .Include(x=>x.DeviceType)
            .ToList();

                return data;
            }
            else
            {
                var data = _context.OrderDeviceTypes.Where(x => (x.DeviceTypeId == 1) &&
                (x.Order.PracticeId.Trim().ToLower().Contains(model.PracticeId.Trim().ToLower())
                || x.Order.Receiver.Name.Trim().ToLower().Contains(model.Receiver.Name.Trim().ToLower())
                || x.Order.Receiver.Address_Line2.Trim().ToLower().Contains(model.Receiver.Address_Line2.Trim().ToLower())
                || x.Order.Customer_Reference.Trim().ToLower().Contains(model.Customer_Reference.Trim().ToLower())
                || DbFunctions.TruncateTime(x.Order.CreatedOn) == model.CreatedOn
                ))
                .Include(x => x.Order.Product)
                .Include(x => x.Order.Receiver)
                .Include(x => x.Order.Sender)
                .Include(x => x.Order.OrderStatus)
                .ToList();

                return data;
            }
        }

        public IEnumerable<OrderDeviceType> GetAll_Accessories()
        {
            var data = _context.OrderDeviceTypes.Where(x => x.DeviceTypeId == 2)
                        .Include(x => x.Order.Product)
                        .Include(x => x.Order.Receiver)
                        .Include(x => x.Order.Sender)
                        .Include(x => x.Order.OrderStatus)
                        .Include(x => x.Order.DeviceType)
                        .ToList()
                        .Union(_context.OrderDeviceTypes.Where(x =>  x.DeviceTypeId == 3)
                       .Include(x => x.Order.Product)
                        .Include(x => x.Order.Receiver)
                        .Include(x => x.Order.Sender)
                        .Include(x => x.Order.OrderStatus)
                        .Include(x => x.Order.DeviceType)

                        .ToList()
                        .Union(_context.OrderDeviceTypes.Where(x => x.DeviceTypeId == 5)
                        .Include(x => x.Order.Product)
                        .Include(x => x.Order.Receiver)
                        .Include(x => x.Order.Sender)
                        .Include(x => x.Order.OrderStatus)
                        .Include(x => x.Order.DeviceType)

                        .ToList()));
            return data;

        }
        public OrderDeviceType AddOrderAssociation(int orderId, int deviceTypeId)
        {
            var deviceType = new OrderDeviceType { DeviceTypeId = deviceTypeId, OrderId = orderId };
            var data = _context.OrderDeviceTypes.Add(deviceType);
            _context.SaveChanges();
            return deviceType;
        }
    }
}
