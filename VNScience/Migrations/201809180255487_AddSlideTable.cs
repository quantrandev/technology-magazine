namespace VNScience.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddSlideTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Slide",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Link = c.String(),
                        Target = c.String(maxLength: 50),
                        DisplayOrder = c.Int(),
                        CreatedAt = c.DateTime(nullable: false),
                        CreatedBy = c.String(maxLength: 128),
                        UpdatedAt = c.DateTime(),
                        UpdatedBy = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.CreatedBy)
                .ForeignKey("dbo.AspNetUsers", t => t.UpdatedBy)
                .Index(t => t.CreatedBy)
                .Index(t => t.UpdatedBy);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Slide", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Slide", "CreatedBy", "dbo.AspNetUsers");
            DropIndex("dbo.Slide", new[] { "UpdatedBy" });
            DropIndex("dbo.Slide", new[] { "CreatedBy" });
            DropTable("dbo.Slide");
        }
    }
}
