namespace DriversJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletedPropModelAddOdometer : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Car", "Odometer", c => c.Int(nullable: false));
            DropColumn("dbo.Car", "Model");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Car", "Model", c => c.String());
            DropColumn("dbo.Car", "Odometer");
        }
    }
}
