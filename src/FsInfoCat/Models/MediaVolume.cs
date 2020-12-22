using System;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public class MediaVolume
    {
        [MinLength(32)]
        [MaxLength(32)]
        [Required()]
        [Key()]
        [RegularExpression(@"^[\da-f]{32}$")]
        [Display(Name = "ID")]
        public Guid VolumeID { get; set; }

        [MaxLength(32)]
        [RegularExpression(@"^([\da-f]{32})?$")]
        [Display(Name = "Host ID")]
        public Guid? HostID { get; set; }

        [Required()]
        [Display(Name = "Created On")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        private string _createdBy = "";
        [Required()]
        [MinLength(1)]
        [MaxLength(256)]
        [Display(Name = "Created By")]
        [DataType(DataType.Text)]
        public string CreatedBy
        {
            get { return _createdBy; }
            set { _createdBy = (null == value) ? "" : value; }
        }

        [Required()]
        [Display(Name = "Modified On")]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; }

        private string _modifiedBy = "";
        [Required()]
        [MinLength(1)]
        [MaxLength(256)]
        [Display(Name = "Modified By")]
        [DataType(DataType.Text)]
        public string ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = (null == value) ? "" : value; }
        }

        private string _rootPathName = "";
        [Required()]
        [MinLength(1)]
        [MaxLength(1024)]
        [Display(Name = "Root Path Name")]
        [DataType(DataType.Text)]
        /// <summary>
        /// Gets the full path name of the volume root directory.
        /// </summary>
        public string RootPathName
        {
            get { return _rootPathName; }
            set { _rootPathName = (null == value) ? "" : value; }
        }

        private string _fileSystemName = "";
        [Required()]
        [MinLength(1)]
        [MaxLength(128)]
        [Display(Name = "Filesystem Name")]
        [DataType(DataType.Text)]
        /// <summary>
        /// Gets the name of the file system.
        /// </summary>
        public string FileSystemName
        {
            get { return _fileSystemName; }
            set { _fileSystemName = (null == value) ? "" : value; }
        }

        private string _volumeName = "";
        [Required()]
        [MinLength(1)]
        [MaxLength(128)]
        [Display(Name = "Volume Name")]
        [DataType(DataType.Text)]
        /// <summary>
        /// Gets the name of the volume.
        /// </summary>
        public string VolumeName
        {
            get { return _volumeName; }
            set { _volumeName = (null == value) ? "" : value; }
        }

        [Required()]
        [Display(Name = "Serial Number")]
        /// <summary>
        /// Gets the volume serial number.
        /// </summary>
        public uint SerialNumber { get; set; }

        [Required()]
        [Display(Name = "Max Name Length")]
        /// <summary>
        /// Gets the maximum length for file/directory names.
        /// </summary>
        public uint MaxNameLength { get; set; }

        [Required()]
        [Display(Name = "Flags")]
        [EnumDataType(typeof(FileSystemFeature))]
        /// <summary>
        /// Gets a value that indicates the volume capabilities and attributes.
        /// </summary>
        public FileSystemFeature Flags { get; set; }

        [Display(Name = "Is Inactive")]
        public bool IsInactive { get; set; }
        private string _notes = "";
        [Required()]
        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        public string Notes
        {
            get { return _notes; }
            set { _notes = (null == value) ? "" : value; }
        }

        public MediaHost Host { get; set; }
    }
}
