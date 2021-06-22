using System;
using System.Management;

namespace FsInfoCat.Desktop.Model
{
    /// <summary>
    /// Wraps the Win32_Directory WMI class which represents a directory entry on a computer system running Windows.
    /// </summary>
    /// <remarks>References
    /// <list type="bullet">
    ///     <item><term>White, S. (2018, May 31)</term>
    ///         <description>Win32_Directory class - Win32 apps
    ///         <para>Retrieved from Microsoft Docs: https://docs.microsoft.com/en-us/windows/win32/cimwin32prov/win32-directory</para></description>
    ///     </item>
    /// </list></remarks>
    public class Win32_Directory
    {
        private readonly ManagementObject _managementObject;

        /// <summary>
        /// A short textual description of the object.
        /// </summary>
        public string Caption => (string)_managementObject[nameof(Caption)];

        /// <summary>
        /// A textual description of the object.
        /// </summary>
        public string Description => (string)_managementObject[nameof(Description)];

        /// <summary>
        /// Indicates when the object was installed. Lack of a value does not indicate that the object is not installed.
        /// </summary>
        public Win32_LocalTime InstallDate => _managementObject.ToWin32LocalTime(nameof(InstallDate));

        /// <summary>
        /// The Name property is a string representing the inherited name that serves as a key of a logical file instance within a file system.
        /// </summary>
        /// <value>
        /// The full path name.
        /// </value>
        public string Name => (string)_managementObject[nameof(Name)];

        /// <summary>
        /// String that indicates the current status of the object.
        /// </summary>
        /// <value>
        /// Includes the following values:
        /// <list type="bullet">
        /// <item><c>"OK"</c></item>
        /// <item><c>"Error"</c></item>
        /// <item><c>"Degraded"</c></item>
        /// <item><c>"Unknown"</c></item>
        /// <item><c>"Pred Fail"</c></item>
        /// <item><c>"Starting"</c></item>
        /// <item><c>"Stopping"</c></item>
        /// <item><c>"Service"</c></item>
        /// <item><c>"Stressed"</c></item>
        /// <item><c>"NonRecover"</c></item>
        /// <item><c>"No Contact"</c></item>
        /// <item><c>"Lost Comm"</c></item>
        /// </list>
        /// </value>
        public string Status => (string)_managementObject[nameof(Status)];

        /// <summary>
        /// Bitmask that represents the access rights required to access or perform specific operations on the directory.
        /// </summary>
        public Win32_AccessMask AccessMask => _managementObject.ToEnumPropertyValue<Win32_AccessMask>(nameof(AccessMask)).Value;

        /// <summary>
        /// Indicates whether the archive bit on the folder has been set.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this <see cref="Win32_Directory"/> should be archived; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>The archive bit is used by backup programs to identify files that should be backed up.</remarks>
        public bool? Archive => (bool?)_managementObject[nameof(Archive)];

        /// <summary>
        /// Indicates whether or not the folder has been compressed.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this <see cref="Win32_Directory"/> is compressed; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>WMI recognizes folders compressed using WMI itself or using the graphical user interface; it does not, however, recognize .ZIP files as being compressed.</remarks>
        public bool? Compressed => (bool?)_managementObject[nameof(Compressed)];

        /// <summary>
        /// Algorithm or tool (usually a method) used to compress the logical file.
        /// </summary>
        /// <value>
        /// The compression algorithm or one of the following values f it is not possible (or not desired) to describe the compression scheme (perhaps because it is not known):
        /// <list type="bullet">
        /// <item><term><c>"Unknown"</c></term>
        ///     <description>It is not known whether the logical file is compressed.</description></item>
        /// <item><term><c>"Compressed"</c></term>
        ///     <description>this <see cref="Win32_Directory"/> is compressed, but either its compression scheme is not known or not disclosed.</description></item>
        /// <item><term><c>"Not Compressed"</c></term>
        ///     <description>The logical file is not compressed.</description></item>
        /// </list>
        /// </value>
        public string CompressionMethod => (string)_managementObject[nameof(CompressionMethod)];

        /// <summary>
        /// Name of the first concrete class to appear in the inheritance chain used in the creation of an instance.
        /// </summary>
        public string CreationClassName => (string)_managementObject[nameof(CreationClassName)];

        /// <summary>
        /// Date that this <see cref="Win32_Directory"/> system object was created.
        /// </summary>
        public Win32_LocalTime CreationDate => _managementObject.ToWin32LocalTime(nameof(CreationDate));

        /// <summary>
        /// Creation class name of the scoping computer system.
        /// </summary>
        public string CSCreationClassName => (string)_managementObject[nameof(CSCreationClassName)];

        /// <summary>
        /// Name of the computer where this <see cref="Win32_Directory"/> system object is stored.
        /// </summary>
        public string CSName => (string)_managementObject[nameof(CSName)];

