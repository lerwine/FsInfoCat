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

namespace FsInfoCat.Desktop
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

        public void OnStopJobCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            ((ViewModels.MainWindowViewModel)DataContext).StopJobCommand.OnCanExecute(sender, e);
        }

        public void OnStopJobExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ((ViewModels.MainWindowViewModel)DataContext).StopJobCommand.OnExecuted(sender, e);
        }

        public void OnExitCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            ((ViewModels.MainWindowViewModel)DataContext).ExitCommand.OnCanExecute(sender, e);
        }

        public void OnExitExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ((ViewModels.MainWindowViewModel)DataContext).ExitCommand.OnExecuted(sender, e);
        }
        public void OnHelpCanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            ((ViewModels.MainWindowViewModel)DataContext).HelpCommand.OnCanExecute(sender, e);
        }

        public void OnHelpExecuted(object sender, ExecutedRoutedEventArgs e)
        {
            ((ViewModels.MainWindowViewModel)DataContext).HelpCommand.OnExecuted(sender, e);
        }

    }
}
