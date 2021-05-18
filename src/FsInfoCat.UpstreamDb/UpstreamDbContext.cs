using FsInfoCat.Model;
using FsInfoCat.Model.Upstream;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Linq;

namespace FsInfoCat.UpstreamDb
{
    public class UpstreamDbContext : DbContext, IUpstreamDbContext
    {
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");
            modelBuilder.Entity<FileSystem>(FileSystem.BuildEntity);
            modelBuilder.Entity<SymbolicName>(SymbolicName.BuildEntity);
            modelBuilder.Entity<Volume>(Volume.BuildEntity);
            modelBuilder.Entity<FsDirectory>(FsDirectory.BuildEntity);
            modelBuilder.Entity<HashCalculation>(HashCalculation.BuildEntity);
            modelBuilder.Entity<FsFile>(FsFile.BuildEntity);
            modelBuilder.Entity<FileComparison>(FileComparison.BuildEntity);
            modelBuilder.Entity<HostDevice>(HostDevice.BuildEntity);
            modelBuilder.Entity<HostPlatform>(HostPlatform.BuildEntity);
            modelBuilder.Entity<Redundancy>(Redundancy.BuildEntity);
            modelBuilder.Entity<FileRelocateTask>(FileRelocateTask.BuildEntity);
            modelBuilder.Entity<DirectoryRelocateTask>(DirectoryRelocateTask.BuildEntity);
            modelBuilder.Entity<UserProfile>(UserProfile.BuildEntity);
            modelBuilder.Entity<UserGroup>(UserGroup.BuildEntity);
            modelBuilder.Entity<UserGroupMembership>(UserGroupMembership.BuildEntity);
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<HashCalculation> Checksums { get; set; }

        public DbSet<FileComparison> Comparisons { get; set; }

        public DbSet<FsFile> Files { get; set; }

        public DbSet<FsDirectory> Subdirectories { get; set; }

        public DbSet<Volume> Volumes { get; set; }

        public DbSet<SymbolicName> FsSymbolicNames { get; set; }

        public DbSet<FileSystem> FileSystems { get; set; }

        public DbSet<HostDevice> HostDevices { get; set; }

        public DbSet<HostPlatform> HostPlatforms { get; set; }

        public DbSet<Redundancy> Redundancies { get; set; }

        public DbSet<FileRelocateTask> FileRelocateTasks { get; set; }

        public DbSet<DirectoryRelocateTask> DirectoryRelocateTasks { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<UserGroup> UserGroups { get; set; }

        public DbSet<UserGroupMembership> UserGroupMemberships { get; set; }

        #region Explicit Members

        IQueryable<IHashCalculation> IDbContext.HashCalculations => Checksums;

        IQueryable<IUpstreamHashCalculation> IUpstreamDbContext.HashCalculations => Checksums;

        IQueryable<IFileComparison> IDbContext.Comparisons => Comparisons;

        IQueryable<IUpstreamFileComparison> IUpstreamDbContext.Comparisons => Comparisons;

        IQueryable<IFile> IDbContext.Files => Files;

        IQueryable<IUpstreamFile> IUpstreamDbContext.Files => Files;

        IQueryable<ISubDirectory> IDbContext.Subdirectories => Subdirectories;

        IQueryable<IUpstreamSubDirectory> IUpstreamDbContext.Subdirectories => Subdirectories;

        IQueryable<IVolume> IDbContext.Volumes => Volumes;

        IQueryable<IUpstreamVolume> IUpstreamDbContext.Volumes => Volumes;

        IQueryable<IFsSymbolicName> IDbContext.SymbolicNames => FsSymbolicNames;

        IQueryable<IUpstreamSymbolicName> IUpstreamDbContext.SymbolicNames => FsSymbolicNames;

        IQueryable<IFileSystem> IDbContext.FileSystems => FileSystems;

        IQueryable<IUpstreamFileSystem> IUpstreamDbContext.FileSystems => FileSystems;

        IQueryable<IHostDevice> IUpstreamDbContext.HostDevices => HostDevices;

        IQueryable<IHostPlatform> IUpstreamDbContext.HostPlatforms => HostPlatforms;

        IQueryable<IRedundancy> IDbContext.Redundancies => Redundancies;

        IQueryable<IUpstreamRedundancy> IUpstreamDbContext.Redundancies => Redundancies;

        IQueryable<IFileRelocateTask> IUpstreamDbContext.FileRelocateTasks => FileRelocateTasks;

        IQueryable<IDirectoryRelocateTask> IUpstreamDbContext.DirectoryRelocateTasks => DirectoryRelocateTasks;

        IQueryable<IUserProfile> IUpstreamDbContext.UserProfiles => UserProfiles;

        IQueryable<IUserGroup> IUpstreamDbContext.UserGroups => UserGroups;

        IQueryable<IUserGroupMembership> IUpstreamDbContext.UserGroupMemberships => throw new NotImplementedException();

        #endregion

        public IDbContextTransaction BeginTransaction() => Database.BeginTransaction();

        public IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel) => Database.BeginTransaction(isolationLevel);

