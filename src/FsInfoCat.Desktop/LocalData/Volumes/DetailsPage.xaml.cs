using FsInfoCat.Desktop.Commands;
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

namespace FsInfoCat.Desktop.LocalData.Volumes
{
    /// <summary>
    /// Interaction logic for DetailsPage.xaml
    /// </summary>
    public partial class DetailsPage : PageFunction<ItemEditResult>
    {
        public DetailsPage(DetailsViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.ChangesSaved += ViewModel_ChangesSaved;
        }

        private void ViewModel_ChangesSaved(object sender, CommandEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}