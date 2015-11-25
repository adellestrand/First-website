namespace DriversJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createTablesFromModels : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Car",
                c => new
                    {
                        Regno = c.String(nullable: false, maxLength: 128),
                        Model = c.String(),
                    })
                .PrimaryKey(t => t.Regno);
            
            CreateTable(
                "dbo.Journal",
                c => new
                    {
                        JournalId = c.Int(nullable: false, identity: true),
                        Travelers = c.String(),
                        ProjectNumber = c.String(),
                        OdometerStart = c.Int(nullable: false),
                        OdometerEnd = c.Int(nullable: false),
                        Date = c.DateTime(nullable: false),
                        From = c.String(),
                        to = c.String(),
                        Debit = c.Int(nullable: false),
                        KmNo = c.Int(nullable: false),
                        Purpose = c.String(),
                        UserId = c.Int(nullable: false),
                        Regno = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.JournalId)
                .ForeignKey("dbo.Car", t => t.Regno)
                .ForeignKey("dbo.JournalUser", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.Regno);
            
            CreateTable(
                "dbo.JournalUser",
                c => new
                    {
                        UserId = c.Int(nullable: false, identity: true),
                        Username = c.String(),
                        Password = c.String(),
                        RoleId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Role", t => t.RoleId, cascadeDelete: true)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.Role",
                c => new
                    {
                        RoleId = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                    })
                .PrimaryKey(t => t.RoleId);
            
            CreateTable(
                "dbo.UserProjects",
                c => new
                    {
                        UserProjectsId = c.Int(nullable: false, identity: true),
                        ProjectNumber = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserProjectsId)
                .ForeignKey("dbo.Project", t => t.ProjectNumber, cascadeDelete: true)
                .ForeignKey("dbo.JournalUser", t => t.UserId, cascadeDelete: true)
                .Index(t => t.ProjectNumber)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Project",
                c => new
                    {
                        ProjectNumber = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Detail = c.String(),
                    })
                .PrimaryKey(t => t.ProjectNumber);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProjects", "UserId", "dbo.JournalUser");
            DropForeignKey("dbo.UserProjects", "ProjectNo", "dbo.Project");
            DropForeignKey("dbo.JournalUser", "RoleId", "dbo.Role");
            DropForeignKey("dbo.Journal", "UserId", "dbo.JournalUser");
            DropForeignKey("dbo.Journal", "Regno", "dbo.Car");
            DropIndex("dbo.UserProjects", new[] { "UserId" });
            DropIndex("dbo.UserProjects", new[] { "ProjectNo" });
            DropIndex("dbo.JournalUser", new[] { "RoleId" });
            DropIndex("dbo.Journal", new[] { "Regno" });
            DropIndex("dbo.Journal", new[] { "UserId" });
            DropTable("dbo.Project");
            DropTable("dbo.UserProjects");
            DropTable("dbo.Role");
            DropTable("dbo.JournalUser");
            DropTable("dbo.Journal");
            DropTable("dbo.Car");
        }
    }
}
