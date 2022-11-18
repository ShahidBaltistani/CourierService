using CourierService.Database;
using CourierService.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Repositories.Orders
{
   public class OrderLabelRepository
    {
        CourierServiceContext _context = new CourierServiceContext();

        public IEnumerable<OrderLabel> GetAll()
        {
            var data = _context.OrderLabels
                .Include(x => x.Order.Product)
                .Include(x => x.Order.OrderStatus)
                       .ToList();

            return data;

        }

        public OrderLabel GetById(int id)
        {
            var data = _context.OrderLabels.Where(x => x.Id == id)
                .Include(x => x.Order.Product)
                .Include(x => x.Order.OrderStatus)
                       .FirstOrDefault();

            return data;

        }
        public OrderLabel Add(OrderLabel model)
        {
            try
            {
                var check = _context.OrderLabels.Where(x=>x.OrderId == model.OrderId).FirstOrDefault();
                if (check == null)
                {
                    var data = _context.OrderLabels.Add(model);
                    _context.SaveChanges();
                    return data;
                }
                return null;
              
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }



        public bool Update(OrderLabel model)
        {
            try
            {
                if (model != null)
                {
                    var entity = _context.OrderLabels.Find(model.Id);
                    if (entity == null)
                    {
                        return false;
                    }

                    _context.Entry(entity).State = EntityState.Modified;
                    _context.SaveChanges();
                    return true;
                }
                return false;

            }
            catch (Exception)
            {

                return false;
            }

        }

        public OrderLabel Delete(int id)
        {
            var entity = _context.OrderLabels.Find(id);
            if (entity != null)
            {
              var data=  _context.OrderLabels.Remove(entity);
                _context.SaveChanges();
                return data;
            }
            return null;
        }

        public List<OrderLabel> LabelByOrderId(int orderId)
        {
            var data = _context.OrderLabels.Where(x => x.OrderId == orderId)
                .Include(x => x.Order.Product)
                .Include(x => x.Order.OrderStatus)
                       .ToList();
           
            return data;
        }
    }
}
