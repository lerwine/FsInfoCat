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
        private Task<Model.Remote.HostDevice> _localMachineRegistrationTask = null;
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

        private static readonly DependencyPropertyKey UserPropertyKey = DependencyProperty.RegisterReadOnly(nameof(User), typeof(Model.Remote.UserProfile), typeof(SettingsViewModel),
            new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as SettingsViewModel).UserPropertyChanged?.Invoke(d, e)));

        public static readonly DependencyProperty UserProperty = UserPropertyKey.DependencyProperty;

        public Model.Remote.UserProfile User
        {
            get { return (Model.Remote.UserProfile)GetValue(UserProperty); }
            private set { SetValue(UserPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey RolesPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Roles), typeof(Model.Remote.UserRole), typeof(SettingsViewModel),
            new PropertyMetadata(Model.Remote.UserRole.None, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as SettingsViewModel).OnRolesPropertyChanged(e)));

        private void OnRolesPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try
            {
                RegisterLocalMachineCommand.IsEnabled = args.NewValue is Model.Remote.UserRole role && (role.HasFlag(Model.Remote.UserRole.ITSupport) || role.HasFlag(Model.Remote.UserRole.Contributor));
            }
            finally { RolesPropertyChanged?.Invoke(this, args); }
        }

        public static readonly DependencyProperty RolesProperty = RolesPropertyKey.DependencyProperty;

        public Model.Remote.UserRole Roles
        {
            get { return (Model.Remote.UserRole)GetValue(RolesProperty); }
            private set { SetValue(RolesPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey HostDeviceRegistrationPropertyKey = DependencyProperty.RegisterReadOnly(nameof(HostDeviceRegistration), typeof(Model.Remote.HostDevice), typeof(SettingsViewModel),
            new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as SettingsViewModel).OnHostDeviceRegistrationPropertyChanged(e)));


        public static readonly DependencyProperty HostDeviceRegistrationProperty = HostDeviceRegistrationPropertyKey.DependencyProperty;

        public Model.Remote.HostDevice HostDeviceRegistration
        {
            get { return (Model.Remote.HostDevice)GetValue(HostDeviceRegistrationProperty); }
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

        private async Task<Model.Remote.HostDevice> CheckHostDeviceRegistrationAsync(string machineName, Action<string> setMachineSid)
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
                        using (Model.Remote.RemoteDbContainer dbContext = new Model.Remote.RemoteDbContainer())
                            return await dbContext.HostDevices.FirstOrDefaultAsync(h => h.MachineIdentifer == sidString && h.MachineName == machineName);
                    }
                }
            }
            return null;
        }

        internal Task<Model.Remote.HostDevice> CheckHostDeviceRegistrationAsync(bool forceRecheck)
        {
            VerifyAccess();
            Task<Model.Remote.HostDevice> task;
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

        private async Task<Model.Remote.HostDevice> ForceRegisterLocalMachineAsync(string sidString, string machineName)
        {
            using (Model.Remote.RemoteDbContainer dbContext = new Model.Remote.RemoteDbContainer())
                return await dbContext.HostDevices.FirstOrDefaultAsync(h => h.MachineIdentifer == sidString && h.MachineName == machineName);
        }

        private Task<Model.Remote.HostDevice> RegisterLocalMachineAsync(string sidString, string machineName)
        {
            VerifyAccess();
            Task<Model.Remote.HostDevice> task = _localMachineRegistrationTask;
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

        private async Task<Model.Remote.HostDevice> ForceUnregisterLocalMachineAsync(Model.Remote.HostDevice hostDeviceRegistration)
        {
            using (Model.Remote.RemoteDbContainer dbContext = new Model.Remote.RemoteDbContainer())
            {
                dbContext.HostDevices.Attach(hostDeviceRegistration);
                dbContext.HostDevices.Remove(hostDeviceRegistration);
                await dbContext.SaveChangesAsync();
                return null;
            }
        }

        private Task<Model.Remote.HostDevice> UnregisterLocalMachineAsync(Model.Remote.HostDevice hostDeviceRegistration)
        {
            VerifyAccess();
            Task<Model.Remote.HostDevice> task = _localMachineRegistrationTask;
            if (task is null || task.IsCompleted)
            {
                RegisterLocalMachineCommand.IsEnabled = false;
                _localMachineRegistrationTask = task = ForceUnregisterLocalMachineAsync(hostDeviceRegistration);
                task.ContinueWith((Task<Model.Remote.HostDevice> t) =>
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
