using FsInfoCat.Model;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat;

/// <summary>
/// Represents a foreign key identifier and the optional associated nagivation property.
/// </summary>
/// <typeparam name="TEntity">The enity type for the navigation property.</typeparam>
public class ForeignKeyReference<TEntity> : IForeignKeyReference<TEntity>, IEquatable<ForeignKeyReference<TEntity>>
    where TEntity : class, IHasSimpleIdentifier, IEquatable<TEntity>
{
    private Guid? _id;
    private TEntity _entity;

    /// <summary>
    /// Gets the object that is to be used to synchronize access to the current object.
    /// </summary>
    public object SyncRoot { get; }

    /// <summary>
    /// The unique identifer of the related record or <see langword="null"/> if there is no related entity.
    /// </summary>
    public Guid? IdValue
    {
        get
        {
            TEntity entity = _entity;
            return (entity is null) ? _id : entity.TryGetId(out Guid id) ? id : null;
        }
    }

    /// <summary>
    /// Gets the primary key value of the related entity.
    /// </summary>
    [Display(Name = nameof(Properties.Resources.UniqueIdentifier), ResourceType = typeof(Properties.Resources))]
    public Guid Id => _entity?.Id ?? _id ?? Guid.Empty;

    /// <summary>
    /// Gets the navigation entity object.
    /// </summary>
    /// <value>The navigation entity object or <see langword="null"/> if the entity has not been set.</value>
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

    /// <summary>
    /// Initializes a new <c>ForeignKeyReference</c> object from an existing entity.
    /// </summary>
    /// <param name="entity">The related entity.</param>
    /// <param name="syncRoot">The optional object that is to be used to synchronize access to the new <c>ForeignKeyReference</c> object.</param>
    public ForeignKeyReference([DisallowNull] TEntity entity, object syncRoot = null)
    {
        SyncRoot = syncRoot ?? new();
        _entity = entity;
    }

    /// <summary>
    /// Initializes a new <c>ForeignKeyReference</c> object from a related record unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the related record that is of type <typeparamref name="TEntity"/>.</param>
    /// <param name="syncRoot">The optional object that is to be used to synchronize access to the new <c>ForeignKeyReference</c> object.</param>
    public ForeignKeyReference(Guid id, object syncRoot = null)
    {
        SyncRoot = syncRoot ?? new();
        _id = id;
    }

    /// <summary>
    /// Initializes a new <c>ForeignKeyReference</c> object that has no related record.
    /// </summary>
    /// <param name="syncRoot">The optional object that is to be used to synchronize access to the new <c>ForeignKeyReference</c> object.</param>
    public ForeignKeyReference(object syncRoot) { SyncRoot = syncRoot ?? new(); }

    /// <summary>
    /// Determines whether the foreign key value or the primary key of the navigation property has been set.
    /// </summary>
    /// <returns><see langword="true"/> foreign key value has been set or the primary key of the navigation object has been set;
    /// otherwise, <see langword="false"/>.</returns>
    public bool HasId() => this.SyncDerive(() => (_entity is null) ? _id.HasValue : _entity.TryGetId(out _));

    /// <summary>
    /// Sets the foreign key value.
    /// </summary>
    /// <param name="id">The foreign key value or <see langword="null"/> to unset the foreign key reference.</param>
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

    /// <summary>
    /// Asynchonously returns the value produced by a delegate with synchronized access to the current <c>ForeignKeyReference</c> object.
    /// </summary>
    /// <param name="ifEntityHasId">Delegate which produces the return value when <see cref="Entity"/> is not <see langword="null"/> and its <see cref="IHasSimpleIdentifier.Id"/> has been set.</param>
    /// <param name="ifEntityHasNoId">Delegate which produces the return value when <see cref="Entity"/> is not <see langword="null"/>, but its <see cref="IHasSimpleIdentifier.Id"/> has not been set.</param>
    /// <param name="ifIdOnly">Delegate which produces the return value when <see cref="Entity"/> is <see langword="null"/>, but <see cref="IdValue"/> is not <see langword="null"/>.</param>
    /// <param name="ifNoReference">Delegate which produces the return value when both <see cref="Entity"/> and <see cref="IdValue"/> are <see langword="null"/>.</param>
    /// <returns>A <see cref="Task{TResult}"/> object that returns the value that was produced by either <paramref name="ifEntityHasId"/>, <paramref name="ifEntityHasNoId"/>, <paramref name="ifIdOnly"/>, or <paramref name="ifNoReference"/>.</returns>
    public async Task<TResult> DeriveAsync<TResult>([DisallowNull] Func<TEntity, Task<TResult>> ifEntityHasId, [DisallowNull] Func<TEntity, Task<TResult>> ifEntityHasNoId,
        [DisallowNull] Func<Guid, Task<TResult>> ifIdOnly, [DisallowNull] Func<Task<TResult>> ifNoReference)
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

    /// <summary>
    /// Asynchonously returns the value produced by a delegate with synchronized access to the current <c>ForeignKeyReference</c> object.
    /// </summary>
    /// <param name="ifEntityNotNull">Delegate which produces the return value when <see cref="Entity"/> is not <see langword="null"/>.</param>
    /// <param name="ifIdOnly">Delegate which produces the return value when <see cref="Entity"/> is <see langword="null"/>, but <see cref="IdValue"/> is not <see langword="null"/>.</param>
    /// <param name="ifNoReference">Delegate which produces the return value when both <see cref="Entity"/> and <see cref="IdValue"/> are <see langword="null"/>.</param>
    /// <returns>A <see cref="Task{TResult}"/> object that returns the value that was produced by either <paramref name="ifEntityNotNull"/>, <paramref name="ifIdOnly"/>, or <paramref name="ifNoReference"/>.</returns>
    public async Task<TResult> DeriveAsync<TResult>([DisallowNull] Func<TEntity, Task<TResult>> ifEntityNotNull, [DisallowNull] Func<Guid, Task<TResult>> ifIdOnly, [DisallowNull] Func<Task<TResult>> ifNoReference)
    {
        Monitor.Enter(SyncRoot);
        try { return await ((_entity is null) ? (_id.HasValue ? ifIdOnly(_id.Value) : ifNoReference()) : ifEntityNotNull(_entity)); }
        finally { Monitor.Exit(SyncRoot); }
    }

    /// <summary>
    /// Asynchonously returns the value produced by a delegate with synchronized access to the current <c>ForeignKeyReference</c> object.
    /// </summary>
    /// <param name="ifEntityNotNull">Delegate which produces the return value when <see cref="Entity"/> is not <see langword="null"/>.</param>
    /// <param name="ifEntityNull">Delegate which produces the return value when <see cref="Entity"/> is <see langword="null"/>.</param>
    /// <returns>A <see cref="Task{TResult}"/> object that returns the value that was produced by either <paramref name="ifEntityNotNull"/> or <paramref name="ifEntityNull"/>.</returns>
    public async Task<TResult> DeriveAsync<TResult>([DisallowNull] Func<TEntity, Task<TResult>> ifEntityNotNull, [DisallowNull] Func<Guid?, Task<TResult>> ifEntityNull)
    {
        Monitor.Enter(SyncRoot);
        try { return await ((_entity is null) ? ifEntityNull(_id) : ifEntityNotNull(_entity)); }
        finally { Monitor.Exit(SyncRoot); }
    }

    /// <summary>
    /// Asynchonously returns the value produced by a delegate with synchronized access to the current <c>ForeignKeyReference</c> object.
    /// </summary>
    /// <param name="ifHasId">Delegate which produces the return value when <see cref="IdValue"/> is not <see langword="null"/>.</param>
    /// <param name="ifHasNoId">Delegate which produces the return value when <see cref="IdValue"/> not <see langword="null"/>.</param>
    /// <returns>A <see cref="Task{TResult}"/> object that returns the value that was produced by either <paramref name="ifHasId"/> or <paramref name="ifHasNoId"/>.</returns>
    public async Task<TResult> DeriveAsync<TResult>([DisallowNull] Func<Guid, Task<TResult>> ifHasId, [DisallowNull] Func<Task<TResult>> ifHasNoId)
    {
        Monitor.Enter(SyncRoot);
        try { return await ((_entity is null) ? (_id.HasValue ? ifHasId(_id.Value) : ifHasNoId()) : _entity.TryGetId(out Guid id) ? ifHasId(id) : ifHasNoId()); }
        finally { Monitor.Exit(SyncRoot); }
    }

    /// <summary>
    /// Asynchronously invokes a delegate with synchronized access to the current <c>ForeignKeyReference</c> object.
    /// </summary>
    /// <param name="ifEntityHasId">Delegate that is invoked when <see cref="Entity"/> is not <see langword="null"/> and its <see cref="IHasSimpleIdentifier.Id"/> has been set.</param>
    /// <param name="ifEntityHasNoId">Delegate that is invoked when <see cref="Entity"/> is not <see langword="null"/>, but its <see cref="IHasSimpleIdentifier.Id"/> has not been set.</param>
    /// <param name="ifIdOnly">Delegate that is invoked when <see cref="Entity"/> is <see langword="null"/>, but <see cref="IdValue"/> is not <see langword="null"/>.</param>
    /// <param name="ifNoReference">Delegate that is invoked when both <see cref="Entity"/> and <see cref="IdValue"/> are <see langword="null"/>.</param>
    /// <returns>A <see cref="Task"/> object for the asynchronous invocation of either <paramref name="ifEntityHasId"/>, <paramref name="ifEntityHasNoId"/>, <paramref name="ifIdOnly"/>, or <paramref name="ifNoReference"/>.</returns>
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

    /// <summary>
    /// Asynchronously invokes a delegate with synchronized access to the current <c>ForeignKeyReference</c> object.
    /// </summary>
    /// <param name="ifEntityNotNull">Delegate that is invoked when <see cref="Entity"/> is not <see langword="null"/>.</param>
    /// <param name="ifIdOnly">Delegate that is invoked when <see cref="Entity"/> is <see langword="null"/>, but <see cref="IdValue"/> is not <see langword="null"/>.</param>
    /// <param name="ifNoReference">Delegate that is invoked when both <see cref="Entity"/> and <see cref="IdValue"/> are <see langword="null"/>.</param>
    /// <returns>A <see cref="Task"/> object for the asynchronous invocation of either <paramref name="ifEntityNotNull"/>, <paramref name="ifIdOnly"/>, or <paramref name="ifNoReference"/>.</returns>
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

    /// <summary>
    /// Asynchronously invokes a delegate with synchronized access to the current <c>ForeignKeyReference</c> object.
    /// </summary>
    /// <param name="ifEntityNotNull">Delegate which produces the return value when <see cref="Entity"/> is not <see langword="null"/>.</param>
    /// <param name="ifEntityNull">Delegate which produces the return value when <see cref="Entity"/> is <see langword="null"/>.</param>
    /// <returns>A <see cref="Task"/> object for the asynchronous invocation of either <paramref name="ifEntityNotNull"/> or <paramref name="ifEntityNull"/>.</returns>
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

    /// <summary>
    /// Asynchronously invokes a delegate with synchronized access to the current <c>ForeignKeyReference</c> object.
    /// </summary>
    /// <param name="ifHasId">Delegate which produces the return value when <see cref="IdValue"/> is not <see langword="null"/>.</param>
    /// <param name="ifNoId">Delegate which produces the return value when <see cref="IdValue"/> not <see langword="null"/>.</param>
    /// <returns>A <see cref="Task"/> object for the asynchronous invocation of either <paramref name="ifHasId"/> or <paramref name="ifNoId"/>.</returns>
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

    /// <summary>
    /// Returns the value produced by a delegate with synchronized access to the current <c>ForeignKeyReference</c> object.
    /// </summary>
    /// <param name="ifEntityHasId">Delegate which produces the return value when <see cref="Entity"/> is not <see langword="null"/> and its <see cref="IHasSimpleIdentifier.Id"/> has been set.</param>
    /// <param name="ifEntityHasNoId">Delegate which produces the return value when <see cref="Entity"/> is not <see langword="null"/>, but its <see cref="IHasSimpleIdentifier.Id"/> has not been set.</param>
    /// <param name="ifIdOnly">Delegate which produces the return value when <see cref="Entity"/> is <see langword="null"/>, but <see cref="IdValue"/> is not <see langword="null"/>.</param>
    /// <param name="ifNoReference">Delegate which produces the return value when both <see cref="Entity"/> and <see cref="IdValue"/> are <see langword="null"/>.</param>
    /// <returns>The value that was produced by either <paramref name="ifEntityHasId"/>, <paramref name="ifEntityHasNoId"/>, <paramref name="ifIdOnly"/>, or <paramref name="ifNoReference"/>.</returns>
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

    /// <summary>
    /// Returns the value produced by a delegate with synchronized access to the current <c>ForeignKeyReference</c> object.
    /// </summary>
    /// <param name="ifEntityNotNull">Delegate which produces the return value when <see cref="Entity"/> is not <see langword="null"/>.</param>
    /// <param name="ifIdOnly">Delegate which produces the return value when <see cref="Entity"/> is <see langword="null"/>, but <see cref="IdValue"/> is not <see langword="null"/>.</param>
    /// <param name="ifNoReference">Delegate which produces the return value when both <see cref="Entity"/> and <see cref="IdValue"/> are <see langword="null"/>.</param>
    /// <returns>The value that was produced by either <paramref name="ifEntityNotNull"/>, <paramref name="ifIdOnly"/>, or <paramref name="ifNoReference"/>.</returns>
    public TResult SyncDerive<TResult>([DisallowNull] Func<TEntity, TResult> ifEntityNotNull, [DisallowNull] Func<Guid, TResult> ifIdOnly, [DisallowNull] Func<TResult> ifNoReference)
    {
        Monitor.Enter(SyncRoot);
        try { return (_entity is null) ? (_id.HasValue ? ifIdOnly(_id.Value) : ifNoReference()) : ifEntityNotNull(_entity); }
        finally { Monitor.Exit(SyncRoot); }
    }

    /// <summary>
    /// Returns the value produced by a delegate with synchronized access to the current <c>ForeignKeyReference</c> object.
    /// </summary>
    /// <param name="ifEntityNotNull">Delegate which produces the return value when <see cref="Entity"/> is not <see langword="null"/>.</param>
    /// <param name="ifEntityNull">Delegate which produces the return value when <see cref="Entity"/> is <see langword="null"/>.</param>
    /// <returns>The value that was produced by either <paramref name="ifEntityNotNull"/> or <paramref name="ifEntityNull"/>.</returns>
    public TResult SyncDerive<TResult>([DisallowNull] Func<TEntity, TResult> ifEntityNotNull, [DisallowNull] Func<Guid?, TResult> ifEntityNull)
    {
        Monitor.Enter(SyncRoot);
        try { return (_entity is null) ? ifEntityNull(_id) : ifEntityNotNull(_entity); }
        finally { Monitor.Exit(SyncRoot); }
    }

    /// <summary>
    /// Returns the value produced by a delegate with synchronized access to the current <c>ForeignKeyReference</c> object.
    /// </summary>
    /// <param name="ifHasId">Delegate which produces the return value when <see cref="IdValue"/> is not <see langword="null"/>.</param>
    /// <param name="ifHasNoId">Delegate which produces the return value when <see cref="IdValue"/> not <see langword="null"/>.</param>
    /// <returns>The value that was produced by either <paramref name="ifHasId"/> or <paramref name="ifHasNoId"/>.</returns>
    public TResult SyncDerive<TResult>([DisallowNull] Func<Guid, TResult> ifHasId, [DisallowNull] Func<TResult> ifHasNoId)
    {
        Monitor.Enter(SyncRoot);
        try { return (_entity is null) ? (_id.HasValue ? ifHasId(_id.Value) : ifHasNoId()) : _entity.TryGetId(out Guid id) ? ifHasId(id) : ifHasNoId(); }
        finally { Monitor.Exit(SyncRoot); }
    }

    /// <summary>
    /// Invokes a delegate with synchronized access to the current <c>ForeignKeyReference</c> object.
    /// </summary>
    /// <param name="ifEntityHasId">Delegate that is invoked when <see cref="Entity"/> is not <see langword="null"/> and its <see cref="IHasSimpleIdentifier.Id"/> has been set.</param>
    /// <param name="ifEntityHasNoId">Delegate that is invoked when <see cref="Entity"/> is not <see langword="null"/>, but its <see cref="IHasSimpleIdentifier.Id"/> has not been set.</param>
    /// <param name="ifIdOnly">Delegate that is invoked when <see cref="Entity"/> is <see langword="null"/>, but <see cref="IdValue"/> is not <see langword="null"/>.</param>
    /// <param name="ifNoReference">Delegate that is invoked when both <see cref="Entity"/> and <see cref="IdValue"/> are <see langword="null"/>.</param>
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

    /// <summary>
    /// Invokes a delegate with synchronized access to the current <c>ForeignKeyReference</c> object.
    /// </summary>
    /// <param name="ifEntityNotNull">Delegate that is invoked when <see cref="Entity"/> is not <see langword="null"/>.</param>
    /// <param name="ifIdOnly">Delegate that is invoked when <see cref="Entity"/> is <see langword="null"/>, but <see cref="IdValue"/> is not <see langword="null"/>.</param>
    /// <param name="ifNoReference">Delegate that is invoked when both <see cref="Entity"/> and <see cref="IdValue"/> are <see langword="null"/>.</param>
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

    /// <summary>
    /// Invokes a delegate with synchronized access to the current <c>ForeignKeyReference</c> object.
    /// </summary>
    /// <param name="ifEntityNotNull">Delegate which produces the return value when <see cref="Entity"/> is not <see langword="null"/>.</param>
    /// <param name="ifEntityNull">Delegate which produces the return value when <see cref="Entity"/> is <see langword="null"/>.</param>
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

    /// <summary>
    /// Invokes a delegate with synchronized access to the current <c>ForeignKeyReference</c> object.
    /// </summary>
    /// <param name="ifHasId">Delegate which produces the return value when <see cref="IdValue"/> is not <see langword="null"/>.</param>
    /// <param name="ifNoId">Delegate which produces the return value when <see cref="IdValue"/> not <see langword="null"/>.</param>
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

    /// <summary>
    /// Returns the value produced by a delegate with synchronized access to the current <c>ForeignKeyReference</c> object if it has a related entity.
    /// </summary>
    /// <param name="ifEntityHasId">Delegate which produces the return value when <see cref="Entity"/> is not <see langword="null"/> and its <see cref="IHasSimpleIdentifier.Id"/> has been set.</param>
    /// <param name="ifEntityHasNoId">Delegate which produces the return value when <see cref="Entity"/> is not <see langword="null"/>, but its <see cref="IHasSimpleIdentifier.Id"/> has not been set.</param>
    /// <param name="ifIdOnly">Delegate which produces the return value when <see cref="Entity"/> is <see langword="null"/>, but <see cref="IdValue"/> is not <see langword="null"/>.</param>
    /// <param name="result">When this method returns <see langword="true"/>, the value that was produced by either <paramref name="ifEntityHasId"/>, <paramref name="ifEntityHasNoId"/>, or <paramref name="ifIdOnly"/>.</param>
    /// <returns><see langword="true"/> if either <see cref="Entity"/> or <see cref="IdValue"/> was not <see langword="null"/>; otherwise, <see langword="false"/> if no delegates were called.</returns>
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

    /// <summary>
    /// Returns the value produced by a delegate with synchronized access to the current <c>ForeignKeyReference</c> object if it has a related entity.
    /// </summary>
    /// <param name="ifEntityNotNull">Delegate that is invoked when <see cref="Entity"/> is not <see langword="null"/>.</param>
    /// <param name="ifIdOnly">Delegate that is invoked when <see cref="Entity"/> is <see langword="null"/>, but <see cref="IdValue"/> is not <see langword="null"/>.</param>
    /// <param name="result">When this method returns <see langword="true"/>, the value that was produced by either <paramref name="ifEntityNotNull"/> or <paramref name="ifIdOnly"/>.</param>
    /// <returns><see langword="true"/> if either <see cref="Entity"/> or <see cref="IdValue"/> was not <see langword="null"/>; otherwise, <see langword="false"/> if no delegates were called.</returns>
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

    /// <summary>
    /// Returns the value produced by a delegate with synchronized access to the current <c>ForeignKeyReference</c> object if it has no unique identifier.
    /// </summary>
    /// <param name="ifEntityHasNoId">Delegate which produces the return value when <see cref="Entity"/> is not <see langword="null"/>, but its <see cref="IHasSimpleIdentifier.Id"/> has not been set.</param>
    /// <param name="ifNoReference">Delegate which produces the return value when both <see cref="Entity"/> and <see cref="IdValue"/> are <see langword="null"/>.</param>
    /// <param name="result">When this method returns <see langword="true"/>, the value that was produced by either <paramref name="ifEntityHasNoId"/> or <paramref name="ifNoReference"/>.</param>
    /// <returns><see langword="true"/> if <see cref="IdValue"/> was <see langword="null"/>; otherwise, <see langword="false"/> if no delegates were called.</returns>
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

    /// <summary>
    /// Returns the value produced by a delegate with synchronized access to the current <c>ForeignKeyReference</c> object if it has a unique identifier.
    /// </summary>
    /// <param name="ifHasId">Delegate which produces the return value when either <see cref="Entity"/> is not null and its its <see cref="IHasSimpleIdentifier.Id"/> has been set or <see cref="IdValue"/> is not <see langword="null"/>.</param>
    /// <param name="result">When this method returns <see langword="true"/>, the value that was produced by either <paramref name="ifHasId"/>.</param>
    /// <returns><see langword="true"/> if <see cref="IdValue"/> was not <see langword="null"/>; otherwise, <see langword="false"/> if no delegates were called.</returns>
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

    /// <summary>
    /// Returns the value produced by a delegate with synchronized access to the current <c>ForeignKeyReference</c> object if it has a unique identifier.
    /// </summary>
    /// <param name="ifEntityHasId">Delegate which produces the return value when <see cref="Entity"/> is not <see langword="null"/>, and its <see cref="IHasSimpleIdentifier.Id"/> has been set.</param>
    /// <param name="ifIdOnly">Delegate which produces the return value when both <see cref="Entity"/> is <see langword="null"/> and <see cref="IdValue"/> is not <see langword="null"/>.</param>
    /// <param name="result">When this method returns <see langword="true"/>, the value that was produced by either <paramref name="ifEntityHasId"/> or <paramref name="ifIdOnly"/>.</param>
    /// <returns><see langword="true"/> if <see cref="IdValue"/> was not <see langword="null"/>; otherwise, <see langword="false"/> if no delegates were called.</returns>
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

    /// <summary>
    /// Invokes a delegate with synchronized access to the current <c>ForeignKeyReference</c> object if it has a related entity.
    /// </summary>
    /// <param name="ifEntityHasId">Delegate which produces the return value when <see cref="Entity"/> is not <see langword="null"/> and its <see cref="IHasSimpleIdentifier.Id"/> has been set.</param>
    /// <param name="ifEntityHasNoId">Delegate which produces the return value when <see cref="Entity"/> is not <see langword="null"/>, but its <see cref="IHasSimpleIdentifier.Id"/> has not been set.</param>
    /// <param name="ifIdOnly">Delegate which produces the return value when <see cref="Entity"/> is <see langword="null"/>, but <see cref="IdValue"/> is not <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if either <paramref name="ifEntityHasId"/>, <paramref name="ifEntityHasNoId"/>, or <paramref name="ifIdOnly"/> was invoked; otherwise, <see langword="false"/> if no delegates were invoked.</returns>
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

    /// <summary>
    /// Invokes a delegate with synchronized access to the current <c>ForeignKeyReference</c> object if it has a related entity.
    /// </summary>
    /// <param name="ifEntityNotNull">Delegate that is invoked when <see cref="Entity"/> is not <see langword="null"/>.</param>
    /// <param name="ifIdOnly">Delegate that is invoked when <see cref="Entity"/> is <see langword="null"/>, but <see cref="IdValue"/> is not <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if either <paramref name="ifEntityNotNull"/> or <paramref name="ifIdOnly"/> was invoked; otherwise, <see langword="false"/> if no delegates were invoked.</returns>
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

    /// <summary>
    /// Invokes a delegate with synchronized access to the current <c>ForeignKeyReference</c> object if it has no unique identifier.
    /// </summary>
    /// <param name="ifEntityHasNoId">Delegate that is invoked when <see cref="Entity"/> is not <see langword="null"/>, but its <see cref="IHasSimpleIdentifier.Id"/> has not been set.</param>
    /// <param name="ifNoReference">Delegate that is invoked when both <see cref="Entity"/> and <see cref="IdValue"/> are <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if either <paramref name="ifEntityHasNoId"/> or <paramref name="ifNoReference"/> was invoked; otherwise, <see langword="false"/> if no delegates were invoked.</returns>
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

    /// <summary>
    /// Invokes a delegate with synchronized access to the current <c>ForeignKeyReference</c> object if it has a unique identifier.
    /// </summary>
    /// <param name="ifHasId">Delegate that is invoked when either <see cref="Entity"/> is not null and its its <see cref="IHasSimpleIdentifier.Id"/> has been set or <see cref="IdValue"/> is not <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="ifHasId"/> was invoked; otherwise, <see langword="false"/> if it was not invoked.</returns>
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

    /// <summary>
    /// Invokes a delegate with synchronized access to the current <c>ForeignKeyReference</c> object if it has a unique identifier.
    /// </summary>
    /// <param name="ifEntityHasId">Delegate that is invoked when <see cref="Entity"/> is not <see langword="null"/>, and its <see cref="IHasSimpleIdentifier.Id"/> has been set.</param>
    /// <param name="ifIdOnly">Delegate that is invoked when both <see cref="Entity"/> is <see langword="null"/> and <see cref="IdValue"/> is not <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if either <paramref name="ifEntityHasId"/> or <paramref name="ifIdOnly"/> was invoked; otherwise, <see langword="false"/> if no delegates were invoked.</returns>
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

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
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
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}


