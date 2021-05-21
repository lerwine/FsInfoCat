using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public interface ILocalVolume : IVolume, ILocalDbEntity
    {
        new ILocalFileSystem FileSystem { get; }
    }
}
