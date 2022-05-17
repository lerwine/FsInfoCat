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
    // TODO: Document FileSystemListItem class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    public class FileSystemListItem : FileSystemRow, ILocalFileSystemListItem, IEquatable<FileSystemListItem>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    {
        public const string VIEW_NAME = "vFileSystemListing";

        public Guid? PrimarySymbolicNameId { get; set; }

        [NotNull]
        public string PrimarySymbolicName { get; set; }

        public long SymbolicNameCount { get; set; }

        public long VolumeCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<FileSystemListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

        public bool Equals(FileSystemListItem other) => other is not null && (ReferenceEquals(this, other) ||
            (TryGetId(out Guid id) ? other.TryGetId(out Guid id2) && id.Equals(id2) : !other.TryGetId(out _) && ArePropertiesEqual(other)));

        public bool Equals(IFileSystemListItem other)
        {
            if (other is null) return false;
            if (other is FileSystemListItem listItem) return Equals(listItem);
            if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
            return !other.TryGetId(out _) && (other is ILocalFileSystemListItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (obj is FileSystemListItem listItem) return Equals(listItem);
            if (obj is IFileSystemListItem other)
            {
                if (TryGetId(out Guid id)) return other.TryGetId(out Guid id2) && id.Equals(id2);
                return !other.TryGetId(out _) && (other is ILocalFileSystemListItem local) ? ArePropertiesEqual(local) : ArePropertiesEqual(other);
            }
            return false;
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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
