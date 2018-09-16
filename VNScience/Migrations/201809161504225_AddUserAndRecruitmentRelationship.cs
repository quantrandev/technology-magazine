namespace VNScience.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserAndRecruitmentRelationship : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Recruitment", "CreatedBy", c => c.String(maxLength: 128));
            AlterColumn("dbo.Recruitment", "UpdatedBy", c => c.String(maxLength: 128));
            CreateIndex("dbo.Recruitment", "CreatedBy");
            CreateIndex("dbo.Recruitment", "UpdatedBy");
            AddForeignKey("dbo.Recruitment", "CreatedBy", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Recruitment", "UpdatedBy", "dbo.AspNetUsers", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Recruitment", "UpdatedBy", "dbo.AspNetUsers");
            DropForeignKey("dbo.Recruitment", "CreatedBy", "dbo.AspNetUsers");
            DropIndex("dbo.Recruitment", new[] { "UpdatedBy" });
            DropIndex("dbo.Recruitment", new[] { "CreatedBy" });
            AlterColumn("dbo.Recruitment", "UpdatedBy", c => c.String(maxLength: 128, unicode: false));
            AlterColumn("dbo.Recruitment", "CreatedBy", c => c.String(maxLength: 128, unicode: false));
        }
    }
}
