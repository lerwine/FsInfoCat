using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
                if (string.IsNullOrWhiteSpace(settingsViewModel.MachineSID) || string.IsNullOrWhiteSpace(settingsViewModel.MachineName))
                {
                    MessageBox.Show("Failed to detect machine identifier", "Initialization failure", MessageBoxButton.OK, MessageBoxImage.Error);
                    DialogResult = false;
                    Close();
                }
                (new LoginWindow { Owner = this }).ShowDialog();
                if (settingsViewModel.User is null)
                    Close();
            }
        }
    }
}
