using System;
using System.Collections.Generic;

namespace FsInfoCat.Desktop.Model
{
    [System.Obsolete("Use FsInfoCat.Model.IHashCalculation")]
    public interface IHashCalculation
    {
        IReadOnlyCollection<byte> Data { get; }
        IReadOnlyCollection<IFile> Files { get; }
        Guid Id { get; }
        long Length { get; }
        bool TryGetMD5Checksum(out UInt128 result);
    }
}
