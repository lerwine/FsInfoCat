namespace FsInfoCat.Models.Volumes
{
    public interface IVolumeInfo
    {
        /// <summary>
        /// Gets the full path name of the volume root directory.
        /// /// </summary>
        string RootPathName { get; set; }

        /// <summary>
        /// Gets the name of the volume.
        /// </summary>
        string VolumeName { get; set; }

        /// <summary>
        /// Gets the name of the volume.
        /// </summary>
        string DriveFormat { get; set; }

        /// <summary>
        /// Gets the volume serial number.
        /// </summary>
        uint SerialNumber { get; set; }

        bool CaseSensitive { get; set; }
    }
}