        internal EntityEntry<HashCalculation> AddHashCalculation(HashCalculation hashCalculation)
        {
            Checksums.Attach(hashCalculation ?? throw new ArgumentNullException(nameof(hashCalculation)));
            return Checksums.Add(hashCalculation);
        }

        void IUpstreamDbContext.AddHashCalculation(IUpstreamHashCalculation hashCalculation) => AddHashCalculation((HashCalculation)hashCalculation);

        internal EntityEntry<HashCalculation> UpdateHashCalculation(HashCalculation hashCalculation)
        {
            Checksums.Attach(hashCalculation ?? throw new ArgumentNullException(nameof(hashCalculation)));
            return Checksums.Update(hashCalculation);
        }

        void IUpstreamDbContext.UpdateHashCalculation(IUpstreamHashCalculation hashCalculation) => UpdateHashCalculation((HashCalculation)hashCalculation);

        internal EntityEntry<HashCalculation> RemoveHashCalculation(HashCalculation hashCalculation)
        {
            Checksums.Attach(hashCalculation ?? throw new ArgumentNullException(nameof(hashCalculation)));
            return Checksums.Remove(hashCalculation);
        }

        void IUpstreamDbContext.RemoveHashCalculation(IUpstreamHashCalculation hashCalculation) => RemoveHashCalculation((HashCalculation)hashCalculation);

        internal EntityEntry<FileComparison> AddComparison(FileComparison fileComparison)
        {
            Comparisons.Attach(fileComparison ?? throw new ArgumentNullException(nameof(fileComparison)));
            return Comparisons.Add(fileComparison);
        }

        void IUpstreamDbContext.AddComparison(IUpstreamFileComparison fileComparison) => AddComparison((FileComparison)fileComparison);

        internal EntityEntry<FileComparison> UpdateComparison(FileComparison fileComparison)
        {
            Comparisons.Attach(fileComparison ?? throw new ArgumentNullException(nameof(fileComparison)));
            return Comparisons.Update(fileComparison);
        }

        void IUpstreamDbContext.UpdateComparison(IUpstreamFileComparison fileComparison) => UpdateComparison((FileComparison)fileComparison);

        internal EntityEntry<FileComparison> RemoveComparison(FileComparison fileComparison)
        {
            Comparisons.Attach(fileComparison ?? throw new ArgumentNullException(nameof(fileComparison)));
            return Comparisons.Remove(fileComparison);
        }

        void IUpstreamDbContext.RemoveComparison(IUpstreamFileComparison fileComparison) => RemoveComparison((FileComparison)fileComparison);

