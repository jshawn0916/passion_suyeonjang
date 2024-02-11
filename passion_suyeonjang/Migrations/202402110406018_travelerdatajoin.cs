namespace passion_suyeonjang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class travelerdatajoin : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Journeys", "TravelerId", "dbo.Travelers");
            DropIndex("dbo.Journeys", new[] { "TravelerId" });
            CreateTable(
                "dbo.TravelersJourneys",
                c => new
                    {
                        Travelers_TravelerId = c.Int(nullable: false),
                        Journeys_JourneyId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Travelers_TravelerId, t.Journeys_JourneyId })
                .ForeignKey("dbo.Travelers", t => t.Travelers_TravelerId, cascadeDelete: true)
                .ForeignKey("dbo.Journeys", t => t.Journeys_JourneyId, cascadeDelete: true)
                .Index(t => t.Travelers_TravelerId)
                .Index(t => t.Journeys_JourneyId);
            
            DropColumn("dbo.Journeys", "TravelerId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Journeys", "TravelerId", c => c.Int(nullable: false));
            DropForeignKey("dbo.TravelersJourneys", "Journeys_JourneyId", "dbo.Journeys");
            DropForeignKey("dbo.TravelersJourneys", "Travelers_TravelerId", "dbo.Travelers");
            DropIndex("dbo.TravelersJourneys", new[] { "Journeys_JourneyId" });
            DropIndex("dbo.TravelersJourneys", new[] { "Travelers_TravelerId" });
            DropTable("dbo.TravelersJourneys");
            CreateIndex("dbo.Journeys", "TravelerId");
            AddForeignKey("dbo.Journeys", "TravelerId", "dbo.Travelers", "TravelerId", cascadeDelete: true);
        }
    }
}
