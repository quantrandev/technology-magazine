namespace VNScience.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddPostTagRelationship : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.PostTags",
                c => new
                    {
                        Post_Id = c.Long(nullable: false),
                        Tag_Id = c.String(nullable: false, maxLength: 100, unicode: false),
                    })
                .PrimaryKey(t => new { t.Post_Id, t.Tag_Id })
                .ForeignKey("dbo.Post", t => t.Post_Id, cascadeDelete: true)
                .ForeignKey("dbo.Tag", t => t.Tag_Id, cascadeDelete: true)
                .Index(t => t.Post_Id)
                .Index(t => t.Tag_Id);
            
            DropTable("dbo.PostTag");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.PostTag",
                c => new
                    {
                        TagId = c.String(nullable: false, maxLength: 100, unicode: false),
                        PostId = c.Long(nullable: false),
                    })
                .PrimaryKey(t => new { t.TagId, t.PostId });
            
            DropForeignKey("dbo.PostTags", "Tag_Id", "dbo.Tag");
            DropForeignKey("dbo.PostTags", "Post_Id", "dbo.Post");
            DropIndex("dbo.PostTags", new[] { "Tag_Id" });
            DropIndex("dbo.PostTags", new[] { "Post_Id" });
            DropTable("dbo.PostTags");
        }
    }
}
