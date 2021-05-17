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
        IQueryable<IHashCalculation> HashCalculations { get; }
        IQueryable<IFileComparison> Comparisons { get; }
        IQueryable<IFile> Files { get; }
        IQueryable<ISubDirectory> Subdirectories { get; }
        IQueryable<IVolume> Volumes { get; }
        IQueryable<IFsSymbolicName> SymbolicNames { get; }
        IQueryable<IFileSystem> FileSystems { get; }
        IQueryable<IRedundancy> Redundancies { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        IDbContextTransaction BeginTransaction();
        IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel);
    }
}
