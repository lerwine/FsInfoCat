using System.Threading.Tasks;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public interface IAsyncOpViewModel
    {
        void Cancel(bool throwOnFirstException);
        void Cancel();
        Task GetTask();
    }
}
