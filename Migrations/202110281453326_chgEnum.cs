namespace Lawave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chgEnum : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.lawyerGoodAtTypes", "goodAtInfoId", "dbo.goodAtInfoes");
            DropIndex("dbo.lawyerGoodAtTypes", new[] { "goodAtInfoId" });
        }
        
        public override void Down()
        {
            CreateIndex("dbo.lawyerGoodAtTypes", "goodAtInfoId");
            AddForeignKey("dbo.lawyerGoodAtTypes", "goodAtInfoId", "dbo.goodAtInfoes", "id", cascadeDelete: true);
        }
    }
}
