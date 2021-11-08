using System.Windows.Threading;

namespace FsInfoCat.Desktop
{
    public interface IWindowsOperationProgress : IAsyncOperationProgress
    {
        DispatcherOperation BeginReport(IAsyncOperationInfo operationInfo);
    }
}
