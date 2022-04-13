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

        [NotNull]
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
            Guid id = Id;
            if (id.Equals(Guid.Empty))
                unchecked
                {
                    int hash = 23;
                    hash = hash * 31 + FileSystemDisplayName.GetHashCode();
                    hash = hash * 31 + Name.GetHashCode();
                    hash = hash * 31 + Notes.GetHashCode();
                    hash = hash * 31 + IsInactive.GetHashCode();
                    hash = hash * 31 + Priority.GetHashCode();
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 31);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 31);
                    hash = hash * 31 + CreatedOn.GetHashCode();
                    hash = hash * 31 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return id.GetHashCode();
        }
    }
}
