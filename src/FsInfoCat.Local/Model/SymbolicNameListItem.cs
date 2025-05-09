using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// List item DB entity that representa a symbolic name for a file system type.
    /// </summary>
    /// <seealso cref="SymbolicName" />
    /// <seealso cref="LocalDbContext.SymbolicNameListing" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class SymbolicNameListItem : SymbolicNameRow, ILocalSymbolicNameListItem, IEquatable<SymbolicNameListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private const string VIEW_NAME = "vSymbolicNameListing";

        private string _fileSystemDisplayName = string.Empty;

        [NotNull]
        [BackingField(nameof(_fileSystemDisplayName))]
        public string FileSystemDisplayName { get => _fileSystemDisplayName; set => _fileSystemDisplayName = value.AsWsNormalizedOrEmpty(); }

        internal static void OnBuildEntity(EntityTypeBuilder<SymbolicNameListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public bool Equals(SymbolicNameListItem other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public bool Equals(ISymbolicNameListItem other)
        {
            if (other is null) return false;
            if (other is SymbolicNameListItem symbolicName) return Equals(symbolicName);
            if (TryGetId(out Guid id1)) return other.TryGetId(out Guid id2) && id1.Equals(id2);
            return !other.TryGetId(out _) && ((other is ILocalSymbolicNameListItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other));
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is SymbolicNameListItem listItem) return Equals(listItem);
            if (obj is ISymbolicNameListItem other)
            {
                if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
                return !other.TryGetId(out _) && (other is ILocalSymbolicNameListItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
            }
            return false;
        }

        protected override string PropertiesToString() => $"{base.PropertiesToString()}, FileSystemId={FileSystemId}, FileSystemDisplayName=\"{ExtensionMethods.EscapeCsString(_fileSystemDisplayName)}\"";
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member

    }
}