        internal EntityEntry<FsFile> AddFile(FsFile file)
        {
            Files.Attach(file ?? throw new ArgumentNullException(nameof(file)));
            return Files.Add(file);
        }

        void IUpstreamDbContext.AddFile(IUpstreamFile file) => AddFile((FsFile)file);

        internal EntityEntry<FsFile> UpdateFile(FsFile file)
        {
            Files.Attach(file ?? throw new ArgumentNullException(nameof(file)));
            return Files.Update(file);
        }

        void IUpstreamDbContext.UpdateFile(IUpstreamFile file) => UpdateFile((FsFile)file);

        internal EntityEntry<FsFile> RemoveFile(FsFile file)
        {
            Files.Attach(file ?? throw new ArgumentNullException(nameof(file)));
            return Files.Remove(file);
        }

        void IUpstreamDbContext.RemoveFile(IUpstreamFile file) => RemoveFile((FsFile)file);

        internal EntityEntry<FsDirectory> AddSubDirectory(FsDirectory subDirectory)
        {
            Subdirectories.Attach(subDirectory ?? throw new ArgumentNullException(nameof(subDirectory)));
            return Subdirectories.Add(subDirectory);
        }

        void IUpstreamDbContext.AddSubDirectory(IUpstreamSubDirectory subDirectory) => AddSubDirectory((FsDirectory)subDirectory);

        internal EntityEntry<FsDirectory> UpdateSubDirectory(FsDirectory subDirectory)
        {
            Subdirectories.Attach(subDirectory ?? throw new ArgumentNullException(nameof(subDirectory)));
            return Subdirectories.Update(subDirectory);
        }

        void IUpstreamDbContext.UpdateSubDirectory(IUpstreamSubDirectory subDirectory) => UpdateSubDirectory((FsDirectory)subDirectory);

        internal EntityEntry<FsDirectory> RemoveSubDirectory(FsDirectory subDirectory)
        {
            Subdirectories.Attach(subDirectory ?? throw new ArgumentNullException(nameof(subDirectory)));
            return Subdirectories.Remove(subDirectory);
        }

        void IUpstreamDbContext.RemoveSubDirectory(IUpstreamSubDirectory subDirectory) => RemoveSubDirectory((FsDirectory)subDirectory);

        internal EntityEntry<Volume> AddVolume(Volume volume)
        {
            Volumes.Attach(volume ?? throw new ArgumentNullException(nameof(volume)));
            return Volumes.Add(volume);
        }

        void IUpstreamDbContext.AddVolume(IUpstreamVolume volume) => AddVolume((Volume)volume);

        internal EntityEntry<Volume> UpdateVolume(Volume volume)
        {
            Volumes.Attach(volume ?? throw new ArgumentNullException(nameof(volume)));
            return Volumes.Update(volume);
        }

        void IUpstreamDbContext.UpdateVolume(IUpstreamVolume volume) => UpdateVolume((Volume)volume);

        internal EntityEntry<Volume> RemoveVolume(Volume volume)
        {
            Volumes.Attach(volume ?? throw new ArgumentNullException(nameof(volume)));
            return Volumes.Remove(volume);
        }

        void IUpstreamDbContext.RemoveVolume(IUpstreamVolume volume) => RemoveVolume((Volume)volume);

        internal EntityEntry<SymbolicName> AddSymbolicName(SymbolicName symbolicName)
        {
            FsSymbolicNames.Attach(symbolicName ?? throw new ArgumentNullException(nameof(symbolicName)));
            return FsSymbolicNames.Add(symbolicName);
        }

        void IUpstreamDbContext.AddSymbolicName(IUpstreamSymbolicName symbolicName) => AddSymbolicName((SymbolicName)symbolicName);

        internal EntityEntry<SymbolicName> UpdateSymbolicName(SymbolicName symbolicName)
        {
            FsSymbolicNames.Attach(symbolicName ?? throw new ArgumentNullException(nameof(symbolicName)));
            return FsSymbolicNames.Update(symbolicName);
        }

