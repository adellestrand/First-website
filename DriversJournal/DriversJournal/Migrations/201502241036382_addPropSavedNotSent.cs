namespace DriversJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPropSavedNotSent : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Journal", "SavedNotSent", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Journal", "SavedNotSent");
        }
    }
}
