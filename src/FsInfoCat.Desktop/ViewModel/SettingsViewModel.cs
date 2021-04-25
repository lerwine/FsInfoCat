using FsInfoCat.Desktop.Model;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Data.Entity;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Management;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public sealed class SettingsViewModel : DependencyObject
    {
        private static readonly ILogger<SettingsViewModel> _logger = App.LoggerFactory.CreateLogger<SettingsViewModel>();
        private Task<HostDevice> _localMachineRegistrationTask = null;
        private const string RegisterLocalMachine_MenuItem_Text = "Register Local Machine";
        private const string UnregisterLocalMachine_MenuItem_Text = "Un-Register Local Machine";

        public event DependencyPropertyChangedEventHandler UserPropertyChanged;

        public event DependencyPropertyChangedEventHandler RolesPropertyChanged;

        public event DependencyPropertyChangedEventHandler HostDeviceRegistrationPropertyChanged;

        #region Properties

        public event DependencyPropertyChangedEventHandler LocalDatabasePathPropertyChanged;

        public static readonly DependencyProperty LocalDatabasePathProperty =
            DependencyProperty.Register(nameof(LocalDatabasePath), typeof(string), typeof(SettingsViewModel),
                new PropertyMetadata("",
                    (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as SettingsViewModel).OnLocalDatabasePathPropertyChanged(e)));

        public string LocalDatabasePath
        {
            get { return GetValue(LocalDatabasePathProperty) as string; }
            set { SetValue(LocalDatabasePathProperty, value); }
        }

        private void OnLocalDatabasePathPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try
            {
                CancellationTokenSource tokenSource = _validateLocalDatabasePathTokenSource;
                _validateLocalDatabasePathTokenSource = null;
                if (!(tokenSource is null))
                {
                    if (!tokenSource.IsCancellationRequested)
                        tokenSource.Cancel(true);
                    tokenSource.Dispose();
                }
                _validateLocalDatabasePathTokenSource = tokenSource = new CancellationTokenSource();
                ValidateLocalDatabasePathAsync(args.NewValue as string, tokenSource.Token).ContinueWith(t =>
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        if (ReferenceEquals(tokenSource, _validateLocalDatabasePathTokenSource))
                        {
                            tokenSource.Dispose();
                            _validateLocalDatabasePathTokenSource = null;
                        }
                    }));
                });
            }
            finally { LocalDatabasePathPropertyChanged?.Invoke(this, args); }
        }


        public event DependencyPropertyChangedEventHandler ValidatedLocalDbPathPropertyChanged;

        private static readonly DependencyPropertyKey ValidatedLocalDbPathPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(ValidatedLocalDbPath), typeof(string), typeof(SettingsViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as SettingsViewModel)?.ValidatedLocalDbPathPropertyChanged?.Invoke(d, e)));

        public static readonly DependencyProperty ValidatedLocalDbPathProperty = ValidatedLocalDbPathPropertyKey.DependencyProperty;

        public string ValidatedLocalDbPath
        {
            get { return GetValue(ValidatedLocalDbPathProperty) as string; }
            private set { SetValue(ValidatedLocalDbPathPropertyKey, value); }
        }

        private CancellationTokenSource _validateLocalDatabasePathTokenSource;

        private Task<LocalDbValidationStateValue> ValidateLocalDatabasePathAsync(string path, CancellationToken cancellationToken) => Task.Factory.StartNew<Tuple<LocalDbValidationStateValue, string>>(() =>
        {
            if (cancellationToken.IsCancellationRequested)
                return new Tuple<LocalDbValidationStateValue, string>(LocalDbValidationStateValue.AccessError, path);
            if (string.IsNullOrWhiteSpace(path))
                return new Tuple<LocalDbValidationStateValue, string>(LocalDbValidationStateValue.NotSpecified, null);
            if (!Path.IsPathRooted(path))
                path = Path.GetFullPath(Path.Combine(App.EnsureAppDataPath().FullName, path));
            if (System.IO.File.Exists(path))
                return new Tuple<LocalDbValidationStateValue, string>(LocalDbValidationStateValue.NotInitialized, path);
            return new Tuple<LocalDbValidationStateValue, string>(Directory.Exists(path) ? LocalDbValidationStateValue.NotAFile : LocalDbValidationStateValue.NotFound, path);
        }).ContinueWith(task => EnsureLocalDb(task, cancellationToken, Dispatcher.AsBeginInvocationAction<string>(m => LocalDatabaseAccessMessage = m)))
            .ContinueWith(task => OnLocalDbValidated(task, cancellationToken,
                Dispatcher.AsBeginInvocationAction<LocalDbValidationStateValue, string>((s, m) =>
                {
                    LocalDatabaseAccessMessage = m;
                    LocalDbStatus = s;
                }),
                Dispatcher.AsBeginInvocationAction<LocalDbValidationStateValue, string>((s, p) =>
                {
                    Type t = s.GetType();
                    string n = Enum.GetName(t, s);
                    LocalDatabaseAccessMessage = t.GetField(n).GetCustomAttributes(false).OfType<DescriptionAttribute>().Select(a => a.Description).Where(d => !string.IsNullOrWhiteSpace(d))
                        .DefaultIfEmpty(n).First();
                    if (s >= LocalDbValidationStateValue.Closed)
                        ValidatedLocalDbPath = p;
                    LocalDbStatus = s;
                })));

        private static Tuple<LocalDbValidationStateValue, string> EnsureLocalDb(Task<Tuple<LocalDbValidationStateValue, string>> task, CancellationToken cancellationToken,
            Action<string> setMessage)
        {
            if (task.IsCanceled || cancellationToken.IsCancellationRequested)
                return new Tuple<LocalDbValidationStateValue, string>(LocalDbValidationStateValue.AccessError, task.Result.Item2);
            if (task.IsFaulted)
            {
                setMessage($"Error validating local database path: {(string.IsNullOrWhiteSpace(task.Exception.Message) ? task.Exception.ToString() : task.Exception.Message)}");
                return new Tuple<LocalDbValidationStateValue, string>(LocalDbValidationStateValue.AccessError, task.Result.Item2);
            }
            switch (task.Result.Item1)
            {
                case LocalDbValidationStateValue.NotAFile:
                    setMessage($"Local database file name does not reference a file: {task.Result.Item2}");
                    return new Tuple<LocalDbValidationStateValue, string>(LocalDbValidationStateValue.AccessError, task.Result.Item2);
                case LocalDbValidationStateValue.NotSpecified:
                    setMessage("Local database file name not specified.");
                    return new Tuple<LocalDbValidationStateValue, string>(LocalDbValidationStateValue.AccessError, task.Result.Item2);
                case LocalDbValidationStateValue.NotFound:
                    SqlCeConnectionStringBuilder connectionStringBuilder = new SqlCeConnectionStringBuilder
                    {
                        DataSource = task.Result.Item2
                    };
                    using (SqlCeEngine engine = new SqlCeEngine(connectionStringBuilder.ConnectionString))
                        engine.CreateDatabase();
                    using (LocalDbContext dbContext = new LocalDbContext(connectionStringBuilder.ConnectionString))
                        dbContext.Database.Initialize(true);
                    break;
            }
            return new Tuple<LocalDbValidationStateValue, string>(LocalDbValidationStateValue.Closed, task.Result.Item2);
        }

        private static LocalDbValidationStateValue OnLocalDbValidated(Task<Tuple<LocalDbValidationStateValue, string>> task, CancellationToken cancellationToken,
            Action<LocalDbValidationStateValue, string> setMessage, Action<LocalDbValidationStateValue, string> setValidatedPath)
        {
            if (task.IsCanceled || cancellationToken.IsCancellationRequested)
                return LocalDbValidationStateValue.AccessError;
            if (task.IsFaulted)
            {
                setMessage(LocalDbValidationStateValue.AccessError,
                    $"Error initializing local database: {(string.IsNullOrWhiteSpace(task.Exception.Message) ? task.Exception.ToString() : task.Exception.Message)}");
                return LocalDbValidationStateValue.AccessError;
            }
            setValidatedPath(task.Result.Item1, task.Result.Item2);
            return task.Result.Item1;
        }

        private static readonly DependencyPropertyKey LocalDbStatusPropertyKey = DependencyProperty.RegisterReadOnly(nameof(LocalDbStatus), typeof(LocalDbValidationStateValue),
            typeof(SettingsViewModel), new PropertyMetadata(LocalDbValidationStateValue.Validating));

        public static readonly DependencyProperty LocalDbStatusProperty = LocalDbStatusPropertyKey.DependencyProperty;

        public LocalDbValidationStateValue LocalDbStatus
        {
            get { return (LocalDbValidationStateValue)GetValue(LocalDbStatusProperty); }
            private set { SetValue(LocalDbStatusPropertyKey, value); }
        }

        public event DependencyPropertyChangedEventHandler LocalDatabaseAccessMessagePropertyChanged;

        private static readonly DependencyPropertyKey LocalDatabaseAccessMessagePropertyKey = DependencyProperty.RegisterReadOnly(nameof(LocalDatabaseAccessMessage), typeof(string),
            typeof(SettingsViewModel), new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                (d as SettingsViewModel).OnLocalDatabaseAccessMessagePropertyChanged(e)));

        public static readonly DependencyProperty LocalDatabaseAccessMessageProperty = LocalDatabaseAccessMessagePropertyKey.DependencyProperty;

        public string LocalDatabaseAccessMessage
        {
            get { return GetValue(LocalDatabaseAccessMessageProperty) as string; }
            private set { SetValue(LocalDatabaseAccessMessagePropertyKey, value); }
        }

        private void OnLocalDatabaseAccessMessagePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnLocalDatabaseAccessMessagePropertyChanged(args.OldValue as string, args.NewValue as string); }
            finally { LocalDatabaseAccessMessagePropertyChanged?.Invoke(this, args); }
        }

        private void OnLocalDatabaseAccessMessagePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnLocalDatabaseAccessMessagePropertyChanged Logic
        }

        private static readonly DependencyPropertyKey MachineSIDPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MachineSID), typeof(string), typeof(SettingsViewModel),
            new PropertyMetadata(""));

        public static readonly DependencyProperty MachineSIDProperty = MachineSIDPropertyKey.DependencyProperty;

        public string MachineSID
        {
            get { return GetValue(MachineSIDProperty) as string; }
            private set { SetValue(MachineSIDPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey MachineNamePropertyKey = DependencyProperty.RegisterReadOnly(nameof(MachineName), typeof(string), typeof(SettingsViewModel), new PropertyMetadata(""));

        public static readonly DependencyProperty MachineNameProperty = MachineNamePropertyKey.DependencyProperty;

        public string MachineName
        {
            get { return GetValue(MachineNameProperty) as string; }
            private set { SetValue(MachineNamePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey UserPropertyKey = DependencyProperty.RegisterReadOnly(nameof(User), typeof(UserAccount), typeof(SettingsViewModel),
            new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as SettingsViewModel).UserPropertyChanged?.Invoke(d, e)));

        public static readonly DependencyProperty UserProperty = UserPropertyKey.DependencyProperty;

        public UserAccount User
        {
            get { return (UserAccount)GetValue(UserProperty); }
            private set { SetValue(UserPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey RolesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Roles), typeof(UserRole), typeof(SettingsViewModel),
            new PropertyMetadata(UserRole.None, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as SettingsViewModel).OnRolesPropertyChanged(e)));

        private void OnRolesPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try
            {
                RegisterLocalMachineCommand.IsEnabled = args.NewValue is UserRole role && (role.HasFlag(UserRole.ITSupport) || role.HasFlag(UserRole.Contributor));
            }
            finally { RolesPropertyChanged?.Invoke(this, args); }
        }

        public static readonly DependencyProperty RolesProperty = RolesPropertyKey.DependencyProperty;

        public UserRole Roles
        {
            get { return (UserRole)GetValue(RolesProperty); }
            private set { SetValue(RolesPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey HostDeviceRegistrationPropertyKey = DependencyProperty.RegisterReadOnly(nameof(HostDeviceRegistration), typeof(HostDevice), typeof(SettingsViewModel),
            new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as SettingsViewModel).OnHostDeviceRegistrationPropertyChanged(e)));


        public static readonly DependencyProperty HostDeviceRegistrationProperty = HostDeviceRegistrationPropertyKey.DependencyProperty;

        public HostDevice HostDeviceRegistration
        {
            get { return (HostDevice)GetValue(HostDeviceRegistrationProperty); }
            private set { SetValue(HostDeviceRegistrationPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey RegisterLocalMachineCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RegisterLocalMachineCommand),
            typeof(Commands.RelayCommand), typeof(SettingsViewModel), new PropertyMetadata(null, null, (DependencyObject d, object baseValue) =>
                (baseValue is Commands.RelayCommand rc) ? rc : new Commands.RelayCommand(((SettingsViewModel)d).OnRegisterLocalMachineExecute)));

        public static readonly DependencyProperty RegisterLocalMachineCommandProperty = RegisterLocalMachineCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand RegisterLocalMachineCommand => (Commands.RelayCommand)GetValue(RegisterLocalMachineCommandProperty);

        private static readonly DependencyPropertyKey RegisterLocalMachineMenuItemTextPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RegisterLocalMachineMenuItemText), typeof(string),
            typeof(MainViewModel), new PropertyMetadata(RegisterLocalMachine_MenuItem_Text));

        public static readonly DependencyProperty RegisterLocalMachineMenuItemTextProperty = RegisterLocalMachineMenuItemTextPropertyKey.DependencyProperty;

        public string RegisterLocalMachineMenuItemText
        {
            get { return GetValue(RegisterLocalMachineMenuItemTextProperty) as string; }
            private set { SetValue(RegisterLocalMachineMenuItemTextPropertyKey, value); }
        }

        #endregion

        public SettingsViewModel()
        {
            Task.Factory.StartNew(() =>
            {
                string path = Properties.Settings.Default.LocalDbFile;
                if (string.IsNullOrWhiteSpace(path))
                    return new Tuple<LocalDbValidationStateValue, string>(LocalDbValidationStateValue.NotSpecified, null);
                path = Path.GetFullPath(Path.IsPathRooted(path) ? path : Path.Combine(App.EnsureAppDataPath().FullName, path));
                if (System.IO.File.Exists(path))
                    return new Tuple<LocalDbValidationStateValue, string>(LocalDbValidationStateValue.NotInitialized, path);
                return new Tuple<LocalDbValidationStateValue, string>(Directory.Exists(path) ? LocalDbValidationStateValue.NotAFile : LocalDbValidationStateValue.NotFound, path);
            }).ContinueWith(task =>
            {
                if (task.IsCanceled)
                    LocalDatabaseAccessMessage = "Path calculation task canceled.";
                else if (task.IsFaulted)
                    LocalDatabaseAccessMessage = string.IsNullOrWhiteSpace(task.Exception.Message) ? task.Exception.ToString() : task.Exception.Message;
                else if (task.Result.Item1 == LocalDbValidationStateValue.NotSpecified)
                    LocalDatabaseAccessMessage = "Local database file name not specified.";
                else if (task.Result.Item1 == LocalDbValidationStateValue.NotAFile)
                    LocalDatabaseAccessMessage = "Found subdirectory at local database file path instead of a database file.";
                else
                {
                    if (task.Result.Item1 == LocalDbValidationStateValue.NotFound)
                    {

                    }
                }
            });
        }
        private void OnHostDeviceRegistrationPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { RegisterLocalMachineMenuItemText = (args.NewValue is null) ? RegisterLocalMachine_MenuItem_Text : UnregisterLocalMachine_MenuItem_Text; }
            finally { HostDeviceRegistrationPropertyChanged?.Invoke(this, args); }
        }

        private void OnRegisterLocalMachineExecute(object parameter)
        {
            if (HostDeviceRegistration is null)
                RegisterLocalMachineAsync(MachineSID, MachineName);
            else
                UnregisterLocalMachineAsync(HostDeviceRegistration);
        }

        private async Task<HostDevice> CheckHostDeviceRegistrationAsync(string machineName, Action<string> setMachineSid)
        {
            SelectQuery selectQuery = new SelectQuery("SELECT * from Win32_UserAccount WHERE Name=\"Administrator\"");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(selectQuery))
            {
                ManagementObjectCollection managementObjectCollection = searcher.Get();
                if (managementObjectCollection.Count > 0)
                {
                    ManagementObject item = managementObjectCollection.OfType<ManagementObject>().First();
                    SecurityIdentifier sid = new SecurityIdentifier(item["SID"] as string).AccountDomainSid;
                    if (!(sid is null))
                    {
                        string sidString = sid.ToString();
                        setMachineSid(sidString);
                        using (DbModel dbContext = new DbModel())
                            return await dbContext.HostDevices.FirstOrDefaultAsync(h => h.MachineIdentifer == sidString && h.MachineName == machineName);
                    }
                }
            }
            return null;
        }

        internal Task<HostDevice> CheckHostDeviceRegistrationAsync(bool forceRecheck)
        {
            VerifyAccess();
            Task<HostDevice> task;
            lock (this)
            {
                if ((task = _localMachineRegistrationTask) is null || (forceRecheck && task.IsCompleted))
                {
                    string machineName = Environment.MachineName.ToLower();
                    MachineName = machineName;
                    _localMachineRegistrationTask = task = CheckHostDeviceRegistrationAsync(machineName, Dispatcher.AsBeginInvocationAction<string>(sidString => MachineSID = sidString));
                }
            }

            return task;
        }

        private async Task<HostDevice> ForceRegisterLocalMachineAsync(string sidString, string machineName)
        {
            using (DbModel dbContext = new DbModel())
                return await dbContext.HostDevices.FirstOrDefaultAsync(h => h.MachineIdentifer == sidString && h.MachineName == machineName);
        }

        private Task<HostDevice> RegisterLocalMachineAsync(string sidString, string machineName)
        {
            VerifyAccess();
            Task<HostDevice> task = _localMachineRegistrationTask;
            if (task is null || task.IsCompleted)
            {
                RegisterLocalMachineCommand.IsEnabled = false;
                _localMachineRegistrationTask = task = ForceRegisterLocalMachineAsync(sidString, machineName);
                task.ContinueWith(t =>
                {
                    Dispatcher.BeginInvoke(new Action(() => RegisterLocalMachineCommand.IsEnabled = true));
                    if (t.IsCanceled)
                        _logger.LogWarning("{CommandName} for registration canceled", nameof(RegisterLocalMachineCommand));
                    else if (t.IsFaulted)
                        _logger.LogError(t.Exception, "{CommandName} for registration threw an exception: {Message}", nameof(RegisterLocalMachineCommand), t.Exception.Message);
                    else
                    {
                        _logger.LogInformation("{CommandName} for registration succeeded", nameof(RegisterLocalMachineCommand));
                        Dispatcher.BeginInvoke(new Action(() => HostDeviceRegistration = task.Result));
                    }
                });
            }
            return task;
        }

        private async Task<HostDevice> ForceUnregisterLocalMachineAsync(HostDevice hostDeviceRegistration)
        {
            using (DbModel dbContext = new DbModel())
            {
                dbContext.HostDevices.Attach(hostDeviceRegistration);
                dbContext.HostDevices.Remove(hostDeviceRegistration);
                await dbContext.SaveChangesAsync();
                return null;
            }
        }

        private Task<HostDevice> UnregisterLocalMachineAsync(HostDevice hostDeviceRegistration)
        {
            VerifyAccess();
            Task<HostDevice> task = _localMachineRegistrationTask;
            if (task is null || task.IsCompleted)
            {
                RegisterLocalMachineCommand.IsEnabled = false;
                _localMachineRegistrationTask = task = ForceUnregisterLocalMachineAsync(hostDeviceRegistration);
                task.ContinueWith((Task<HostDevice> t) =>
                {
                    Dispatcher.BeginInvoke(new Action(() => RegisterLocalMachineCommand.IsEnabled = true));
                    if (t.IsCanceled)
                        _logger.LogWarning("{CommandName} for un-registration canceled", nameof(RegisterLocalMachineCommand));
                    else if (t.IsFaulted)
                        _logger.LogError(t.Exception, "{CommandName} for un-registration threw an exception: {Message}", nameof(RegisterLocalMachineCommand), t.Exception.Message);
                    else
                    {
                        _logger.LogInformation("{CommandName} for un-registration succeeded", nameof(RegisterLocalMachineCommand));
                        Dispatcher.BeginInvoke(new Action(() => HostDeviceRegistration = null));
                    }
                });
            }
            return task;
        }
    }
}
