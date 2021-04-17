using FsInfoCat.Desktop.Model;
using System;
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
                System.Threading.Tasks.Task<HostDevice> task = settingsViewModel.CheckHostDeviceRegistrationAsync(false);
                bool? result = (new LoginWindow { Owner = this }).ShowDialog();
                if (settingsViewModel.User is null)
                {
                    DialogResult = result.HasValue && result.Value;
                    Close();
                }
                else
                    task.ContinueWith(t =>
                    {
                        if (t.IsCanceled)
                            Dispatcher.BeginInvoke(new Action(() => OnCheckHostDeviceRegistrationFailed(null)));
                        else if (t.IsFaulted)
                            Dispatcher.BeginInvoke(new Action(() => OnCheckHostDeviceRegistrationFailed(t.Exception)));
                        else
                            Dispatcher.BeginInvoke(new Action(() => OnCheckHostDeviceRegistrationFinished(t.Result)));
                    });
            }
        }

        private void OnCheckHostDeviceRegistrationFinished(HostDevice result)
        {
            if (result is null)
            {
                ViewModel.SettingsViewModel settingsViewModel = App.GetSettingsVM();
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
