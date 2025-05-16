using System.Windows;

namespace FsInfoCat.Desktop
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(IApplicationNavigation dataContext)
        {
            InitializeComponent();
            DataContext = dataContext;
        }
    }
}
