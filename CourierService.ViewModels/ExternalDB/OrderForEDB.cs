using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.ViewModels.ExternalDB
{
    public class OrderForEDB
    {
        public int Id { get; set; }
        public string Customer_Reference { get; set; }
        public int? ReceiverId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string PracticeId { get; set; }
        public bool IsDelivered { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public string EHR { get; set; }
        public int? SourceId { get; set; }
        public int? DeviceTypeId { get; set; }
        
    }

    public class OrderForSVPKDB
    {
        public int CourierOrderId { get; set; }
        public string Customer_Reference { get; set; }
        public int? ReceiverId { get; set; }
        public DateTime? CreatedOn { get; set; }
        public string PracticeId { get; set; }
        public bool IsDelivered { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public string EHR { get; set; }
        public int SourceId { get; set; }
        public int DeviceTypeId { get; set; }
    }

    // New
    public class OrderForSVPKDB_New
    {
        public int CourierOrderId { get; set; }
        public string Customer_Reference { get; set; }
        public DateTime? Order_Created_Date { get; set; }
        public string Practice { get; set; }
        public DateTime? DeliveredDate { get; set; }
        public string EHR { get; set; }
    }
}
