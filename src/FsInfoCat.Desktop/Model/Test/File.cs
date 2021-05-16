namespace FsInfoCat.Desktop.Model.Test
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class File
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public File()
        {
            Comparisons = new HashSet<Comparison>();
            Comparisons1 = new HashSet<Comparison>();
            Redundancies = new HashSet<Redundancy>();
        }

        public Guid Id { get; set; }

        public Guid ParentId { get; set; }

        public Guid HashCalculationId { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comparison> Comparisons { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Comparison> Comparisons1 { get; set; }

        public virtual Directory Directory { get; set; }

        public virtual HashCalculation HashCalculation { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Redundancy> Redundancies { get; set; }
    }
}
