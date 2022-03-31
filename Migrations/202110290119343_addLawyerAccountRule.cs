namespace Lawave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addLawyerAccountRule : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.lawyerAccounts", "appointmentRule_sum", c => c.String());
            AddColumn("dbo.lawyerAccounts", "appointmentRule_mon", c => c.String());
            AddColumn("dbo.lawyerAccounts", "appointmentRule_tues", c => c.String());
            AddColumn("dbo.lawyerAccounts", "appointmentRule_wed", c => c.String());
            AddColumn("dbo.lawyerAccounts", "appointmentRule_thur", c => c.String());
            AddColumn("dbo.lawyerAccounts", "appointmentRule_fri", c => c.String());
            AddColumn("dbo.lawyerAccounts", "appointmentRule_sat", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.lawyerAccounts", "appointmentRule_sat");
            DropColumn("dbo.lawyerAccounts", "appointmentRule_fri");
            DropColumn("dbo.lawyerAccounts", "appointmentRule_thur");
            DropColumn("dbo.lawyerAccounts", "appointmentRule_wed");
            DropColumn("dbo.lawyerAccounts", "appointmentRule_tues");
            DropColumn("dbo.lawyerAccounts", "appointmentRule_mon");
            DropColumn("dbo.lawyerAccounts", "appointmentRule_sum");
        }
    }
}
