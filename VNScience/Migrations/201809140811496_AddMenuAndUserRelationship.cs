namespace VNScience.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMenuAndUserRelationship : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Menu", "CreatedBy", c => c.String(maxLength: 128));
            AlterColumn("dbo.Menu", "UpdatedBy", c => c.String(maxLength: 128));
            CreateIndex("dbo.Menu", "CreatedBy");
            CreateIndex("dbo.Menu", "UpdatedBy");
            AddForeignKey("dbo.Menu", "CreatedBy", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Menu", "UpdatedBy", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Menu", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Menu", "CreatedBy", "dbo.AspNetUsers");
            DropIndex("dbo.Menu", new[] { "UpdatedBy" });
            DropIndex("dbo.Menu", new[] { "CreatedBy" });
            AlterColumn("dbo.Menu", "UpdatedBy", c => c.String(maxLength: 128, unicode: false));
            AlterColumn("dbo.Menu", "CreatedBy", c => c.String(maxLength: 128, unicode: false));
        }
    }
}
