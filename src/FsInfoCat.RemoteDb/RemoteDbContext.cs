using FsInfoCat.Model;
using FsInfoCat.Model.Remote;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Linq;

namespace FsInfoCat.RemoteDb
{
    public class RemoteDbContext : DbContext, IRemoteDbContext
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

        IQueryable<IRemoteHashCalculation> IRemoteDbContext.HashCalculations => Checksums;

        IQueryable<IFileComparison> IDbContext.Comparisons => Comparisons;

        IQueryable<IRemoteFileComparison> IRemoteDbContext.Comparisons => Comparisons;

        IQueryable<IFile> IDbContext.Files => Files;

        IQueryable<IRemoteFile> IRemoteDbContext.Files => Files;

        IQueryable<ISubDirectory> IDbContext.Subdirectories => Subdirectories;

        IQueryable<IRemoteSubDirectory> IRemoteDbContext.Subdirectories => Subdirectories;

        IQueryable<IVolume> IDbContext.Volumes => Volumes;

        IQueryable<IRemoteVolume> IRemoteDbContext.Volumes => Volumes;

        IQueryable<IFsSymbolicName> IDbContext.SymbolicNames => FsSymbolicNames;

        IQueryable<IRemoteSymbolicName> IRemoteDbContext.SymbolicNames => FsSymbolicNames;

        IQueryable<IFileSystem> IDbContext.FileSystems => FileSystems;

        IQueryable<IRemoteFileSystem> IRemoteDbContext.FileSystems => FileSystems;

        IQueryable<IHostDevice> IRemoteDbContext.HostDevices => HostDevices;

        IQueryable<IHostPlatform> IRemoteDbContext.HostPlatforms => HostPlatforms;

        IQueryable<IRedundancy> IDbContext.Redundancies => Redundancies;

        IQueryable<IRemoteRedundancy> IRemoteDbContext.Redundancies => Redundancies;

        IQueryable<IFileRelocateTask> IRemoteDbContext.FileRelocateTasks => FileRelocateTasks;

        IQueryable<IDirectoryRelocateTask> IRemoteDbContext.DirectoryRelocateTasks => DirectoryRelocateTasks;

        IQueryable<IUserProfile> IRemoteDbContext.UserProfiles => UserProfiles;

        IQueryable<IUserGroup> IRemoteDbContext.UserGroups => UserGroups;

        IQueryable<IUserGroupMembership> IRemoteDbContext.UserGroupMemberships => throw new NotImplementedException();

        #endregion

        public IDbContextTransaction BeginTransaction() => Database.BeginTransaction();

        public IDbContextTransaction BeginTransaction(IsolationLevel isolationLevel) => Database.BeginTransaction(isolationLevel);

        internal EntityEntry<HashCalculation> AddHashCalculation(HashCalculation hashCalculation)
        {
            Checksums.Attach(hashCalculation ?? throw new ArgumentNullException(nameof(hashCalculation)));
            return Checksums.Add(hashCalculation);
        }

        void IRemoteDbContext.AddHashCalculation(IRemoteHashCalculation hashCalculation) => AddHashCalculation((HashCalculation)hashCalculation);

        internal EntityEntry<HashCalculation> UpdateHashCalculation(HashCalculation hashCalculation)
        {
            Checksums.Attach(hashCalculation ?? throw new ArgumentNullException(nameof(hashCalculation)));
            return Checksums.Update(hashCalculation);
        }

        void IRemoteDbContext.UpdateHashCalculation(IRemoteHashCalculation hashCalculation) => UpdateHashCalculation((HashCalculation)hashCalculation);

        internal EntityEntry<HashCalculation> RemoveHashCalculation(HashCalculation hashCalculation)
        {
            Checksums.Attach(hashCalculation ?? throw new ArgumentNullException(nameof(hashCalculation)));
            return Checksums.Remove(hashCalculation);
        }

        void IRemoteDbContext.RemoveHashCalculation(IRemoteHashCalculation hashCalculation) => RemoveHashCalculation((HashCalculation)hashCalculation);

