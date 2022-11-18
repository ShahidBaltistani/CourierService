using CourierService.Entities;
using CourierService.Entities.Accounts;
using CourierService.Entities.Bulk;
using CourierService.Entities.NewPracticeF;
using CourierService.Entities.Orders;
using CourierService.Entities.TaskManagements;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourierService.Database
{
    public class CourierServiceContext : DbContext
    {
        public CourierServiceContext() : base("con")
        {
        }

        public DbSet<Order> Orders { get; set; }
        public DbSet<Sender> Senders { get; set; }
        public DbSet<Receiver> Receivers { get; set; }
        public DbSet<OrderStatus> OrderStatus { get; set; }
        public DbSet<APIResponse> APIResponses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<OrderLabel> OrderLabels { get; set; }
        public DbSet<Source> Sources { get; set; }
        public DbSet<DeviceType> DeviceTypes { get; set; }
        public DbSet<OrderLog> OrderLogs { get; set; }
        public DbSet<BulkPackage> BulkPackages { get; set; }
        public DbSet<NewPractice> NewPractices { get; set; }


        //Accounts
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        //Task
        public DbSet<TaskMail> TaskMails { get; set; }
        public DbSet<TaskDetail> TaskDetails { get; set; }
        //public DbSet<Practice> Practices { get; set; }
        public DbSet<TaskLog> TaskLogs { get; set; }
        public DbSet<MainDeviceType> MainDeviceTypes { get; set; }
    }
}
