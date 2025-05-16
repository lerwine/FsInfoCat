using FsInfoCat.Desktop.ViewModel;
using System.Windows.Navigation;

namespace FsInfoCat.Desktop.LocalData.SharedTagDefinitions
{
    /// <summary>
    /// Interaction logic for EditPage.xaml
    /// </summary>
    public partial class EditPage : PageFunction<ItemFunctionResultEventArgs>
    {
        public EditPage(EditViewModel viewModel)
        {
            InitializeComponent();
            viewModel.Completed += (object sender, ItemFunctionResultEventArgs e) => OnReturn(new ReturnEventArgs<ItemFunctionResultEventArgs>(e));
        }
    }
}
