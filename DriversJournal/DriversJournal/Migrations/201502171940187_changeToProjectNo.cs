namespace DriversJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeToProjectNo : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.UserProjects", "ProjectNumber", "dbo.Project");
            RenameColumn(table: "dbo.UserProjects", name: "ProjectNumber", newName: "ProjectNo");
            RenameIndex(table: "dbo.UserProjects", name: "IX_ProjectNumber", newName: "IX_ProjectNo");
            DropPrimaryKey("dbo.Project");
            AddColumn("dbo.Project", "ProjectNo", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Project", "ProjectNo");
            AddForeignKey("dbo.UserProjects", "ProjectNo", "dbo.Project", "ProjectNo", cascadeDelete: true);
            DropColumn("dbo.Project", "ProjectNumber");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Project", "ProjectNumber", c => c.Int(nullable: false));
            DropForeignKey("dbo.UserProjects", "ProjectNo", "dbo.Project");
            DropPrimaryKey("dbo.Project");
            DropColumn("dbo.Project", "ProjectNo");
            AddPrimaryKey("dbo.Project", "ProjectNumber");
            RenameIndex(table: "dbo.UserProjects", name: "IX_ProjectNo", newName: "IX_ProjectNumber");
            RenameColumn(table: "dbo.UserProjects", name: "ProjectNo", newName: "ProjectNumber");
            AddForeignKey("dbo.UserProjects", "ProjectNumber", "dbo.Project", "ProjectNumber", cascadeDelete: true);
        }
    }
}
