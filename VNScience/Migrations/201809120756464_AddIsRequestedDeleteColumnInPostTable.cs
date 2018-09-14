namespace VNScience.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsRequestedDeleteColumnInPostTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Post", "IsRequestedDelete", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Post", "IsRequestedDelete");
        }
    }
}
