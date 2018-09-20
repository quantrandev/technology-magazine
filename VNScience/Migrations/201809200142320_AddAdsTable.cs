namespace VNScience.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAdsTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Ad",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Content = c.String(),
                        Link = c.String(),
                        Target = c.String(maxLength: 50),
                        ClickCount = c.Long(nullable: false),
                        Position = c.Int(nullable: false),
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
            DropForeignKey("dbo.Ad", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Ad", "CreatedBy", "dbo.AspNetUsers");
            DropIndex("dbo.Ad", new[] { "UpdatedBy" });
            DropIndex("dbo.Ad", new[] { "CreatedBy" });
            DropTable("dbo.Ad");
        }
    }
}
