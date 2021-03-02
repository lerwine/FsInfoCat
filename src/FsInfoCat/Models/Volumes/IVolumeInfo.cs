using System;
using FsInfoCat.Util;

namespace FsInfoCat.Models.Volumes
{
    public interface IVolumeInfo
    {
        /// <summary>
        /// Gets the absolute file URI of the the volume root directory.
        /// </summary>
        FileUri RootUri { get; set; }

        // TODO: Need to swith to using URI (FileUri) instead of RootPathName - Change to calculated property for human readability
        /// <summary>
        /// Gets the full path name of the volume root directory.
        /// </summary>
        string RootPathName { get; set; }

        /// <summary>
        /// Gets the name of the volume.
        /// </summary>
        string VolumeName { get; set; }

        /// <summary>
        /// Gets the name of the volume.
        /// </summary>
        string DriveFormat { get; set; }

        /*
            Get-WmiObject -Class 'Win32_LogicalDisk' can be used to get 32-bit serial number in windows
            lsblk -a -b -f -J -o NAME,LABEL,MOUNTPOINT,SIZE,FSTYPE,UUID
        */
        VolumeIdentifier Identifier { get; set; }

        bool CaseSensitive { get; set; }
    }
}
