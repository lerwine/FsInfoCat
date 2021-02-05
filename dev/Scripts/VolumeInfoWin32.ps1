Add-Type -TypeDefinition @'
namespace FsInfoCat
{
    using System;
    /// <summary>
    /// Represents file system capabilities and attributes.
    /// </summary>
    [Flags]
    public enum FileSystemFeature : uint
    {
        /// <summary>
        /// The file system preserves the case of file names when it places a name on disk.
        /// </summary>
        CasePreservedNames = 2,

        /// <summary>
        /// The file system supports case-sensitive file names.
        /// </summary>
        CaseSensitiveSearch = 1,

        /// <summary>
        /// The specified volume is a direct access (DAX) volume. This flag was introduced in Windows 10, version 1607.
        /// </summary>
        DaxVolume = 0x20000000,

        /// <summary>
        /// The file system supports file-based compression.
        /// </summary>
        FileCompression = 0x10,

        /// <summary>
        /// The file system supports named streams.
        /// </summary>
        NamedStreams = 0x40000,

        /// <summary>
        /// The file system preserves and enforces access control lists (ACL).
        /// </summary>
        PersistentACLS = 8,

        /// <summary>
        /// The specified volume is read-only.
        /// </summary>
        ReadOnlyVolume = 0x80000,

        /// <summary>
        /// The volume supports a single sequential write.
        /// </summary>
        SequentialWriteOnce = 0x100000,

        /// <summary>
        /// The file system supports the Encrypted File System (EFS).
        /// </summary>
        SupportsEncryption = 0x20000,

        /// <summary>
        /// The specified volume supports extended attributes. An extended attribute is a piece of
        /// application-specific metadata that an application can associate with a file and is not part
        /// of the file's data.
        /// </summary>
        SupportsExtendedAttributes = 0x00800000,

        /// <summary>
        /// The specified volume supports hard links. For more information, see Hard Links and Junctions.
        /// </summary>
        SupportsHardLinks = 0x00400000,

        /// <summary>
        /// The file system supports object identifiers.
        /// </summary>
        SupportsObjectIDs = 0x10000,

        /// <summary>
        /// The file system supports open by FileID. For more information, see FILE_ID_BOTH_DIR_INFO.
        /// </summary>
        SupportsOpenByFileId = 0x01000000,

        /// <summary>
        /// The file system supports re-parse points.
        /// </summary>
        SupportsReparsePoints = 0x80,

        /// <summary>
        /// The file system supports sparse files.
        /// </summary>
        SupportsSparseFiles = 0x40,

        /// <summary>
        /// The volume supports transactions.
        /// </summary>
        SupportsTransactions = 0x200000,

        /// <summary>
        /// The specified volume supports update sequence number (USN) journals. For more information,
        /// see Change Journal Records.
        /// </summary>
        SupportsUsnJournal = 0x02000000,

        /// <summary>
        /// The file system supports Unicode in file names as they appear on disk.
        /// </summary>
        UnicodeOnDisk = 4,

        /// <summary>
        /// The specified volume is a compressed volume, for example, a DoubleSpace volume.
        /// </summary>
        VolumeIsCompressed = 0x8000,

        /// <summary>
        /// The file system supports disk quotas.
        /// </summary>
        VolumeQuotas = 0x20
    }
}

