namespace DriversJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addFirstNameLastname : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JournalUser", "FirstName", c => c.String());
            AddColumn("dbo.JournalUser", "LastName", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.JournalUser", "LastName");
            DropColumn("dbo.JournalUser", "FirstName");
        }
    }
}