        internal EntityEntry<FileComparison> AddComparison(FileComparison fileComparison)
        {
            Comparisons.Attach(fileComparison ?? throw new ArgumentNullException(nameof(fileComparison)));
            return Comparisons.Add(fileComparison);
        }

        void IRemoteDbContext.AddComparison(IRemoteFileComparison fileComparison) => AddComparison((FileComparison)fileComparison);

        internal EntityEntry<FileComparison> UpdateComparison(FileComparison fileComparison)
        {
            Comparisons.Attach(fileComparison ?? throw new ArgumentNullException(nameof(fileComparison)));
            return Comparisons.Update(fileComparison);
        }

        void IRemoteDbContext.UpdateComparison(IRemoteFileComparison fileComparison) => UpdateComparison((FileComparison)fileComparison);

        internal EntityEntry<FileComparison> RemoveComparison(FileComparison fileComparison)
        {
            Comparisons.Attach(fileComparison ?? throw new ArgumentNullException(nameof(fileComparison)));
            return Comparisons.Remove(fileComparison);
        }

        void IRemoteDbContext.RemoveComparison(IRemoteFileComparison fileComparison) => RemoveComparison((FileComparison)fileComparison);

        internal EntityEntry<FsFile> AddFile(FsFile file)
        {
            Files.Attach(file ?? throw new ArgumentNullException(nameof(file)));
            return Files.Add(file);
        }

        void IRemoteDbContext.AddFile(IRemoteFile file) => AddFile((FsFile)file);

        internal EntityEntry<FsFile> UpdateFile(FsFile file)
        {
            Files.Attach(file ?? throw new ArgumentNullException(nameof(file)));
            return Files.Update(file);
        }

        void IRemoteDbContext.UpdateFile(IRemoteFile file) => UpdateFile((FsFile)file);

        internal EntityEntry<FsFile> RemoveFile(FsFile file)
        {
            Files.Attach(file ?? throw new ArgumentNullException(nameof(file)));
            return Files.Remove(file);
        }

        void IRemoteDbContext.RemoveFile(IRemoteFile file) => RemoveFile((FsFile)file);

        internal EntityEntry<FsDirectory> AddSubDirectory(FsDirectory subDirectory)
        {
            Subdirectories.Attach(subDirectory ?? throw new ArgumentNullException(nameof(subDirectory)));
            return Subdirectories.Add(subDirectory);
        }

        void IRemoteDbContext.AddSubDirectory(IRemoteSubDirectory subDirectory) => AddSubDirectory((FsDirectory)subDirectory);

        internal EntityEntry<FsDirectory> UpdateSubDirectory(FsDirectory subDirectory)
        {
            Subdirectories.Attach(subDirectory ?? throw new ArgumentNullException(nameof(subDirectory)));
            return Subdirectories.Update(subDirectory);
        }

        void IRemoteDbContext.UpdateSubDirectory(IRemoteSubDirectory subDirectory) => UpdateSubDirectory((FsDirectory)subDirectory);

        internal EntityEntry<FsDirectory> RemoveSubDirectory(FsDirectory subDirectory)
        {
            Subdirectories.Attach(subDirectory ?? throw new ArgumentNullException(nameof(subDirectory)));
            return Subdirectories.Remove(subDirectory);
        }

        void IRemoteDbContext.RemoveSubDirectory(IRemoteSubDirectory subDirectory) => RemoveSubDirectory((FsDirectory)subDirectory);

        internal EntityEntry<Volume> AddVolume(Volume volume)
        {
            Volumes.Attach(volume ?? throw new ArgumentNullException(nameof(volume)));
            return Volumes.Add(volume);
        }

        void IRemoteDbContext.AddVolume(IRemoteVolume volume) => AddVolume((Volume)volume);

        internal EntityEntry<Volume> UpdateVolume(Volume volume)
        {
            Volumes.Attach(volume ?? throw new ArgumentNullException(nameof(volume)));
            return Volumes.Update(volume);
        }

        void IRemoteDbContext.UpdateVolume(IRemoteVolume volume) => UpdateVolume((Volume)volume);

        internal EntityEntry<Volume> RemoveVolume(Volume volume)
        {
            Volumes.Attach(volume ?? throw new ArgumentNullException(nameof(volume)));
            return Volumes.Remove(volume);
        }