/// <summary>
/// Represents a foreign key identifier and the optional associated nagivation property that as cast as a base type.
/// </summary>
/// <typeparam name="TBase">The base entity type that is returned.</typeparam>
/// <typeparam name="TEntity">The actual enity type for the navigation property.</typeparam>
#pragma warning disable CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
public class ForeignKeyReference<TBase, TEntity> : ForeignKeyReference<TEntity>, IForeignKeyReference<TBase>
#pragma warning restore CS0659 // Type overrides Object.Equals(object o) but does not override Object.GetHashCode()
    where TBase : class, IHasSimpleIdentifier, IEquatable<TBase>
    where TEntity : class, TBase, IEquatable<TEntity>
{
    TBase IForeignKeyReference<TBase>.Entity => Entity;

    /// <summary>
    /// Initializes a new <c>ForeignKeyReference</c> object from an existing entity.
    /// </summary>
    /// <param name="entity">The related entity.</param>
    /// <param name="syncRoot">The optional object that is to be used to synchronize access to the new <c>ForeignKeyReference</c> object.</param>
    public ForeignKeyReference(TEntity entity, object syncRoot = null) : base(entity, syncRoot) { }

    /// <summary>
    /// Initializes a new <c>ForeignKeyReference</c> object from a related record unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the related record that is of type <typeparamref name="TEntity"/>.</param>
    /// <param name="syncRoot">The optional object that is to be used to synchronize access to the new <c>ForeignKeyReference</c> object.</param>
    public ForeignKeyReference(Guid id, object syncRoot = null) : base(id, syncRoot) { }

    /// <summary>
    /// Initializes a new <c>ForeignKeyReference</c> object that has no related record.
    /// </summary>
    /// <param name="syncRoot">The optional object that is to be used to synchronize access to the new <c>ForeignKeyReference</c> object.</param>
    public ForeignKeyReference(object syncRoot) : base(syncRoot) { }

    bool IEquatable<IForeignKeyReference<TBase>>.Equals(IForeignKeyReference<TBase> other)
    {
        throw new NotImplementedException();
    }

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public override bool Equals(object obj) => obj is not null && ((obj is ForeignKeyReference<TBase, TEntity> other) ? Equals(other) : base.Equals(obj));
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
