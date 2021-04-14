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
            ViewModel.MainViewModel mainViewModel = (ViewModel.MainViewModel)DataContext;
            mainViewModel.Login += MainViewModel_Login;
            mainViewModel.LogOut += MainViewModel_LogOut;
        }

        private void MainViewModel_LogOut(object sender, EventArgs e)
        {
            // TODO: Implement MainViewModel_LogOut
        }

        private void MainViewModel_Login(object sender, EventArgs e)
        {
            // TODO: Implement MainViewModel_Login
        }
    }
}
