namespace IEPProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AuctionCreatorAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Auctions", "Creator_Id", c => c.String(maxLength: 128));
            CreateIndex("dbo.Auctions", "Creator_Id");
            AddForeignKey("dbo.Auctions", "Creator_Id", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Auctions", "Creator_Id", "dbo.AspNetUsers");
            DropIndex("dbo.Auctions", new[] { "Creator_Id" });
            DropColumn("dbo.Auctions", "Creator_Id");
        }
    }
}
