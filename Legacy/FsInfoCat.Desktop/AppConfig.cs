using FsInfoCat.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;

namespace FsInfoCat.Desktop
{
    class AppConfig
    {
        internal const string DEFAULT_LOCAL_DB_FILENAME = "FsInfoCat";
        internal const string DEFAULT_REMOTE_SQL_CONNECTION_STRING = "Initial Catalog=FsInfoCat;Persist Security Info=True;MultipleActiveResultSets=True;Connect Timeout=30;Encrypt=True;Application Name=FsInfoCat.Desktop";
        internal const string DEFAULT_REMOTE_EDM_METADATA = "res://*/Model.Remote.RemoteDbModel.csdl|res://*/Model.Remote.RemoteDbModel.ssdl|res://*/Model.Remote.RemoteDbModel.msl";
        internal const string DEFAULT_LOCAL_EDM_METADATA = "res://*/Model.Local.LocalDbContainer.csdl|res://*/Model.Local.LocalDbContainer.ssdl|res://*/Model.Local.LocalDbContainer.msl";

        private static readonly ILogger<AppConfig> _logger = Services.GetLoggingService().CreateLogger<AppConfig>();

        public static string GetProviderFactoryInvariantName<TProvider>()
            where TProvider : DbProviderFactory
        {
            System.Data.DataRow[] dataRows = DbProviderFactories.GetFactoryClasses().Select($"[{DbConstants.ProviderFactories_AssemblyQualifiedName}]='{typeof(TProvider).AssemblyQualifiedName}'");
            return (dataRows.Length > 0) ? dataRows[0][DbConstants.ProviderFactories_InvariantName] as string : null;
        }

        public static bool TryGetProviderFactoryInvariantName<TProvider>(out string invariantName)
            where TProvider : DbProviderFactory
        {
            System.Data.DataRow[] dataRows = DbProviderFactories.GetFactoryClasses().Select($"[{DbConstants.ProviderFactories_AssemblyQualifiedName}]='{typeof(TProvider).AssemblyQualifiedName}'");
            if (dataRows.Length > 0)
            {
                invariantName = dataRows[0][DbConstants.ProviderFactories_InvariantName] as string;
                return true;
            }
            _logger.LogWarning($"No registered provider factory found for type {{{nameof(Type.AssemblyQualifiedName)}}}", typeof(TProvider).AssemblyQualifiedName);
            invariantName = null;
            return false;
        }

        public static bool TryGetConnectionStringBuilder(string name, out DbConnectionStringBuilder connectionStringBuilder, out string providerName)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentOutOfRangeException(nameof(name));
            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[name];
            if (connectionStringSettings is null)
            {
                _logger.LogWarning($"No connection string named \"{{{nameof(ConnectionStringSettings.Name)}}}\" found.", name);
                connectionStringBuilder = null;
                providerName = null;
                return false;
            }
            providerName = connectionStringSettings.ProviderName;
            if (string.IsNullOrWhiteSpace(connectionStringSettings.ConnectionString))
                _logger.LogWarning($"{nameof(ConnectionStringSettings)} \"{{{nameof(ConnectionStringSettings.Name)}}}\" does not have a {nameof(ConnectionStringSettings.ConnectionString)} specified.", name);
            else
            {
                DbProviderFactory providerFactory;
                if (string.IsNullOrWhiteSpace(providerName))
                {
                    _logger.LogWarning($"{nameof(ConnectionStringSettings)} \"{{{nameof(ConnectionStringSettings.Name)}}}\" does not have a {nameof(ConnectionStringSettings.ProviderName)} specified.",
                        name);
                    TryGetProviderFactoryInvariantName<SqlClientFactory>(out providerName);
                    providerFactory = SqlClientFactory.Instance;
                }
                else
                    try { providerFactory = DbProviderFactories.GetFactory(providerName); }
                    catch (Exception exception)
                    {
                        _logger.LogCritical(exception, $"Failed to find a database provider factory for {nameof(ConnectionStringSettings)} \"{{{nameof(ConnectionStringSettings.Name)}}}\" using provider name \"{{{nameof(ConnectionStringSettings.ProviderName)}}}\".",
                            name, providerName);
                        connectionStringBuilder = null;
                        return false;
                    }
                try
                {
                    (connectionStringBuilder = providerFactory.CreateConnectionStringBuilder()).ConnectionString = connectionStringSettings.ConnectionString;
                    return true;
                }
                catch (Exception exception)
                {
                    if (string.IsNullOrWhiteSpace(providerName))
                        _logger.LogCritical(exception, $"Failed to parse {nameof(ConnectionStringSettings.ConnectionString)} for {nameof(ConnectionStringSettings)} \"{{{nameof(ConnectionStringSettings.Name)}}}\".",
                            name);
                    else
                        _logger.LogCritical(exception, $"Failed to parse {nameof(ConnectionStringSettings.ConnectionString)} for {nameof(ConnectionStringSettings)} \"{{{nameof(ConnectionStringSettings.Name)}}}\" using the \"{{{nameof(ConnectionStringSettings.ProviderName)}}}\" database provider.",
                            name, providerName);
                }
            }
            connectionStringBuilder = null;
            return false;
        }

