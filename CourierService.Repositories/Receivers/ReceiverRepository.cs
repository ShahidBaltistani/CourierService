using CourierService.Database;
using CourierService.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Repositories.Receivers
{
  public class ReceiverRepository
    {
         CourierServiceContext _context = new CourierServiceContext();

        public Receiver Add(Receiver model)
        {
            try
            {
                var data = _context.Receivers.Add(model);
                _context.SaveChanges();
                return data;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        public Order AddOrder(Order model)
        {
            try
            {
               var senderId = _context.Senders.Select(x => x.Id).FirstOrDefault();
               var productId = _context.Products.Select(x => x.Id).FirstOrDefault();
                //model.First_Mile_Option = "pickup";
                //model.Description = "Please Call Support Number";
                //model.OrderStatusId = 1;
                //model.DeliveredDate = null;
                model.SenderId = senderId;
                model.ProductId = productId;
                var data = _context.Orders.Add(model);
                _context.SaveChanges();
                return data;

            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
