namespace CourierService.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_New_Table_MainDeviceType : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.MainDeviceTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.DeviceTypes", "MainDeviceTypeId", c => c.Int());
            CreateIndex("dbo.DeviceTypes", "MainDeviceTypeId");
            AddForeignKey("dbo.DeviceTypes", "MainDeviceTypeId", "dbo.MainDeviceTypes", "Id");
            DropColumn("dbo.DeviceTypes", "MainTypeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.DeviceTypes", "MainTypeId", c => c.Int(nullable: false));
            DropForeignKey("dbo.DeviceTypes", "MainDeviceTypeId", "dbo.MainDeviceTypes");
            DropIndex("dbo.DeviceTypes", new[] { "MainDeviceTypeId" });
            DropColumn("dbo.DeviceTypes", "MainDeviceTypeId");
            DropTable("dbo.MainDeviceTypes");
        }
    }
}
