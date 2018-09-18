namespace VNScience.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProductCategoryTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProductCategory",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 100),
                        CoverImage = c.String(),
                        DisplayOrder = c.Int(),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 128),
                        ParentId = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.ProductCategory", t => t.ParentId)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy)
                .Index(t => t.ParentId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProductCategory", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProductCategory", "ParentId", "dbo.ProductCategory");
            DropForeignKey("dbo.ProductCategory", "CreatedBy", "dbo.AspNetUsers");
            DropIndex("dbo.ProductCategory", new[] { "ParentId" });
            DropIndex("dbo.ProductCategory", new[] { "UpdatedBy" });
            DropIndex("dbo.ProductCategory", new[] { "CreatedBy" });
            DropTable("dbo.ProductCategory");
        }
    }
}
