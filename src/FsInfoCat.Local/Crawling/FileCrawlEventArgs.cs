using FsInfoCat.Services;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FsInfoCat.Local.Crawling
{
    public class FileCrawlEventArgs : CrawlActivityEventArgs, IFsItemCrawlEventArgs
    {
        public FileCrawlEventArgs([DisallowNull] IBgOperationEventArgs bgOperation, [DisallowNull] ICurrentFile file, MessageCode statusDescription, string currentOperation)
            : base(bgOperation, statusDescription, currentOperation ?? file.GetFullName())
        {
            Entity = file.Entity;
            Target = file.Target;
            Parent = file.Parent;
        }

        public ILocalFile Entity { get; }

        public FileInfo Target { get; }

        ILocalDbFsItem ICurrentItem.Entity { get; }

        public ICurrentDirectory Parent { get; }

        public string Name => Target?.Name ?? Entity?.Name ?? "";

        FileSystemInfo ICurrentItem.Target => Target;

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
