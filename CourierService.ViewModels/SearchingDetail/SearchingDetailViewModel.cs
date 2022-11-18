using CourierService.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.ViewModels.SearchingDetail
{
    public class SearchingDetailViewModel
    {
        public OrderViewModel OrderViewModel { get; set; }
        public OrderTrackingViewModel OrderTrackingViewModel { get; set; }
    }
}
