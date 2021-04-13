using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class DbConnectionViewModel : DependencyObject
    {
        private bool _ignoreChange = false;
        private SqlConnectionStringBuilder _backingBuilder = new SqlConnectionStringBuilder();

        public static readonly DependencyProperty ApplicationIntentProperty =
            DependencyProperty.Register(nameof(ApplicationIntent), typeof(ApplicationIntent), typeof(DbConnectionViewModel),
                new PropertyMetadata(ApplicationIntent.ReadWrite, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnApplicationIntentPropertyChanged((ApplicationIntent)e.OldValue, (ApplicationIntent)e.NewValue)));

        public ApplicationIntent ApplicationIntent
        {
            get { return (ApplicationIntent)GetValue(ApplicationIntentProperty); }
            set { this.SetValue(ApplicationIntentProperty, value); }
        }

        public static readonly DependencyProperty ApplicationNameProperty =
            DependencyProperty.Register(nameof(ApplicationName), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnApplicationNamePropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string ApplicationName
        {
            get { return GetValue(ApplicationNameProperty) as string; }
            set { SetValue(ApplicationNameProperty, value); }
        }

        public static readonly DependencyProperty AsynchronousProcessingProperty =
            DependencyProperty.Register(nameof(AsynchronousProcessing), typeof(bool), typeof(DbConnectionViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnAsynchronousProcessingPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool AsynchronousProcessing
        {
            get { return (bool)GetValue(AsynchronousProcessingProperty); }
            set { this.SetValue(AsynchronousProcessingProperty, value); }
        }

        public static readonly DependencyProperty AuthenticationProperty =
            DependencyProperty.Register(nameof(Authentication), typeof(SqlAuthenticationMethod), typeof(DbConnectionViewModel),
                new PropertyMetadata(SqlAuthenticationMethod.NotSpecified, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnAuthenticationPropertyChanged((SqlAuthenticationMethod)e.OldValue, (SqlAuthenticationMethod)e.NewValue)));

        public SqlAuthenticationMethod Authentication
        {
            get { return (SqlAuthenticationMethod)GetValue(AuthenticationProperty); }
            set { this.SetValue(AuthenticationProperty, value); }
        }

        public static readonly DependencyProperty BrowsableConnectionStringProperty =
            DependencyProperty.Register(nameof(BrowsableConnectionString), typeof(bool), typeof(DbConnectionViewModel),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnBrowsableConnectionStringPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool BrowsableConnectionString
        {
            get { return (bool)GetValue(BrowsableConnectionStringProperty); }
            set { this.SetValue(BrowsableConnectionStringProperty, value); }
        }

        public static readonly DependencyProperty ColumnEncryptionSettingProperty =
            DependencyProperty.Register(nameof(ColumnEncryptionSetting), typeof(SqlConnectionColumnEncryptionSetting), typeof(DbConnectionViewModel),
                new PropertyMetadata(SqlConnectionColumnEncryptionSetting.Disabled, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnColumnEncryptionSettingPropertyChanged((SqlConnectionColumnEncryptionSetting)e.OldValue, (SqlConnectionColumnEncryptionSetting)e.NewValue)));

        public SqlConnectionColumnEncryptionSetting ColumnEncryptionSetting
        {
            get { return (SqlConnectionColumnEncryptionSetting)GetValue(ColumnEncryptionSettingProperty); }
            set { this.SetValue(ColumnEncryptionSettingProperty, value); }
        }

        public static readonly DependencyProperty ConnectionStringProperty =
            DependencyProperty.Register(nameof(ConnectionString), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnConnectionStringPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string ConnectionString
        {
            get { return GetValue(ConnectionStringProperty) as string; }
            set { SetValue(ConnectionStringProperty, value); }
        }

        public static readonly DependencyProperty ConnectRetryCountProperty =
            DependencyProperty.Register(nameof(ConnectRetryCount), typeof(int), typeof(DbConnectionViewModel),
                new PropertyMetadata(1, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnConnectRetryCountPropertyChanged((int)e.OldValue, (int)e.NewValue)));

        public int ConnectRetryCount
        {
            get { return (int)GetValue(ConnectRetryCountProperty); }
            set { this.SetValue(ConnectRetryCountProperty, value); }
        }

        public static readonly DependencyProperty ConnectRetryIntervalProperty =
            DependencyProperty.Register(nameof(ConnectRetryInterval), typeof(int), typeof(DbConnectionViewModel),
                new PropertyMetadata(10, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnConnectRetryIntervalPropertyChanged((int)e.OldValue, (int)e.NewValue)));

        public int ConnectRetryInterval
        {
            get { return (int)GetValue(ConnectRetryIntervalProperty); }
            set { this.SetValue(ConnectRetryIntervalProperty, value); }
        }

        public static readonly DependencyProperty ConnectTimeoutProperty =
            DependencyProperty.Register(nameof(ConnectTimeout), typeof(int), typeof(DbConnectionViewModel),
                new PropertyMetadata(30, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnConnectTimeoutPropertyChanged((int)e.OldValue, (int)e.NewValue)));

        public int ConnectTimeout
        {
            get { return (int)GetValue(ConnectTimeoutProperty); }
            set { this.SetValue(ConnectTimeoutProperty, value); }
        }

        public static readonly DependencyProperty ContextConnectionProperty =
            DependencyProperty.Register(nameof(ContextConnection), typeof(bool), typeof(DbConnectionViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnContextConnectionPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool ContextConnection
        {
            get { return (bool)GetValue(ContextConnectionProperty); }
            set { this.SetValue(ContextConnectionProperty, value); }
        }

        public static readonly DependencyProperty CurrentLanguageProperty =
            DependencyProperty.Register(nameof(CurrentLanguage), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnCurrentLanguagePropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string CurrentLanguage
        {
            get { return GetValue(CurrentLanguageProperty) as string; }
            set { SetValue(CurrentLanguageProperty, value); }
        }

        public static readonly DependencyProperty DataSourceProperty =
            DependencyProperty.Register(nameof(DataSource), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnDataSourcePropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string DataSource
        {
            get { return GetValue(DataSourceProperty) as string; }
            set { SetValue(DataSourceProperty, value); }
        }

        public static readonly DependencyProperty EnclaveAttestationUrlProperty =
            DependencyProperty.Register(nameof(EnclaveAttestationUrl), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnEnclaveAttestationUrlPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string EnclaveAttestationUrl
        {
            get { return GetValue(EnclaveAttestationUrlProperty) as string; }
            set { SetValue(EnclaveAttestationUrlProperty, value); }
        }

        public static readonly DependencyProperty EncryptProperty =
            DependencyProperty.Register(nameof(Encrypt), typeof(bool), typeof(DbConnectionViewModel),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnEncryptPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool Encrypt
        {
            get { return (bool)GetValue(EncryptProperty); }
            set { this.SetValue(EncryptProperty, value); }
        }

        public static readonly DependencyProperty EnlistProperty =
            DependencyProperty.Register(nameof(Enlist), typeof(bool), typeof(DbConnectionViewModel),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnEnlistPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool Enlist
        {
            get { return (bool)GetValue(EnlistProperty); }
            set { this.SetValue(EnlistProperty, value); }
        }

        public static readonly DependencyProperty FailoverPartnerProperty =
            DependencyProperty.Register(nameof(FailoverPartner), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnFailoverPartnerPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string FailoverPartner
        {
            get { return GetValue(FailoverPartnerProperty) as string; }
            set { SetValue(FailoverPartnerProperty, value); }
        }

        public static readonly DependencyProperty InitialCatalogProperty =
            DependencyProperty.Register(nameof(InitialCatalog), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata("FsInfoCat", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnInitialCatalogPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string InitialCatalog
        {
            get { return GetValue(InitialCatalogProperty) as string; }
            set { SetValue(InitialCatalogProperty, value); }
        }

        public static readonly DependencyProperty IntegratedSecurityProperty =
            DependencyProperty.Register(nameof(IntegratedSecurity), typeof(bool), typeof(DbConnectionViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnIntegratedSecurityPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool IntegratedSecurity
        {
            get { return (bool)GetValue(IntegratedSecurityProperty); }
            set { this.SetValue(IntegratedSecurityProperty, value); }
        }

        public static readonly DependencyProperty LoadBalanceTimeoutProperty =
            DependencyProperty.Register(nameof(LoadBalanceTimeout), typeof(int), typeof(DbConnectionViewModel),
                new PropertyMetadata(0, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnLoadBalanceTimeoutPropertyChanged((int)e.OldValue, (int)e.NewValue)));

        public int LoadBalanceTimeout
        {
            get { return (int)GetValue(LoadBalanceTimeoutProperty); }
            set { this.SetValue(LoadBalanceTimeoutProperty, value); }
        }

        public static readonly DependencyProperty MaxPoolSizeProperty =
            DependencyProperty.Register(nameof(MaxPoolSize), typeof(int), typeof(DbConnectionViewModel),
                new PropertyMetadata(100, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnMaxPoolSizePropertyChanged((int)e.OldValue, (int)e.NewValue)));

        public int MaxPoolSize
        {
            get { return (int)GetValue(MaxPoolSizeProperty); }
            set { this.SetValue(MaxPoolSizeProperty, value); }
        }

        public static readonly DependencyProperty MinPoolSizeProperty =
            DependencyProperty.Register(nameof(MinPoolSize), typeof(int), typeof(DbConnectionViewModel),
                new PropertyMetadata(0, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnMinPoolSizePropertyChanged((int)e.OldValue, (int)e.NewValue)));

        public int MinPoolSize
        {
            get { return (int)GetValue(MinPoolSizeProperty); }
            set { this.SetValue(MinPoolSizeProperty, value); }
        }

        public static readonly DependencyProperty MultipleActiveResultSetsProperty =
            DependencyProperty.Register(nameof(MultipleActiveResultSets), typeof(bool), typeof(DbConnectionViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnMultipleActiveResultSetsPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool MultipleActiveResultSets
        {
            get { return (bool)GetValue(MultipleActiveResultSetsProperty); }
            set { this.SetValue(MultipleActiveResultSetsProperty, value); }
        }

        public static readonly DependencyProperty MultiSubnetFailoverProperty =
            DependencyProperty.Register(nameof(MultiSubnetFailover), typeof(bool), typeof(DbConnectionViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnMultiSubnetFailoverPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool MultiSubnetFailover
        {
            get { return (bool)GetValue(MultiSubnetFailoverProperty); }
            set { this.SetValue(MultiSubnetFailoverProperty, value); }
        }

        public static readonly DependencyProperty NetworkLibraryProperty =
            DependencyProperty.Register(nameof(NetworkLibrary), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnNetworkLibraryPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string NetworkLibrary
        {
            get { return GetValue(NetworkLibraryProperty) as string; }
            set { SetValue(NetworkLibraryProperty, value); }
        }

        public static readonly DependencyProperty PacketSizeProperty =
            DependencyProperty.Register(nameof(PacketSize), typeof(int), typeof(DbConnectionViewModel),
                new PropertyMetadata(8000, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnPacketSizePropertyChanged((int)e.OldValue, (int)e.NewValue)));

        public int PacketSize
        {
            get { return (int)GetValue(PacketSizeProperty); }
            set { this.SetValue(PacketSizeProperty, value); }
        }

        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.Register(nameof(Password), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnPasswordPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string Password
        {
            get { return GetValue(PasswordProperty) as string; }
            set { SetValue(PasswordProperty, value); }
        }

        public static readonly DependencyProperty PersistSecurityInfoProperty =
            DependencyProperty.Register(nameof(PersistSecurityInfo), typeof(bool), typeof(DbConnectionViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnPersistSecurityInfoPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool PersistSecurityInfo
        {
            get { return (bool)GetValue(PersistSecurityInfoProperty); }
            set { this.SetValue(PersistSecurityInfoProperty, value); }
        }

        public static readonly DependencyProperty PoolBlockingPeriodProperty =
            DependencyProperty.Register(nameof(PoolBlockingPeriod), typeof(PoolBlockingPeriod), typeof(DbConnectionViewModel),
                new PropertyMetadata(PoolBlockingPeriod.Auto, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnPoolBlockingPeriodPropertyChanged((PoolBlockingPeriod)e.OldValue, (PoolBlockingPeriod)e.NewValue)));

        public PoolBlockingPeriod PoolBlockingPeriod
        {
            get { return (PoolBlockingPeriod)GetValue(PoolBlockingPeriodProperty); }
            set { this.SetValue(PoolBlockingPeriodProperty, value); }
        }

        public static readonly DependencyProperty PoolingProperty =
            DependencyProperty.Register(nameof(Pooling), typeof(bool), typeof(DbConnectionViewModel),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnPoolingPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool Pooling
        {
            get { return (bool)GetValue(PoolingProperty); }
            set { this.SetValue(PoolingProperty, value); }
        }

        public static readonly DependencyProperty ReplicationProperty =
            DependencyProperty.Register(nameof(Replication), typeof(bool), typeof(DbConnectionViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnReplicationPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool Replication
        {
            get { return (bool)GetValue(ReplicationProperty); }
            set { this.SetValue(ReplicationProperty, value); }
        }

        public static readonly DependencyProperty TransparentNetworkIPResolutionProperty =
            DependencyProperty.Register(nameof(TransparentNetworkIPResolution), typeof(bool), typeof(DbConnectionViewModel),
                new PropertyMetadata(true, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnTransparentNetworkIPResolutionPropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool TransparentNetworkIPResolution
        {
            get { return (bool)GetValue(TransparentNetworkIPResolutionProperty); }
            set { this.SetValue(TransparentNetworkIPResolutionProperty, value); }
        }

        public static readonly DependencyProperty TrustServerCertificateProperty =
            DependencyProperty.Register(nameof(TrustServerCertificate), typeof(bool), typeof(DbConnectionViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnTrustServerCertificatePropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool TrustServerCertificate
        {
            get { return (bool)GetValue(TrustServerCertificateProperty); }
            set { this.SetValue(TrustServerCertificateProperty, value); }
        }

        public static readonly DependencyProperty UserIDProperty =
            DependencyProperty.Register(nameof(UserID), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata("fsinfocatadmin", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnUserIDPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string UserID
        {
            get { return GetValue(UserIDProperty) as string; }
            set { SetValue(UserIDProperty, value); }
        }

        public static readonly DependencyProperty UserInstanceProperty =
            DependencyProperty.Register(nameof(UserInstance), typeof(bool), typeof(DbConnectionViewModel),
                new PropertyMetadata(false, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnUserInstancePropertyChanged((bool)e.OldValue, (bool)e.NewValue)));

        public bool UserInstance
        {
            get { return (bool)GetValue(UserInstanceProperty); }
            set { this.SetValue(UserInstanceProperty, value); }
        }

        public static readonly DependencyProperty WorkstationIDProperty =
            DependencyProperty.Register(nameof(WorkstationID), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as DbConnectionViewModel).OnWorkstationIDPropertyChanged(e.OldValue as string, e.NewValue as string)));

        public string WorkstationID
        {
            get { return GetValue(WorkstationIDProperty) as string; }
            set { SetValue(WorkstationIDProperty, value); }
        }

        public static readonly DependencyPropertyKey ApplicationIntentErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ApplicationIntentError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty ApplicationIntentErrorProperty = ApplicationIntentErrorPropertyKey.DependencyProperty;

        public string ApplicationIntentError
        {
            get { return GetValue(ApplicationIntentErrorProperty) as string; }
            private set { SetValue(ApplicationIntentErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey ApplicationNameErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ApplicationNameError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty ApplicationNameErrorProperty = ApplicationNameErrorPropertyKey.DependencyProperty;

        public string ApplicationNameError
        {
            get { return GetValue(ApplicationNameErrorProperty) as string; }
            private set { SetValue(ApplicationNameErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey AsynchronousProcessingErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(AsynchronousProcessingError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty AsynchronousProcessingErrorProperty = AsynchronousProcessingErrorPropertyKey.DependencyProperty;

        public string AsynchronousProcessingError
        {
            get { return GetValue(AsynchronousProcessingErrorProperty) as string; }
            private set { SetValue(AsynchronousProcessingErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey AuthenticationErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(AuthenticationError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty AuthenticationErrorProperty = AuthenticationErrorPropertyKey.DependencyProperty;

        public string AuthenticationError
        {
            get { return GetValue(AuthenticationErrorProperty) as string; }
            private set { SetValue(AuthenticationErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey BrowsableConnectionStringErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(BrowsableConnectionStringError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty BrowsableConnectionStringErrorProperty = BrowsableConnectionStringErrorPropertyKey.DependencyProperty;

        public string BrowsableConnectionStringError
        {
            get { return GetValue(BrowsableConnectionStringErrorProperty) as string; }
            private set { SetValue(BrowsableConnectionStringErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey ColumnEncryptionSettingErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ColumnEncryptionSettingError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty ColumnEncryptionSettingErrorProperty = ColumnEncryptionSettingErrorPropertyKey.DependencyProperty;

        public string ColumnEncryptionSettingError
        {
            get { return GetValue(ColumnEncryptionSettingErrorProperty) as string; }
            private set { SetValue(ColumnEncryptionSettingErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey ConnectionStringErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ConnectionStringError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty ConnectionStringErrorProperty = ConnectionStringErrorPropertyKey.DependencyProperty;

        public string ConnectionStringError
        {
            get { return GetValue(ConnectionStringErrorProperty) as string; }
            private set { SetValue(ConnectionStringErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey ConnectRetryCountErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ConnectRetryCountError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty ConnectRetryCountErrorProperty = ConnectRetryCountErrorPropertyKey.DependencyProperty;

        public string ConnectRetryCountError
        {
            get { return GetValue(ConnectRetryCountErrorProperty) as string; }
            private set { SetValue(ConnectRetryCountErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey ConnectRetryIntervalErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ConnectRetryIntervalError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty ConnectRetryIntervalErrorProperty = ConnectRetryIntervalErrorPropertyKey.DependencyProperty;

        public string ConnectRetryIntervalError
        {
            get { return GetValue(ConnectRetryIntervalErrorProperty) as string; }
            private set { SetValue(ConnectRetryIntervalErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey ConnectTimeoutErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ConnectTimeoutError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty ConnectTimeoutErrorProperty = ConnectTimeoutErrorPropertyKey.DependencyProperty;

        public string ConnectTimeoutError
        {
            get { return GetValue(ConnectTimeoutErrorProperty) as string; }
            private set { SetValue(ConnectTimeoutErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey ContextConnectionErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ContextConnectionError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty ContextConnectionErrorProperty = ContextConnectionErrorPropertyKey.DependencyProperty;

        public string ContextConnectionError
        {
            get { return GetValue(ContextConnectionErrorProperty) as string; }
            private set { SetValue(ContextConnectionErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey CurrentLanguageErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(CurrentLanguageError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty CurrentLanguageErrorProperty = CurrentLanguageErrorPropertyKey.DependencyProperty;

        public string CurrentLanguageError
        {
            get { return GetValue(CurrentLanguageErrorProperty) as string; }
            private set { SetValue(CurrentLanguageErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey DataSourceErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(DataSourceError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty DataSourceErrorProperty = DataSourceErrorPropertyKey.DependencyProperty;

        public string DataSourceError
        {
            get { return GetValue(DataSourceErrorProperty) as string; }
            private set { SetValue(DataSourceErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey EnclaveAttestationUrlErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(EnclaveAttestationUrlError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty EnclaveAttestationUrlErrorProperty = EnclaveAttestationUrlErrorPropertyKey.DependencyProperty;

        public string EnclaveAttestationUrlError
        {
            get { return GetValue(EnclaveAttestationUrlErrorProperty) as string; }
            private set { SetValue(EnclaveAttestationUrlErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey EncryptErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(EncryptError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty EncryptErrorProperty = EncryptErrorPropertyKey.DependencyProperty;

        public string EncryptError
        {
            get { return GetValue(EncryptErrorProperty) as string; }
            private set { SetValue(EncryptErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey EnlistErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(EnlistError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty EnlistErrorProperty = EnlistErrorPropertyKey.DependencyProperty;

        public string EnlistError
        {
            get { return GetValue(EnlistErrorProperty) as string; }
            private set { SetValue(EnlistErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey FailoverPartnerErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(FailoverPartnerError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty FailoverPartnerErrorProperty = FailoverPartnerErrorPropertyKey.DependencyProperty;

        public string FailoverPartnerError
        {
            get { return GetValue(FailoverPartnerErrorProperty) as string; }
            private set { SetValue(FailoverPartnerErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey InitialCatalogErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(InitialCatalogError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty InitialCatalogErrorProperty = InitialCatalogErrorPropertyKey.DependencyProperty;

        public string InitialCatalogError
        {
            get { return GetValue(InitialCatalogErrorProperty) as string; }
            private set { SetValue(InitialCatalogErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey IntegratedSecurityErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(IntegratedSecurityError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty IntegratedSecurityErrorProperty = IntegratedSecurityErrorPropertyKey.DependencyProperty;

        public string IntegratedSecurityError
        {
            get { return GetValue(IntegratedSecurityErrorProperty) as string; }
            private set { SetValue(IntegratedSecurityErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey LoadBalanceTimeoutErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(LoadBalanceTimeoutError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty LoadBalanceTimeoutErrorProperty = LoadBalanceTimeoutErrorPropertyKey.DependencyProperty;

        public string LoadBalanceTimeoutError
        {
            get { return GetValue(LoadBalanceTimeoutErrorProperty) as string; }
            private set { SetValue(LoadBalanceTimeoutErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey MaxPoolSizeErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MaxPoolSizeError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty MaxPoolSizeErrorProperty = MaxPoolSizeErrorPropertyKey.DependencyProperty;

        public string MaxPoolSizeError
        {
            get { return GetValue(MaxPoolSizeErrorProperty) as string; }
            private set { SetValue(MaxPoolSizeErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey MinPoolSizeErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MinPoolSizeError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty MinPoolSizeErrorProperty = MinPoolSizeErrorPropertyKey.DependencyProperty;

        public string MinPoolSizeError
        {
            get { return GetValue(MinPoolSizeErrorProperty) as string; }
            private set { SetValue(MinPoolSizeErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey MultipleActiveResultSetsErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MultipleActiveResultSetsError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty MultipleActiveResultSetsErrorProperty = MultipleActiveResultSetsErrorPropertyKey.DependencyProperty;

        public string MultipleActiveResultSetsError
        {
            get { return GetValue(MultipleActiveResultSetsErrorProperty) as string; }
            private set { SetValue(MultipleActiveResultSetsErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey MultiSubnetFailoverErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MultiSubnetFailoverError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty MultiSubnetFailoverErrorProperty = MultiSubnetFailoverErrorPropertyKey.DependencyProperty;

        public string MultiSubnetFailoverError
        {
            get { return GetValue(MultiSubnetFailoverErrorProperty) as string; }
            private set { SetValue(MultiSubnetFailoverErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey NetworkLibraryErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(NetworkLibraryError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty NetworkLibraryErrorProperty = NetworkLibraryErrorPropertyKey.DependencyProperty;

        public string NetworkLibraryError
        {
            get { return GetValue(NetworkLibraryErrorProperty) as string; }
            private set { SetValue(NetworkLibraryErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey PacketSizeErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(PacketSizeError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty PacketSizeErrorProperty = PacketSizeErrorPropertyKey.DependencyProperty;

        public string PacketSizeError
        {
            get { return GetValue(PacketSizeErrorProperty) as string; }
            private set { SetValue(PacketSizeErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey PasswordErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(PasswordError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty PasswordErrorProperty = PasswordErrorPropertyKey.DependencyProperty;

        public string PasswordError
        {
            get { return GetValue(PasswordErrorProperty) as string; }
            private set { SetValue(PasswordErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey PersistSecurityInfoErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(PersistSecurityInfoError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty PersistSecurityInfoErrorProperty = PersistSecurityInfoErrorPropertyKey.DependencyProperty;

        public string PersistSecurityInfoError
        {
            get { return GetValue(PersistSecurityInfoErrorProperty) as string; }
            private set { SetValue(PersistSecurityInfoErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey PoolBlockingPeriodErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(PoolBlockingPeriodError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty PoolBlockingPeriodErrorProperty = PoolBlockingPeriodErrorPropertyKey.DependencyProperty;

        public string PoolBlockingPeriodError
        {
            get { return GetValue(PoolBlockingPeriodErrorProperty) as string; }
            private set { SetValue(PoolBlockingPeriodErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey PoolingErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(PoolingError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty PoolingErrorProperty = PoolingErrorPropertyKey.DependencyProperty;

        public string PoolingError
        {
            get { return GetValue(PoolingErrorProperty) as string; }
            private set { SetValue(PoolingErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey ReplicationErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ReplicationError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty ReplicationErrorProperty = ReplicationErrorPropertyKey.DependencyProperty;

        public string ReplicationError
        {
            get { return GetValue(ReplicationErrorProperty) as string; }
            private set { SetValue(ReplicationErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey TransparentNetworkIPResolutionErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(TransparentNetworkIPResolutionError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty TransparentNetworkIPResolutionErrorProperty = TransparentNetworkIPResolutionErrorPropertyKey.DependencyProperty;

        public string TransparentNetworkIPResolutionError
        {
            get { return GetValue(TransparentNetworkIPResolutionErrorProperty) as string; }
            private set { SetValue(TransparentNetworkIPResolutionErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey TrustServerCertificateErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(TrustServerCertificateError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty TrustServerCertificateErrorProperty = TrustServerCertificateErrorPropertyKey.DependencyProperty;

        public string TrustServerCertificateError
        {
            get { return GetValue(TrustServerCertificateErrorProperty) as string; }
            private set { SetValue(TrustServerCertificateErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey UserIDErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(UserIDError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty UserIDErrorProperty = UserIDErrorPropertyKey.DependencyProperty;

        public string UserIDError
        {
            get { return GetValue(UserIDErrorProperty) as string; }
            private set { SetValue(UserIDErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey UserInstanceErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(UserInstanceError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty UserInstanceErrorProperty = UserInstanceErrorPropertyKey.DependencyProperty;

        public string UserInstanceError
        {
            get { return GetValue(UserInstanceErrorProperty) as string; }
            private set { SetValue(UserInstanceErrorPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey WorkstationIDErrorPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(WorkstationIDError), typeof(string), typeof(DbConnectionViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty WorkstationIDErrorProperty = WorkstationIDErrorPropertyKey.DependencyProperty;

        public string WorkstationIDError
        {
            get { return GetValue(WorkstationIDErrorProperty) as string; }
            private set { SetValue(WorkstationIDErrorPropertyKey, value); }
        }
        protected virtual void OnApplicationIntentPropertyChanged(ApplicationIntent oldValue, ApplicationIntent newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.ApplicationIntent = newValue;
                OnConnectionStringComponentChanged();
                ApplicationIntentError = "";
            }
            catch (Exception exception)
            {
                ApplicationIntentError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnApplicationNamePropertyChanged(string oldValue, string newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.ApplicationName = newValue;
                OnConnectionStringComponentChanged();
                ApplicationNameError = "";
            }
            catch (Exception exception)
            {
                ApplicationNameError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnAsynchronousProcessingPropertyChanged(bool oldValue, bool newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.AsynchronousProcessing = newValue;
                OnConnectionStringComponentChanged();
                AsynchronousProcessingError = "";
            }
            catch (Exception exception)
            {
                AsynchronousProcessingError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnAuthenticationPropertyChanged(SqlAuthenticationMethod oldValue, SqlAuthenticationMethod newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.Authentication = newValue;
                OnConnectionStringComponentChanged();
                AuthenticationError = "";
            }
            catch (Exception exception)
            {
                AuthenticationError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnBrowsableConnectionStringPropertyChanged(bool oldValue, bool newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.BrowsableConnectionString = newValue;
                OnConnectionStringComponentChanged();
                BrowsableConnectionStringError = "";
            }
            catch (Exception exception)
            {
                BrowsableConnectionStringError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnColumnEncryptionSettingPropertyChanged(SqlConnectionColumnEncryptionSetting oldValue, SqlConnectionColumnEncryptionSetting newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.ColumnEncryptionSetting = newValue;
                OnConnectionStringComponentChanged();
                ColumnEncryptionSettingError = "";
            }
            catch (Exception exception)
            {
                ColumnEncryptionSettingError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnConnectionStringPropertyChanged(string oldValue, string newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.ConnectionString = newValue;
                OnConnectionStringComponentChanged();
                ConnectionStringError = "";


                if (_backingBuilder.ApplicationIntent != ApplicationIntent)
                    try { _backingBuilder.ApplicationIntent = ApplicationIntent; } catch { /* okay to ignore */ }
                if (_backingBuilder.ApplicationName != ApplicationName)
                    try { _backingBuilder.ApplicationName = ApplicationName; } catch { /* okay to ignore */ }
                if (_backingBuilder.AsynchronousProcessing != AsynchronousProcessing)
                    try { _backingBuilder.AsynchronousProcessing = AsynchronousProcessing; } catch { /* okay to ignore */ }
                if (_backingBuilder.Authentication != Authentication)
                    try { _backingBuilder.Authentication = Authentication; } catch { /* okay to ignore */ }
                if (_backingBuilder.BrowsableConnectionString != BrowsableConnectionString)
                    try { _backingBuilder.BrowsableConnectionString = BrowsableConnectionString; } catch { /* okay to ignore */ }
                if (_backingBuilder.ColumnEncryptionSetting != ColumnEncryptionSetting)
                    try { _backingBuilder.ColumnEncryptionSetting = ColumnEncryptionSetting; } catch { /* okay to ignore */ }
                if (_backingBuilder.ConnectionString != ConnectionString)
                    try { _backingBuilder.ConnectionString = ConnectionString; } catch { /* okay to ignore */ }
                if (_backingBuilder.ConnectRetryCount != ConnectRetryCount)
                    try { _backingBuilder.ConnectRetryCount = ConnectRetryCount; } catch { /* okay to ignore */ }
                if (_backingBuilder.ConnectRetryInterval != ConnectRetryInterval)
                    try { _backingBuilder.ConnectRetryInterval = ConnectRetryInterval; } catch { /* okay to ignore */ }
                if (_backingBuilder.ConnectTimeout != ConnectTimeout)
                    try { _backingBuilder.ConnectTimeout = ConnectTimeout; } catch { /* okay to ignore */ }
                if (_backingBuilder.ContextConnection != ContextConnection)
                    try { _backingBuilder.ContextConnection = ContextConnection; } catch { /* okay to ignore */ }
                if (_backingBuilder.CurrentLanguage != CurrentLanguage)
                    try { _backingBuilder.CurrentLanguage = CurrentLanguage; } catch { /* okay to ignore */ }
                if (_backingBuilder.DataSource != DataSource)
                    try { _backingBuilder.DataSource = DataSource; } catch { /* okay to ignore */ }
                if (_backingBuilder.EnclaveAttestationUrl != EnclaveAttestationUrl)
                    try { _backingBuilder.EnclaveAttestationUrl = EnclaveAttestationUrl; } catch { /* okay to ignore */ }
                if (_backingBuilder.Encrypt != Encrypt)
                    try { _backingBuilder.Encrypt = Encrypt; } catch { /* okay to ignore */ }
                if (_backingBuilder.Enlist != Enlist)
                    try { _backingBuilder.Enlist = Enlist; } catch { /* okay to ignore */ }
                if (_backingBuilder.FailoverPartner != FailoverPartner)
                    try { _backingBuilder.FailoverPartner = FailoverPartner; } catch { /* okay to ignore */ }
                if (_backingBuilder.InitialCatalog != InitialCatalog)
                    try { _backingBuilder.InitialCatalog = InitialCatalog; } catch { /* okay to ignore */ }
                if (_backingBuilder.IntegratedSecurity != IntegratedSecurity)
                    try { _backingBuilder.IntegratedSecurity = IntegratedSecurity; } catch { /* okay to ignore */ }
                if (_backingBuilder.LoadBalanceTimeout != LoadBalanceTimeout)
                    try { _backingBuilder.LoadBalanceTimeout = LoadBalanceTimeout; } catch { /* okay to ignore */ }
                if (_backingBuilder.MaxPoolSize != MaxPoolSize)
                    try { _backingBuilder.MaxPoolSize = MaxPoolSize; } catch { /* okay to ignore */ }
                if (_backingBuilder.MinPoolSize != MinPoolSize)
                    try { _backingBuilder.MinPoolSize = MinPoolSize; } catch { /* okay to ignore */ }
                if (_backingBuilder.MultipleActiveResultSets != MultipleActiveResultSets)
                    try { _backingBuilder.MultipleActiveResultSets = MultipleActiveResultSets; } catch { /* okay to ignore */ }
                if (_backingBuilder.MultiSubnetFailover != MultiSubnetFailover)
                    try { _backingBuilder.MultiSubnetFailover = MultiSubnetFailover; } catch { /* okay to ignore */ }
                if (_backingBuilder.NetworkLibrary != NetworkLibrary)
                    try { _backingBuilder.NetworkLibrary = NetworkLibrary; } catch { /* okay to ignore */ }
                if (_backingBuilder.PacketSize != PacketSize)
                    try { _backingBuilder.PacketSize = PacketSize; } catch { /* okay to ignore */ }
                if (_backingBuilder.Password != Password)
                    try { _backingBuilder.Password = Password; } catch { /* okay to ignore */ }
                if (_backingBuilder.PersistSecurityInfo != PersistSecurityInfo)
                    try { _backingBuilder.PersistSecurityInfo = PersistSecurityInfo; } catch { /* okay to ignore */ }
                if (_backingBuilder.PoolBlockingPeriod != PoolBlockingPeriod)
                    try { _backingBuilder.PoolBlockingPeriod = PoolBlockingPeriod; } catch { /* okay to ignore */ }
                if (_backingBuilder.Pooling != Pooling)
                    try { _backingBuilder.Pooling = Pooling; } catch { /* okay to ignore */ }
                if (_backingBuilder.Replication != Replication)
                    try { _backingBuilder.Replication = Replication; } catch { /* okay to ignore */ }
                if (_backingBuilder.TransparentNetworkIPResolution != TransparentNetworkIPResolution)
                    try { _backingBuilder.TransparentNetworkIPResolution = TransparentNetworkIPResolution; } catch { /* okay to ignore */ }
                if (_backingBuilder.TrustServerCertificate != TrustServerCertificate)
                    try { _backingBuilder.TrustServerCertificate = TrustServerCertificate; } catch { /* okay to ignore */ }
                if (_backingBuilder.UserID != UserID)
                    try { _backingBuilder.UserID = UserID; } catch { /* okay to ignore */ }
                if (_backingBuilder.UserInstance != UserInstance)
                    try { _backingBuilder.UserInstance = UserInstance; } catch { /* okay to ignore */ }
                if (_backingBuilder.WorkstationID != WorkstationID)
                    try { _backingBuilder.WorkstationID = WorkstationID; } catch { /* okay to ignore */ }
            }
            catch (Exception exception)
            {
                ConnectionStringError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnConnectRetryCountPropertyChanged(int oldValue, int newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.ConnectRetryCount = newValue;
                OnConnectionStringComponentChanged();
                ConnectRetryCountError = "";
            }
            catch (Exception exception)
            {
                ConnectRetryCountError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnConnectRetryIntervalPropertyChanged(int oldValue, int newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.ConnectRetryInterval = newValue;
                OnConnectionStringComponentChanged();
                ConnectRetryIntervalError = "";
            }
            catch (Exception exception)
            {
                ConnectRetryIntervalError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnConnectTimeoutPropertyChanged(int oldValue, int newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.ConnectTimeout = newValue;
                OnConnectionStringComponentChanged();
                ConnectTimeoutError = "";
            }
            catch (Exception exception)
            {
                ConnectTimeoutError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnContextConnectionPropertyChanged(bool oldValue, bool newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.ContextConnection = newValue;
                OnConnectionStringComponentChanged();
                ContextConnectionError = "";
            }
            catch (Exception exception)
            {
                ContextConnectionError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnCurrentLanguagePropertyChanged(string oldValue, string newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.CurrentLanguage = newValue;
                OnConnectionStringComponentChanged();
                CurrentLanguageError = "";
            }
            catch (Exception exception)
            {
                CurrentLanguageError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnDataSourcePropertyChanged(string oldValue, string newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.DataSource = newValue;
                OnConnectionStringComponentChanged();
                DataSourceError = "";
            }
            catch (Exception exception)
            {
                DataSourceError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnEnclaveAttestationUrlPropertyChanged(string oldValue, string newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.EnclaveAttestationUrl = newValue;
                OnConnectionStringComponentChanged();
                EnclaveAttestationUrlError = "";
            }
            catch (Exception exception)
            {
                EnclaveAttestationUrlError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnEncryptPropertyChanged(bool oldValue, bool newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.Encrypt = newValue;
                OnConnectionStringComponentChanged();
                EncryptError = "";
            }
            catch (Exception exception)
            {
                EncryptError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnEnlistPropertyChanged(bool oldValue, bool newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.Enlist = newValue;
                OnConnectionStringComponentChanged();
                EnlistError = "";
            }
            catch (Exception exception)
            {
                EnlistError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnFailoverPartnerPropertyChanged(string oldValue, string newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.FailoverPartner = newValue;
                OnConnectionStringComponentChanged();
                FailoverPartnerError = "";
            }
            catch (Exception exception)
            {
                FailoverPartnerError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnInitialCatalogPropertyChanged(string oldValue, string newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.InitialCatalog = newValue;
                OnConnectionStringComponentChanged();
                InitialCatalogError = "";
            }
            catch (Exception exception)
            {
                InitialCatalogError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnIntegratedSecurityPropertyChanged(bool oldValue, bool newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.IntegratedSecurity = newValue;
                OnConnectionStringComponentChanged();
                IntegratedSecurityError = "";
            }
            catch (Exception exception)
            {
                IntegratedSecurityError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnLoadBalanceTimeoutPropertyChanged(int oldValue, int newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.LoadBalanceTimeout = newValue;
                OnConnectionStringComponentChanged();
                LoadBalanceTimeoutError = "";
            }
            catch (Exception exception)
            {
                LoadBalanceTimeoutError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnMaxPoolSizePropertyChanged(int oldValue, int newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.MaxPoolSize = newValue;
                OnConnectionStringComponentChanged();
                MaxPoolSizeError = "";
            }
            catch (Exception exception)
            {
                MaxPoolSizeError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnMinPoolSizePropertyChanged(int oldValue, int newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.MinPoolSize = newValue;
                OnConnectionStringComponentChanged();
                MinPoolSizeError = "";
            }
            catch (Exception exception)
            {
                MinPoolSizeError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnMultipleActiveResultSetsPropertyChanged(bool oldValue, bool newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                MultipleActiveResultSetsError = "";
            }
            catch (Exception exception)
            {
                _backingBuilder.MultipleActiveResultSets = newValue;
                OnConnectionStringComponentChanged();
                MultipleActiveResultSetsError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnMultiSubnetFailoverPropertyChanged(bool oldValue, bool newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.MultiSubnetFailover = newValue;
                OnConnectionStringComponentChanged();
                MultiSubnetFailoverError = "";
            }
            catch (Exception exception)
            {
                MultiSubnetFailoverError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnNetworkLibraryPropertyChanged(string oldValue, string newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.NetworkLibrary = newValue;
                OnConnectionStringComponentChanged();
                NetworkLibraryError = "";
            }
            catch (Exception exception)
            {
                NetworkLibraryError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnPacketSizePropertyChanged(int oldValue, int newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.PacketSize = newValue;
                OnConnectionStringComponentChanged();
                PacketSizeError = "";
            }
            catch (Exception exception)
            {
                PacketSizeError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnPasswordPropertyChanged(string oldValue, string newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.Password = newValue;
                OnConnectionStringComponentChanged();
                PasswordError = "";
            }
            catch (Exception exception)
            {
                PasswordError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnPersistSecurityInfoPropertyChanged(bool oldValue, bool newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.PersistSecurityInfo = newValue;
                OnConnectionStringComponentChanged();
                PersistSecurityInfoError = "";
            }
            catch (Exception exception)
            {
                PersistSecurityInfoError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnPoolBlockingPeriodPropertyChanged(PoolBlockingPeriod oldValue, PoolBlockingPeriod newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.PoolBlockingPeriod = newValue;
                OnConnectionStringComponentChanged();
                PoolBlockingPeriodError = "";
            }
            catch (Exception exception)
            {
                PoolBlockingPeriodError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnPoolingPropertyChanged(bool oldValue, bool newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.Pooling = newValue;
                OnConnectionStringComponentChanged();
                PoolingError = "";
            }
            catch (Exception exception)
            {
                PoolingError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnReplicationPropertyChanged(bool oldValue, bool newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.Replication = newValue;
                OnConnectionStringComponentChanged();
                ReplicationError = "";
            }
            catch (Exception exception)
            {
                ReplicationError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnTransparentNetworkIPResolutionPropertyChanged(bool oldValue, bool newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.TransparentNetworkIPResolution = newValue;
                OnConnectionStringComponentChanged();
                TransparentNetworkIPResolutionError = "";
            }
            catch (Exception exception)
            {
                TransparentNetworkIPResolutionError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnTrustServerCertificatePropertyChanged(bool oldValue, bool newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.TrustServerCertificate = newValue;
                OnConnectionStringComponentChanged();
                TrustServerCertificateError = "";
            }
            catch (Exception exception)
            {
                TrustServerCertificateError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnUserIDPropertyChanged(string oldValue, string newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.UserID = newValue;
                OnConnectionStringComponentChanged();
                UserIDError = "";
            }
            catch (Exception exception)
            {
                UserIDError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnUserInstancePropertyChanged(bool oldValue, bool newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.UserInstance = newValue;
                OnConnectionStringComponentChanged();
                UserInstanceError = "";
            }
            catch (Exception exception)
            {
                UserInstanceError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        protected virtual void OnWorkstationIDPropertyChanged(string oldValue, string newValue)
        {
            if (_ignoreChange || oldValue == newValue)
                return;
            try
            {
                _backingBuilder.WorkstationID = newValue;
                WorkstationIDError = "";
                OnConnectionStringComponentChanged();
            }
            catch (Exception exception)
            {
                WorkstationIDError = string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message;
            }
            finally { _ignoreChange = false; }
        }

        private void OnConnectionStringComponentChanged()
        {
            // TODO: Implement OnConnectionStringComponentChanged
            throw new NotImplementedException();
        }
    }
}