        private static void SetConnectionString(Configuration configuration, string name, string connectionString, string providerName, bool doNotRefreshConfigurationManager = false)
        {
            ConnectionStringSettings connectionStringSettings = configuration.ConnectionStrings.ConnectionStrings[name];
            if (connectionStringSettings is null)
            {
                connectionStringSettings = new ConnectionStringSettings(name, connectionString, providerName);
                ConfigurationManager.ConnectionStrings.Add(connectionStringSettings);
            }
            if (!doNotRefreshConfigurationManager)
                ConfigurationManager.RefreshSection(configuration.ConnectionStrings.SectionInformation.Name);
        }

        public static bool TestProviderFactoryName<TProvider>(string invariantName)
            where TProvider : DbProviderFactory
        {
            return DbProviderFactories.GetFactoryClasses().Select($"[AssemblyQualifiedName]='{typeof(TProvider).AssemblyQualifiedName}' AND [InvariantName]='{invariantName}'").Length > 0;
        }

        public static bool TryGetUpstreamDbConnectionStringBuilder(out DbConnectionStringBuilder connectionStringBuilder, out string providerName)
        {
            if (TryGetConnectionStringBuilder($"{typeof(Properties.Settings).FullName}.{nameof(Properties.Settings.UpstreamDb)}", out connectionStringBuilder, out providerName))
                return true;
            connectionStringBuilder = new SqlConnectionStringBuilder(Properties.Settings.Default.UpstreamDb);
            return false;
        }

        internal static void SetUpstreamDbContainerConnectionString(string connectionString, bool doNotRefreshConfigurationManager = false)
        {
            SetConnectionString(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal),
                $"{typeof(Properties.Settings).FullName}.{nameof(Properties.Settings.UpstreamDb)}",
                GetProviderFactoryInvariantName<SqlClientFactory>(), connectionString, doNotRefreshConfigurationManager);
        }

        [Obsolete("Use TryGetUpstreamDbConnectionStringBuilder")]
        public static SqlConnectionStringBuilder GetRemoteDbContainerConnectionStringBuilder()
        {
            throw new NotImplementedException();
            //string name = nameof(Model.Remote.RemoteDbContainer);
            //string connectionString = GetConnectionString(name, out string providerName);
            //if (string.IsNullOrWhiteSpace(connectionString))
            //    return new SqlConnectionStringBuilder(DEFAULT_REMOTE_SQL_CONNECTION_STRING);
            //EntityConnectionStringBuilder builder = new EntityConnectionStringBuilder(connectionString);
            //if (!(string.IsNullOrWhiteSpace(providerName) || TestProviderFactoryName<SqlClientFactory>(builder.Provider)))
            //    throw new InvalidOperationException($"Unsupported data provider type ({providerName})");
            //return new SqlConnectionStringBuilder(string.IsNullOrWhiteSpace(builder.ProviderConnectionString) ? DEFAULT_REMOTE_SQL_CONNECTION_STRING : builder.ProviderConnectionString);
        }

        [Obsolete("Use SetUpstreamDbContainerConnectionString")]
        internal static void SetRemoteDbContainerConnectionString(string connectionString, bool doNotRefreshConfigurationManager = false)
        {
            throw new NotImplementedException();
            //SetConnectionString(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal), nameof(Model.Remote.RemoteDbContainer),
            //    GetProviderFactoryInvariantName<SqlClientFactory>(), connectionString, doNotRefreshConfigurationManager);
        }
    }
}
