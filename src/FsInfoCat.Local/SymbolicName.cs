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
    public class SymbolicName : SymbolicNameRow, ILocalSymbolicName, ISimpleIdentityReference<SymbolicName>, IEquatable<SymbolicName>
    {
        private FileSystem _fileSystem;

        public override Guid FileSystemId
        {
            get
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _fileSystem?.Id;
                    if (id.HasValue && id.Value != base.FileSystemId)
                    {
                        base.FileSystemId = id.Value;
                        return id.Value;
                    }
                    return base.FileSystemId;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    Guid? id = _fileSystem?.Id;
                    if (id.HasValue && id.Value != value)
                        _fileSystem = null;
                    base.FileSystemId = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_FileSystemRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        [BackingField(nameof(_fileSystem))]
        public virtual FileSystem FileSystem
        {
            get => _fileSystem;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (value is null)
                    {
                        if (_fileSystem is not null)
                            base.FileSystemId = Guid.Empty;
                    }
                    else
                        base.FileSystemId = value.Id;
                    _fileSystem = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        #region Explicit Members

        ILocalFileSystem ILocalSymbolicName.FileSystem { get => FileSystem; }

        IFileSystem ISymbolicName.FileSystem { get => FileSystem; }

        SymbolicName IIdentityReference<SymbolicName>.Entity => this;

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

        protected bool ArePropertiesEqual([DisallowNull] ILocalSymbolicName other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] ISymbolicName other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(SymbolicName other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(ISymbolicName other)
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
                    int hash = EntityExtensions.HashRelatedEntity(FileSystem, () => FileSystemId, 23, 31);
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
