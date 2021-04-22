using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FsInfoCat.Desktop.Model
{
    public static class Extensions
    {
        public static UserRole AsNormalized(this UserRole userRole)
        {
            if (userRole.HasFlag(UserRole.Administrator))
            {
                foreach (UserRole r in Enum.GetValues(typeof(UserRole)))
                    userRole |= r;
            }
            else if (userRole != UserRole.None)
                return userRole | UserRole.Viewer;
            return userRole;
        }

        public static UserRole GetEffectiveRoles(this UserAccount user)
        {
            if (user is null)
                return UserRole.None;
            if (user.Memberships is null)
                return user.ExplicitRoles;
            return AsNormalized(user.Memberships.Select(m => (m is null || m.Group is null) ? UserRole.None : m.Group.Roles).Aggregate(user.ExplicitRoles, (a, r) => a | r));
        }

        public static UserAccount GetUserById(this DbModel dbContext, Guid id) => (dbContext is null) ? null : GetUserById(dbContext.UserAccounts, id);

        public static UserAccount GetUserById(this DbSet<UserAccount> dbSet, Guid id)
        {
            if (dbSet is null)
                return null;
            return (from u in dbSet where u.Id == id select u).FirstOrDefault();
        }

        public static UserAccount GetSystemAccount(this DbModel dbContext)
        {
            Guid id = Guid.Empty;
            UserAccount userAccount = GetUserById(dbContext, id);
            if (userAccount is null)
                throw new InvalidOperationException("Could not find system account");
            return userAccount;
        }
    }

    public partial class DbModel : IDbContext
    {
        internal DbModel(string connectionString) : base(connectionString) { }

        IQueryable<IChecksumCalculation> IDbContext.Checksums => ChecksumCalculations.Cast<IChecksumCalculation>();

        IQueryable<IFileComparison> IDbContext.Comparisons => Comparisons.Cast<IFileComparison>();

        IQueryable<IFile> IDbContext.Files => Files.Cast<IFile>();

        IQueryable<ISubDirectory> IDbContext.Subdirectories => Subdirectories.Cast<ISubDirectory>();

        IQueryable<IVolume> IDbContext.Volumes => Volumes.Cast<IVolume>();
    }

    public partial class ChecksumCalculation : IChecksumCalculation
    {
        private ReadOnlyCollectionDelegateWrapper<byte, byte> _checksumWrapper;
        private ReadOnlyCollectionDelegateWrapper<File, IFile> _filesWrapper;

        IReadOnlyCollection<byte> IChecksumCalculation.Checksum
        {
            get
            {
                ReadOnlyCollectionDelegateWrapper<byte, byte> result = _checksumWrapper;
                if (result is null)
                    _checksumWrapper = result = new ReadOnlyCollectionDelegateWrapper<byte, byte>(() => Checksum);
                return result;
            }
        }

        IReadOnlyCollection<IFile> IChecksumCalculation.Files
        {
            get
            {
                ReadOnlyCollectionDelegateWrapper<File, IFile> result = _filesWrapper;
                if (result is null)
                    _filesWrapper = result = new ReadOnlyCollectionDelegateWrapper<File, IFile>(() => Files);
                return result;
            }
        }

        public bool TryGetMD5Checksum(out MD5Checksum result) => MD5Checksum.TryCreate(Checksum, out result);
    }

    public partial class Comparison : IFileComparison
    {
        IFile IFileComparison.File1 => File1;

        IFile IFileComparison.File2 => File2;
    }

    public partial class File : IFile
    {
        private ReadOnlyCollectionDelegateWrapper<Comparison, IFileComparison> _comparisonsWrapper1;
        private ReadOnlyCollectionDelegateWrapper<Comparison, IFileComparison> _comparisonsWrapper2;

        IChecksumCalculation IFile.ChecksumCalculation => ChecksumCalculation;

        IReadOnlyCollection<IFileComparison> IFile.Comparisons1
        {
            get
            {
                ReadOnlyCollectionDelegateWrapper<Comparison, IFileComparison> result = _comparisonsWrapper1;
                if (result is null)
                    _comparisonsWrapper1 = result = new ReadOnlyCollectionDelegateWrapper<Comparison, IFileComparison>(() => Comparisons1);
                return result;
            }
        }

        IReadOnlyCollection<IFileComparison> IFile.Comparisons2
        {
            get
            {
                ReadOnlyCollectionDelegateWrapper<Comparison, IFileComparison> result = _comparisonsWrapper2;
                if (result is null)
                    _comparisonsWrapper2 = result = new ReadOnlyCollectionDelegateWrapper<Comparison, IFileComparison>(() => Comparisons2);
                return result;
            }
        }

        ISubDirectory IFile.ParentDirectory => ParentDirectory;
    }

    public partial class Subdirectory : ISubDirectory
    {
        private ReadOnlyCollectionDelegateWrapper<File, IFile> _filesWrapper;
        private ReadOnlyCollectionDelegateWrapper<Subdirectory, ISubDirectory> _subdirectoriesWrapper;
        IReadOnlyCollection<IFile> ISubDirectory.Files
        {
            get
            {
                ReadOnlyCollectionDelegateWrapper<File, IFile> result = _filesWrapper;
                if (result is null)
                    _filesWrapper = result = new ReadOnlyCollectionDelegateWrapper<File, IFile>(() => Files);
                return _filesWrapper;
            }
        }

        ISubDirectory ISubDirectory.ParentDirectory => ParentDirectory;

        IReadOnlyCollection<ISubDirectory> ISubDirectory.SubDirectories
        {
            get
            {
                ReadOnlyCollectionDelegateWrapper<Subdirectory, ISubDirectory> result = _subdirectoriesWrapper;
                if (result is null)
                    _subdirectoriesWrapper = result = new ReadOnlyCollectionDelegateWrapper<Subdirectory, ISubDirectory>(() => Subdirectories);
                return _subdirectoriesWrapper;
            }
        }

        IVolume ISubDirectory.Volume => Volume;
    }

    public partial class Volume : IVolume
    {
        private ReadOnlyCollectionDelegateWrapper<Subdirectory, ISubDirectory> _subdirectoriesWrapper;
        IReadOnlyCollection<ISubDirectory> IVolume.SubDirectories
        {
            get
            {
                ReadOnlyCollectionDelegateWrapper<Subdirectory, ISubDirectory> result = _subdirectoriesWrapper;
                if (result is null)
                    _subdirectoriesWrapper = result = new ReadOnlyCollectionDelegateWrapper<Subdirectory, ISubDirectory>(() => Subdirectories);
                return _subdirectoriesWrapper;
            }
        }
    }
}
