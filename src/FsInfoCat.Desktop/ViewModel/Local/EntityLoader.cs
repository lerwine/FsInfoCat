using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.ViewModel.Local
{
    class EntityLoader<TDbEntity, TVm> : IEntityLoader<TDbEntity, TVm, TDbEntity>
        where TDbEntity : LocalDbEntity, new()
        where TVm : EditDbEntityVM<TDbEntity>, new()
    {
        public string LoadingTitle { get; }

        public string InitialLoadingMessage { get; }

        private readonly Func<LocalDbContext, AsyncOps.IStatusListener, Task<TDbEntity>> _getDbEntityOpAsync;

        internal EntityLoader([DisallowNull] string loadingTitle, [DisallowNull] string initialLoadingMessage,
            [DisallowNull] Func<LocalDbContext, AsyncOps.IStatusListener, Task<TDbEntity>> getDbEntityOpAsync)
        {
            if (string.IsNullOrWhiteSpace(loadingTitle))
                throw new ArgumentException($"'{nameof(loadingTitle)}' cannot be null or whitespace.", nameof(loadingTitle));
            if (string.IsNullOrWhiteSpace(initialLoadingMessage))
                throw new ArgumentException($"'{nameof(initialLoadingMessage)}' cannot be null or whitespace.", nameof(initialLoadingMessage));
            if (getDbEntityOpAsync is null)
                throw new ArgumentNullException(nameof(getDbEntityOpAsync));
            LoadingTitle = loadingTitle;
            InitialLoadingMessage = initialLoadingMessage;
            _getDbEntityOpAsync = getDbEntityOpAsync;
        }

        public TDbEntity GetEntity(TDbEntity loadResult) => loadResult;

        public void InitializeViewModel(TVm viewModel, TDbEntity loadResult, EntityState entityState)
        {
            throw new NotImplementedException();
        }

        public Task<TDbEntity> LoadAsync(LocalDbContext dbContext, AsyncOps.IStatusListener statusListener) => _getDbEntityOpAsync(dbContext, statusListener);
    }
}
