using System.Windows;

namespace FsInfoCat.Desktop.View
{
    /// <summary>
    /// Interaction logic for RemoteConnectionAdvancedWindow.xaml
    /// </summary>
    public partial class RemoteConnectionAdvancedWindow : Window
    {
        public RemoteConnectionAdvancedWindow()
        {
            InitializeComponent();
        }

        private void RemoteConnectionConfigViewModel_CloseWindow(object sender, ViewModel.CloseWindowEventArgs e)
        {

        }

        private void RemoteConnectionConfigViewModel_TestConnection(object sender, ViewModel.SqlConnectionStringEventArgs e)
        {

        }
    }
}
