namespace CourierService.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_SR_In_TaskDetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.TaskDetails", "SendleRefrence", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.TaskDetails", "SendleRefrence");
        }
    }
}
