using CourierService.Repositories.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CourierService.ViewModels.Orders;
using CourierService.Entities.Orders;
using CourierService.ViewModels.Filters;
using CourierService.ViewModels.TaskManagements;

namespace CourierService.Services.Orders
{
   public class OrderService: AutoMapperProfile
    {
        OrderRepository _repo = new OrderRepository();

        public IEnumerable<OrderViewModel> DashboardDataByPractice(string Practice)
        {
            var data = _repo.DashboardDataByPractice(Practice);
            var res = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return res;
        }

        public DashboardOrdersViewModel Dashboard()
        {
            var data = _repo.Dashboard();
            var res = _mapper.Map<DashboardOrdersViewModel>(data);
            return res;
        }
        public IEnumerable<OrderViewModel> GetAllTodayCreatedOrders()
        {
            var data = _repo.GetAllTodayCreatedOrders();
            var res = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return res;
        }
        public List<OrderViewModel> PendingSerialDetails()
        {
            var data = _repo.PendingSerialDetails();
            var res = _mapper.Map<List<OrderViewModel>>(data);
            return res;
        }
        public List<TaskDetailViewModel> PendingTaskDetails()
        {
            var data = _repo.PendingTaskDetails();
            var res = _mapper.Map<List<TaskDetailViewModel>>(data);
            return res;
        }
        public List<OrderViewModel> InTransitDetails()
        {
            var data = _repo.InTransitDetails();
            var res = _mapper.Map<List<OrderViewModel>>(data);
            return res;
        }

        public IEnumerable<OrderViewModel> GetAllYesterdayDeliveredOrders()
        {
            var data = _repo.GetAllYesterdayDeliveredOrders();
            var res = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return res;
        }

        public IEnumerable<OrderLog> CheckHistory(int ReceiverId)
        {
            var data = _repo.CheckHistory(ReceiverId);
            return data;
        }

        public IEnumerable<OrderLogViewModel> SearchLikeSendle(string Search)
        {
            var data = _repo.SearchLikeSendle(Search);
            var res = _mapper.Map<IEnumerable<OrderLogViewModel>>(data);
            return res;
        }
        //public IEnumerable<OrderLogViewModel> SearchLikeSendle(string Search, int page, int pageSize, out int totalPage, out int totalRecord)
        //{
        //    var data = _repo.SearchLikeSendle(Search, page, pageSize, out totalPage, out totalRecord);
        //    var res = _mapper.Map<IEnumerable<OrderLogViewModel>>(data);
        //    return res;
        //}
        public OrderViewModel PatientDetail(int Id)
        {
            var data = _repo.PatientDetail(Id);
            var res = _mapper.Map<OrderViewModel>(data);
            return res;
        }

        public Order AddSingleOrder(OrderViewModel model)
        {
            var res = _mapper.Map<Order>(model);
            var data = _repo.AddSingleOrder(res);
            return data;
        }

        public Order AddSingleReturnOrder(OrderViewModel model)
        {
            var res = _mapper.Map<Order>(model);
            var data = _repo.AddSingleReturnOrder(res);
            return data;
        }
        public IEnumerable<OrderViewModel> GetAll()
        {
            var data = _repo.GetAll();
            var res = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return res;
        }
        public OrderViewModel GetById(int id)
        {
            var data = _repo.GetById(id);
            var res = _mapper.Map<OrderViewModel>(data);
            return res;
        }

        public bool Add(List<OrderViewModel> model)
        {
            var res = _mapper.Map<List<Order>>(model);
           var data = _repo.Add(res);
            return data;
        }

        public bool Update(OrderViewModel model)
        {
            var res = _mapper.Map<Order>(model);
            var data = _repo.Update(res);
            return data;
        }

        public OrderViewModel UpdateOrderStatus(int orderId, int statusId)
        {
            var data = _repo.UpdateOrderStatus(orderId, statusId);
            var res = _mapper.Map<OrderViewModel>(data);
            return res;
        }

        public bool UpdateCustomerRefrenceSingle(string customerRefrence, int orderId)
        {
            var data = _repo.UpdateCustomerRefrenceSingle(customerRefrence, orderId);
            return data;
        }

        public OrderViewModel UpdateCustomerRefrence(string customerRefrence, int orderId)
        {
            var data = _repo.UpdateCustomerRefrence(customerRefrence, orderId);
            var res = _mapper.Map<OrderViewModel>(data);
            return res;
        }

        public IEnumerable<OrderViewModel> GetAllTodayOrders()
        {
            var data = _repo.GetAllTodayOrders();
            var res = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return res;

        }

