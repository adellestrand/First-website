namespace DriversJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeTableUsers : DbMigration
    {
        public override void Up()
        {
            RenameTable(name: "dbo.User", newName: "JournalUser");
        }
        
        public override void Down()
        {
            RenameTable(name: "dbo.JournalUser", newName: "User");
        }
    }
}
