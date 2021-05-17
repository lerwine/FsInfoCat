using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.Model.Local
{
    [Obsolete("This is remote only")]
    public class FileRelocateTask
    {
        public FileRelocateTask()
        {
            Notes = "";
            Files = new HashSet<File>();
        }

        public Guid Id { get; set; }
        public AppTaskStatus Status { get; set; }
        public PriorityLevel Priority { get; set; }
        public string ShortDescription { get; set; }
        public Guid TargetDirectoryId { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public virtual ICollection<File> Files { get; set; }
        public virtual Directory TargetDirectory { get; set; }
    }
}
