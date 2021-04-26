using System;
using System.Collections.Generic;

namespace FsInfoCat.Desktop.Model
{
    public interface IChecksumCalculation
    {
        IReadOnlyCollection<byte> Checksum { get; }
        IReadOnlyCollection<IFile> Files { get; }
        Guid Id { get; }
        long Length { get; }
        bool TryGetMD5Checksum(out UInt128 result);
    }
}
