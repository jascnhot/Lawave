namespace Lawave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addevalTime : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.appointmentlists", "evalTime", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.appointmentlists", "evalTime");
        }
    }
}
