namespace passion_suyeonjang.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editColumn : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Destinations", "DestinationDescription");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Destinations", "DestinationDescription", c => c.String());
        }
    }
}
