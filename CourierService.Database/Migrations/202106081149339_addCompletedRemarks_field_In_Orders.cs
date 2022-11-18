namespace CourierService.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addCompletedRemarks_field_In_Orders : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "CompletedRemarks", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "CompletedRemarks");
        }
    }
}
