namespace FsInfoCat.RemoteDb
{
    internal class RemoteDbService
    {
        internal HostDevice NewHostDevice() => new HostDevice();
        internal HostPlatform NewHostPlatform() => new HostPlatform();
        internal Redundancy NewRedundancy() => new Redundancy();
        internal FileRelocateTask NewFileRelocateTask() => new FileRelocateTask();
        internal DirectoryRelocateTask NewDirectoryRelocateTask() => new DirectoryRelocateTask();
        internal UserProfile NewUserProfile() => new UserProfile();
        internal UserGroup NewUserGroup() => new UserGroup();
    }
}
