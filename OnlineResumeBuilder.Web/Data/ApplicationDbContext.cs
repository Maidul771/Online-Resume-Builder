using Microsoft.EntityFrameworkCore;
using OnlineResumeBuilder.Web.Models;

namespace OnlineResumeBuilder.Web.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserInfo> UserInfos { get; set; }
        public DbSet<Objective> Objectives { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Education> Educations { get; set; }
        public DbSet<Certification> Certifications { get; set; }
        public DbSet<Reference> References { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure UserInfo as the principal entity
            modelBuilder.Entity<UserInfo>()
                .HasMany(u => u.Objectives)
                .WithOne(o => o.UserInfo)
                .HasForeignKey(o => o.UserInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserInfo>()
                .HasMany(u => u.Skills)
                .WithOne(s => s.UserInfo)
                .HasForeignKey(s => s.UserInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserInfo>()
                .HasMany(u => u.Experiences)
                .WithOne(e => e.UserInfo)
                .HasForeignKey(e => e.UserInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserInfo>()
                .HasMany(u => u.Projects)
                .WithOne(p => p.UserInfo)
                .HasForeignKey(p => p.UserInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserInfo>()
                .HasMany(u => u.Educations)
                .WithOne(e => e.UserInfo)
                .HasForeignKey(e => e.UserInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserInfo>()
                .HasMany(u => u.Certifications)
                .WithOne(c => c.UserInfo)
                .HasForeignKey(c => c.UserInfoId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<UserInfo>()
                .HasMany(u => u.References)
                .WithOne(r => r.UserInfo)
                .HasForeignKey(r => r.UserInfoId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
} 