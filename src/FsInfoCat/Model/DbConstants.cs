namespace FsInfoCat.Model
{
    public static class DbConstants
    {
        public const string TableName_FsDirectory = "Directories";
        public const string TableName_FsFile = "Files";
        public const string TableName_FileComparison = "Comparisons";
        public const string ProviderFactories_AssemblyQualifiedName = "AssemblyQualifiedName";
        public const string ProviderFactories_InvariantName = "InvariantName";
        public const int DbColMaxLen_SimpleName = 128;
        public const int DbColMaxLen_FileSystemName = 1024;
        public const int DbColMaxLen_DisplayName = 256;
        public const int DbColMaxLen_VolumeName = 128;
        public const int DbColMaxLen_VolumeIdentifier = 256;
        public const int DbColMaxLen_ShortDescription = 1024;
        public const int DbColMaxLen_MachineIdentifer = 128;
        public const int DbColMaxLen_MachineName = 128;
        public const int DbColMaxLen_FirstName = 32;
        public const int DbColMaxLen_LastName = 64;
        public const int DbColMaxLen_MI = 1;
        public const int DbColMaxLen_Suffix = 32;
        public const int DbColMaxLen_Title = 32;
        public const int DbColMaxLen_SID = 85;
        public const int DbColMaxLen_LoginName = 128;
        public const int DefaultValue_MaxFileSystemNameLength = 255;
    }
}
