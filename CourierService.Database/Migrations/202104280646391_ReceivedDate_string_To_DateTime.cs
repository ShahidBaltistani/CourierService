namespace CourierService.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ReceivedDate_string_To_DateTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.TaskMails", "ReceivedDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.TaskMails", "ReceivedDate", c => c.String());
        }
    }
}
