using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace FsInfoCat.Desktop.Model
{
    public class VolumeInfo
    {
        /// <summary>
        /// The serial number that was assigned to the drive when it was formatted.
        /// </summary>
        public uint SerialNumber { get; }

        /// <summary>
        /// The full name of the root directory of the drive.
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// The name of the volume.
        /// </summary>
        public string VolumeName { get; }

        /// <summary>
        /// The name of the file system that is on the volume.
        /// </summary>
        public string FileSystemName { get; }

        /// <summary>
        /// The maximum supported length for file name components.
        /// </summary>
        public uint MaxNameLength { get; }

        /// <summary>
        /// Flags which contains features and capabilities of volume.
        /// </summary>
        public FileSystemFeature Features { get; }

        /// <summary>
        /// Type of drive for the volume.
        /// </summary>
        public DriveType Type { get; }

        [DllImport("Kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public extern static bool GetVolumeInformation(string rootPathName, StringBuilder volumeNameBuffer, int volumeNameSize, out uint volumeSerialNumber,
            out uint maximumComponentLength, out FileSystemFeature fileSystemFlags, StringBuilder fileSystemNameBuffer, int nFileSystemNameSize);

        /// <summary>
        /// Creates a new instance of <see cref="VolumeInfo"/> from the root path name or drive name.
        /// </summary>
        /// <param name="rootPathName">The root path name or drive name.</param>
        public VolumeInfo(string rootPathName)
        {
            DirectoryInfo dir = new DirectoryInfo(rootPathName);
            if (dir.Root != null)
                dir = dir.Root;
            try
            {
                DriveInfo drive = new DriveInfo(dir.FullName);
                dir = drive.RootDirectory;
                Type = drive.DriveType;
            }
            catch { Type = DriveType.Unknown; }
            FullName = dir.FullName;
            StringBuilder v = new StringBuilder(261);
            StringBuilder f = new StringBuilder(261);
            if (!GetVolumeInformation(dir.FullName, v, v.Capacity, out uint s, out uint m, out FileSystemFeature flags, f, f.Capacity))
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            VolumeName = v.ToString();
            FileSystemName = f.ToString();
            SerialNumber = s;
            MaxNameLength = m;
            Features = flags;
        }
    }
}
