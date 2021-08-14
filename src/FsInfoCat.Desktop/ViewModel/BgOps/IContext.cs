using System;
using System.Threading;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.BgOps
{
    public interface IContext
    {
        CancellationToken Token { get; }
        void ReportProgress(string message, Exception exception, string detail = null, StatusMessageLevel level = StatusMessageLevel.Error,
            DispatcherPriority priority = DispatcherPriority.Background);
        public void ReportProgress(string message, Exception exception, StatusMessageLevel level, DispatcherPriority priority = DispatcherPriority.Background);
        public void ReportProgress(string message, string detail = null, StatusMessageLevel level = StatusMessageLevel.Information,
            DispatcherPriority priority = DispatcherPriority.Background);
        public void ReportProgress(string message, StatusMessageLevel level, DispatcherPriority priority = DispatcherPriority.Background);
        public DispatcherOperation ReportProgressAsync(string message, Exception exception, string detail = null, StatusMessageLevel level = StatusMessageLevel.Error,
            DispatcherPriority priority = DispatcherPriority.Background);
        public DispatcherOperation ReportProgressAsync(string message, Exception exception, StatusMessageLevel level,
            DispatcherPriority priority = DispatcherPriority.Background);
        public DispatcherOperation ReportProgressAsync(string message, string detail = null, StatusMessageLevel level = StatusMessageLevel.Information,
            DispatcherPriority priority = DispatcherPriority.Background);
        public DispatcherOperation ReportProgressAsync(string message, StatusMessageLevel level, DispatcherPriority priority = DispatcherPriority.Background);
    }

    public interface IContext<TState> : IContext
    {
        TState State { get; }
        void ReportStatus(TState state, string message, Exception exception, string detail = null, StatusMessageLevel level = StatusMessageLevel.Error,
                DispatcherPriority priority = DispatcherPriority.Background);
        void ReportStatus(TState state, string message, Exception exception, StatusMessageLevel level, DispatcherPriority priority = DispatcherPriority.Background);
        void ReportStatus(TState state, string message, string detail = null, StatusMessageLevel level = StatusMessageLevel.Information,
            DispatcherPriority priority = DispatcherPriority.Background);
        void ReportStatus(TState state, string message, StatusMessageLevel level, DispatcherPriority priority = DispatcherPriority.Background);
        DispatcherOperation ReportStatusAsync(TState state, string message, Exception exception, string detail = null, StatusMessageLevel level = StatusMessageLevel.Error,
            DispatcherPriority priority = DispatcherPriority.Background);
        DispatcherOperation ReportStatusAsync(TState state, string message, Exception exception, StatusMessageLevel level,
            DispatcherPriority priority = DispatcherPriority.Background);
        DispatcherOperation ReportStatusAsync(TState state, string message, string detail = null, StatusMessageLevel level = StatusMessageLevel.Information,
            DispatcherPriority priority = DispatcherPriority.Background);
        public DispatcherOperation ReportStatusAsync(TState state, string message, StatusMessageLevel level, DispatcherPriority priority = DispatcherPriority.Background);
    }
}
