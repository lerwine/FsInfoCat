using FsInfoCat.Desktop.ViewModel;
using System.Windows.Navigation;

namespace FsInfoCat.Desktop.LocalData.SymbolicNames
{
    /// <summary>
    /// Interaction logic for EditPage.xaml
    /// </summary>
    public partial class EditPage : PageFunction<ItemFunctionResultEventArgs>
    {
        public EditPage(EditViewModel viewModel)
        {
            InitializeComponent();
            DataContext = viewModel;
#warning View model needs to implement Completed event
            //viewModel.Completed += (object sender, ItemFunctionResultEventArgs e) => OnReturn(new ReturnEventArgs<ItemFunctionResultEventArgs>(e));
        }
    }
}