        public IEnumerable<OrderViewModel> GetOrdersByDate(DateTime dateTime)
        {
            var data = _repo.GetOrdersByDate(dateTime);
            var res = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return res;

        }

        public IEnumerable<OrderViewModel> GetAllMiscellaneousOrders()
        {
            var data = _repo.GetAllMiscellaneousOrders();
            var res = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return res;

        }

        public IEnumerable<OrderViewModel> GetAllExpireOrders()
        {
            var data = _repo.GetAllExpireOrders();
            var res = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return res;

        }

        public IEnumerable<OrderViewModel> GetAllProcessedOrders()
        {
            var data = _repo.GetAllProcessedOrders();
            var res = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return res;

        }

        public IEnumerable<OrderViewModel> GetAllAccessories()
        {
            var data = _repo.GetAllAccessories();
            var res = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return res;

        }

        public IEnumerable<OrderViewModel> GetAllBulkPackaging()
        {
            var data = _repo.GetAllBulkPackaging();
            var res = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return res;

        }

        public IEnumerable<OrderViewModel> GetAllReturns()
        {
            var data = _repo.GetAllReturns();
            var res = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return res;

        }

        public IEnumerable<OrderViewModel> GetAllCanceledOrders()
        {
            var data = _repo.GetAllCanceledOrders();
            var res = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return res;
        }


        public OrderLogViewModel OrderDetailPage(int Id)
        {
            var data = _repo.OrderDetailPage(Id);
            var res = _mapper.Map<OrderLogViewModel>(data);
            return res;
        }

        public IEnumerable<OrderViewModel> GetAllProcessedOrders(string Search)
        {
            var data = _repo.GetAllProcessedOrders(Search);
            var res = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return res;

        }

        public IEnumerable<OrderViewModel> GetInprocessOrder()
        {
            var data = _repo.GetInprocessOrder();
            var res = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return res;

        }

        public IEnumerable<OrderViewModel> GetInprocessOrder(string Search)
        {
            var data = _repo.GetInprocessOrder(Search);
            var res = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return res;

        }

        public List<OrderViewModel> GetInprocessOrderforAllSelected()
        {
            var data = _repo.GetInprocessOrder();
            var res = _mapper.Map<List<OrderViewModel>>(data);
            return res;

        }
        public List<OrderViewModel> GetPendingInprocessOrder()
        {
            var data = _repo.GetPendingInprocessOrder();
            var res = _mapper.Map<List<OrderViewModel>>(data);
            return res;

        }

        public List<OrderViewModel> GetUnsuccessfullInprocessOrder()
        {
            var data = _repo.GetUnsuccessfullInprocessOrder();
            var res = _mapper.Map<List<OrderViewModel>>(data);
            return res;

        }

        public bool CheckExistForRechecking(string name, string address)
        {
            var data = _repo.CheckExistForRechecking(name,address);
            return data;

        }



        public bool CheckExist(string EHR)
        {
            var data = _repo.CheckExist(EHR);
            return data;

        }
        public bool GetDuplicateDate(string name, string address)
        {
            var data = _repo.GetDuplicateDate(name, address);
            return data;

        }




        // Code By : Kashif

        public ReceiverViewModel CheckDuplicateWithEHR(string EHR)
        {
            var data = _repo.CheckDuplicateWithEHR(EHR);
            var res = _mapper.Map<ReceiverViewModel>(data);
            return res;

        }

        public List<OrderViewModel> DeliveredOrders()
        {
            var data = _repo.DeliveredOrders();
            var res = _mapper.Map<List<OrderViewModel>>(data);
            return res;
        }
        public bool UpdateIsDelivered(int id, DateTime ddate)
        {
            var data = _repo.UpdateIsDelivered(id, ddate);
            return data;
        }

        public bool UpdateOrderStatusId(int id)
        {
            var data = _repo.UpdateOrderStatusId(id);
            return data;
        }
        public IEnumerable<OrderViewModel> DeliveredOrdersBetweenDate(DateTime startdate, DateTime enddate)
        {
            var data = _repo.DeliveredOrdersBetweenDate(startdate, enddate);
            var res = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return res;
        }

        public IEnumerable<string> GetAllUniquePracticeId()
        {
            var data = _repo.GetAllUniquePracticeId();
            return data;
        }

        public IEnumerable<OrderViewModel> FilterProcessedOrders(OrderViewModel model)
        {
            var res = _mapper.Map<Order>(model);
            var data = _repo.FilterProcessedOrders(res);
            var list = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return list;
        }

