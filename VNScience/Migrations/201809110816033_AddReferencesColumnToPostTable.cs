namespace VNScience.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddReferencesColumnToPostTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Post", "References", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Post", "References");
        }
    }
}
