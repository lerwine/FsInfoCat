using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public class DirectoryCrawlErrorEventArgs : DirectoryCrawlEndEventArgs, ICrawlErrorEventArgs
    {
        public Exception Exception { get; }

        public DirectoryCrawlErrorEventArgs([DisallowNull] Exception exception, [DisallowNull] ICrawlJob source, [DisallowNull] ICurrentDirectory target, MessageCode statusMessage)
            : base(source, target, statusMessage)
        {
            Exception = exception ?? throw new ArgumentNullException(nameof(exception));
        }

        public override string ToString() => $@"{base.ToString()}.Exception = {ExtensionMethods.ToPseudoCsText(Exception).AsIndented()}";
    }
}
