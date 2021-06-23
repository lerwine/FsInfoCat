using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public interface IFileSystemDetailService
    {
        IFileDetailProvider CreateFileDetailProvider(string filePath);
        Task<Volume> GetVolumeAsync(DirectoryInfo directoryInfo, LocalDbContext dbContext, CancellationToken cancellationToken);
    }
}
