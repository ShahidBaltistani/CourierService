using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.ViewModels.Orders
{
    public class OrderTrackingViewModel
    {
        public string State { get; set; }

        public IEnumerable<TrackingEventsViewModel> Tracking_Events { get; set; }
       
       

        public OrderTrackingStatus Status { get; set; }

        public class Destination
        {
            public string Country { get; set; }
        }
        public class OrderTrackingStatus
        {
            public string description { get; set; }
            public string last_changed_at { get; set; }
        }
        public class TrackingEventsViewModel
        {
            public string Event_Type { get; set; }
            public string Scan_Time { get; set; }
            public string Description { get; set; }
            public string Origin_Location { get; set; }
            public string Destination_Location { get; set; }
            public string Requester { get; set; }
            public string Location { get; set; }
        }
        public class Origin
        {
            public string Country { get; set; }
        }
    }


    
}
