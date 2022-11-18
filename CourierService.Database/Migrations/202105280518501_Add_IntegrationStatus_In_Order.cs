namespace CourierService.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_IntegrationStatus_In_Order : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "IntegrationStatus", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "IntegrationStatus");
        }
    }
}
