namespace CourierService.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_IsCompleted_In_Order : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "iscompleted", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Orders", "iscompleted");
        }
    }
}
