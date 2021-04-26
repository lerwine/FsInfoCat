using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FsInfoCat.Desktop.Model.Remote
{
    public static class Extensions
    {
        public static UserRole AsNormalized(this UserRole userRole)
        {
            if (userRole.HasFlag(UserRole.SystemAdmin))
            {
                foreach (UserRole r in Enum.GetValues(typeof(UserRole)))
                    userRole |= r;
            }
            else if (userRole.HasFlag(UserRole.AppAdministrator))
            {
                foreach (UserRole r in Enum.GetValues(typeof(UserRole)))
                {
                    if (r != UserRole.SystemAdmin)
                        userRole |= r;
                }
            }
            else if (userRole.HasFlag(UserRole.ChangeAdministrator))
                return userRole | UserRole.Reader | UserRole.Contributor;
            else if (userRole.HasFlag(UserRole.Auditor) || userRole.HasFlag(UserRole.Contributor))
                return userRole | UserRole.Reader;
            return userRole;
        }

        public static UserRole GetEffectiveRoles(this UserProfile user)
        {
            if (user is null)
                return UserRole.None;
            if (user.AssignmentGroups is null)
                return user.ExplicitRoles;
            return AsNormalized(user.AssignmentGroups.Select(g => (g is null) ? UserRole.None : g.Roles).Aggregate(user.ExplicitRoles, (a, r) => a | r));
        }

        public static UserProfile GetUserById(this RemoteDbContainer dbContext, Guid id) => (dbContext is null) ? null : GetUserById(dbContext.UserProfiles, id);

        public static UserProfile GetUserById(this DbSet<UserProfile> dbSet, Guid id)
        {
            if (dbSet is null)
                return null;
            return (from u in dbSet where u.Id == id select u).FirstOrDefault();
        }

        public static UserProfile GetSystemAccount(this RemoteDbContainer dbContext)
        {
            Guid id = Guid.Empty;
            UserProfile userAccount = GetUserById(dbContext, id);
            if (userAccount is null)
                throw new InvalidOperationException("Could not find system account");
            return userAccount;
        }
    }

    public partial class RemoteDbContainer : IDbContext
    {
        internal RemoteDbContainer(string connectionString) : base(connectionString) { }

        IQueryable<IHashCalculation> IDbContext.Checksums => HashCalculations.Cast<IHashCalculation>();

        IQueryable<IFileComparison> IDbContext.Comparisons => Comparisons.Cast<IFileComparison>();

        IQueryable<IFile> IDbContext.Files => Files.Cast<IFile>();

        IQueryable<ISubDirectory> IDbContext.Subdirectories => Directories.Cast<ISubDirectory>();

        IQueryable<IVolume> IDbContext.Volumes => Volumes.Cast<IVolume>();
    }

    public partial class HashCalculation : IHashCalculation
    {
        private ReadOnlyCollectionDelegateWrapper<byte, byte> _checksumWrapper;
        private ReadOnlyCollectionDelegateWrapper<File, IFile> _filesWrapper;

        IReadOnlyCollection<byte> IHashCalculation.Data
        {
            get
            {
                ReadOnlyCollectionDelegateWrapper<byte, byte> result = _checksumWrapper;
                if (result is null)
                    _checksumWrapper = result = new ReadOnlyCollectionDelegateWrapper<byte, byte>(() => Data);
                return result;
            }
        }

        IReadOnlyCollection<IFile> IHashCalculation.Files
        {
            get
            {
                ReadOnlyCollectionDelegateWrapper<File, IFile> result = _filesWrapper;
                if (result is null)
                    _filesWrapper = result = new ReadOnlyCollectionDelegateWrapper<File, IFile>(() => Files);
                return result;
            }
        }

        public bool TryGetMD5Checksum(out UInt128 result) => UInt128.TryCreate(Data, out result);
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

        IHashCalculation IFile.HashCalculation => HashCalculation;

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

        ISubDirectory IFile.Parent => Parent;
    }

    public partial class Directory : ISubDirectory
    {
        private ReadOnlyCollectionDelegateWrapper<File, IFile> _filesWrapper;
        private ReadOnlyCollectionDelegateWrapper<Directory, ISubDirectory> _subdirectoriesWrapper;

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

        ISubDirectory ISubDirectory.Parent => Parent;

        IReadOnlyCollection<ISubDirectory> ISubDirectory.SubDirectories
        {
            get
            {
                ReadOnlyCollectionDelegateWrapper<Directory, ISubDirectory> result = _subdirectoriesWrapper;
                if (result is null)
                    _subdirectoriesWrapper = result = new ReadOnlyCollectionDelegateWrapper<Directory, ISubDirectory>(() => SubDirectories);
                return _subdirectoriesWrapper;
            }
        }

        IVolume ISubDirectory.Volume => Volume;
    }

    public partial class Volume : IVolume
    {
        private ReadOnlyCollectionDelegateWrapper<Directory, ISubDirectory> _subdirectoriesWrapper;

        ISubDirectory IVolume.RootDirectory => RootDirectory;

        IFileSystem IVolume.FileSystem => FileSystem;
    }

    public partial class FileSystem : IFileSystem
    {
        private ReadOnlyCollectionDelegateWrapper<Volume, IVolume> _volumesWrapper;
        private ReadOnlyCollectionDelegateWrapper<FsSymbolicName, IFsSymbolicName> _symbolicNamesWrapper;

        IReadOnlyCollection<IVolume> IFileSystem.Volumes
        {
            get
            {
                ReadOnlyCollectionDelegateWrapper<Volume, IVolume> result = _volumesWrapper;
                if (result is null)
                    _volumesWrapper = result = new ReadOnlyCollectionDelegateWrapper<Volume, IVolume>(() => Volumes);
                return _volumesWrapper;
            }
        }

        IReadOnlyCollection<IFsSymbolicName> IFileSystem.SymbolicNames
        {
            get
            {
                ReadOnlyCollectionDelegateWrapper<FsSymbolicName, IFsSymbolicName> result = _symbolicNamesWrapper;
                if (result is null)
                    _symbolicNamesWrapper = result = new ReadOnlyCollectionDelegateWrapper<FsSymbolicName, IFsSymbolicName>(() => SymbolicNames);
                return _symbolicNamesWrapper;
            }
        }

        IFsSymbolicName IFileSystem.DefaultSymbolicName => DefaultSymbolicName;
    }

    public partial class FsSymbolicName : IFsSymbolicName
    {

    }
}
