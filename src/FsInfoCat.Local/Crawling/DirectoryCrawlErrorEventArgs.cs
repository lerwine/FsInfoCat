using FsInfoCat.Services;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public class DirectoryCrawlErrorEventArgs : DirectoryCrawlEndEventArgs, ICrawlErrorEventArgs
    {
        public Exception Exception { get; }
        
        public DirectoryCrawlErrorEventArgs([DisallowNull] Exception exception, [DisallowNull] IBgOperationEventArgs bgOperation, [DisallowNull] ICurrentDirectory target, MessageCode statusMessage)
            : base(bgOperation, target, statusMessage)
        {
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        }

        public override string ToString() => $@"{base.ToString()}.Exception = {ExtensionMethods.ToPseudoCsText(Exception).AsIndented()}";
    }
}
