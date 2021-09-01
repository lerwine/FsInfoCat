using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public class StatusListener : IWindowsStatusListener
    {
        private readonly BackgroundJobVM _viewModel;

        internal StatusListener([DisallowNull] BackgroundJobVM viewModel, Guid concurrencyId, [DisallowNull] ILogger logger, CancellationToken token)
        {
            Logger = logger;
            _viewModel = viewModel;
            CancellationToken = token;
            ConcurrencyId = concurrencyId;
        }

        public CancellationToken CancellationToken { get; }

        public Guid ConcurrencyId { get; }

        [NotNull]
        public ILogger Logger { get; }

        public DispatcherOperation BeginSetMessage([AllowNull] string message, StatusMessageLevel level) => _viewModel.Dispatcher.BeginInvoke(() =>
        {
            _viewModel.MessageLevel = level;
            _viewModel.Message = message;
        }, DispatcherPriority.Background);

        public DispatcherOperation BeginSetMessage([AllowNull] string message) => _viewModel.Dispatcher.BeginInvoke(() => _viewModel.Message = message);

        public void SetMessage([AllowNull] string message, StatusMessageLevel level, TimeSpan timeout) => _viewModel.Dispatcher.CheckInvoke(CancellationToken, timeout, () =>
        {
            _viewModel.MessageLevel = level;
            _viewModel.Message = message;
        });

        public void SetMessage([AllowNull] string message, StatusMessageLevel level) => _viewModel.Dispatcher.CheckInvoke(CancellationToken, () =>
        {
            _viewModel.MessageLevel = level;
            _viewModel.Message = message;
        });

        public void SetMessage([AllowNull] string message, TimeSpan timeout) => _viewModel.Dispatcher.CheckInvoke(CancellationToken, timeout, () => _viewModel.Message = message);

        public void SetMessage([AllowNull] string message) => _viewModel.Dispatcher.CheckInvoke(CancellationToken, () => _viewModel.Message = message);

        Task IStatusListener.BeginSetMessage(string message, StatusMessageLevel level) => BeginSetMessage(message, level).Task;

        Task IStatusListener.BeginSetMessage(string message) => BeginSetMessage(message).Task;
    }
}
