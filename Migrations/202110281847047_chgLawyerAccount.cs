namespace Lawave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chgLawyerAccount : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.lawyerAccounts", "verifyPhotolawyer", c => c.String());
            AddColumn("dbo.lawyerAccounts", "verifyPhotoFir", c => c.String());
            AddColumn("dbo.lawyerAccounts", "verifyPhotoSec", c => c.String());
            DropColumn("dbo.lawyerAccounts", "verifyPhoto");
        }
        
        public override void Down()
        {
            AddColumn("dbo.lawyerAccounts", "verifyPhoto", c => c.String());
            DropColumn("dbo.lawyerAccounts", "verifyPhotoSec");
            DropColumn("dbo.lawyerAccounts", "verifyPhotoFir");
            DropColumn("dbo.lawyerAccounts", "verifyPhotolawyer");
        }
    }
}
