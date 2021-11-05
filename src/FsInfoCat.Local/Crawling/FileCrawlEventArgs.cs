using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FsInfoCat.Local.Crawling
{
    public abstract class FileCrawlEventArgs : CrawlActivityEventArgs, IFsItemCrawlEventArgs
    {
        public ILocalFile Entity { get; }

        [System.Obsolete("Use GetFullName() or GetRelativeParentPath()")]
        public string FullName { get; }

        public FileInfo Target { get; }

        ILocalDbFsItem ICurrentItem.Entity { get; }

        public DirectoryCrawlEventArgs Parent { get; }

        public string Name => Target?.Name ?? Entity?.Name ?? "";

        FileSystemInfo ICurrentItem.Target => Target;

        protected FileCrawlEventArgs([DisallowNull] ICrawlJob source, [DisallowNull] ICurrentFile target, MessageCode? statusDescription = null)
            : base((source ?? throw new ArgumentNullException(nameof(source))).ConcurrencyId, source.Status, source.Activity,
                  statusDescription ?? source.StatusDescription, (target ?? throw new ArgumentNullException(nameof(target))).GetRelativeParentPath(), ((IAsyncOperationInfo)source).AsyncState)
        {
            Target = target.Target;
            Entity = target.Entity;
        }

        public string GetFullName()
        {
            if (Target is null)
            {
                string path = Parent?.GetFullName();
                if (string.IsNullOrEmpty(path))
                    return Target?.Name ?? Entity?.Name.NullIfEmpty();
                string name = Target?.Name ?? Entity?.Name;
                return string.IsNullOrEmpty(name) ? path : Path.Combine(path, name);
            }
            return Target.FullName;
        }

        public string GetRelativeParentPath()
        {
            if (Parent is null)
                return "";
            string path = Parent.GetRelativeParentPath();
            if (string.IsNullOrEmpty(path))
                return Target?.Name ?? Entity?.Name.NullIfEmpty();
            string name = Target?.Name ?? Entity?.Name;
            return string.IsNullOrEmpty(name) ? path : Path.Combine(path, name);
        }

        public override string ToString() => $@"{GetType().ToCsTypeName(true)}
{{
  {base.ToString()},
  Entity = {ExtensionMethods.ToPseudoCsText(Entity).AsIndented()},
  Target = {ExtensionMethods.ToPseudoCsText(Target).AsIndented()},
  Parent = {ExtensionMethods.ToPseudoCsText(Parent).AsIndented()}
}}";
    }
}
