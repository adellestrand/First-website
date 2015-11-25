namespace DriversJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class changeProjectNumber : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Journal", "ProjectNumber", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Journal", "ProjectNumber", c => c.Int(nullable: false));
        }
    }
}
