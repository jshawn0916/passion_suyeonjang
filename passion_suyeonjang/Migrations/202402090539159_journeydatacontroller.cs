namespace passion_suyeonjang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class journeydatacontroller : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.JourneyDestinations", "DestinationId", "dbo.Destinations");
            DropForeignKey("dbo.JourneyDestinations", "JourneyId", "dbo.Journeys");
            DropIndex("dbo.JourneyDestinations", new[] { "JourneyId" });
            DropIndex("dbo.JourneyDestinations", new[] { "DestinationId" });
            DropTable("dbo.JourneyDestinations");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.JourneyDestinations",
                c => new
                    {
                        JourneyDestinationId = c.Int(nullable: false, identity: true),
                        JourneyId = c.Int(nullable: false),
                        DestinationId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.JourneyDestinationId);
            
            CreateIndex("dbo.JourneyDestinations", "DestinationId");
            CreateIndex("dbo.JourneyDestinations", "JourneyId");
            AddForeignKey("dbo.JourneyDestinations", "JourneyId", "dbo.Journeys", "JourneyId", cascadeDelete: true);
            AddForeignKey("dbo.JourneyDestinations", "DestinationId", "dbo.Destinations", "DestinationId", cascadeDelete: true);
        }
    }
}
