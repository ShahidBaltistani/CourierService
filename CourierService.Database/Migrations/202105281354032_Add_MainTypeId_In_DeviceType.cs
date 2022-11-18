namespace CourierService.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_MainTypeId_In_DeviceType : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.DeviceTypes", "MainTypeId", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.DeviceTypes", "MainTypeId");
        }
    }
}
