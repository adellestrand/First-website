namespace DriversJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class addpropActive : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Project", "Active", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Project", "Active");
        }
    }
}
