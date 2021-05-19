using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Model
{
    public interface IDbContext : IDisposable
    {
        /// <summary>
        /// Gets the datbase entities that contain file system volume information.
        /// </summary>
        /// <value>
        /// The <see cref="IVolume"/> objects for volumes containing file systems that are being tracked..
        /// </value>
        IQueryable<IVolume> Volumes { get; }

        /// <summary>
        /// Gets the datbase entities that contain information about known filesystem types.
        /// </summary>
        /// <value>
        /// The <see cref="IFileSystem"/> objects that contain information about known file system types.
        /// </value>
        IQueryable<IFileSystem> FileSystems { get; }

        /// <summary>
        /// Gets the datbase entities that contain known symbolic names of file system types.
        /// </summary>
        /// <value>
        /// The <see cref="IFsSymbolicName"/> objects that contain known symbolic names that can refer to a particular <see cref="IFileSystem"/>.
        /// </value>
        IQueryable<IFsSymbolicName> SymbolicNames { get; }

        /// <summary>
        /// Gets the datbase entities that represent subdirectories on a file system.
        /// </summary>
        /// <value>
        /// The <see cref="ISubDirectory"/> objects that represent subdirectories on a file system.
        /// </value>
        IQueryable<ISubDirectory> Subdirectories { get; }

        /// <summary>
        /// Gets the database entities that contain information about files found on a file system.
        /// </summary>
        /// <value>
        /// The <see cref="IFile"/> objects that contain information about files found on a file system.
        /// </value>
        IQueryable<IFile> Files { get; }

        /// <summary>
        /// Gets the database entities that contain unique file sizes and MD5 hash codes found among the files that were processed.
        /// </summary>
        /// <value>
        /// The <see cref="IContentInfo"/> objects that contain unique file sizes and MD5 hash codes found among the files that were processed.
        /// </value>
        IQueryable<IContentInfo> ContentInfos { get; }

        /// <summary>
        /// Gets the database entities that contain the results of direct file comparisons.
        /// </summary>
        /// <value>
        /// The <see cref="IFileComparison"/> objects that contain the results of direct file comparisons.
        /// </value>
        IQueryable<IFileComparison> Comparisons { get; }

        IQueryable<IRedundantSet> RedundantSets { get; }

        IQueryable<IRedundancy> Redundancies { get; }

        bool HasChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        IDbContextTransaction BeginTransaction();

        IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel);
    }
}
