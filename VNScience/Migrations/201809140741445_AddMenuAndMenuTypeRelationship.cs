namespace VNScience.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddMenuAndMenuTypeRelationship : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Menu", "MenuTypeId", c => c.Byte(nullable: false));
            CreateIndex("dbo.Menu", "MenuTypeId");
            AddForeignKey("dbo.Menu", "MenuTypeId", "dbo.MenuType", "Id", cascadeDelete: true);
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Menu", "MenuTypeId", "dbo.MenuType");
            DropIndex("dbo.Menu", new[] { "MenuTypeId" });
            AlterColumn("dbo.Menu", "MenuTypeId", c => c.Byte());
        }
    }
}
