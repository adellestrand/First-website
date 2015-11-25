namespace DriversJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class creatPropAccountConfirmed : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.JournalUser", "AccountConfirmed", c => c.Short(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.JournalUser", "AccountConfirmed");
        }
    }
}
