namespace DriversJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changePropFrom : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Journal", "FromDestination", c => c.String());
            DropColumn("dbo.Journal", "From");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Journal", "From", c => c.String());
            DropColumn("dbo.Journal", "FromDestination");
        }
    }
}
