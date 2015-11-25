namespace DriversJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deltedTableUserProjects : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserProjects", "UserId", "dbo.JournalUser");
            DropForeignKey("dbo.UserProjects", "ProjectNo", "dbo.Project");
            DropIndex("dbo.UserProjects", new[] { "ProjectNo" });
            DropIndex("dbo.UserProjects", new[] { "UserId" });
            DropPrimaryKey("dbo.Project");
            AddColumn("dbo.Project", "ProjectId", c => c.Int(nullable: false, identity: true));
            AddPrimaryKey("dbo.Project", "ProjectId");
            DropTable("dbo.UserProjects");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.UserProjects",
                c => new
                    {
                        UserProjectsId = c.Int(nullable: false, identity: true),
                        ProjectNo = c.Int(nullable: false),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserProjectsId);
            
            DropPrimaryKey("dbo.Project");
            DropColumn("dbo.Project", "ProjectId");
            AddPrimaryKey("dbo.Project", "ProjectNo");
            CreateIndex("dbo.UserProjects", "UserId");
            CreateIndex("dbo.UserProjects", "ProjectNo");
            AddForeignKey("dbo.UserProjects", "ProjectNo", "dbo.Project", "ProjectNo", cascadeDelete: true);
            AddForeignKey("dbo.UserProjects", "UserId", "dbo.JournalUser", "UserId", cascadeDelete: true);
        }
    }
}
