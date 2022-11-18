namespace CourierService.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_BulkPackage_Entity : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "IsPrinted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "IsPrinted");
        }
    }
}
