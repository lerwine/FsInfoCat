using FsInfoCat.Background;
using System.IO;
using System.Threading.Tasks;

namespace FsInfoCat.Local.Background
{
    public interface IImportBranchBackgroundService : ILongRunningAsyncService
    {
        DirectoryInfo Source { get; set; }

        new Task<ISubdirectory> Task { get; }
    }

    public interface IImportBranchBackgroundService<TSubdirectory> : ILongRunningAsyncService<TSubdirectory>, IImportBranchBackgroundService
        where TSubdirectory : DbEntity, ISubdirectory
    {
    }
}
