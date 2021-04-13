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

namespace DevHelperUI.RegexBuilder
{
    /// <summary>
    /// Interaction logic for RegexBuilderWindow.xaml
    /// </summary>
    public partial class RegexBuilderWindow : Window
    {
        public RegexBuilderWindow()
        {
            InitializeComponent();
            DataContext = ViewModel = new RegexBuilderViewModel();
        }

        public RegexBuilderViewModel ViewModel { get; }
    }
}
