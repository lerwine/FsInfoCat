using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
    /// <summary>
    /// Database entity that represents a symbolic name for a file system type.
    /// </summary>
    /// <seealso cref="SymbolicNameRow" />
    /// <seealso cref="ILocalSymbolicName" />
    /// <seealso cref="IEquatable{T}" />
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class SymbolicName : SymbolicNameRow, ILocalSymbolicName, IEquatable<SymbolicName>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        [Obsolete("Replace with ForeignKeyReference<FileSystem>")]
        private Guid? _fileSystemId;
        private FileSystem _fileSystem;

        #region Properties

        public override Guid FileSystemId
        {
            get => _fileSystem?.Id ?? _fileSystemId ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_fileSystem is not null)
                    {
                        if (_fileSystem.Id.Equals(value)) return;
                        _fileSystem = null;
                    }
                    _fileSystemId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_FileSystemRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual FileSystem FileSystem
        {
            get => _fileSystem;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is not null && _fileSystem is not null && ReferenceEquals(value, _fileSystem)) return;
                    _fileSystemId = null;
                    _fileSystem = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        #endregion

        #region Explicit Members

        ILocalFileSystem ILocalSymbolicName.FileSystem { get => FileSystem; }

        IFileSystem ISymbolicName.FileSystem { get => FileSystem; }

        #endregion

        internal static void OnBuildEntity([DisallowNull] EntityTypeBuilder<SymbolicName> builder)
        {
            _ = builder.HasOne(sn => sn.FileSystem).WithMany(d => d.SymbolicNames).HasForeignKey(nameof(FileSystemId)).IsRequired().OnDelete(DeleteBehavior.Restrict);
        }

        internal static async Task<int> ImportAsync(LocalDbContext dbContext, ILogger<LocalDbContext> logger, Guid fileSystemId, XElement symbolicNameElement)
        {
            return await new InsertQueryBuilder(nameof(LocalDbContext.SymbolicNames), symbolicNameElement, nameof(Id)).AppendGuid(nameof(FileSystemId), fileSystemId)
                .AppendString(nameof(Name)).AppendInnerText(nameof(Notes)).AppendBoolean(nameof(IsInactive)).AppendInt32(nameof(Priority))
                .AppendDateTime(nameof(CreatedOn)).AppendDateTime(nameof(ModifiedOn)).AppendDateTime(nameof(LastSynchronizedOn))
                .AppendGuid(nameof(UpstreamId)).ExecuteSqlAsync(dbContext.Database);
        }

        // DEFERRED: Change to async with LocalDbContext
        internal XElement Export(bool includeFileSystemId = false)
        {
            XElement result = new(nameof(FileSystem),
                   new XAttribute(nameof(Id), XmlConvert.ToString(Id)),
                   new XAttribute(nameof(Name), Name)
               );
            if (includeFileSystemId)
            {
                Guid fileSystemId = FileSystemId;
                if (!fileSystemId.Equals(Guid.Empty))
                    result.SetAttributeValue(nameof(fileSystemId), XmlConvert.ToString(fileSystemId));
            }
            if (IsInactive)
                result.SetAttributeValue(nameof(IsInactive), IsInactive);
            if (Priority != 0)
                result.SetAttributeValue(nameof(Priority), Priority);
            AddExportAttributes(result);
            return result;
        }

        /// <summary>
        /// Tests whether the current database entity is equal to another.
        /// </summary>
        /// <param name="other">The <see cref="SymbolicName" /> to compare to.</param>
        /// <returns><see langword="true" /> if the <paramref name="other"/> entity is equal to the current entity; otherwise, <see langword="false" />.</returns>
        public bool Equals(SymbolicName other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        /// <summary>
        /// Tests whether the current database entity is equal to another.
        /// </summary>
        /// <param name="other">The <see cref="ILocalSymbolicName" /> to compare to.</param>
        /// <returns><see langword="true" /> if the <paramref name="other"/> entity is equal to the current entity; otherwise, <see langword="false" />.</returns>
        public bool Equals(ILocalSymbolicName other)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Tests whether the current database entity is equal to another.
        /// </summary>
        /// <param name="other">The <see cref="ISymbolicName" /> to compare to.</param>
        /// <returns><see langword="true" /> if the <paramref name="other"/> entity is equal to the current entity; otherwise, <see langword="false" />.</returns>
        public bool Equals(ISymbolicName other)
        {
            if (other is null) return false;
            if (other is SymbolicName symbolicName) return Equals(symbolicName);
            if (TryGetId(out Guid id1)) return other.TryGetId(out Guid id2) && id1.Equals(id2);
            return !other.TryGetId(out _) && ((other is ILocalSymbolicName local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other));
        }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public override bool Equals(object obj)
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
        {
            if (obj is null) return false;
            if (obj is SymbolicName subdirectory) return Equals(subdirectory);
            return obj is ISymbolicNameRow row && (TryGetId(out Guid id1) ? row.TryGetId(out Guid id2) && id1.Equals(id2) :
                (!row.TryGetId(out _) && ((row is ILocalSymbolicNameRow local) ? ArePropertiesEqual(local) : ArePropertiesEqual(row))));
        }

        public bool TryGetFileSystemId(out Guid fileSystemId)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_fileSystem is null)
                {
                    if (_fileSystemId.HasValue)
                    {
                        fileSystemId = _fileSystemId.Value;
                        return true;
                    }
                }
                else
                    return _fileSystem.TryGetId(out fileSystemId);
            }
            finally { Monitor.Exit(SyncRoot); }
            fileSystemId = Guid.Empty;
            return false;
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
