namespace CourierService.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_LogTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaskLogs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        TaskStatus = c.Boolean(nullable: false),
                        CreatedBy = c.Int(),
                        CreatedOn = c.DateTime(),
                        TaskDetailId = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TaskDetails", t => t.TaskDetailId)
                .Index(t => t.TaskDetailId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskLogs", "TaskDetailId", "dbo.TaskDetails");
            DropIndex("dbo.TaskLogs", new[] { "TaskDetailId" });
            DropTable("dbo.TaskLogs");
        }
    }
}
