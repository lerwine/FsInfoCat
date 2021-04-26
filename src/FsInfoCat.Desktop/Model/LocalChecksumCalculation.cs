using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FsInfoCat.Desktop.Model
{
    public class LocalChecksumCalculation : IChecksumCalculation
    {
        private readonly ReadOnlyCollectionDelegateWrapper<byte, byte> _checksumWrapper;
        private readonly ReadOnlyCollectionDelegateWrapper<LocalFile, IFile> _filesWrapper;

        public LocalChecksumCalculation()
        {
            _checksumWrapper = new ReadOnlyCollectionDelegateWrapper<byte, byte>(() => Checksum);
            _filesWrapper = new ReadOnlyCollectionDelegateWrapper<LocalFile, IFile>(() => Files);
        }

        public Guid Id { get; set; }

        public long Length { get; set; }

        public byte[] Checksum { get; set; }
        
        public List<LocalFile> Files { get; set; }

        IReadOnlyCollection<byte> IChecksumCalculation.Checksum => _checksumWrapper;

        IReadOnlyCollection<IFile> IChecksumCalculation.Files => _filesWrapper;

        public bool TryGetMD5Checksum(out UInt128 result) => UInt128.TryCreate(Checksum, out result);
    }
}
