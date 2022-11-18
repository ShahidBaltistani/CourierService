using CourierService.Database;
using CourierService.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Repositories
{
   public class APIResponseRepository
    {

        CourierServiceContext _context = new CourierServiceContext();

        public IEnumerable<APIResponse> GetAll()
        {
            var data = _context.APIResponses
                .Include(x => x.Order.OrderStatus)
                .Include(x => x.Order.Product)
                       .ToList();

            return data;

        }

        public APIResponse GetById(int id)
        {
            var data = _context.APIResponses.Where(x => x.Id == id)
               .Include(x => x.Order.OrderStatus)
                .Include(x => x.Order.Product)
                       .FirstOrDefault();

            return data;

        }

        public APIResponse GetByOrderId(int id)
        {
            var data = _context.APIResponses.Where(x => x.OrderId == id)
               .Include(x => x.Order.OrderStatus)
                .Include(x => x.Order.Product)
                       .OrderByDescending(x => x.Id).FirstOrDefault();
            return data;

        }

        


        public APIResponse Add(APIResponse model)
        {
            var data = _context.APIResponses.Add(model);
            _context.SaveChanges();
            return data;

        }


        public bool CheckIsExist(string SendleReference)
        {
            var data = _context.APIResponses.Where(x => x.SendleRefrence == SendleReference).FirstOrDefault();
            if (data==null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
