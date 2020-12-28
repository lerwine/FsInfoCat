using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public interface IVolume : IModficationAuditable
    {
        [Required()]
        [Key()]
        [Display(Name = "ID")]
        Guid VolumeID { get; set; }

        Guid? HostID { get; set; }

        [MaxLength(DB.Volume.Max_Length_DisplayName, ErrorMessage = DB.Volume.Error_Message_DisplayName)]
        [Display(Name = DB.Volume.DisplayName_DisplayName)]
        string DisplayName { get; set; }

        [Required(ErrorMessage = DB.Volume.Error_Message_RootPathName_Empty)]
        [MaxLength(DB.Volume.Max_Length_RootPathName, ErrorMessage = DB.Volume.Error_Message_RootPathName_Length)]
        [RegularExpression(ModelHelper.PATTERN_PATH_OR_URL, ErrorMessage = DB.Volume.Error_Message_RootPathName_Invalid)]
        [Display(Name = DB.Volume.DisplayName_RootPathName, Description = "Enter a file URI or a windows file path.")]
        /// <summary>
        /// Gets the full path name of the volume root directory.
        /// </summary>
        string RootPathName { get; set; }

        [Required(ErrorMessage = DB.Volume.Error_Message_FileSystemName_Empty)]
        [MaxLength(DB.Volume.Max_Length_FileSystemName, ErrorMessage = DB.Volume.Error_Message_FileSystemName_Length)]
        [Display(Name = DB.Volume.DisplayName_FileSystemName)]
        /// <summary>
        /// Gets the name of the file system.
        /// </summary>
        string FileSystemName { get; set; }

        [Required(ErrorMessage = DB.Volume.Error_Message_VolumeName_Empty)]
        [MaxLength(DB.Volume.Max_Length_Volume, ErrorMessage = DB.Volume.Error_Message_VolumeName_Length)]
        [Display(Name = DB.Volume.DisplayName_VolumeName)]
        /// <summary>
        /// Gets the name of the volume.
        /// </summary>
        string VolumeName { get; set; }

        [Required()]
        [Display(Name = "Serial Number")]
        /// <summary>
        /// Gets the volume serial number.
        /// </summary>
        uint SerialNumber { get; set; }

        [Required()]
        [Display(Name = "Max Name Length")]
        /// <summary>
        /// Gets the maximum length for file/directory names.
        /// </summary>
        uint MaxNameLength { get; set; }

        [Required()]
        [Display(Name = "Flags")]
        [EnumDataType(typeof(FileSystemFeature))]
        /// <summary>
        /// Gets a value that indicates the volume capabilities and attributes.
        /// </summary>
        FileSystemFeature Flags { get; set; }

        [Display(Name = "Is Inactive")]
        bool IsInactive { get; set; }

        [Required()]
        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        string Notes { get; set; }
    }
}
