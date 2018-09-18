namespace VNScience.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductCategorySelfRelationship : DbMigration
    {
        public override void Up()
        {
            DropIndex("dbo.ProductCategory", new[] { "ParentId" });
            AlterColumn("dbo.ProductCategory", "ParentId", c => c.Int());
            CreateIndex("dbo.ProductCategory", "ParentId");
        }
        
        public override void Down()
        {
            DropIndex("dbo.ProductCategory", new[] { "ParentId" });
            AlterColumn("dbo.ProductCategory", "ParentId", c => c.Int(nullable: false));
            CreateIndex("dbo.ProductCategory", "ParentId");
        }
    }
}