        void IRemoteDbContext.RemoveVolume(IRemoteVolume volume) => RemoveVolume((Volume)volume);

        internal EntityEntry<SymbolicName> AddSymbolicName(SymbolicName symbolicName)
        {
            FsSymbolicNames.Attach(symbolicName ?? throw new ArgumentNullException(nameof(symbolicName)));
            return FsSymbolicNames.Add(symbolicName);
        }

        void IRemoteDbContext.AddSymbolicName(IRemoteSymbolicName symbolicName) => AddSymbolicName((SymbolicName)symbolicName);

        internal EntityEntry<SymbolicName> UpdateSymbolicName(SymbolicName symbolicName)
        {
            FsSymbolicNames.Attach(symbolicName ?? throw new ArgumentNullException(nameof(symbolicName)));
            return FsSymbolicNames.Update(symbolicName);
        }

        void IRemoteDbContext.UpdateSymbolicName(IRemoteSymbolicName symbolicName) => UpdateSymbolicName((SymbolicName)symbolicName);

        internal EntityEntry<SymbolicName> RemoveSymbolicName(SymbolicName symbolicName)
        {
            FsSymbolicNames.Attach(symbolicName ?? throw new ArgumentNullException(nameof(symbolicName)));
            return FsSymbolicNames.Remove(symbolicName);
        }

        void IRemoteDbContext.RemoveSymbolicName(IRemoteSymbolicName symbolicName) => RemoveSymbolicName((SymbolicName)symbolicName);

        internal EntityEntry<FileSystem> AddFileSystem(FileSystem fileSystem)
        {
            FileSystems.Attach(fileSystem ?? throw new ArgumentNullException(nameof(fileSystem)));
            return FileSystems.Add(fileSystem);
        }

        void IRemoteDbContext.AddFileSystem(IRemoteFileSystem fileSystem) => AddFileSystem((FileSystem)fileSystem);

        internal EntityEntry<FileSystem> UpdateFileSystem(FileSystem fileSystem)
        {
            FileSystems.Attach(fileSystem ?? throw new ArgumentNullException(nameof(fileSystem)));
            return FileSystems.Update(fileSystem);
        }

        void IRemoteDbContext.UpdateFileSystem(IRemoteFileSystem fileSystem) => UpdateFileSystem((FileSystem)fileSystem);

        internal EntityEntry<FileSystem> RemoveFileSystem(FileSystem fileSystem)
        {
            FileSystems.Attach(fileSystem ?? throw new ArgumentNullException(nameof(fileSystem)));
            return FileSystems.Remove(fileSystem);
        }

        void IRemoteDbContext.RemoveFileSystem(IRemoteFileSystem fileSystem) => RemoveFileSystem((FileSystem)fileSystem);

        internal EntityEntry<HostDevice> AddHostDevice(HostDevice hostDevice)
        {
            HostDevices.Attach(hostDevice ?? throw new ArgumentNullException(nameof(hostDevice)));
            return HostDevices.Add(hostDevice);
        }

        void IRemoteDbContext.AddHostDevice(IHostDevice hostDevice) => AddHostDevice((HostDevice)hostDevice);

        internal EntityEntry<HostDevice> UpdateHostDevice(HostDevice hostDevice)
        {
            HostDevices.Attach(hostDevice ?? throw new ArgumentNullException(nameof(hostDevice)));
            return HostDevices.Update(hostDevice);
        }

        void IRemoteDbContext.UpdateHostDevice(IHostDevice hostDevice) => UpdateHostDevice((HostDevice)hostDevice);

        internal EntityEntry<HostDevice> RemoveHostDevice(HostDevice hostDevice)
        {
            HostDevices.Attach(hostDevice ?? throw new ArgumentNullException(nameof(hostDevice)));
            return HostDevices.Remove(hostDevice);
        }

        void IRemoteDbContext.RemoveHostDevice(IHostDevice hostDevice) => RemoveHostDevice((HostDevice)hostDevice);

