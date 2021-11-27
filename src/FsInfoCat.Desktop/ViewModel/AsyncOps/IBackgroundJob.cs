using System.Threading.Tasks;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    [System.Obsolete("Use FsInfoCat.AsyncOps.IBackgroundOperation, instead")]
    public interface IBackgroundJob : IAsyncJob
    {
        Task RaiseStatusChangedAsync();
    }
}
