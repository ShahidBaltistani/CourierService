namespace CourierService.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Manage_Table_For_Practices : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.NewPractices",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        SVPracticeId = c.Guid(nullable: false),
                        IsActive = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Orders", "NewPracticeId", c => c.Int());
            AddColumn("dbo.TaskDetails", "NewPracticeId", c => c.Int());
            CreateIndex("dbo.Orders", "NewPracticeId");
            CreateIndex("dbo.TaskDetails", "NewPracticeId");
            AddForeignKey("dbo.Orders", "NewPracticeId", "dbo.NewPractices", "Id");
            AddForeignKey("dbo.TaskDetails", "NewPracticeId", "dbo.NewPractices", "Id");
            DropColumn("dbo.Orders", "SVPracticeId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "SVPracticeId", c => c.Guid(nullable: false));
            DropForeignKey("dbo.TaskDetails", "NewPracticeId", "dbo.NewPractices");
            DropForeignKey("dbo.Orders", "NewPracticeId", "dbo.NewPractices");
            DropIndex("dbo.TaskDetails", new[] { "NewPracticeId" });
            DropIndex("dbo.Orders", new[] { "NewPracticeId" });
            DropColumn("dbo.TaskDetails", "NewPracticeId");
            DropColumn("dbo.Orders", "NewPracticeId");
            DropTable("dbo.NewPractices");
        }
    }
}
