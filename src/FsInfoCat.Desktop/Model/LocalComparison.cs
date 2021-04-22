using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FsInfoCat.Desktop.Model
{
    public class LocalComparison : IFileComparison
    {
        [Required]
        [Key]
        public Guid FileId1 { get; set; }

        [Required]
        [ForeignKey(nameof(FileId1))]
        public LocalFile File1 { get; set; }

        [Required]
        [Key]
        public Guid FileId2 { get; set; }

        [Required]
        [ForeignKey(nameof(FileId2))]
        public LocalFile File2 { get; set; }

        [Required]
        public bool AreEqual { get; set; }

        IFile IFileComparison.File1 => File1;

        IFile IFileComparison.File2 => File2;
    }
}
