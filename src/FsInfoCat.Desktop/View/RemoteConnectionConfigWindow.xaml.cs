using FsInfoCat.Desktop.ViewModel;
using System.Windows;

namespace FsInfoCat.Desktop.View
{
    /// <summary>
    /// Interaction logic for RemovteConnectionConfigWindow.xaml
    /// </summary>
    public partial class RemoteConnectionConfigWindow : Window
    {
        public RemoteConnectionConfigWindow()
        {
            InitializeComponent();
        }

        private void RemoteConnectionConfigViewModel_Advanced(object sender, SqlConnectionStringBuilderEventArgs e)
        {

        }

        private void RemoteConnectionConfigViewModel_TestConnection(object sender, SqlConnectionStringEventArgs e)
        {

        }

        private void RemoteConnectionConfigViewModel_CloseWindow(object sender, CloseWindowEventArgs e)
        {
            DialogResult = e.DialogResult;
            Close();
        }
    }
}