        public IEnumerable<string> GetAllUniqueInProcessPracticeId()
        {
            var data = _repo.GetAllUniqueInProcessPracticeId();
            return data;
        }

        public IEnumerable<OrderViewModel> FilterInProcessOrders(OrderViewModel model)
        {
            var res = _mapper.Map<Order>(model);
            var data = _repo.FilterInProcessOrders(res);
            var list = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return list;
        }

        public IEnumerable<string> GetAllUniqueCanceledPracticeId()
        {
            var data = _repo.GetAllUniqueCanceledPracticeId();
            return data;
        }

        public IEnumerable<OrderViewModel> FilterCanceledOrders(OrderViewModel model)
        {
            var res = _mapper.Map<Order>(model);
            var data = _repo.FilterCanceledOrders(res);
            var list = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return list;
        }

        public IEnumerable<string> GetAllUniqueDeliveredPracticeId()
        {
            var data = _repo.GetAllUniqueDeliveredPracticeId();
            return data;
        }

        public IEnumerable<OrderViewModel> FilterDeliveredOrders(OrderViewModel model)
        {
            var res = _mapper.Map<Order>(model);
            var data = _repo.FilterDeliveredOrders(res);
            var list = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return list;
        }

        public bool UpdateEHR(string Name,string Address, string EHR)
        {
            var data = _repo.UpdateEHR(Name, Address, EHR);
            return data;
        }

        public IEnumerable<dynamic> DeliveredPatients()
        {
            var data = _repo.DeliveredPatients();
            return data;
        }

        public IEnumerable<dynamic> GetDataByPracticeId(string PracticeId)
        {
            var data = _repo.GetDataByPracticeId(PracticeId);
            return data;
        }

        public IEnumerable<Source> SourceDropdown()
        {
            return _repo.SourceDropdown();
        }

        public IEnumerable<DeviceType> DeviceTypesDropdown()
        {
            return _repo.DeviceTypesDropdown();
        }

        public bool UpdateIsPrinted(int id)
        {
            var data = _repo.IsPrintedUpdate(id);
            return data;
        }

        public IEnumerable<dynamic> GetDuplicatePatients()
        {
            var duplicate = _repo.GetDuplicatePatients();
            return duplicate;
        }
        public IEnumerable<dynamic> DuplicatePatientDetail(string name)
        {
            var duplicate = _repo.DuplicatePatientDetail(name);
            return duplicate;
        }
        public IEnumerable<OrderViewModel> GetReplacementOrders()
        {
            var data = _repo.GetReplacementOrders();
            var res = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return res;
        }

        public IEnumerable<OrderViewModel> GetAllProcessedOrdersHangfire()
        {
            var data = _repo.GetAllProcessedOrdersHangfire();
            var res = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return res;
        }
        public bool DeleteOrder(int id)
        {
            var data = _repo.DeleteOrder(id);
            return data;
        }


        #region

        public IEnumerable<OrderViewModel> FilterAccessoriesOrder(OrderViewModel model)
        {
            var res = _mapper.Map<Order>(model);
            var data = _repo.FilterAccessoriesOrder(res);
            var list = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return list;
        }
        public IEnumerable<OrderViewModel> FilterReturnOrder(OrderViewModel model)
        {
            var res = _mapper.Map<Order>(model);
            var data = _repo.FilterReturnOrder(res);
            var list = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return list;
        }
        public IEnumerable<OrderViewModel> FilterReplacementOrder(OrderViewModel model)
        {
            var res = _mapper.Map<Order>(model);
            var data = _repo.FilterReplacementOrder(res);
            var list = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return list;
        }
        public IEnumerable<OrderViewModel> FilterCanceledOrder(OrderViewModel model)
        {
            var res = _mapper.Map<Order>(model);
            var data = _repo.FilterCanceledOrder(res);
            var list = _mapper.Map<IEnumerable<OrderViewModel>>(data);
            return list;
        }




        #endregion


        public OrderViewModel UpdateCurrentStatus(int orderId, string currentStatus)
        {
            var data = _repo.UpdateCurrentStatus(orderId, currentStatus);
            var res = _mapper.Map<OrderViewModel>(data);
            return res;
        }

        public SenderViewModel GetSender()
        {
            var data = _repo.GetSender();
            var res = _mapper.Map<SenderViewModel>(data);
            return res;
        }

        public ReceiverViewModel AddReceiver(ReceiverViewModel model)
        {
            var res = _mapper.Map<Receiver>(model);
            var data = _repo.AddReceiver(res);
            var data2 = _mapper.Map<ReceiverViewModel>(data);
            return data2;
        }


    }
}
