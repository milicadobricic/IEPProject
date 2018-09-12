namespace IEPProject.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class NumTokens : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "NumTokens", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "NumTokens");
        }
    }
}
