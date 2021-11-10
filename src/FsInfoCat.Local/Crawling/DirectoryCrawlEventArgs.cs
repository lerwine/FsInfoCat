using FsInfoCat.Services;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace FsInfoCat.Local.Crawling
{
    public class DirectoryCrawlEventArgs : CrawlActivityEventArgs, IFsItemCrawlEventArgs
    {
        public DirectoryCrawlEventArgs([DisallowNull] IBgOperationEventArgs bgOperation, [DisallowNull] ICurrentDirectory directory, MessageCode statusDescription, string currentOperation)
            : base(bgOperation, statusDescription, currentOperation ?? directory.GetFullName())
        {
            Entity = directory.Entity;
            Target = directory.Target;
            Parent = directory.Parent;
        }

        public Subdirectory Entity { get; }

        public DirectoryInfo Target { get; }

        public string Name => Target?.Name ?? Entity?.Name ?? "";

        ILocalDbFsItem ICurrentItem.Entity => Entity;

        FileSystemInfo ICurrentItem.Target => Target;

        public ICurrentDirectory Parent { get; }

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
