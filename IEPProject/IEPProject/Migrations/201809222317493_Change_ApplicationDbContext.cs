namespace IEPProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change_ApplicationDbContext : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Auctions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 100),
                        ImagePath = c.String(nullable: false, maxLength: 1000),
                        Duration = c.Int(nullable: false),
                        StartPrice = c.Double(nullable: false),
                        CurrentPrice = c.Double(nullable: false),
                        CreationTime = c.DateTime(nullable: false),
                        OpeningTime = c.DateTime(),
                        ClosingTime = c.DateTime(),
                        State = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Bids",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Time = c.DateTime(nullable: false),
                        State = c.Int(nullable: false),
                        Auction_Id = c.Int(),
                        User_Id = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Auctions", t => t.Auction_Id)
                .ForeignKey("dbo.AspNetUsers", t => t.User_Id)
                .Index(t => t.Auction_Id)
                .Index(t => t.User_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Bids", "User_Id", "dbo.AspNetUsers");
            DropForeignKey("dbo.Bids", "Auction_Id", "dbo.Auctions");
            DropIndex("dbo.Bids", new[] { "User_Id" });
            DropIndex("dbo.Bids", new[] { "Auction_Id" });
            DropTable("dbo.Bids");
            DropTable("dbo.Auctions");
        }
    }
}
