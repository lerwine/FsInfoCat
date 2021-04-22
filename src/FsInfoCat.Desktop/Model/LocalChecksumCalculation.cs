using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FsInfoCat.Desktop.Model
{
    public class LocalChecksumCalculation : IChecksumCalculation
    {
        private readonly ReadOnlyListDelegateWrapper<byte, byte> _checksumWrapper;
        private readonly ReadOnlyListDelegateWrapper<LocalFile, IFile> _filesWrapper;

        public LocalChecksumCalculation()
        {
            _checksumWrapper = new ReadOnlyListDelegateWrapper<byte, byte>(() => Checksum);
            _filesWrapper = new ReadOnlyListDelegateWrapper<LocalFile, IFile>(() => Files);
        }

        [Required]
        [Key]
        public Guid Id { get; set; }

        [Required]
        public long Length { get; set; }

        [MaxLength(MD5Checksum.MD5ByteSize)]
        [MinLength(MD5Checksum.MD5ByteSize)]
        public byte[] Checksum { get; set; }

        [InverseProperty(nameof(LocalComparison.FileId1))]
        public List<LocalFile> Files { get; set; }

        IReadOnlyList<byte> IChecksumCalculation.Checksum => _checksumWrapper;

        IReadOnlyList<IFile> IChecksumCalculation.Files => _filesWrapper;

        public bool TryGetMD5Checksum(out MD5Checksum result) => MD5Checksum.TryCreate(Checksum, out result);
    }
}
