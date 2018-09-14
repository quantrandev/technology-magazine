namespace VNScience.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddIdentityTables : DbMigration
    {
        public override void Up()
        {
            //CreateTable(
            //    "dbo.About",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Content = c.String(),
            //            IsDisplayed = c.Boolean(),
            //            CreatedAt = c.DateTime(nullable: false),
            //            CreatedBy = c.String(maxLength: 128, unicode: false),
            //            UpdatedAt = c.DateTime(),
            //            UpdatedBy = c.String(maxLength: 128, unicode: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.Branch",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(maxLength: 500),
            //            Address = c.String(maxLength: 500),
            //            Homepage = c.String(maxLength: 500, unicode: false),
            //            Tel = c.String(maxLength: 100, unicode: false),
            //            Fax = c.String(maxLength: 100, unicode: false),
            //            CreatedAt = c.DateTime(),
            //            CreatedBy = c.String(maxLength: 128, unicode: false),
            //            UpdatedAt = c.DateTime(),
            //            UpdatedBy = c.String(maxLength: 128, unicode: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.Contact",
            //    c => new
            //        {
            //            Id = c.Long(nullable: false, identity: true),
            //            Email = c.String(maxLength: 50, unicode: false),
            //            FullName = c.String(maxLength: 500),
            //            Title = c.String(maxLength: 500),
            //            Message = c.String(maxLength: 2000),
            //            CreatedAt = c.DateTime(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.Menu",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Title = c.String(maxLength: 100),
            //            Link = c.String(unicode: false),
            //            Target = c.String(maxLength: 50, unicode: false),
            //            DisplayOrder = c.Int(),
            //            CreatedAt = c.DateTime(nullable: false),
            //            CreatedBy = c.String(maxLength: 128, unicode: false),
            //            UpdatedAt = c.DateTime(),
            //            UpdatedBy = c.String(maxLength: 128, unicode: false),
            //            MenuTypeId = c.Byte(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.MenuType",
            //    c => new
            //        {
            //            Id = c.Byte(nullable: false, identity: true),
            //            Name = c.String(nullable: false, maxLength: 50),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.PostCategory",
            //    c => new
            //        {
            //            Id = c.Int(nullable: false, identity: true),
            //            Name = c.String(maxLength: 100),
            //            MetaTitle = c.String(maxLength: 100, unicode: false),
            //            Description = c.String(),
            //            IsActive = c.Boolean(),
            //            IsDisplayed = c.Boolean(),
            //            DisplayOrder = c.Int(),
            //            CreatedAt = c.DateTime(),
            //            CreatedBy = c.String(maxLength: 128, unicode: false),
            //            UpdatedAt = c.DateTime(),
            //            UpdatedBy = c.String(maxLength: 128, unicode: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.Post",
            //    c => new
            //        {
            //            Id = c.Long(nullable: false, identity: true),
            //            Title = c.String(),
            //            MetaTitle = c.String(unicode: false),
            //            Author = c.String(maxLength: 128, unicode: false),
            //            Summary = c.String(maxLength: 500),
            //            Content = c.String(),
            //            CoverImage = c.String(unicode: false),
            //            ViewCount = c.Int(),
            //            IsApproved = c.Boolean(),
            //            CreatedAt = c.DateTime(),
            //            CreatedBy = c.String(maxLength: 128, unicode: false),
            //            UpdatedAt = c.DateTime(),
            //            UpdatedBy = c.String(maxLength: 128, unicode: false),
            //            CategoryId = c.Int(),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.PostTag",
            //    c => new
            //        {
            //            TagId = c.String(nullable: false, maxLength: 100, unicode: false),
            //            PostId = c.Long(nullable: false),
            //        })
            //    .PrimaryKey(t => new { t.TagId, t.PostId });
            
            //CreateTable(
            //    "dbo.Recruitment",
            //    c => new
            //        {
            //            Id = c.Long(nullable: false, identity: true),
            //            Title = c.String(maxLength: 500),
            //            MetaTitle = c.String(maxLength: 500, unicode: false),
            //            Summary = c.String(maxLength: 500),
            //            JobTitle = c.String(maxLength: 500),
            //            JobDescription = c.String(),
            //            JobRequirements = c.String(),
            //            JobWelfare = c.String(),
            //            JobAdditionalInfo = c.String(),
            //            Quantity = c.Int(),
            //            JobWorkPlace = c.String(maxLength: 500),
            //            CreatedAt = c.DateTime(),
            //            CreatedBy = c.String(maxLength: 128, unicode: false),
            //            UpdatedAt = c.DateTime(),
            //            UpdatedBy = c.String(maxLength: 128, unicode: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            //CreateTable(
            //    "dbo.SystemInfo",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 50, unicode: false),
            //            Content = c.String(),
            //            UpdatedAt = c.DateTime(),
            //            UpdatedBy = c.String(maxLength: 128, unicode: false),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            //CreateTable(
            //    "dbo.Tag",
            //    c => new
            //        {
            //            Id = c.String(nullable: false, maxLength: 100, unicode: false),
            //            Name = c.String(nullable: false, maxLength: 100),
            //        })
            //    .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.Tag");
            DropTable("dbo.SystemInfo");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Recruitment");
            DropTable("dbo.PostTag");
            DropTable("dbo.Post");
            DropTable("dbo.PostCategory");
            DropTable("dbo.MenuType");
            DropTable("dbo.Menu");
            DropTable("dbo.Contact");
            DropTable("dbo.Branch");
            DropTable("dbo.About");
        }
    }
}
