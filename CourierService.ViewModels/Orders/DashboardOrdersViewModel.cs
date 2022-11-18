﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.ViewModels.Orders
{
    public class DashboardOrdersViewModel
    {
        public int Today { get; set; }
        public int Delivered { get; set; }
        public int PendingSerialAssigning { get; set; }
        public int PendingTask { get; set; }
        public int InTransit { get; set; }
    }
}
