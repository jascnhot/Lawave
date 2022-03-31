namespace Lawave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addSNSMailVericode : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.mailVerifications",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        mail = c.Int(nullable: false),
                        veriCode = c.Int(nullable: false),
                        initDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
            CreateTable(
                "dbo.snsVerifications",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        phone = c.Int(nullable: false),
                        veriCode = c.Int(nullable: false),
                        initDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.snsVerifications");
            DropTable("dbo.mailVerifications");
        }
    }
}
