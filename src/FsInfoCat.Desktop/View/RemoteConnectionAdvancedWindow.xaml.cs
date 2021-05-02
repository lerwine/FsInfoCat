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
using System.Windows.Shapes;

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