        internal EntityEntry<HostPlatform> AddHostPlatform(HostPlatform hostPlatform)
        {
            HostPlatforms.Attach(hostPlatform ?? throw new ArgumentNullException(nameof(hostPlatform)));
            return HostPlatforms.Add(hostPlatform);
        }

        void IRemoteDbContext.AddHostPlatform(IHostPlatform hostPlatform) => AddHostPlatform((HostPlatform)hostPlatform);

        internal EntityEntry<HostPlatform> UpdateHostPlatform(HostPlatform hostPlatform)
        {
            HostPlatforms.Attach(hostPlatform ?? throw new ArgumentNullException(nameof(hostPlatform)));
            return HostPlatforms.Update(hostPlatform);
        }

        void IRemoteDbContext.UpdateHostPlatform(IHostPlatform hostPlatform) => UpdateHostPlatform((HostPlatform)hostPlatform);

        internal EntityEntry<HostPlatform> RemoveHostPlatform(HostPlatform hostPlatform)
        {
            HostPlatforms.Attach(hostPlatform ?? throw new ArgumentNullException(nameof(hostPlatform)));
            return HostPlatforms.Remove(hostPlatform);
        }

        void IRemoteDbContext.RemoveHostPlatform(IHostPlatform hostPlatform) => AddHostPlatform((HostPlatform)hostPlatform);

        internal EntityEntry<Redundancy> AddRedundancy(Redundancy redundancy)
        {
            Redundancies.Attach(redundancy ?? throw new ArgumentNullException(nameof(redundancy)));
            return Redundancies.Add(redundancy);
        }

        void IRemoteDbContext.AddRedundancy(IRemoteRedundancy redundancy) => AddRedundancy((Redundancy)redundancy);

        internal EntityEntry<Redundancy> UpdateRedundancy(Redundancy redundancy)
        {
            Redundancies.Attach(redundancy ?? throw new ArgumentNullException(nameof(redundancy)));
            return Redundancies.Update(redundancy);
        }

        void IRemoteDbContext.UpdateRedundancy(IRemoteRedundancy redundancy) => UpdateRedundancy((Redundancy)redundancy);

        internal EntityEntry<Redundancy> RemoveRedundancy(Redundancy redundancy)
        {
            Redundancies.Attach(redundancy ?? throw new ArgumentNullException(nameof(redundancy)));
            return Redundancies.Remove(redundancy);
        }

        void IRemoteDbContext.RemoveRedundancy(IRemoteRedundancy redundancy) => RemoveRedundancy((Redundancy)redundancy);

        internal EntityEntry<FileRelocateTask> AddFileRelocateTask(FileRelocateTask fileRelocateTask)
        {
            FileRelocateTasks.Attach(fileRelocateTask ?? throw new ArgumentNullException(nameof(fileRelocateTask)));
            return FileRelocateTasks.Add(fileRelocateTask);
        }

        void IRemoteDbContext.AddFileRelocateTask(IFileRelocateTask fileRelocateTask) => AddFileRelocateTask((FileRelocateTask)fileRelocateTask);

        internal EntityEntry<FileRelocateTask> UpdateFileRelocateTask(FileRelocateTask fileRelocateTask)
        {
            FileRelocateTasks.Attach(fileRelocateTask ?? throw new ArgumentNullException(nameof(fileRelocateTask)));
            return FileRelocateTasks.Update(fileRelocateTask);
        }

        void IRemoteDbContext.UpdateFileRelocateTask(IFileRelocateTask fileRelocateTask) => UpdateFileRelocateTask((FileRelocateTask)fileRelocateTask);

        internal EntityEntry<FileRelocateTask> RemoveFileRelocateTask(FileRelocateTask fileRelocateTask)
        {
            FileRelocateTasks.Attach(fileRelocateTask ?? throw new ArgumentNullException(nameof(fileRelocateTask)));
            return FileRelocateTasks.Remove(fileRelocateTask);
        }

        void IRemoteDbContext.RemoveFileRelocateTask(IFileRelocateTask fileRelocateTask) => RemoveFileRelocateTask((FileRelocateTask)fileRelocateTask);

