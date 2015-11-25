namespace DriversJournal.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class createTableSalt : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Salt",
                c => new
                    {
                        SaltId = c.Int(nullable: false, identity: true),
                        SaltValue = c.String(),
                        UserId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.SaltId)
                .ForeignKey("dbo.JournalUser", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Salt", "UserId", "dbo.JournalUser");
            DropIndex("dbo.Salt", new[] { "UserId" });
            DropTable("dbo.Salt");
        }
    }
}
