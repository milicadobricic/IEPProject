namespace IEPProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OfferedPriceAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bids", "OfferedPrice", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bids", "OfferedPrice");
        }
    }
}
