using CourierService.Database;
using CourierService.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Threading.Tasks;
using CourierService.ViewModels.StreamVitalsIntegration;

namespace CourierService.Repositories.StreamVitals
{
    public class StreamVitalRepository
    {
        CourierServiceContext _context = new CourierServiceContext();

        public IEnumerable<Order> AssignDevice()
        {
            var data = _context.Orders.Where(

                x =>

                (x.NewPracticeId == 1 || x.NewPracticeId == 2 || x.NewPracticeId == 3 || x.NewPracticeId == 14)

                && (x.Receiver.SVPatientId != null)
                && (x.Receiver.EHR != null)
                && (x.DeliveredDate != null)

                && (x.DeviceTypeId == 1 || x.DeviceTypeId == 6 || x.DeviceTypeId == 8)

                ).Include(x => x.Receiver).Include(x => x.OrderStatus).ToList();


            foreach (var order in data.ToList())
            {
                if (order.DeviceTypeId == 1 && order.IsCompleted == true && order.IntegrationStatus == true)
                {
                    data.Remove(order);
                }
                else if (order.DeviceTypeId == 6 && order.IntegrationStatus == true)
                {
                    data.Remove(order);
                }
                else if (order.DeviceTypeId == 8 && order.IntegrationStatus == true)
                {
                    data.Remove(order);
                }


                if (order.DeviceTypeId == 1 && order.Customer_Reference == null)
                {
                    data.Remove(order);
                }
            }
            return data;
        }
        public IEnumerable<Order> GetPairList(int Id, DateTime CreatedOn)
        {
            try
            {
                var res = _context.Orders.OrderBy(x => x.SenderId)

                 .Where(x => x.ReceiverId == Id
                 && (x.OrderStatusId == 6)
                 && (x.DeviceTypeId == 8)
                 && (DbFunctions.TruncateTime(x.CreatedOn) == DbFunctions.TruncateTime(CreatedOn))
                 //&& (x.IsCompleted == false)
                 && x.IntegrationStatus == false).ToList();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Order GetReplacementReturn(int Id)
        {
            try
            {
                int con = Id+1;

                var res = _context.Orders.OrderBy(x => x.SenderId)
                 .Where(x => x.Id == con
                 && (x.OrderStatusId == 6)
                 && (x.DeviceTypeId == 8)
                 //&& (x.IsCompleted == false)
                 && x.IntegrationStatus == false).FirstOrDefault();
                return res;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public IEnumerable<Order> AssignDevice()
        //{
        //    var data = _context.Orders.Where(

        //        x => x.IsCompleted == false && (x.IntegrationStatus == false)

        //        && (x.NewPracticeId==1 || x.NewPracticeId == 2 || x.NewPracticeId == 3)

        //        && (x.Receiver.SVPatientId != null)
        //        && (x.Receiver.EHR != null)
        //        && (x.DeliveredDate != null)

        //        && (x.DeviceTypeId==1 || x.DeviceTypeId == 6 || x.DeviceTypeId == 8)

        //        ).Include(x => x.Receiver).Include(x => x.OrderStatus).ToList();




        //    foreach (var item in data.ToList())
        //    {
        //        List<int> NewOrderId = new List<int>();
        //        List<int> ReturnOrderId = new List<int>();
        //        List<int> VerifiedOrderId = new List<int>();

        //        if (item.DeviceTypeId == 1 && item.Customer_Reference == null)
        //        {
        //            data.Remove(item);
        //        }
        //        if (item.DeviceTypeId == 8)
        //        {
        //            var res = _context.Orders.OrderBy(x=>x.SenderId)

        //                .Where(x => x.ReceiverId == item.ReceiverId
        //                && (x.OrderStatusId == 6)
        //                && (x.DeviceTypeId == 8)
        //                && (DbFunctions.TruncateTime(x.CreatedOn) == DbFunctions.TruncateTime(item.CreatedOn))
        //                && (x.IsCompleted == false)
        //                && x.IntegrationStatus == false).ToList();

        //            foreach (var Rep in res)
        //            {
        //                if (Rep.SenderId != null  && Rep.Customer_Reference != null && Rep.DeliveredDate != null)
        //                {
        //                    NewOrderId.Add(Rep.Id);
        //                }
        //            }
        //            foreach (var Rep1 in res)
        //            {
        //                int con = Rep1.Id - 1;
        //                if (Rep1.SenderId == null && Rep1.DeliveredDate != null)
        //                {
        //                    var test = NewOrderId.FirstOrDefault(y=>y==con);
        //                    if (test > 0)
        //                    {
        //                        ReturnOrderId.Add(Rep1.Id);
        //                        VerifiedOrderId.Add(test);
        //                    }
        //                }
        //            }
        //            var neworder = VerifiedOrderId.FirstOrDefault(z => z == item.Id);
        //            if (neworder <= 0)
        //            {
        //                data.Remove(item);
        //            }
        //            var returnorder = ReturnOrderId.FirstOrDefault(z => z == item.Id);
        //            if (returnorder > 0)
        //            {
        //                UpdateIsCompleted(returnorder);
        //                UpdateIntegrationStatus(returnorder);
        //            }
        //        }
        //    }
        //    return data;
        //}
        public Order GetById(int id)
        {
            var data = _context.Orders.Where(x => x.Id == id)
                .Include(x => x.Product)
                .Include(x => x.Receiver)
                .Include(x => x.Sender)
                .Include(x => x.OrderStatus)
                       .FirstOrDefault();

            return data;

        }
        public bool UpdateIsCompleted(int id)
        {
            try
            {
                if (id != 0)
                {
                    var entity = _context.Orders.Find(id);
                    if (entity == null)
                    {
                        return false;
                    }
                    entity.IsCompleted = true;
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
        public bool UpdateIntegrationStatus(int id)
        {
            try
            {
                if (id != 0)
                {
                    var entity = _context.Orders.Find(id);
                    if (entity == null)
                    {
                        return false;
                    }
                    entity.IntegrationStatus = true;
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
        public bool UpdateInegrationFields(ResponceViewModel responce)
        {
            try
            {
                if (responce.RefrenceId != 0)
                {
                    var entity = _context.Orders.Find(responce.RefrenceId);
                    if (entity == null)
                    {
                        return false;
                    }

                    entity.IsCompleted = responce.IsCompleted;
                    entity.IntegrationRemarks = responce.NotCompletedRemarks;

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
    }
}