namespace FsInfoCat.Util
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Runtime.InteropServices;
    using System.Text;
    /// <summary>
    /// Contains information about a filesystem volume.
    /// </summary>
    public class VolumeInformation : IEquatable<VolumeInformation>, IEqualityComparer<string>
    {
        #region Fields

        public const int InteropStringCapacity = 261;

        private StringComparer _nameComparer;
        private static VolumeInformation _default = null;
        private readonly DirectoryInfo _root;

        #endregion

        #region Properties

        /// <summary>
        /// Volume information for system drive.
        /// </summary>
        public static VolumeInformation Default
        {
            get
            {
                if (_default == null)
                    _default = new VolumeInformation();
                return _default;
            }
        }

        /// <summary>
        /// Gets the name of the volume root directory.
        /// </summary>
        public string Name { get { return _root.Name; } }

        /// <summary>
        /// Gets a value indicating whether the volume / root directory exists
        /// </summary>
        public bool Exists { get { return _root.Exists; } }

        /// <summary>
        /// Gets the full path name of the volume root directory.
        /// </summary>
        public string RootPathName { get { return _root.FullName; } }

        /// <summary>
        /// Gets the name of the file system.
        /// </summary>
        public string FileSystemName { get; private set; }

        /// <summary>
        /// Gets the name of the volume.
        /// </summary>
        public string VolumeName { get; private set; }

        /// <summary>
        /// Gets the volume serial number.
        /// </summary>
        public uint SerialNumber { get; private set; }

        /// <summary>
        /// Gets the maximum length for file/directory names.
        /// </summary>
        public uint MaxNameLength { get; private set; }

        /// <summary>
        /// Gets a value that indicates the volume capabilities and attributes.
        /// </summary>
        public FileSystemFeature Flags { get; private set; }

        /// <summary>
        /// Indicates whether the current volume file names are case-sensitive.
        /// </summary>
        public bool IsCaseSensitive { get { return Flags.HasFlag(FileSystemFeature.CasePreservedNames); } }

        #endregion

        #region Constructors
        
        /// <summary>
        /// Initializes a new instance of <see cref="VolumeInformation"/>.
        /// </summary>
        /// <param name="directory">A child <seealso cref="DirectoryInfo"/> of the volume information to retrieve or null for the volume that contains the system directory.</param>
        public VolumeInformation(DirectoryInfo directory)
        {
            _root = ((directory == null) ? new DirectoryInfo(Environment.SystemDirectory) : directory).Root;

            StringBuilder volumeNameBuffer = new StringBuilder(InteropStringCapacity);
            StringBuilder fsn = new StringBuilder(InteropStringCapacity);
            uint sn = 0U, maxNameLength = 0U;
            FileSystemFeature flags = default(FileSystemFeature);
            if (!GetVolumeInformation(_root.FullName, volumeNameBuffer, InteropStringCapacity, out sn, out maxNameLength, out flags, fsn, InteropStringCapacity))
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            VolumeName = volumeNameBuffer.ToString();
            FileSystemName = fsn.ToString();
            Flags = flags;
            SerialNumber = sn;
            MaxNameLength = maxNameLength;
            _nameComparer = (IsCaseSensitive) ? StringComparer.InvariantCulture : StringComparer.InvariantCultureIgnoreCase;
        }
        
        /// <summary>
        /// Initializes a new instance of <see cref="VolumeInformation"/>.
        /// </summary>
        /// <param name="directory">A child <seealso cref="DirectoryInfo"/> of the volume information to retrieve or null for the volume that contains the system directory.</param>
        public VolumeInformation(string rootPath)
        {
            _root = new DirectoryInfo(rootPath);

            StringBuilder volumeNameBuffer = new StringBuilder(InteropStringCapacity);
            StringBuilder fsn = new StringBuilder(InteropStringCapacity);
            uint sn = 0U, maxNameLength = 0U;
            FileSystemFeature flags = default(FileSystemFeature);
            if (!GetVolumeInformation(rootPath, volumeNameBuffer, InteropStringCapacity, out sn, out maxNameLength, out flags, fsn, InteropStringCapacity))
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            VolumeName = volumeNameBuffer.ToString();
            FileSystemName = fsn.ToString();
            Flags = flags;
            SerialNumber = sn;
            MaxNameLength = maxNameLength;
            _nameComparer = (IsCaseSensitive) ? StringComparer.InvariantCulture : StringComparer.InvariantCultureIgnoreCase;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="VolumeInformation"/> describing the volume that contains system directory.
        /// </summary>
        public VolumeInformation() : this((DirectoryInfo)null) { }

        #endregion

        #region Methods
        
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool GetVolumeInformation(string rootPathName, StringBuilder volumeNameBuffer, int volumeNameSize, out uint volumeSerialNumber,
            out uint maximumComponentLength, out FileSystemFeature fileSystemFlags, StringBuilder fileSystemNameBuffer, int nFileSystemNameSize);

        /// <summary>
        /// Determines whether the current <see cref="VolumeInformation"/> object is equal to another.
        /// </summary>
        /// <param name="other">Other <see cref="VolumeInformation"/> to compare.</param>
        /// <returns><c>true</c> if the current <see cref="VolumeInformation"/> is equal to <paramref name="other"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(VolumeInformation other) { return other != null && (ReferenceEquals(this, other) || SerialNumber == other.SerialNumber); }

        /// <summary>
        /// Determines whether the current <see cref="VolumeInformation"/> object is equal to another object.
        /// </summary>
        /// <param name="obj">Object to compare</param>
        /// <returns><c>true</c> if <paramref name="other"/> is a <see cref="VolumeInformation"/> and is equal to the current <see cref="VolumeInformation"/>; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj) { return obj != null && obj is VolumeInformation && Equals((VolumeInformation)obj); }

        /// <summary>
        /// Returns the hash code for the current <see cref="VolumeInformation"/> object.
        /// </summary>
        /// <returns>The hash code for the current <see cref="VolumeInformation"/> object.</returns>
        public override int GetHashCode() { return (VolumeName == null) ? Name.GetHashCode() : (int)SerialNumber; }

        /// <summary>
        /// Gets a string representation of the current <see cref="VolumeInformation"/> object.
        /// </summary>
        /// <returns>A string version of the current object.</returns>
        public override string ToString() { return (FileSystemName.Length == 0) ? Path.DirectorySeparatorChar + _root.Name : (SerialNumber >> 16).ToString("X4") + "-" + (SerialNumber & 0xFFFF).ToString("X4") + Path.DirectorySeparatorChar + VolumeName; }

        private static readonly char[] PathSeparators = new char[] { Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar, Path.VolumeSeparatorChar };

        /// <summary>
        /// Gets a normalized path string.
        /// </summary>
        /// <param name="path">Path string to normalize.</param>
        /// <param name="keepRelative">If <c>true</c>, then path is not resolved to a full path name; otherwise, the path is resolved and normalized to a fully qualified path name.</param>
        /// <returns>The normalized path string.</returns>
        public string NormalizePath(string path, bool keepRelative)
        {
            if (!keepRelative || Path.IsPathRooted(path))
                return NormalizePath(path, null);

            if (string.IsNullOrEmpty(path) || path == ".")
                return ".";

            int index = path.IndexOfAny(PathSeparators);
            if (index < 0)
                return path;

            if (path[index] == Path.VolumeSeparatorChar)
            {
                if (index == path.Length - 1)
                    return path + Path.DirectorySeparatorChar;
                string p = NormalizePath(path.Substring(index + 1));
                return (p[0] == Path.DirectorySeparatorChar) ? path.Substring(0, index) + p : ((p == ".") ? path + Path.DirectorySeparatorChar :
                    ((p.Length > 1 && p[0] == '.' && p[1] == Path.DirectorySeparatorChar) ? path + p.Substring(1) : path + Path.DirectorySeparatorChar + p));
            }

            List<string> elements = new List<string>(path.Split(PathSeparators));
            for (int i = 1; i < elements.Count; i++)
            {
                switch (elements[i])
                {
                    case "":
                    case ".":
                        elements.RemoveAt(i);
                        i--;
                        break;
                    case "..":
                        if (i == 1)
                        {
                            switch (elements[0])
                            {
                                case ".":
                                    elements[0] = "..";
                                    elements.RemoveAt(1);
                                    i--;
                                    break;
                                case "..":
                                case "":
                                    break;
                                default:
                                    elements[0] = ".";
                                    elements.RemoveAt(1);
                                    i--;
                                    break;
                            }
                        }
                        else
                        {
                            elements.RemoveAt(i);
                            elements.RemoveAt(i - 1);
                            i -= 2;
                        }
                        break;
                }
            }

            if (elements.Count > 1 && elements[0] == ".")
                elements.RemoveAt(0);

            return String.Join(new string(new char[] { Path.DirectorySeparatorChar }), elements);
        }

        /// <summary>
        /// Gets a normalized path string.
        /// </summary>
        /// <param name="path">Path string to normalize.</param>
        /// <param name="basePath">If not null or empty and <paramref name="path"/> is relative, then the <paramref name="path"/> will be considered relative to this.</param>
        /// <returns>The fully qualified, normalized path string.</returns>
        public string NormalizePath(string path, string basePath)
        {
            if (string.IsNullOrEmpty(path) || path == ".")
                path = ".";
            if (Path.IsPathRooted(path) || String.IsNullOrEmpty(basePath))
                path = Path.GetFullPath(path);
            else
                path = Path.GetFullPath(Path.Combine(basePath, path));
            if (string.IsNullOrEmpty(Path.GetFileName(path)))
            {
                string d = Path.GetDirectoryName(path);
                if (!String.IsNullOrEmpty(d))
                    return d;
            }
            return path;
        }

        /// <summary>
        /// Gets a normalized path string.
        /// </summary>
        /// <param name="path">Path string to normalize.</param>
        /// <returns>The fully qualified, normalized path string.</returns>
        public string NormalizePath(string path) { return NormalizePath(path, null); }

        /// <summary>
        /// Determines whether two path strings are equal.
        /// </summary>
        /// <param name="x">Path string being compared.</param>
        /// <param name="y">Path string being compared to.</param>
        /// <param name="doNotNormalize">If <c>true</c>, then path strings are compared as-is; otherwise their fully qualified, normalized equivalents are compared.</param>
        /// <returns>><c>true</c> if <paramref name="x"/> is equal to <paramref name="y"/>; otherwise, <c>false</c>.</returns>
        public bool ArePathsEqual(string x, string y, bool doNotNormalize) { return string.IsNullOrEmpty(x) ? string.IsNullOrEmpty(y) :
            (!string.IsNullOrEmpty(y) && (doNotNormalize ? _nameComparer.Equals(x, y) : _nameComparer.Equals(NormalizePath(x), NormalizePath(y)))); }

        /// <summary>
        /// Determines whether the two fully qualifed, normalized versions of the path strings are equal.
        /// </summary>
        /// <param name="x">Path string being compared.</param>
        /// <param name="y">Path string being compared to.</param>
        /// <returns>><c>true</c> if <paramref name="x"/> is equal to <paramref name="y"/>; otherwise, <c>false</c>.</returns>
        public bool ArePathsEqual(string x, string y) { return ArePathsEqual(x, y, false); }

        /// <summary>
        /// Determines whether a <seealso cref="DirectoryInfo"/> object represents a subdirectory within the current volume.
        /// </summary>
        /// <param name="directory"><seealso cref="DirectoryInfo"/> to test.</param>
        /// <returns><c>true</c> if <seealso cref="DirectoryInfo"/> object represents a subdirectory within the current volume; otherwise, <c>false</c>.</returns>
        public bool Contains(DirectoryInfo directory) { return directory != null && ArePathsEqual(_root.FullName, directory.Root.FullName); }

        /// <summary>
        /// Determines whether two file names are equals.
        /// </summary>
        /// <param name="x">File name being compared.</param>
        /// <param name="y">File name being compared to.</param>
        /// <returns>><c>true</c> if <paramref name="x"/> is equal to <paramref name="y"/>; otherwise, <c>false</c>.</returns>
        public bool Equals(string x, string y) { return (string.IsNullOrEmpty(x)) ? string.IsNullOrEmpty(y) : !string.IsNullOrEmpty(y) && _nameComparer.Equals(x, y); }

        /// <summary>
        /// Gets the hash code for a file name.
        /// </summary>
        /// <param name="obj">Value representing a file name.</param>
        /// <returns>Hashcode value for a file name.</returns>
        public int GetHashCode(string obj) { return _nameComparer.GetHashCode(obj ?? ""); }

        #endregion
    }
}
'@ -ErrorAction Stop;

