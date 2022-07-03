using FsInfoCat.Model;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat
{
    // TODO: Document IUserGroupListItem class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class ForeignKeyReference<TEntity> : IForeignKeyReference<TEntity>, IEquatable<ForeignKeyReference<TEntity>>
        where TEntity : class, IHasSimpleIdentifier, IEquatable<TEntity>
    {
        private Guid? _id;
        private TEntity _entity;

        public object SyncRoot { get; }

        public Guid? IdValue
        {
            get
            {
                TEntity entity = _entity;
                return (entity is null) ? _id : entity.TryGetId(out Guid id) ? id : null;
            }
        }

        public Guid Id => _entity?.Id ?? _id ?? Guid.Empty;

        public TEntity Entity
        {
            get => _entity;
            set => this.SyncInvoke(() =>
            {
                _entity = value;
                _id = null;
            });
        }

        IHasSimpleIdentifier IForeignKeyReference.Entity => Entity;

        public ForeignKeyReference(TEntity entity, object syncRoot = null)
        {
            SyncRoot = syncRoot ?? new();
            _entity = entity;
        }

        public ForeignKeyReference(Guid id, object syncRoot = null)
        {
            SyncRoot = syncRoot ?? new();
            _id = id;
        }

        public ForeignKeyReference(object syncRoot) { SyncRoot = syncRoot ?? new(); }

        public bool HasId() => this.SyncDerive(() => (_entity is null) ? _id.HasValue : _entity.TryGetId(out _));

        public void SetId(Guid? id) => this.SyncInvoke(() =>
        {
            if (_entity is not null)
            {
                if (id.HasValue ? _entity.Id.Equals(id.Value) : !_entity.TryGetId(out _)) return;
                _entity = null;
            }
            _id = id;
        });

        /// <summary>
        /// Gets the unique identifier of the associated entity if it has been assigned.
        /// </summary>
        /// <param name="result">Receives the unique identifier value.</param>
        /// <returns><see langword="true" /> if the unique identifier has been set; otherwise, <see langword="false" />.</returns>
        public bool TryGetId(out Guid result)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_entity is not null) return _entity.TryGetId(out result);
                if (_id.HasValue)
                {
                    result = _id.Value;
                    return true;
                }
            }
            finally { Monitor.Exit(SyncRoot); }
            result = Guid.Empty;
            return false;
        }

        public async Task<TResult> DeriveAsync<TResult>([DisallowNull] Func<TEntity, Task<TResult>> ifEntityHasId, [DisallowNull] Func<TEntity, Task<TResult>> ifEntityHasNoId, [DisallowNull] Func<Guid, Task<TResult>> ifIdOnly, [DisallowNull] Func<Task<TResult>> ifNoReference)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_entity is null)
                {
                    if (_id.HasValue) return await ifIdOnly(_id.Value);
                    return await ifNoReference();
                }
                if (_entity.TryGetId(out _)) return await ifEntityHasId(_entity);
                return await ifEntityHasNoId(_entity);
            }
            finally { Monitor.Exit(SyncRoot); }
        }

        public async Task<TResult> DeriveAsync<TResult>([DisallowNull] Func<TEntity, Task<TResult>> ifEntityNotNull, [DisallowNull] Func<Guid, Task<TResult>> ifIdOnly, [DisallowNull] Func<Task<TResult>> ifNoReference)
        {
            Monitor.Enter(SyncRoot);
            try { return await ((_entity is null) ? ((_id.HasValue) ? ifIdOnly(_id.Value) : ifNoReference()) : ifEntityNotNull(_entity)); }
            finally { Monitor.Exit(SyncRoot); }
        }

        public async Task<TResult> DeriveAsync<TResult>([DisallowNull] Func<TEntity, Task<TResult>> ifEntityNotNull, [DisallowNull] Func<Guid?, Task<TResult>> ifEntityNull)
        {
            Monitor.Enter(SyncRoot);
            try { return await ((_entity is null) ? ifEntityNull(_id) : ifEntityNotNull(_entity)); }
            finally { Monitor.Exit(SyncRoot); }
        }

        public async Task<TResult> DeriveAsync<TResult>([DisallowNull] Func<Guid, Task<TResult>> ifHasId, [DisallowNull] Func<Task<TResult>> ifHasNoId)
        {
            Monitor.Enter(SyncRoot);
            try { return await ((_entity is null) ? (_id.HasValue ? ifHasId(_id.Value) : ifHasNoId()) : _entity.TryGetId(out Guid id) ? ifHasId(id) : ifHasNoId()); }
            finally { Monitor.Exit(SyncRoot); }
        }

        public async Task InvokeAsync([DisallowNull] Func<TEntity, Task> ifEntityHasId, [DisallowNull] Func<TEntity, Task> ifEntityHasNoId, [DisallowNull] Func<Guid, Task> ifIdOnly, [DisallowNull] Func<Task> ifNoReference)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_entity is null)
                {
                    if (_id.HasValue)
                        await ifIdOnly(_id.Value);
                    else
                        await ifNoReference();
                }
                else if (_entity.TryGetId(out _))
                    await ifEntityHasId(_entity);
                else
                    await ifEntityHasNoId(_entity);
            }
            finally { Monitor.Exit(SyncRoot); }
        }

        public async Task InvokeAsync([DisallowNull] Func<TEntity, Task> ifEntityNotNull, [DisallowNull] Func<Guid, Task> ifIdOnly, [DisallowNull] Func<Task> ifNoReference)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_entity is not null)
                    await ifEntityNotNull(_entity);
                else if (_id.HasValue)
                    await ifIdOnly(_id.Value);
                else
                    await ifNoReference();
            }
            finally { Monitor.Exit(SyncRoot); }
        }

        public async Task InvokeAsync([DisallowNull] Func<TEntity, Task> ifEntityNotNull, [DisallowNull] Func<Guid?, Task> ifEntityNull)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_entity is not null)
                    await ifEntityNotNull(_entity);
                else if (_id.HasValue)
                    await ifEntityNull(_id);
            }
            finally { Monitor.Exit(SyncRoot); }
        }

        public async Task InvokeAsync([DisallowNull] Func<Guid, Task> ifHasId, [DisallowNull] Func<Task> ifNoId)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_entity is null)
                {
                    if (_id.HasValue)
                        await ifHasId(_id.Value);
                    else
                        await ifNoId();
                }
                else if (_entity.TryGetId(out Guid id))
                    await ifHasId(id);
                else
                    await ifNoId();
            }
            finally { Monitor.Exit(SyncRoot); }
        }

        public TResult SyncDerive<TResult>([DisallowNull] Func<TEntity, TResult> ifEntityHasId, [DisallowNull] Func<TEntity, TResult> ifEntityHasNoId, [DisallowNull] Func<Guid, TResult> ifIdOnly, [DisallowNull] Func<TResult> ifNoReference)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_entity is null)
                {
                    if (_id.HasValue) return ifIdOnly(_id.Value);
                    return ifNoReference();
                }
                if (_entity.TryGetId(out _)) return ifEntityHasId(_entity);
                return ifEntityHasNoId(_entity);
            }
            finally { Monitor.Exit(SyncRoot); }
        }

        public TResult SyncDerive<TResult>([DisallowNull] Func<TEntity, TResult> ifEntityNotNull, [DisallowNull] Func<Guid, TResult> ifIdOnly, [DisallowNull] Func<TResult> ifNoReference)
        {
            Monitor.Enter(SyncRoot);
            try { return (_entity is null) ? ((_id.HasValue) ? ifIdOnly(_id.Value) : ifNoReference()) : ifEntityNotNull(_entity); }
            finally { Monitor.Exit(SyncRoot); }
        }

        public TResult SyncDerive<TResult>([DisallowNull] Func<TEntity, TResult> ifEntityNotNull, [DisallowNull] Func<Guid?, TResult> ifEntityNull)
        {
            Monitor.Enter(SyncRoot);
            try { return (_entity is null) ? ifEntityNull(_id) : ifEntityNotNull(_entity); }
            finally { Monitor.Exit(SyncRoot); }
        }

        public TResult SyncDerive<TResult>([DisallowNull] Func<Guid, TResult> ifHasId, [DisallowNull] Func<TResult> ifHasNoId)
        {
            Monitor.Enter(SyncRoot);
            try { return (_entity is null) ? (_id.HasValue ? ifHasId(_id.Value) : ifHasNoId()) : _entity.TryGetId(out Guid id) ? ifHasId(id) : ifHasNoId(); }
            finally { Monitor.Exit(SyncRoot); }
        }

        public void SyncInvoke([DisallowNull] Action<TEntity> ifEntityHasId, [DisallowNull] Action<TEntity> ifEntityHasNoId, [DisallowNull] Action<Guid> ifIdOnly, [DisallowNull] Action ifNoReference)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_entity is null)
                {
                    if (_id.HasValue)
                        ifIdOnly(_id.Value);
                    else
                        ifNoReference();
                }
                else if (_entity.TryGetId(out _))
                    ifEntityHasId(_entity);
                else
                    ifEntityHasNoId(_entity);
            }
            finally { Monitor.Exit(SyncRoot); }
        }

        public void SyncInvoke([DisallowNull] Action<TEntity> ifEntityNotNull, [DisallowNull] Action<Guid> ifIdOnly, [DisallowNull] Action ifNoReference)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_entity is not null)
                    ifEntityNotNull(_entity);
                else if (_id.HasValue)
                    ifIdOnly(_id.Value);
                else
                    ifNoReference();
            }
            finally { Monitor.Exit(SyncRoot); }
        }

        public void SyncInvoke([DisallowNull] Action<TEntity> ifEntityNotNull, [DisallowNull] Action<Guid?> ifEntityNull)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_entity is not null)
                    ifEntityNotNull(_entity);
                else if (_id.HasValue)
                    ifEntityNull(_id);
            }
            finally { Monitor.Exit(SyncRoot); }
        }

        public void SyncInvoke([DisallowNull] Action<Guid> ifHasId, [DisallowNull] Action ifNoId)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_entity is null)
                {
                    if (_id.HasValue)
                        ifHasId(_id.Value);
                    else
                        ifNoId();
                }
                else if (_entity.TryGetId(out Guid id))
                    ifHasId(id);
                else
                    ifNoId();
            }
            finally { Monitor.Exit(SyncRoot); }
        }

        public bool TrySyncDerive<TResult>([DisallowNull] Func<TEntity, TResult> ifEntityHasId, [DisallowNull] Func<TEntity, TResult> ifEntityHasNoId, [DisallowNull] Func<Guid, TResult> ifIdOnly, out TResult result)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_entity is null)
                {
                    if (_id.HasValue)
                    {
                        result = ifIdOnly(_id.Value);
                        return true;
                    }
                }
                else
                {
                    if (_entity.TryGetId(out _))
                        result = ifEntityHasId(_entity);
                    else
                        result = ifEntityHasNoId(_entity);
                    return true;
                }
            }
            finally { Monitor.Exit(SyncRoot); }
            result = default;
            return false;
        }

        public bool TrySyncDerive<TResult>([DisallowNull] Func<TEntity, TResult> ifEntityNotNull, [DisallowNull] Func<Guid, TResult> ifIdOnly, out TResult result)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_entity is not null)
                    result = ifEntityNotNull(_entity);
                else if (_id.HasValue)
                    result = ifIdOnly(_id.Value);
                else
                {
                    result = default;
                    return false;
                }
            }
            finally { Monitor.Exit(SyncRoot); }
            return true;
        }

        public bool TrySyncDerive<TResult>([DisallowNull] Func<TEntity, TResult> ifEntityHasNoId, [DisallowNull] Func<TResult> ifNoReference, out TResult result)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_entity is not null)
                    result = ifEntityHasNoId(_entity);
                else if (!_id.HasValue)
                    result = ifNoReference();
                else
                {
                    result = default;
                    return false;
                }
            }
            finally { Monitor.Exit(SyncRoot); }
            return true;
        }

        public bool TrySyncDerive<TResult>([DisallowNull] Func<Guid, TResult> ifHasId, out TResult result)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_entity is null)
                {
                    if (_id.HasValue)
                    {
                        result = ifHasId(_id.Value);
                        return true;
                    }
                }
                else if (_entity.TryGetId(out Guid id))
                {
                    result = ifHasId(id);
                    return true;
                }
            }
            finally { Monitor.Exit(SyncRoot); }
            result = default;
            return false;
        }

        public bool TrySyncDeriveIfHasId<TResult>([DisallowNull] Func<TEntity, TResult> ifEntityHasId, [DisallowNull] Func<Guid, TResult> ifIdOnly, out TResult result)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_entity is null)
                {
                    if (_id.HasValue)
                    {
                        result = ifIdOnly(_id.Value);
                        return true;
                    }
                }
                else if (_entity.TryGetId(out _))
                {
                    result = ifEntityHasId(_entity);
                    return true;
                }
            }
            finally { Monitor.Exit(SyncRoot); }
            result = default;
            return false;
        }

        public bool TrySyncInvoke([DisallowNull] Action<TEntity> ifEntityHasId, [DisallowNull] Action<TEntity> ifEntityHasNoId, [DisallowNull] Action<Guid> ifIdOnly)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_entity is null)
                {
                    if (_id.HasValue)
                        ifIdOnly(_id.Value);
                    else
                        return false;
                }
                else if (_entity.TryGetId(out _))
                    ifEntityHasId(_entity);
                else
                    ifEntityHasNoId(_entity);
            }
            finally { Monitor.Exit(SyncRoot); }
            return true;
        }

        public bool TrySyncInvoke([DisallowNull] Action<TEntity> ifEntityNotNull, [DisallowNull] Action<Guid> ifIdOnly)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_entity is not null)
                    ifEntityNotNull(_entity);
                else if (_id.HasValue)
                    ifIdOnly(_id.Value);
                else
                    return false;
            }
            finally { Monitor.Exit(SyncRoot); }
            return true;
        }

        public bool TrySyncInvoke([DisallowNull] Action<TEntity> ifEntityHasNoId, [DisallowNull] Action ifNoReference)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_entity is null)
                {
                    if (!_id.HasValue)
                    {
                        ifNoReference();
                        return true;
                    }
                }
                else if (!_entity.TryGetId(out _))
                {
                    ifEntityHasNoId(_entity);
                    return true;
                }
            }
            finally { Monitor.Exit(SyncRoot); }
            return false;
        }

        public bool TrySyncInvoke([DisallowNull] Action<Guid> ifHasId)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_entity is null)
                {
                    if (_id.HasValue)
                    {
                        ifHasId(_id.Value);
                        return true;
                    }
                }
                else if (_entity.TryGetId(out Guid id))
                {
                    ifHasId(id);
                    return true;
                }
            }
            finally { Monitor.Exit(SyncRoot); }
            return false;
        }

        public bool TrySyncInvokeIfHasId([DisallowNull] Action<TEntity> ifEntityHasId, [DisallowNull] Action<Guid> ifIdOnly)
        {
            Monitor.Enter(SyncRoot);
            try
            {
                if (_entity is null)
                {
                    if (_id.HasValue)
                    {
                        ifIdOnly(_id.Value);
                        return true;
                    }
                }
                else if (_entity.TryGetId(out _))
                {
                    ifEntityHasId(_entity);
                    return true;
                }
            }
            finally { Monitor.Exit(SyncRoot); }
            return false;
        }

        public bool Equals(ForeignKeyReference<TEntity> other)
        {
            if (other is null) return false;
            if (ReferenceEquals(this, other)) return true;
            Monitor.Enter(SyncRoot);
            try
            {
                if (_entity is null)
                {
                    if (_id.HasValue)
                        return other.TryGetId(out Guid id) && id.Equals(_id.Value);
                    return other._entity is null && !other._id.HasValue;
                }
                if (_entity.TryGetId(out Guid g))
                    return other.TryGetId(out Guid id) && id.Equals(g);
                return other._entity is not null && _entity.Equals(other._entity);
            }
            finally { Monitor.Exit(SyncRoot); }
        }

        public override bool Equals(object obj) => obj is IForeignKeyReference<TEntity> other && Equals(other);

        public override int GetHashCode() => _entity?.GetHashCode() ?? _id?.GetHashCode() ?? 0;

        public bool Equals(IForeignKeyReference<TEntity> other)
        {
            throw new NotImplementedException();
        }
    }

    public class ForeignKeyReference<TBase, TEntity> : ForeignKeyReference<TEntity>, IForeignKeyReference<TBase>
        where TBase : class, IHasSimpleIdentifier, IEquatable<TBase>
        where TEntity : class, TBase, IEquatable<TEntity>
    {
        TBase IForeignKeyReference<TBase>.Entity => Entity;

        public ForeignKeyReference(TEntity entity, object syncRoot = null) : base(entity, syncRoot) { }

        public ForeignKeyReference(Guid id, object syncRoot = null) : base(id, syncRoot) { }

        public ForeignKeyReference(object syncRoot) : base(syncRoot) { }

        bool IEquatable<IForeignKeyReference<TBase>>.Equals(IForeignKeyReference<TBase> other)
        {
            throw new NotImplementedException();
        }
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
