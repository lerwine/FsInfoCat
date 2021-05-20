using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.Sqlite;

namespace FsInfoCat.Local
{
    public class LocalDbContext : DbContext
    {
        public virtual DbSet<SymbolicName> SymbolicNames { get; set; }

        public virtual DbSet<FileSystem> FileSystems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Entity<SymbolicName>(SymbolicName.BuildEntity);
            modelBuilder.Entity<FileSystem>(FileSystem.BuildEntity);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            string path = @"C:\Users\lerwi\AppData\Local\FsInfoCat.Desktop\1.0.0.0\test.db";
            SqliteConnectionStringBuilder connectionStringBuilder = new SqliteConnectionStringBuilder();
            connectionStringBuilder.DataSource = path;
            string connectionString = connectionStringBuilder.ConnectionString;
            if (!File.Exists(path))
            {
                connectionStringBuilder.Mode = SqliteOpenMode.ReadWriteCreate;
                using (SqliteConnection connection = new SqliteConnection(connectionStringBuilder.ConnectionString))
                {
                    connection.Open();
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "CREATE TABLE [SymbolicNames] ([Id] uniqueidentifier PRIMARY KEY NOT NULL,[Name] nvarchar(256)  NOT NULL)";
                        command.ExecuteNonQuery();
                    }
                    using (var command = connection.CreateCommand())
                    {
                        command.CommandText = "CREATE TABLE [FileSystems] ([Id] uniqueidentifier PRIMARY KEY NOT NULL,[DisplayName] nvarchar(1024)  NOT NULL, [DefaultSymbolicNameId] uniqueidentifier CONSTRAINT 'FK_FileSystemSymbolicName' REFERENCES [SymbolicNames] ([Id]))";
                        command.ExecuteNonQuery();
                    }
                }
            }
            optionsBuilder.UseSqlite(connectionString);
        }
    }
}
