using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;
using System.Data.SqlServerCe;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop
{
    class AppConfig
    {
        internal const string DEFAULT_LOCAL_DB_FILENAME = "FsInfoCat";
        internal const string DEFAULT_REMOTE_SQL_CONNECTION_STRING = "Initial Catalog=FsInfoCat;Persist Security Info=True;MultipleActiveResultSets=True;Connect Timeout=30;Encrypt=True;Application Name=FsInfoCat.Desktop";
        internal const string DEFAULT_REMOTE_EDM_METADATA = "res://*/Model.Remote.RemoteDbModel.csdl|res://*/Model.Remote.RemoteDbModel.ssdl|res://*/Model.Remote.RemoteDbModel.msl";
        internal const string DEFAULT_LOCAL_EDM_METADATA = "res://*/Model.Local.LocalDbContainer.csdl|res://*/Model.Local.LocalDbContainer.ssdl|res://*/Model.Local.LocalDbContainer.msl";

        private static string GetConnectionString(string name, out string providerName)
        {
            ConnectionStringSettings connectionStringSettings = ConfigurationManager.ConnectionStrings[name];
            if (connectionStringSettings is null)
            {
                providerName = null;
                return null;
            }
            providerName = string.IsNullOrWhiteSpace(connectionStringSettings.ProviderName) ? GetProviderFactoryInvariantName<SqlClientFactory>() :
                connectionStringSettings.ProviderName;
            return connectionStringSettings.ConnectionString;
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

        public static string GetProviderFactoryInvariantName<TProvider>()
            where TProvider : DbProviderFactory
        {
            System.Data.DataRow[] dataRows = DbProviderFactories.GetFactoryClasses().Select($"[AssemblyQualifiedName]='{typeof(TProvider).AssemblyQualifiedName}'");
            return (dataRows.Length > 0) ? dataRows[0]["InvariantName"] as string : null;
        }

        public static bool TestProviderFactoryName<TProvider>(string invariantName)
            where TProvider : DbProviderFactory
        {
            return DbProviderFactories.GetFactoryClasses().Select($"[AssemblyQualifiedName]='{typeof(TProvider).AssemblyQualifiedName}' AND [InvariantName]='{invariantName}'").Length > 0;
        }

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

        internal static void SetRemoteDbContainerConnectionString(string connectionString, bool doNotRefreshConfigurationManager = false)
        {
            throw new NotImplementedException();
            //SetConnectionString(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.PerUserRoamingAndLocal), nameof(Model.Remote.RemoteDbContainer),
            //    GetProviderFactoryInvariantName<SqlClientFactory>(), connectionString, doNotRefreshConfigurationManager);
        }
    }
}
