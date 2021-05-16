namespace FsInfoCat.Desktop.Model.Test
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class FileSystem
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public FileSystem()
        {
            FsSymbolicNames = new HashSet<FsSymbolicName>();
            Volumes = new HashSet<Volume>();
        }

        public Guid Id { get; set; }

        [Required]
        [StringLength(128)]
        public string DisplayName { get; set; }

        public bool CaseSensitiveSearch { get; set; }

        public bool ReadOnly { get; set; }

        public long MaxNameLength { get; set; }

        public byte? DefaultDriveType { get; set; }

        [Required]
        public string Notes { get; set; }

        public bool IsInactive { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<FsSymbolicName> FsSymbolicNames { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Volume> Volumes { get; set; }
    }
}
