using CourierService.Controllers;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.Owin;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

[assembly: OwinStartup(typeof(CourierService.App_Start.Startup))]
namespace CourierService.App_Start
{
    public class Startup
    {
        StreamVitalsController StreamVitals = new StreamVitalsController();


        [Obsolete]
        public void Configuration(IAppBuilder app)
        {

            //GlobalConfiguration.Configuration
            //   .UseSqlServerStorage(@"Server=34.73.221.148; Database=RPD_Courier_New; user=sa;password=360VUSolutions");
            GlobalConfiguration.Configuration
               .UseSqlServerStorage(@"Server=DESKTOP-U1PEDN3\SQLEXPRESS; Database=RPDCourier_Local; Integrated Security=true;");




            //app.UseHangfireDashboard();
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[] { new HangFireAuthorizationFilter() }
            });
            app.UseHangfireServer();
            HangFireService obj = new HangFireService();

            //RecurringJob.AddOrUpdate(() => obj.GetAllProcessedOrders(), "0,0 9,16 * * *", TimeZoneInfo.Local);
            //RecurringJob.AddOrUpdate(() => StreamVitals.DeviceAssignWithSingleObject(), "0 4 * * *", TimeZoneInfo.Local);
        }

        public class HangFireAuthorizationFilter : IDashboardAuthorizationFilter
        {
            public bool Authorize(DashboardContext context)
            {
                var owinContext = new OwinContext(context.GetOwinEnvironment());
                return owinContext.Authentication.User.Identity.IsAuthenticated;
            }
        }

    }
}