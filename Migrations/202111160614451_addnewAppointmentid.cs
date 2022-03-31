namespace Lawave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addnewAppointmentid : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Chatlogs", "AppointmentId", c => c.Int(nullable: false));
            CreateIndex("dbo.Chatlogs", "AppointmentId");
            AddForeignKey("dbo.Chatlogs", "AppointmentId", "dbo.appointmentlists", "id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Chatlogs", "AppointmentId", "dbo.appointmentlists");
            DropIndex("dbo.Chatlogs", new[] { "AppointmentId" });
            DropColumn("dbo.Chatlogs", "AppointmentId");
        }
    }
}
