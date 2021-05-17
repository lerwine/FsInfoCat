using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Model
{
    public interface IHashCalculation : ITimeStampedEntity, IValidatableObject
    {
        IReadOnlyCollection<byte> Data { get; }
        IReadOnlyCollection<IFile> Files { get; }
        Guid Id { get; }
        long Length { get; }
        bool TryGetMD5Checksum(out UInt128 result);
    }
}
