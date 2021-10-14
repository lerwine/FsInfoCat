using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Background
{
    public interface IMarkBranchIncompleteBackgroundService : ILongRunningAsyncService<bool>
    {
    }

    public interface IDeleteBranchBackgroundService : ILongRunningAsyncService<bool>
    {
    }

    public interface IDeleteVolumeBackgroundService : ILongRunningAsyncService<bool>
    {
    }
}
