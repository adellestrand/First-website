namespace DriversJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class connect : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "UserId", c => c.Int(nullable: false));
            CreateIndex("dbo.Project", "UserId");
            AddForeignKey("dbo.Project", "UserId", "dbo.JournalUser", "UserId", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Project", "UserId", "dbo.JournalUser");
            DropIndex("dbo.Project", new[] { "UserId" });
            DropColumn("dbo.Project", "UserId");
        }
    }
}
