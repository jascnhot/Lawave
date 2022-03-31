namespace Lawave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chgFQAs : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FAQs", "initDate", c => c.DateTime());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FAQs", "initDate", c => c.DateTime(nullable: false));
        }
    }
}
