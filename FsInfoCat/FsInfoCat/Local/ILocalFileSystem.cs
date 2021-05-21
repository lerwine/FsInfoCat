using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public interface ILocalFileSystem : IFileSystem, ILocalDbEntity
    {
        new IEnumerable<ILocalVolume> Volumes { get; }

        new IEnumerable<ILocalSymbolicName> SymbolicNames { get; }
    }
}
