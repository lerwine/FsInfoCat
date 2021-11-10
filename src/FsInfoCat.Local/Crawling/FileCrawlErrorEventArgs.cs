using FsInfoCat.Services;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public class FileCrawlErrorEventArgs : FileCrawlEndEventArgs, ICrawlErrorEventArgs
    {
        public Exception Exception { get; }

        public FileCrawlErrorEventArgs([DisallowNull] Exception exception, [DisallowNull] IBgOperationEventArgs bgOperation, [DisallowNull] ICurrentFile target, MessageCode statusMessage)
            : base(bgOperation, target, statusMessage)
        {
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        }

        public override string ToString() => $@"{base.ToString()}.Exception = {ExtensionMethods.ToPseudoCsText(Exception).AsIndented()}";
    }
}
