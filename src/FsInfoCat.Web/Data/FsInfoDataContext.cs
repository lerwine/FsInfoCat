using FsInfoCat.Models.DB;
using Microsoft.EntityFrameworkCore;

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
                .HasForeignKey(p => p.ModifiedBy);
            modelBuilder.Entity<Account>()
                .HasOne(a => a.UserCredential)
                .WithOne(c => c.Account)
                .HasForeignKey<UserCredential>(c => c.AccountID);
            modelBuilder.Entity<UserCredential>()
                .HasOne(p => p.Creator)
                .WithMany()
                .HasForeignKey(p => p.CreatedBy);
            modelBuilder.Entity<UserCredential>()
                .HasOne(p => p.Modifier)
                .WithMany()
                .HasForeignKey(p => p.ModifiedBy);
            modelBuilder.Entity<HostDevice>()
                .HasOne(p => p.Creator)
                .WithMany()
                .HasForeignKey(p => p.CreatedBy);
            modelBuilder.Entity<HostDevice>()
                .HasOne(p => p.Modifier)
                .WithMany()
                .HasForeignKey(p => p.ModifiedBy);
            modelBuilder.Entity<Volume>()
                .HasOne(p => p.Creator)
                .WithMany()
                .HasForeignKey(p => p.CreatedBy);
            modelBuilder.Entity<Volume>()
                .HasOne(p => p.Modifier)
                .WithMany()
                .HasForeignKey(p => p.ModifiedBy);
            modelBuilder.Entity<Volume>()
                .HasOne(p => p.Host)
                .WithMany()
                .HasForeignKey(p => p.HostDeviceID);
        }

        public DbSet<UserCredential> UserCredential { get; set; }

        public DbSet<Account> Account { get; set; }

        public DbSet<HostDevice> HostDevice { get; set; }

        public DbSet<Volume> Volume { get; set; }

        // public DbSet<FsInfoCat.Models.Volume> Volume { get; set; }
    }
}
