using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public class MediaHost
    {
        public const string PATTERN_DOTTED_NAME = @"^[a-z][a-z\d_]*(\.[a-z][a-z\d_]*)$";
        private string _displayName = "";
        private string _machineName = "";
        private string _notes = "";

        public MediaHost() { }

        public MediaHost(string machineName, bool isWindows, Guid createdBy)
        {
            HostID = Guid.NewGuid();
            MachineName = machineName;
            IsWindows = isWindows;
            CreatedOn = ModifiedOn = DateTime.Now;
            CreatedBy = ModifiedBy = createdBy;
        }

        [Required()]
        [Key()]
        [Display(Name = "ID")]
        public Guid HostID { get; set; }

        [MaxLength(256)]
        [Display(Name = "Display Name")]
        [DataType(DataType.Text)]
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = (null == value) ? "" : value; }
        }

        [Required()]
        [MinLength(1)]
        [MaxLength(256)]
        [Display(Name = "Machine Name")]
        [DataType(DataType.Text)]
        [RegularExpression(PATTERN_DOTTED_NAME)]
        public string MachineName
        {
            get { return _machineName; }
            set { _machineName = (null == value) ? "" : value; }
        }

        [Display(Name = "Is Windows")]
        public bool IsWindows { get; set; }

        [Display(Name = "Is Inactive")]
        public bool IsInactive { get; set; }

        [Required()]
        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        public string Notes
        {
            get { return _notes; }
            set { _notes = (null == value) ? "" : value; }
        }

        [Required()]
        [Display(Name = "Created On")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedOn { get; set; }

        [Required()]
        [Display(Name = "Created By")]
        public Guid CreatedBy { get; set; }

        [Required()]
        [Display(Name = "Modified On")]
        [DataType(DataType.DateTime)]
        public DateTime ModifiedOn { get; set; }

        [Required()]
        [Display(Name = "Modified By")]
        public Guid ModifiedBy { get; set; }

        public ICollection<MediaVolume> Volumes { get; set; }
    }
}