        void IUpstreamDbContext.UpdateSymbolicName(IUpstreamSymbolicName symbolicName) => UpdateSymbolicName((SymbolicName)symbolicName);

        internal EntityEntry<SymbolicName> RemoveSymbolicName(SymbolicName symbolicName)
        {
            FsSymbolicNames.Attach(symbolicName ?? throw new ArgumentNullException(nameof(symbolicName)));
            return FsSymbolicNames.Remove(symbolicName);
        }

        void IUpstreamDbContext.RemoveSymbolicName(IUpstreamSymbolicName symbolicName) => RemoveSymbolicName((SymbolicName)symbolicName);

        internal EntityEntry<FileSystem> AddFileSystem(FileSystem fileSystem)
        {
            FileSystems.Attach(fileSystem ?? throw new ArgumentNullException(nameof(fileSystem)));
            return FileSystems.Add(fileSystem);
        }

        void IUpstreamDbContext.AddFileSystem(IUpstreamFileSystem fileSystem) => AddFileSystem((FileSystem)fileSystem);

        internal EntityEntry<FileSystem> UpdateFileSystem(FileSystem fileSystem)
        {
            FileSystems.Attach(fileSystem ?? throw new ArgumentNullException(nameof(fileSystem)));
            return FileSystems.Update(fileSystem);
        }

        void IUpstreamDbContext.UpdateFileSystem(IUpstreamFileSystem fileSystem) => UpdateFileSystem((FileSystem)fileSystem);

        internal EntityEntry<FileSystem> RemoveFileSystem(FileSystem fileSystem)
        {
            FileSystems.Attach(fileSystem ?? throw new ArgumentNullException(nameof(fileSystem)));
            return FileSystems.Remove(fileSystem);
        }

        void IUpstreamDbContext.RemoveFileSystem(IUpstreamFileSystem fileSystem) => RemoveFileSystem((FileSystem)fileSystem);

        internal EntityEntry<HostDevice> AddHostDevice(HostDevice hostDevice)
        {
            HostDevices.Attach(hostDevice ?? throw new ArgumentNullException(nameof(hostDevice)));
            return HostDevices.Add(hostDevice);
        }

        void IUpstreamDbContext.AddHostDevice(IHostDevice hostDevice) => AddHostDevice((HostDevice)hostDevice);

        internal EntityEntry<HostDevice> UpdateHostDevice(HostDevice hostDevice)
        {
            HostDevices.Attach(hostDevice ?? throw new ArgumentNullException(nameof(hostDevice)));
            return HostDevices.Update(hostDevice);
        }

        void IUpstreamDbContext.UpdateHostDevice(IHostDevice hostDevice) => UpdateHostDevice((HostDevice)hostDevice);

        internal EntityEntry<HostDevice> RemoveHostDevice(HostDevice hostDevice)
        {
            HostDevices.Attach(hostDevice ?? throw new ArgumentNullException(nameof(hostDevice)));
            return HostDevices.Remove(hostDevice);
        }

        void IUpstreamDbContext.RemoveHostDevice(IHostDevice hostDevice) => RemoveHostDevice((HostDevice)hostDevice);

        internal EntityEntry<HostPlatform> AddHostPlatform(HostPlatform hostPlatform)
        {
            HostPlatforms.Attach(hostPlatform ?? throw new ArgumentNullException(nameof(hostPlatform)));
            return HostPlatforms.Add(hostPlatform);
        }

        void IUpstreamDbContext.AddHostPlatform(IHostPlatform hostPlatform) => AddHostPlatform((HostPlatform)hostPlatform);

        internal EntityEntry<HostPlatform> UpdateHostPlatform(HostPlatform hostPlatform)
        {
            HostPlatforms.Attach(hostPlatform ?? throw new ArgumentNullException(nameof(hostPlatform)));
            return HostPlatforms.Update(hostPlatform);
        }

