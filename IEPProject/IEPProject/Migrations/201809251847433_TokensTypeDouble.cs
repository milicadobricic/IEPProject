namespace IEPProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class TokensTypeDouble : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AspNetUsers", "NumTokens", c => c.Double(nullable: false));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AspNetUsers", "NumTokens", c => c.Int(nullable: false));
        }
    }
}
