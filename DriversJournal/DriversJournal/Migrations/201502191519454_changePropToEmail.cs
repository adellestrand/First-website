namespace DriversJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changePropToEmail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JournalUser", "Email", c => c.String());
            DropColumn("dbo.JournalUser", "Username");
        }
        
        public override void Down()
        {
            AddColumn("dbo.JournalUser", "Username", c => c.String());
            DropColumn("dbo.JournalUser", "Email");
        }
    }
}
