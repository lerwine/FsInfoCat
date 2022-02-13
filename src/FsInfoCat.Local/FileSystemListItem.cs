using System;
using System.Linq;
using System.Threading.Tasks;
using FsInfoCat.Activities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.DependencyInjection;

namespace FsInfoCat.Local
{
    public class FileSystemListItem : FileSystemRow, ILocalFileSystemListItem
    {
        public const string VIEW_NAME = "vFileSystemListing";

        public Guid? PrimarySymbolicNameId { get; set; }

        public string PrimarySymbolicName { get; set; }

        public long SymbolicNameCount { get; set; }

        public long VolumeCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<FileSystemListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

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
