using System;

namespace FsInfoCat.Desktop.Model.Local
{
    public class Comparison : IFileComparison
    {
        public Guid Id { get; set; }
        public Guid FileId1 { get; set; }
        public Guid FileId2 { get; set; }
        public bool AreEqual { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }

        public virtual File File1 { get; set; }
        IFile IFileComparison.File1 => File1;
        public virtual File File2 { get; set; }
        IFile IFileComparison.File2 => File2;
    }
}
