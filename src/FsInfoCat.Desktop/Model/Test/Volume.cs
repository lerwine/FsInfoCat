namespace FsInfoCat.Desktop.Model.Test
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Volume
    {
        public Guid Id { get; set; }

        [Required]
        [StringLength(128)]
        public string DisplayName { get; set; }

        [Required]
        [StringLength(1024)]
        public string RootPathName { get; set; }

        [Required]
        [StringLength(128)]
        public string VolumeName { get; set; }

        [Required]
        [StringLength(1024)]
        public string Identifier { get; set; }

        public Guid FileSystemId { get; set; }

        public byte Type { get; set; }

        public bool? CaseSensitiveSearch { get; set; }

        public bool? ReadOnly { get; set; }

        public long? MaxNameLength { get; set; }

        [Required]
        public string Notes { get; set; }

        public bool IsInactive { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public Guid RootDirectory_Id { get; set; }

        public virtual Directory Directory { get; set; }

        public virtual FileSystem FileSystem { get; set; }
    }
}
