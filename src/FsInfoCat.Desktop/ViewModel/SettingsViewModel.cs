using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Management;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class SettingsViewModel : DependencyObject
    {
        private readonly ILogger<SettingsViewModel> _logger;

        public static readonly DependencyPropertyKey MachineSIDPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MachineSID), typeof(string), typeof(SettingsViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty MachineSIDProperty = MachineSIDPropertyKey.DependencyProperty;

        public string MachineSID
        {
            get { return GetValue(MachineSIDProperty) as string; }
            private set { SetValue(MachineSIDPropertyKey, value); }
        }

        public static readonly DependencyPropertyKey MachineNamePropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(MachineName), typeof(string), typeof(SettingsViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty MachineNameProperty = MachineNamePropertyKey.DependencyProperty;

        public string MachineName
        {
            get { return GetValue(MachineNameProperty) as string; }
            private set { SetValue(MachineNamePropertyKey, value); }
        }

        public event DependencyPropertyChangedEventHandler UserPropertyChanged;

        public static readonly DependencyPropertyKey UserPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(User), typeof(UserCredential), typeof(SettingsViewModel),
                new PropertyMetadata(null, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
                    (d as SettingsViewModel).OnUserPropertyChanged(e)));

        public static readonly DependencyProperty UserProperty = UserPropertyKey.DependencyProperty;

        public Account User
        {
            get { return (Account)GetValue(UserProperty); }
            private set { SetValue(UserPropertyKey, value); }
        }

        protected virtual void OnUserPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnUserPropertyChanged((Account)args.OldValue, (Account)args.NewValue); }
            finally { UserPropertyChanged?.Invoke(this, args); }
        }

        protected virtual void OnUserPropertyChanged(Account oldValue, Account newValue)
        {
            // TODO: Implement OnUserPropertyChanged Logic
        }

        public SettingsViewModel()
        {
            _logger = App.LoggerFactory.CreateLogger<SettingsViewModel>();
            MachineName = Environment.MachineName;
            try
            {
                SelectQuery selectQuery = new SelectQuery("SELECT * from Win32_UserAccount WHERE Name=\"Administrator\"");
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(selectQuery))
                {
                    ManagementObjectCollection managementObjectCollection = searcher.Get();
                    if (managementObjectCollection.Count > 0)
                    {
                        ManagementObject item = managementObjectCollection.OfType<ManagementObject>().First();
                        SecurityIdentifier sid = new SecurityIdentifier(item["SID"] as string);
                        MachineSID = sid.AccountDomainSid.ToString();
                    }
                }
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Unable to get machine SID");
            }
        }
    }
}
