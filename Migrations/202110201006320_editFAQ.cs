namespace Lawave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editFAQ : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FAQs", "title", c => c.String());
            AlterColumn("dbo.FAQs", "ans", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FAQs", "ans", c => c.Int(nullable: false));
            AlterColumn("dbo.FAQs", "title", c => c.Int(nullable: false));
        }
    }
}