        void IUpstreamDbContext.UpdateHostPlatform(IHostPlatform hostPlatform) => UpdateHostPlatform((HostPlatform)hostPlatform);

        internal EntityEntry<HostPlatform> RemoveHostPlatform(HostPlatform hostPlatform)
        {
            HostPlatforms.Attach(hostPlatform ?? throw new ArgumentNullException(nameof(hostPlatform)));
            return HostPlatforms.Remove(hostPlatform);
        }

        void IUpstreamDbContext.RemoveHostPlatform(IHostPlatform hostPlatform) => AddHostPlatform((HostPlatform)hostPlatform);

        internal EntityEntry<Redundancy> AddRedundancy(Redundancy redundancy)
        {
            Redundancies.Attach(redundancy ?? throw new ArgumentNullException(nameof(redundancy)));
            return Redundancies.Add(redundancy);
        }

        void IUpstreamDbContext.AddRedundancy(IUpstreamRedundancy redundancy) => AddRedundancy((Redundancy)redundancy);

        internal EntityEntry<Redundancy> UpdateRedundancy(Redundancy redundancy)
        {
            Redundancies.Attach(redundancy ?? throw new ArgumentNullException(nameof(redundancy)));
            return Redundancies.Update(redundancy);
        }

        void IUpstreamDbContext.UpdateRedundancy(IUpstreamRedundancy redundancy) => UpdateRedundancy((Redundancy)redundancy);

        internal EntityEntry<Redundancy> RemoveRedundancy(Redundancy redundancy)
        {
            Redundancies.Attach(redundancy ?? throw new ArgumentNullException(nameof(redundancy)));
            return Redundancies.Remove(redundancy);
        }

        void IUpstreamDbContext.RemoveRedundancy(IUpstreamRedundancy redundancy) => RemoveRedundancy((Redundancy)redundancy);

        internal EntityEntry<FileRelocateTask> AddFileRelocateTask(FileRelocateTask fileRelocateTask)
        {
            FileRelocateTasks.Attach(fileRelocateTask ?? throw new ArgumentNullException(nameof(fileRelocateTask)));
            return FileRelocateTasks.Add(fileRelocateTask);
        }

        void IUpstreamDbContext.AddFileRelocateTask(IFileRelocateTask fileRelocateTask) => AddFileRelocateTask((FileRelocateTask)fileRelocateTask);

        internal EntityEntry<FileRelocateTask> UpdateFileRelocateTask(FileRelocateTask fileRelocateTask)
        {
            FileRelocateTasks.Attach(fileRelocateTask ?? throw new ArgumentNullException(nameof(fileRelocateTask)));
            return FileRelocateTasks.Update(fileRelocateTask);
        }

        void IUpstreamDbContext.UpdateFileRelocateTask(IFileRelocateTask fileRelocateTask) => UpdateFileRelocateTask((FileRelocateTask)fileRelocateTask);

        internal EntityEntry<FileRelocateTask> RemoveFileRelocateTask(FileRelocateTask fileRelocateTask)
        {
            FileRelocateTasks.Attach(fileRelocateTask ?? throw new ArgumentNullException(nameof(fileRelocateTask)));
            return FileRelocateTasks.Remove(fileRelocateTask);
        }

        void IUpstreamDbContext.RemoveFileRelocateTask(IFileRelocateTask fileRelocateTask) => RemoveFileRelocateTask((FileRelocateTask)fileRelocateTask);

        internal EntityEntry<DirectoryRelocateTask> AddDirectoryRelocateTask(DirectoryRelocateTask directoryRelocateTask)
        {
            DirectoryRelocateTasks.Attach(directoryRelocateTask ?? throw new ArgumentNullException(nameof(directoryRelocateTask)));
            return DirectoryRelocateTasks.Add(directoryRelocateTask);
        }

