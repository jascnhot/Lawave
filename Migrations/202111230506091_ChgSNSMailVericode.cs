namespace Lawave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChgSNSMailVericode : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.mailVerifications", "mail", c => c.String(nullable: false));
            AlterColumn("dbo.mailVerifications", "veriCode", c => c.String(nullable: false));
            AlterColumn("dbo.snsVerifications", "veriCode", c => c.String(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.snsVerifications", "veriCode", c => c.Int(nullable: false));
            AlterColumn("dbo.mailVerifications", "veriCode", c => c.Int(nullable: false));
            AlterColumn("dbo.mailVerifications", "mail", c => c.Int(nullable: false));
        }
    }
}
