namespace FsInfoCat.Desktop.Model.Test
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Comparison
    {
        public Guid Id { get; set; }

        public Guid FileId1 { get; set; }

        public Guid FileId2 { get; set; }

        public bool AreEqual { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime ModifiedOn { get; set; }

        public virtual File File { get; set; }

        public virtual File File1 { get; set; }
    }
}
