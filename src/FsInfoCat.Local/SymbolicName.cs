using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace FsInfoCat.Local
{
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class SymbolicName : SymbolicNameRow, ILocalSymbolicName, ISimpleIdentityReference<SymbolicName>, IEquatable<SymbolicName>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        private readonly ForeignKeyReference<FileSystem> _fileSystemNav;

        #region Properties

        public override Guid FileSystemId
        {
            get => _fileSystemNav.Id;
            set => _fileSystemNav.SetId(value);
        }

        [Display(Name = nameof(FsInfoCat.Properties.Resources.DisplayName_FileSystem), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        [Required(ErrorMessageResourceName = nameof(FsInfoCat.Properties.Resources.ErrorMessage_FileSystemRequired),
            ErrorMessageResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual FileSystem FileSystem
        {
            get => _fileSystemNav.Entity;
            set => _fileSystemNav.Entity = value;
        }

        #endregion

        #region Explicit Members

        ILocalFileSystem ILocalSymbolicName.FileSystem { get => FileSystem; }

        IFileSystem ISymbolicName.FileSystem { get => FileSystem; }

        SymbolicName IIdentityReference<SymbolicName>.Entity => this;

        #endregion

        public SymbolicName() => _fileSystemNav = new(null, SyncRoot);

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
            // TODO: Implement Equals(object)
            throw new NotImplementedException();
        }

        public bool TryGetFileSystemId(out Guid fileSystemId) => _fileSystemNav.TryGetId(out fileSystemId);
    }
}
