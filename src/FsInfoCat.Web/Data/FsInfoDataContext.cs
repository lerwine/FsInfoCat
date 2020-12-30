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
            modelBuilder.Entity<WebHostContributor>().HasNoKey();
        }

        public DbSet<Account> Account { get; set; }

        public DbSet<HostDevice> HostDevice { get; set; }

        public DbSet<WebHostContributor> WebHostContributor { get; set; }

        public DbSet<Volume> Volume { get; set; }

        // public DbSet<FsInfoCat.Models.Volume> Volume { get; set; }
    }
}
