using FsInfoCat.Desktop.Model;
using System;
using System.Linq;
using System.Security.Principal;
using System.Windows;

namespace FsInfoCat.Desktop.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {
            base.OnActivated(e);
            ViewModel.SettingsViewModel settingsViewModel = App.GetSettingsVM();
            if (settingsViewModel.User is null)
            {
                OuterGrid.Visibility = Visibility.Collapsed;
                if (!DbInitializeWindow.CheckConfiguration(() => new DbInitializeWindow
                {
                    Owner = this
                }.ShowDialog() ?? false, (Exception exc, string message) => MessageBox.Show(this,
                    $"Error reading from database: {(string.IsNullOrWhiteSpace(exc.Message) ? exc.ToString() : exc.Message)}",
                    "DB Access Error", MessageBoxButton.OK, MessageBoxImage.Error)))
                {
                    DialogResult = false;
                    Close();
                    return;
                }
                System.Threading.Tasks.Task<HostDevice> task = settingsViewModel.CheckHostDeviceRegistrationAsync(false);
                settingsViewModel.AuthenticateUserAsync(WindowsIdentity.GetCurrent()).ContinueWith(t =>
                {
                    UserAccount userAccount;
                    if (t.IsCanceled)
                        userAccount = null;
                    else if (t.IsFaulted)
                    {
                        Dispatcher.Invoke(new Action(() => MessageBox.Show($"Error checking auto-login status: {(string.IsNullOrWhiteSpace(t.Exception.Message) ? t.Exception.ToString() : t.Exception.Message)}", "Login Check Error", MessageBoxButton.OK, MessageBoxImage.Error)));
                        userAccount = null;
                    }
                    else
                        userAccount = t.Result;
                    if (!(userAccount is null) || Dispatcher.Invoke(new Func<bool>(() =>
                    {
                        bool? result = (new LoginWindow { Owner = this }).ShowDialog();
                        if (settingsViewModel.User is null)
                        {
                            DialogResult = result.HasValue && result.Value;
                            Close();
                            return false;
                        }
                        return true;
                    })))
                        task.ContinueWith(c =>
                        {
                            if (c.IsCanceled)
                                Dispatcher.BeginInvoke(new Action(() => OnCheckHostDeviceRegistrationFailed(null)));
                            else if (c.IsFaulted)
                                Dispatcher.BeginInvoke(new Action(() => OnCheckHostDeviceRegistrationFailed(c.Exception)));
                            else
                                Dispatcher.BeginInvoke(new Action(() => OnCheckHostDeviceRegistrationFinished(c.Result)));
                        });
                    Dispatcher.BeginInvoke(new Action(() => OuterGrid.Visibility = Visibility.Visible));
                });
            }
        }

        private void OnCheckHostDeviceRegistrationFinished(HostDevice result)
        {
            ViewModel.SettingsViewModel settingsViewModel = App.GetSettingsVM();
            if (result is null)
            {
                if (string.IsNullOrWhiteSpace(settingsViewModel.MachineSID) || string.IsNullOrWhiteSpace(settingsViewModel.MachineName))
                {
                    MessageBox.Show("Failed to detect machine identifier", "Initialization failure", MessageBoxButton.OK, MessageBoxImage.Error);
                    DialogResult = false;
                    Close();
                }
            }
        }

        private void OnCheckHostDeviceRegistrationFailed(AggregateException exception)
        {
            string message = (exception is null) ? "Operation canceled" : (string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message);
            MessageBox.Show($"Failed to check current host device registration status: {message}", "Initialization failure", MessageBoxButton.OK, MessageBoxImage.Error);
            DialogResult = false;
            Close();
        }

        private void ViewHostDevicesMenuItem_Click(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
