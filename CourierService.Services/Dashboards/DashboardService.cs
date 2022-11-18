using CourierService.Repositories.Dashboards;
using CourierService.ViewModels.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Services.Dashboards
{
   public class DashboardService : AutoMapperProfile
    {
        DashboardRepository _repo = new DashboardRepository();
    }
}
