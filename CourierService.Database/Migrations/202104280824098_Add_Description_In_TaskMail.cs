namespace CourierService.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Description_In_TaskMail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskMails", "Description", c => c.String());
            DropColumn("dbo.TaskDetails", "Description");
        }
        
        public override void Down()
        {
            AddColumn("dbo.TaskDetails", "Description", c => c.String());
            DropColumn("dbo.TaskMails", "Description");
        }
    }
}
