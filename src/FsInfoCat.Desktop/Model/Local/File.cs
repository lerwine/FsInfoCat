using System;
using System.Collections.Generic;

namespace FsInfoCat.Desktop.Model.Local
{
    public class File : IFile
    {
        private readonly ReadOnlyCollectionDelegateWrapper<Comparison, IFileComparison> _comparisonsWrapper1;
        private readonly ReadOnlyCollectionDelegateWrapper<Comparison, IFileComparison> _comparisonsWrapper2;
        public File()
        {
            Redundancies = new HashSet<Redundancy>();
            Comparisons1 = new HashSet<Comparison>();
            Comparisons2 = new HashSet<Comparison>();
            _comparisonsWrapper1 = new ReadOnlyCollectionDelegateWrapper<Comparison, IFileComparison>(() => Comparisons1);
            _comparisonsWrapper2 = new ReadOnlyCollectionDelegateWrapper<Comparison, IFileComparison>(() => Comparisons2);
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
        IHashCalculation IFile.HashCalculation => throw new NotImplementedException();
        public virtual Directory Parent { get; set; }
        ISubDirectory IFile.Parent => throw new NotImplementedException();
        public virtual ICollection<Redundancy> Redundancies { get; set; }
        public virtual ICollection<Comparison> Comparisons1 { get; set; }
        IReadOnlyCollection<IFileComparison> IFile.Comparisons1 => _comparisonsWrapper1;
        public virtual ICollection<Comparison> Comparisons2 { get; set; }
        IReadOnlyCollection<IFileComparison> IFile.Comparisons2 => _comparisonsWrapper2;
        public virtual FileRelocateTask FileRelocateTask { get; set; }
    }
}
