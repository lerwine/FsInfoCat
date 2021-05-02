using FsInfoCat.Desktop.ViewModel;
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
