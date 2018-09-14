namespace VNScience.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIsRequestedDeleteColumnOfPostCategoryTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PostCategory", "IsRequestedDelete", c => c.Boolean());
        }
        
        public override void Down()
        {
            DropColumn("dbo.PostCategory", "IsRequestedDelete");
        }
    }
}
