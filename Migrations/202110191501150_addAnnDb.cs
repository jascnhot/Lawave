namespace Lawave.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addAnnDb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.appointmentlists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        lawyerAccountId = c.Int(nullable: false),
                        publicAccountId = c.Int(),
                        status = c.Int(nullable: false),
                        caseInfo = c.String(),
                        caseType = c.String(),
                        rejection = c.String(),
                        lawyerOpinion = c.String(),
                        lawyerStar = c.Int(),
                        lawaveOpinion = c.String(),
                        LawaveStar = c.Int(),
                        rejectionTime = c.DateTime(),
                        startTime = c.DateTime(nullable: false),
                        InitDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.lawyerAccounts", t => t.lawyerAccountId, cascadeDelete: true)
                .ForeignKey("dbo.publicAccounts", t => t.publicAccountId)
                .Index(t => t.lawyerAccountId)
                .Index(t => t.publicAccountId);
            
            CreateTable(
                "dbo.lawyerAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        mail = c.String(nullable: false, maxLength: 200),
                        password = c.String(maxLength: 200),
                        isCommunity = c.Boolean(nullable: false),
                        googleID = c.String(),
                        firstName = c.String(maxLength: 200),
                        LastName = c.String(maxLength: 200),
                        shot = c.String(),
                        verifyPhoto = c.String(),
                        isPublic = c.Boolean(nullable: false),
                        office = c.String(maxLength: 200),
                        phone = c.String(maxLength: 30),
                        saying = c.String(),
                        introduction = c.String(),
                        experience = c.String(),
                        education = c.String(),
                        professional = c.String(),
                        cost = c.String(),
                        isCertification = c.Boolean(nullable: false),
                        InitDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.lawyerAreas",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        lawyerAccountId = c.Int(nullable: false),
                        area = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.lawyerAccounts", t => t.lawyerAccountId, cascadeDelete: true)
                .Index(t => t.lawyerAccountId);
            
            CreateTable(
                "dbo.lawyerBlacklists",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        lawyerAccountId = c.Int(nullable: false),
                        publicAccountId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.lawyerAccounts", t => t.lawyerAccountId, cascadeDelete: true)
                .ForeignKey("dbo.publicAccounts", t => t.publicAccountId, cascadeDelete: true)
                .Index(t => t.lawyerAccountId)
                .Index(t => t.publicAccountId);
            
            CreateTable(
                "dbo.publicAccounts",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        mail = c.String(nullable: false, maxLength: 200),
                        password = c.String(maxLength: 200),
                        isCommunity = c.Boolean(nullable: false),
                        googleID = c.String(),
                        firstName = c.String(maxLength: 200),
                        LastName = c.String(maxLength: 200),
                        shot = c.String(),
                        phone = c.String(maxLength: 30),
                        introduction = c.String(),
                        InitDate = c.DateTime(nullable: false, defaultValueSql: "GETDATE()"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.publicCollections",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        lawyerAccountId = c.Int(nullable: false),
                        publicAccountId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.lawyerAccounts", t => t.lawyerAccountId, cascadeDelete: true)
                .ForeignKey("dbo.publicAccounts", t => t.publicAccountId, cascadeDelete: true)
                .Index(t => t.lawyerAccountId)
                .Index(t => t.publicAccountId);
            
            CreateTable(
                "dbo.lawyerGoodAtTypes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        lawyerAccountId = c.Int(nullable: false),
                        goodAtInfoId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.goodAtInfoes", t => t.goodAtInfoId, cascadeDelete: true)
                .ForeignKey("dbo.lawyerAccounts", t => t.lawyerAccountId, cascadeDelete: true)
                .Index(t => t.lawyerAccountId)
                .Index(t => t.goodAtInfoId);
            
            CreateTable(
                "dbo.goodAtInfoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        name = c.String(nullable: false, maxLength: 20),
                        isFix = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.lawyerGoodAtTypes", "lawyerAccountId", "dbo.lawyerAccounts");
            DropForeignKey("dbo.lawyerGoodAtTypes", "goodAtInfoId", "dbo.goodAtInfoes");
            DropForeignKey("dbo.publicCollections", "publicAccountId", "dbo.publicAccounts");
            DropForeignKey("dbo.publicCollections", "lawyerAccountId", "dbo.lawyerAccounts");
            DropForeignKey("dbo.lawyerBlacklists", "publicAccountId", "dbo.publicAccounts");
            DropForeignKey("dbo.appointmentlists", "publicAccountId", "dbo.publicAccounts");
            DropForeignKey("dbo.lawyerBlacklists", "lawyerAccountId", "dbo.lawyerAccounts");
            DropForeignKey("dbo.lawyerAreas", "lawyerAccountId", "dbo.lawyerAccounts");
            DropForeignKey("dbo.appointmentlists", "lawyerAccountId", "dbo.lawyerAccounts");
            DropIndex("dbo.lawyerGoodAtTypes", new[] { "goodAtInfoId" });
            DropIndex("dbo.lawyerGoodAtTypes", new[] { "lawyerAccountId" });
            DropIndex("dbo.publicCollections", new[] { "publicAccountId" });
            DropIndex("dbo.publicCollections", new[] { "lawyerAccountId" });
            DropIndex("dbo.lawyerBlacklists", new[] { "publicAccountId" });
            DropIndex("dbo.lawyerBlacklists", new[] { "lawyerAccountId" });
            DropIndex("dbo.lawyerAreas", new[] { "lawyerAccountId" });
            DropIndex("dbo.appointmentlists", new[] { "publicAccountId" });
            DropIndex("dbo.appointmentlists", new[] { "lawyerAccountId" });
            DropTable("dbo.goodAtInfoes");
            DropTable("dbo.lawyerGoodAtTypes");
            DropTable("dbo.publicCollections");
            DropTable("dbo.publicAccounts");
            DropTable("dbo.lawyerBlacklists");
            DropTable("dbo.lawyerAreas");
            DropTable("dbo.lawyerAccounts");
            DropTable("dbo.appointmentlists");
        }
    }
}