        void IUpstreamDbContext.AddDirectoryRelocateTask(IDirectoryRelocateTask directoryRelocateTask) => AddDirectoryRelocateTask((DirectoryRelocateTask)directoryRelocateTask);

        internal EntityEntry<DirectoryRelocateTask> UpdateDirectoryRelocateTask(DirectoryRelocateTask directoryRelocateTask)
        {
            DirectoryRelocateTasks.Attach(directoryRelocateTask ?? throw new ArgumentNullException(nameof(directoryRelocateTask)));
            return DirectoryRelocateTasks.Update(directoryRelocateTask);
        }

        void IUpstreamDbContext.UpdateDirectoryRelocateTask(IDirectoryRelocateTask directoryRelocateTask) => UpdateDirectoryRelocateTask((DirectoryRelocateTask)directoryRelocateTask);

        internal EntityEntry<DirectoryRelocateTask> RemoveDirectoryRelocateTask(DirectoryRelocateTask directoryRelocateTask)
        {
            DirectoryRelocateTasks.Attach(directoryRelocateTask ?? throw new ArgumentNullException(nameof(directoryRelocateTask)));
            return DirectoryRelocateTasks.Remove(directoryRelocateTask);
        }

        void IUpstreamDbContext.RemoveDirectoryRelocateTask(IDirectoryRelocateTask directoryRelocateTask) => RemoveDirectoryRelocateTask((DirectoryRelocateTask)directoryRelocateTask);

        internal EntityEntry<UserProfile> AddUserProfile(UserProfile userProfile)
        {
            UserProfiles.Attach(userProfile ?? throw new ArgumentNullException(nameof(userProfile)));
            return UserProfiles.Add(userProfile);
        }

        void IUpstreamDbContext.AddUserProfile(IUserProfile userProfile) => AddUserProfile((UserProfile)userProfile);

        internal EntityEntry<UserProfile> UpdateUserProfile(UserProfile userProfile)
        {
            UserProfiles.Attach(userProfile ?? throw new ArgumentNullException(nameof(userProfile)));
            return UserProfiles.Update(userProfile);
        }

        void IUpstreamDbContext.UpdateUserProfile(IUserProfile userProfile) => UpdateUserProfile((UserProfile)userProfile);

        internal EntityEntry<UserProfile> RemoveUserProfile(UserProfile userProfile)
        {
            UserProfiles.Attach(userProfile ?? throw new ArgumentNullException(nameof(userProfile)));
            return UserProfiles.Remove(userProfile);
        }

        void IUpstreamDbContext.RemoveUserProfile(IUserProfile userProfile) => RemoveUserProfile((UserProfile)userProfile);

        internal EntityEntry<UserGroup> AddUserGroup(UserGroup userGroup)
        {
            UserGroups.Attach(userGroup ?? throw new ArgumentNullException(nameof(userGroup)));
            return UserGroups.Add(userGroup);
        }

        void IUpstreamDbContext.AddUserGroup(IUserGroup userGroup) => AddUserGroup((UserGroup)userGroup);

        internal EntityEntry<UserGroup> UpdateUserGroup(UserGroup userGroup)
        {
            UserGroups.Attach(userGroup ?? throw new ArgumentNullException(nameof(userGroup)));
            return UserGroups.Update(userGroup);
        }

        void IUpstreamDbContext.UpdateUserGroup(IUserGroup userGroup) => UpdateUserGroup((UserGroup)userGroup);

        internal EntityEntry<UserGroup> RemoveUserGroup(UserGroup userGroup)
        {
            UserGroups.Attach(userGroup ?? throw new ArgumentNullException(nameof(userGroup)));
            return UserGroups.Remove(userGroup);
        }

        void IUpstreamDbContext.RemoveUserGroup(IUserGroup userGroup) => RemoveUserGroup((UserGroup)userGroup);

        public bool HasChanges() => ChangeTracker.HasChanges();
    }
}
