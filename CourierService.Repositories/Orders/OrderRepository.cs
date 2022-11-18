using CourierService.Database;
using CourierService.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using CourierService.ViewModels.Filters;
using CourierService.Entities.TaskManagements;

namespace CourierService.Repositories.Orders
{
   public class OrderRepository
    {
        CourierServiceContext _context = new CourierServiceContext();

        public IEnumerable<Order> DashboardDataByPractice(string Practice)
        {
            var data = _context.Orders.OrderByDescending(x=>x.Id).Include(x=>x.Receiver).Where(x=>x.PracticeId==Practice && x.IsDeleted==false).Include(x=>x.DeviceType.MainDeviceType).ToList();
            return data;
        }

        public DashboardOrders Dashboard()
        {
            var data = new DashboardOrders
            {
                PendingSerialAssigning = _context.Orders.Where(x => x.Customer_Reference == null && x.IsDeleted == false && (x.OrderStatusId == 6 || x.OrderStatusId == 2)).ToList().Count(),
                PendingTask = _context.TaskDetails.Where(x => x.Status == false && x.IsDeleted==false && x.IsActive==true).ToList().Count(),
                InTransit = _context.Orders.Where(x=>x.OrderStatusId==2 && x.IsDeleted == false).ToList().Count()
            };
            return data;
        }

        public IEnumerable<Order> PendingSerialDetails()
        {
            var data = _context.Orders.Include(x=>x.Receiver).Where(x => x.Customer_Reference == null && x.IsDeleted == false && (x.OrderStatusId == 6 || x.OrderStatusId == 2)).ToList();
            return data;
        }
        public IEnumerable<TaskDetail> PendingTaskDetails()
        {
            var data = _context.TaskDetails.Where(x => x.Status == false && x.IsDeleted == false && x.IsActive == true).ToList();
            return data;
        }
        public IEnumerable<Order> InTransitDetails()
        {
            var data = _context.Orders.Include(x => x.Receiver).Where(x => x.OrderStatusId == 2 && x.IsDeleted == false).ToList();
            return data;
        }

        public IEnumerable<Order> GetAllTodayCreatedOrders()
        {
            var today = DateTime.Today;
            var data = _context.Orders.Where(x => DbFunctions.TruncateTime(x.CreatedOn) == today)
                .Include(x => x.Receiver)
                .ToList();
            return data;
        }

        public IEnumerable<Order> GetAllYesterdayDeliveredOrders()
        {
            var yesterday = DateTime.Today.AddDays(-1);
            var data = _context.Orders.Where(x => DbFunctions.TruncateTime(x.DeliveredDate) == yesterday)
                .Include(x => x.Receiver)
                .ToList();
            return data;
        }


        public IEnumerable<OrderLog> CheckHistory(int ReceiverId)
        {
            var data=_context.OrderLogs.Where(x=>x.ReceiverId==ReceiverId)
                .Include(x => x.Order.Product)
                .Include(x => x.Receiver)
                .Include(x => x.Order.Sender)
                .Include(x => x.Order.OrderStatus)
                .Include(x => x.DeviceType)
                .Include(x => x.Order.Source)
                .ToList();
            return data;
        }
        public IEnumerable<OrderLog> SearchLikeSendle(string Search)
        {


            var data = _context.OrderLogs.Where(x => x.Receiver.Name.Trim().Replace(" ","").ToLower().Contains(Search.Trim().Replace(" ", "").ToLower()))
                     .Include(x => x.Order.Product)
                     .Include(x => x.Order.Receiver)
                     .Include(x => x.Order.Sender)
                     .Include(x => x.Order.OrderStatus)
                     .Include(x => x.DeviceType)
                     .Include(x => x.Order.Source)
                     .ToList();
            return data;
        }
        //public IEnumerable<OrderLog> SearchLikeSendle(string Search, int page, int pageSize, out int totalPage, out int totalRecord)
        //{
        //    var data = _context.OrderLogs.Where(x => x.Receiver.Name.Trim().ToLower().Contains(Search.Trim().ToLower()))
        //             .Include(x => x.Order.Product)
        //             .Include(x => x.Order.Receiver)
        //             .Include(x => x.Order.Sender)
        //             .Include(x => x.Order.OrderStatus)
        //             .Include(x => x.DeviceType)
        //             .Include(x => x.Order.Source)
        //             .ToList();
        //    totalRecord = data.Count();
        //    totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
        //    data = data.OrderBy(a => a.Id).Skip(((page - 1) * pageSize)).Take(pageSize).ToList();
        //    return data;
        //}
        //public IEnumerable<Order> SearchLikeSendle(string Search, int page, int pageSize, out int totalPage, out int totalRecord)
        //{
        //    var data = _context.Orders.Where(x => x.Receiver.Name.Trim().ToLower().Contains(Search.Trim().ToLower()))
        //             .Include(x => x.Product)
        //             .Include(x => x.Receiver)
        //             .Include(x => x.Sender)
        //             .Include(x => x.OrderStatus)
        //             .Include(x => x.DeviceType)
        //             .Include(x => x.Source)
        //             .ToList();
        //    totalRecord = data.Count();
        //    totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
        //    data = data.OrderBy(a => a.Id).Skip(((page - 1) * pageSize)).Take(pageSize).ToList();
        //    return data;
        //}
        public Order PatientDetail(int Id)
        {
            var data = _context.Orders.Where(x => x.Id == Id)
                .Include(x => x.Product)
                .Include(x => x.Receiver)
                .Include(x => x.Sender)
                .Include(x => x.OrderStatus)
                .Include(x => x.DeviceType)
                .Include(x => x.Source)
                .FirstOrDefault();
            return data;
        }

