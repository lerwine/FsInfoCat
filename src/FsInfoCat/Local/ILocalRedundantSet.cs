using System.Collections.Generic;

namespace FsInfoCat.Local
{
    public interface ILocalRedundantSet : IRedundantSet, ILocalDbEntity
    {
        new ILocalBinaryPropertySet BinaryProperties { get; set; }

        new IEnumerable<ILocalRedundancy> Redundancies { get; }
    }
}
