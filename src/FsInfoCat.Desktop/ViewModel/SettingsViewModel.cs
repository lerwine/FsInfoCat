using FsInfoCat.Desktop.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Data.Entity;
using System.Linq;
using System.Management;
using System.Security;
using System.Security.Principal;
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
            finally { RolesPropertyChanged?.Invoke(this, args);  }
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

        internal async Task<UserAccount> AuthenticateUserAsync(string userName, SecureString password, Action<string> onErrorMessage)
        {
            // TODO: Implement AuthenticateUserAsync
            throw new NotImplementedException();
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
