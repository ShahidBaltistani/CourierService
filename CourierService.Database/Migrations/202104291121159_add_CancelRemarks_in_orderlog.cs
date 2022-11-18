namespace CourierService.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_CancelRemarks_in_orderlog : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderLogs", "CancelRemarks", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.OrderLogs", "CancelRemarks");
        }
    }
}
