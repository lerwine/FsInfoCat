using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.WebApp.Models
{
    public class MediaHost
    {
        public const string PATTERN_DOTTED_NAME = @"^[a-z][a-z\d_]*(\.[a-z][a-z\d_]*)$";

        [MinLength(32)]
        [MaxLength(32)]
        [Required()]
        [Key()]
        [RegularExpression(@"^[\da-f]{32}$")]
        [Display(Name = "ID")]
        public Guid HostID { get; set; }

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
        [RegularExpression(PATTERN_DOTTED_NAME)]
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
        [RegularExpression(PATTERN_DOTTED_NAME)]
        public string ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = (null == value) ? "" : value; }
        }

        private string _machineName = "";
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

        private string _notes = "";
        [Required()]
        [Display(Name = "Notes")]
        [DataType(DataType.MultilineText)]
        public string Notes
        {
            get { return _notes; }
            set { _notes = (null == value) ? "" : value; }
        }


        public ICollection<MediaVolume> Volumes { get; set; }
    }
}
