using FsInfoCat.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace FsInfoCat.Models.Volumes
{
    /// <summary>
    /// Represents file system volume information.
    /// </summary>
    public interface IVolumeInfo : INotifyPropertyValueChanging, INotifyPropertyValueChanged
    {
        /// <summary>
        /// Gets or sets the absolute file URI of the the volume root directory.
        /// </summary>
        FileUri RootUri { get; set; }

        /// <summary>
        /// Gets the full path name of the volume root directory.
        /// </summary>
        string RootPathName { get; }

        /// <summary>
        /// Gets or sets the name of the volume.
        /// </summary>
        string VolumeName { get; set; }

        /// <summary>
        /// Gets or sets the The name of the filesystem / format for the volume.
        /// </summary>
        string DriveFormat { get; set; }

        /*
            Get-WmiObject -Class 'Win32_LogicalDisk' can be used to get 32-bit serial number in windows
            lsblk -a -b -f -J -o NAME,LABEL,MOUNTPOINT,SIZE,FSTYPE,UUID
        */
        /// <summary>
        /// Gets or sets the unique identifier for the volume.
        /// </summary>
        VolumeIdentifier Identifier { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the file names are case-sensitive when saved to the target volume.
        /// </summary>
        bool CaseSensitive { get; set; }

        IEqualityComparer<string> GetPathComparer();
    }
}