        /// <summary>
        /// Drive letter of the drive (including colon) where this <see cref="Win32_Directory"/> system object is stored.
        /// </summary>
        public string Drive => (string)_managementObject[nameof(Drive)];

        /// <summary>
        /// MS-DOS -compatible name for the folder.
        /// </summary>
        public string EightDotThreeFileName => (string)_managementObject[nameof(EightDotThreeFileName)];

        /// <summary>
        /// Indicates whether or not the folder has been encrypted.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the folder is encrypted; otherwise, <c>false</c>.
        /// </value>
        public bool? Encrypted => (bool?)_managementObject[nameof(Encrypted)];

        /// <summary>
        /// Algorithm or tool used to encrypt the logical file.
        /// </summary>
        /// <value>
        /// The encryption algorithm or one of the following values if it is not possible (or not desired) to describe the encryption scheme (perhaps because it is not known):
        /// <list type="bullet">
        /// <item><term><c>"Unknown"</c></term>
        ///     <description>It is not known whether the logical file is encrypted.</description></item>
        /// <item><term><c>"Encrypted"</c></term>
        ///     <description>this <see cref="Win32_Directory"/> is encrypted, but either its encrypted scheme is not known or not disclosed.</description></item>
        /// <item><term><c>"Not Encrypted"</c></term>
        ///     <description>The logical file is not encrypted.</description></item>
        /// </list>
        /// </value>
        public string EncryptionMethod => (string)_managementObject[nameof(EncryptionMethod)];

        /// <summary>
        /// File name extension for this <see cref="Win32_Directory"/> system object, not including the dot (.) that separates the extension from the file name.
        /// </summary>
        public string Extension => (string)_managementObject[nameof(Extension)];

        /// <summary>
        /// File name (without the dot or extension) of this <see cref="Win32_Directory"/>.
        /// </value>
        public string FileName => (string)_managementObject[nameof(FileName)];

        /// <summary>
        /// Size of the file system object, in bytes.
        /// </summary>
        /// <value>
        /// Although folders possess a FileSize property, the value 0 is always returned.
        /// </value>
        public ulong? FileSize => (ulong?)_managementObject[nameof(FileSize)];

        /// <summary>
        /// File type.
        /// </summary>
        /// <value>
        /// Folders are typically reported as <c>"Folder"</c>.
        /// </value>
        public string FileType => (string)_managementObject[nameof(FileType)];

        /// <summary>
        /// Class of the file system.
        /// </summary>
        public string FSCreationClassName => (string)_managementObject[nameof(FSCreationClassName)];

        /// <summary>
        /// Type of file system (NTFS, FAT, FAT32) installed on the drive where this <see cref="Win32_Directory"/> is located.
        /// </summary>
        public string FSName => (string)_managementObject[nameof(FSName)];

        /// <summary>
        /// Indicates whether tthis <see cref="Win32_Directory"/> is hidden.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this <see cref="Win32_Directory"/> is hidden; otherwise, <c>false</c>.
        /// </value>
        public bool? Hidden => (bool?)_managementObject[nameof(Hidden)];

        /// <summary>
        /// Number of "file opens" that are currently active against this <see cref="Win32_Directory"/>.
        /// </summary>
        public ulong? InUseCount => (ulong?)_managementObject[nameof(InUseCount)];

        /// <summary>
        /// Date this <see cref="Win32_Directory"/> was last accessed.
        /// </summary>
        public Win32_LocalTime LastAccessed => _managementObject.ToWin32LocalTime(nameof(LastAccessed));

        /// <summary>
        /// Date this <see cref="Win32_Directory"/> was last modified.
        /// </summary>
        public Win32_LocalTime LastModified => _managementObject.ToWin32LocalTime(nameof(LastModified));

        /// <summary>
        /// Path for this <see cref="Win32_Directory"/>.
        /// </summary>
        /// <value>
        /// The path, including the leading and trailing backslashes, but not the drive letter or the folder name.
        /// </value>
        public string Path => (string)_managementObject[nameof(Path)];

        /// <summary>
        /// Indicates whether you can read items in this <see cref="Win32_Directory"/>.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this <see cref="Win32_Directory"/> can be read; otherwise, <c>false</c>.
        /// </value>
        public bool Readable => (bool)_managementObject[nameof(Readable)];

        /// <summary>
        /// Indicates whether this <see cref="Win32_Directory"/> is a system file.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this <see cref="Win32_Directory"/> is a system file; otherwise, <c>false</c>.
        /// </value>
        public bool? System => (bool?)_managementObject[nameof(System)];

        /// <summary>
        /// Indicates whether this <see cref="Win32_Directory"/> is writeable.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this <see cref="Win32_Directory"/> can be written to; otherwise, <c>false</c>.
        /// </value>
        public bool? Writeable => (bool?)_managementObject[nameof(Writeable)];

        public Win32_Directory(ManagementObject managementObject)
        {
            _managementObject = managementObject ?? throw new ArgumentNullException(nameof(managementObject));
        }
    }
}
