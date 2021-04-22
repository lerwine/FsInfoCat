using System;
using System.Collections.Generic;

namespace FsInfoCat.Desktop.Model
{
    public interface IChecksumCalculation
    {
        IReadOnlyList<byte> Checksum { get; }
        IReadOnlyList<IFile> Files { get; }
        Guid Id { get; }
        long Length { get; }
        bool TryGetMD5Checksum(out MD5Checksum result);
    }
}
