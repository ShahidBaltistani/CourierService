namespace CourierService.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_oneToMany_relation_taskDetail_taskmail : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.TaskDetails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Description = c.String(),
                        Remarks = c.String(),
                        PatientName = c.String(),
                        PatientAddress = c.String(),
                        PatientClinic = c.String(),
                        Status = c.Boolean(nullable: false),
                        TaskMailId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.TaskMails", t => t.TaskMailId, cascadeDelete: true)
                .Index(t => t.TaskMailId);
            
            CreateTable(
                "dbo.TaskMails",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        From = c.String(),
                        Subject = c.String(),
                        ReceivedDate = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.TaskDetails", "TaskMailId", "dbo.TaskMails");
            DropIndex("dbo.TaskDetails", new[] { "TaskMailId" });
            DropTable("dbo.TaskMails");
            DropTable("dbo.TaskDetails");
        }
    }
}
