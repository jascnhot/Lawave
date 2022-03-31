namespace Lawave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chglawyerAccountRuleName : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.lawyerAccounts", "rule_sum", c => c.String());
            AddColumn("dbo.lawyerAccounts", "rule_mon", c => c.String());
            AddColumn("dbo.lawyerAccounts", "rule_tues", c => c.String());
            AddColumn("dbo.lawyerAccounts", "rule_wed", c => c.String());
            AddColumn("dbo.lawyerAccounts", "rule_thur", c => c.String());
            AddColumn("dbo.lawyerAccounts", "rule_fri", c => c.String());
            AddColumn("dbo.lawyerAccounts", "rule_sat", c => c.String());
            DropColumn("dbo.lawyerAccounts", "appointmentRule_sum");
            DropColumn("dbo.lawyerAccounts", "appointmentRule_mon");
            DropColumn("dbo.lawyerAccounts", "appointmentRule_tues");
            DropColumn("dbo.lawyerAccounts", "appointmentRule_wed");
            DropColumn("dbo.lawyerAccounts", "appointmentRule_thur");
            DropColumn("dbo.lawyerAccounts", "appointmentRule_fri");
            DropColumn("dbo.lawyerAccounts", "appointmentRule_sat");
        }
        
        public override void Down()
        {
            AddColumn("dbo.lawyerAccounts", "appointmentRule_sat", c => c.String());
            AddColumn("dbo.lawyerAccounts", "appointmentRule_fri", c => c.String());
            AddColumn("dbo.lawyerAccounts", "appointmentRule_thur", c => c.String());
            AddColumn("dbo.lawyerAccounts", "appointmentRule_wed", c => c.String());
            AddColumn("dbo.lawyerAccounts", "appointmentRule_tues", c => c.String());
            AddColumn("dbo.lawyerAccounts", "appointmentRule_mon", c => c.String());
            AddColumn("dbo.lawyerAccounts", "appointmentRule_sum", c => c.String());
            DropColumn("dbo.lawyerAccounts", "rule_sat");
            DropColumn("dbo.lawyerAccounts", "rule_fri");
            DropColumn("dbo.lawyerAccounts", "rule_thur");
            DropColumn("dbo.lawyerAccounts", "rule_wed");
            DropColumn("dbo.lawyerAccounts", "rule_tues");
            DropColumn("dbo.lawyerAccounts", "rule_mon");
            DropColumn("dbo.lawyerAccounts", "rule_sum");
        }
    }
}
