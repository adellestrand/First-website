namespace DriversJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addPropDates : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Journal", "StartDate", c => c.DateTime(nullable: false));
            AddColumn("dbo.Journal", "EndDate", c => c.DateTime(nullable: false));
            DropColumn("dbo.Journal", "Date");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Journal", "Date", c => c.DateTime(nullable: false));
            DropColumn("dbo.Journal", "EndDate");
            DropColumn("dbo.Journal", "StartDate");
        }
    }
}