        internal EntityEntry<DirectoryRelocateTask> AddDirectoryRelocateTask(DirectoryRelocateTask directoryRelocateTask)
        {
            DirectoryRelocateTasks.Attach(directoryRelocateTask ?? throw new ArgumentNullException(nameof(directoryRelocateTask)));
            return DirectoryRelocateTasks.Add(directoryRelocateTask);
        }

        void IRemoteDbContext.AddDirectoryRelocateTask(IDirectoryRelocateTask directoryRelocateTask) => AddDirectoryRelocateTask((DirectoryRelocateTask)directoryRelocateTask);

        internal EntityEntry<DirectoryRelocateTask> UpdateDirectoryRelocateTask(DirectoryRelocateTask directoryRelocateTask)
        {
            DirectoryRelocateTasks.Attach(directoryRelocateTask ?? throw new ArgumentNullException(nameof(directoryRelocateTask)));
            return DirectoryRelocateTasks.Update(directoryRelocateTask);
        }

        void IRemoteDbContext.UpdateDirectoryRelocateTask(IDirectoryRelocateTask directoryRelocateTask) => UpdateDirectoryRelocateTask((DirectoryRelocateTask)directoryRelocateTask);

        internal EntityEntry<DirectoryRelocateTask> RemoveDirectoryRelocateTask(DirectoryRelocateTask directoryRelocateTask)
        {
            DirectoryRelocateTasks.Attach(directoryRelocateTask ?? throw new ArgumentNullException(nameof(directoryRelocateTask)));
            return DirectoryRelocateTasks.Remove(directoryRelocateTask);
        }

        void IRemoteDbContext.RemoveDirectoryRelocateTask(IDirectoryRelocateTask directoryRelocateTask) => RemoveDirectoryRelocateTask((DirectoryRelocateTask)directoryRelocateTask);

        internal EntityEntry<UserProfile> AddUserProfile(UserProfile userProfile)
        {
            UserProfiles.Attach(userProfile ?? throw new ArgumentNullException(nameof(userProfile)));
            return UserProfiles.Add(userProfile);
        }

        void IRemoteDbContext.AddUserProfile(IUserProfile userProfile) => AddUserProfile((UserProfile)userProfile);

        internal EntityEntry<UserProfile> UpdateUserProfile(UserProfile userProfile)
        {
            UserProfiles.Attach(userProfile ?? throw new ArgumentNullException(nameof(userProfile)));
            return UserProfiles.Update(userProfile);
        }

        void IRemoteDbContext.UpdateUserProfile(IUserProfile userProfile) => UpdateUserProfile((UserProfile)userProfile);

        internal EntityEntry<UserProfile> RemoveUserProfile(UserProfile userProfile)
        {
            UserProfiles.Attach(userProfile ?? throw new ArgumentNullException(nameof(userProfile)));
            return UserProfiles.Remove(userProfile);
        }

        void IRemoteDbContext.RemoveUserProfile(IUserProfile userProfile) => RemoveUserProfile((UserProfile)userProfile);

        internal EntityEntry<UserGroup> AddUserGroup(UserGroup userGroup)
        {
            UserGroups.Attach(userGroup ?? throw new ArgumentNullException(nameof(userGroup)));
            return UserGroups.Add(userGroup);
        }

        void IRemoteDbContext.AddUserGroup(IUserGroup userGroup) => AddUserGroup((UserGroup)userGroup);

        internal EntityEntry<UserGroup> UpdateUserGroup(UserGroup userGroup)
        {
            UserGroups.Attach(userGroup ?? throw new ArgumentNullException(nameof(userGroup)));
            return UserGroups.Update(userGroup);
        }

        void IRemoteDbContext.UpdateUserGroup(IUserGroup userGroup) => UpdateUserGroup((UserGroup)userGroup);

        internal EntityEntry<UserGroup> RemoveUserGroup(UserGroup userGroup)
        {
            UserGroups.Attach(userGroup ?? throw new ArgumentNullException(nameof(userGroup)));
            return UserGroups.Remove(userGroup);
        }

        void IRemoteDbContext.RemoveUserGroup(IUserGroup userGroup) => RemoveUserGroup((UserGroup)userGroup);

        public bool HasChanges() => ChangeTracker.HasChanges();
    }
}
