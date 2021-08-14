using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.BgOps
{
    public partial class BackgroundJobViewModel
    {
        public class Context<TState> : IContext<TState>
        {
            private readonly BackgroundJobViewModel _viewModel;
            private Action<JobProgressEventArgs<TState>> _raiseProgressEvent;

            public TState State { get; private set; }

            public CancellationToken Token { get; }

            internal Context(TState state, CancellationToken token, BackgroundJobViewModel viewModel, Action<JobProgressEventArgs<TState>> raiseProgressEvent)
            {
                State = state;
                Token = token;
                _viewModel = viewModel;
                _raiseProgressEvent = raiseProgressEvent;
            }

            private void OnReportStatus(TState state, string message, Exception exception, string detail, StatusMessageLevel level)
            {
                _viewModel.StatusMessage = message;
                _viewModel.StatusDetail = detail;
                _viewModel.MessageLevel = level;
                _raiseProgressEvent(new JobProgressEventArgs<TState>
                {
                    State = state,
                    Message = message ?? "",
                    Exception = exception,
                    Detail = detail ?? "",
                    Level = level,
                    IsCompleted = false
                });
            }

            public void ReportProgress(string message, Exception exception, string detail = null, StatusMessageLevel level = StatusMessageLevel.Error,
                DispatcherPriority priority = DispatcherPriority.Background) =>
                ReportStatus(State, message, exception, detail, level, priority);

            public void ReportProgress(string message, Exception exception, StatusMessageLevel level, DispatcherPriority priority = DispatcherPriority.Background) =>
                ReportProgress(message, exception, null, level, priority);

            public void ReportProgress(string message, string detail = null, StatusMessageLevel level = StatusMessageLevel.Information,
                DispatcherPriority priority = DispatcherPriority.Background) => ReportStatus(State, message, detail, level, priority);

            public void ReportProgress(string message, StatusMessageLevel level, DispatcherPriority priority = DispatcherPriority.Background) =>
                ReportProgress(message, (string)null, level, priority);

            public DispatcherOperation ReportProgressAsync(string message, Exception exception, string detail = null, StatusMessageLevel level = StatusMessageLevel.Error,
                DispatcherPriority priority = DispatcherPriority.Background) => ReportStatusAsync(State, message, exception, detail, level, priority);

            public DispatcherOperation ReportProgressAsync(string message, Exception exception, StatusMessageLevel level,
                DispatcherPriority priority = DispatcherPriority.Background) => ReportProgressAsync(message, exception, null, level, priority);

            public DispatcherOperation ReportProgressAsync(string message, string detail = null, StatusMessageLevel level = StatusMessageLevel.Information,
                DispatcherPriority priority = DispatcherPriority.Background) => ReportStatusAsync(State, message, detail, level, priority);

            public DispatcherOperation ReportProgressAsync(string message, StatusMessageLevel level, DispatcherPriority priority = DispatcherPriority.Background) =>
                ReportProgressAsync(message, (string)null, level, priority);

            public void ReportStatus(TState state, string message, Exception exception, string detail = null, StatusMessageLevel level = StatusMessageLevel.Error,
                DispatcherPriority priority = DispatcherPriority.Background) =>
                _viewModel.Dispatcher.Invoke(() => OnReportStatus(state, message, exception, detail, level), priority, Token);

            public void ReportStatus(TState state, string message, Exception exception, StatusMessageLevel level, DispatcherPriority priority = DispatcherPriority.Background) =>
                ReportStatus(state, message, exception, null, level, priority);

            public void ReportStatus(TState state, string message, string detail = null, StatusMessageLevel level = StatusMessageLevel.Information,
                DispatcherPriority priority = DispatcherPriority.Background) =>
                _viewModel.Dispatcher.Invoke(() => OnReportStatus(state, message, null, detail, level), priority, Token);

            public void ReportStatus(TState state, string message, StatusMessageLevel level, DispatcherPriority priority = DispatcherPriority.Background) =>
                ReportStatus(state, message, (string)null, level, priority);

            public DispatcherOperation ReportStatusAsync(TState state, string message, Exception exception, string detail = null, StatusMessageLevel level = StatusMessageLevel.Error,
                DispatcherPriority priority = DispatcherPriority.Background) =>
                _viewModel.Dispatcher.InvokeAsync(() => OnReportStatus(state, message, exception, detail, level), priority, Token);

            public DispatcherOperation ReportStatusAsync(TState state, string message, Exception exception, StatusMessageLevel level,
                DispatcherPriority priority = DispatcherPriority.Background) => ReportStatusAsync(state, message, exception, null, level, priority);

            public DispatcherOperation ReportStatusAsync(TState state, string message, string detail = null, StatusMessageLevel level = StatusMessageLevel.Information,
                DispatcherPriority priority = DispatcherPriority.Background) =>
                _viewModel.Dispatcher.InvokeAsync(() => OnReportStatus(state, message, null, detail, level), priority, Token);

            public DispatcherOperation ReportStatusAsync(TState state, string message, StatusMessageLevel level, DispatcherPriority priority = DispatcherPriority.Background) =>
                ReportStatusAsync(state, message, (string)null, level, priority);
        }
    }
}
