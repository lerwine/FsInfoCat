using System;
using System.Threading.Tasks;

namespace FsInfoCat
{
    public interface IForeignKeyReference : IHasSimpleIdentifier, ISynchronizable
    {
        IHasSimpleIdentifier Entity { get; }
        bool HasId();
        void SetId(Guid? id);
    }

    public interface IForeignKeyReference<TEntity> : IForeignKeyReference//, IEquatable<IForeignKeyReference<TEntity>>
        where TEntity : IHasSimpleIdentifier
    {
        new TEntity Entity { get; }

        //Task<TResult> DeriveAsync<TResult>(Func<TEntity, Task<TResult>> ifEntityHasId, Func<TEntity, Task<TResult>> ifEntityHasNoId, Func<Guid, Task<TResult>> ifIdOnly, Func<Task<TResult>> ifNoReference);

        //Task<TResult> DeriveAsync<TResult>(Func<TEntity, Task<TResult>> ifEntityNotNull, Func<Guid, Task<TResult>> ifIdOnly, Func<Task<TResult>> ifNoReference);

        //Task<TResult> DeriveAsync<TResult>(Func<TEntity, Task<TResult>> ifEntityNotNull, Func<Guid?, Task<TResult>> ifEntityNull);

        //Task<TResult> DeriveAsync<TResult>(Func<Guid, Task<TResult>> ifHasId, Func<Task<TResult>> ifHasNoId);

        //Task InvokeAsync(Func<TEntity, Task> ifEntityHasId, Func<TEntity, Task> ifEntityHasNoId, Func<Guid, Task> ifIdOnly, Func<Task> ifNoReference);

        //Task InvokeAsync(Func<TEntity, Task> ifEntityNotNull, Func<Guid, Task> ifIdOnly, Func<Task> ifNoReference);

        //Task InvokeAsync(Func<TEntity, Task> ifEntityNotNull, Func<Guid?, Task> ifEntityNull);

        //Task InvokeAsync(Func<Guid, Task> ifHasId, Func<Task> ifNoId);

        //TResult SyncDerive<TResult>(Func<TEntity, TResult> ifEntityHasId, Func<TEntity, TResult> ifEntityHasNoId, Func<Guid, TResult> ifIdOnly, Func<TResult> ifNoReference);

        //TResult SyncDerive<TResult>(Func<TEntity, TResult> ifEntityNotNull, Func<Guid, TResult> ifIdOnly, Func<TResult> ifNoReference);

        //TResult SyncDerive<TResult>(Func<TEntity, TResult> ifEntityNotNull, Func<Guid?, TResult> ifEntityNull);

        //TResult SyncDerive<TResult>(Func<Guid, TResult> ifHasId, Func<TResult> ifHasNoId);

        //void SyncInvoke(Action<TEntity> ifEntityHasId, Action<TEntity> ifEntityHasNoId, Action<Guid> ifIdOnly, Action ifNoReference);

        //void SyncInvoke(Action<TEntity> ifEntityNotNull, Action<Guid> ifIdOnly, Action ifNoReference);

        //void SyncInvoke(Action<TEntity> ifEntityNotNull, Action<Guid?> ifEntityNull);

        //void SyncInvoke(Action<Guid> ifHasId, Action ifNoId);

        //bool TrySyncDerive<TResult>(Func<TEntity, TResult> ifEntityHasId, Func<TEntity, TResult> ifEntityHasNoId, Func<Guid, TResult> ifIdOnly, out TResult result);

        //bool TrySyncDerive<TResult>(Func<TEntity, TResult> ifEntityNotNull, Func<Guid, TResult> ifIdOnly, out TResult result);

        //bool TrySyncDerive<TResult>(Func<TEntity, TResult> ifEntityHasNoId, Func<TResult> ifNoReference, out TResult result);

        //bool TrySyncDerive<TResult>(Func<Guid, TResult> ifHasId, out TResult result);

        //bool TrySyncDeriveIfHasId<TResult>(Func<TEntity, TResult> ifEntityHasId, Func<Guid, TResult> ifIdOnly, out TResult result);

        //bool TrySyncInvoke(Action<TEntity> ifEntityHasId, Action<TEntity> ifEntityHasNoId, Action<Guid> ifIdOnly);

        //bool TrySyncInvoke(Action<TEntity> ifEntityNotNull, Action<Guid> ifIdOnly);

        //bool TrySyncInvoke(Action<TEntity> ifEntityHasNoId, Action ifNoReference);

        //bool TrySyncInvoke(Action<Guid> ifHasId);

        //bool TrySyncInvokeIfHasId(Action<TEntity> ifEntityHasId, Action<Guid> ifIdOnly);
    }
}
