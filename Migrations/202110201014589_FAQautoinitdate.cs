namespace Lawave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FAQautoinitdate : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.FAQs", "InitDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.FAQs", "InitDate", c => c.DateTime(nullable: false));
        }
    }
}
