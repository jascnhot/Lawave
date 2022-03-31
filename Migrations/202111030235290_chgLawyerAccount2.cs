namespace Lawave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class chgLawyerAccount2 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.LawyerEducations",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        lawyerAccountId = c.Int(nullable: false),
                        schoolName = c.String(),
                        departmentName = c.String(),
                        degree = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.lawyerAccounts", t => t.lawyerAccountId, cascadeDelete: true)
                .Index(t => t.lawyerAccountId);
            
            CreateTable(
                "dbo.LawyerExperiences",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        lawyerAccountId = c.Int(nullable: false),
                        companyName = c.String(),
                        jobTitle = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.lawyerAccounts", t => t.lawyerAccountId, cascadeDelete: true)
                .Index(t => t.lawyerAccountId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.LawyerExperiences", "lawyerAccountId", "dbo.lawyerAccounts");
            DropForeignKey("dbo.LawyerEducations", "lawyerAccountId", "dbo.lawyerAccounts");
            DropIndex("dbo.LawyerExperiences", new[] { "lawyerAccountId" });
            DropIndex("dbo.LawyerEducations", new[] { "lawyerAccountId" });
            DropTable("dbo.LawyerExperiences");
            DropTable("dbo.LawyerEducations");
        }
    }
}
