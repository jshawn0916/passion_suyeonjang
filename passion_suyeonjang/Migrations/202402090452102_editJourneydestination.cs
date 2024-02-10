namespace passion_suyeonjang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editJourneydestination : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.JourneyDestinations",
                c => new
                    {
                        JourneyDestinationId = c.Int(nullable: false, identity: true),
                        JourneyId = c.Int(nullable: false),
                        DestinationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JourneyDestinationId)
                .ForeignKey("dbo.Destinations", t => t.DestinationId, cascadeDelete: true)
                .ForeignKey("dbo.Journeys", t => t.JourneyId, cascadeDelete: true)
                .Index(t => t.JourneyId)
                .Index(t => t.DestinationId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.JourneyDestinations", "JourneyId", "dbo.Journeys");
            DropForeignKey("dbo.JourneyDestinations", "DestinationId", "dbo.Destinations");
            DropIndex("dbo.JourneyDestinations", new[] { "DestinationId" });
            DropIndex("dbo.JourneyDestinations", new[] { "JourneyId" });
            DropTable("dbo.JourneyDestinations");
        }
    }
}
