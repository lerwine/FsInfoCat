using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.Model.Local
{
    [System.Obsolete("Use FsInfoCat.LocalDb.HashCalculation or FsInfoCat.Model.Local.ILocalHashCalculation")]
    public class HashCalculation
    {
        public HashCalculation()
        {
            Files = new HashSet<File>();
        }

        public Guid Id { get; set; }
        public long Length { get; set; }
        public byte[] Data { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public virtual ICollection<File> Files { get; set; }
    }
}
