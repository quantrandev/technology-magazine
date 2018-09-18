namespace VNScience.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsPreviewColumnInPostTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Post", "IsPreview", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Post", "IsPreview");
        }
    }
}
