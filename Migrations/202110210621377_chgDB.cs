namespace Lawave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chgDB : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.lawyerAccounts", "bachelor", c => c.String());
            AddColumn("dbo.lawyerAccounts", "phoneCost", c => c.String());
            AddColumn("dbo.lawyerAccounts", "faceCost", c => c.String());
            DropColumn("dbo.lawyerAccounts", "cost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.lawyerAccounts", "cost", c => c.String());
            DropColumn("dbo.lawyerAccounts", "faceCost");
            DropColumn("dbo.lawyerAccounts", "phoneCost");
            DropColumn("dbo.lawyerAccounts", "bachelor");
        }
    }
}
