using CourierService.Entities.NewPracticeF;
using CourierService.Entities.TaskManagements;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Entities.Orders
{
    public class Order : BaseEntity
    {

        public string Pickup_Date { get; set; }
        public string First_Mile_Option { get; set; }
        public string Description { get; set; }

        // For Product
        public int? ProductId { get; set; }
        public Product Product { get; set; }
        public string Customer_Reference { get; set; }

        // For Sender
        public int? SenderId { get; set; }
        public Sender Sender { get; set; }

        // For Receiver
        public int? ReceiverId { get; set; }
        public Receiver Receiver { get; set; }

        // For OrderStatus
        public int? OrderStatusId { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public string PracticeId { get; set; }
        public bool IsDelivered { get; set; }
        public DateTime? DeliveredDate { get; set; }

        // For Source
        public int? SourceId { get; set; }
        public Source Source { get; set; }

        // For DeviceType
        public int? DeviceTypeId { get; set; }
        public DeviceType DeviceType { get; set; }
        public int PatientId { get; set; }
        public bool IsPrinted { get; set; }
        public string CurrentStatus { get; set; }


        // For Practice
        public int? NewPracticeId { get; set; }
        public NewPractice NewPractice { get; set; }

        // For Integration
        public bool IsCompleted { get; set; }
        public bool IntegrationStatus { get; set; }
        public string IntegrationRemarks { get; set; }

        // Order Address
        public string Address { get; set; }
        public string CompletedRemarks { get; set; }

    }
}
