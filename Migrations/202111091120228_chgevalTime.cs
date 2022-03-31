namespace Lawave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chgevalTime : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.appointmentlists", "evalTime", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.appointmentlists", "evalTime", c => c.DateTime(nullable: false));
        }
    }
}
