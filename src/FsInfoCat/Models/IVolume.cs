using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public interface IVolume : IModficationAuditable
    {
        Guid VolumeID { get; set; }

        Guid? HostDeviceID { get; set; }

        string DisplayName { get; set; }

        /// <summary>
        /// Gets the full path name of the volume root directory.
        /// </summary>
        string RootPathName { get; set; }

        /// <summary>
        /// Gets the name of the file system.
        /// </summary>
        string FileSystemName { get; set; }

        /// <summary>
        /// Gets the name of the volume.
        /// </summary>
        string VolumeName { get; set; }

        /// <summary>
        /// Gets the volume serial number.
        /// </summary>
        uint SerialNumber { get; set; }

        /// <summary>
        /// Gets the maximum length for file/directory names.
        /// </summary>
        uint MaxNameLength { get; set; }

        /// <summary>
        /// Gets a value that indicates the volume capabilities and attributes.
        /// </summary>
        FileSystemFeature Flags { get; set; }

        bool IsInactive { get; set; }

        string Notes { get; set; }
    }
}
