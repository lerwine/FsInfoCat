using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Configuration;
using System.Data.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class RemoteConnectionConfigViewModel : NotifyErrorViewModel
    {
        private const string ERROR_MESSAGE_REQUIRED = "* Required";
        private const string ERROR_MESSAGE_FILE_NOT_FOUND = "File not found";
        private const string ERROR_MESSAGE_PATH_NOT_ABSOLUTE = "Path is not absolute";
        private const string ERROR_MESSAGE_INVALID_PATH = "Invalid path";
        private const string ERROR_MESSAGE_MIN_MAX_POOL_SIZE_INVERTED = "Max Pool Size cannot be less than Min Pool Size";
        private readonly ILogger<RemoteConnectionConfigViewModel> _logger;
        private readonly SqlConnectionStringBuilder _backingBuilder;

        public event EventHandler<SqlConnectionStringBuilderEventArgs> Advanced;

        public event EventHandler<SqlConnectionStringEventArgs> TestConnection;

        public event EventHandler<CloseWindowEventArgs> CloseWindow;

        #region Dependency Properties

        #region SelectedPropertyName

        public static readonly DependencyProperty SelectedPropertyNameProperty = DependencyProperty.Register(nameof(SelectedPropertyName), typeof(string),
            typeof(RemoteConnectionConfigViewModel), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnSelectedPropertyNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string SelectedPropertyName
        {
            get { return GetValue(SelectedPropertyNameProperty) as string; }
            set { SetValue(SelectedPropertyNameProperty, value); }
        }

        private static readonly DependencyPropertyKey SelectedPropertyTitlePropertyKey = DependencyProperty.RegisterReadOnly(nameof(SelectedPropertyTitle),
            typeof(string), typeof(RemoteConnectionConfigViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty SelectedPropertyTitleProperty = SelectedPropertyTitlePropertyKey.DependencyProperty;

        public string SelectedPropertyTitle
        {
            get { return GetValue(SelectedPropertyTitleProperty) as string; }
            private set { SetValue(SelectedPropertyTitlePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey SelectedPropertyDescriptionPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SelectedPropertyDescription),
            typeof(string), typeof(RemoteConnectionConfigViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty SelectedPropertyDescriptionProperty = SelectedPropertyDescriptionPropertyKey.DependencyProperty;

        public string SelectedPropertyDescription
        {
            get { return GetValue(SelectedPropertyDescriptionProperty) as string; }
            private set { SetValue(SelectedPropertyDescriptionPropertyKey, value); }
        }

        #endregion
        #region ApplicationName

        private static readonly DependencyPropertyKey ApplicationNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(ApplicationName), typeof(string),
            typeof(RemoteConnectionConfigViewModel), new PropertyMetadata("", null, (DependencyObject d, object baseValue) => (baseValue is string s) ? s : ""));

        public static readonly DependencyProperty ApplicationNameProperty = ApplicationNamePropertyKey.DependencyProperty;

        public string ApplicationName
        {
            get { return GetValue(ApplicationNameProperty) as string; }
            private set { SetValue(ApplicationNamePropertyKey, value); }
        }

        #endregion
        #region AsynchronousProcessing

        public static readonly DependencyProperty AsynchronousProcessingProperty = DependencyProperty.Register(nameof(AsynchronousProcessing), typeof(bool),
            typeof(RemoteConnectionConfigViewModel), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnAsynchronousProcessingPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool AsynchronousProcessing
        {
            get { return (bool)GetValue(AsynchronousProcessingProperty); }
            set { SetValue(AsynchronousProcessingProperty, value); }
        }

        #endregion
        #region AttachDbFile

        public static readonly DependencyProperty AttachDbFileProperty = DependencyProperty.Register(nameof(AttachDbFile), typeof(bool), typeof(RemoteConnectionConfigViewModel),
            new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnAttachDbFilePropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool AttachDbFile
        {
            get { return (bool)GetValue(AttachDbFileProperty); }
            set { SetValue(AttachDbFileProperty, value); }
        }

        #endregion
        #region AttachDBFilename

        public static readonly DependencyProperty AttachDBFilenameProperty = DependencyProperty.Register(nameof(AttachDBFilename), typeof(string), typeof(RemoteConnectionConfigViewModel),
            new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnAttachDBFilenamePropertyChanged(e.OldValue as string, e.NewValue as string),
                (DependencyObject d, object baseValue) => (baseValue is string s) ? s : ""));

        public string AttachDBFilename
        {
            get { return GetValue(AttachDBFilenameProperty) as string; }
            set { SetValue(AttachDBFilenameProperty, value); }
        }

        #endregion
        #region ColumnEncryptionSetting

        public static readonly DependencyProperty ColumnEncryptionSettingProperty = DependencyProperty.Register(nameof(ColumnEncryptionSetting),
            typeof(SqlConnectionColumnEncryptionSetting), typeof(RemoteConnectionConfigViewModel), new PropertyMetadata(SqlConnectionColumnEncryptionSetting.Disabled,
                (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as RemoteConnectionConfigViewModel).OnColumnEncryptionSettingPropertyChanged((SqlConnectionColumnEncryptionSetting)e.OldValue,
                        (SqlConnectionColumnEncryptionSetting)e.NewValue)));

        public SqlConnectionColumnEncryptionSetting ColumnEncryptionSetting
        {
            get { return (SqlConnectionColumnEncryptionSetting)GetValue(ColumnEncryptionSettingProperty); }
            set { SetValue(ColumnEncryptionSettingProperty, value); }
        }

        #endregion
        #region ConnectionString

        public static readonly DependencyProperty ConnectionStringProperty = DependencyProperty.Register(nameof(ConnectionString), typeof(string),
            typeof(RemoteConnectionConfigViewModel), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnConnectionStringPropertyChanged(e.OldValue as string, e.NewValue as string),
                (DependencyObject d, object baseValue) => (baseValue is string s) ? s : ""));

        public string ConnectionString
        {
            get { return GetValue(ConnectionStringProperty) as string; }
            set { SetValue(ConnectionStringProperty, value); }
        }

        #endregion
        #region ConnectRetryCount

        public static readonly DependencyProperty ConnectRetryCountProperty = DependencyProperty.Register(nameof(ConnectRetryCount), typeof(string),
            typeof(RemoteConnectionConfigViewModel), new PropertyMetadata("1", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnConnectRetryCountPropertyChanged(e.OldValue as string, e.NewValue as string),
                (DependencyObject d, object baseValue) => (baseValue is string s) ? s : ""));

        public string ConnectRetryCount
        {
            get { return GetValue(ConnectRetryCountProperty) as string; }
            set { SetValue(ConnectRetryCountProperty, value); }
        }

        #endregion
        #region ConnectRetryInterval

        public static readonly DependencyProperty ConnectRetryIntervalProperty =
            DependencyProperty.Register(nameof(ConnectRetryInterval), typeof(string), typeof(RemoteConnectionConfigViewModel),
                new PropertyMetadata("10", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as RemoteConnectionConfigViewModel).OnConnectRetryIntervalPropertyChanged(e.OldValue as string, e.NewValue as string),
                    (DependencyObject d, object baseValue) => (baseValue is string s) ? s : ""));

        public string ConnectRetryInterval
        {
            get { return GetValue(ConnectRetryIntervalProperty) as string; }
            set { SetValue(ConnectRetryIntervalProperty, value); }
        }

        #endregion
        #region ConnectTimeout

        public static readonly DependencyProperty ConnectTimeoutProperty =
            DependencyProperty.Register(nameof(ConnectTimeout), typeof(string), typeof(RemoteConnectionConfigViewModel),
                new PropertyMetadata("30", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as RemoteConnectionConfigViewModel).OnConnectTimeoutPropertyChanged(e.OldValue as string, e.NewValue as string),
                    (DependencyObject d, object baseValue) => (baseValue is string s) ? s : ""));

        public string ConnectTimeout
        {
            get { return GetValue(ConnectTimeoutProperty) as string; }
            set { SetValue(ConnectTimeoutProperty, value); }
        }

        #endregion
        #region ContextConnection

        public static readonly DependencyProperty ContextConnectionProperty = DependencyProperty.Register(nameof(ContextConnection), typeof(bool), typeof(RemoteConnectionConfigViewModel),
            new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnContextConnectionPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool ContextConnection
        {
            get { return (bool)GetValue(ContextConnectionProperty); }
            set { SetValue(ContextConnectionProperty, value); }
        }

        #endregion
        #region CurrentLanguage

        public static readonly DependencyProperty CurrentLanguageProperty = DependencyProperty.Register(nameof(CurrentLanguage), typeof(string), typeof(RemoteConnectionConfigViewModel),
            new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnCurrentLanguagePropertyChanged(e.OldValue as string, e.NewValue as string),
                (DependencyObject d, object baseValue) => (baseValue is string s) ? s : ""));

        /// <summary>
        /// The SQL Server Language record name.
        /// </summary>
        public string CurrentLanguage
        {
            get { return GetValue(CurrentLanguageProperty) as string; }
            set { SetValue(CurrentLanguageProperty, value); }
        }

        #endregion
        #region DataSource

        public static readonly DependencyProperty DataSourceProperty = DependencyProperty.Register(nameof(DataSource), typeof(string), typeof(RemoteConnectionConfigViewModel),
            new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnDataSourcePropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// The name or network address of the instance of SQL Server to connect to.
        /// </summary>
        public string DataSource
        {
            get { return GetValue(DataSourceProperty) as string; }
            set { SetValue(DataSourceProperty, value); }
        }

        public static readonly DependencyPropertyKey DataSourceErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(DataSourceError), typeof(string),
            typeof(RemoteConnectionConfigViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty DataSourceErrorProperty = DataSourceErrorPropertyKey.DependencyProperty;

        public string DataSourceError
        {
            get { return GetValue(DataSourceErrorProperty) as string; }
            private set { SetValue(DataSourceErrorPropertyKey, value); }
        }

        #endregion
        #region EnclaveAttestationUrl

        public static readonly DependencyProperty EnclaveAttestationUrlProperty = DependencyProperty.Register(nameof(EnclaveAttestationUrl), typeof(string),
            typeof(RemoteConnectionConfigViewModel), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnEnclaveAttestationUrlPropertyChanged(e.OldValue as string, e.NewValue as string),
                (DependencyObject d, object baseValue) => (baseValue is string s) ? s : ""));

        public string EnclaveAttestationUrl
        {
            get { return GetValue(EnclaveAttestationUrlProperty) as string; }
            set { SetValue(EnclaveAttestationUrlProperty, value); }
        }

        #endregion
        #region Encrypt

        public static readonly DependencyProperty EncryptProperty = DependencyProperty.Register(nameof(Encrypt), typeof(bool), typeof(RemoteConnectionConfigViewModel),
            new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnEncryptPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        /// <summary>
        /// Indicates whether SQL Server uses SSL encryption for all data sent between the client and server if the server has a certificate installed.
        /// </summary>
        public bool Encrypt
        {
            get { return (bool)GetValue(EncryptProperty); }
            set { SetValue(EncryptProperty, value); }
        }

        #endregion
        #region Enlist

        public static readonly DependencyProperty EnlistProperty = DependencyProperty.Register(nameof(Enlist), typeof(bool), typeof(RemoteConnectionConfigViewModel),
            new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnEnlistPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        /// <summary>
        /// Indicates whether the SQL Server connection pooler automatically enlists the connection in the creation thread's current transaction context.
        /// </summary>
        public bool Enlist
        {
            get { return (bool)GetValue(EnlistProperty); }
            set { SetValue(EnlistProperty, value); }
        }

        #endregion
        #region FailoverPartner

        public static readonly DependencyProperty FailoverPartnerProperty = DependencyProperty.Register(nameof(FailoverPartner), typeof(string), typeof(RemoteConnectionConfigViewModel),
            new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnFailoverPartnerPropertyChanged(e.OldValue as string, e.NewValue as string),
                (DependencyObject d, object baseValue) => (baseValue is string s) ? s : ""));

        /// <summary>
        /// The name or address of the partner server to connect to if the primary server is down.
        /// </summary>
        public string FailoverPartner
        {
            get { return GetValue(FailoverPartnerProperty) as string; }
            set { SetValue(FailoverPartnerProperty, value); }
        }

        #endregion
        #region InitialCatalog

        public static readonly DependencyProperty InitialCatalogProperty = DependencyProperty.Register(nameof(InitialCatalog), typeof(string), typeof(RemoteConnectionConfigViewModel),
            new PropertyMetadata("FsInfoCat", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnInitialCatalogPropertyChanged(e.OldValue as string, e.NewValue as string),
                (DependencyObject d, object baseValue) => (baseValue is string s) ? s : ""));

        /// <summary>
        /// The name of the database associated with the connection.
        /// </summary>
        public string InitialCatalog
        {
            get { return GetValue(InitialCatalogProperty) as string; }
            set { SetValue(InitialCatalogProperty, value); }
        }

        #endregion
        #region IntegratedSecurity

        public static readonly DependencyProperty IntegratedSecurityProperty = DependencyProperty.Register(nameof(IntegratedSecurity), typeof(bool), typeof(RemoteConnectionConfigViewModel),
            new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnIntegratedSecurityPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        /// <summary>
        /// Indicates whether the current Windows account credentials are used for authentication (when true).
        /// </summary>
        public bool IntegratedSecurity
        {
            get { return (bool)GetValue(IntegratedSecurityProperty); }
            set { SetValue(IntegratedSecurityProperty, value); }
        }

        #endregion
        #region LoadBalanceTimeout

        public static readonly DependencyProperty LoadBalanceTimeoutProperty = DependencyProperty.Register(nameof(LoadBalanceTimeout), typeof(string),
            typeof(RemoteConnectionConfigViewModel), new PropertyMetadata("0", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnLoadBalanceTimeoutPropertyChanged(e.OldValue as string, e.NewValue as string),
                (DependencyObject d, object baseValue) => (baseValue is string s) ? s : ""));

        public string LoadBalanceTimeout
        {
            get { return GetValue(LoadBalanceTimeoutProperty) as string; }
            set { SetValue(LoadBalanceTimeoutProperty, value); }
        }

        #endregion
        #region MaxPoolSize

        public static readonly DependencyProperty MaxPoolSizeProperty = DependencyProperty.Register(nameof(MaxPoolSize), typeof(string), typeof(RemoteConnectionConfigViewModel),
            new PropertyMetadata("100", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnMaxPoolSizePropertyChanged(e.OldValue as string, e.NewValue as string),
                (DependencyObject d, object baseValue) => (baseValue is string s) ? s : ""));

        public string MaxPoolSize
        {
            get { return GetValue(MaxPoolSizeProperty) as string; }
            set { SetValue(MaxPoolSizeProperty, value); }
        }

        #endregion
        #region MinPoolSize

        public static readonly DependencyProperty MinPoolSizeProperty =
            DependencyProperty.Register(nameof(MinPoolSize), typeof(string), typeof(RemoteConnectionConfigViewModel),
                new PropertyMetadata("0", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as RemoteConnectionConfigViewModel).OnMinPoolSizePropertyChanged(e.OldValue as string, e.NewValue as string),
                    (DependencyObject d, object baseValue) => (baseValue is string s) ? s : ""));

        public string MinPoolSize
        {
            get { return GetValue(MinPoolSizeProperty) as string; }
            set { SetValue(MinPoolSizeProperty, value); }
        }

        #endregion
        #region MultipleActiveResultSets

        public static readonly DependencyProperty MultipleActiveResultSetsProperty = DependencyProperty.Register(nameof(MultipleActiveResultSets), typeof(bool),
            typeof(RemoteConnectionConfigViewModel), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnMultipleActiveResultSetsPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        [DisplayName("MultipleActiveResultSets")]
        [Description("When true, an application can maintain multiple active result sets (MARS). " +
            "When false, an application must process or cancel all result sets from one batch before it can execute any other batch on that connection.")]
        public bool MultipleActiveResultSets
        {
            get { return (bool)GetValue(MultipleActiveResultSetsProperty); }
            set { SetValue(MultipleActiveResultSetsProperty, value); }
        }

        #endregion
        #region MultiSubnetFailover

        public static readonly DependencyProperty MultiSubnetFailoverProperty = DependencyProperty.Register(nameof(MultiSubnetFailover), typeof(bool),
            typeof(RemoteConnectionConfigViewModel), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnMultiSubnetFailoverPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool MultiSubnetFailover
        {
            get { return (bool)GetValue(MultiSubnetFailoverProperty); }
            set { SetValue(MultiSubnetFailoverProperty, value); }
        }

        #endregion
        #region NetworkLibrary

        public static readonly DependencyProperty NetworkLibraryProperty = DependencyProperty.Register(nameof(NetworkLibrary), typeof(string), typeof(RemoteConnectionConfigViewModel),
            new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnNetworkLibraryPropertyChanged(e.OldValue as string, e.NewValue as string),
                (DependencyObject d, object baseValue) => (baseValue is string s) ? s : ""));

        [DisplayName("Network Library")]
        [Description("The name of the network library used to establish a connection to the SQL Server.")]
        public string NetworkLibrary
        {
            get { return GetValue(NetworkLibraryProperty) as string; }
            set { SetValue(NetworkLibraryProperty, value); }
        }

        #endregion
        #region PacketSize

        public static readonly DependencyProperty PacketSizeProperty =
            DependencyProperty.Register(nameof(PacketSize), typeof(string), typeof(RemoteConnectionConfigViewModel),
                new PropertyMetadata("8000", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as RemoteConnectionConfigViewModel).OnPacketSizePropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string PacketSize
        {
            get { return GetValue(PacketSizeProperty) as string; }
            set { SetValue(PacketSizeProperty, value); }
        }

        #endregion
        #region Password

        public static readonly DependencyProperty PasswordProperty = DependencyProperty.Register(nameof(Password), typeof(string), typeof(RemoteConnectionConfigViewModel),
            new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnPasswordPropertyChanged(e.OldValue as string, e.NewValue as string),
                (DependencyObject d, object baseValue) => (baseValue is string s) ? s : ""));

        public string Password
        {
            get { return GetValue(PasswordProperty) as string; }
            set { SetValue(PasswordProperty, value); }
        }

        #endregion
        #region PersistSecurityInfo

        public static readonly DependencyProperty PersistSecurityInfoProperty = DependencyProperty.Register(nameof(PersistSecurityInfo), typeof(bool),
            typeof(RemoteConnectionConfigViewModel),
            new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnPersistSecurityInfoPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool PersistSecurityInfo
        {
            get { return (bool)GetValue(PersistSecurityInfoProperty); }
            set { SetValue(PersistSecurityInfoProperty, value); }
        }

        #endregion
        #region PoolBlockingPeriod

        public static readonly DependencyProperty PoolBlockingPeriodProperty = DependencyProperty.Register(nameof(PoolBlockingPeriod), typeof(PoolBlockingPeriod),
            typeof(RemoteConnectionConfigViewModel),
            new PropertyMetadata(PoolBlockingPeriod.Auto, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnPoolBlockingPeriodPropertyChanged((PoolBlockingPeriod)e.OldValue, (PoolBlockingPeriod)e.NewValue)));

        public PoolBlockingPeriod PoolBlockingPeriod
        {
            get { return (PoolBlockingPeriod)GetValue(PoolBlockingPeriodProperty); }
            set { SetValue(PoolBlockingPeriodProperty, value); }
        }

        #endregion
        #region Pooling

        public static readonly DependencyProperty PoolingProperty = DependencyProperty.Register(nameof(Pooling), typeof(bool), typeof(RemoteConnectionConfigViewModel),
            new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnPoolingPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool Pooling
        {
            get { return (bool)GetValue(PoolingProperty); }
            set { SetValue(PoolingProperty, value); }
        }

        #endregion
        #region Replication

        public static readonly DependencyProperty ReplicationProperty = DependencyProperty.Register(nameof(Replication), typeof(bool), typeof(RemoteConnectionConfigViewModel),
            new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnReplicationPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool Replication
        {
            get { return (bool)GetValue(ReplicationProperty); }
            set { SetValue(ReplicationProperty, value); }
        }

        #endregion
        #region TransparentNetworkIPResolution

        public static readonly DependencyProperty TransparentNetworkIPResolutionProperty = DependencyProperty.Register(nameof(TransparentNetworkIPResolution), typeof(bool),
            typeof(RemoteConnectionConfigViewModel), new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnTransparentNetworkIPResolutionPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool TransparentNetworkIPResolution
        {
            get { return (bool)GetValue(TransparentNetworkIPResolutionProperty); }
            set { SetValue(TransparentNetworkIPResolutionProperty, value); }
        }

        #endregion
        #region TrustServerCertificate

        public static readonly DependencyProperty TrustServerCertificateProperty = DependencyProperty.Register(nameof(TrustServerCertificate), typeof(bool),
            typeof(RemoteConnectionConfigViewModel), new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnTrustServerCertificatePropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool TrustServerCertificate
        {
            get { return (bool)GetValue(TrustServerCertificateProperty); }
            set { SetValue(TrustServerCertificateProperty, value); }
        }

        #endregion
        #region UserID

        public static readonly DependencyProperty UserIDProperty = DependencyProperty.Register(nameof(UserID), typeof(string), typeof(RemoteConnectionConfigViewModel),
                new PropertyMetadata("fsinfocatadmin", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as RemoteConnectionConfigViewModel).OnUserIDPropertyChanged(e.OldValue as string, e.NewValue as string),
                    (DependencyObject d, object baseValue) => (baseValue is string s) ? s : ""));

        public string UserID
        {
            get { return GetValue(UserIDProperty) as string; }
            set { SetValue(UserIDProperty, value); }
        }

        #endregion
        #region UserInstance

        public static readonly DependencyProperty UserInstanceProperty = DependencyProperty.Register(nameof(UserInstance), typeof(bool), typeof(RemoteConnectionConfigViewModel),
            new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as RemoteConnectionConfigViewModel).OnUserInstancePropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool UserInstance
        {
            get { return (bool)GetValue(UserInstanceProperty); }
            set { SetValue(UserInstanceProperty, value); }
        }

        public static readonly DependencyPropertyKey UserInstanceErrorPropertyKey = DependencyProperty.RegisterReadOnly(nameof(UserInstanceError), typeof(string),
            typeof(RemoteConnectionConfigViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty UserInstanceErrorProperty = UserInstanceErrorPropertyKey.DependencyProperty;

        public string UserInstanceError
        {
            get { return GetValue(UserInstanceErrorProperty) as string; }
            private set { SetValue(UserInstanceErrorPropertyKey, value); }
        }

        #endregion
        #region WorkstationID

        private static readonly DependencyPropertyKey WorkstationIDPropertyKey = DependencyProperty.RegisterReadOnly(nameof(WorkstationID), typeof(string),
            typeof(RemoteConnectionConfigViewModel), new PropertyMetadata("", null, (DependencyObject d, object baseValue) => (baseValue is string s) ? s : ""));

        public static readonly DependencyProperty WorkstationIDProperty = WorkstationIDPropertyKey.DependencyProperty;

        public string WorkstationID
        {
            get { return GetValue(WorkstationIDProperty) as string; }
            private set { SetValue(WorkstationIDPropertyKey, value); }
        }

        #endregion

        #endregion

        #region Commands

        private static readonly DependencyPropertyKey SetSelectedPropertyNameCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SetSelectedPropertyNameCommand),
            typeof(Commands.RelayCommand), typeof(RemoteConnectionConfigViewModel), new PropertyMetadata(null, null, (DependencyObject d, object baseValue) =>
                (baseValue is Commands.RelayCommand rc) ? rc : new Commands.RelayCommand(((RemoteConnectionConfigViewModel)d).OnSetSelectedPropertyNameExecute)));

        public static readonly DependencyProperty SetSelectedPropertyNameCommandProperty = SetSelectedPropertyNameCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand SetSelectedPropertyNameCommand => (Commands.RelayCommand)GetValue(SetSelectedPropertyNameCommandProperty);

        private void OnSetSelectedPropertyNameExecute(object parameter)
        {
            if (parameter is string propertyName)
                SelectedPropertyName = propertyName;
        }

        private static readonly DependencyPropertyKey AdvancedCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(AdvancedCommand),
            typeof(Commands.RelayCommand), typeof(RemoteConnectionConfigViewModel), new PropertyMetadata(null, null, (DependencyObject d, object baseValue) =>
                (baseValue is Commands.RelayCommand rc) ? rc : new Commands.RelayCommand(() =>
                {
                    if (d is RemoteConnectionConfigViewModel vm)
                        vm.OnAdvancedExecute();
                })));

        public static readonly DependencyProperty AdvancedCommandProperty = AdvancedCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand AdvancedCommand => (Commands.RelayCommand)GetValue(AdvancedCommandProperty);

        private static readonly DependencyPropertyKey TestConnectionCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(TestConnectionCommand),
            typeof(Commands.RelayCommand), typeof(RemoteConnectionConfigViewModel), new PropertyMetadata(null, null, (DependencyObject d, object baseValue) =>
                (baseValue is Commands.RelayCommand rc) ? rc : new Commands.RelayCommand(() =>
                {
                    if (d is RemoteConnectionConfigViewModel vm)
                        vm.OnTestConnectionExecute();
                })));

        public static readonly DependencyProperty TestConnectionCommandProperty = TestConnectionCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand TestConnectionCommand => (Commands.RelayCommand)GetValue(TestConnectionCommandProperty);

        private static readonly DependencyPropertyKey SaveCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(SaveCommand),
            typeof(Commands.RelayCommand), typeof(RemoteConnectionConfigViewModel), new PropertyMetadata(null, null, (DependencyObject d, object baseValue) =>
                (baseValue is Commands.RelayCommand rc) ? rc : new Commands.RelayCommand(() =>
                {
                    if (d is RemoteConnectionConfigViewModel vm)
                        vm.OnSave();
                })));

        public static readonly DependencyProperty SaveCommandProperty = SaveCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand SaveCommand => (Commands.RelayCommand)GetValue(SaveCommandProperty);

        private static readonly DependencyPropertyKey CancelCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CancelCommand),
            typeof(Commands.RelayCommand), typeof(RemoteConnectionConfigViewModel), new PropertyMetadata(null, null, (DependencyObject d, object baseValue) =>
                (baseValue is Commands.RelayCommand rc) ? rc : new Commands.RelayCommand(() =>
                {
                    if (d is RemoteConnectionConfigViewModel vm)
                        vm.CloseWindow?.Invoke(vm, new CloseWindowEventArgs(false));
                })));

        public static readonly DependencyProperty CancelCommandProperty = CancelCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand CancelCommand => (Commands.RelayCommand)GetValue(CancelCommandProperty);

        #endregion

        public RemoteConnectionConfigViewModel()
        {
            _logger = App.LoggerFactory.CreateLogger<RemoteConnectionConfigViewModel>();
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

        private void OnAdvancedExecute()
        {
            AdvancedCommand.IsEnabled = false;
            Advanced?.Invoke(this, new SqlConnectionStringBuilderEventArgs(_backingBuilder, Dispatcher.ToInvocationAction<SqlConnectionStringBuilder>(builder =>
            {
                try
                {
                    if (!(builder is null))
                        ConnectionString = builder.ConnectionString;
                }
                finally { AdvancedCommand.IsEnabled = true; }
            }, true)));
        }

        private void OnTestConnectionExecute()
        {
            AdvancedCommand.IsEnabled = false;
            TestConnection?.Invoke(this, new SqlConnectionStringEventArgs(_backingBuilder.ConnectionString, Dispatcher.ToInvocationAction(() => AdvancedCommand.IsEnabled = true, true)));
        }

        protected virtual void OnSave()
        {
            SaveCommand.IsEnabled = false;
            Task.Factory.StartNew(() => AppConfig.SetRemoteDbContainerConnectionString(_backingBuilder.ConnectionString)).ContinueWith(task => Dispatcher.Invoke(() =>
            {
                try
                {
                    if (task.Status == TaskStatus.RanToCompletion)
                        CloseWindow?.Invoke(this, new CloseWindowEventArgs(true));
                }
                finally { SaveCommand.IsEnabled = true; }
            }));
        }

        protected virtual void OnSelectedPropertyNamePropertyChanged(string oldValue, string newValue)
        {
            PropertyInfo propertyInfo = GetType().GetProperty(newValue);
            SelectedPropertyDescription = propertyInfo.GetCustomAttributes<DescriptionAttribute>().Select(d => d.Description)
                .Where(d => !string.IsNullOrWhiteSpace(d)).DefaultIfEmpty("").First();
            SelectedPropertyTitle = propertyInfo.GetCustomAttributes<DisplayNameAttribute>().Select(d => d.DisplayName)
                .Where(d => !string.IsNullOrWhiteSpace(d)).DefaultIfEmpty(newValue).First();
        }

        protected virtual void OnAsynchronousProcessingPropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                UpdateModelProperty(_backingBuilder, nameof(AsynchronousProcessing), newValue, (m, v) => m.AsynchronousProcessing = v,
                    m => ConnectionString = m.ConnectionString);
        }

        protected virtual void OnAttachDbFilePropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue == newValue)
                return;
            if (newValue)
            {
                UpdateModelProperty(_backingBuilder, nameof(AttachDBFilename), AttachDBFilename, (m, v) =>
                {
                    m.InitialCatalog = "";
                    m.AttachDBFilename = v;
                }, m => ConnectionString = m.ConnectionString, CheckAttachDbFilename);
                ClearErrors("", nameof(InitialCatalog));
            }
            else
            {
                UpdateModelProperty(_backingBuilder, nameof(InitialCatalog), InitialCatalog, (m, v) =>
                {
                    m.AttachDBFilename = "";
                    m.InitialCatalog = v;
                }, m => ConnectionString = m.ConnectionString, s => string.IsNullOrWhiteSpace(s) ? ERROR_MESSAGE_REQUIRED : null);
                ClearErrors("", nameof(AttachDBFilename));
            }
        }

        private string CheckAttachDbFilename(string attachDBFilename)
        {
            if (AttachDbFile)
            {
                if (string.IsNullOrWhiteSpace(attachDBFilename))
                    return ERROR_MESSAGE_REQUIRED;
                try
                {
                    if (!System.IO.File.Exists(attachDBFilename))
                        return ERROR_MESSAGE_FILE_NOT_FOUND;
                    else if (!System.IO.Path.IsPathRooted(attachDBFilename))
                        return ERROR_MESSAGE_PATH_NOT_ABSOLUTE;
                }
                catch
                {
                    return ERROR_MESSAGE_INVALID_PATH;
                }
            }
            return null;
        }

        protected virtual void OnAttachDBFilenamePropertyChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue && AttachDbFile)
                UpdateModelProperty(_backingBuilder, nameof(AttachDBFilename), newValue, (m, v) => m.AttachDBFilename = v,
                    m => ConnectionString = m.ConnectionString, CheckAttachDbFilename);
        }

        protected virtual void OnColumnEncryptionSettingPropertyChanged(SqlConnectionColumnEncryptionSetting oldValue, SqlConnectionColumnEncryptionSetting newValue)
        {
            if (oldValue != newValue)
                UpdateModelProperty(_backingBuilder, nameof(ColumnEncryptionSetting), ColumnEncryptionSetting, (m, v) => m.ColumnEncryptionSetting = v,
                    m => ConnectionString = m.ConnectionString);
        }

        protected virtual void OnConnectRetryCountPropertyChanged(string oldValue, string newValue)
        {
            if (oldValue == newValue)
                return;
            UpdateParsedIntModelProperty(_backingBuilder, nameof(ConnectRetryCount), newValue, (m, i) =>
            {
                if (i.HasValue)
                    m.ConnectRetryCount = i.Value;
                else
                    m.Remove("ConnectRetryCount");
            }, m => ConnectionString = m.ConnectionString);
        }

        protected virtual void OnConnectRetryIntervalPropertyChanged(string oldValue, string newValue)
        {
            if (oldValue == newValue)
                return;
            UpdateParsedIntModelProperty(_backingBuilder, nameof(ConnectRetryInterval), newValue, (m, i) =>
            {
                if (i.HasValue)
                    m.ConnectRetryInterval = i.Value;
                else
                    m.Remove("ConnectRetryInterval");
            }, m => ConnectionString = m.ConnectionString);
        }

        protected virtual void OnConnectTimeoutPropertyChanged(string oldValue, string newValue)
        {
            if (oldValue == newValue)
                return;
            UpdateParsedIntModelProperty(_backingBuilder, nameof(ConnectTimeout), newValue, (m, i) =>
            {
                if (i.HasValue)
                    m.ConnectTimeout = i.Value;
                else
                    m.Remove("Connect Timeout");
            }, m => ConnectionString = m.ConnectionString);
        }

        protected virtual void OnContextConnectionPropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                UpdateModelProperty(_backingBuilder, nameof(ContextConnection), newValue, (m, v) => m.ContextConnection = v,
                    m => ConnectionString = m.ConnectionString);
        }

        protected virtual void OnCurrentLanguagePropertyChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue)
                UpdateModelProperty(_backingBuilder, nameof(CurrentLanguage), newValue, (m, v) => m.CurrentLanguage = v,
                    m => ConnectionString = m.ConnectionString);
        }

        protected virtual void OnDataSourcePropertyChanged(string oldValue, string newValue)
        {
            UpdateModelProperty(_backingBuilder, nameof(DataSource), newValue, (m, v) => m.DataSource = v,
                m => ConnectionString = m.ConnectionString, s => string.IsNullOrWhiteSpace(s) ? ERROR_MESSAGE_REQUIRED : null);
        }

        protected virtual void OnEnclaveAttestationUrlPropertyChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue)
                UpdateModelProperty(_backingBuilder, nameof(EnclaveAttestationUrl), newValue, (m, v) => m.EnclaveAttestationUrl = v,
                    m => ConnectionString = m.ConnectionString);
        }

        protected virtual void OnEncryptPropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                UpdateModelProperty(_backingBuilder, nameof(Encrypt), newValue, (m, v) => m.Encrypt = v,
                    m => ConnectionString = m.ConnectionString);
        }

        protected virtual void OnEnlistPropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                UpdateModelProperty(_backingBuilder, nameof(Enlist), newValue, (m, v) => m.Enlist = v,
                    m => ConnectionString = m.ConnectionString);
        }

        protected virtual void OnFailoverPartnerPropertyChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue)
                UpdateModelProperty(_backingBuilder, nameof(FailoverPartner), newValue, (m, v) => m.FailoverPartner = v,
                    m => ConnectionString = m.ConnectionString);
        }

        // TOOD: Validate not empty
        protected virtual void OnInitialCatalogPropertyChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue && !AttachDbFile)
                UpdateModelProperty(_backingBuilder, nameof(InitialCatalog), newValue, (m, v) => m.InitialCatalog = v,
                    m => ConnectionString = m.ConnectionString, s => string.IsNullOrWhiteSpace(s) ? ERROR_MESSAGE_REQUIRED : null);
        }

        protected virtual void OnIntegratedSecurityPropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                UpdateModelProperty(_backingBuilder, nameof(IntegratedSecurity), newValue, (m, v) =>
                {
                    if (v)
                    {
                        ClearErrors("", nameof(UserID));
                        ClearErrors("", nameof(Password));
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(UserID))
                            SetError(UserID, ERROR_MESSAGE_REQUIRED, nameof(UserID));
                        if (string.IsNullOrWhiteSpace(Password))
                            SetError(Password, ERROR_MESSAGE_REQUIRED, nameof(Password));
                    }
                    m.IntegratedSecurity = v;
                }, m => ConnectionString = m.ConnectionString);
        }

        protected virtual void OnLoadBalanceTimeoutPropertyChanged(string oldValue, string newValue)
        {
            if (oldValue == newValue)
                return;
            UpdateParsedIntModelProperty(_backingBuilder, nameof(LoadBalanceTimeout), newValue, (m, i) =>
            {
                if (i.HasValue)
                    m.LoadBalanceTimeout = i.Value;
                else
                    m.Remove("Load Balance Timeout");
            }, m => ConnectionString = m.ConnectionString);
        }

        protected virtual void OnMaxPoolSizePropertyChanged(string oldValue, string newValue)
        {
            if (oldValue == newValue)
                return;
            UpdateParsedIntModelProperty(_backingBuilder, nameof(MaxPoolSize), newValue, (m, i) =>
            {
                if (i.HasValue)
                    m.MaxPoolSize = i.Value;
                else
                    m.Remove("Max Pool Size");
            }, m => ConnectionString = m.ConnectionString, i => ((i ?? _backingBuilder.MaxPoolSize) < _backingBuilder.MinPoolSize) ?
            ERROR_MESSAGE_MIN_MAX_POOL_SIZE_INVERTED : null);
        }

        protected virtual void OnMinPoolSizePropertyChanged(string oldValue, string newValue)
        {
            if (oldValue == newValue)
                return;
            UpdateParsedIntModelProperty(_backingBuilder, nameof(MinPoolSize), newValue, (m, i) =>
            {
                int s;
                if (i.HasValue)
                    s = m.MinPoolSize = i.Value;
                else
                {
                    m.Remove("Min Pool Size");
                    s = _backingBuilder.MinPoolSize;
                }
                if (s > _backingBuilder.MaxPoolSize)
                    SetError(MaxPoolSize, ERROR_MESSAGE_MIN_MAX_POOL_SIZE_INVERTED, nameof(MaxPoolSize));
                else
                    ClearErrors(MaxPoolSize, nameof(MaxPoolSize));
            }, m => ConnectionString = m.ConnectionString);
        }

        protected virtual void OnMultipleActiveResultSetsPropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                UpdateModelProperty(_backingBuilder, nameof(MultipleActiveResultSets), newValue, (m, v) => m.MultipleActiveResultSets = v,
                    m => ConnectionString = m.ConnectionString);
        }

        protected virtual void OnMultiSubnetFailoverPropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                UpdateModelProperty(_backingBuilder, nameof(MultiSubnetFailover), newValue, (m, v) => m.MultiSubnetFailover = v,
                    m => ConnectionString = m.ConnectionString);
        }

        protected virtual void OnNetworkLibraryPropertyChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue)
                UpdateModelProperty(_backingBuilder, nameof(NetworkLibrary), newValue, (m, v) => m.NetworkLibrary = v,
                    m => ConnectionString = m.ConnectionString);
        }

        protected virtual void OnPacketSizePropertyChanged(string oldValue, string newValue)
        {
            if (oldValue == newValue)
                return;
            UpdateParsedIntModelProperty(_backingBuilder, nameof(PacketSize), newValue, (m, i) =>
            {
                if (i.HasValue)
                    m.PacketSize = i.Value;
                else
                    m.Remove("Packet Size");
            }, m => ConnectionString = m.ConnectionString);
        }

        protected virtual void OnPasswordPropertyChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue && !IntegratedSecurity)
                UpdateModelProperty(_backingBuilder, nameof(Password), newValue, (m, v) => m.Password = v,
                    m => ConnectionString = m.ConnectionString, s => string.IsNullOrWhiteSpace(s) ? ERROR_MESSAGE_REQUIRED : null);
        }

        protected virtual void OnPersistSecurityInfoPropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                UpdateModelProperty(_backingBuilder, nameof(PersistSecurityInfo), newValue, (m, v) => m.PersistSecurityInfo = v,
                    m => ConnectionString = m.ConnectionString);
        }

        protected virtual void OnPoolBlockingPeriodPropertyChanged(PoolBlockingPeriod oldValue, PoolBlockingPeriod newValue)
        {
            if (oldValue != newValue)
                UpdateModelProperty(_backingBuilder, nameof(PoolBlockingPeriod), newValue, (m, v) => m.PoolBlockingPeriod = v,
                    m => ConnectionString = m.ConnectionString);
        }

        protected virtual void OnPoolingPropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                UpdateModelProperty(_backingBuilder, nameof(Pooling), newValue, (m, v) => m.Pooling = v,
                    m => ConnectionString = m.ConnectionString);
        }

        protected virtual void OnReplicationPropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                UpdateModelProperty(_backingBuilder, nameof(Replication), newValue, (m, v) => m.Replication = v,
                    m => ConnectionString = m.ConnectionString);
        }

        protected virtual void OnTransparentNetworkIPResolutionPropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                UpdateModelProperty(_backingBuilder, nameof(TransparentNetworkIPResolution), newValue, (m, v) => m.TransparentNetworkIPResolution = v,
                    m => ConnectionString = m.ConnectionString);
        }

        protected virtual void OnTrustServerCertificatePropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                UpdateModelProperty(_backingBuilder, nameof(TrustServerCertificate), newValue, (m, v) => m.TrustServerCertificate = v,
                    m => ConnectionString = m.ConnectionString);
        }

        protected virtual void OnUserIDPropertyChanged(string oldValue, string newValue)
        {
            if (oldValue != newValue && !IntegratedSecurity)
                UpdateModelProperty(_backingBuilder, nameof(UserID), newValue, (m, v) => m.UserID = v,
                    m => ConnectionString = m.ConnectionString, s => string.IsNullOrWhiteSpace(s) ? ERROR_MESSAGE_REQUIRED : null);
        }

        protected virtual void OnUserInstancePropertyChanged(bool oldValue, bool newValue)
        {
            if (oldValue != newValue)
                UpdateModelProperty(_backingBuilder, nameof(UserInstance), newValue, (m, v) => m.UserInstance = v,
                    m => ConnectionString = m.ConnectionString);
        }

        protected virtual void OnConnectionStringPropertyChanged(string oldValue, string newValue)
        {
            if (IsModelUpdating() || oldValue == newValue)
                return;
            UpdateModelProperty(_backingBuilder, nameof(ConnectionString), newValue, (m, v) =>
            {
                m.ConnectionString = v;
                RefreshFromConnectionString();
            });
        }

        private void RefreshFromConnectionString()
        {
            ApplicationName = _backingBuilder.ApplicationName;
            WorkstationID = _backingBuilder.WorkstationID;
            if (_backingBuilder.AsynchronousProcessing != AsynchronousProcessing)
                try { AsynchronousProcessing = _backingBuilder.AsynchronousProcessing; } catch { /* okay to ignore */ }
            if (_backingBuilder.ColumnEncryptionSetting != ColumnEncryptionSetting)
                try { ColumnEncryptionSetting = _backingBuilder.ColumnEncryptionSetting; } catch { /* okay to ignore */ }
            if (_backingBuilder.ConnectionString != ConnectionString)
                try { ConnectionString = _backingBuilder.ConnectionString; } catch { /* okay to ignore */ }
            string s = _backingBuilder.ConnectRetryCount.ToString();
            if (s != ConnectRetryCount)
                try { ConnectRetryCount = s; } catch { /* okay to ignore */ }
            s = _backingBuilder.ConnectRetryInterval.ToString();
            if (s != ConnectRetryInterval)
                try { ConnectRetryInterval = s; } catch { /* okay to ignore */ }
            s = _backingBuilder.ConnectTimeout.ToString();
            if (s != ConnectTimeout)
                try { ConnectTimeout = s; } catch { /* okay to ignore */ }
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
            s = _backingBuilder.LoadBalanceTimeout.ToString();
            if (s != LoadBalanceTimeout)
                try { LoadBalanceTimeout = s; } catch { /* okay to ignore */ }
            s = _backingBuilder.MaxPoolSize.ToString();
            if (s != MaxPoolSize)
                try { MaxPoolSize = s; } catch { /* okay to ignore */ }
            s = _backingBuilder.MinPoolSize.ToString();
            if (s != MinPoolSize)
                try { MinPoolSize = s; } catch { /* okay to ignore */ }
            if (_backingBuilder.MultipleActiveResultSets != MultipleActiveResultSets)
                try { MultipleActiveResultSets = _backingBuilder.MultipleActiveResultSets; } catch { /* okay to ignore */ }
            if (_backingBuilder.MultiSubnetFailover != MultiSubnetFailover)
                try { MultiSubnetFailover = _backingBuilder.MultiSubnetFailover; } catch { /* okay to ignore */ }
            if (_backingBuilder.NetworkLibrary != NetworkLibrary)
                try { NetworkLibrary = _backingBuilder.NetworkLibrary; } catch { /* okay to ignore */ }
            s = _backingBuilder.PacketSize.ToString();
            if (s != PacketSize)
                try { PacketSize = s; } catch { /* okay to ignore */ }
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

        protected override void LogException(Exception exception, string message) => _logger.LogError(exception, message);

        protected override void LogException(Exception exception, string message, params object[] args) => _logger.LogError(exception, message, args);

        #endregion

    }
}
