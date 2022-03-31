namespace Lawave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chgLawyerAccount_totalScore : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.lawyerAccounts", "totalScore", c => c.Int());
        }
        
        public override void Down()
        {
            DropColumn("dbo.lawyerAccounts", "totalScore");
        }
    }
}
