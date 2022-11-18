namespace CourierService.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class initial_new : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.APIResponses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(),
                        ResponseText = c.String(),
                        SendleOrderId = c.String(),
                        SendleOrderUrl = c.String(),
                        SendleRefrence = c.String(),
                        SendleTrackingUrl = c.String(),
                        CreatedOn = c.DateTime(),
                        CreatedBy = c.Int(),
                        ModifiedOn = c.DateTime(),
                        ModifiedBy = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.Orders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Pickup_Date = c.String(),
                        First_Mile_Option = c.String(),
                        Description = c.String(),
                        ProductId = c.Int(),
                        Customer_Reference = c.String(),
                        SenderId = c.Int(),
                        ReceiverId = c.Int(),
                        OrderStatusId = c.Int(),
                        PracticeId = c.String(),
                        IsDelivered = c.Boolean(nullable: false),
                        DeliveredDate = c.DateTime(),
                        SourceId = c.Int(),
                        DeviceTypeId = c.Int(),
                        PatientId = c.Int(nullable: false),
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
                .Index(t => t.ProductId)
                .Index(t => t.SenderId)
                .Index(t => t.ReceiverId)
                .Index(t => t.OrderStatusId)
                .Index(t => t.SourceId)
                .Index(t => t.DeviceTypeId);
            
            CreateTable(
                "dbo.DeviceTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderStatus",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Products",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        VolumeValue = c.String(),
                        VolumeUnits = c.String(),
                        WeightValue = c.String(),
                        WeightUnits = c.String(),
                        Height = c.String(),
                        Width = c.String(),
                        Length = c.String(),
                        CreatedOn = c.DateTime(),
                        CreatedBy = c.Int(),
                        ModifiedOn = c.DateTime(),
                        ModifiedBy = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Receivers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Instructions = c.String(),
                        Name = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        Company = c.String(),
                        Address_Line1 = c.String(),
                        Address_Line2 = c.String(),
                        Suburb = c.String(),
                        Postcode = c.String(),
                        State_Name = c.String(),
                        Country = c.String(),
                        ReferenceId = c.String(),
                        EHR = c.String(),
                        CreatedOn = c.DateTime(),
                        CreatedBy = c.Int(),
                        ModifiedOn = c.DateTime(),
                        ModifiedBy = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Senders",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Instructions = c.String(),
                        Name = c.String(),
                        Email = c.String(),
                        Phone = c.String(),
                        Company = c.String(),
                        Address_Line1 = c.String(),
                        Address_Line2 = c.String(),
                        Suburb = c.String(),
                        Postcode = c.String(),
                        State_Name = c.String(),
                        Country = c.String(),
                        CreatedOn = c.DateTime(),
                        CreatedBy = c.Int(),
                        ModifiedOn = c.DateTime(),
                        ModifiedBy = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Sources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.OrderLabels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(),
                        Formate = c.String(),
                        Size = c.String(),
                        Url = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .Index(t => t.OrderId);
            
            CreateTable(
                "dbo.OrderLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        OrderId = c.Int(),
                        ReceiverId = c.Int(),
                        DeviceTypeId = c.Int(),
                        ReferenceOrderId = c.Int(),
                        SendleRefrence = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.DeviceTypes", t => t.DeviceTypeId)
                .ForeignKey("dbo.Orders", t => t.OrderId)
                .ForeignKey("dbo.Receivers", t => t.ReceiverId)
                .Index(t => t.OrderId)
                .Index(t => t.ReceiverId)
                .Index(t => t.DeviceTypeId);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        RoleId = c.Int(nullable: false),
                        CreatedOn = c.DateTime(),
                        CreatedBy = c.Int(),
                        ModifiedOn = c.DateTime(),
                        ModifiedBy = c.Int(),
                        IsDeleted = c.Boolean(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Roles", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "RoleId", "dbo.Roles");
            DropForeignKey("dbo.OrderLogs", "ReceiverId", "dbo.Receivers");
            DropForeignKey("dbo.OrderLogs", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.OrderLogs", "DeviceTypeId", "dbo.DeviceTypes");
            DropForeignKey("dbo.OrderLabels", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.APIResponses", "OrderId", "dbo.Orders");
            DropForeignKey("dbo.Orders", "SourceId", "dbo.Sources");
            DropForeignKey("dbo.Orders", "SenderId", "dbo.Senders");
            DropForeignKey("dbo.Orders", "ReceiverId", "dbo.Receivers");
            DropForeignKey("dbo.Orders", "ProductId", "dbo.Products");
            DropForeignKey("dbo.Orders", "OrderStatusId", "dbo.OrderStatus");
            DropForeignKey("dbo.Orders", "DeviceTypeId", "dbo.DeviceTypes");
            DropIndex("dbo.Users", new[] { "RoleId" });
            DropIndex("dbo.OrderLogs", new[] { "DeviceTypeId" });
            DropIndex("dbo.OrderLogs", new[] { "ReceiverId" });
            DropIndex("dbo.OrderLogs", new[] { "OrderId" });
            DropIndex("dbo.OrderLabels", new[] { "OrderId" });
            DropIndex("dbo.Orders", new[] { "DeviceTypeId" });
            DropIndex("dbo.Orders", new[] { "SourceId" });
            DropIndex("dbo.Orders", new[] { "OrderStatusId" });
            DropIndex("dbo.Orders", new[] { "ReceiverId" });
            DropIndex("dbo.Orders", new[] { "SenderId" });
            DropIndex("dbo.Orders", new[] { "ProductId" });
            DropIndex("dbo.APIResponses", new[] { "OrderId" });
            DropTable("dbo.Users");
            DropTable("dbo.Roles");
            DropTable("dbo.OrderLogs");
            DropTable("dbo.OrderLabels");
            DropTable("dbo.Sources");
            DropTable("dbo.Senders");
            DropTable("dbo.Receivers");
            DropTable("dbo.Products");
            DropTable("dbo.OrderStatus");
            DropTable("dbo.DeviceTypes");
            DropTable("dbo.Orders");
            DropTable("dbo.APIResponses");
        }
    }
}
