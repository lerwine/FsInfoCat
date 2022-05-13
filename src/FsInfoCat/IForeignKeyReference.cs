using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace FsInfoCat
{
    public interface IForeignKeyReference : IHasSimpleIdentifier, ISynchronizable
    {
        IHasSimpleIdentifier Entity { get; }
        bool HasId();
        void SetId(Guid? id);
        bool HasReference();
        Task<TResult> DeriveAsync<TResult>([DisallowNull] Func<Guid, Task<TResult>> ifHasId, [DisallowNull] Func<Task<TResult>> ifHasNoId);
        Task InvokeAsync([DisallowNull] Func<Guid, Task> ifHasId, [DisallowNull] Func<Task> ifNoId);
        TResult SyncDerive<TResult>([DisallowNull] Func<Guid, TResult> ifHasId, [DisallowNull] Func<TResult> ifHasNoId);
        void SyncInvoke([DisallowNull] Action<Guid> ifHasId, [DisallowNull] Action ifNoId);
        bool TrySyncDerive<TResult>([DisallowNull] Func<Guid, TResult> ifHasId, out TResult result);
        bool TrySyncInvoke([DisallowNull] Action<Guid> ifHasId);
    }

    public interface IForeignKeyReference<TEntity> : IForeignKeyReference, IEquatable<IForeignKeyReference<TEntity>>
        where TEntity : class, IHasSimpleIdentifier, IEquatable<TEntity>
    {
        new TEntity Entity { get; }
        Task<TResult> DeriveAsync<TResult>([DisallowNull] Func<TEntity, Task<TResult>> ifEntityHasId, [DisallowNull] Func<TEntity, Task<TResult>> ifEntityHasNoId, [DisallowNull] Func<Guid, Task<TResult>> ifIdOnly,
            [DisallowNull] Func<Task<TResult>> ifNoReference);
        Task<TResult> DeriveAsync<TResult>([DisallowNull] Func<TEntity, Task<TResult>> ifEntityNotNull, [DisallowNull] Func<Guid, Task<TResult>> ifIdOnly, [DisallowNull] Func<Task<TResult>> ifNoReference);
        Task<TResult> DeriveAsync<TResult>([DisallowNull] Func<TEntity, Task<TResult>> ifEntityNotNull, [DisallowNull] Func<Guid?, Task<TResult>> ifEntityNull);
        Task<TResult> DeriveAsync<TResult>([DisallowNull] Func<Guid, Task<TResult>> ifHasId, [DisallowNull] Func<TEntity, Task<TResult>> ifHasNoId);
        Task InvokeAsync([DisallowNull] Func<TEntity, Task> ifEntityHasId, [DisallowNull] Func<TEntity, Task> ifEntityHasNoId, [DisallowNull] Func<Guid, Task> ifIdOnly, [DisallowNull] Func<Task> ifNoReference);
        Task InvokeAsync([DisallowNull] Func<TEntity, Task> ifEntityNotNull, [DisallowNull] Func<Guid, Task> ifIdOnly, [DisallowNull] Func<Task> ifNoReference);
        Task InvokeAsync([DisallowNull] Func<TEntity, Task> ifEntityNotNull, [DisallowNull] Func<Guid?, Task> ifEntityNull);
        Task InvokeAsync([DisallowNull] Func<Guid, Task> ifHasId, [DisallowNull] Func<TEntity, Task> ifNoId);
        TResult SyncDerive<TResult>([DisallowNull] Func<TEntity, TResult> ifEntityHasId, [DisallowNull] Func<TEntity, TResult> ifEntityHasNoId, [DisallowNull] Func<Guid, TResult> ifIdOnly, [DisallowNull] Func<TResult> ifNoReference);
        TResult SyncDerive<TResult>([DisallowNull] Func<TEntity, TResult> ifEntityNotNull, [DisallowNull] Func<Guid, TResult> ifIdOnly, [DisallowNull] Func<TResult> ifNoReference);
        TResult SyncDerive<TResult>([DisallowNull] Func<TEntity, TResult> ifEntityNotNull, [DisallowNull] Func<Guid?, TResult> ifEntityNull);
        TResult SyncDerive<TResult>([DisallowNull] Func<Guid, TResult> ifHasId, [DisallowNull] Func<TEntity, TResult> ifHasNoId);
        void SyncInvoke([DisallowNull] Action<TEntity> ifEntityHasId, [DisallowNull] Action<TEntity> ifEntityHasNoId, [DisallowNull] Action<Guid> ifIdOnly, [DisallowNull] Action ifNoReference);
        void SyncInvoke([DisallowNull] Action<TEntity> ifEntityNotNull, [DisallowNull] Action<Guid> ifIdOnly, [DisallowNull] Action ifNoReference);
        void SyncInvoke([DisallowNull] Action<TEntity> ifEntityNotNull, [DisallowNull] Action<Guid?> ifEntityNull);
        void SyncInvoke([DisallowNull] Action<Guid> ifHasId, [DisallowNull] Action<TEntity> ifNoId);
        bool TrySyncDerive<TResult>([DisallowNull] Func<TEntity, TResult> ifEntityHasId, [DisallowNull] Func<TEntity, TResult> ifEntityHasNoId, [DisallowNull] Func<Guid, TResult> ifIdOnly, out TResult result);
        bool TrySyncDerive<TResult>([DisallowNull] Func<TEntity, TResult> ifEntityNotNull, [DisallowNull] Func<Guid, TResult> ifIdOnly, out TResult result);
        bool TrySyncDerive<TResult>([DisallowNull] Func<TEntity, TResult> ifEntityHasNoId, [DisallowNull] Func<TResult> ifNoReference, out TResult result);
        bool TrySyncDeriveIfHasId<TResult>([DisallowNull] Func<TEntity, TResult> ifEntityHasId, [DisallowNull] Func<Guid, TResult> ifIdOnly, out TResult result);
        bool TrySyncInvoke([DisallowNull] Action<TEntity> ifEntityHasId, [DisallowNull] Action<TEntity> ifEntityHasNoId, [DisallowNull] Action<Guid> ifIdOnly);
        bool TrySyncInvoke([DisallowNull] Action<TEntity> ifEntityNotNull, [DisallowNull] Action<Guid> ifIdOnly);
        bool TrySyncInvoke([DisallowNull] Action<TEntity> ifEntityHasNoId, [DisallowNull] Action ifNoReference);
        bool TrySyncInvokeIfHasId([DisallowNull] Action<TEntity> ifEntityHasId, [DisallowNull] Action<Guid> ifIdOnly);
    }
}
