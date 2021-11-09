using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FsInfoCat.Local.Crawling
{
    public class DirectoryCrawlEventArgs : CrawlActivityEventArgs, IFsItemCrawlEventArgs
    {
        public Subdirectory Entity { get; }

        public DirectoryInfo Target { get; }

        public string Name => Target?.Name ?? Entity?.Name ?? "";

        [Obsolete("Use GetFullName() or GetRelativeParentPath()")]
        public string FullName { get; }

        ILocalDbFsItem ICurrentItem.Entity { get; }

        FileSystemInfo ICurrentItem.Target => Target;

        public DirectoryCrawlEventArgs Parent { get; }

        public DirectoryCrawlEventArgs([DisallowNull] DirectoryCrawlEventArgs currentItem, [DisallowNull] IAsyncOperationInfo progress) : base((currentItem ?? throw new ArgumentNullException(nameof(currentItem))).ConcurrencyId,
            (progress ?? throw new ArgumentNullException(nameof(progress))).Status, progress.Activity ?? currentItem.Activity,
            (progress.Activity.HasValue && progress.StatusDescription.HasValue) ? progress.StatusDescription.Value : currentItem.StatusDescription, progress.CurrentOperation, progress.AsyncState, progress.ParentOperation) { }

        [Obsolete("ICrawlJob can't be passed from worker")]
        // TODO: Create constructor that does not use ICrawlJob parameter
        protected DirectoryCrawlEventArgs([DisallowNull] ICrawlJob source, [DisallowNull] ICurrentDirectory target, MessageCode? statusDescription = null, IAsyncOperationInfo parentOperation = null)
            : base((source ?? throw new ArgumentNullException(nameof(source))).ConcurrencyId, source.Status, source.Activity,
                  statusDescription ?? source.StatusDescription, (target ?? throw new ArgumentNullException(nameof(target))).GetRelativeParentPath(), ((IAsyncOperationInfo)source).AsyncState, parentOperation)
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
