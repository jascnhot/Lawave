namespace Lawave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chgLawyerAccount1 : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.lawyerAccounts", "office");
            DropColumn("dbo.lawyerAccounts", "experience");
            DropColumn("dbo.lawyerAccounts", "education");
            DropColumn("dbo.lawyerAccounts", "bachelor");
        }
        
        public override void Down()
        {
            AddColumn("dbo.lawyerAccounts", "bachelor", c => c.String());
            AddColumn("dbo.lawyerAccounts", "education", c => c.String());
            AddColumn("dbo.lawyerAccounts", "experience", c => c.String());
            AddColumn("dbo.lawyerAccounts", "office", c => c.String(maxLength: 200));
        }
    }
}
