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
            using (Local.LocalDbContext context = new Local.LocalDbContext())
            {
                Local.SymbolicName sn = new Local.SymbolicName();
                sn.Name = "Yes";
                Local.FileSystem fs = new Local.FileSystem();
                fs.DisplayName = "test";
                fs.DefaultSymbolicName = sn;
                context.FileSystems.Attach(fs);
                context.Add(fs);
                context.SaveChanges();
            }
        }
    }
}
