using System;
using Microsoft.EntityFrameworkCore;
using FsInfoCat.Models;

namespace FsInfoCat.Web.Data
{
    public class FsInfoDataContext : DbContext
    {
        public FsInfoDataContext(DbContextOptions<FsInfoDataContext> options)
            : base(options)
        {
        }

        public DbSet<RegisteredUser> RegisteredUser { get; set; }

        public DbSet<MediaHost> MediaHost { get; set; }

        public DbSet<MediaVolume> MediaVolume { get; set; }
    }
}
