using FsInfoCat.Local;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop
{
    public interface IHasSubdirectoryEntity
    {
        ISimpleIdentityReference<Subdirectory> GetSubdirectoryEntity();

        Task<ISimpleIdentityReference<Subdirectory>> GetSubdirectoryEntityAsync([DisallowNull] IWindowsStatusListener statusListener);
    }
}
