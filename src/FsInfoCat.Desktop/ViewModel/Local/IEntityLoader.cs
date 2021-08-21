using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    public interface IEntityLoader<TDbEntity, TVm, TLoadResult>
        where TDbEntity : LocalDbEntity, new()
        where TVm : EditDbEntityVM<TDbEntity>, new()
    {
        string LoadingTitle { get; }

        string InitialLoadingMessage { get; }

        TDbEntity GetEntity(TLoadResult loadResult);

        Task<TLoadResult> LoadAsync(LocalDbContext dbContext, IWindowsStatusListener statusListener);

        void InitializeViewModel(TVm viewModel, TLoadResult loadResult, EntityState entityState);
    }
}
