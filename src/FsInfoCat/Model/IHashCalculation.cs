using System;
using System.Collections.Generic;

namespace FsInfoCat.Model
{
    public interface IHashCalculation : ITimeStampedEntity
    {
        IReadOnlyCollection<byte> Data { get; }
        IReadOnlyCollection<IFile> Files { get; }
        Guid Id { get; }
        long Length { get; }
        bool TryGetMD5Checksum(out UInt128 result);
    }
}
