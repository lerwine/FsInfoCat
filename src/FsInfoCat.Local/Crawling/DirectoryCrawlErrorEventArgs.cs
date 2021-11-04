using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public class DirectoryCrawlErrorEventArgs : DirectoryCrawlEndEventArgs, ICrawlErrorEventArgs
    {
        public Exception Exception { get; }

        public DirectoryCrawlErrorEventArgs([DisallowNull] Exception exception, [DisallowNull] DirectoryCrawlEventArgs args, string message = null, StatusMessageLevel level = StatusMessageLevel.Error, AsyncJobStatus status = AsyncJobStatus.Faulted)
            : base(args, string.IsNullOrWhiteSpace(message) ? (string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message) : message, level, status)
        {
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        }

        public override string ToString() => $@"{base.ToString()}.Exception = {ExtensionMethods.ToPseudoCsText(Exception).AsIndented()}";
    }
}
