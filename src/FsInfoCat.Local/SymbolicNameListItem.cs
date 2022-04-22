using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class SymbolicNameListItem : SymbolicNameRow, ILocalSymbolicNameListItem, IEquatable<SymbolicNameListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public const string VIEW_NAME = "vSymbolicNameListing";

        private string _fileSystemDisplayName = string.Empty;

        [NotNull]
        [BackingField(nameof(_fileSystemDisplayName))]
        public string FileSystemDisplayName { get => _fileSystemDisplayName; set => _fileSystemDisplayName = value.AsWsNormalizedOrEmpty(); }

        internal static void OnBuildEntity(EntityTypeBuilder<SymbolicNameListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

        public bool Equals(SymbolicNameListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(ISymbolicNameListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            // TODO: Implement Equals(object)
            throw new NotImplementedException();
        }
    }
}
