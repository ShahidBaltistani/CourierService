using CourierService.ViewModels.NewPracticeF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.ViewModels.Orders
{
    public class OrderViewModel : BaseEntityViewModel
    {
        public string Pickup_Date { get; set; }
        public string First_Mile_Option { get; set; }
        public string Description { get; set; }
        public int? ProductId { get; set; }
        public ProductViewModel Product { get; set; }
        public string Customer_Reference { get; set; }
        public int? SenderId { get; set; }
        public SenderViewModel Sender { get; set; }
        public int? ReceiverId { get; set; }
        public virtual ReceiverViewModel Receiver { get; set; }
        public int? OrderStatusId { get; set; }
        public OrderStatusViewModel OrderStatus { get; set; }
        public bool IsExist { get; set; } = false;
        public string PracticeId { get; set; }
        public string DeliveryStatus { get; set; }
        public bool IsDelivered { get; set; }
        public DateTime DeliveredDate { get; set; }
        public int? SourceId { get; set; }
        public SourceViewModel Source { get; set; }
        public int? DeviceTypeId { get; set; }
        public DeviceTypeViewModel DeviceType { get; set; }
        public int PatientId { get; set; }
        public bool IsPrinted { get; set; }
        public string CurrentStatus { get; set; }

        // For Practice
        public int? NewPracticeId { get; set; }
        public NewPracticeViewModel NewPractice { get; set; }

        // For Integration
        public bool IsCompleted { get; set; }
        public bool IntegrationStatus { get; set; }
        public string IntegrationRemarks { get; set; }
        // Order Address
        public string Address { get; set; }
    }
}
