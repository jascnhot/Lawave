namespace Lawave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chglawyerAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.lawyerAccounts", "bachelor", c => c.String());
            AddColumn("dbo.lawyerAccounts", "phoneCost", c => c.String());
            AddColumn("dbo.lawyerAccounts", "faceCost", c => c.String());
            AlterColumn("dbo.FAQs", "title", c => c.Int(nullable: false));
            AlterColumn("dbo.FAQs", "ans", c => c.Int(nullable: false));
            AlterColumn("dbo.FAQs", "InitDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.lawyerAccounts", "cost");
        }
        
        public override void Down()
        {
            AddColumn("dbo.lawyerAccounts", "cost", c => c.String());
            AlterColumn("dbo.FAQs", "InitDate", c => c.DateTime(nullable: false));
            AlterColumn("dbo.FAQs", "ans", c => c.String());
            AlterColumn("dbo.FAQs", "title", c => c.String());
            DropColumn("dbo.lawyerAccounts", "faceCost");
            DropColumn("dbo.lawyerAccounts", "phoneCost");
            DropColumn("dbo.lawyerAccounts", "bachelor");
        }
    }
}
