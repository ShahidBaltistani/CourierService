using AutoMapper;
using CourierService.Entities;
using CourierService.Entities.Accounts;
using CourierService.Entities.Bulk;
using CourierService.Entities.DTOForTaskManagements;
using CourierService.Entities.NewPracticeF;
using CourierService.Entities.Orders;
using CourierService.Entities.TaskManagements;
using CourierService.ViewModels;
using CourierService.ViewModels.Accounts;
using CourierService.ViewModels.Bulk;
using CourierService.ViewModels.DTOForTaskManagements;
using CourierService.ViewModels.NewPracticeF;
using CourierService.ViewModels.Orders;
using CourierService.ViewModels.TaskManagements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Services
{
   public class AutoMapperProfile 
    {
        protected readonly IMapper _mapper;
        protected AutoMapperProfile()
        {
            var config = new MapperConfiguration(x =>
            {
                x.CreateMap<Order, OrderViewModel>();
                x.CreateMap<OrderViewModel, Order>();

                x.CreateMap<Sender, SenderViewModel>();
                x.CreateMap<SenderViewModel, Sender>();

                x.CreateMap<Receiver, ReceiverViewModel>();
                x.CreateMap<ReceiverViewModel, Receiver>();

                x.CreateMap<Product, ProductViewModel>();
                x.CreateMap<ProductViewModel, Product>();


                x.CreateMap<APIResponse, APIResponseViewModel>();
                x.CreateMap<APIResponseViewModel, APIResponse>();

                x.CreateMap<OrderStatus, OrderStatusViewModel>();
                x.CreateMap<OrderStatusViewModel, OrderStatus>();

                x.CreateMap<OrderLabel, OrderLabelViewModel>();
                x.CreateMap<OrderLabelViewModel, OrderLabel>();

                //Accounts

                x.CreateMap<User, UserViewModel>();
                x.CreateMap<UserViewModel, User>();

                x.CreateMap<Role, RoleViewModel>();
                x.CreateMap<RoleViewModel, Role>();

                x.CreateMap<Source, SourceViewModel>();
                x.CreateMap<SourceViewModel, Source>();

                x.CreateMap<DeviceType, DeviceTypeViewModel>();
                x.CreateMap<DeviceTypeViewModel, DeviceType>();

                x.CreateMap<OrderLog, OrderLogViewModel>();
                x.CreateMap<OrderLogViewModel, OrderLog>();

                x.CreateMap<DashboardOrders, DashboardOrdersViewModel>();
                x.CreateMap<DashboardOrdersViewModel, DashboardOrders>();

                x.CreateMap<BulkPackage, BulkPackageViewModel>();
                x.CreateMap<BulkPackageViewModel, BulkPackage>();


                //Task
                x.CreateMap<TaskDetail, TaskDetailViewModel>();
                x.CreateMap<TaskDetailViewModel, TaskDetail>();

                x.CreateMap<TaskMail, TaskMailViewModel>();
                x.CreateMap<TaskMailViewModel, TaskMail>();

                //x.CreateMap<Practice, PracticeViewModel>();
                //x.CreateMap<PracticeViewModel, Practice>();

                x.CreateMap<NewPractice, NewPracticeViewModel>();
                x.CreateMap<NewPracticeViewModel, NewPractice>();


                // Task DTO
                x.CreateMap<TaskDetailDTO, TaskDetailDTOViewModel>();
                x.CreateMap<TaskDetailDTOViewModel, TaskDetailDTO>();

                x.CreateMap<TaskMailDTO, TaskMailDTOViewModel>();
                x.CreateMap<TaskMailDTOViewModel, TaskMailDTO>();

                x.CreateMap<MainDeviceType, MainDeviceTypeViewModel>();
                x.CreateMap<MainDeviceTypeViewModel, MainDeviceType>();
            });

            _mapper = config.CreateMapper();
        }
    }
}
