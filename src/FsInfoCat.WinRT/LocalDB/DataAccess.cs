using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Windows.Storage;

namespace FsInfoCat.LocalDB
{
    public partial class DataAccess : DbContext
    {
        // TODO: Use app setting, instead
        private const string DB_FOLDER_NAME = "AppData";
        private const string DB_NAME = "FsInfoCat.db";
        private const string INIT_NAME = "FsInfoCat.xml";
        private const string ELEMENT_NAME_Init = "Init";
        private const string ELEMENT_NAME_Command = "Command";

        private static readonly object _syncRoot = new object();
        private readonly ILogger<DataAccess> _logger;
        private static Task<string> _initDbTask;

        public virtual DbSet<CrawlConfiguration> CrawlConfigurations { get; set; }

        public DataAccess(DbContextOptions<DataAccess> options)
            : base(options)
        {
            _logger = App.GetRequiredService<ILogger<DataAccess>>();
            _logger.LogInformation($"Instantiating {nameof(DataAccess)}: {nameof(DbContextId.InstanceId)}={{{nameof(DbContextId.InstanceId)}}}, {nameof(DbContextId.Lease)}={{{nameof(DbContextId.Lease)}}}; ConnectionString={{ConnectionString}}",
                ContextId.InstanceId, ContextId.Lease, Database.GetDbConnection().ConnectionString);
        }

        private static async Task InitializeDatabaseAsync(XDocument initCommands, SqliteConnectionStringBuilder builder, CancellationToken cancellationToken)
        {
            builder.Mode = SqliteOpenMode.ReadWriteCreate;
            using (SqliteConnection connection = new SqliteConnection(builder.ConnectionString))
            {
                await connection.OpenAsync(cancellationToken);
                foreach (XElement commandElement in initCommands.Root.Elements(ELEMENT_NAME_Init).Elements(ELEMENT_NAME_Command))
                {
                    using (SqliteCommand sqliteCommand = new SqliteCommand(commandElement.Value, connection))
                    {
                        string sql = commandElement.Value.Trim();
                        sqliteCommand.CommandText = sql;
                        sqliteCommand.CommandType = System.Data.CommandType.Text;
                        try { _ = await sqliteCommand.ExecuteNonQueryAsync(cancellationToken); }
                        catch (Exception exception)
                        {
                            throw new Exception($"Error executing query '{sql}': {exception.Message}");
                        }
                    }
                }
            }
        }

        public static async Task<StorageFolder> GetAppDataFolderAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return await ApplicationData.Current.LocalFolder.CreateFolderAsync(DB_FOLDER_NAME, CreationCollisionOption.OpenIfExists);
        }

        private static async Task<string> GetConnectionStringAsync(Task<StorageFolder> getAppDataFolder, CancellationToken cancellationToken)
        {
            Windows.System.
            StorageFolder appDataFolder = await getAppDataFolder;
            SqliteConnectionStringBuilder builder = new SqliteConnectionStringBuilder();
            StorageFile file;
            try { file = await appDataFolder.GetFileAsync(DB_NAME); }
            catch { file = null; }
            if (file is null)
            {
                builder.DataSource = Path.Combine(appDataFolder.Path, DB_NAME);
                string connectionString = builder.ConnectionString;
                file = await StorageFile.GetFileFromApplicationUriAsync(new Uri($"ms-appx:///{DB_FOLDER_NAME}/{INIT_NAME}"));
                using (Stream stream = await file.OpenStreamForReadAsync())
                {
                    XDocument initCommands = await XDocument.LoadAsync(stream, LoadOptions.None, cancellationToken);
                    await InitializeDatabaseAsync(initCommands, builder, cancellationToken);
                }
                return connectionString;
            }
            builder.DataSource = file.Path;
            return builder.ConnectionString;
        }

        public static Task<string> GetConnectionStringAsync(CancellationToken cancellationToken)
        {
            lock (_syncRoot)
            {
                if (_initDbTask is null)
                    _initDbTask = GetConnectionStringAsync(GetAppDataFolderAsync(cancellationToken), cancellationToken);
            }
            return _initDbTask;
        }

        [ConfigureServicesHandler()]
        private static void ConfigureServices(IServiceCollection services)
        {
            string connectionString = GetConnectionStringAsync(CancellationToken.None).Result;
            _ = services.AddDbContextPool<DataAccess>(options => options.AddInterceptors(new Interceptor()).UseSqlite(connectionString));
        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder
        //        .Entity<CrawlConfiguration>(CrawlConfiguration.OnBuildEntity);
        //}

        public override void Dispose()
        {
            _logger.LogInformation($"Disposing {nameof(DataAccess)}: {nameof(DbContextId.InstanceId)}={{{nameof(DbContextId.InstanceId)}}}, {nameof(DbContextId.Lease)}={{{nameof(DbContextId.Lease)}}}",
                ContextId.InstanceId, ContextId.Lease);
            base.Dispose();
        }
    }
}
