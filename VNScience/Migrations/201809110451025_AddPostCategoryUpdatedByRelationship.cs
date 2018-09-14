namespace VNScience.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostCategoryUpdatedByRelationship : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.PostCategory", "CreatedBy", c => c.String(maxLength: 128));
            AlterColumn("dbo.PostCategory", "UpdatedBy", c => c.String(maxLength: 128));
            CreateIndex("dbo.PostCategory", "CreatedBy");
            CreateIndex("dbo.PostCategory", "UpdatedBy");
            AddForeignKey("dbo.PostCategory", "CreatedBy", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.PostCategory", "UpdatedBy", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.PostCategory", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.PostCategory", "CreatedBy", "dbo.AspNetUsers");
            DropIndex("dbo.PostCategory", new[] { "UpdatedBy" });
            DropIndex("dbo.PostCategory", new[] { "CreatedBy" });
            AlterColumn("dbo.PostCategory", "UpdatedBy", c => c.String(maxLength: 128, unicode: false));
            AlterColumn("dbo.PostCategory", "CreatedBy", c => c.String(maxLength: 128, unicode: false));
        }
    }
}
