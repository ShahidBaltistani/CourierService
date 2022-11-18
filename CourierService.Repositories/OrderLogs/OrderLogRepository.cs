using CourierService.Database;
using CourierService.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Repositories.OrderLogs
{
   public class OrderLogRepository
    {

        CourierServiceContext _context = new CourierServiceContext();

        public IEnumerable<OrderLog> GetAll()
        {
            var data = _context.OrderLogs
                .Include(x => x.Order)
                .Include(x => x.Receiver)
                .Include(x => x.DeviceType)
                       .ToList();

            return data;

        }

        public OrderLog GetById(int id)
        {
            var data = _context.OrderLogs.Where(x => x.Id == id)
                 .Include(x => x.Order)
                .Include(x => x.Receiver)
                .Include(x => x.DeviceType)
                       .FirstOrDefault();

            return data;

        }
        public OrderLog Add(OrderLog model)
        {
            try
            {
                    var data = _context.OrderLogs.Add(model);
                    _context.SaveChanges();
                    return data;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public OrderLog GetOrderLogOrderId(int id)
        {
            var data = _context.OrderLogs.Where(x => x.OrderId == id)
                 .Include(x => x.Order)
                .Include(x => x.Receiver)
                .Include(x => x.DeviceType)
                       .FirstOrDefault();

            return data;

        }
        public OrderLog UpdateCancelRemarks(int OrderId, string remarks)
        {
            try
            {
                var data = _context.OrderLogs.Where(x => x.OrderId == OrderId).FirstOrDefault();
                if (data != null)
                {
                    data.CancelRemarks = remarks;
                    _context.SaveChanges();
                    return data;
                }
                return data;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

    }
}
