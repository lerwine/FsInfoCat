using System;
using Microsoft.EntityFrameworkCore;
using FsInfoCat.Models.DB;

namespace FsInfoCat.Web.Data
{
    public class FsInfoDataContext : DbContext
    {
        public FsInfoDataContext(DbContextOptions<FsInfoDataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Account>()
                .HasOne(p => p.Creator)
                .WithMany()
                .HasForeignKey(p => p.CreatedBy);
            modelBuilder.Entity<Account>()
                .HasOne(p => p.Modifier)
                .WithMany()
                .HasForeignKey(p => p.CreatedBy);
            modelBuilder.Entity<HostDevice>()
                .HasOne(p => p.Creator)
                .WithMany()
                .HasForeignKey(p => p.CreatedBy);
            modelBuilder.Entity<HostDevice>()
                .HasOne(p => p.Modifier)
                .WithMany()
                .HasForeignKey(p => p.CreatedBy);
            modelBuilder.Entity<Volume>()
                .HasOne(p => p.Creator)
                .WithMany()
                .HasForeignKey(p => p.CreatedBy);
            modelBuilder.Entity<Volume>()
                .HasOne(p => p.Modifier)
                .WithMany()
                .HasForeignKey(p => p.CreatedBy);
            // modelBuilder.Entity<HostContributor>().HasNoKey();
            modelBuilder.Entity<HostContributor>()
                .HasKey(c => new { c.AccountID, c.HostDeviceID });
            modelBuilder.Entity<HostContributor>()
                .HasOne(p => p.Creator)
                .WithMany()
                .HasForeignKey(p => p.CreatedBy);
        }

        public DbSet<Account> Account { get; set; }

        public DbSet<HostDevice> HostDevice { get; set; }

        public DbSet<HostContributor> HostContributor { get; set; }

        public DbSet<Volume> Volume { get; set; }

        // public DbSet<FsInfoCat.Models.Volume> Volume { get; set; }
    }
}
