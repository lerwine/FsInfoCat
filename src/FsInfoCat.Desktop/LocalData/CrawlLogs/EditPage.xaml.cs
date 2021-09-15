using FsInfoCat.Desktop.ViewModel;
using System.Windows.Navigation;

namespace FsInfoCat.Desktop.LocalData.CrawlLogs
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
            viewModel.Completed += (object sender, ItemFunctionResultEventArgs e) => OnReturn(new ReturnEventArgs<ItemFunctionResultEventArgs>(e));
        }
    }
}
