namespace CourierService.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class bulkpackages : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.BulkPackages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderDate = c.DateTime(),
                        CustomerReference = c.String(),
                        ShortSerial = c.String(),
                        Practice = c.String(),
                        SenderId = c.Int(),
                        ReceiverId = c.Int(),
                        OrderStatusId = c.Int(),
                        ProductId = c.Int(),
                        SourceId = c.Int(),
                        DeviceTypeId = c.Int(),
                        CreatedOn = c.DateTime(),
                        CreatedBy = c.Int(),
                        ModifiedOn = c.DateTime(),
                        ModifiedBy = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DeviceTypes", t => t.DeviceTypeId)
                .ForeignKey("dbo.OrderStatus", t => t.OrderStatusId)
                .ForeignKey("dbo.Products", t => t.ProductId)
                .ForeignKey("dbo.Receivers", t => t.ReceiverId)
                .ForeignKey("dbo.Senders", t => t.SenderId)
                .ForeignKey("dbo.Sources", t => t.SourceId)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId)
                .Index(t => t.OrderStatusId)
                .Index(t => t.ProductId)
                .Index(t => t.SourceId)
                .Index(t => t.DeviceTypeId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.BulkPackages", "SourceId", "dbo.Sources");
            DropForeignKey("dbo.BulkPackages", "SenderId", "dbo.Senders");
            DropForeignKey("dbo.BulkPackages", "ReceiverId", "dbo.Receivers");
            DropForeignKey("dbo.BulkPackages", "ProductId", "dbo.Products");
            DropForeignKey("dbo.BulkPackages", "OrderStatusId", "dbo.OrderStatus");
            DropForeignKey("dbo.BulkPackages", "DeviceTypeId", "dbo.DeviceTypes");
            DropIndex("dbo.BulkPackages", new[] { "DeviceTypeId" });
            DropIndex("dbo.BulkPackages", new[] { "SourceId" });
            DropIndex("dbo.BulkPackages", new[] { "ProductId" });
            DropIndex("dbo.BulkPackages", new[] { "OrderStatusId" });
            DropIndex("dbo.BulkPackages", new[] { "ReceiverId" });
            DropIndex("dbo.BulkPackages", new[] { "SenderId" });
            DropTable("dbo.BulkPackages");
        }
    }
}
