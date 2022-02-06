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

        private readonly IPropertyChangeTracker<Guid?> _primarySymbolicNameId;
        private readonly IPropertyChangeTracker<string> _primarySymbolicName;
        private readonly IPropertyChangeTracker<long> _symbolicNameCount;
        private readonly IPropertyChangeTracker<long> _volumeCount;

        public Guid? PrimarySymbolicNameId { get => _primarySymbolicNameId.GetValue(); set => _primarySymbolicNameId.SetValue(value); }

        public string PrimarySymbolicName { get => _primarySymbolicName.GetValue(); set => _primarySymbolicName.SetValue(value); }

        public long SymbolicNameCount { get => _symbolicNameCount.GetValue(); set => _symbolicNameCount.SetValue(value); }

        public long VolumeCount { get => _volumeCount.GetValue(); set => _volumeCount.SetValue(value); }

        internal static void OnBuildEntity(EntityTypeBuilder<FileSystemListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

        public FileSystemListItem()
        {
            _primarySymbolicNameId = AddChangeTracker<Guid?>(nameof(PrimarySymbolicNameId), null);
            _primarySymbolicName = AddChangeTracker(nameof(PrimarySymbolicName), "", TrimmedNonNullStringCoersion.Default);
            _symbolicNameCount = AddChangeTracker(nameof(SymbolicNameCount), 0L);
            _volumeCount = AddChangeTracker(nameof(VolumeCount), 0L);
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
