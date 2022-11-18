namespace CourierService.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_fields_In_OrderTable : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.TaskDetails", "NewPracticeId", "dbo.NewPractices");
            DropIndex("dbo.TaskDetails", new[] { "NewPracticeId" });
            AddColumn("dbo.Orders", "IntegrationRemarks", c => c.String());
            AddColumn("dbo.Orders", "Address", c => c.String());
            DropColumn("dbo.TaskDetails", "NewPracticeId");
            DropTable("dbo.Practices");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Practices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.TaskDetails", "NewPracticeId", c => c.Int());
            DropColumn("dbo.Orders", "Address");
            DropColumn("dbo.Orders", "IntegrationRemarks");
            CreateIndex("dbo.TaskDetails", "NewPracticeId");
            AddForeignKey("dbo.TaskDetails", "NewPracticeId", "dbo.NewPractices", "Id");
        }
    }
}
