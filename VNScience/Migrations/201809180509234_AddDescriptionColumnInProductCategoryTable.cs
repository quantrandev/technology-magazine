namespace VNScience.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDescriptionColumnInProductCategoryTable : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ProductCategory", "Description", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.ProductCategory", "Description");
        }
    }
}
