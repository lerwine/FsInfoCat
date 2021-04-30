using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class DbConnectionSettingsViewModel : DependencyObject
    {
        private bool _ignoreChange = false;
        private readonly SqlConnectionStringBuilder _backingBuilder;

        #region Dependency Properties

        public static readonly DependencyProperty AsynchronousProcessingProperty = DependencyProperty.Register(nameof(AsynchronousProcessing), typeof(bool),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnAsynchronousProcessingPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool AsynchronousProcessing
        {
            get { return (bool)GetValue(AsynchronousProcessingProperty); }
            set { SetValue(AsynchronousProcessingProperty, value); }
        }

        public static readonly DependencyPropertyKey AsynchronousProcessingErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(AsynchronousProcessingError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty AsynchronousProcessingErrorProperty = AsynchronousProcessingErrorPropertyKey.DependencyProperty;

        public string AsynchronousProcessingError
        {
            get { return GetValue(AsynchronousProcessingErrorProperty) as string; }
            private set { SetValue(AsynchronousProcessingErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty ColumnEncryptionSettingProperty = DependencyProperty.Register(nameof(ColumnEncryptionSetting),
            typeof(SqlConnectionColumnEncryptionSetting), typeof(DbConnectionSettingsViewModel), new PropertyMetadata(SqlConnectionColumnEncryptionSetting.Disabled,
                (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionSettingsViewModel).OnColumnEncryptionSettingPropertyChanged((SqlConnectionColumnEncryptionSetting)e.OldValue,
                        (SqlConnectionColumnEncryptionSetting)e.NewValue)));

        public SqlConnectionColumnEncryptionSetting ColumnEncryptionSetting
        {
            get { return (SqlConnectionColumnEncryptionSetting)GetValue(ColumnEncryptionSettingProperty); }
            set { SetValue(ColumnEncryptionSettingProperty, value); }
        }

        public static readonly DependencyPropertyKey ColumnEncryptionSettingErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ColumnEncryptionSettingError), typeof(string), typeof(DbConnectionSettingsViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty ColumnEncryptionSettingErrorProperty = ColumnEncryptionSettingErrorPropertyKey.DependencyProperty;

        public string ColumnEncryptionSettingError
        {
            get { return GetValue(ColumnEncryptionSettingErrorProperty) as string; }
            private set { SetValue(ColumnEncryptionSettingErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty ConnectionStringProperty = DependencyProperty.Register(nameof(ConnectionString), typeof(string), typeof(DbConnectionSettingsViewModel),
            new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnConnectionStringPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string ConnectionString
        {
            get { return GetValue(ConnectionStringProperty) as string; }
            set { SetValue(ConnectionStringProperty, value); }
        }

        public static readonly DependencyPropertyKey ConnectionStringErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ConnectionStringError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty ConnectionStringErrorProperty = ConnectionStringErrorPropertyKey.DependencyProperty;

        public string ConnectionStringError
        {
            get { return GetValue(ConnectionStringErrorProperty) as string; }
            private set { SetValue(ConnectionStringErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty ConnectRetryCountTextProperty = DependencyProperty.Register(nameof(ConnectRetryCountText), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnConnectRetryCountTextPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string ConnectRetryCountText
        {
            get { return GetValue(ConnectRetryCountTextProperty) as string; }
            set { SetValue(ConnectRetryCountTextProperty, value); }
        }

        public static readonly DependencyProperty ConnectRetryCountProperty = DependencyProperty.Register(nameof(ConnectRetryCount), typeof(int), typeof(DbConnectionSettingsViewModel),
            new PropertyMetadata(1, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnConnectRetryCountPropertyChanged((int)e.OldValue, (int)e.NewValue)));

        /// <summary>
        /// The number of reconnections attempted after identifying that there was an idle connection failure.
        /// </summary>
        /// <remarks>This must be an integer between 0 and 255. Default is 1. Set to 0 to disable reconnecting on idle connection failures.</remarks>
        public int ConnectRetryCount
        {
            get { return (int)GetValue(ConnectRetryCountProperty); }
            set { SetValue(ConnectRetryCountProperty, value); }
        }

        public static readonly DependencyPropertyKey ConnectRetryCountErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ConnectRetryCountError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty ConnectRetryCountErrorProperty = ConnectRetryCountErrorPropertyKey.DependencyProperty;

        public string ConnectRetryCountError
        {
            get { return GetValue(ConnectRetryCountErrorProperty) as string; }
            private set { SetValue(ConnectRetryCountErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty ConnectRetryIntervalTextProperty =
            DependencyProperty.Register(nameof(ConnectRetryIntervalText), typeof(string), typeof(DbConnectionSettingsViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionSettingsViewModel).OnConnectRetryIntervalTextPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string ConnectRetryIntervalText
        {
            get { return GetValue(ConnectRetryIntervalTextProperty) as string; }
            set { SetValue(ConnectRetryIntervalTextProperty, value); }
        }

        public static readonly DependencyProperty ConnectRetryIntervalProperty = DependencyProperty.Register(nameof(ConnectRetryInterval), typeof(int),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(10, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnConnectRetryIntervalPropertyChanged((int)e.OldValue, (int)e.NewValue)));

        /// <summary>
        /// Amount of time (in seconds) between each reconnection attempt after identifying that there was an idle connection failure.
        /// </summary>
        /// <remarks>This must be an integer between 1 and 60. The default is 10 seconds.</remarks>
        public int ConnectRetryInterval
        {
            get { return (int)GetValue(ConnectRetryIntervalProperty); }
            set { SetValue(ConnectRetryIntervalProperty, value); }
        }

        public static readonly DependencyPropertyKey ConnectRetryIntervalErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ConnectRetryIntervalError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty ConnectRetryIntervalErrorProperty = ConnectRetryIntervalErrorPropertyKey.DependencyProperty;

        public string ConnectRetryIntervalError
        {
            get { return GetValue(ConnectRetryIntervalErrorProperty) as string; }
            private set { SetValue(ConnectRetryIntervalErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty ConnectTimeoutTextProperty =
            DependencyProperty.Register(nameof(ConnectTimeoutText), typeof(string), typeof(DbConnectionSettingsViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionSettingsViewModel).OnConnectTimeoutTextPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string ConnectTimeoutText
        {
            get { return GetValue(ConnectTimeoutTextProperty) as string; }
            set { SetValue(ConnectTimeoutTextProperty, value); }
        }

        public static readonly DependencyProperty ConnectTimeoutProperty = DependencyProperty.Register(nameof(ConnectTimeout), typeof(int), typeof(DbConnectionSettingsViewModel),
            new PropertyMetadata(30, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnConnectTimeoutPropertyChanged((int)e.OldValue, (int)e.NewValue)));

        /// <summary>
        /// The length of time (in seconds) to wait for a connection to the server before terminating the attempt and generating an error.
        /// </summary>
        public int ConnectTimeout
        {
            get { return (int)GetValue(ConnectTimeoutProperty); }
            set { SetValue(ConnectTimeoutProperty, value); }
        }

        public static readonly DependencyPropertyKey ConnectTimeoutErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ConnectTimeoutError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty ConnectTimeoutErrorProperty = ConnectTimeoutErrorPropertyKey.DependencyProperty;

        public string ConnectTimeoutError
        {
            get { return GetValue(ConnectTimeoutErrorProperty) as string; }
            private set { SetValue(ConnectTimeoutErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty ContextConnectionProperty = DependencyProperty.Register(nameof(ContextConnection), typeof(bool), typeof(DbConnectionSettingsViewModel),
            new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnContextConnectionPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool ContextConnection
        {
            get { return (bool)GetValue(ContextConnectionProperty); }
            set { SetValue(ContextConnectionProperty, value); }
        }

        public static readonly DependencyPropertyKey ContextConnectionErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ContextConnectionError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty ContextConnectionErrorProperty = ContextConnectionErrorPropertyKey.DependencyProperty;

        public string ContextConnectionError
        {
            get { return GetValue(ContextConnectionErrorProperty) as string; }
            private set { SetValue(ContextConnectionErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty CurrentLanguageProperty = DependencyProperty.Register(nameof(CurrentLanguage), typeof(string), typeof(DbConnectionSettingsViewModel),
            new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnCurrentLanguagePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// The SQL Server Language record name.
        /// </summary>
        public string CurrentLanguage
        {
            get { return GetValue(CurrentLanguageProperty) as string; }
            set { SetValue(CurrentLanguageProperty, value); }
        }

        public static readonly DependencyPropertyKey CurrentLanguageErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CurrentLanguageError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty CurrentLanguageErrorProperty = CurrentLanguageErrorPropertyKey.DependencyProperty;

        public string CurrentLanguageError
        {
            get { return GetValue(CurrentLanguageErrorProperty) as string; }
            private set { SetValue(CurrentLanguageErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty DataSourceProperty = DependencyProperty.Register(nameof(DataSource), typeof(string), typeof(DbConnectionSettingsViewModel),
            new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnDataSourcePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// The name or network address of the instance of SQL Server to connect to.
        /// </summary>
        public string DataSource
        {
            get { return GetValue(DataSourceProperty) as string; }
            set { SetValue(DataSourceProperty, value); }
        }

        public static readonly DependencyPropertyKey DataSourceErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(DataSourceError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty DataSourceErrorProperty = DataSourceErrorPropertyKey.DependencyProperty;

        public string DataSourceError
        {
            get { return GetValue(DataSourceErrorProperty) as string; }
            private set { SetValue(DataSourceErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty EnclaveAttestationUrlProperty = DependencyProperty.Register(nameof(EnclaveAttestationUrl), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnEnclaveAttestationUrlPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string EnclaveAttestationUrl
        {
            get { return GetValue(EnclaveAttestationUrlProperty) as string; }
            set { SetValue(EnclaveAttestationUrlProperty, value); }
        }

        public static readonly DependencyPropertyKey EnclaveAttestationUrlErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EnclaveAttestationUrlError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty EnclaveAttestationUrlErrorProperty = EnclaveAttestationUrlErrorPropertyKey.DependencyProperty;

        public string EnclaveAttestationUrlError
        {
            get { return GetValue(EnclaveAttestationUrlErrorProperty) as string; }
            private set { SetValue(EnclaveAttestationUrlErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty EncryptProperty =
            DependencyProperty.Register(nameof(Encrypt), typeof(bool), typeof(DbConnectionSettingsViewModel),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionSettingsViewModel).OnEncryptPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        /// <summary>
        /// Indicates whether SQL Server uses SSL encryption for all data sent between the client and server if the server has a certificate installed.
        /// </summary>
        public bool Encrypt
        {
            get { return (bool)GetValue(EncryptProperty); }
            set { SetValue(EncryptProperty, value); }
        }

        public static readonly DependencyPropertyKey EncryptErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EncryptError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty EncryptErrorProperty = EncryptErrorPropertyKey.DependencyProperty;

        public string EncryptError
        {
            get { return GetValue(EncryptErrorProperty) as string; }
            private set { SetValue(EncryptErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty EnlistProperty = DependencyProperty.Register(nameof(Enlist), typeof(bool), typeof(DbConnectionSettingsViewModel),
            new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnEnlistPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        /// <summary>
        /// Indicates whether the SQL Server connection pooler automatically enlists the connection in the creation thread's current transaction context.
        /// </summary>
        public bool Enlist
        {
            get { return (bool)GetValue(EnlistProperty); }
            set { SetValue(EnlistProperty, value); }
        }

        public static readonly DependencyPropertyKey EnlistErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(EnlistError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty EnlistErrorProperty = EnlistErrorPropertyKey.DependencyProperty;

        public string EnlistError
        {
            get { return GetValue(EnlistErrorProperty) as string; }
            private set { SetValue(EnlistErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty FailoverPartnerProperty = DependencyProperty.Register(nameof(FailoverPartner), typeof(string), typeof(DbConnectionSettingsViewModel),
            new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnFailoverPartnerPropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// The name or address of the partner server to connect to if the primary server is down.
        /// </summary>
        public string FailoverPartner
        {
            get { return GetValue(FailoverPartnerProperty) as string; }
            set { SetValue(FailoverPartnerProperty, value); }
        }

        public static readonly DependencyPropertyKey FailoverPartnerErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(FailoverPartnerError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty FailoverPartnerErrorProperty = FailoverPartnerErrorPropertyKey.DependencyProperty;

        public string FailoverPartnerError
        {
            get { return GetValue(FailoverPartnerErrorProperty) as string; }
            private set { SetValue(FailoverPartnerErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty InitialCatalogProperty = DependencyProperty.Register(nameof(InitialCatalog), typeof(string), typeof(DbConnectionSettingsViewModel),
            new PropertyMetadata("FsInfoCat", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnInitialCatalogPropertyChanged(e.OldValue as string, e.NewValue as string)));

        // TOOD: Validate not empty
        /// <summary>
        /// The name of the database associated with the connection.
        /// </summary>
        public string InitialCatalog
        {
            get { return GetValue(InitialCatalogProperty) as string; }
            set { SetValue(InitialCatalogProperty, value); }
        }

        public static readonly DependencyPropertyKey InitialCatalogErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(InitialCatalogError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty InitialCatalogErrorProperty = InitialCatalogErrorPropertyKey.DependencyProperty;

        public string InitialCatalogError
        {
            get { return GetValue(InitialCatalogErrorProperty) as string; }
            private set { SetValue(InitialCatalogErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty IntegratedSecurityProperty = DependencyProperty.Register(nameof(IntegratedSecurity), typeof(bool), typeof(DbConnectionSettingsViewModel),
            new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnIntegratedSecurityPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        /// <summary>
        /// Indicates whether the current Windows account credentials are used for authentication (when true).
        /// </summary>
        public bool IntegratedSecurity
        {
            get { return (bool)GetValue(IntegratedSecurityProperty); }
            set { SetValue(IntegratedSecurityProperty, value); }
        }

        public static readonly DependencyPropertyKey IntegratedSecurityErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IntegratedSecurityError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty IntegratedSecurityErrorProperty = IntegratedSecurityErrorPropertyKey.DependencyProperty;

        public string IntegratedSecurityError
        {
            get { return GetValue(IntegratedSecurityErrorProperty) as string; }
            private set { SetValue(IntegratedSecurityErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty LoadBalanceTimeoutTextProperty = DependencyProperty.Register(nameof(LoadBalanceTimeoutText), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnLoadBalanceTimeoutTextPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string LoadBalanceTimeoutText
        {
            get { return GetValue(LoadBalanceTimeoutTextProperty) as string; }
            set { SetValue(LoadBalanceTimeoutTextProperty, value); }
        }

        public static readonly DependencyProperty LoadBalanceTimeoutProperty = DependencyProperty.Register(nameof(LoadBalanceTimeout), typeof(int), typeof(DbConnectionSettingsViewModel),
            new PropertyMetadata(0, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnLoadBalanceTimeoutPropertyChanged((int)e.OldValue, (int)e.NewValue)));

        /// <summary>
        /// The minimum time, in seconds, for the connection to live in the connection pool before being destroyed.
        /// </summary>
        public int LoadBalanceTimeout
        {
            get { return (int)GetValue(LoadBalanceTimeoutProperty); }
            set { SetValue(LoadBalanceTimeoutProperty, value); }
        }

        public static readonly DependencyPropertyKey LoadBalanceTimeoutErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LoadBalanceTimeoutError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty LoadBalanceTimeoutErrorProperty = LoadBalanceTimeoutErrorPropertyKey.DependencyProperty;

        public string LoadBalanceTimeoutError
        {
            get { return GetValue(LoadBalanceTimeoutErrorProperty) as string; }
            private set { SetValue(LoadBalanceTimeoutErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty MaxPoolSizeTextProperty =
            DependencyProperty.Register(nameof(MaxPoolSizeText), typeof(string), typeof(DbConnectionSettingsViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionSettingsViewModel).OnMaxPoolSizeTextPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string MaxPoolSizeText
        {
            get { return GetValue(MaxPoolSizeTextProperty) as string; }
            set { SetValue(MaxPoolSizeTextProperty, value); }
        }

        public static readonly DependencyProperty MaxPoolSizeProperty = DependencyProperty.Register(nameof(MaxPoolSize), typeof(int), typeof(DbConnectionSettingsViewModel),
            new PropertyMetadata(100, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnMaxPoolSizePropertyChanged((int)e.OldValue, (int)e.NewValue)));

        /// <summary>
        /// The maximum number of connections allowed in the connection pool for this current connection string.
        /// </summary>
        public int MaxPoolSize
        {
            get { return (int)GetValue(MaxPoolSizeProperty); }
            set { SetValue(MaxPoolSizeProperty, value); }
        }

        public static readonly DependencyPropertyKey MaxPoolSizeErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MaxPoolSizeError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty MaxPoolSizeErrorProperty = MaxPoolSizeErrorPropertyKey.DependencyProperty;

        public string MaxPoolSizeError
        {
            get { return GetValue(MaxPoolSizeErrorProperty) as string; }
            private set { SetValue(MaxPoolSizeErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty MinPoolSizeTextProperty =
            DependencyProperty.Register(nameof(MinPoolSizeText), typeof(string), typeof(DbConnectionSettingsViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionSettingsViewModel).OnMinPoolSizeTextPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string MinPoolSizeText
        {
            get { return GetValue(MinPoolSizeTextProperty) as string; }
            set { SetValue(MinPoolSizeTextProperty, value); }
        }

        public static readonly DependencyProperty MinPoolSizeProperty = DependencyProperty.Register(nameof(MinPoolSize), typeof(int), typeof(DbConnectionSettingsViewModel),
            new PropertyMetadata(0, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnMinPoolSizePropertyChanged((int)e.OldValue, (int)e.NewValue)));

        /// <summary>
        /// The minimum number of connections allowed in the connection pool for this specific connection string.
        /// </summary>
        public int MinPoolSize
        {
            get { return (int)GetValue(MinPoolSizeProperty); }
            set { SetValue(MinPoolSizeProperty, value); }
        }

        public static readonly DependencyPropertyKey MinPoolSizeErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MinPoolSizeError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty MinPoolSizeErrorProperty = MinPoolSizeErrorPropertyKey.DependencyProperty;

        public string MinPoolSizeError
        {
            get { return GetValue(MinPoolSizeErrorProperty) as string; }
            private set { SetValue(MinPoolSizeErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty MultipleActiveResultSetsProperty = DependencyProperty.Register(nameof(MultipleActiveResultSets), typeof(bool),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnMultipleActiveResultSetsPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool MultipleActiveResultSets
        {
            get { return (bool)GetValue(MultipleActiveResultSetsProperty); }
            set { SetValue(MultipleActiveResultSetsProperty, value); }
        }

        public static readonly DependencyPropertyKey MultipleActiveResultSetsErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MultipleActiveResultSetsError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty MultipleActiveResultSetsErrorProperty = MultipleActiveResultSetsErrorPropertyKey.DependencyProperty;

        public string MultipleActiveResultSetsError
        {
            get { return GetValue(MultipleActiveResultSetsErrorProperty) as string; }
            private set { SetValue(MultipleActiveResultSetsErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty MultiSubnetFailoverProperty = DependencyProperty.Register(nameof(MultiSubnetFailover), typeof(bool),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnMultiSubnetFailoverPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool MultiSubnetFailover
        {
            get { return (bool)GetValue(MultiSubnetFailoverProperty); }
            set { SetValue(MultiSubnetFailoverProperty, value); }
        }

        public static readonly DependencyPropertyKey MultiSubnetFailoverErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MultiSubnetFailoverError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty MultiSubnetFailoverErrorProperty = MultiSubnetFailoverErrorPropertyKey.DependencyProperty;

        public string MultiSubnetFailoverError
        {
            get { return GetValue(MultiSubnetFailoverErrorProperty) as string; }
            private set { SetValue(MultiSubnetFailoverErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty NetworkLibraryProperty = DependencyProperty.Register(nameof(NetworkLibrary), typeof(string), typeof(DbConnectionSettingsViewModel),
            new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnNetworkLibraryPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string NetworkLibrary
        {
            get { return GetValue(NetworkLibraryProperty) as string; }
            set { SetValue(NetworkLibraryProperty, value); }
        }

        public static readonly DependencyPropertyKey NetworkLibraryErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(NetworkLibraryError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty NetworkLibraryErrorProperty = NetworkLibraryErrorPropertyKey.DependencyProperty;

        public string NetworkLibraryError
        {
            get { return GetValue(NetworkLibraryErrorProperty) as string; }
            private set { SetValue(NetworkLibraryErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty PacketSizeTextProperty =
            DependencyProperty.Register(nameof(PacketSizeText), typeof(string), typeof(DbConnectionSettingsViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionSettingsViewModel).OnPacketSizeTextPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string PacketSizeText
        {
            get { return GetValue(PacketSizeTextProperty) as string; }
            set { SetValue(PacketSizeTextProperty, value); }
        }

        public static readonly DependencyProperty PacketSizeProperty = DependencyProperty.Register(nameof(PacketSize), typeof(int), typeof(DbConnectionSettingsViewModel),
            new PropertyMetadata(8000, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnPacketSizePropertyChanged((int)e.OldValue, (int)e.NewValue)));

        public int PacketSize
        {
            get { return (int)GetValue(PacketSizeProperty); }
            set { SetValue(PacketSizeProperty, value); }
        }

        public static readonly DependencyPropertyKey PacketSizeErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PacketSizeError), typeof(string), typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty PacketSizeErrorProperty = PacketSizeErrorPropertyKey.DependencyProperty;

        public string PacketSizeError
        {
            get { return GetValue(PacketSizeErrorProperty) as string; }
            private set { SetValue(PacketSizeErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(nameof(Password), typeof(string), typeof(DbConnectionSettingsViewModel),
            new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnPasswordPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string Password
        {
            get { return GetValue(PasswordProperty) as string; }
            set { SetValue(PasswordProperty, value); }
        }

        public static readonly DependencyPropertyKey PasswordErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PasswordError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty PasswordErrorProperty = PasswordErrorPropertyKey.DependencyProperty;

        public string PasswordError
        {
            get { return GetValue(PasswordErrorProperty) as string; }
            private set { SetValue(PasswordErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty PersistSecurityInfoProperty = DependencyProperty.Register(nameof(PersistSecurityInfo), typeof(bool),
            typeof(DbConnectionSettingsViewModel),
            new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnPersistSecurityInfoPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool PersistSecurityInfo
        {
            get { return (bool)GetValue(PersistSecurityInfoProperty); }
            set { SetValue(PersistSecurityInfoProperty, value); }
        }

        public static readonly DependencyPropertyKey PersistSecurityInfoErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PersistSecurityInfoError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty PersistSecurityInfoErrorProperty = PersistSecurityInfoErrorPropertyKey.DependencyProperty;

        public string PersistSecurityInfoError
        {
            get { return GetValue(PersistSecurityInfoErrorProperty) as string; }
            private set { SetValue(PersistSecurityInfoErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty PoolBlockingPeriodProperty = DependencyProperty.Register(nameof(PoolBlockingPeriod), typeof(PoolBlockingPeriod),
            typeof(DbConnectionSettingsViewModel),
            new PropertyMetadata(PoolBlockingPeriod.Auto, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnPoolBlockingPeriodPropertyChanged((PoolBlockingPeriod)e.OldValue, (PoolBlockingPeriod)e.NewValue)));

        public PoolBlockingPeriod PoolBlockingPeriod
        {
            get { return (PoolBlockingPeriod)GetValue(PoolBlockingPeriodProperty); }
            set { SetValue(PoolBlockingPeriodProperty, value); }
        }

        public static readonly DependencyPropertyKey PoolBlockingPeriodErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PoolBlockingPeriodError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty PoolBlockingPeriodErrorProperty = PoolBlockingPeriodErrorPropertyKey.DependencyProperty;

        public string PoolBlockingPeriodError
        {
            get { return GetValue(PoolBlockingPeriodErrorProperty) as string; }
            private set { SetValue(PoolBlockingPeriodErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty PoolingProperty = DependencyProperty.Register(nameof(Pooling), typeof(bool), typeof(DbConnectionSettingsViewModel),
            new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnPoolingPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool Pooling
        {
            get { return (bool)GetValue(PoolingProperty); }
            set { SetValue(PoolingProperty, value); }
        }

        public static readonly DependencyPropertyKey PoolingErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(PoolingError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty PoolingErrorProperty = PoolingErrorPropertyKey.DependencyProperty;

        public string PoolingError
        {
            get { return GetValue(PoolingErrorProperty) as string; }
            private set { SetValue(PoolingErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty ReplicationProperty = DependencyProperty.Register(nameof(Replication), typeof(bool), typeof(DbConnectionSettingsViewModel),
            new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnReplicationPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool Replication
        {
            get { return (bool)GetValue(ReplicationProperty); }
            set { SetValue(ReplicationProperty, value); }
        }

        public static readonly DependencyPropertyKey ReplicationErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ReplicationError), typeof(string), typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty ReplicationErrorProperty = ReplicationErrorPropertyKey.DependencyProperty;

        public string ReplicationError
        {
            get { return GetValue(ReplicationErrorProperty) as string; }
            private set { SetValue(ReplicationErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty TransparentNetworkIPResolutionProperty = DependencyProperty.Register(nameof(TransparentNetworkIPResolution), typeof(bool),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnTransparentNetworkIPResolutionPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool TransparentNetworkIPResolution
        {
            get { return (bool)GetValue(TransparentNetworkIPResolutionProperty); }
            set { SetValue(TransparentNetworkIPResolutionProperty, value); }
        }

        public static readonly DependencyPropertyKey TransparentNetworkIPResolutionErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(TransparentNetworkIPResolutionError), typeof(string), typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty TransparentNetworkIPResolutionErrorProperty = TransparentNetworkIPResolutionErrorPropertyKey.DependencyProperty;

        public string TransparentNetworkIPResolutionError
        {
            get { return GetValue(TransparentNetworkIPResolutionErrorProperty) as string; }
            private set { SetValue(TransparentNetworkIPResolutionErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty TrustServerCertificateProperty = DependencyProperty.Register(nameof(TrustServerCertificate), typeof(bool),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnTrustServerCertificatePropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool TrustServerCertificate
        {
            get { return (bool)GetValue(TrustServerCertificateProperty); }
            set { SetValue(TrustServerCertificateProperty, value); }
        }

        public static readonly DependencyPropertyKey TrustServerCertificateErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(TrustServerCertificateError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty TrustServerCertificateErrorProperty = TrustServerCertificateErrorPropertyKey.DependencyProperty;

        public string TrustServerCertificateError
        {
            get { return GetValue(TrustServerCertificateErrorProperty) as string; }
            private set { SetValue(TrustServerCertificateErrorPropertyKey, value); }
        }

        public static readonly DependencyProperty UserIDProperty = DependencyProperty.Register(nameof(UserID), typeof(string), typeof(DbConnectionSettingsViewModel),
                new PropertyMetadata("fsinfocatadmin", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionSettingsViewModel).OnUserIDPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string UserID
        {
            get { return GetValue(UserIDProperty) as string; }
            set { SetValue(UserIDProperty, value); }
        }

        public static readonly DependencyProperty UserInstanceProperty = DependencyProperty.Register(nameof(UserInstance), typeof(bool), typeof(DbConnectionSettingsViewModel),
            new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as DbConnectionSettingsViewModel).OnUserInstancePropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool UserInstance
        {
            get { return (bool)GetValue(UserInstanceProperty); }
            set { SetValue(UserInstanceProperty, value); }
        }

        public static readonly DependencyPropertyKey UserIDErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(UserIDError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty UserIDErrorProperty = UserIDErrorPropertyKey.DependencyProperty;

        public string UserIDError
        {
            get { return GetValue(UserIDErrorProperty) as string; }
            private set { SetValue(UserIDErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey UserInstanceErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(UserInstanceError), typeof(string),
            typeof(DbConnectionSettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty UserInstanceErrorProperty = UserInstanceErrorPropertyKey.DependencyProperty;

        public string UserInstanceError
        {
            get { return GetValue(UserInstanceErrorProperty) as string; }
            private set { SetValue(UserInstanceErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey WorkstationIDErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(WorkstationIDError), typeof(string), typeof(DbConnectionSettingsViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty WorkstationIDErrorProperty = WorkstationIDErrorPropertyKey.DependencyProperty;
        private readonly ILogger<DbConnectionSettingsViewModel> _logger;

        public string WorkstationIDError
        {
            get { return GetValue(WorkstationIDErrorProperty) as string; }
            private set { SetValue(WorkstationIDErrorPropertyKey, value); }
        }

        #endregion

        #region Commands

        public event EventHandler Advanced;

        private Commands.RelayCommand _advancedCommand = null;

        public Commands.RelayCommand AdvancedCommand
        {
            get
            {
                if (_advancedCommand == null)
                    _advancedCommand = new Commands.RelayCommand(parameter =>
                        Advanced?.Invoke(this, EventArgs.Empty));
                return _advancedCommand;
            }
        }

        public event EventHandler Save;

        private Commands.RelayCommand _saveCommand = null;

        public Commands.RelayCommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                    _saveCommand = new Commands.RelayCommand(parameter =>
                    {
                        try { OnSave(parameter); }
                        finally { Save?.Invoke(this, EventArgs.Empty); }
                    });
                return _saveCommand;
            }
        }

        public event EventHandler Cancel;

        private Commands.RelayCommand _cancelCommand = null;

        public Commands.RelayCommand CancelCommand
        {
            get
            {
                if (_cancelCommand == null)
                    _cancelCommand = new Commands.RelayCommand(parameter =>
                        Cancel?.Invoke(this, EventArgs.Empty));
                return _cancelCommand;
            }
        }

        #endregion

        public DbConnectionSettingsViewModel()
        {
            _logger = App.LoggerFactory.CreateLogger<DbConnectionSettingsViewModel>();
            try
            {
                _backingBuilder = AppConfig.GetRemoteDbContainerConnectionStringBuilder();
            }
            catch (Exception exc)
            {
                _logger.LogCritical(exc, "Error parsing EDM connection string");
                MessageBox.Show("Error parsing connection settings", "Settings error", MessageBoxButton.OK, MessageBoxImage.Error);

            }
            RefreshFromConnectionString();
        }

        #region Callbacks

        protected virtual void OnSave(object parameter)
        {
            // TODO: Implement OnSave Logic
        }

        protected virtual void OnAsynchronousProcessingPropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                TryChangeConnectionSetting(nameof(AsynchronousProcessing), newValue, () => _backingBuilder.AsynchronousProcessing = newValue, m => AsynchronousProcessingError = m);
        }

        protected virtual void OnColumnEncryptionSettingPropertyChanged(SqlConnectionColumnEncryptionSetting oldValue, SqlConnectionColumnEncryptionSetting newValue)
        {
            if (oldValue != newValue)
                TryChangeConnectionSetting(nameof(ColumnEncryptionSetting), newValue, () => _backingBuilder.ColumnEncryptionSetting = newValue,
                    m => ColumnEncryptionSettingError = m);
        }

        private int _lastParsedConnectRetryCount = -1;
        protected virtual void OnConnectRetryCountTextPropertyChanged(string oldValue, string newValue)
        {
            if (_ignoreChange)
                return;
            if (string.IsNullOrWhiteSpace(newValue))
            {
                _ignoreChange = true;
                try
                {
                    _backingBuilder.Remove("ConnectRetryCount");
                    ConnectRetryCount = _backingBuilder.ConnectRetryCount;
                    ConnectionString = _backingBuilder.ConnectionString;
                }
                finally { _ignoreChange = false; }
                return;
            }
            if (double.TryParse(newValue.Trim(), out double d) && d >= 0.0 && d <= int.MaxValue)
            {
                int i = Convert.ToInt32(d);
                if (Convert.ToDouble(i) == d)
                {
                    _lastParsedConnectRetryCount = ConnectRetryCount = i;
                    return;
                }
            }
            ConnectRetryCountError = "Invalid number";
            _lastParsedConnectRetryCount = -1;
        }

        protected virtual void OnConnectRetryCountPropertyChanged(int oldValue, int newValue)
        {
            if (oldValue != newValue && TryChangeConnectionSetting(nameof(ConnectRetryCount), newValue, () => _backingBuilder.ConnectRetryCount = newValue,
                m => ConnectRetryCountError = m) && _lastParsedConnectRetryCount != newValue)
            {
                _ignoreChange = true;
                try
                {
                    _lastParsedConnectRetryCount = newValue;
                    ConnectRetryCountText = newValue.ToString();
                }
                finally { _ignoreChange = false; }
                if (newValue < 0 || newValue > 255)
                    ConnectRetryCountError = "Value cannot be less than 0 or greater than 255";
            }
        }

        private int _lastParsedConnectRetryInterval = -1;

        protected virtual void OnConnectRetryIntervalTextPropertyChanged(string oldValue, string newValue)
        {
            if (_ignoreChange)
                return;
            if (string.IsNullOrWhiteSpace(newValue))
            {
                _ignoreChange = true;
                try
                {
                    _backingBuilder.Remove("ConnectRetryInterval");
                    ConnectRetryInterval = _backingBuilder.ConnectRetryInterval;
                    ConnectionString = _backingBuilder.ConnectionString;
                }
                finally { _ignoreChange = false; }
                return;
            }
            if (double.TryParse(newValue.Trim(), out double d) && d >= 0.0 && d <= int.MaxValue)
            {
                int i = Convert.ToInt32(d);
                if (Convert.ToDouble(i) == d)
                {
                    _lastParsedConnectRetryInterval = ConnectRetryInterval = i;
                    return;
                }
            }
            ConnectRetryIntervalError = "Invalid number";
            _lastParsedConnectRetryInterval = -1;
        }

        protected virtual void OnConnectRetryIntervalPropertyChanged(int oldValue, int newValue)
        {
            if (oldValue != newValue && TryChangeConnectionSetting(nameof(ConnectRetryInterval), newValue, () => _backingBuilder.ConnectRetryInterval = newValue,
                m => ConnectRetryIntervalError = m) && _lastParsedConnectRetryInterval != newValue)
            {
                _ignoreChange = true;
                try
                {
                    _lastParsedConnectRetryInterval = newValue;
                    ConnectRetryIntervalText = newValue.ToString();
                }
                finally { _ignoreChange = false; }
                if (newValue < 1 || newValue > 60)
                    ConnectRetryIntervalError = "Value cannot be less than 1 or greater than 60";
            }
        }

        private int _lastParsedConnectTimeout = -1;

        protected virtual void OnConnectTimeoutTextPropertyChanged(string oldValue, string newValue)
        {
            if (_ignoreChange)
                return;
            if (string.IsNullOrWhiteSpace(newValue))
            {
                _ignoreChange = true;
                try
                {
                    _backingBuilder.Remove("Connect Timeout");
                    ConnectTimeout = _backingBuilder.ConnectTimeout;
                    ConnectionString = _backingBuilder.ConnectionString;
                }
                finally { _ignoreChange = false; }
                return;
            }
            if (double.TryParse(newValue.Trim(), out double d) && d >= 0.0 && d <= int.MaxValue)
            {
                int i = Convert.ToInt32(d);
                if (Convert.ToDouble(i) == d)
                {
                    _lastParsedConnectTimeout = ConnectTimeout = i;
                    return;
                }
            }
            ConnectTimeoutError = "Invalid number";
            _lastParsedConnectTimeout = -1;
        }

        protected virtual void OnConnectTimeoutPropertyChanged(int oldValue, int newValue)
        {
            if (oldValue != newValue && TryChangeConnectionSetting(nameof(ConnectTimeout), newValue, () => _backingBuilder.ConnectTimeout = newValue,
                m => ConnectTimeoutError = m) && _lastParsedConnectTimeout != newValue)
            {
                _ignoreChange = true;
                try
                {
                    _lastParsedConnectTimeout = newValue;
                    ConnectTimeoutText = newValue.ToString();
                }
                finally { _ignoreChange = false; }
                if (newValue < 0)
                    ConnectTimeoutError = "Value cannot be negative";
            }
        }

        protected virtual void OnContextConnectionPropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                TryChangeConnectionSetting(nameof(ContextConnection), newValue, () => _backingBuilder.ContextConnection = newValue,
                    m => ContextConnectionError = m);
        }

        protected virtual void OnCurrentLanguagePropertyChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue)
                TryChangeConnectionSetting(nameof(CurrentLanguage), newValue, () => _backingBuilder.CurrentLanguage = newValue,
                    m => CurrentLanguageError = m);
        }

        protected virtual void OnDataSourcePropertyChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue && TryChangeConnectionSetting(nameof(DataSource), newValue, () => _backingBuilder.DataSource = newValue, m => DataSourceError = m) &&
                string.IsNullOrWhiteSpace(newValue))
            {
                DataSourceError = "Server name or network address required";
            }
        }

        protected virtual void OnEnclaveAttestationUrlPropertyChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue)
                TryChangeConnectionSetting(nameof(EnclaveAttestationUrl), newValue, () => _backingBuilder.EnclaveAttestationUrl = newValue,
                    m => EnclaveAttestationUrlError = m);
        }

        protected virtual void OnEncryptPropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                TryChangeConnectionSetting(nameof(Encrypt), newValue, () => _backingBuilder.Encrypt = newValue,
                    m => EncryptError = m);
        }

        protected virtual void OnEnlistPropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                TryChangeConnectionSetting(nameof(Enlist), newValue, () => _backingBuilder.Enlist = newValue,
                    m => EnlistError = m);
        }

        protected virtual void OnFailoverPartnerPropertyChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue)
                TryChangeConnectionSetting(nameof(FailoverPartner), newValue, () => _backingBuilder.FailoverPartner = newValue,
                    m => FailoverPartnerError = m);
        }

        protected virtual void OnInitialCatalogPropertyChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue)
                TryChangeConnectionSetting(nameof(InitialCatalog), newValue, () => _backingBuilder.InitialCatalog = newValue,
                    m => InitialCatalogError = m);
        }

        protected virtual void OnIntegratedSecurityPropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue && TryChangeConnectionSetting(nameof(IntegratedSecurity), newValue, () => _backingBuilder.IntegratedSecurity = newValue,
                m => IntegratedSecurityError = m))
            {
                if (newValue)
                {
                    UserIDError = "";
                    PasswordError = "";
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(UserID) && string.IsNullOrWhiteSpace(UserIDError))
                        UserIDError = "User login required";
                    if (string.IsNullOrWhiteSpace(Password) && string.IsNullOrWhiteSpace(PasswordError))
                        PasswordError = "Password required";
                }
            }
        }

        private int _lastParsedLoadBalanceTimeout = -1;

        protected virtual void OnLoadBalanceTimeoutTextPropertyChanged(string oldValue, string newValue)
        {
            if (_ignoreChange)
                return;
            if (string.IsNullOrWhiteSpace(newValue))
            {
                _ignoreChange = true;
                try
                {
                    _backingBuilder.Remove("Load Balance Timeout");
                    LoadBalanceTimeout = _backingBuilder.LoadBalanceTimeout;
                    ConnectionString = _backingBuilder.ConnectionString;
                }
                finally { _ignoreChange = false; }
                return;
            }
            if (double.TryParse(newValue.Trim(), out double d) && d >= 0.0 && d <= int.MaxValue)
            {
                int i = Convert.ToInt32(d);
                if (Convert.ToDouble(i) == d)
                {
                    _lastParsedLoadBalanceTimeout = LoadBalanceTimeout = i;
                    return;
                }
            }
            LoadBalanceTimeoutError = "Invalid number";
            _lastParsedLoadBalanceTimeout = -1;
        }

        protected virtual void OnLoadBalanceTimeoutPropertyChanged(int oldValue, int newValue)
        {
            if (oldValue != newValue && TryChangeConnectionSetting(nameof(LoadBalanceTimeout), newValue, () => _backingBuilder.LoadBalanceTimeout = newValue,
                m => LoadBalanceTimeoutError = m) && _lastParsedLoadBalanceTimeout != newValue)
            {
                _ignoreChange = true;
                try
                {
                    _lastParsedLoadBalanceTimeout = newValue;
                    LoadBalanceTimeoutText = newValue.ToString();
                }
                finally { _ignoreChange = false; }
                if (newValue < 0)
                    LoadBalanceTimeoutError = "Value cannot be negative";
            }
        }

        private int _lastParsedMaxPoolSize = -1;

        protected virtual void OnMaxPoolSizeTextPropertyChanged(string oldValue, string newValue)
        {
            if (_ignoreChange)
                return;
            if (string.IsNullOrWhiteSpace(newValue))
            {
                _ignoreChange = true;
                try
                {
                    _backingBuilder.Remove("Max Pool Size");
                    MaxPoolSize = _backingBuilder.MaxPoolSize;
                    ConnectionString = _backingBuilder.ConnectionString;
                }
                finally { _ignoreChange = false; }
                return;
            }
            if (double.TryParse(newValue.Trim(), out double d) && d >= 0.0 && d <= int.MaxValue)
            {
                int i = Convert.ToInt32(d);
                if (Convert.ToDouble(i) == d)
                {
                    _lastParsedMaxPoolSize = MaxPoolSize = i;
                    return;
                }
            }
            MaxPoolSizeError = "Invalid number";
            _lastParsedMaxPoolSize = -1;
        }

        protected virtual void OnMaxPoolSizePropertyChanged(int oldValue, int newValue)
        {
            if (oldValue != newValue && TryChangeConnectionSetting(nameof(MaxPoolSize), newValue, () => _backingBuilder.MaxPoolSize = newValue, m => MaxPoolSizeError = m) &&
                _lastParsedMaxPoolSize != newValue)
            {
                _ignoreChange = true;
                try
                {
                    _lastParsedMaxPoolSize = newValue;
                    MaxPoolSizeText = newValue.ToString();
                }
                finally { _ignoreChange = false; }
                if (newValue < MinPoolSize)
                    MaxPoolSizeError = "Value cannot be less than the Minimum Pool Size";
            }
        }

        private int _lastParsedMinPoolSize = -1;

        protected virtual void OnMinPoolSizeTextPropertyChanged(string oldValue, string newValue)
        {
            if (_ignoreChange)
                return;
            if (string.IsNullOrWhiteSpace(newValue))
            {
                _ignoreChange = true;
                try
                {
                    _backingBuilder.Remove("Min Pool Size");
                    MinPoolSize = _backingBuilder.MinPoolSize;
                    ConnectionString = _backingBuilder.ConnectionString;
                }
                finally { _ignoreChange = false; }
                return;
            }
            if (double.TryParse(newValue.Trim(), out double d) && d >= 0.0 && d <= int.MaxValue)
            {
                int i = Convert.ToInt32(d);
                if (Convert.ToDouble(i) == d)
                {
                    _lastParsedMinPoolSize = MinPoolSize = i;
                    return;
                }
            }
            MinPoolSizeError = "Invalid number";
            _lastParsedMinPoolSize = -1;
        }

        protected virtual void OnMinPoolSizePropertyChanged(int oldValue, int newValue)
        {
            if (oldValue != newValue && TryChangeConnectionSetting(nameof(MinPoolSize), newValue, () => _backingBuilder.MinPoolSize = newValue, m => MinPoolSizeError = m) &&
                _lastParsedMinPoolSize != newValue)
            {
                _ignoreChange = true;
                try
                {
                    _lastParsedMinPoolSize = newValue;
                    MinPoolSizeText = newValue.ToString();
                }
                finally { _ignoreChange = false; }
                if (newValue < 0)
                    MinPoolSizeError = "Value cannot be negative";
            }
        }

        protected virtual void OnMultipleActiveResultSetsPropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                TryChangeConnectionSetting(nameof(MultipleActiveResultSets), newValue, () => _backingBuilder.MultipleActiveResultSets = newValue,
                    m => MultipleActiveResultSetsError = m);
        }

        protected virtual void OnMultiSubnetFailoverPropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                TryChangeConnectionSetting(nameof(MultiSubnetFailover), newValue, () => _backingBuilder.MultiSubnetFailover = newValue, m => MultiSubnetFailoverError = m);
        }

        protected virtual void OnNetworkLibraryPropertyChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue)
                TryChangeConnectionSetting(nameof(NetworkLibrary), newValue, () => _backingBuilder.NetworkLibrary = newValue, m => NetworkLibraryError = m);
        }

        private int _lastParsedPacketSize = -1;

        protected virtual void OnPacketSizeTextPropertyChanged(string oldValue, string newValue)
        {
            if (_ignoreChange)
                return;
            if (string.IsNullOrWhiteSpace(newValue))
            {
                _ignoreChange = true;
                try
                {
                    _backingBuilder.Remove("Packet Size");
                    PacketSize = _backingBuilder.PacketSize;
                    ConnectionString = _backingBuilder.ConnectionString;
                }
                finally { _ignoreChange = false; }
                return;
            }
            if (double.TryParse(newValue.Trim(), out double d) && d >= 0.0 && d <= int.MaxValue)
            {
                int i = Convert.ToInt32(d);
                if (Convert.ToDouble(i) == d)
                {
                    _lastParsedPacketSize = PacketSize = i;
                    return;
                }
            }
            PacketSizeError = "Invalid number";
            _lastParsedPacketSize = -1;
        }

        protected virtual void OnPacketSizePropertyChanged(int oldValue, int newValue)
        {
            if (oldValue != newValue && TryChangeConnectionSetting(nameof(PacketSize), newValue, () => _backingBuilder.PacketSize = newValue, m => PacketSizeError = m) &&
                _lastParsedPacketSize != newValue)
            {
                _ignoreChange = true;
                try
                {
                    _lastParsedPacketSize = newValue;
                    PacketSizeText = newValue.ToString();
                }
                finally { _ignoreChange = false; }
                if (newValue < 512 || newValue > 32768)
                    PacketSizeError = "Value cannot be less than 512 or greater than 32768";
            }
        }

        protected virtual void OnPasswordPropertyChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue && TryChangeConnectionSetting(nameof(Password), newValue, () => _backingBuilder.Password = newValue, m => PasswordError = m) &&
                string.IsNullOrWhiteSpace(newValue) && !IntegratedSecurity)
                PasswordError = "Password required";
        }

        protected virtual void OnPersistSecurityInfoPropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                TryChangeConnectionSetting(nameof(PersistSecurityInfo), newValue, () => _backingBuilder.PersistSecurityInfo = newValue,
                    m => PersistSecurityInfoError = m);
        }

        protected virtual void OnPoolBlockingPeriodPropertyChanged(PoolBlockingPeriod oldValue, PoolBlockingPeriod newValue)
        {
            if (oldValue != newValue)
                TryChangeConnectionSetting(nameof(PoolBlockingPeriod), newValue, () => _backingBuilder.PoolBlockingPeriod = newValue,
                    m => PoolBlockingPeriodError = m);
        }

        protected virtual void OnPoolingPropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                TryChangeConnectionSetting(nameof(Pooling), newValue, () => _backingBuilder.Pooling = newValue,
                    m => PoolingError = m);
        }

        protected virtual void OnReplicationPropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                TryChangeConnectionSetting(nameof(Replication), newValue, () => _backingBuilder.Replication = newValue,
                    m => ReplicationError = m);
        }

        protected virtual void OnTransparentNetworkIPResolutionPropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                TryChangeConnectionSetting(nameof(TransparentNetworkIPResolution), newValue, () => _backingBuilder.TransparentNetworkIPResolution = newValue,
                    m => TransparentNetworkIPResolutionError = m);
        }

        protected virtual void OnTrustServerCertificatePropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                TryChangeConnectionSetting(nameof(TrustServerCertificate), newValue, () => _backingBuilder.TrustServerCertificate = newValue,
                    m => TrustServerCertificateError = m);
        }

        protected virtual void OnUserIDPropertyChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue && TryChangeConnectionSetting(nameof(UserID), newValue, () => _backingBuilder.UserID = newValue, m => UserIDError = m) &&
                string.IsNullOrWhiteSpace(newValue) && !IntegratedSecurity)
                UserIDError = "User login required";
        }

        protected virtual void OnUserInstancePropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                TryChangeConnectionSetting(nameof(UserInstance), newValue, () => _backingBuilder.UserInstance = newValue,
                    m => UserInstanceError = m);
        }

        protected virtual void OnConnectionStringPropertyChanged(string oldValue, string newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.ConnectionString = newValue;
                RefreshFromConnectionString();
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "{ExceptionType} occurred while setting connection property {PropertyName} to \"{Value}\": {Message}", exception.GetType().Name,
                    nameof(_backingBuilder.ConnectionString), newValue, exception.Message);
                ConnectionStringError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        private void RefreshFromConnectionString()
        {
            ConnectionStringError = "";

            if (_backingBuilder.AsynchronousProcessing != AsynchronousProcessing)
                try { AsynchronousProcessing = _backingBuilder.AsynchronousProcessing; } catch { /* okay to ignore */ }
            if (_backingBuilder.ColumnEncryptionSetting != ColumnEncryptionSetting)
                try { ColumnEncryptionSetting = _backingBuilder.ColumnEncryptionSetting; } catch { /* okay to ignore */ }
            if (_backingBuilder.ConnectionString != ConnectionString)
                try { ConnectionString = _backingBuilder.ConnectionString; } catch { /* okay to ignore */ }
            if (_backingBuilder.ConnectRetryCount != ConnectRetryCount)
                try { ConnectRetryCount = _backingBuilder.ConnectRetryCount; } catch { /* okay to ignore */ }
            if (_backingBuilder.ConnectRetryInterval != ConnectRetryInterval)
                try { ConnectRetryInterval = _backingBuilder.ConnectRetryInterval; } catch { /* okay to ignore */ }
            if (_backingBuilder.ConnectTimeout != ConnectTimeout)
                try { ConnectTimeout = _backingBuilder.ConnectTimeout; } catch { /* okay to ignore */ }
            if (_backingBuilder.ContextConnection != ContextConnection)
                try { ContextConnection = _backingBuilder.ContextConnection; } catch { /* okay to ignore */ }
            if (_backingBuilder.CurrentLanguage != CurrentLanguage)
                try { CurrentLanguage = _backingBuilder.CurrentLanguage; } catch { /* okay to ignore */ }
            if (_backingBuilder.DataSource != DataSource)
                try { DataSource = _backingBuilder.DataSource; } catch { /* okay to ignore */ }
            if (_backingBuilder.EnclaveAttestationUrl != EnclaveAttestationUrl)
                try { EnclaveAttestationUrl = _backingBuilder.EnclaveAttestationUrl; } catch { /* okay to ignore */ }
            if (_backingBuilder.Encrypt != Encrypt)
                try { Encrypt = _backingBuilder.Encrypt; } catch { /* okay to ignore */ }
            if (_backingBuilder.Enlist != Enlist)
                try { Enlist = _backingBuilder.Enlist; } catch { /* okay to ignore */ }
            if (_backingBuilder.FailoverPartner != FailoverPartner)
                try { FailoverPartner = _backingBuilder.FailoverPartner; } catch { /* okay to ignore */ }
            if (_backingBuilder.InitialCatalog != InitialCatalog)
                try { InitialCatalog = _backingBuilder.InitialCatalog; } catch { /* okay to ignore */ }
            if (_backingBuilder.IntegratedSecurity != IntegratedSecurity)
                try { IntegratedSecurity = _backingBuilder.IntegratedSecurity; } catch { /* okay to ignore */ }
            if (_backingBuilder.LoadBalanceTimeout != LoadBalanceTimeout)
                try { LoadBalanceTimeout = _backingBuilder.LoadBalanceTimeout; } catch { /* okay to ignore */ }
            if (_backingBuilder.MaxPoolSize != MaxPoolSize)
                try { MaxPoolSize = _backingBuilder.MaxPoolSize; } catch { /* okay to ignore */ }
            if (_backingBuilder.MinPoolSize != MinPoolSize)
                try { MinPoolSize = _backingBuilder.MinPoolSize; } catch { /* okay to ignore */ }
            if (_backingBuilder.MultipleActiveResultSets != MultipleActiveResultSets)
                try { MultipleActiveResultSets = _backingBuilder.MultipleActiveResultSets; } catch { /* okay to ignore */ }
            if (_backingBuilder.MultiSubnetFailover != MultiSubnetFailover)
                try { MultiSubnetFailover = _backingBuilder.MultiSubnetFailover; } catch { /* okay to ignore */ }
            if (_backingBuilder.NetworkLibrary != NetworkLibrary)
                try { NetworkLibrary = _backingBuilder.NetworkLibrary; } catch { /* okay to ignore */ }
            if (_backingBuilder.PacketSize != PacketSize)
                try { PacketSize = _backingBuilder.PacketSize; } catch { /* okay to ignore */ }
            if (_backingBuilder.Password != Password)
                try { Password = _backingBuilder.Password; } catch { /* okay to ignore */ }
            if (_backingBuilder.PersistSecurityInfo != PersistSecurityInfo)
                try { PersistSecurityInfo = _backingBuilder.PersistSecurityInfo; } catch { /* okay to ignore */ }
            if (_backingBuilder.PoolBlockingPeriod != PoolBlockingPeriod)
                try { PoolBlockingPeriod = _backingBuilder.PoolBlockingPeriod; } catch { /* okay to ignore */ }
            if (_backingBuilder.Pooling != Pooling)
                try { Pooling = _backingBuilder.Pooling; } catch { /* okay to ignore */ }
            if (_backingBuilder.Replication != Replication)
                try { Replication = _backingBuilder.Replication; } catch { /* okay to ignore */ }
            if (_backingBuilder.TransparentNetworkIPResolution != TransparentNetworkIPResolution)
                try { TransparentNetworkIPResolution = _backingBuilder.TransparentNetworkIPResolution; } catch { /* okay to ignore */ }
            if (_backingBuilder.TrustServerCertificate != TrustServerCertificate)
                try { TrustServerCertificate = _backingBuilder.TrustServerCertificate; } catch { /* okay to ignore */ }
            if (_backingBuilder.UserID != UserID)
                try { UserID = _backingBuilder.UserID; } catch { /* okay to ignore */ }
            if (_backingBuilder.UserInstance != UserInstance)
                try { UserInstance = _backingBuilder.UserInstance; } catch { /* okay to ignore */ }
        }

        private bool TryChangeConnectionSetting<T>(string name, T value, Action valueSetter, Action<string> errorSetter)
        {
            if (_ignoreChange)
            {
                errorSetter("");
                return false;
            }
            _ignoreChange = true;
            try
            {
                valueSetter();
                ConnectionString = _backingBuilder.ConnectionString;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "{ExceptionType} occurred while setting connection property {PropertyName} to \"{Value}\": {Message}", exception.GetType().Name,
                    nameof(name), value, exception.Message);
                errorSetter(string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message);
                return false;
            }
            finally { _ignoreChange = false; }
            errorSetter("");
            return true;
        }

        #endregion

    }
}
