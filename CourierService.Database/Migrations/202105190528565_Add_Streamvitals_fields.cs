namespace CourierService.Database.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Add_Streamvitals_fields : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Orders", "SVPracticeId", c => c.Guid(nullable: false));
            AddColumn("dbo.Orders", "CurrentStatus", c => c.String());
            AddColumn("dbo.Receivers", "SVPatientId", c => c.Guid(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Receivers", "SVPatientId");
            DropColumn("dbo.Orders", "CurrentStatus");
            DropColumn("dbo.Orders", "SVPracticeId");
        }
    }
}
