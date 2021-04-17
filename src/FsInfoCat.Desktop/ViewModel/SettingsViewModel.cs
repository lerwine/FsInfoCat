using FsInfoCat.Desktop.Model;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management;
using System.Security;
using System.Security.Principal;
using System.Text;
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

        public event EventHandler<AsyncResultEventArgs<HostDevice>> LocalMachineRegistrationChanged;

        public event DependencyPropertyChangedEventHandler UserPropertyChanged;

        public event DependencyPropertyChangedEventHandler HostDeviceRegistrationPropertyChanged;

        #region Properties

        private static readonly DependencyPropertyKey MachineSIDPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MachineSID), typeof(string), typeof(SettingsViewModel), new PropertyMetadata(""));

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

        private static readonly DependencyPropertyKey UserPropertyKey = DependencyProperty.RegisterReadOnly(nameof(User), typeof(UserCredential), typeof(SettingsViewModel),
            new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as SettingsViewModel).OnUserPropertyChanged(e)));

        public static readonly DependencyProperty UserProperty = UserPropertyKey.DependencyProperty;

        public Account User
        {
            get { return (Account)GetValue(UserProperty); }
            private set { SetValue(UserPropertyKey, value); }
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

        private void OnUserPropertyChanged(DependencyPropertyChangedEventArgs args) => UserPropertyChanged?.Invoke(this, args);

        private void OnHostDeviceRegistrationPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { RegisterLocalMachineMenuItemText = (args.NewValue is null) ? RegisterLocalMachine_MenuItem_Text : UnregisterLocalMachine_MenuItem_Text; }
            finally { HostDeviceRegistrationPropertyChanged?.Invoke(this, args); }
        }

        private void OnRegisterLocalMachineExecute(object parameter)
        {
            if (HostDeviceRegistration is null)
                StartRegisterLocalMachineAsync(MachineSID, MachineName);
            else
                StartUnregisterLocalMachineAsync(HostDeviceRegistration);
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
                    Dispatcher.BeginInvoke(new Action(() => MachineName = machineName));
                    _localMachineRegistrationTask = task = Task.Factory.StartNew(() =>
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
                                    Dispatcher.BeginInvoke(new Action(() => MachineSID = sidString));
                                    using (FsInfoCatEntities dbContext = new FsInfoCatEntities())
                                        return (from h in dbContext.HostDevices where h.MachineIdentifer == sidString && h.MachineName == machineName select h)
                                            .FirstOrDefault();
                                }
                            }
                        }
                        return null;
                    });
                }
            }

            return task;
        }

        internal Task<Account> AuthenticateUserAsync(string userName, SecureString securePassword)
        {
            VerifyAccess();
            return Task.Factory.StartNew(() =>
            {
                using (FsInfoCatEntities dbContext = new FsInfoCatEntities())
                {
                    Account account = (from c in dbContext.Accounts where c.LoginName == userName select c).FirstOrDefault();
                    if (!(account is null))
                    {
                        UserCredential userCredential = (from u in dbContext.UserCredentials where u.AccountID == account.AccountID select u).FirstOrDefault();
                        if (!(userCredential is null || string.IsNullOrWhiteSpace(userCredential.PwHash)) &&
                            PwHash.TryCreate(userCredential.PwHash, out PwHash? result) && result.HasValue && result.Value.Test(securePassword))
                        {
                            Dispatcher.BeginInvoke(new Action(() => User = account));
                            return account;
                        }
                    }
                }
                return null;
            });
        }

        private Task<HostDevice> StartRegisterLocalMachineAsync(string sidString, string machineName)
        {
            VerifyAccess();
            Task<HostDevice> task = _localMachineRegistrationTask;
            if (task is null || task.IsCompleted)
            {
                RegisterLocalMachineCommand.IsEnabled = false;
                _localMachineRegistrationTask = task = Task.Factory.StartNew(() =>
                {
                    using (FsInfoCatEntities dbContext = new FsInfoCatEntities())
                        return (from h in dbContext.HostDevices where h.MachineIdentifer == sidString && h.MachineName == machineName select h)
                            .FirstOrDefault();
                });
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

        private Task<HostDevice> StartUnregisterLocalMachineAsync(HostDevice hostDeviceRegistration)
        {
            VerifyAccess();
            Task<HostDevice> task = _localMachineRegistrationTask;
            if (task is null || task.IsCompleted)
            {
                RegisterLocalMachineCommand.IsEnabled = false;
                _localMachineRegistrationTask = task = Task.Factory.StartNew<HostDevice>(() =>
                {
                    using (FsInfoCatEntities dbContext = new FsInfoCatEntities())
                    {
                        dbContext.HostDevices.Attach(hostDeviceRegistration);
                        dbContext.HostDevices.Remove(hostDeviceRegistration);
                        dbContext.SaveChanges();
                        return null;
                    }
                });
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
