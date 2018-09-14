namespace VNScience.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ChangeMaximumLengthOfPostSummary : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Post", "Summary", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Post", "Summary", c => c.String(maxLength: 500));
        }
    }
}
