﻿using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Crawling
{
    public class DirectoryCrawlEventArgs : CrawlActivityEventArgs, ICrawlManagerFsItemEventArgs
    {
        public ILocalSubdirectory Target { get; }

        public string FullName { get; }

        ILocalDbFsItem ICurrentItem.Target { get; }

        public DirectoryCrawlEventArgs([DisallowNull] ILocalSubdirectory target, [DisallowNull] string fullName, string message, Guid concurrencyId,
            StatusMessageLevel level = StatusMessageLevel.Information) : base(message, StatusMessageLevel.Information, AsyncJobStatus.Running, concurrencyId)
        {
            Target = target ?? throw new ArgumentNullException(nameof(target));
            FullName = fullName ?? throw new ArgumentNullException(nameof(fullName));
        }

        protected DirectoryCrawlEventArgs([DisallowNull] DirectoryCrawlEventArgs args, string message, StatusMessageLevel level)
            : base(message, level, AsyncJobStatus.Running, (args ?? throw new ArgumentNullException(nameof(args))).ConcurrencyId)
        {
            Target = args.Target;
            FullName = args.FullName;
        }
    }
}