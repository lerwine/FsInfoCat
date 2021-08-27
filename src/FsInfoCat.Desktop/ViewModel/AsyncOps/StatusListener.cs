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

        internal StatusListener(BackgroundJobVM viewModel, Guid concurrencyId, CancellationToken token)
        {
            _viewModel = viewModel;
            CancellationToken = token;
            ConcurrencyId = concurrencyId;
        }

        public CancellationToken CancellationToken { get; }

        public Guid ConcurrencyId { get; }

        public ILogger Logger => throw new NotImplementedException();

        public DispatcherOperation BeginSetMessage([AllowNull] string message, StatusMessageLevel level)
        {
            throw new NotImplementedException();
        }

        public DispatcherOperation BeginSetMessage([AllowNull] string message)
        {
            throw new NotImplementedException();
        }

        public void SetMessage([AllowNull] string message, StatusMessageLevel level, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public void SetMessage([AllowNull] string message, StatusMessageLevel level)
        {
            throw new NotImplementedException();
        }

        public void SetMessage([AllowNull] string message, TimeSpan timeout)
        {
            throw new NotImplementedException();
        }

        public void SetMessage([AllowNull] string message)
        {
            throw new NotImplementedException();
        }

        Task IStatusListener.BeginSetMessage(string message, StatusMessageLevel level)
        {
            throw new NotImplementedException();
        }

        Task IStatusListener.BeginSetMessage(string message)
        {
            throw new NotImplementedException();
        }
    }
}
