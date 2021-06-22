using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    public partial class LocalDbContext : DbContext, ILocalDbContext
    {
        private static readonly object _syncRoot = new();
        private static bool _connectionStringValidated;
        private readonly ILogger<LocalDbContext> _logger;
        private readonly IDisposable _loggerScope;

        public virtual DbSet<FileSystem> FileSystems { get; set; }

        public virtual DbSet<SymbolicName> SymbolicNames { get; set; }

        public virtual DbSet<Volume> Volumes { get; set; }

        public virtual DbSet<VolumeAccessError> VolumeAccessErrors { get; set; }

        public virtual DbSet<Subdirectory> Subdirectories { get; set; }

        public virtual DbSet<SubdirectoryAccessError> SubdirectoryAccessErrors { get; set; }

        public virtual DbSet<DbFile> Files { get; set; }

        public virtual DbSet<FileAccessError> FileAccessErrors { get; set; }

        [Obsolete]
        public virtual DbSet<ExtendedProperties> ExtendedProperties { get; set; }

        public virtual DbSet<SummaryPropertySet> SummaryPropertySets { get; }

        public virtual DbSet<DocumentPropertySet> DocumentPropertySets { get; }

        public virtual DbSet<AudioPropertySet> AudioPropertySets { get; }

        public virtual DbSet<DRMPropertySet> DRMPropertySets { get; }

        public virtual DbSet<GPSPropertySet> GPSPropertySets { get; }

        public virtual DbSet<ImagePropertySet> ImagePropertySets { get; }

        public virtual DbSet<MediaPropertySet> MediaPropertySets { get; }

        public virtual DbSet<MusicPropertySet> MusicPropertySets { get; }

        public virtual DbSet<PhotoPropertySet> PhotoPropertySets { get; }

        public virtual DbSet<RecordedTVPropertySet> RecordedTVPropertySets { get; }

        public virtual DbSet<VideoPropertySet> VideoPropertySets { get; }

        public virtual DbSet<BinaryPropertySet> BinaryPropertySets { get; set; }

        public virtual DbSet<FileComparison> Comparisons { get; set; }

        public virtual DbSet<RedundantSet> RedundantSets { get; set; }

        public virtual DbSet<Redundancy> Redundancies { get; set; }

        public virtual DbSet<CrawlConfiguration> CrawlConfigurations { get; set; }

        public LocalDbContext(DbContextOptions<LocalDbContext> options)
            : base(options)
        {
            _logger = Services.ServiceProvider.GetRequiredService<ILogger<LocalDbContext>>();
            _loggerScope = _logger.BeginScope(ContextId);
            _logger.LogInformation($"Creating new {nameof(LocalDbContext)}: {nameof(DbContextId.InstanceId)}={{{nameof(DbContextId.InstanceId)}}}, {nameof(DbContextId.Lease)}={{{nameof(DbContextId.Lease)}}}",
                ContextId.InstanceId, ContextId.Lease);
            lock (_syncRoot)
            {
                if (!_connectionStringValidated)
                {
                    _connectionStringValidated = true;
                    SqliteConnectionStringBuilder builder = new(Database.GetConnectionString());
                    string connectionString = builder.ConnectionString;
                    _logger.LogInformation($"Using {nameof(SqliteConnectionStringBuilder.ConnectionString)} {{{nameof(SqliteConnectionStringBuilder.ConnectionString)}}}",
                        connectionString);
                    if (!File.Exists(builder.DataSource))
                    {
                        builder.Mode = SqliteOpenMode.ReadWriteCreate;
                        _logger.LogInformation("Initializing new database");
                        using SqliteConnection connection = new(builder.ConnectionString);
                        connection.Open();
                        foreach (var element in XDocument.Parse(Properties.Resources.DbCommands).Root.Elements("DbCreation").Elements("Text"))
                        {
                            _logger.LogInformation($"{{Message}}; {nameof(SqliteCommand)}={{{nameof(SqliteCommand.CommandText)}}}",
                                element.Attributes("Message").Select(a => a.Value).DefaultIfEmpty("").First(), element.Value);
                            using SqliteCommand command = connection.CreateCommand();
                            command.CommandText = element.Value;
                            command.CommandType = System.Data.CommandType.Text;
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }
        }

        [SuppressMessage("Usage", "CA1816:Dispose methods should call SuppressFinalize", Justification = "Inherited class will have called SuppressFinalize if necessary.")]
        public override void Dispose()
        {
            _logger.LogInformation($"Disposing {nameof(LocalDbContext)}: {nameof(DbContextId.InstanceId)}={{{nameof(DbContextId.InstanceId)}}}, {nameof(DbContextId.Lease)}={{{nameof(DbContextId.Lease)}}}",
                ContextId.InstanceId, ContextId.Lease);
            base.Dispose();
            _loggerScope.Dispose();
        }

        public override int SaveChanges()
        {
            var entries = ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);
            foreach (var e in entries)
            {
                ValidationContext validationContext = new(e.Entity, new DbContextServiceProvider(this, e), null);
                if (e.Entity is IDbEntity dbEntity)
                    dbEntity.BeforeSave(validationContext);
                Validator.ValidateObject(e.Entity, validationContext, true);
            }
            int result = base.SaveChanges();
            foreach (var e in entries)
            {
                if (e.State == EntityState.Unchanged && e.Entity is IDbEntity dbEntity)
                    dbEntity.AcceptChanges();

            }
            return result;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            //modelBuilder.Entity<FileSystem>(FileSystem.BuildEntity);
            modelBuilder.Entity<SymbolicName>(SymbolicName.BuildEntity);
            modelBuilder.Entity<Volume>(Volume.BuildEntity);
            modelBuilder.Entity<Subdirectory>(Subdirectory.BuildEntity);
            modelBuilder.Entity<DbFile>(DbFile.BuildEntity);
            modelBuilder.Entity<BinaryPropertySet>(Local.BinaryPropertySet.BuildEntity);
            modelBuilder.Entity<FileComparison>(FileComparison.BuildEntity);
            modelBuilder.Entity<RedundantSet>(RedundantSet.BuildEntity);
            modelBuilder.Entity<Redundancy>(Redundancy.BuildEntity);
        }

        public static void ConfigureServices(IServiceCollection services, Assembly assembly, string dbFileName) =>
            ConfigureServices(services, GetDbFilePath(assembly, dbFileName));

        public static void ConfigureServices(IServiceCollection services, string dbPath)
        {
            string connectionString = GetConnectionString(dbPath);
            services.AddDbContextPool<LocalDbContext>(options =>
            {
                options.AddInterceptors(Interceptor.Instance);
                options.UseSqlite(connectionString);
            });
            // this.Model.GetDefaultSchema();
        }

        public static string GetConnectionString(Assembly assembly, string dbFileName) => GetConnectionString(GetDbFilePath(assembly, dbFileName));

        public static string GetConnectionString(string dbPath)
        {
            var builder = new SqliteConnectionStringBuilder
            {
                DataSource = dbPath,
                ForeignKeys = true,
                Mode = SqliteOpenMode.ReadWrite
            };
            return builder.ConnectionString;
        }

        public static string GetDbFilePath(Assembly assembly, string dbFileName)
        {
            if (string.IsNullOrWhiteSpace(dbFileName))
                dbFileName = Services.DEFAULT_LOCAL_DB_FILENAME;
            if (Path.IsPathFullyQualified(dbFileName))
                return Path.GetFullPath(dbFileName);
            return Path.Combine(Services.GetAppDataPath(assembly), dbFileName);
        }

        #region Explicit Members

        IEnumerable<ILocalComparison> ILocalDbContext.Comparisons => Comparisons.Cast<ILocalComparison>();

        IEnumerable<ILocalBinaryPropertySet> ILocalDbContext.BinaryPropertySets => BinaryPropertySets.Cast<ILocalBinaryPropertySet>();

        [Obsolete]
        IEnumerable<ILocalExtendedProperties> ILocalDbContext.ExtendedProperties => ExtendedProperties.Cast<ILocalExtendedProperties>();

        IEnumerable<IAccessError<ILocalFile>> ILocalDbContext.FileAccessErrors => FileAccessErrors.Cast<IAccessError<ILocalFile>>();

        IEnumerable<ILocalFile> ILocalDbContext.Files => Files.Cast<ILocalFile>();

        IEnumerable<ILocalFileSystem> ILocalDbContext.FileSystems => FileSystems.Cast<ILocalFileSystem>();

        IEnumerable<ILocalRedundancy> ILocalDbContext.Redundancies => Redundancies.Cast<ILocalRedundancy>();

        IEnumerable<ILocalRedundantSet> ILocalDbContext.RedundantSets => RedundantSets.Cast<ILocalRedundantSet>();

        IEnumerable<ILocalSubdirectory> ILocalDbContext.Subdirectories => Subdirectories.Cast<ILocalSubdirectory>();

        IEnumerable<IAccessError<ILocalSubdirectory>> ILocalDbContext.SubdirectoryAccessErrors => SubdirectoryAccessErrors.Cast<IAccessError<ILocalSubdirectory>>();

        IEnumerable<ILocalSymbolicName> ILocalDbContext.SymbolicNames => SymbolicNames.Cast<ILocalSymbolicName>();

        IEnumerable<IAccessError<ILocalVolume>> ILocalDbContext.VolumeAccessErrors => VolumeAccessErrors.Cast<IAccessError<ILocalVolume>>();

        IEnumerable<ILocalVolume> ILocalDbContext.Volumes => Volumes.Cast<ILocalVolume>();

        IEnumerable<ILocalCrawlConfiguration> ILocalDbContext.CrawlConfigurations => CrawlConfigurations.Cast<ILocalCrawlConfiguration>();

        IEnumerable<IComparison> IDbContext.Comparisons => Comparisons.Cast<IComparison>();

        IEnumerable<IBinaryPropertySet> IDbContext.BinaryPropertySets => BinaryPropertySets.Cast<IBinaryPropertySet>();

        [Obsolete]
        IEnumerable<IExtendedProperties> IDbContext.ExtendedProperties => ExtendedProperties.Cast<IExtendedProperties>();

        IEnumerable<IAccessError<IFile>> IDbContext.FileAccessErrors => FileAccessErrors.Cast<IAccessError<IFile>>();

        IEnumerable<IFile> IDbContext.Files => Files.Cast<IFile>();

        IEnumerable<IFileSystem> IDbContext.FileSystems => FileSystems.Cast<IFileSystem>();

        IEnumerable<IRedundancy> IDbContext.Redundancies => Redundancies.Cast<IRedundancy>();

        IEnumerable<IRedundantSet> IDbContext.RedundantSets => RedundantSets.Cast<IRedundantSet>();

        IEnumerable<ISubdirectory> IDbContext.Subdirectories => Subdirectories.Cast<ISubdirectory>();

        IEnumerable<IAccessError<ISubdirectory>> IDbContext.SubdirectoryAccessErrors => SubdirectoryAccessErrors.Cast<IAccessError<ISubdirectory>>();

        IEnumerable<ISymbolicName> IDbContext.SymbolicNames => SymbolicNames.Cast<ISymbolicName>();

        IEnumerable<IAccessError<IVolume>> IDbContext.VolumeAccessErrors => VolumeAccessErrors.Cast<IAccessError<IVolume>>();

        IEnumerable<IVolume> IDbContext.Volumes => Volumes.Cast<IVolume>();

        IEnumerable<ICrawlConfiguration> IDbContext.CrawlConfigurations => CrawlConfigurations.Cast<ICrawlConfiguration>();

        IEnumerable<ILocalSummaryProperties> ILocalDbContext.SummaryPropertySets => SummaryPropertySets.Cast<ILocalSummaryProperties>();

        IEnumerable<ILocalDocumentPropertySet> ILocalDbContext.DocumentPropertySets => DocumentPropertySets.Cast<ILocalDocumentPropertySet>();

        IEnumerable<ILocalAudioPropertySet> ILocalDbContext.AudioPropertySets => AudioPropertySets.Cast<ILocalAudioPropertySet>();

        IEnumerable<ILocalDRMPropertySet> ILocalDbContext.DRMPropertySets => DRMPropertySets.Cast<ILocalDRMPropertySet>();

        IEnumerable<ILocalGPSPropertySet> ILocalDbContext.GPSPropertySets => GPSPropertySets.Cast<ILocalGPSPropertySet>();

        IEnumerable<ILocalImagePropertySet> ILocalDbContext.ImagePropertySets => ImagePropertySets.Cast<ILocalImagePropertySet>();

        IEnumerable<ILocalMediaPropertySet> ILocalDbContext.MediaPropertySets => MediaPropertySets.Cast<ILocalMediaPropertySet>();

        IEnumerable<ILocalMusicPropertySet> ILocalDbContext.MusicPropertySets => MusicPropertySets.Cast<ILocalMusicPropertySet>();

        IEnumerable<ILocalPhotoPropertySet> ILocalDbContext.PhotoPropertySets => PhotoPropertySets.Cast<ILocalPhotoPropertySet>();

        IEnumerable<ILocalRecordedTVPropertySet> ILocalDbContext.RecordedTVPropertySets => RecordedTVPropertySets.Cast<ILocalRecordedTVPropertySet>();

        IEnumerable<ILocalVideoPropertySet> ILocalDbContext.VideoPropertySets => VideoPropertySets.Cast<ILocalVideoPropertySet>();

        IEnumerable<ISummaryPropertySet> IDbContext.SummaryPropertySets => SummaryPropertySets.Cast<ISummaryPropertySet>();

        IEnumerable<IDocumentPropertySet> IDbContext.DocumentPropertySets => DocumentPropertySets.Cast<IDocumentPropertySet>();

        IEnumerable<IAudioPropertySet> IDbContext.AudioPropertySets => AudioPropertySets.Cast<IAudioPropertySet>();

        IEnumerable<IDRMPropertySet> IDbContext.DRMPropertySets => DRMPropertySets.Cast<IDRMPropertySet>();

        IEnumerable<IGPSPropertySet> IDbContext.GPSPropertySets => GPSPropertySets.Cast<IGPSPropertySet>();

        IEnumerable<IImagePropertySet> IDbContext.ImagePropertySets => ImagePropertySets.Cast<IImagePropertySet>();

        IEnumerable<IMediaPropertySet> IDbContext.MediaPropertySets => MediaPropertySets.Cast<IMediaPropertySet>();

        IEnumerable<IMusicPropertySet> IDbContext.MusicPropertySets => MusicPropertySets.Cast<IMusicPropertySet>();

        IEnumerable<IPhotoPropertySet> IDbContext.PhotoPropertySets => PhotoPropertySets.Cast<IPhotoPropertySet>();

        IEnumerable<IRecordedTVPropertySet> IDbContext.RecordedTVPropertySets => RecordedTVPropertySets.Cast<IRecordedTVPropertySet>();

        IEnumerable<IVideoPropertySet> IDbContext.VideoPropertySets => VideoPropertySets.Cast<IVideoPropertySet>();

        void ILocalDbContext.ForceDeleteBinaryProperties(ILocalBinaryPropertySet target) => ForceDeleteBinaryProperties((BinaryPropertySet)target);

        void ILocalDbContext.ForceDeleteRedundantSet(ILocalRedundantSet target) => ForceDeleteRedundantSet((RedundantSet)target);

        void ILocalDbContext.ForceDeleteFileSystem(ILocalFileSystem target) => ForceDeleteFileSystem((FileSystem)target);

        void IDbContext.ForceDeleteBinaryProperties(IBinaryPropertySet target) => ForceDeleteBinaryProperties((BinaryPropertySet)target);

        void IDbContext.ForceDeleteRedundantSet(IRedundantSet target) => ForceDeleteRedundantSet((RedundantSet)target);

        #endregion
    }
}
