using System.Threading.Tasks;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public interface IBackgroundJob : IAsyncJob
    {
        Task RaiseStatusChangedAsync();
    }
}