        public Order AddSingleOrder(Order model)
        {
            var order = new Order();
            try
            {
                var senderId = _context.Senders.Select(x => x.Id).FirstOrDefault();
                var productId = _context.Products.Select(x => x.Id).FirstOrDefault();
                if (model != null)
                {
                    model.First_Mile_Option = "pickup";
                    model.Description = "Please Call Support Number";
                    model.SenderId = senderId;
                    model.ProductId = productId;

                    model.OrderStatusId = 1;

                    model.DeliveredDate = null;
                    //model.IsActive = true;



                    var data =  _context.Orders.Add(model);
                    _context.SaveChanges();
                    if (data != null)
                    {
                        data.PatientId = data.ReceiverId.Value;
                        _context.SaveChanges();
                    }


                    return data;
                }

                return order;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public Order AddSingleReturnOrder(Order model)
        {
            var order = new Order();
            try
            {
                var productId = _context.Products.Select(x => x.Id).FirstOrDefault();
                if (model != null)
                {
                    model.First_Mile_Option = "pickup";
                    model.Description = "Please Call Support Number";
                    model.ProductId = productId;
                    model.DeliveredDate = null;

                    //model.IsActive = true;

                    var data = _context.Orders.Add(model);
                    _context.SaveChanges();
                    
                    
                    return data;
                }

                return order;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }


        public IEnumerable<Order> GetAll()
        {
            var data = _context.Orders
                .Include(x=>x.Product)
                .Include(x => x.Receiver)
                .Include(x => x.Sender)
                .Include(x=>x.OrderStatus)
                       .ToList();

            return data;
                
        }

        public Order GetById(int id)
        {
            var data = _context.Orders.Where(x=>x.Id == id)
                .Include(x => x.Product)
                .Include(x => x.Receiver)
                .Include(x => x.Sender)
                .Include(x => x.OrderStatus)
                       .FirstOrDefault();

            return data;

        }
        public bool Add(List<Order> model)
        {
            try
            {
                var orderList = new List<Order>();
                try
                {
                    var senderId = _context.Senders.Select(x => x.Id).FirstOrDefault();
                    var productId = _context.Products.Select(x => x.Id).FirstOrDefault();
                    if (model != null)
                    {
                        foreach (var item in model)
                        {
                            item.First_Mile_Option = "pickup";
                            item.Description = "Please Call Support Number";
                            item.SenderId = senderId;
                            item.ProductId = productId;
                            item.OrderStatusId = 1;

                            item.DeliveredDate = null;
                            item.IsActive = true;
                            item.Receiver.IsActive = true;
                            

                            _context.Orders.Add(item);
                            _context.SaveChanges();



                            item.PatientId = item.ReceiverId.GetValueOrDefault();
                            _context.SaveChanges();
                        }
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
              
               
                return false;
                
            }
            catch (Exception)
            {

               return false;
            }

        }



        public bool Update(Order model)
        {
            try
            {
                if (model != null)
                {
                    var item = GetById(model.Id);
                     model.DeliveredDate = null;

                    item.Customer_Reference = model.Customer_Reference;
                    item.PracticeId = model.PracticeId;
                    item.Receiver.Instructions = model.Receiver.Instructions;
                    item.Receiver.Name = model.Receiver.Name;
                    item.Receiver.Email = model.Receiver.Email;
                    item.Receiver.Phone = model.Receiver.Phone;
                    item.Receiver.Address_Line1 = model.Receiver.Address_Line1;
                    item.Receiver.Address_Line2 = model.Receiver.Address_Line2;
                    item.Receiver.Suburb = model.Receiver.Suburb;
                    item.Receiver.Postcode = model.Receiver.Postcode;
                    item.Receiver.State_Name = model.Receiver.State_Name;
                    item.Receiver.Country = model.Receiver.Country;
                    item.ModifiedBy = model.ModifiedBy;
                    item.ModifiedOn = DateTime.Now;
                    item.Receiver.ModifiedBy = model.Receiver.ModifiedBy;
                    item.Receiver.ModifiedOn = DateTime.Now;
                    _context.Entry(item).State = EntityState.Modified;
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

        public Order UpdateOrderStatus(int OrderId, int statusId)
        {
            try
            {
                var data = _context.Orders.Where(x => x.Id == OrderId).FirstOrDefault();
                if (data != null)
                {
                    data.OrderStatusId = statusId;
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

        public bool UpdateCustomerRefrenceSingle(string customerRefrence, int OrderId)
        {
            try
            {
                var data = _context.Orders.Where(x => x.Id == OrderId).FirstOrDefault();
                if (data != null)
                {
                    data.Customer_Reference = customerRefrence;
                    _context.SaveChanges();
                    return true;
                }
                return true;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Order UpdateCustomerRefrence(string customerRefrence, int OrderId)
        {
            try
            {
                var data = _context.Orders.Where(x => x.Id == OrderId).FirstOrDefault();
                if (data != null)
                {
                    data.Customer_Reference = customerRefrence;
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

        public IEnumerable<Order> GetAllTodayOrders()
        {
            var data = _context.Orders.Where(x=>x.CreatedOn.Value.Date == DateTime.Today)
                .Include(x => x.Product)
                 .Include(x => x.Receiver)
                .Include(x => x.Sender)
                .Include(x => x.OrderStatus)
                       .ToList();

            return data;

        }

        public IEnumerable<Order> GetOrdersByDate(DateTime dateTime)
        {
            var data = _context.Orders.Where(x => x.CreatedOn.Value.Date == dateTime.Date)
                .Include(x => x.Product)
                 .Include(x => x.Receiver)
                .Include(x => x.Sender)
                .Include(x => x.OrderStatus)
                       .ToList();

            return data;

        }

        
        public OrderLog OrderDetailPage(int Id)
        {
            var data = _context.OrderLogs.Where(x => x.OrderId == Id)
                .Include(x => x.Order.Product)
                .Include(x => x.Receiver)
                .Include(x => x.Order.Sender)
                .Include(x => x.DeviceType)
                .Include(x => x.Order.OrderStatus)
                .FirstOrDefault();
            return data;
        }

        //public IEnumerable<Order> GetAllProcessedOrders()
        //{
        //    var data = _context.Orders.Where(x => x.OrderStatusId == 2 && x.DeviceTypeId == 1)
        //        .Include(x => x.Product)
        //         .Include(x => x.Receiver)
        //        .Include(x => x.Sender)
        //        .Include(x => x.OrderStatus)
        //               .ToList();

        //    return data;

        //}

        public IEnumerable<Order> GetReplacementOrders()
        {
            var data = _context.Orders.OrderByDescending(x => x.Id).Where(x => x.DeviceTypeId == 8 && x.SenderId==1 && x.IsDeleted== false && (x.OrderStatusId == 6 || x.OrderStatusId == 2))
                .Include(x => x.Product)
                 .Include(x => x.Receiver)
                .Include(x => x.Sender)
                .Include(x => x.OrderStatus)
                       .ToList();

            return data;
        }

        public IEnumerable<Order> GetAllProcessedOrders()
        {

            var data = _context.Orders.OrderByDescending(x=>x.Id).Where(x => x.DeviceTypeId == 1 &&  x.IsDeleted==false && (x.OrderStatusId == 6 || x.OrderStatusId == 2))
            .Include(x => x.Product)
             .Include(x => x.Receiver)
            .Include(x => x.Sender)
            .Include(x => x.OrderStatus)
                   .ToList();
            return data;
        }

        public IEnumerable<Order> GetAllProcessedOrdersHangfire()
        {
            var data = _context.Orders.Where(x => x.OrderStatusId == 2)
                .Include(x => x.Product)
                 .Include(x => x.Receiver)
                .Include(x => x.Sender)
                .Include(x => x.OrderStatus)
                       .ToList();

            return data;

        }

        public IEnumerable<Order> GetAllReturns()
        {
            var data = _context.Orders.OrderByDescending(x => x.Id).Where(x => x.DeviceTypeId==6 && x.IsDeleted == false && (x.OrderStatusId == 6 || x.OrderStatusId == 2))
                        .Include(x => x.Product)
                        .Include(x => x.Receiver)
                        .Include(x => x.Sender)
                        .Include(x => x.OrderStatus)
                        .ToList().Union(
                            _context.Orders.OrderByDescending(x => x.Id).Where(x => x.DeviceTypeId == 8 && x.SenderId == null && x.IsDeleted == false && (x.OrderStatusId == 6 || x.OrderStatusId == 2))
                            .Include(x => x.Product)
                            .Include(x => x.Receiver)
                            .Include(x => x.Sender)
                            .Include(x => x.OrderStatus)
                            .ToList()
                        );
            return data;

        }

        public IEnumerable<Order> GetAllMiscellaneousOrders()
        {
            var data = _context.Orders.Where(x => x.OrderStatusId == 6 && x.Customer_Reference==null && x.IsDeleted == false)
                        .Include(x => x.Product)
                        .Include(x => x.Receiver)
                        .Include(x => x.Sender)
                        .Include(x => x.OrderStatus)
                        .ToList();
            return data;

        }

        public IEnumerable<Order> GetAllExpireOrders()
        {

            var WorkStart= new DateTime(2021, 2, 1);
            var today = DateTime.Today.AddDays(-30);
            string searchStr = "Parcel has been scheduled for pickup on";

            var data = _context.Orders
                .Where(x =>
                (DbFunctions.TruncateTime(x.CreatedOn.Value) <= today.Date && DbFunctions.TruncateTime(x.CreatedOn.Value) >= WorkStart.Date)
                && (x.OrderStatusId == 2)
                && (x.CurrentStatus.Replace(" ", "").ToLower().Contains(searchStr.Replace(" ","").ToLower()))
                )
                        .Include(x => x.Product)
                        .Include(x => x.Receiver)
                        .Include(x => x.Sender)
                        .Include(x => x.OrderStatus)
                        .ToList();
            return data;
        }

        public IEnumerable<Order> GetAllBulkPackaging()
        {
            var data = _context.Orders.Where(x => x.DeviceTypeId == 5 && x.IsDeleted == false)
                        .Include(x => x.Product)
                        .Include(x => x.Receiver)
                        .Include(x => x.Sender)
                        .Include(x => x.OrderStatus)
                        .ToList();
            return data;

        }


        public IEnumerable<Order> GetAllAccessories()
        {
            var data = _context.Orders.OrderByDescending(x => x.Id).Where(x => x.DeviceType.MainDeviceTypeId==2 && x.IsDeleted == false && (x.OrderStatusId == 6 || x.OrderStatusId == 2))
                        .Include(x => x.Product)
                        .Include(x => x.Receiver)
                        .Include(x => x.Sender)
                        .Include(x => x.OrderStatus)
                        .Include(x => x.DeviceType)
                        .Include(x=>x.DeviceType.MainDeviceType)
                        .ToList();
            return data;
        }
        public IEnumerable<Order> GetAllCanceledOrders()
        {
            var data = _context.Orders.OrderByDescending(x => x.Id).Where( x => x.IsDeleted == false && (x.OrderStatusId == 4 || x.OrderStatusId == 5))
                        .Include(x => x.Product)
                        .Include(x => x.Receiver)
                        .Include(x => x.Sender)
                        .Include(x => x.OrderStatus)
                        .ToList();
            return data;
        }

        public IEnumerable<Order> GetAllProcessedOrders(string Search)
        {
            var data = _context.Orders.Where(x=>(x.DeviceTypeId == 1 && (x.OrderStatusId == 6 || x.OrderStatusId == 2)) &&
                        (x.Receiver.Name.ToLower().Contains(Search.ToLower()) || x.Receiver.Address_Line2.ToLower().Contains(Search.ToLower())
                        || x.Customer_Reference.ToLower().Contains(Search.ToLower()) || x.PracticeId.ToLower().Contains(Search.ToLower())))
                .Include(x => x.Product)
                 .Include(x => x.Receiver)
                .Include(x => x.Sender)
                .Include(x => x.OrderStatus)
                       .ToList();

            return data;

        }

        public IEnumerable<Order> GetInprocessOrder()
        {
            var data = _context.Orders.Where(x => x.OrderStatusId == 1 || x.OrderStatusId == 3 )
                .Include(x => x.Product)
                 .Include(x => x.Receiver)
                .Include(x => x.Sender)
                .Include(x => x.OrderStatus)
                       .ToList();

            return data;

        }

        public IEnumerable<Order> GetInprocessOrder(string Search)
        {
            var data = _context.Orders.Where(x => (x.OrderStatusId == 1 || x.OrderStatusId == 3) &&
                        (x.Receiver.Name.ToLower().Contains(Search.ToLower()) || x.Receiver.Address_Line2.ToLower().Contains(Search.ToLower())
                        || x.Customer_Reference.ToLower().Contains(Search.ToLower()) || x.PracticeId.ToLower().Contains(Search.ToLower())))
                .Include(x => x.Product)
                 .Include(x => x.Receiver)
                .Include(x => x.Sender)
                .Include(x => x.OrderStatus)
                       .ToList();
            
            return data;
        }

        public IEnumerable<Order> GetPendingInprocessOrder()
        {
            var data = _context.Orders.Where(x => x.OrderStatusId == 1)
                .Include(x => x.Product)
                 .Include(x => x.Receiver)
                .Include(x => x.Sender)
                .Include(x => x.OrderStatus)
                       .ToList();

            return data;

        }

        public IEnumerable<Order> GetUnsuccessfullInprocessOrder()
        {
            var data = _context.Orders.Where(x => x.OrderStatusId == 3)
                .Include(x => x.Product)
                 .Include(x => x.Receiver)
                .Include(x => x.Sender)
                .Include(x => x.OrderStatus)
                       .ToList();

            return data;

        }

        public bool CheckExistForRechecking(string name, string address)
        {
            try
            {
                var data = _context.Receivers.Where(x => x.Name.Replace(" ", "").ToLower() == name.Replace(" ", "").ToLower() && x.Address_Line2.Replace(" ", "").ToLower() == address.Replace(" ", "").ToLower()).ToList();
                if (data.Count() != 0)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool GetDuplicateDate(string name, string address)
        {
            try
            {

                var data = _context.Receivers.Where(x => x.Name.Replace(" ", "").ToLower() == name.Replace(" ", "").ToLower() && x.Address_Line2.Replace(" ", "").ToLower() == address.Replace(" ", "").ToLower()).FirstOrDefault();

                if (data != null)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool CheckExist(string EHR)
        {
            try
            {
                var data = _context.Receivers.Where(x => x.EHR.Replace(" ", "").ToLower() == EHR.Replace(" ", "").ToLower()).ToList();
                if (data.Count() != 0)
                {
                    return true;
                }
                return false;

            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        // Code By : Kashif

        public Receiver CheckDuplicateWithEHR(string EHR)
        {
            try
            {
                var data = _context.Receivers.Where(x => x.EHR.Replace(" ", "").ToLower() == EHR.Replace(" ", "").ToLower()).FirstOrDefault();
                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public bool UpdateIsDelivered(int id, DateTime ddate)
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
                    entity.IsDelivered = true;
                    entity.DeliveredDate = ddate;
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
        public bool UpdateOrderStatusId(int id)
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
                    entity.OrderStatusId = 4;
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

        public IEnumerable<Order> DeliveredOrdersBetweenDate(DateTime startdate,DateTime enddate)
        {
            var data = _context.Orders.Where(x=>x.IsDelivered==true && x.DeliveredDate >= startdate.Date && x.DeliveredDate <=enddate )
                .Include(x => x.Product)
                 .Include(x => x.Receiver)
                .Include(x => x.Sender)
                .Include(x => x.OrderStatus)
                       .ToList();

            return data;

        }

        public IEnumerable<string> GetAllUniquePracticeId()
        {
            var data = _context.Orders.ToList().Where(x => x.PracticeId != null).Select(x => x.PracticeId).Distinct();
            return data;

        }

        public IEnumerable<Order> FilterProcessedOrders(Order model)
        {
            if (model.IsDelivered==true)
            {
                var data = _context.Orders.Where(x => x.OrderStatusId == 6 && x.DeviceTypeId==1)
            .Include(x => x.Product)
            .Include(x => x.Receiver)
            .Include(x => x.Sender)
            .Include(x => x.OrderStatus)
            .ToList();

                return data;
            }
            else
            {
                var data = _context.Orders.Where(x => (x.DeviceTypeId == 1 && (x.OrderStatusId == 6 || x.OrderStatusId == 2)) &&
                (x.PracticeId.Trim().ToLower().Contains(model.PracticeId.Trim().ToLower())
                || x.Receiver.Name.Trim().ToLower().Contains(model.Receiver.Name.Trim().ToLower())
                || x.Receiver.Address_Line2.Trim().ToLower().Contains(model.Receiver.Address_Line2.Trim().ToLower())
                || x.Customer_Reference.Trim().ToLower().Contains(model.Customer_Reference.Trim().ToLower())
                || DbFunctions.TruncateTime(x.CreatedOn) == model.CreatedOn
                ))
                .Include(x => x.Product)
                .Include(x => x.Receiver)
                .Include(x => x.Sender)
                .Include(x => x.OrderStatus)
                .ToList();

                return data;
            }
        }

        public IEnumerable<string> GetAllUniqueInProcessPracticeId()
        {
            var data = _context.Orders.ToList().Where(x => x.OrderStatusId == 1 || x.OrderStatusId== 3).Select(x => x.PracticeId).Distinct();
            return data;

        }

        public IEnumerable<Order> FilterInProcessOrders(Order model)
        {
            var data = _context.Orders.Where(x => (x.OrderStatusId == 1 || x.OrderStatusId == 3) &&
            (x.PracticeId.Trim().ToLower().Contains(model.PracticeId.Trim().ToLower())
            || x.Receiver.Name.Trim().ToLower().Contains(model.Receiver.Name.Trim().ToLower())
            || x.Receiver.Address_Line2.Trim().ToLower().Contains(model.Receiver.Address_Line2.Trim().ToLower())
            || x.Customer_Reference.Trim().ToLower().Contains(model.Customer_Reference.Trim().ToLower())))
            .Include(x => x.Product)
            .Include(x => x.Receiver)
            .Include(x => x.Sender)
            .Include(x => x.OrderStatus)
            .ToList();

            return data;

        }

        public IEnumerable<string> GetAllUniqueCanceledPracticeId()
        {
            var data = _context.Orders.ToList().Where(x => x.OrderStatusId == 4).Select(x => x.PracticeId).Distinct();
            return data;

        }

        public IEnumerable<Order> FilterCanceledOrders(Order model)
        {
            var data = _context.Orders.Where(x => (x.OrderStatusId == 4) &&
            (x.PracticeId.Trim().ToLower().Contains(model.PracticeId.Trim().ToLower())
            || x.Receiver.Name.Trim().ToLower().Contains(model.Receiver.Name.Trim().ToLower())
            || x.Receiver.Address_Line2.Trim().ToLower().Contains(model.Receiver.Address_Line2.Trim().ToLower())))
            .Include(x => x.Product)
            .Include(x => x.Receiver)
            .Include(x => x.Sender)
            .Include(x => x.OrderStatus)
            .ToList();

            return data;

        }

        public IEnumerable<string> GetAllUniqueDeliveredPracticeId()
        {
            var data = _context.Orders.ToList().Where(x => x.IsDelivered == true).Select(x => x.PracticeId).Distinct();
            return data;

        }

        public IEnumerable<Order> FilterDeliveredOrders(Order model)
        {
            var data = _context.Orders.Where(x => (x.IsDelivered == true) &&
            (x.PracticeId.Trim().ToLower().Contains(model.PracticeId.Trim().ToLower())
            || x.Receiver.Name.Trim().ToLower().Contains(model.Receiver.Name.Trim().ToLower())
            || x.Receiver.Address_Line2.Trim().ToLower().Contains(model.Receiver.Address_Line2.Trim().ToLower())))
            .Include(x => x.Product)
            .Include(x => x.Receiver)
            .Include(x => x.Sender)
            .Include(x => x.OrderStatus)
            .ToList();

            return data;

        }

        public IEnumerable<Order> DeliveredOrders()
        {
            var data=_context.Orders.Where(x=>x.IsDelivered==true).Include(x => x.Product)
            .Include(x => x.Receiver)
            .Include(x => x.Sender)
            .Include(x => x.OrderStatus)
            .ToList();
            return data;
        }

        public bool UpdateEHR(string Name,string Address,string EHR)
        {
            try
            {
                if (EHR!=null)
                {
                    var entity = _context.Receivers.Where(x=>x.Name.Replace(" ", "").ToLower()==Name.Replace(" ", "").ToLower()  && x.Address_Line2.Replace(" ", "").ToLower() == Address.Replace(" ", "").ToLower()).FirstOrDefault();
                    if (entity == null)
                    {
                        return false;
                    }
                    entity.EHR = EHR;
                    _context.Entry(entity).State = EntityState.Modified;
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IEnumerable<dynamic> DeliveredPatients()
        {
            var yesterday = DateTime.Today.AddDays(-1);
            var JoinResult = _context.Orders.Where(x => DbFunctions.TruncateTime(x.DeliveredDate) == yesterday).Join(_context.Receivers, o => o.ReceiverId, r => r.Id,
            (o, r) => new
            {
                Name = r.Name,
                Address = r.Address_Line2,
                Practice = o.PracticeId,
                Customer_Reference = o.Customer_Reference,
                Delivery_Status = o.DeliveredDate,
                Phone = r.Phone,
                EHR = r.EHR,
                Order_Status = o.CreatedOn
            });
            return JoinResult.ToList();
        }

        public IEnumerable<dynamic> GetDataByPracticeId(string PracticeId)
        {
            var JoinResult = _context.Orders.Where(x => x.PracticeId== PracticeId).Join(_context.Receivers, o => o.ReceiverId, r => r.Id,
            (o, r) => new
            {
                OI=o.Id,
                Name = r.Name,
                Address = r.Address_Line1,
                Practice = o.PracticeId,
                Customer_Reference = o.Customer_Reference,
                Delivery_Status = o.DeliveredDate,
                Phone = r.Phone,
                EHR = r.EHR,
                Order_Status = o.CreatedOn
            });
            return JoinResult.ToList();
        }

        public IEnumerable<Source> SourceDropdown()
        {
            return _context.Sources.ToList();
        }

        public IEnumerable<DeviceType> DeviceTypesDropdown()
        {
            return _context.DeviceTypes.ToList();
        }

        public IEnumerable<dynamic> GetDuplicatePatients()
        {
            var JoinResult = _context.Orders.Join(_context.Receivers, o => o.ReceiverId, r => r.Id,
           (o, r) => new
           {
               Name = r.Name,
               Address = r.Address_Line2,
               
           }).GroupBy(x => new { x.Name,x.Address }).Where(y => y.Count() > 1)
           .Select(x => new { Name = x.Key.Name, Counter = x.Count()});

            var data = JoinResult.ToList();
            return data;
        }

        public IEnumerable<dynamic> DuplicatePatientDetail(string name)
        {
            var JoinResult = _context.Orders.Join(_context.Receivers, o => o.ReceiverId, r => r.Id,
          (o, r) => new
          {
              Name = r.Name,
              DeliveredDate = o.DeliveredDate,
              Address = r.Address_Line2,
              Returned = o.DeviceTypeId,

          }).Where(r => r.Name == name).GroupBy(x => new { x.Name,x.DeliveredDate, x.Address, x.Returned })
          .Select(x => new { Name = x.Key.Name, Counter = x.Count(), Returned = x.Key.Returned, Address = x.Key.Address, DeliveredDate = x.Key.DeliveredDate });

            var data = JoinResult.ToList();
            return data;

        }

        public bool IsPrintedUpdate(int id)
        {
            var data = _context.Orders.Where(x => x.Id == id).FirstOrDefault();
            if (data == null)
            {
                return false;
            }
            else
            {
                data.IsPrinted = true;
                _context.SaveChanges();
                return true;
            }
        }

        

        public bool DeleteOrder(int id)
        {
            var data = _context.Orders.Where(x => x.Id == id).FirstOrDefault();
            if (data == null)
            {
                return false;
            }
            else
            {
                data.IsDeleted = true;
                _context.SaveChanges();
                return true;
            }
        }


        #region
        public IEnumerable<Order> FilterAccessoriesOrder(Order model)
        {
            if (model.IsDelivered == true)
            {
                var data = _context.Orders.Where(x => x.OrderStatusId == 6 && x.DeviceType.MainDeviceTypeId == 2 && x.IsDeleted == false)
            .Include(x => x.Product)
            .Include(x => x.Receiver)
            .Include(x => x.Sender)
            .Include(x => x.OrderStatus)
            .ToList();
                return data;
            }
            else
            {
                var data = _context.Orders.Where(x => (x.DeviceType.MainDeviceTypeId == 2 && x.IsDeleted == false && (x.OrderStatusId == 6 || x.OrderStatusId == 2)) &&
                (x.PracticeId.Trim().ToLower().Contains(model.PracticeId.Trim().ToLower())
                || x.Receiver.Name.Trim().ToLower().Contains(model.Receiver.Name.Trim().ToLower())
                || x.Receiver.Address_Line2.Trim().ToLower().Contains(model.Receiver.Address_Line2.Trim().ToLower())
                || x.Customer_Reference.Trim().ToLower().Contains(model.Customer_Reference.Trim().ToLower())
                || DbFunctions.TruncateTime(x.CreatedOn) == model.CreatedOn
                ))
                .Include(x => x.Product)
                .Include(x => x.Receiver)
                .Include(x => x.Sender)
                .Include(x => x.OrderStatus)
                .ToList();
                return data;
            }
        }

        public IEnumerable<Order> FilterReturnOrder(Order model)
        {
            if (model.IsDelivered==true)
            {
                var data = _context.Orders.Where(x =>x.DeviceTypeId == 6 && x.IsDeleted == false && x.OrderStatusId == 6)
                   .Include(x => x.Product)
                   .Include(x => x.Receiver)
                   .Include(x => x.Sender)
                   .Include(x => x.OrderStatus)
                   .ToList().Union(
                    _context.Orders.Where(x => x.DeviceTypeId == 8 && x.SenderId==null && x.IsDeleted == false && x.OrderStatusId == 6)
                   .Include(x => x.Product)
                   .Include(x => x.Receiver)
                   .Include(x => x.Sender)
                   .Include(x => x.OrderStatus)
                   .ToList()
                   );

                return data;
            }
            else
            {
                var data = _context.Orders.Where(x => (x.DeviceTypeId == 6 && x.IsDeleted == false && (x.OrderStatusId == 6 || x.OrderStatusId == 2)) &&
                    (x.PracticeId.Trim().ToLower().Contains(model.PracticeId.Trim().ToLower())
                    || x.Receiver.Name.Trim().ToLower().Contains(model.Receiver.Name.Trim().ToLower())
                    || x.Receiver.Address_Line2.Trim().ToLower().Contains(model.Receiver.Address_Line2.Trim().ToLower())
                    || x.Customer_Reference.Trim().ToLower().Contains(model.Customer_Reference.Trim().ToLower())
                    || DbFunctions.TruncateTime(x.CreatedOn) == model.CreatedOn
                    ))
                    .Include(x => x.Product)
                    .Include(x => x.Receiver)
                    .Include(x => x.Sender)
                    .Include(x => x.OrderStatus)
                    .ToList().Union(

                    _context.Orders.Where(x => (x.DeviceTypeId == 8 && x.SenderId==null && x.IsDeleted == false && (x.OrderStatusId == 6 || x.OrderStatusId == 2)) &&
                    (x.PracticeId.Trim().ToLower().Contains(model.PracticeId.Trim().ToLower())
                    || x.Receiver.Name.Trim().ToLower().Contains(model.Receiver.Name.Trim().ToLower())
                    || x.Receiver.Address_Line2.Trim().ToLower().Contains(model.Receiver.Address_Line2.Trim().ToLower())
                    || x.Customer_Reference.Trim().ToLower().Contains(model.Customer_Reference.Trim().ToLower())
                    || DbFunctions.TruncateTime(x.CreatedOn) == model.CreatedOn
                    ))
                    .Include(x => x.Product)
                    .Include(x => x.Receiver)
                    .Include(x => x.Sender)
                    .Include(x => x.OrderStatus)
                    .ToList()

                    );
                return data;
            }
        }

        public IEnumerable<Order> FilterReplacementOrder(Order model)
        {
            if (model.IsDelivered == true)
            {
                var data = _context.Orders.Where(x => x.DeviceTypeId == 8 && x.SenderId==1 && x.IsDeleted == false && x.OrderStatusId==6)
                    .Include(x => x.Product)
                    .Include(x => x.Receiver)
                    .Include(x => x.Sender)
                    .Include(x => x.OrderStatus)
                    .ToList();
                return data;
            }
            else
            {


                var data = _context.Orders.Where(x => (x.DeviceTypeId == 8 && x.SenderId == 1 && x.IsDeleted == false && (x.OrderStatusId == 6 || x.OrderStatusId == 2)) &&
                    (x.PracticeId.Trim().ToLower().Contains(model.PracticeId.Trim().ToLower())
                    || x.Receiver.Name.Trim().ToLower().Contains(model.Receiver.Name.Trim().ToLower())
                    || x.Receiver.Address_Line2.Trim().ToLower().Contains(model.Receiver.Address_Line2.Trim().ToLower())
                    || x.Customer_Reference.Trim().ToLower().Contains(model.Customer_Reference.Trim().ToLower())
                    || DbFunctions.TruncateTime(x.CreatedOn) == model.CreatedOn
                    ))
                    .Include(x => x.Product)
                    .Include(x => x.Receiver)
                    .Include(x => x.Sender)
                    .Include(x => x.OrderStatus)
                    .ToList();
                return data;
            }


        }

        public IEnumerable<Order> FilterCanceledOrder(Order model)
        {
            var data = _context.Orders.Where(x => x.IsDeleted == false 
               &&( x.OrderStatusId == 4 || x.OrderStatusId == 5)
               &&(x.PracticeId.Trim().ToLower().Contains(model.PracticeId.Trim().ToLower())
               || x.Receiver.Name.Trim().ToLower().Contains(model.Receiver.Name.Trim().ToLower())
               || x.Receiver.Address_Line2.Trim().ToLower().Contains(model.Receiver.Address_Line2.Trim().ToLower())
               || x.Customer_Reference.Trim().ToLower().Contains(model.Customer_Reference.Trim().ToLower())
               || DbFunctions.TruncateTime(x.CreatedOn) == model.CreatedOn
               ))
               .Include(x => x.Product)
               .Include(x => x.Receiver)
               .Include(x => x.Sender)
               .Include(x => x.OrderStatus)
               .ToList();
            return data;
        }
        #endregion

        public Order UpdateCurrentStatus(int OrderId, string currentStatus)
        {
            try
            {
                var data = _context.Orders.Where(x => x.Id == OrderId).FirstOrDefault();
                if (data != null)
                {
                    data.CurrentStatus = currentStatus;
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

        public Sender GetSender()
        {
            var data = _context.Senders.First();
            return data;
        }

        public Receiver AddReceiver(Receiver model)
        {
            var data = _context.Receivers.Add(model);
            _context.SaveChanges();
            return data;
        }
    }
}
