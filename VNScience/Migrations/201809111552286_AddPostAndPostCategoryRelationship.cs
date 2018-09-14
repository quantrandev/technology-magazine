namespace VNScience.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostAndPostCategoryRelationship : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Post", "CategoryId", c => c.Int(nullable: false));
            CreateIndex("dbo.Post", "CategoryId");
            AddForeignKey("dbo.Post", "CategoryId", "dbo.PostCategory", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Post", "CategoryId", "dbo.PostCategory");
            DropIndex("dbo.Post", new[] { "CategoryId" });
            AlterColumn("dbo.Post", "CategoryId", c => c.Int());
        }
    }
}
