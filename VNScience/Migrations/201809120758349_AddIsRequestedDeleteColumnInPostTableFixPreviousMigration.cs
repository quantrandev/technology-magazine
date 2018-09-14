namespace VNScience.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsRequestedDeleteColumnInPostTableFixPreviousMigration : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Post", "IsRequestedDelete", c => c.Boolean());
            DropColumn("dbo.Post", "IsRequestdDelete");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Post", "IsRequestdDelete", c => c.Boolean());
            DropColumn("dbo.Post", "IsRequestedDelete");
        }
    }
}
