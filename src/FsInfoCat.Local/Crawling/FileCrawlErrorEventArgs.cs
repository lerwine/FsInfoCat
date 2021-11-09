using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public class FileCrawlErrorEventArgs : FileCrawlEndEventArgs, ICrawlErrorEventArgs
    {
        public Exception Exception { get; }

        // TODO: Create constructor that does not use ICrawlJob parameter
        public FileCrawlErrorEventArgs([DisallowNull] Exception exception, [DisallowNull] ICrawlJob source, [DisallowNull] ICurrentFile target, MessageCode statusMessage)
            : base(source, target, statusMessage)
        {
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        }
        public override string ToString() => $@"{base.ToString()}.Exception = {ExtensionMethods.ToPseudoCsText(Exception).AsIndented()}";
    }
}
