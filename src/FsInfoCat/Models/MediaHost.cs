using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Models
{
    public class MediaHost
    {
        public const string PATTERN_DOTTED_NAME = @"^[a-z][a-z\d_]*(\.[a-z][a-z\d_]*)$";
        private string _createdBy = "";
        private string _modifiedBy = "";
        private string _displayName = "";
        private string _machineName = "";
        private string _notes = "";

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

        [Required()]
        [MinLength(1)]
        [MaxLength(RegisteredUser.Max_Length_Login_Name)]
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

        [Required()]
        [MinLength(1)]
        [MaxLength(RegisteredUser.Max_Length_Login_Name)]
        [Display(Name = "Modified By")]
        [DataType(DataType.Text)]
        [RegularExpression(PATTERN_DOTTED_NAME)]
        public string ModifiedBy
        {
            get { return _modifiedBy; }
            set { _modifiedBy = (null == value) ? "" : value; }
        }

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


        public ICollection<MediaVolume> Volumes { get; set; }
    }
}
