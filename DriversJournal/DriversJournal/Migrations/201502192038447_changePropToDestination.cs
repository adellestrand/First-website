namespace DriversJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changePropToDestination : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Journal", "ToDestination", c => c.String());
            DropColumn("dbo.Journal", "to");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Journal", "to", c => c.String());
            DropColumn("dbo.Journal", "ToDestination");
        }
    }
}
