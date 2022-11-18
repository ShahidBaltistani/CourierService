namespace CourierService.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class BaseEntity_INTO_Task : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskDetails", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.TaskDetails", "CreatedBy", c => c.Int());
            AddColumn("dbo.TaskDetails", "ModifiedOn", c => c.DateTime());
            AddColumn("dbo.TaskDetails", "ModifiedBy", c => c.Int());
            AddColumn("dbo.TaskDetails", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.TaskDetails", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.TaskMails", "IsCompleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.TaskMails", "CreatedOn", c => c.DateTime());
            AddColumn("dbo.TaskMails", "CreatedBy", c => c.Int());
            AddColumn("dbo.TaskMails", "ModifiedOn", c => c.DateTime());
            AddColumn("dbo.TaskMails", "ModifiedBy", c => c.Int());
            AddColumn("dbo.TaskMails", "IsDeleted", c => c.Boolean(nullable: false));
            AddColumn("dbo.TaskMails", "IsActive", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaskMails", "IsActive");
            DropColumn("dbo.TaskMails", "IsDeleted");
            DropColumn("dbo.TaskMails", "ModifiedBy");
            DropColumn("dbo.TaskMails", "ModifiedOn");
            DropColumn("dbo.TaskMails", "CreatedBy");
            DropColumn("dbo.TaskMails", "CreatedOn");
            DropColumn("dbo.TaskMails", "IsCompleted");
            DropColumn("dbo.TaskDetails", "IsActive");
            DropColumn("dbo.TaskDetails", "IsDeleted");
            DropColumn("dbo.TaskDetails", "ModifiedBy");
            DropColumn("dbo.TaskDetails", "ModifiedOn");
            DropColumn("dbo.TaskDetails", "CreatedBy");
            DropColumn("dbo.TaskDetails", "CreatedOn");
        }
    }
}
