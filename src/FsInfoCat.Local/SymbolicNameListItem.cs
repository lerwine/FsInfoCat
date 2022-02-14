using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class SymbolicNameListItem : SymbolicNameRow, ILocalSymbolicNameListItem, IEquatable<SymbolicNameListItem>
    {
        public const string VIEW_NAME = "vSymbolicNameListing";

        private string _fileSystemDisplayName = string.Empty;

        public string FileSystemDisplayName { get => _fileSystemDisplayName; set => _fileSystemDisplayName = value.AsWsNormalizedOrEmpty(); }

        internal static void OnBuildEntity(EntityTypeBuilder<SymbolicNameListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

        protected bool ArePropertiesEqual([DisallowNull] ILocalSymbolicNameListItem other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] ISymbolicNameListItem other)
        {
            throw new NotImplementedException();
        }

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
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
