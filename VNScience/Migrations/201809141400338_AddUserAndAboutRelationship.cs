namespace VNScience.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserAndAboutRelationship : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.About", "CreatedBy", c => c.String(maxLength: 128));
            AlterColumn("dbo.About", "UpdatedBy", c => c.String(maxLength: 128));
            CreateIndex("dbo.About", "CreatedBy");
            CreateIndex("dbo.About", "UpdatedBy");
            AddForeignKey("dbo.About", "CreatedBy", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.About", "UpdatedBy", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.About", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.About", "CreatedBy", "dbo.AspNetUsers");
            DropIndex("dbo.About", new[] { "UpdatedBy" });
            DropIndex("dbo.About", new[] { "CreatedBy" });
            AlterColumn("dbo.About", "UpdatedBy", c => c.String(maxLength: 128, unicode: false));
            AlterColumn("dbo.About", "CreatedBy", c => c.String(maxLength: 128, unicode: false));
        }
    }
}
