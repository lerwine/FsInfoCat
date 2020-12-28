using System;
using Microsoft.EntityFrameworkCore;
using FsInfoCat.Models.DB;

namespace FsInfoCat.WebApp.Data
{
    public class FsInfoDataContext : DbContext
    {
        public FsInfoDataContext(DbContextOptions<FsInfoDataContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Account { get; set; }

        public DbSet<HostDevice> HostDevice { get; set; }

        public DbSet<Volume> Volume { get; set; }
    }
}
