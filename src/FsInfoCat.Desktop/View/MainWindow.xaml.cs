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
            // TODO: Probably should't be doing all of this in the view object
            if (settingsViewModel.User is null)
            {
                bool? result = (new LoginWindow { Owner = this }).ShowDialog();
                if (settingsViewModel.User is null)
                {
                    DialogResult = result.HasValue && result.Value;
                    Close();
                    return;
                }
                settingsViewModel.CheckHostDeviceRegistrationAsync(false).ContinueWith(c =>
                {
                    if (c.IsCanceled)
                        Dispatcher.BeginInvoke(new Action(() => OnCheckHostDeviceRegistrationFailed(null)));
                    else if (c.IsFaulted)
                        Dispatcher.BeginInvoke(new Action(() => OnCheckHostDeviceRegistrationFailed(c.Exception)));
                    else
                        Dispatcher.BeginInvoke(new Action(() => OnCheckHostDeviceRegistrationFinished(c.Result)));
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
