namespace VNScience.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserPostRelationship : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Post", "CreatedBy", c => c.String(maxLength: 128));
            AlterColumn("dbo.Post", "UpdatedBy", c => c.String(maxLength: 128));
            CreateIndex("dbo.Post", "CreatedBy");
            CreateIndex("dbo.Post", "UpdatedBy");
            AddForeignKey("dbo.Post", "CreatedBy", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Post", "UpdatedBy", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Post", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Post", "CreatedBy", "dbo.AspNetUsers");
            DropIndex("dbo.Post", new[] { "UpdatedBy" });
            DropIndex("dbo.Post", new[] { "CreatedBy" });
            AlterColumn("dbo.Post", "UpdatedBy", c => c.String(maxLength: 128, unicode: false));
            AlterColumn("dbo.Post", "CreatedBy", c => c.String(maxLength: 128, unicode: false));
        }
    }
}
