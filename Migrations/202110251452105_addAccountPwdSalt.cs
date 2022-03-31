namespace Lawave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAccountPwdSalt : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.lawyerAccounts", "PasswordSalt", c => c.String());
            AddColumn("dbo.publicAccounts", "PasswordSalt", c => c.String());
            AlterColumn("dbo.lawyerAccounts", "password", c => c.String());
            AlterColumn("dbo.publicAccounts", "password", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.publicAccounts", "password", c => c.String(maxLength: 200));
            AlterColumn("dbo.lawyerAccounts", "password", c => c.String(maxLength: 200));
            DropColumn("dbo.publicAccounts", "PasswordSalt");
            DropColumn("dbo.lawyerAccounts", "PasswordSalt");
        }
    }
}
