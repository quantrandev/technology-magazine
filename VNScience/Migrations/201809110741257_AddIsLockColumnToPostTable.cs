namespace VNScience.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsLockColumnToPostTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Post", "IsLock", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Post", "IsLock");
        }
    }
}
