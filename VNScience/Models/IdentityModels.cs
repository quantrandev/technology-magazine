﻿using System.Collections.Generic;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using VNScience.Models.Core;

namespace VNScience.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public string Avatar { get; set; }
        public string FullName { get; set; }

        public virtual ICollection<PostCategory> UpdatedPostCategories { get; set; }
        public virtual ICollection<PostCategory> CreatedPostCategories { get; set; }
        public virtual ICollection<Post> CreatedPosts { get; set; }
        public virtual ICollection<Post> UpdatedPosts { get; set; }
        public virtual ICollection<Menu> CreatedMenus { get; set; }
        public virtual ICollection<Menu> UpdatedMenus { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("name=VNScienceDbContext")
        {
        }

        public virtual DbSet<About> Abouts { get; set; }
        public virtual DbSet<Branch> Branches { get; set; }
        public virtual DbSet<Contact> Contacts { get; set; }
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<MenuType> MenuTypes { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<PostCategory> PostCategories { get; set; }
        public virtual DbSet<Recruitment> Recruitments { get; set; }
        public virtual DbSet<SystemInfo> SystemInfoes { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<About>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<About>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Branch>()
                .Property(e => e.Homepage)
                .IsUnicode(false);

            modelBuilder.Entity<Branch>()
                .Property(e => e.Tel)
                .IsUnicode(false);

            modelBuilder.Entity<Branch>()
                .Property(e => e.Fax)
                .IsUnicode(false);

            modelBuilder.Entity<Branch>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Branch>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Contact>()
                .Property(e => e.Email)
                .IsUnicode(false);

            modelBuilder.Entity<Menu>()
                .Property(e => e.Link)
                .IsUnicode(false);

            modelBuilder.Entity<Menu>()
                .Property(e => e.Target)
                .IsUnicode(false);

            modelBuilder.Entity<Menu>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Menu>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Post>()
                .Property(e => e.MetaTitle)
                .IsUnicode(false);

            modelBuilder.Entity<Post>()
                .Property(e => e.Author)
                .IsUnicode(false);

            modelBuilder.Entity<Post>()
                .Property(e => e.CoverImage)
                .IsUnicode(false);

            modelBuilder.Entity<Post>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Post>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<PostCategory>()
                .Property(e => e.MetaTitle)
                .IsUnicode(false);

            modelBuilder.Entity<PostCategory>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<PostCategory>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Recruitment>()
                .Property(e => e.MetaTitle)
                .IsUnicode(false);

            modelBuilder.Entity<Recruitment>()
                .Property(e => e.CreatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Recruitment>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<SystemInfo>()
                .Property(e => e.Id)
                .IsUnicode(false);

            modelBuilder.Entity<SystemInfo>()
                .Property(e => e.UpdatedBy)
                .IsUnicode(false);

            modelBuilder.Entity<Tag>()
                .Property(e => e.Id)
                .IsUnicode(false);

            //user vs postCategort
            modelBuilder.Entity<PostCategory>()
                .HasOptional(e => e.UpdatingUser)
                .WithMany(e => e.UpdatedPostCategories)
                .HasForeignKey(e => e.UpdatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<PostCategory>()
                .HasOptional(e => e.CreatingUser)
                .WithMany(e => e.CreatedPostCategories)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Post>()
                .HasMany(e => e.Tags)
                .WithMany(e => e.Posts);

            //user vs post
            modelBuilder.Entity<Post>()
                 .HasOptional(e => e.UpdatingUser)
                 .WithMany(e => e.UpdatedPosts)
                 .HasForeignKey(e => e.UpdatedBy)
                 .WillCascadeOnDelete(false);

            modelBuilder.Entity<Post>()
                .HasOptional(e => e.CreatingUser)
                .WithMany(e => e.CreatedPosts)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);
            
            //post vs postcategory
            modelBuilder.Entity<Post>()
                .HasRequired(e => e.PostCategory)
                .WithMany(e => e.Posts)
                .HasForeignKey(e => e.CategoryId);

            //menu vs menutype
            modelBuilder.Entity<Menu>()
                .HasRequired(e => e.MenuType)
                .WithMany(e => e.Menus)
                .HasForeignKey(e => e.MenuTypeId);

            //user vs menu
            modelBuilder.Entity<Menu>()
                 .HasOptional(e => e.UpdatingUser)
                 .WithMany(e => e.UpdatedMenus)
                 .HasForeignKey(e => e.UpdatedBy)
                 .WillCascadeOnDelete(false);

            modelBuilder.Entity<Menu>()
                .HasOptional(e => e.CreatingUser)
                .WithMany(e => e.CreatedMenus)
                .HasForeignKey(e => e.CreatedBy)
                .WillCascadeOnDelete(false);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}