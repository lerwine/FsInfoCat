using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    public interface IDbContext : IDisposable
    {
        IEnumerable<IComparison> Comparisons { get; }

        IEnumerable<IBinaryProperties> BinaryProperties { get; }

        [Obsolete()]
        IEnumerable<IExtendedProperties> ExtendedProperties { get; }

        IEnumerable<ISummaryProperties> SummaryProperties { get; }

        IEnumerable<IDocumentProperties> DocumentProperties { get; }

        IEnumerable<IAudioProperties> AudioProperties { get; }

        IEnumerable<IDRMProperties> DRMProperties { get; }

        IEnumerable<IGPSProperties> GPSProperties { get; }

        IEnumerable<IImageProperties> ImageProperties { get; }

        IEnumerable<IMediaProperties> MediaProperties { get; }

        IEnumerable<IMusicProperties> MusicProperties { get; }

        IEnumerable<IPhotoProperties> PhotoProperties { get; }

        IEnumerable<IRecordedTVProperties> RecordedTVProperties { get; }

        IEnumerable<IVideoProperties> VideoProperties { get; }

        IEnumerable<IAccessError<IFile>> FileAccessErrors { get; }

        IEnumerable<IFile> Files { get; }

        IEnumerable<IFileSystem> FileSystems { get; }

        IEnumerable<IRedundancy> Redundancies { get; }

        IEnumerable<IRedundantSet> RedundantSets { get; }

        IEnumerable<ISubdirectory> Subdirectories { get; }

        IEnumerable<IAccessError<ISubdirectory>> SubdirectoryAccessErrors { get; }

        IEnumerable<ISymbolicName> SymbolicNames { get; }

        IEnumerable<IAccessError<IVolume>> VolumeAccessErrors { get; }

        IEnumerable<IVolume> Volumes { get; }

        IEnumerable<ICrawlConfiguration> CrawlConfigurations { get; }

        void ForceDeleteBinaryProperties(IBinaryProperties target);

        void ForceDeleteRedundantSet(IRedundantSet target);

        int SaveChanges();

        int SaveChanges(bool acceptAllChangesOnSuccess);

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        EntityEntry<TEntity> Entry<TEntity>([NotNull] TEntity entity) where TEntity : class;
    }
}
