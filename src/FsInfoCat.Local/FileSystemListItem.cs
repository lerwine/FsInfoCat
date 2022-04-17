using FsInfoCat.Activities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public class FileSystemListItem : FileSystemRow, ILocalFileSystemListItem, IEquatable<FileSystemListItem>
    {
        public const string VIEW_NAME = "vFileSystemListing";

        public Guid? PrimarySymbolicNameId { get; set; }

        [NotNull]
        public string PrimarySymbolicName { get; set; }

        public long SymbolicNameCount { get; set; }

        public long VolumeCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<FileSystemListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

        protected bool ArePropertiesEqual([DisallowNull] ILocalFileSystemListItem other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IFileSystemListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(FileSystemListItem other) => other is not null && ReferenceEquals(this, other) || Id.Equals(Guid.Empty) ? ArePropertiesEqual(this) : Id.Equals(other.Id);

        public bool Equals(IFileSystemListItem other)
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
                    int hash = 29;
                    hash = hash * 37 + DisplayName.GetHashCode();
                    hash = hash * 37 + ReadOnly.GetHashCode();
                    hash = hash * 37 + MaxNameLength.GetHashCode();
                    hash = EntityExtensions.HashNullable(DefaultDriveType, hash, 37);
                    hash = hash * 37 + Notes.GetHashCode();
                    hash = hash * 37 + IsInactive.GetHashCode();
                    hash = EntityExtensions.HashNullable(UpstreamId, hash, 37);
                    hash = EntityExtensions.HashNullable(LastSynchronizedOn, hash, 37);
                    hash = hash * 37 + CreatedOn.GetHashCode();
                    hash = hash * 37 + ModifiedOn.GetHashCode();
                    return hash;
                }
            return id.GetHashCode();
        }

        public async Task<(SymbolicName[], VolumeListItem[])> LoadRelatedItemsAsync(IActivityProgress progress)
        {
            Guid id = Id;
            using IServiceScope scope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = scope.ServiceProvider.GetRequiredService<LocalDbContext>();
            SymbolicName[] symbolicNames = await dbContext.SymbolicNames.Where(n => n.FileSystemId == id).ToArrayAsync(progress.Token);
            VolumeListItem[] volumes = await dbContext.VolumeListing.Where(v => v.FileSystemId == id).ToArrayAsync(progress.Token);
            SymbolicNameCount = symbolicNames.LongLength;
            VolumeCount = volumes.LongLength;
            return (symbolicNames, volumes);
        }
    }
}
