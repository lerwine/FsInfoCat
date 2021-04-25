using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FsInfoCat.Desktop.Model
{
    public class LocalComparison : IFileComparison
    {
        public Guid FileId1 { get; set; }

        public LocalFile File1 { get; set; }

        public Guid FileId2 { get; set; }

        public LocalFile File2 { get; set; }

        public bool AreEqual { get; set; }

        IFile IFileComparison.File1 => File1;

        IFile IFileComparison.File2 => File2;
    }
}
