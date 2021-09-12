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

namespace FsInfoCat.Desktop.LocalData.VideoPropertySets
{
    /// <summary>
    /// Interaction logic for EditPage.xaml
    /// </summary>
    public partial class EditPage : PageFunction<ItemEditResult>
    {
        public EditPage(EditViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
            viewModel.ChangesSaved += ViewModel_ChangesSaved;
            viewModel.ChangesDiscarded += ViewModel_ChangesDiscarded;
        }

        private void ViewModel_ChangesSaved(object sender, Commands.CommandEventArgs e)
        {
            EditViewModel viewModel = DataContext as EditViewModel;
            if (viewModel is not null)
                OnReturn(new(new(viewModel.ListItem,
                    viewModel.IsNew ? ViewModel.EntityEditResultState.Added : ViewModel.EntityEditResultState.Modified)));
        }

        private void ViewModel_ChangesDiscarded(object sender, Commands.CommandEventArgs e)
        {
            EditViewModel viewModel = DataContext as EditViewModel;
            if (viewModel is not null)
                OnReturn(new(new(viewModel.ListItem,
                    viewModel.IsNew ? ViewModel.EntityEditResultState.Deleted : ViewModel.EntityEditResultState.Unchanged)));
        }
    }
}
