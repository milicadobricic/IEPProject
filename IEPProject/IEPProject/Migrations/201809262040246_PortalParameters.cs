namespace IEPProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class PortalParameters : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PortalParameters",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        N = c.Int(nullable: false),
                        D = c.Int(nullable: false),
                        S = c.Int(nullable: false),
                        G = c.Int(nullable: false),
                        P = c.Int(nullable: false),
                        C = c.String(nullable: false, maxLength: 100),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.PortalParameters");
        }
    }
}
