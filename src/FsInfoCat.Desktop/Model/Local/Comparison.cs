using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.Model.Local
{
    public class Comparison
    {
        public Guid Id { get; set; }
        public Guid FileId1 { get; set; }
        public Guid FileId2 { get; set; }
        public bool AreEqual { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public virtual File File1 { get; set; }
        public virtual File File2 { get; set; }
    }
}