<#
   TypeName: System.Management.ManagementObject#root\cimv2\Win32_LogicalDisk

Name                         MemberType    Definition                                                                                                               
----                         ----------    ----------                                                                                                               
PSComputerName               AliasProperty PSComputerName = __SERVER                                                                                                
Chkdsk                       Method        System.Management.ManagementBaseObject Chkdsk(System.Boolean FixErrors, System.Boolean VigorousIndexCheck, System.Bool...
Reset                        Method        System.Management.ManagementBaseObject Reset()                                                                           
SetPowerState                Method        System.Management.ManagementBaseObject SetPowerState(System.UInt16 PowerState, System.String Time)                       
Access                       Property      uint16 Access {get;set;}                                                                                                 
Availability                 Property      uint16 Availability {get;set;}                                                                                           
BlockSize                    Property      uint64 BlockSize {get;set;}                                                                                              
Caption                      Property      string Caption {get;set;}                                                                                                
Compressed                   Property      bool Compressed {get;set;}                                                                                               
ConfigManagerErrorCode       Property      uint32 ConfigManagerErrorCode {get;set;}                                                                                 
ConfigManagerUserConfig      Property      bool ConfigManagerUserConfig {get;set;}                                                                                  
CreationClassName            Property      string CreationClassName {get;set;}                                                                                      
Description                  Property      string Description {get;set;}                                                                                            
DeviceID                     Property      string DeviceID {get;set;}                                                                                               
DriveType                    Property      uint32 DriveType {get;set;}                                                                                              
ErrorCleared                 Property      bool ErrorCleared {get;set;}                                                                                             
ErrorDescription             Property      string ErrorDescription {get;set;}                                                                                       
ErrorMethodology             Property      string ErrorMethodology {get;set;}                                                                                       
FileSystem                   Property      string FileSystem {get;set;}                                                                                             
FreeSpace                    Property      uint64 FreeSpace {get;set;}                                                                                              
InstallDate                  Property      string InstallDate {get;set;}                                                                                            
LastErrorCode                Property      uint32 LastErrorCode {get;set;}                                                                                          
MaximumComponentLength       Property      uint32 MaximumComponentLength {get;set;}                                                                                 
MediaType                    Property      uint32 MediaType {get;set;}                                                                                              
Name                         Property      string Name {get;set;}                                                                                                   
NumberOfBlocks               Property      uint64 NumberOfBlocks {get;set;}                                                                                         
PNPDeviceID                  Property      string PNPDeviceID {get;set;}                                                                                            
PowerManagementCapabilities  Property      uint16[] PowerManagementCapabilities {get;set;}                                                                          
PowerManagementSupported     Property      bool PowerManagementSupported {get;set;}                                                                                 
ProviderName                 Property      string ProviderName {get;set;}                                                                                           
Purpose                      Property      string Purpose {get;set;}                                                                                                
QuotasDisabled               Property      bool QuotasDisabled {get;set;}                                                                                           
QuotasIncomplete             Property      bool QuotasIncomplete {get;set;}                                                                                         
QuotasRebuilding             Property      bool QuotasRebuilding {get;set;}                                                                                         
Size                         Property      uint64 Size {get;set;}                                                                                                   
Status                       Property      string Status {get;set;}                                                                                                 
StatusInfo                   Property      uint16 StatusInfo {get;set;}                                                                                             
SupportsDiskQuotas           Property      bool SupportsDiskQuotas {get;set;}                                                                                       
SupportsFileBasedCompression Property      bool SupportsFileBasedCompression {get;set;}                                                                             
SystemCreationClassName      Property      string SystemCreationClassName {get;set;}                                                                                
SystemName                   Property      string SystemName {get;set;}                                                                                             
VolumeDirty                  Property      bool VolumeDirty {get;set;}                                                                                              
VolumeName                   Property      string VolumeName {get;set;}                                                                                             
VolumeSerialNumber           Property      string VolumeSerialNumber {get;set;}                                                                                     
__CLASS                      Property      string __CLASS {get;set;}                                                                                                
__DERIVATION                 Property      string[] __DERIVATION {get;set;}                                                                                         
__DYNASTY                    Property      string __DYNASTY {get;set;}                                                                                              
__GENUS                      Property      int __GENUS {get;set;}                                                                                                   
__NAMESPACE                  Property      string __NAMESPACE {get;set;}                                                                                            
__PATH                       Property      string __PATH {get;set;}                                                                                                 
__PROPERTY_COUNT             Property      int __PROPERTY_COUNT {get;set;}                                                                                          
__RELPATH                    Property      string __RELPATH {get;set;}                                                                                              
__SERVER                     Property      string __SERVER {get;set;}                                                                                               
__SUPERCLASS                 Property      string __SUPERCLASS {get;set;}                                                                                           
PSStatus                     PropertySet   PSStatus {Status, Availability, DeviceID, StatusInfo}                                                                    
ConvertFromDateTime          ScriptMethod  System.Object ConvertFromDateTime();                                                                                     
ConvertToDateTime            ScriptMethod  System.Object ConvertToDateTime();
#>
Resolve-DnsName -Name "servicenowdiag479.file.core.windows.net"
<#
$connectTestResult = Test-NetConnection -ComputerName servicenowdiag479.file.core.windows.net -Port 445
if ($connectTestResult.TcpTestSucceeded) {
    # Save the password so the drive will persist on reboot
    cmd.exe /C "cmdkey /add:`"servicenowdiag479.file.core.windows.net`" /user:`"Azure\servicenowdiag479`" /pass:`"jFpbf9ilT+uDN1sQYY6ClGXzrX7xjFwSd8nmg1AIMCA7AzDadASW51CBKVfcpivqf0cvFP7Yjq0ER/fyxZ25KQ==`""
    # Mount the drive
    New-PSDrive -Name Z -PSProvider FileSystem -Root "\\servicenowdiag479.file.core.windows.net\testazureshare" -Persist
} else {
    Write-Error -Message "Unable to reach the Azure storage account via port 445. Check to make sure your organization or ISP is not blocking port 445, or use Azure P2S VPN, Azure S2S VPN, or Express Route to tunnel SMB traffic over a different port."
}



            public static IEnumerable TestCases
            {
                get
                {
                    yield return new TestCaseData(new IVolumeInfo[0], new VolumeInfo
                    {
                        RootPathName = "",
                        VolumeName = "",
                        DriveFormat = "",
                        SerialNumber = 0U,
                        CaseSensitive = false
                    });
                    Func<VolumeInfo, TestCaseData> expectedFromArg = v => new TestCaseData(new IVolumeInfo[] { v }, v.Clone());
                    yield return expectedFromArg(new VolumeInfo
                    {
                        RootPathName = "C:\\",
                        VolumeName = "OS",
                        DriveFormat = "NTFS",
                        SerialNumber = 0x9E497DE8U,
                        CaseSensitive = false
                    });
                    yield return expectedFromArg(new VolumeInfo
                    {
                        RootPathName = "F:\\",
                        VolumeName = "HP_TOOLS",
                        DriveFormat = "FAT32",
                        SerialNumber = 0x3B518D4BU,
                        CaseSensitive = false
                    });
                    yield return expectedFromArg(new VolumeInfo
                    {
                        RootPathName = "Z:\\",
                        VolumeName = "",
                        DriveFormat = "MAFS",
                        SerialNumber = 0xB04D955DU,
                        CaseSensitive = false
                    });
                    yield return expectedFromArg(new VolumeInfo
                    {
                        RootPathName = "/",
                        VolumeName = "",
                        DriveFormat = "ext3",
                        SerialNumber = 0xB04D955DU,
                        CaseSensitive = false
                    });
                }
            }

#>

$LogicalDisks = @(Get-WmiObject -Class 'Win32_LogicalDisk' -Amended);
$LogicalDisks | Out-GridView -Title $LogicalDisks[0].ClassPath;
$LogicalDisks | ForEach-Object { ($_.Properties | Select-Object -Property 'IsArray', 'Name', 'Origin', 'Type', 'Value') | Out-GridView -Title "($($_.__PATH)).Properties" }
$LogicalDisks | ForEach-Object { ($_.GetRelationships() | Select-Object -Property 'GroupComponent', 'PartComponent') | Out-GridView -Title "($($_.__PATH)).Relationships" }
($LogicalDisks.Properties  | Select-Object -First 1) | Get-Member