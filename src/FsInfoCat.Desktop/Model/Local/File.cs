using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.Model.Local
{
    public class File
    {
        public File()
        {
            Redundancies = new HashSet<Redundancy>();
            Comparisons1 = new HashSet<Comparison>();
            Comparisons2 = new HashSet<Comparison>();
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public FileStatus Status { get; set; }
        public Guid ParentId { get; set; }
        public Guid HashCalculationId { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public Guid? FileRelocateTaskId { get; set; }

        public virtual HashCalculation HashCalculation { get; set; }
        public virtual Directory Parent { get; set; }
        public virtual ICollection<Redundancy> Redundancies { get; set; }
        public virtual ICollection<Comparison> Comparisons1 { get; set; }
        public virtual ICollection<Comparison> Comparisons2 { get; set; }
        public virtual FileRelocateTask FileRelocateTask { get; set; }
    }
}
