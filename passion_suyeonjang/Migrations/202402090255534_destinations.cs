namespace passion_suyeonjang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class destinations : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Destinations",
                c => new
                    {
                        DestinationId = c.Int(nullable: false, identity: true),
                        DestinationName = c.String(),
                        DestinationCategory = c.String(),
                        DestinationLocation = c.String(),
                        DestinationDescription = c.String(),
                    })
                .PrimaryKey(t => t.DestinationId);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Destinations");
        }
    }
}
