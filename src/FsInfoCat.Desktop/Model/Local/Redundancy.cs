using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.Model.Local
{
    [Obsolete]
    public class Redundancy
    {
        public Redundancy()
        {
            Files = new HashSet<File>();
        }

        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public virtual ICollection<File> Files { get; set; }
    }
}
