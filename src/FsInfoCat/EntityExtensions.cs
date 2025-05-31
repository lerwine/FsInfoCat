using FsInfoCat.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat;

/// <summary>
/// Extension methods for entity objects.
/// </summary>
public static class EntityExtensions
{
    /// <summary>
    /// The default file name comparer.
    /// </summary>
    public static readonly StringComparer FileNameComparer = StringComparer.InvariantCultureIgnoreCase;

    /// <summary>
    /// Gets a value from a delegate with synchronized access to 2 <see cref="IForeignKeyReference{TEntity}"/> objects.
    /// </summary>
    /// <typeparam name="TEntity1">The type of the entity for the first <see cref="IForeignKeyReference{TEntity}"/>.</typeparam>
    /// <typeparam name="TEntity2">The type of the entity for the second <see cref="IForeignKeyReference{TEntity}"/>.</typeparam>
    /// <typeparam name="TResult">The type of value to be returned by one of the delegates.</typeparam>
    /// <param name="ref1">The first <see cref="IForeignKeyReference{TEntity}"/> object.</param>
    /// <param name="ref2">The second <see cref="IForeignKeyReference{TEntity}"/> object.</param>
    /// <param name="ifBothHaveIds">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for both <paramref name="ref1"/> and <paramref name="ref1"/>.</param>
    /// <param name="ifOnlyRef1HasId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for <paramref name="ref1"/> and <see langword="false"/> for <paramref name="ref1"/>.</param>
    /// <param name="ifOnlyRef2HasId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="false"/> for <paramref name="ref1"/> and <see langword="true"/> for <paramref name="ref1"/>.</param>
    /// <param name="ifNeitherRefHasId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="false"/> for both <paramref name="ref1"/> and <paramref name="ref1"/>.</param>
    /// <returns>The value that was produced by either <paramref name="ifBothHaveIds"/>, <paramref name="ifOnlyRef1HasId"/>, <paramref name="ifOnlyRef2HasId"/>, or <paramref name="ifNeitherRefHasId"/></returns>
    public static TResult SyncDerive<TEntity1, TEntity2, TResult>([DisallowNull] this IForeignKeyReference<TEntity1> ref1, [DisallowNull] IForeignKeyReference<TEntity2> ref2,
            [DisallowNull] Func<Guid, Guid, TResult> ifBothHaveIds, [DisallowNull] Func<Guid, TEntity2, TResult> ifOnlyRef1HasId, [DisallowNull] Func<TEntity1, Guid, TResult> ifOnlyRef2HasId,
            [DisallowNull] Func<TEntity1, TEntity2, TResult> ifNeitherRefHasId)
        where TEntity1 : class, IHasSimpleIdentifier, IEquatable<TEntity1>
        where TEntity2 : class, IHasSimpleIdentifier, IEquatable<TEntity2>
    {
        Monitor.Enter(ref1.SyncRoot);
        try
        {
            if (ReferenceEquals(ref1.SyncRoot, ref2.SyncRoot))
            {
                if (ref1.TryGetId(out Guid id1))
                    return ref2.TryGetId(out Guid id2) ? ifBothHaveIds(id1, id2) : ifOnlyRef1HasId(id1, ref2.Entity);
                return ref2.TryGetId(out Guid id) ? ifOnlyRef2HasId(ref1.Entity, id) : ifNeitherRefHasId(ref1.Entity, ref2.Entity);
            }
            Monitor.Enter(ref2.SyncRoot);
            try
            {
                if (ref1.TryGetId(out Guid id1))
                    return ref2.TryGetId(out Guid id2) ? ifBothHaveIds(id1, id2) : ifOnlyRef1HasId(id1, ref2.Entity);
                return ref2.TryGetId(out Guid id) ? ifOnlyRef2HasId(ref1.Entity, id) : ifNeitherRefHasId(ref1.Entity, ref2.Entity);
            }
            finally { Monitor.Exit(ref2.SyncRoot); }
        }
        finally { Monitor.Exit(ref1.SyncRoot); }
    }

    /// <summary>
    /// Gets a value from a delegate with synchronized access to a <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}"/> object.
    /// </summary>
    /// <typeparam name="TEntity1">The type of the entity for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/>.</typeparam>
    /// <typeparam name="TEntity2">The type of the entity for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/>.</typeparam>
    /// <typeparam name="TResult">The type of value to be returned by one of the delegates.</typeparam>
    /// <param name="source">The object to have synchronized access to.</param>
    /// <param name="ifBothHaveIds">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for
    /// both <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> and <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/> on <paramref name="source"/>.</param>
    /// <param name="ifOnlyRef1HasId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/>
    /// for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> and <see langword="false"/> for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/>
    /// on <paramref name="source"/>.</param>
    /// <param name="ifOnlyRef2HasId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="false"/>
    /// for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> and <see langword="true"/> for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/>
    /// on <paramref name="source"/>.</param>
    /// <param name="ifNeitherRefHasId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="false"/> for
    /// both <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> and <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/> on <paramref name="source"/>.</param>
    /// <returns>The value that was produced by either <paramref name="ifBothHaveIds"/>, <paramref name="ifOnlyRef1HasId"/>, <paramref name="ifOnlyRef2HasId"/>, or <paramref name="ifNeitherRefHasId"/></returns>
    public static TResult SyncDerive<TEntity1, TEntity2, TResult>([DisallowNull] this IHasMembershipKeyReference<TEntity1, TEntity2> source, [DisallowNull] Func<Guid, Guid, TResult> ifBothHaveIds,
        [DisallowNull] Func<Guid, TEntity2, TResult> ifOnlyRef1HasId, [DisallowNull] Func<TEntity1, Guid, TResult> ifOnlyRef2HasId, [DisallowNull] Func<TEntity1, TEntity2, TResult> ifNeitherRefHasId)
        where TEntity1 : class, IHasSimpleIdentifier, IEquatable<TEntity1>
        where TEntity2 : class, IHasSimpleIdentifier, IEquatable<TEntity2>
    {
        Monitor.Enter(source.SyncRoot);
        try
        {
            if (source.Ref1 is null)
            {
                if (source.Ref2 is null) return ifNeitherRefHasId(null, null);
                return source.Ref2.TryGetId(out Guid id2) ? ifOnlyRef2HasId(null, id2) : ifNeitherRefHasId(null, source.Ref2.Entity);
            }
            if (source.Ref1.TryGetId(out Guid id1))
            {
                if (source.Ref2 is null) return ifOnlyRef1HasId(id1, null);
                return source.Ref2.TryGetId(out Guid id2) ? ifBothHaveIds(id1, id2) : ifOnlyRef1HasId(id1, source.Ref2.Entity);
            }
            if (source.Ref2 is null) return ifNeitherRefHasId(source.Ref1.Entity, null);
            return source.Ref2.TryGetId(out Guid id) ? ifOnlyRef2HasId(source.Ref1.Entity, id) : ifNeitherRefHasId(source.Ref1.Entity, source.Ref2.Entity);
        }
        finally { Monitor.Exit(source.SyncRoot); }
    }

    /// <summary>
    /// Gets a value from a delegate with synchronized access to 2 <see cref="IForeignKeyReference{TEntity}"/> objects.
    /// </summary>
    /// <typeparam name="TEntity1">The type of the entity for the first <see cref="IForeignKeyReference{TEntity}"/>.</typeparam>
    /// <typeparam name="TEntity2">The type of the entity for the second <see cref="IForeignKeyReference{TEntity}"/>.</typeparam>
    /// <typeparam name="TResult">The type of value to be returned by one of the delegates.</typeparam>
    /// <param name="ref1">The first <see cref="IForeignKeyReference{TEntity}"/> object.</param>
    /// <param name="ref2">The second <see cref="IForeignKeyReference{TEntity}"/> object.</param>
    /// <param name="ifBothHaveIds">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for both <paramref name="ref1"/> and <paramref name="ref2"/>.</param>
    /// <param name="ifAnyHasNoId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for <paramref name="ref1"/>, or <paramref name="ref2"/>.</param>
    /// <returns>The value that was produced by either <paramref name="ifBothHaveIds"/> or <paramref name="ifAnyHasNoId"/></returns>
    public static TResult SyncDerive<TEntity1, TEntity2, TResult>([DisallowNull] this IForeignKeyReference<TEntity1> ref1, [DisallowNull] IForeignKeyReference<TEntity2> ref2,
            [DisallowNull] Func<Guid, Guid, TResult> ifBothHaveIds, [DisallowNull] Func<TResult> ifAnyHasNoId)
        where TEntity1 : class, IHasSimpleIdentifier, IEquatable<TEntity1>
        where TEntity2 : class, IHasSimpleIdentifier, IEquatable<TEntity2>
    {
        Monitor.Enter(ref1.SyncRoot);
        try
        {
            if (ReferenceEquals(ref1.SyncRoot, ref2.SyncRoot))
                return (ref1.TryGetId(out Guid id1) && ref2.TryGetId(out Guid id2)) ? ifBothHaveIds(id1, id2) : ifAnyHasNoId();
            Monitor.Enter(ref2.SyncRoot);
            try { return (ref1.TryGetId(out Guid id1) && ref2.TryGetId(out Guid id2)) ? ifBothHaveIds(id1, id2) : ifAnyHasNoId(); }
            finally { Monitor.Exit(ref2.SyncRoot); }
        }
        finally { Monitor.Exit(ref1.SyncRoot); }
    }

    /// <summary>
    /// Gets a value from a delegate with synchronized access to a <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}"/> object.
    /// </summary>
    /// <typeparam name="TEntity1">The type of the entity for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/>.</typeparam>
    /// <typeparam name="TEntity2">The type of the entity for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/>.</typeparam>
    /// <typeparam name="TResult">The type of value to be returned by one of the delegates.</typeparam>
    /// <param name="source">The object to have synchronized access to.</param>
    /// <param name="ifBothHaveIds">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for
    /// both <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> on <paramref name="source"/> and <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/>
    /// on <paramref name="source"/>.</param>
    /// <param name="ifAnyHasNoId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/>
    /// for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> or <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/> on <paramref name="source"/>.</param>
    /// <returns>The value that was produced by either <paramref name="ifBothHaveIds"/> or <paramref name="ifAnyHasNoId"/></returns>
    public static TResult SyncDerive<TEntity1, TEntity2, TResult>([DisallowNull] this IHasMembershipKeyReference<TEntity1, TEntity2> source, [DisallowNull] Func<Guid, Guid, TResult> ifBothHaveIds,
        [DisallowNull] Func<TResult> ifAnyHasNoId)
        where TEntity1 : class, IHasSimpleIdentifier, IEquatable<TEntity1>
        where TEntity2 : class, IHasSimpleIdentifier, IEquatable<TEntity2>
    {
        Monitor.Enter(source.SyncRoot);
        try { return (source.Ref1 is not null && source.Ref2 is not null && source.Ref1.TryGetId(out Guid id1) && source.Ref2.TryGetId(out Guid id2)) ? ifBothHaveIds(id1, id2) : ifAnyHasNoId(); }
        finally { Monitor.Exit(source.SyncRoot); }
    }

    /// <summary>
    /// Invokes a delegate with synchronized access to 2 <see cref="IForeignKeyReference{TEntity}"/> objects.
    /// </summary>
    /// <typeparam name="TEntity1">The type of the entity for the first <see cref="IForeignKeyReference{TEntity}"/>.</typeparam>
    /// <typeparam name="TEntity2">The type of the entity for the second <see cref="IForeignKeyReference{TEntity}"/>.</typeparam>
    /// <param name="ref1">The first <see cref="IForeignKeyReference{TEntity}"/> object.</param>
    /// <param name="ref2">The second <see cref="IForeignKeyReference{TEntity}"/> object.</param>
    /// <param name="ifBothHaveIds">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for both <paramref name="ref1"/> and <paramref name="ref1"/>.</param>
    /// <param name="ifOnlyRef1HasId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for <paramref name="ref1"/> and <see langword="false"/> for <paramref name="ref1"/>.</param>
    /// <param name="ifOnlyRef2HasId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="false"/> for <paramref name="ref1"/> and <see langword="true"/> for <paramref name="ref1"/>.</param>
    /// <param name="ifNeitherRefHasId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="false"/> for both <paramref name="ref1"/> and <paramref name="ref1"/>.</param>
    public static void SyncInvoke<TEntity1, TEntity2>([DisallowNull] this IForeignKeyReference<TEntity1> ref1, [DisallowNull] IForeignKeyReference<TEntity2> ref2, [DisallowNull] Action<Guid, Guid> ifBothHaveIds,
        [DisallowNull] Action<Guid, TEntity2> ifOnlyRef1HasId, [DisallowNull] Action<TEntity1, Guid> ifOnlyRef2HasId, [DisallowNull] Action<TEntity1, TEntity2> ifNeitherRefHasId)
        where TEntity1 : class, IHasSimpleIdentifier, IEquatable<TEntity1>
        where TEntity2 : class, IHasSimpleIdentifier, IEquatable<TEntity2>
    {
        Monitor.Enter(ref1.SyncRoot);
        try
        {
            if (ReferenceEquals(ref1.SyncRoot, ref2.SyncRoot))
            {
                if (ref1.TryGetId(out Guid id1))
                {
                    if (ref2.TryGetId(out Guid id2))
                        ifBothHaveIds(id1, id2);
                    else
                        ifOnlyRef1HasId(id1, ref2.Entity);
                }
                else if (ref2.TryGetId(out Guid id))
                    ifOnlyRef2HasId(ref1.Entity, id);
                else
                    ifNeitherRefHasId(ref1.Entity, ref2.Entity);
            }
            Monitor.Enter(ref2.SyncRoot);
            try
            {
                if (ref1.TryGetId(out Guid id1))
                {
                    if (ref2.TryGetId(out Guid id2))
                        ifBothHaveIds(id1, id2);
                    else
                        ifOnlyRef1HasId(id1, ref2.Entity);
                }
                else if (ref2.TryGetId(out Guid id))
                    ifOnlyRef2HasId(ref1.Entity, id);
                else
                    ifNeitherRefHasId(ref1.Entity, ref2.Entity);
            }
            finally { Monitor.Exit(ref2.SyncRoot); }
        }
        finally { Monitor.Exit(ref1.SyncRoot); }
    }

    /// <summary>
    /// Invokes a delegate with synchronized access to a <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}"/> object.
    /// </summary>
    /// <typeparam name="TEntity1">The type of the entity for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/>.</typeparam>
    /// <typeparam name="TEntity2">The type of the entity for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/>.</typeparam>
    /// <param name="source">The object to have synchronized access to.</param>
    /// <param name="ifBothHaveIds">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for
    /// both <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> and <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/> on <paramref name="source"/>.</param>
    /// <param name="ifOnlyRef1HasId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/>
    /// for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> and <see langword="false"/> for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/>
    /// on <paramref name="source"/>.</param>
    /// <param name="ifOnlyRef2HasId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="false"/>
    /// for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> and <see langword="true"/> for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/>
    /// on <paramref name="source"/>.</param>
    /// <param name="ifNeitherRefHasId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="false"/> for
    /// both <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> and <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/> on <paramref name="source"/>.</param>
    public static void SyncInvoke<TEntity1, TEntity2>([DisallowNull] this IHasMembershipKeyReference<TEntity1, TEntity2> source, [DisallowNull] Action<Guid, Guid> ifBothHaveIds,
        [DisallowNull] Action<Guid, TEntity2> ifOnlyRef1HasId, [DisallowNull] Action<TEntity1, Guid> ifOnlyRef2HasId, [DisallowNull] Action<TEntity1, TEntity2> ifNeitherRefHasId)
        where TEntity1 : class, IHasSimpleIdentifier, IEquatable<TEntity1>
        where TEntity2 : class, IHasSimpleIdentifier, IEquatable<TEntity2>
    {
        Monitor.Enter(source.SyncRoot);
        try
        {
            if (source.Ref1 is null)
            {
                if (source.Ref2 is null)
                    ifNeitherRefHasId(null, null);
                else if (source.Ref2.TryGetId(out Guid id))
                    ifOnlyRef2HasId(null, id);
                else
                    ifNeitherRefHasId(null, source.Ref2.Entity);
            }
            else if (source.Ref1.TryGetId(out Guid id1))
            {
                if (source.Ref2 is null)
                    ifOnlyRef1HasId(id1, null);
                else if (source.Ref2.TryGetId(out Guid id2))
                    ifBothHaveIds(id1, id2);
                else
                    ifOnlyRef1HasId(id1, source.Ref2.Entity);
            }
            else if (source.Ref2 is null)
                ifNeitherRefHasId(source.Ref1.Entity, null);
            else if (source.Ref2.TryGetId(out Guid id))
                ifOnlyRef2HasId(source.Ref1.Entity, id);
            else
                ifNeitherRefHasId(source.Ref1.Entity, source.Ref2.Entity);
        }
        finally { Monitor.Exit(source.SyncRoot); }
    }

    /// <summary>
    /// Invokes a delegate with synchronized access to 2 <see cref="IForeignKeyReference{TEntity}"/> objects.
    /// </summary>
    /// <typeparam name="TEntity1">The type of the entity for the first <see cref="IForeignKeyReference{TEntity}"/>.</typeparam>
    /// <typeparam name="TEntity2">The type of the entity for the second <see cref="IForeignKeyReference{TEntity}"/>.</typeparam>
    /// <param name="ref1">The first <see cref="IForeignKeyReference{TEntity}"/> object.</param>
    /// <param name="ref2">The second <see cref="IForeignKeyReference{TEntity}"/> object.</param>
    /// <param name="ifBothHaveIds">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for both <paramref name="ref1"/> and <paramref name="ref2"/>.</param>
    /// <param name="ifAnyHasNoId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for <paramref name="ref1"/>, or <paramref name="ref2"/>.</param>
    public static void SyncInvoke<TEntity1, TEntity2>([DisallowNull] this IForeignKeyReference<TEntity1> ref1, [DisallowNull] IForeignKeyReference<TEntity2> ref2, [DisallowNull] Action<Guid, Guid> ifBothHaveIds, [DisallowNull] Action ifAnyHasNoId)
        where TEntity1 : class, IHasSimpleIdentifier, IEquatable<TEntity1>
        where TEntity2 : class, IHasSimpleIdentifier, IEquatable<TEntity2>
    {
        Monitor.Enter(ref1.SyncRoot);
        try
        {
            if (ReferenceEquals(ref1.SyncRoot, ref2.SyncRoot))
            {
                if (ref1.TryGetId(out Guid id1) && ref2.TryGetId(out Guid id2))
                    ifBothHaveIds(id1, id2);
                else
                    ifAnyHasNoId();
            }
            Monitor.Enter(ref2.SyncRoot);
            try
            {
                if (ref1.TryGetId(out Guid id1) && ref2.TryGetId(out Guid id2))
                    ifBothHaveIds(id1, id2);
                else
                    ifAnyHasNoId();
            }
            finally { Monitor.Exit(ref2.SyncRoot); }
        }
        finally { Monitor.Exit(ref1.SyncRoot); }
    }

    /// <summary>
    /// Invokes a delegate with synchronized access to a <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}"/> object.
    /// </summary>
    /// <typeparam name="TEntity1">The type of the entity for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/>.</typeparam>
    /// <typeparam name="TEntity2">The type of the entity for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/>.</typeparam>
    /// <param name="source">The object to have synchronized access to.</param>
    /// <param name="ifBothHaveIds">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for
    /// both <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> on <paramref name="source"/> and <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/>
    /// on <paramref name="source"/>.</param>
    /// <param name="ifAnyHasNoId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/>
    /// for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> or <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/> on <paramref name="source"/>.</param>
    public static void SyncInvoke<TEntity1, TEntity2>([DisallowNull] this IHasMembershipKeyReference<TEntity1, TEntity2> source, [DisallowNull] Action<Guid, Guid> ifBothHaveIds, [DisallowNull] Action ifAnyHasNoId)
        where TEntity1 : class, IHasSimpleIdentifier, IEquatable<TEntity1>
        where TEntity2 : class, IHasSimpleIdentifier, IEquatable<TEntity2>
    {
        Monitor.Enter(source.SyncRoot);
        try
        {
            if (source.Ref1 is not null && source.Ref2 is not null && source.Ref1.TryGetId(out Guid id1) && source.Ref2.TryGetId(out Guid id2))
                ifBothHaveIds(id1, id2);
            else
                ifAnyHasNoId();
        }
        finally { Monitor.Exit(source.SyncRoot); }
    }

    /// <summary>
    /// Gets a value from a delegate with synchronized access to 2 <see cref="IForeignKeyReference{TEntity}"/> objects if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for both.
    /// </summary>
    /// <typeparam name="TEntity1">The type of the entity for the first <see cref="IForeignKeyReference{TEntity}"/>.</typeparam>
    /// <typeparam name="TEntity2">The type of the entity for the second <see cref="IForeignKeyReference{TEntity}"/>.</typeparam>
    /// <typeparam name="TResult">The type of value to be returned by the delegate.</typeparam>
    /// <param name="ref1">The first <see cref="IForeignKeyReference{TEntity}"/> object.</param>
    /// <param name="ref2">The second <see cref="IForeignKeyReference{TEntity}"/> object.</param>
    /// <param name="ifBothHaveIds">The delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> return <see langword="true"/> for both <paramref name="ref1"/> and <paramref name="ref2"/>.</param>
    /// <param name="result">If <see cref="IForeignKeyReference.HasId()"/> returned <see langword="true"/> for both <paramref name="ref1"/> and <paramref name="ref2"/>, the value returned by the <paramref name="ifBothHaveIds"/> delegate.</param>
    /// <returns><see langword="true"/> if <paramref name="ifBothHaveIds"/> delegate was invoked; otherwise, <see langword="false"/> if it was not.</returns>
    public static bool TrySyncDerive<TEntity1, TEntity2, TResult>([DisallowNull] this IForeignKeyReference<TEntity1> ref1, [DisallowNull] IForeignKeyReference<TEntity2> ref2,
        [DisallowNull] Func<Guid, Guid, TResult> ifBothHaveIds, out TResult result)
        where TEntity1 : class, IHasSimpleIdentifier, IEquatable<TEntity1>
        where TEntity2 : class, IHasSimpleIdentifier, IEquatable<TEntity2>
    {
        Monitor.Enter(ref1.SyncRoot);
        try
        {
            if (ReferenceEquals(ref1.SyncRoot, ref2.SyncRoot) && ref1.TryGetId(out Guid id1) && ref2.TryGetId(out Guid id2))
            {
                result = ifBothHaveIds(id1, id2);
                return true;
            }
            Monitor.Enter(ref2.SyncRoot);
            try
            {
                if (ref1.TryGetId(out Guid id3) && ref2.TryGetId(out Guid id4))
                {
                    result = ifBothHaveIds(id3, id4);
                    return true;
                }
            }
            finally { Monitor.Exit(ref2.SyncRoot); }
        }
        finally { Monitor.Exit(ref1.SyncRoot); }
        result = default;
        return false;
    }

    /// <summary>
    /// Gets a value from a delegate with synchronized access to a <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}"/> object if <see cref="IForeignKeyReference.HasId()"/>
    /// returns <see langword="true"/> for both <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> and <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/>.
    /// </summary>
    /// <typeparam name="TEntity1">The type of the entity for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/>.</typeparam>
    /// <typeparam name="TEntity2">The type of the entity for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/>.</typeparam>
    /// <typeparam name="TResult">The type of value to be returned by the delegate.</typeparam>
    /// <param name="source">The object to have synchronized access to.</param>
    /// <param name="ifBothHaveIds">The delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> return <see langword="true"/> for
    /// both <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> and <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/> on <paramref name="source"/>.</param>
    /// <param name="result">If <see cref="IForeignKeyReference.HasId()"/> returned <see langword="true"/> for both <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/>
    /// and <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/> on <paramref name="source"/>, the value returned by the <paramref name="ifBothHaveIds"/> delegate.</param>
    /// <returns><see langword="true"/> if <paramref name="ifBothHaveIds"/> delegate was invoked; otherwise, <see langword="false"/> if it was not.</returns>
    public static bool TrySyncDerive<TEntity1, TEntity2, TResult>([DisallowNull] this IHasMembershipKeyReference<TEntity1, TEntity2> source, [DisallowNull] Func<Guid, Guid, TResult> ifBothHaveIds, out TResult result)
        where TEntity1 : class, IHasSimpleIdentifier, IEquatable<TEntity1>
        where TEntity2 : class, IHasSimpleIdentifier, IEquatable<TEntity2>
    {
        Monitor.Enter(source.SyncRoot);
        try
        {
            if (source.Ref1 is not null && source.Ref2 is not null && source.Ref1.TryGetId(out Guid id1) && source.Ref2.TryGetId(out Guid id2))
            {
                result = ifBothHaveIds(id1, id2);
                return true;
            }
        }
        finally { Monitor.Exit(source.SyncRoot); }
        result = default;
        return false;
    }

    /// <summary>
    /// Invokes a delegate with synchronized access to 2 <see cref="IForeignKeyReference{TEntity}"/> objects if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for both.
    /// </summary>
    /// <typeparam name="TEntity1">The type of the entity for the first <see cref="IForeignKeyReference{TEntity}"/>.</typeparam>
    /// <typeparam name="TEntity2">The type of the entity for the second <see cref="IForeignKeyReference{TEntity}"/>.</typeparam>
    /// <param name="ref1">The first <see cref="IForeignKeyReference{TEntity}"/> object.</param>
    /// <param name="ref2">The second <see cref="IForeignKeyReference{TEntity}"/> object.</param>
    /// <param name="ifBothHaveIds">The delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> return <see langword="true"/> for both <paramref name="ref1"/> and <paramref name="ref2"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="ifBothHaveIds"/> delegate was invoked; otherwise, <see langword="false"/> if it was not.</returns>
    public static bool TrySyncInvoke<TEntity1, TEntity2>([DisallowNull] this IForeignKeyReference<TEntity1> ref1, [DisallowNull] IForeignKeyReference<TEntity2> ref2, [DisallowNull] Action<Guid, Guid> ifBothHaveIds)
        where TEntity1 : class, IHasSimpleIdentifier, IEquatable<TEntity1>
        where TEntity2 : class, IHasSimpleIdentifier, IEquatable<TEntity2>
    {
        Monitor.Enter(ref1.SyncRoot);
        try
        {
            if (ReferenceEquals(ref1.SyncRoot, ref2.SyncRoot) && ref1.TryGetId(out Guid id1) && ref2.TryGetId(out Guid id2))
            {
                ifBothHaveIds(id1, id2);
                return true;
            }
            Monitor.Enter(ref2.SyncRoot);
            try
            {
                if (ref1.TryGetId(out Guid id3) && ref2.TryGetId(out Guid id4))
                {
                    ifBothHaveIds(id3, id4);
                    return true;
                }
            }
            finally { Monitor.Exit(ref2.SyncRoot); }
        }
        finally { Monitor.Exit(ref1.SyncRoot); }
        return false;
    }

    /// <summary>
    /// Invokes a delegate with synchronized access to a <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}"/> object if <see cref="IForeignKeyReference.HasId()"/>
    /// returns <see langword="true"/> for both <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> and <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/>.
    /// </summary>
    /// <typeparam name="TEntity1">The type of the entity for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/>.</typeparam>
    /// <typeparam name="TEntity2">The type of the entity for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/>.</typeparam>
    /// <param name="source">The object to have synchronized access to.</param>
    /// <param name="ifBothHaveIds">The delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> return <see langword="true"/> for
    /// both <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> and <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/> on <paramref name="source"/>.</param>
    /// <returns><see langword="true"/> if <paramref name="ifBothHaveIds"/> delegate was invoked; otherwise, <see langword="false"/> if it was not.</returns>
    public static bool TrySyncInvoke<TEntity1, TEntity2>([DisallowNull] this IHasMembershipKeyReference<TEntity1, TEntity2> source, [DisallowNull] Action<Guid, Guid> ifBothHaveIds)
        where TEntity1 : class, IHasSimpleIdentifier, IEquatable<TEntity1>
        where TEntity2 : class, IHasSimpleIdentifier, IEquatable<TEntity2>
    {
        Monitor.Enter(source.SyncRoot);
        try
        {
            if (source.Ref1 is not null && source.Ref2 is not null && source.Ref1.TryGetId(out Guid id1) && source.Ref2.TryGetId(out Guid id2))
            {
                ifBothHaveIds(id1, id2);
                return true;
            }
        }
        finally { Monitor.Exit(source.SyncRoot); }
        return false;
    }

    /// <summary>
    /// Asynchronously gets a value from a delegate with synchronized access to 2 <see cref="IForeignKeyReference{TEntity}"/> objects.
    /// </summary>
    /// <typeparam name="TEntity1">The type of the entity for the first <see cref="IForeignKeyReference{TEntity}"/>.</typeparam>
    /// <typeparam name="TEntity2">The type of the entity for the second <see cref="IForeignKeyReference{TEntity}"/>.</typeparam>
    /// <typeparam name="TResult">The type of value to be returned by one of the delegates.</typeparam>
    /// <param name="ref1">The first <see cref="IForeignKeyReference{TEntity}"/> object.</param>
    /// <param name="ref2">The second <see cref="IForeignKeyReference{TEntity}"/> object.</param>
    /// <param name="ifBothHaveIds">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for both <paramref name="ref1"/> and <paramref name="ref1"/>.</param>
    /// <param name="ifOnlyRef1HasId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for <paramref name="ref1"/> and <see langword="false"/> for <paramref name="ref1"/>.</param>
    /// <param name="ifOnlyRef2HasId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="false"/> for <paramref name="ref1"/> and <see langword="true"/> for <paramref name="ref1"/>.</param>
    /// <param name="ifNeitherRefHasId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="false"/> for both <paramref name="ref1"/> and <paramref name="ref1"/>.</param>
    /// <returns>A <see cref="Task{TResult}"/> that returns the value that was produced by either <paramref name="ifBothHaveIds"/>, <paramref name="ifOnlyRef1HasId"/>, <paramref name="ifOnlyRef2HasId"/>, or <paramref name="ifNeitherRefHasId"/></returns>
    public static async Task<TResult> SyncDeriveAsync<TEntity1, TEntity2, TResult>([DisallowNull] this IForeignKeyReference<TEntity1> ref1, [DisallowNull] IForeignKeyReference<TEntity2> ref2,
        [DisallowNull] Func<Guid, Guid, Task<TResult>> ifBothHaveIds, [DisallowNull] Func<Guid, TEntity2, Task<TResult>> ifOnlyRef1HasId, [DisallowNull] Func<TEntity1, Guid, Task<TResult>> ifOnlyRef2HasId,
        [DisallowNull] Func<TEntity1, TEntity2, Task<TResult>> ifNeitherRefHasId)
        where TEntity1 : class, IHasSimpleIdentifier, IEquatable<TEntity1>
        where TEntity2 : class, IHasSimpleIdentifier, IEquatable<TEntity2>
    {
        Monitor.Enter(ref1.SyncRoot);
        try
        {
            if (ReferenceEquals(ref1.SyncRoot, ref2.SyncRoot))
            {
                if (ref1.TryGetId(out Guid id1))
                    return await (ref2.TryGetId(out Guid id2) ? ifBothHaveIds(id1, id2) : ifOnlyRef1HasId(id1, ref2.Entity));
                return await (ref2.TryGetId(out Guid id) ? ifOnlyRef2HasId(ref1.Entity, id) : ifNeitherRefHasId(ref1.Entity, ref2.Entity));
            }
            Monitor.Enter(ref2.SyncRoot);
            try
            {
                if (ref1.TryGetId(out Guid id1))
                    return await (ref2.TryGetId(out Guid id2) ? ifBothHaveIds(id1, id2) : ifOnlyRef1HasId(id1, ref2.Entity));
                return await (ref2.TryGetId(out Guid id) ? ifOnlyRef2HasId(ref1.Entity, id) : ifNeitherRefHasId(ref1.Entity, ref2.Entity));
            }
            finally { Monitor.Exit(ref2.SyncRoot); }
        }
        finally { Monitor.Exit(ref1.SyncRoot); }
    }

    /// <summary>
    /// Asynchronously gets a value from a delegate with synchronized access to a <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}"/> object.
    /// </summary>
    /// <typeparam name="TEntity1">The type of the entity for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/>.</typeparam>
    /// <typeparam name="TEntity2">The type of the entity for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/>.</typeparam>
    /// <typeparam name="TResult">The type of value to be returned by one of the delegates.</typeparam>
    /// <param name="source">The object to have synchronized access to.</param>
    /// <param name="ifBothHaveIds">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for
    /// both <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> and <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/> on <paramref name="source"/>.</param>
    /// <param name="ifOnlyRef1HasId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/>
    /// for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> and <see langword="false"/> for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/>
    /// on <paramref name="source"/>.</param>
    /// <param name="ifOnlyRef2HasId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="false"/>
    /// for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> and <see langword="true"/> for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/>
    /// on <paramref name="source"/>.</param>
    /// <param name="ifNeitherRefHasId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="false"/> for
    /// both <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> and <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/> on <paramref name="source"/>.</param>
    /// <returns>A <see cref="Task{TResult}"/> that returns the value that was produced by either <paramref name="ifBothHaveIds"/>, <paramref name="ifOnlyRef1HasId"/>, <paramref name="ifOnlyRef2HasId"/>, or <paramref name="ifNeitherRefHasId"/></returns>
    public static async Task<TResult> SyncDeriveAsync<TEntity1, TEntity2, TResult>([DisallowNull] this IHasMembershipKeyReference<TEntity1, TEntity2> source,
        [DisallowNull] Func<Guid, Guid, Task<TResult>> ifBothHaveIds, [DisallowNull] Func<Guid, TEntity2, Task<TResult>> ifOnlyRef1HasId, [DisallowNull] Func<TEntity1, Guid, Task<TResult>> ifOnlyRef2HasId,
        [DisallowNull] Func<TEntity1, TEntity2, Task<TResult>> ifNeitherRefHasId)
        where TEntity1 : class, IHasSimpleIdentifier, IEquatable<TEntity1>
        where TEntity2 : class, IHasSimpleIdentifier, IEquatable<TEntity2>
    {
        Monitor.Enter(source.SyncRoot);
        try
        {
            if (source.Ref1 is null)
            {
                if (source.Ref2 is null) return await ifNeitherRefHasId(null, null);
                return await (source.Ref2.TryGetId(out Guid id) ? ifOnlyRef2HasId(null, id) : ifNeitherRefHasId(null, source.Ref2.Entity));
            }
            if (source.Ref1.TryGetId(out Guid id1))
            {
                if (source.Ref2 is null) return await ifOnlyRef1HasId(id1, null);
                return await (source.Ref2.TryGetId(out Guid id) ? ifBothHaveIds(id1, id) : ifOnlyRef1HasId(id1, source.Ref2.Entity));
            }
            return await (source.Ref2.TryGetId(out Guid id2) ? ifOnlyRef2HasId(source.Ref1.Entity, id2) : ifNeitherRefHasId(source.Ref1.Entity, source.Ref2.Entity));
        }
        finally { Monitor.Exit(source.SyncRoot); }
    }

    /// <summary>
    /// Asynchronously gets a value from a delegate with synchronized access to 2 <see cref="IForeignKeyReference{TEntity}"/> objects.
    /// </summary>
    /// <typeparam name="TEntity1">The type of the entity for the first <see cref="IForeignKeyReference{TEntity}"/>.</typeparam>
    /// <typeparam name="TEntity2">The type of the entity for the second <see cref="IForeignKeyReference{TEntity}"/>.</typeparam>
    /// <typeparam name="TResult">The type of value to be returned by one of the delegates.</typeparam>
    /// <param name="ref1">The first <see cref="IForeignKeyReference{TEntity}"/> object.</param>
    /// <param name="ref2">The second <see cref="IForeignKeyReference{TEntity}"/> object.</param>
    /// <param name="ifBothHaveIds">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for both <paramref name="ref1"/> and <paramref name="ref2"/>.</param>
    /// <param name="ifAnyHasNoId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for <paramref name="ref1"/>, or <paramref name="ref2"/>.</param>
    /// <returns>A <see cref="Task{TResult}"/> that returns the value that was produced by either <paramref name="ifBothHaveIds"/> or <paramref name="ifAnyHasNoId"/></returns>
    public static async Task<TResult> SyncDeriveAsync<TEntity1, TEntity2, TResult>([DisallowNull] this IForeignKeyReference<TEntity1> ref1, [DisallowNull] IForeignKeyReference<TEntity2> ref2,
        [DisallowNull] Func<Guid, Guid, Task<TResult>> ifBothHaveIds, [DisallowNull] Func<Task<TResult>> ifAnyHasNoId)
        where TEntity1 : class, IHasSimpleIdentifier, IEquatable<TEntity1>
        where TEntity2 : class, IHasSimpleIdentifier, IEquatable<TEntity2>
    {
        Monitor.Enter(ref1.SyncRoot);
        try
        {
            if (ReferenceEquals(ref1.SyncRoot, ref2.SyncRoot))
                return await ((ref1.TryGetId(out Guid id1) && ref2.TryGetId(out Guid id2)) ? ifBothHaveIds(id1, id2) : ifAnyHasNoId());
            Monitor.Enter(ref2.SyncRoot);
            try { return await ((ref1.TryGetId(out Guid id1) && ref2.TryGetId(out Guid id2)) ? ifBothHaveIds(id1, id2) : ifAnyHasNoId()); }
            finally { Monitor.Exit(ref2.SyncRoot); }
        }
        finally { Monitor.Exit(ref1.SyncRoot); }
    }

    /// <summary>
    /// Asynchronously gets a value from a delegate with synchronized access to a <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}"/> object.
    /// </summary>
    /// <typeparam name="TEntity1">The type of the entity for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/>.</typeparam>
    /// <typeparam name="TEntity2">The type of the entity for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/>.</typeparam>
    /// <typeparam name="TResult">The type of value to be returned by one of the delegates.</typeparam>
    /// <param name="source">The object to have synchronized access to.</param>
    /// <param name="ifBothHaveIds">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for
    /// both <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> on <paramref name="source"/> and <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/>
    /// on <paramref name="source"/>.</param>
    /// <param name="ifAnyHasNoId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/>
    /// for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> or <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/> on <paramref name="source"/>.</param>
    /// <returns>A <see cref="Task{TResult}"/> that returns the value that was produced by either <paramref name="ifBothHaveIds"/> or <paramref name="ifAnyHasNoId"/></returns>
    public static async Task<TResult> SyncDeriveAsync<TEntity1, TEntity2, TResult>([DisallowNull] this IHasMembershipKeyReference<TEntity1, TEntity2> source,
        [DisallowNull] Func<Guid, Guid, Task<TResult>> ifBothHaveIds, [DisallowNull] Func<Task<TResult>> ifAnyHasNoId)
        where TEntity1 : class, IHasSimpleIdentifier, IEquatable<TEntity1>
        where TEntity2 : class, IHasSimpleIdentifier, IEquatable<TEntity2>
    {
        Monitor.Enter(source.SyncRoot);
        try { return await ((source.Ref1 is not null && source.Ref2 is not null && source.Ref1.TryGetId(out Guid id1) && source.Ref2.TryGetId(out Guid id2)) ? ifBothHaveIds(id1, id2) : ifAnyHasNoId()); }
        finally { Monitor.Exit(source.SyncRoot); }
    }

    /// <summary>
    /// Asynchronously invokes a delegate with synchronized access to 2 <see cref="IForeignKeyReference{TEntity}"/> objects.
    /// </summary>
    /// <typeparam name="TEntity1">The type of the entity for the first <see cref="IForeignKeyReference{TEntity}"/>.</typeparam>
    /// <typeparam name="TEntity2">The type of the entity for the second <see cref="IForeignKeyReference{TEntity}"/>.</typeparam>
    /// <param name="ref1">The first <see cref="IForeignKeyReference{TEntity}"/> object.</param>
    /// <param name="ref2">The second <see cref="IForeignKeyReference{TEntity}"/> object.</param>
    /// <param name="ifBothHaveIds">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for both <paramref name="ref1"/> and <paramref name="ref1"/>.</param>
    /// <param name="ifOnlyRef1HasId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for <paramref name="ref1"/> and <see langword="false"/> for <paramref name="ref1"/>.</param>
    /// <param name="ifOnlyRef2HasId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="false"/> for <paramref name="ref1"/> and <see langword="true"/> for <paramref name="ref1"/>.</param>
    /// <param name="ifNeitherRefHasId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="false"/> for both <paramref name="ref1"/> and <paramref name="ref1"/>.</param>
    /// <returns>A <see cref="Task"/> for the asynchronous invocation of either <paramref name="ifBothHaveIds"/>, <paramref name="ifOnlyRef1HasId"/>, <paramref name="ifOnlyRef2HasId"/>, or <paramref name="ifNeitherRefHasId"/>.</returns>
    public static async Task SyncInvokeAsync<TEntity1, TEntity2>([DisallowNull] this IForeignKeyReference<TEntity1> ref1, [DisallowNull] IForeignKeyReference<TEntity2> ref2,
        [DisallowNull] Func<Guid, Guid, Task> ifBothHaveIds, [DisallowNull] Func<Guid, TEntity2, Task> ifOnlyRef1HasId, [DisallowNull] Func<TEntity1, Guid, Task> ifOnlyRef2HasId,
        [DisallowNull] Func<TEntity1, TEntity2, Task> ifNeitherRefHasId)
        where TEntity1 : class, IHasSimpleIdentifier, IEquatable<TEntity1>
        where TEntity2 : class, IHasSimpleIdentifier, IEquatable<TEntity2>
    {
        Monitor.Enter(ref1.SyncRoot);
        try
        {
            if (ReferenceEquals(ref1.SyncRoot, ref2.SyncRoot))
            {
                if (ref1.TryGetId(out Guid id1))
                {
                    if (ref2.TryGetId(out Guid id2))
                        await ifBothHaveIds(id1, id2);
                    else
                        await ifOnlyRef1HasId(id1, ref2.Entity);
                }
                else if (ref2.TryGetId(out Guid id))
                    await ifOnlyRef2HasId(ref1.Entity, id);
                else
                    await ifNeitherRefHasId(ref1.Entity, ref2.Entity);
            }
            Monitor.Enter(ref2.SyncRoot);
            try
            {
                if (ref1.TryGetId(out Guid id1))
                {
                    if (ref2.TryGetId(out Guid id2))
                        await ifBothHaveIds(id1, id2);
                    else
                        await ifOnlyRef1HasId(id1, ref2.Entity);
                }
                else if (ref2.TryGetId(out Guid id))
                    await ifOnlyRef2HasId(ref1.Entity, id);
                else
                    await ifNeitherRefHasId(ref1.Entity, ref2.Entity);
            }
            finally { Monitor.Exit(ref2.SyncRoot); }
        }
        finally { Monitor.Exit(ref1.SyncRoot); }
    }

    /// <summary>
    /// Asynchronously invokes a delegate with synchronized access to a <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}"/> object.
    /// </summary>
    /// <typeparam name="TEntity1">The type of the entity for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/>.</typeparam>
    /// <typeparam name="TEntity2">The type of the entity for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/>.</typeparam>
    /// <param name="source">The object to have synchronized access to.</param>
    /// <param name="ifBothHaveIds">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for
    /// both <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> and <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/> on <paramref name="source"/>.</param>
    /// <param name="ifOnlyRef1HasId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/>
    /// for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> and <see langword="false"/> for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/>
    /// on <paramref name="source"/>.</param>
    /// <param name="ifOnlyRef2HasId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="false"/>
    /// for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> and <see langword="true"/> for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/>
    /// on <paramref name="source"/>.</param>
    /// <param name="ifNeitherRefHasId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="false"/> for
    /// both <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> and <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/> on <paramref name="source"/>.</param>
    /// <returns>A <see cref="Task"/> for the asynchronous invocation of either <paramref name="ifBothHaveIds"/>, <paramref name="ifOnlyRef1HasId"/>, <paramref name="ifOnlyRef2HasId"/>, or <paramref name="ifNeitherRefHasId"/>.</returns>
    public static async Task SyncInvokeAsync<TEntity1, TEntity2>([DisallowNull] this IHasMembershipKeyReference<TEntity1, TEntity2> source, [DisallowNull] Func<Guid, Guid, Task> ifBothHaveIds,
        [DisallowNull] Func<Guid, TEntity2, Task> ifOnlyRef1HasId, [DisallowNull] Func<TEntity1, Guid, Task> ifOnlyRef2HasId, [DisallowNull] Func<TEntity1, TEntity2, Task> ifNeitherRefHasId)
        where TEntity1 : class, IHasSimpleIdentifier, IEquatable<TEntity1>
        where TEntity2 : class, IHasSimpleIdentifier, IEquatable<TEntity2>
    {
        Monitor.Enter(source.SyncRoot);
        try
        {
            if (source.Ref1 is null)
            {
                if (source.Ref2.TryGetId(out Guid id))
                    await ifOnlyRef2HasId(null, id);
                else
                    await ifNeitherRefHasId(null, source.Ref2.Entity);
            }
            else if (source.Ref1.TryGetId(out Guid id1))
            {
                if (source.Ref2 is null)
                    await ifOnlyRef1HasId(id1, null);
                else if (source.Ref2.TryGetId(out Guid id2))
                    await ifBothHaveIds(id1, id2);
                else
                    await ifOnlyRef1HasId(id1, source.Ref2.Entity);
            }
            else if (source.Ref2 is null)
                await ifNeitherRefHasId(source.Ref1.Entity, null);
            else if (source.Ref2.TryGetId(out Guid id))
                await ifOnlyRef2HasId(source.Ref1.Entity, id);
            else
                await ifNeitherRefHasId(source.Ref1.Entity, source.Ref2.Entity);
        }
        finally { Monitor.Exit(source.SyncRoot); }
    }

    /// <summary>
    /// Asynchronously invokes a delegate with synchronized access to 2 <see cref="IForeignKeyReference{TEntity}"/> objects.
    /// </summary>
    /// <typeparam name="TEntity1">The type of the entity for the first <see cref="IForeignKeyReference{TEntity}"/>.</typeparam>
    /// <typeparam name="TEntity2">The type of the entity for the second <see cref="IForeignKeyReference{TEntity}"/>.</typeparam>
    /// <param name="ref1">The first <see cref="IForeignKeyReference{TEntity}"/> object.</param>
    /// <param name="ref2">The second <see cref="IForeignKeyReference{TEntity}"/> object.</param>
    /// <param name="ifBothHaveIds">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for both <paramref name="ref1"/> and <paramref name="ref2"/>.</param>
    /// <param name="ifAnyHasNoId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="false"/> for <paramref name="ref1"/>, or <paramref name="ref2"/>.</param>
    /// <returns>A <see cref="Task"/> for the asynchronous invocation of either <paramref name="ifBothHaveIds"/> or <paramref name="ifAnyHasNoId"/>.</returns>
    public static async Task SyncInvokeAsync<TEntity1, TEntity2>([DisallowNull] this IForeignKeyReference<TEntity1> ref1, [DisallowNull] IForeignKeyReference<TEntity2> ref2,
        [DisallowNull] Func<Guid, Guid, Task> ifBothHaveIds, [DisallowNull] Func<Task> ifAnyHasNoId)
        where TEntity1 : class, IHasSimpleIdentifier, IEquatable<TEntity1>
        where TEntity2 : class, IHasSimpleIdentifier, IEquatable<TEntity2>
    {
        Monitor.Enter(ref1.SyncRoot);
        try
        {
            if (ReferenceEquals(ref1.SyncRoot, ref2.SyncRoot))
            {
                if (ref1.TryGetId(out Guid id1) && ref2.TryGetId(out Guid id2))
                    await ifBothHaveIds(id1, id2);
                else
                    await ifAnyHasNoId();
            }
            Monitor.Enter(ref2.SyncRoot);
            try
            {
                if (ref1.TryGetId(out Guid id1) && ref2.TryGetId(out Guid id2))
                    await ifBothHaveIds(id1, id2);
                else
                    await ifAnyHasNoId();
            }
            finally { Monitor.Exit(ref2.SyncRoot); }
        }
        finally { Monitor.Exit(ref1.SyncRoot); }
    }

    /// <summary>
    /// Asynchronously invokes a delegate with synchronized access to a <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}"/> object.
    /// </summary>
    /// <typeparam name="TEntity1">The type of the entity for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/>.</typeparam>
    /// <typeparam name="TEntity2">The type of the entity for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/>.</typeparam>
    /// <param name="source">The object to have synchronized access to.</param>
    /// <param name="ifBothHaveIds">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/> for
    /// both <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> on <paramref name="source"/> and <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/>
    /// on <paramref name="source"/>.</param>
    /// <param name="ifAnyHasNoId">Delegate to invoke if <see cref="IForeignKeyReference.HasId()"/> returns <see langword="true"/>
    /// for <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref1"/> or <see cref="IHasMembershipKeyReference{TEntity1, TEntity2}.Ref2"/> on <paramref name="source"/>.</param>
    /// <returns>A <see cref="Task"/> for the asynchronous invocation of either <paramref name="ifBothHaveIds"/> or <paramref name="ifAnyHasNoId"/>.</returns>
    public static async Task SyncInvokeAsync<TEntity1, TEntity2>([DisallowNull] this IHasMembershipKeyReference<TEntity1, TEntity2> source, [DisallowNull] Func<Guid, Guid, Task> ifBothHaveIds,
        [DisallowNull] Func<Task> ifAnyHasNoId)
        where TEntity1 : class, IHasSimpleIdentifier, IEquatable<TEntity1>
        where TEntity2 : class, IHasSimpleIdentifier, IEquatable<TEntity2>
    {
        Monitor.Enter(source.SyncRoot);
        try
        {
            if (source.Ref1 is not null && source.Ref2 is not null && source.Ref1.TryGetId(out Guid id1) && source.Ref2.TryGetId(out Guid id2))
                await ifBothHaveIds(id1, id2);
            else
                await ifAnyHasNoId();
        }
        finally { Monitor.Exit(source.SyncRoot); }
    }

    /// <summary>
    /// Converts byte values to a version string.
    /// </summary>
    /// <param name="value">The value to convert.</param>
    /// <returns>The converted version string.</returns>
    public static string ToVersionString([AllowNull] this Collections.ByteValues value)
    {
        if (value is null || value.Count == 0)
            return "";
        return string.Join('.', value.Select(b => b.ToString()));
    }

    /// <summary>
    /// Translates ancestor names from a DB query into a path string.
    /// </summary>
    /// <param name="ancestorNames">Ancestor names from a DB query.</param>
    /// <returns>A path string with the ancestor names.</returns>
    public static string AncestorNamesToPath(string ancestorNames)
    {
        if (string.IsNullOrWhiteSpace(ancestorNames))
            return "";
        string[] segments = [.. ancestorNames.Split('/').Where(s => s.Length > 0)];
        if (segments.Length < 2)
            return ancestorNames;
        return Path.Combine([.. segments.Reverse()]);
    }

    /// <summary>
    /// Indicates whether the entry exists in the target database.
    /// </summary>
    /// <param name="entry">The <see cref="EntityEntry"/> to test.</param>
    /// <returns><see langword="true"/> if the <paramref name="entry"/> is not <see langword="null"/> and its <see cref="EntityEntry.State"/>
    /// is <see cref="EntityState.Unchanged"/> or <see cref="EntityState.Modified"/>; otherwise, <see langword="false"/>.</returns>
    public static bool ExistsInDb(this EntityEntry entry)
    {
        if (entry is null)
            return false;
        return entry.State switch
        {
            EntityState.Unchanged or EntityState.Modified => true,
            _ => false,
        };
    }

    /// <summary>
    /// Asynchronously removes entities related to a <see cref="EntityEntry{TEntity}"/>
    /// </summary>
    /// <typeparam name="TEntity">The type entity that may have related entities that are to be deleted from the database.</typeparam>
    /// <typeparam name="TRelated">The type of related entity.</typeparam>
    /// <param name="entry">The entity that may have related entities that are to be deleted from the database.</param>
    /// <param name="propertyExpression">Expression that returns the collection navigation.</param>
    /// <param name="dbSet">The database set containing the related entities.</param>
    /// <param name="cancellationToken">A token that is observed for cancelling the asynchronous operation.</param>
    /// <returns>A <see cref="Task{TResult}"/> that returns <see langword="true"/> if <paramref name="entry"/> had any related entities to be deleted; otherwise, <see langword="false"/>.</returns>
    public static async Task<bool> RemoveRelatedEntitiesAsync<TEntity, TRelated>([DisallowNull] this EntityEntry<TEntity> entry,
            [DisallowNull] Expression<Func<TEntity, IEnumerable<TRelated>>> propertyExpression, [DisallowNull] DbSet<TRelated> dbSet, CancellationToken cancellationToken)
        where TEntity : class
        where TRelated : class
    {
        TRelated[] related = [.. await entry.GetRelatedCollectionAsync(propertyExpression, cancellationToken)];
        if (related.Length > 0)
        {
            dbSet.RemoveRange(related);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Asynchronously gets related entities.
    /// </summary>
    /// <typeparam name="TEntity">The type entity that may have related entities.</typeparam>
    /// <typeparam name="TRelated">The type of related entity.</typeparam>
    /// <param name="entry">The entity to retrieve related entries from.</param>
    /// <param name="propertyExpression">Expression that returns the collection navigation.</param>
    /// <param name="cancellationToken">A token that is observed for cancelling the asynchronous operation.</param>
    /// <returns>A <see cref="Task{TResult}"/> that returns an enumeration of the related entities.</returns>
    public static async Task<IEnumerable<TRelated>> GetRelatedCollectionAsync<TEntity, TRelated>([DisallowNull] this EntityEntry<TEntity> entry,
            [DisallowNull] Expression<Func<TEntity, IEnumerable<TRelated>>> propertyExpression, CancellationToken cancellationToken)
        where TEntity : class
        where TRelated : class
    {
        cancellationToken.ThrowIfCancellationRequested();
        CollectionEntry<TEntity, TRelated> collectionEntry = entry.Collection(propertyExpression);
        if (!collectionEntry.IsLoaded)
            await collectionEntry.LoadAsync(cancellationToken);
        return collectionEntry.CurrentValue;
    }

    /// <summary>
    /// Asynchronously gets an enumeration of filtered related entities.
    /// </summary>
    /// <typeparam name="TEntity">The type entity that may have related entities.</typeparam>
    /// <typeparam name="TRelated">The type of related entity.</typeparam>
    /// <param name="entry">The entity to retrieve related entries from.</param>
    /// <param name="propertyExpression">Expression that returns the collection navigation.</param>
    /// <param name="predicate">Lambda expression that determines which related entities are to be enumerated.</param>
    /// <param name="cancellationToken">A token that is observed for cancelling the asynchronous operation.</param>
    /// <returns>A <see cref="Task{TResult}"/> that returns a filtered enumeration of the related entities.</returns>
    public static async Task<TRelated[]> QueryRelatedCollectionAsync<TEntity, TRelated>([DisallowNull] this EntityEntry<TEntity> entry,
            [DisallowNull] Expression<Func<TEntity, IEnumerable<TRelated>>> propertyExpression, [DisallowNull] Expression<Func<TRelated, bool>> predicate, CancellationToken cancellationToken)
        where TEntity : class
        where TRelated : class
    {
        cancellationToken.ThrowIfCancellationRequested();
        CollectionEntry<TEntity, TRelated> collectionEntry = entry.Collection(propertyExpression);
        return await collectionEntry.Query().Where(predicate).ToArrayAsync(cancellationToken);
    }

    /// <summary>
    /// Asynchronously get a related non-collection entity.
    /// </summary>
    /// <typeparam name="TEntity">The type entity that may have related entities.</typeparam>
    /// <typeparam name="TRelated">The type of related entity.</typeparam>
    /// <param name="entry">The entity to retrieve related entry from.</param>
    /// <param name="propertyExpression">Expression that returns the reference navigation.</param>
    /// <param name="cancellationToken">A token that is observed for cancelling the asynchronous operation.</param>
    /// <returns>A <see cref="Task{TResult}"/> that returns the related entity.</returns>
    public static async Task<TRelated> GetRelatedReferenceAsync<TEntity, TRelated>([DisallowNull] this EntityEntry<TEntity> entry,
            [DisallowNull] Expression<Func<TEntity, TRelated>> propertyExpression, CancellationToken cancellationToken)
        where TEntity : class
        where TRelated : class
    {
        cancellationToken.ThrowIfCancellationRequested();
        ReferenceEntry<TEntity, TRelated> referenceEntry = entry.Reference(propertyExpression);
        if (!referenceEntry.IsLoaded)
            await referenceEntry.LoadAsync(cancellationToken);
        return referenceEntry.CurrentValue;
    }

    /// <summary>
    /// Asynchronously get a related non-collection entity entry.
    /// </summary>
    /// <typeparam name="TEntity">The type entity that may have related entities.</typeparam>
    /// <typeparam name="TRelated">The type of related entity.</typeparam>
    /// <param name="entry">The entity to retrieve related entry from.</param>
    /// <param name="propertyExpression">Expression that returns the reference navigation.</param>
    /// <param name="cancellationToken">A token that is observed for cancelling the asynchronous operation.</param>
    /// <returns>A <see cref="Task{TResult}"/> that returns the related entity entry.</returns>
    public static async Task<EntityEntry<TRelated>> GetRelatedTargetEntryAsync<TEntity, TRelated>([DisallowNull] this EntityEntry<TEntity> entry,
        [DisallowNull] Expression<Func<TEntity, TRelated>> propertyExpression, CancellationToken cancellationToken)
        where TEntity : class
        where TRelated : class
    {
        cancellationToken.ThrowIfCancellationRequested();
        ReferenceEntry<TEntity, TRelated> referenceEntry = entry.Reference(propertyExpression);
        if (!referenceEntry.IsLoaded)
            await referenceEntry.LoadAsync(cancellationToken);
        return referenceEntry.TargetEntry;
    }

    /// <summary>
    /// Matches <see cref="IFile"/> objects with <see cref="FileInfo"/> objects with the same file length.
    /// </summary>
    /// <typeparam name="TDbFile">The type of the database entity.</typeparam>
    /// <param name="dbItems">The input database entity objects.</param>
    /// <param name="osItems">The input OS file objects to match up with corresponding <paramref name="dbItems"/>.</param>
    /// <param name="matchingPairs">Contains pairs of <typeparamref name="TDbFile"/> and <see cref="FileInfo"/> objects with the same file length.</param>
    /// <param name="unmatchedDb"><typeparamref name="TDbFile"/> objects where the file length does not match the file length of any <see cref="FileInfo"/> objects.</param>
    /// <param name="unmatchedFs"><see cref="FileInfo"/> objects where the file length does not match the file length of any <typeparamref name="TDbFile"/> objects.</param>
    /// <returns>A <c><see cref="List{T}">List</see>&lt;<see cref="ValueTuple{T1, T2}">ValueTuple</see>&lt;<see cref="List{TDbFile}" />, <see cref="List{FileInfo}" />&gt;&gt;</c> representing sets of multiple <typeparamref name="TDbFile"/> and <see cref="FileInfo"/> objects that share the same file length.</returns>
    private static List<(List<TDbFile> DbFile, List<FileInfo> FileInfo)> MatchByLength<TDbFile>(IEnumerable<TDbFile> dbItems, IEnumerable<FileInfo> osItems, List<(TDbFile DbFile, FileInfo FileInfo)> matchingPairs,
        out List<TDbFile> unmatchedDb, out List<FileInfo> unmatchedFs)
        where TDbFile : class, Model.IFile
    {
        List<TDbFile> udb = [];
        List<(List<TDbFile>, List<FileInfo>)> mm = [];
        long[] values = [.. dbItems.GroupBy(d => d.BinaryProperties.Length).GroupJoin(osItems, g => g.Key, f => f.Length, (g, f) => (Group: g, f.ToList())).Where(a =>
        {
            (IGrouping<long, TDbFile> grp, List<FileInfo> fs) = a;
            List<TDbFile> db = [.. grp];
            if (db.Count == 1 && fs.Count == 1)
                matchingPairs.Add((db[0], fs[0]));
            else if (fs.Count > 0)
                mm.Add((db, fs));
            else
            {
                udb.AddRange(db);
                return false;
            }
            return true;
        }).Select(a => a.Group.Key)];
        unmatchedDb = udb;
        unmatchedFs = [.. (values.Length == 0) ? osItems : osItems.Where(o => !values.Contains(o.Length))];
        return mm;
    }

    /// <summary>
    /// Matches <see cref="IFile"/> objects with <see cref="FileInfo"/> objects with the same last write time.
    /// </summary>
    /// <typeparam name="TDbFile">The type of the database entity.</typeparam>
    /// <param name="dbItems">The input database entity objects.</param>
    /// <param name="osItems">The input OS file objects to match up with corresponding <paramref name="dbItems"/>.</param>
    /// <param name="matchingPairs">Contains pairs of <typeparamref name="TDbFile"/> and <see cref="FileInfo"/> objects with the same file length and last write time.</param>
    /// <param name="partialMatches">Sets of multiple <typeparamref name="TDbFile"/> and <see cref="FileInfo"/> objects that share only the same last write time with no matching file length.</param>
    /// <param name="unmatchedDb"><typeparamref name="TDbFile"/> objects where neither the file length nor last write time matches the file length or last write time of any <see cref="FileInfo"/> objects.</param>
    /// <param name="unmatchedFs"><see cref="FileInfo"/> objects where neither the file length nor last write time matches the file length or last write time of any <typeparamref name="TDbFile"/> objects.</param>
    /// <returns>A <c><see cref="List{T}">List</see>&lt;<see cref="ValueTuple{T1, T2}">ValueTuple</see>&lt;<see cref="List{TDbFile}" />, <see cref="List{FileInfo}" />&gt;&gt;</c> representing sets of multiple <typeparamref name="TDbFile"/> and <see cref="FileInfo"/> objects that share the same file length and last write time.</returns>
    private static List<(List<TDbFile> DbFile, List<FileInfo> FileInfo)> MatchByLastWriteTime<TDbFile>(IEnumerable<TDbFile> dbItems, IEnumerable<FileInfo> osItems, List<(TDbFile DbFile, FileInfo FileInfo)> matchingPairs,
        out List<(List<TDbFile> DbFile, List<FileInfo> FileInfo)> partialMatches, out List<TDbFile> unmatchedDb, out List<FileInfo> unmatchedFs)
        where TDbFile : class, Model.IFile
    {
        List<TDbFile> udb1 = [];
        List<FileInfo> ufs1 = [];
        List<(List<TDbFile>, List<FileInfo>)> mm = [];
        List<(List<TDbFile>, List<FileInfo>)> pm = [];
        DateTime[] values = [.. dbItems.GroupBy(d => d.LastWriteTime).GroupJoin(osItems, g => g.Key, f => f.LastWriteTime, (g, f) => (Group: g, f.ToList())).Where(a =>
        {
            (IGrouping<DateTime, TDbFile> grp, List<FileInfo> fs) = a;
            List<TDbFile> db = [.. grp];
            if (db.Count == 1 && fs.Count == 1)
                matchingPairs.Add((db[0], fs[0]));
            else if (fs.Count > 0)
            {
                mm.AddRange(MatchByLength(db, fs, matchingPairs, out List<TDbFile> udb2, out List<FileInfo> ufs2));
                // udb2/ufs2 = Matches last write time but not length
                if (udb2.Count == 1 && ufs2.Count == 1)
                    matchingPairs.Add((udb2[0], ufs2[0]));
                else if (ufs2.Count > 0)
                    pm.Add((udb2, ufs2));
                else
                    udb1.AddRange(udb2);
            }
            else
            {
                udb1.AddRange(db);
                return false;
            }
            return true;
        }).Select(a => a.Group.Key)];
        unmatchedDb = udb1;
        ufs1.AddRange((values.Length == 0) ? osItems : osItems.Where(o => !values.Contains(o.LastWriteTime)));
        unmatchedFs = ufs1;
        partialMatches = pm;
        return mm;
    }

    /// <summary>
    /// Matches <see cref="ISubdirectory"/> objects with <see cref="DirectoryInfo"/> objects with the same last write time.
    /// </summary>
    /// <typeparam name="TSubdirectory">The type of the database entity.</typeparam>
    /// <param name="dbItems">The input database entity objects.</param>
    /// <param name="osItems">The input OS subdirectory objects to match up with corresponding <paramref name="dbItems"/>.</param>
    /// <param name="matchingPairs">Contains pairs of <typeparamref name="TSubdirectory"/> and <see cref="DirectoryInfo"/> objects with the same last write time.</param>
    /// <param name="unmatchedDb"><typeparamref name="TSubdirectory"/> objects where the last write time does not match the last write time of any <see cref="DirectoryInfo"/> objects.</param>
    /// <param name="unmatchedFs"><see cref="DirectoryInfo"/> objects where the last write time does not match the last write time of any <typeparamref name="TSubdirectory"/> objects.</param>
    /// <returns>A <c><see cref="List{T}">List</see>&lt;<see cref="ValueTuple{T1, T2}">ValueTuple</see>&lt;<see cref="List{TSubdirectory}" />, <see cref="List{DirectoryInfo}" />&gt;&gt;</c> representing sets of multiple <typeparamref name="TSubdirectory"/> and <see cref="DirectoryInfo"/> objects that share the same last write time.</returns>
    private static List<(List<TSubdirectory> DbDir, List<DirectoryInfo> DirectoryInfo)> MatchByLastWriteTime<TSubdirectory>(IEnumerable<TSubdirectory> dbItems, IEnumerable<DirectoryInfo> osItems,
        List<(TSubdirectory DbDir, DirectoryInfo DirectoryInfo)> matchingPairs, out List<TSubdirectory> unmatchedDb, out List<DirectoryInfo> unmatchedFs)
        where TSubdirectory : class, Model.ISubdirectory
    {
        List<TSubdirectory> udb = [];
        List<(List<TSubdirectory>, List<DirectoryInfo>)> mm = [];
        DateTime[] values = [.. dbItems.GroupBy(d => d.LastWriteTime).GroupJoin(osItems, g => g.Key, f => f.LastWriteTime, (g, f) => (Group: g, f.ToList())).Where(a =>
        {
            (IGrouping<DateTime, TSubdirectory> grp, List<DirectoryInfo> fs) = a;
            List<TSubdirectory> db = [.. grp];
            if (db.Count == 1 && fs.Count == 1)
                matchingPairs.Add((db[0], fs[0]));
            else if (fs.Count > 0)
                mm.Add((db, fs));
            else
            {
                udb.AddRange(db);
                return false;
            }
            return true;
        }).Select(a => a.Group.Key)];
        unmatchedDb = udb;
        unmatchedFs = [.. (values.Length == 0) ? osItems : osItems.Where(o => !values.Contains(o.LastWriteTime))];
        return mm;
    }

    /// <summary>
    /// Matches <see cref="IFile"/> objects with <see cref="FileInfo"/> objects with the same creation time.
    /// </summary>
    /// <typeparam name="TDbFile">The type of the database entity.</typeparam>
    /// <param name="dbItems">The input database entity objects.</param>
    /// <param name="osItems">The input OS file objects to match up with corresponding <paramref name="dbItems"/>.</param>
    /// <param name="matchingPairs">Contains pairs of <typeparamref name="TDbFile"/> and <see cref="FileInfo"/> objects with the same file length, last write time, and creation time.</param>
    /// <param name="partialMatches">Sets of multiple <typeparamref name="TDbFile"/> and <see cref="FileInfo"/> objects that share the same creation time and may also share the same last write time.</param>
    /// <param name="unmatchedDb"><typeparamref name="TDbFile"/> objects where neither the file length, last write time, nor creation time matches the file length, last write time or creation time of any <see cref="FileInfo"/> objects.</param>
    /// <param name="unmatchedFs"><see cref="FileInfo"/> objects where neither the file length, last write time, nor creation time matches the file length, last write time or creation time of any <typeparamref name="TDbFile"/> objects.</param>
    /// <returns>A <c><see cref="List{T}">List</see>&lt;<see cref="ValueTuple{T1, T2}">ValueTuple</see>&lt;<see cref="List{TDbFile}" />, <see cref="List{FileInfo}" />&gt;&gt;</c> representing sets of multiple <typeparamref name="TDbFile"/> and <see cref="FileInfo"/> objects that share the same file length, last write time, and creation time.</returns>
    private static List<(List<TDbFile> DbFile, List<FileInfo> FileInfo)> MatchByCreationTime<TDbFile>(IEnumerable<TDbFile> dbItems, IEnumerable<FileInfo> osItems, List<(TDbFile DbFile, FileInfo FileInfo)> matchingPairs,
        out List<(List<TDbFile> DbFile, List<FileInfo> FileInfo)> partialMatches, out List<TDbFile> unmatchedDb, out List<FileInfo> unmatchedFs)
        where TDbFile : class, Model.IFile
    {
        List<TDbFile> udb1 = [];
        List<FileInfo> ufs1 = [];
        List<(List<TDbFile>, List<FileInfo>)> mm = [];
        List<(List<TDbFile>, List<FileInfo>)> pm = [];
        DateTime[] values = [.. dbItems.GroupBy(d => d.CreationTime).GroupJoin(osItems, g => g.Key, f => f.CreationTime, (g, f) => (Group: g, f.ToList())).Where(a =>
        {
            (IGrouping<DateTime, TDbFile> grp, List<FileInfo> fs) = a;
            List<TDbFile> db = [.. grp];
            if (db.Count == 1 && fs.Count == 1)
                matchingPairs.Add((db[0], fs[0]));
            else if (fs.Count > 0)
            {
                mm.AddRange(MatchByLastWriteTime(db, fs, matchingPairs, out List<(List<TDbFile> DbFile, List<FileInfo> FileInfo)> pm2, out List<TDbFile> udb2, out List<FileInfo> ufs2));
                // pm2 = matches creation time and last write time, but not file length
                // udb2/ufs2 = matches creation time, but not last write time or file length.
                if (udb2.Count == 1 && ufs2.Count == 1)
                    matchingPairs.Add((udb2[0], ufs2[0]));
                else if (ufs2.Count > 0)
                {
                    pm.Add((udb2, ufs2));
                }
                else
                    udb1.AddRange(udb2);
                pm.AddRange(pm2);
            }
            else
            {
                udb1.AddRange(db);
                return false;
            }
            return true;
        }).Select(a => a.Group.Key)];
        ufs1.AddRange((values.Length == 0) ? osItems : osItems.Where(o => !values.Contains(o.CreationTime)));
        if (ufs1.Count > 1 && udb1.Count > 1)
            mm.AddRange(MatchByLength(udb1, ufs1, matchingPairs, out unmatchedDb, out unmatchedFs));
        else
        {
            unmatchedFs = ufs1;
            unmatchedDb = udb1;
        }
        partialMatches = pm;
        return mm;
    }

    /// <summary>
    /// Matches <see cref="ISubdirectory"/> objects with <see cref="DirectoryInfo"/> objects with the same creation time.
    /// </summary>
    /// <typeparam name="TSubdirectory">The type of the database entity.</typeparam>
    /// <param name="dbItems">The input database entity objects.</param>
    /// <param name="osItems">The input OS file objects to match up with corresponding <paramref name="dbItems"/>.</param>
    /// <param name="matchingPairs">Contains pairs of <typeparamref name="TSubdirectory"/> and <see cref="DirectoryInfo"/> objects with the same last write time and creation time.</param>
    /// <param name="partialMatches">Sets of multiple <typeparamref name="TSubdirectory"/> and <see cref="DirectoryInfo"/> objects that share only the same last write time with no matching creation time.</param>
    /// <param name="unmatchedDb"><typeparamref name="TSubdirectory"/> objects where neither the last write time nor creation time matches the last write time or creation time of any <see cref="DirectoryInfo"/> objects.</param>
    /// <param name="unmatchedFs"><see cref="DirectoryInfo"/> objects where neither the last write time nor creation time matches the last write time or creation time of any <typeparamref name="TSubdirectory"/> objects.</param>
    /// <returns>A <c><see cref="List{T}">List</see>&lt;<see cref="ValueTuple{T1, T2}">ValueTuple</see>&lt;<see cref="List{TSubdirectory}" />, <see cref="List{DirectoryInfo}" />&gt;&gt;</c> representing sets of multiple <typeparamref name="TSubdirectory"/> and <see cref="DirectoryInfo"/> objects that share the same last write time and creation time.</returns>
    private static List<(List<TSubdirectory> DbDir, List<DirectoryInfo> DirectoryInfo)> MatchByCreationTime<TSubdirectory>(IEnumerable<TSubdirectory> dbItems, IEnumerable<DirectoryInfo> osItems, List<(TSubdirectory DbDir, DirectoryInfo DirectoryInfo)> matchingPairs,
        out List<(List<TSubdirectory> DbDir, List<DirectoryInfo> DirectoryInfo)> partialMatches, out List<TSubdirectory> unmatchedDb, out List<DirectoryInfo> unmatchedFs)
        where TSubdirectory : class, Model.ISubdirectory
    {
        List<TSubdirectory> udb1 = [];
        List<DirectoryInfo> ufs1 = [];
        List<(List<TSubdirectory>, List<DirectoryInfo>)> mm = [];
        List<(List<TSubdirectory>, List<DirectoryInfo>)> pm = [];
        DateTime[] values = [.. dbItems.GroupBy(d => d.CreationTime).GroupJoin(osItems, g => g.Key, f => f.CreationTime, (g, f) => (Group: g, f.ToList())).Where(a =>
        {
            (IGrouping<DateTime, TSubdirectory> grp, List<DirectoryInfo> fs) = a;
            List<TSubdirectory> db = [.. grp];
            if (db.Count == 1 && fs.Count == 1)
                matchingPairs.Add((db[0], fs[0]));
            else if (fs.Count > 0)
            {
                mm.AddRange(MatchByLastWriteTime(db, fs, matchingPairs, out List<TSubdirectory> udb2, out List<DirectoryInfo> ufs2));
                if (udb2.Count == 1 && ufs2.Count == 1)
                    matchingPairs.Add((udb2[0], ufs2[0]));
                else if (ufs2.Count > 0)
                    pm.Add((udb2, ufs2));
                else
                    udb1.AddRange(udb2);
            }
            else
            {
                udb1.AddRange(db);
                return false;
            }
            return true;
        }).Select(a => a.Group.Key)];
        unmatchedDb = udb1;
        ufs1.AddRange((values.Length == 0) ? osItems : osItems.Where(o => !values.Contains(o.CreationTime)));
        unmatchedFs = ufs1;
        partialMatches = pm;
        return mm;
    }

    /// <summary>
    /// Matches <see cref="IFile"/> objects with <see cref="FileInfo"/> objects with the same name.
    /// </summary>
    /// <typeparam name="TDbFile">The type of the database entity.</typeparam>
    /// <param name="dbItems">The input database entity objects.</param>
    /// <param name="osItems">The input OS file objects to match up with corresponding <paramref name="dbItems"/>.</param>
    /// <param name="matchingPairs">Contains pairs of <typeparamref name="TDbFile"/> and <see cref="FileInfo"/> objects with the same last write time and creation time.</param>
    /// <param name="comparer">The name comparer.</param>
    /// <param name="partialMatches">Sets of multiple <typeparamref name="TDbFile"/> and <see cref="FileInfo"/> objects that share the same name and may also share the same creation time or last write time.</param>
    /// <param name="unmatchedDb"><typeparamref name="TDbFile"/> objects where neither the name, file length, last write time, nor creation time matches the name, file length, last write time or creation time of any <see cref="FileInfo"/> objects.</param>
    /// <param name="unmatchedFs"><see cref="FileInfo"/> objects where neither the name, file length, last write time, nor creation time matches the file name, length, last write time or creation time of any <typeparamref name="TDbFile"/> objects.</param>
    /// <returns>A <c><see cref="List{T}">List</see>&lt;<see cref="ValueTuple{T1, T2}">ValueTuple</see>&lt;<see cref="List{TDbFile}" />, <see cref="List{FileInfo}" />&gt;&gt;</c> representing sets of multiple <typeparamref name="TDbFile"/> and <see cref="FileInfo"/> objects that share the same name, file length, last write time, and creation time.</returns>
    private static List<(List<TDbFile> DbFile, List<FileInfo> FileInfo)> MatchByName<TDbFile>(IEnumerable<TDbFile> dbItems, IEnumerable<FileInfo> osItems, List<(TDbFile DbFile, FileInfo FileInfo)> matchingPairs, StringComparer comparer,
        out List<(List<TDbFile> DbFile, List<FileInfo> FileInfo)> partialMatches, out List<TDbFile> unmatchedDb, out List<FileInfo> unmatchedFs)
        where TDbFile : class, Model.IFile
    {
        List<TDbFile> udb1 = [];
        List<FileInfo> ufs1 = [];
        List<(List<TDbFile>, List<FileInfo>)> mm = [];
        List<(List<TDbFile>, List<FileInfo>)> pm = [];
        string[] values = [.. dbItems.GroupBy(d => d.Name ?? "", comparer).GroupJoin(osItems, g => g.Key, f => f.Name, (g, f) => (Group: g, f.ToList()), comparer).Where(a =>
        {
            (IGrouping<string, TDbFile> grp, List<FileInfo> fs) = a;
            List<TDbFile> db = [.. grp];
            if (db.Count == 1 && fs.Count == 1)
                matchingPairs.Add((db[0], fs[0]));
            else if (fs.Count > 0)
            {
                mm.AddRange(MatchByCreationTime(db, fs, matchingPairs, out List<(List<TDbFile> DbFile, List<FileInfo> FileInfo)> pm2, out List<TDbFile> udb2, out List<FileInfo> ufs2));
                // pm2 = matches name and creation time (may match last write time)
                // udb2/ufs2 = matches name, but not creation time, file length or last write time
                if (udb2.Count == 1 && ufs2.Count == 1)
                    matchingPairs.Add((udb2[0], ufs2[0]));
                else if (ufs2.Count > 0)
                    pm.Add((udb2, ufs2));
                else
                    udb1.AddRange(udb2);
                pm.AddRange(pm2);
            }
            else
            {
                udb1.AddRange(db);
                return false;
            }
            return true;
        }).Select(a => a.Group.Key)];
        ufs1.AddRange((values.Length == 0) ? osItems : osItems.Where(o => !values.Contains(o.Name ?? "", comparer)));
        if (ufs1.Count > 1 && udb1.Count > 1)
        {
            mm.AddRange(MatchByLastWriteTime(udb1, ufs1, matchingPairs, out List<(List<TDbFile> DbFile, List<FileInfo> FileInfo)> pm2, out unmatchedDb, out unmatchedFs));
            pm.AddRange(pm2);
        }
        else
        {
            unmatchedDb = udb1;
            unmatchedFs = ufs1;
        }
        partialMatches = pm;
        return mm;
    }

    /// <summary>
    /// Matches <see cref="ISubdirectory"/> objects with <see cref="DirectoryInfo"/> objects with the same creation time.
    /// </summary>
    /// <typeparam name="TSubdirectory">The type of the database entity.</typeparam>
    /// <param name="dbItems">The input database entity objects.</param>
    /// <param name="osItems">The input OS file objects to match up with corresponding <paramref name="dbItems"/>.</param>
    /// <param name="comparer">The name comparer.</param>
    /// <param name="matchingPairs">Contains pairs of <typeparamref name="TSubdirectory"/> and <see cref="DirectoryInfo"/> objects with the same last write time and creation time.</param>
    /// <param name="partialMatches">Sets of multiple <typeparamref name="TSubdirectory"/> and <see cref="DirectoryInfo"/> objects that share the same name and may also share the same creation time or last write time.</param>
    /// <param name="unmatchedDb"><typeparamref name="TSubdirectory"/> objects where neither the last write time nor creation time matches the last write time or creation time of any <see cref="DirectoryInfo"/> objects.</param>
    /// <param name="unmatchedFs"><see cref="DirectoryInfo"/> objects where neither the last write time nor creation time matches the last write time or creation time of any <typeparamref name="TSubdirectory"/> objects.</param>
    /// <returns>A <c><see cref="List{T}">List</see>&lt;<see cref="ValueTuple{T1, T2}">ValueTuple</see>&lt;<see cref="List{TSubdirectory}" />, <see cref="List{DirectoryInfo}" />&lt;&gt;</c> representing sets of multiple <typeparamref name="TSubdirectory"/> and <see cref="DirectoryInfo"/> objects that share the same name, last write time and creation time.</returns>
    private static List<(List<TSubdirectory> DbDir, List<DirectoryInfo> DirectoryInfo)> MatchByName<TSubdirectory>(IEnumerable<TSubdirectory> dbItems, IEnumerable<DirectoryInfo> osItems, List<(TSubdirectory DbDir, DirectoryInfo DirectoryInfo)> matchingPairs,
        StringComparer comparer, out List<(List<TSubdirectory> DbDir, List<DirectoryInfo> DirectoryInfo)> partialMatches, out List<TSubdirectory> unmatchedDb, out List<DirectoryInfo> unmatchedFs)
        where TSubdirectory : class, Model.ISubdirectory
    {
        List<TSubdirectory> udb1 = [];
        List<DirectoryInfo> ufs1 = [];
        List<(List<TSubdirectory>, List<DirectoryInfo>)> mm = [];
        List<(List<TSubdirectory>, List<DirectoryInfo>)> pm = [];
        string[] values = [.. dbItems.GroupBy(d => d.Name ?? "", comparer).GroupJoin(osItems, g => g.Key, f => f.Name, (g, f) => (Group: g, f.ToList()), comparer).Where(a =>
        {
            (IGrouping<string, TSubdirectory> grp, List<DirectoryInfo> fs) = a;
            List<TSubdirectory> db = [.. grp];
            if (db.Count == 1 && fs.Count == 1)
                matchingPairs.Add((db[0], fs[0]));
            else if (fs.Count > 0)
            {
                mm.AddRange(MatchByCreationTime(db, fs, matchingPairs, out List<(List<TSubdirectory> DbDir, List<DirectoryInfo> DirectoryInfo)> pm2, out List<TSubdirectory> udb2, out List<DirectoryInfo> ufs2));
                if (udb2.Count == 1 && ufs2.Count == 1)
                    matchingPairs.Add((udb2[0], ufs2[0]));
                else if (ufs2.Count > 0)
                    pm.Add((udb2, ufs2));
                else
                    udb1.AddRange(udb2);
                pm.AddRange(pm2);
            }
            else
            {
                udb1.AddRange(db);
                return false;
            }
            return true;
        }).Select(a => a.Group.Key)];
        ufs1.AddRange((values.Length == 0) ? osItems : osItems.Where(o => !values.Contains(o.Name ?? "", comparer)));
        if (ufs1.Count > 1 && udb1.Count > 1)
            mm.AddRange(MatchByLastWriteTime(udb1, ufs1, matchingPairs, out unmatchedDb, out unmatchedFs));
        else
        {
            unmatchedDb = udb1;
            unmatchedFs = ufs1;
        }
        partialMatches = pm;
        return mm;
    }

    /// <summary>
    /// Matches <see cref="IFile"/> objects with <see cref="FileInfo"/> objects with the same name and other matching property values.
    /// </summary>
    /// <typeparam name="TDbFile">The type of the database entity.</typeparam>
    /// <param name="source1">The input database entity objects.</param>
    /// <param name="source2">The input OS file objects to match up with corresponding <paramref name="source2"/>.</param>
    /// <param name="unmatchedDb"><typeparamref name="TDbFile"/> objects where neither the length, last write time nor creation time matches the length, last write time or creation time of any <see cref="FileInfo"/> objects.</param>
    /// <param name="unmatchedFs"><see cref="FileInfo"/> objects where neither the length, last write time nor creation time matches the length, last write time or creation time of any <typeparamref name="TDbFile"/> objects.</param>
    /// <returns>A <c><see cref="List{T}">List</see>&lt;<see cref="ValueTuple{TDbFile, FileInfo}" />&gt;</c> representing sets of multiple <typeparamref name="TDbFile"/> and <see cref="FileInfo"/> objects that share the same name, last write time, creation time and length.</returns>
    public static List<(TDbFile DbFile, FileInfo FileInfo)> ToMatchedPairs<TDbFile>(this IEnumerable<TDbFile> source1, IEnumerable<FileInfo> source2, out List<TDbFile> unmatchedDb, out List<FileInfo> unmatchedFs)
        where TDbFile : class, Model.IFile
    {
        List<(TDbFile DbFile, FileInfo FileInfo)> matchingPairs = [];
        List<(List<TDbFile> DbFile, List<FileInfo> FileInfo)> partialMatches;
        foreach ((List<TDbFile> multiMatchDb, List<FileInfo> multiMatchFs) in MatchByName(source1, source2, matchingPairs, StringComparer.InvariantCultureIgnoreCase, out partialMatches, out unmatchedDb, out unmatchedFs))
        {
            List<TDbFile> udb;
            List<FileInfo> ufs;
            List<(List<TDbFile> DbFile, List<FileInfo> FileInfo)> pm;
            foreach ((List<TDbFile> mmDb, List<FileInfo> mmFs) in MatchByName(multiMatchDb, multiMatchFs, matchingPairs, StringComparer.InvariantCulture, out pm, out udb, out ufs))
            {
                if (mmDb.Count > mmFs.Count)
                {
                    matchingPairs.AddRange(mmDb.Take(mmFs.Count).OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).ThenBy(d => d.BinaryProperties.Length).Zip(mmFs.OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime).ThenBy(f => f.Length)));
                    udb.AddRange(mmDb.Skip(mmFs.Count));
                }
                else if (mmDb.Count < mmFs.Count)
                {
                    matchingPairs.AddRange(mmDb.OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).ThenBy(d => d.BinaryProperties.Length).Zip(mmFs.Take(mmDb.Count).OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime).ThenBy(f => f.Length)));
                    ufs.AddRange(mmFs.Skip(mmDb.Count));
                }
                else
                    matchingPairs.AddRange(mmDb.OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).ThenBy(d => d.BinaryProperties.Length).Zip(mmFs.OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime).ThenBy(f => f.Length)));
            }
            partialMatches.AddRange(pm);
            if (udb.Count == 1 && ufs.Count == 1)
                matchingPairs.Add((udb[0], ufs[0]));
            else if (udb.Count > 0)
            {
                if (ufs.Count > 0)
                {
                    if (udb.Count > ufs.Count)
                    {
                        matchingPairs.AddRange(udb.Take(ufs.Count).OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).ThenBy(d => d.BinaryProperties.Length).Zip(ufs.OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime).ThenBy(f => f.Length)));
                        unmatchedDb.AddRange(udb.Skip(ufs.Count));
                    }
                    else if (udb.Count < ufs.Count)
                    {
                        matchingPairs.AddRange(udb.OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).ThenBy(d => d.BinaryProperties.Length).Zip(ufs.Take(udb.Count).OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime).ThenBy(f => f.Length)));
                        unmatchedFs.AddRange(ufs.Skip(udb.Count));
                    }
                    else
                        matchingPairs.AddRange(udb.OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).ThenBy(d => d.BinaryProperties.Length).Zip(ufs.OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime).ThenBy(f => f.Length)));
                }
                else
                    unmatchedDb.AddRange(udb);
            }
            else if (ufs.Count > 0)
                unmatchedFs.AddRange(ufs);
        }
        foreach ((List<TDbFile> db, List<FileInfo> fs) in partialMatches)
        {
            if (db.Count > fs.Count)
            {
                matchingPairs.AddRange(db.Take(fs.Count).OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).ThenBy(d => d.BinaryProperties.Length).Zip(fs.OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime).ThenBy(f => f.Length)));
                unmatchedDb.AddRange(db.Skip(fs.Count));
            }
            else if (db.Count < fs.Count)
            {
                matchingPairs.AddRange(db.OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).ThenBy(d => d.BinaryProperties.Length).Zip(fs.Take(db.Count).OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime).ThenBy(f => f.Length)));
                unmatchedFs.AddRange(fs.Skip(db.Count));
            }
            else
                matchingPairs.AddRange(db.OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).ThenBy(d => d.BinaryProperties.Length).Zip(fs.OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime).ThenBy(f => f.Length)));
        }
        return matchingPairs;
    }

    /// <summary>
    /// Matches <see cref="ISubdirectory"/> objects with <see cref="DirectoryInfo"/> objects with the same name and other matching property values.
    /// </summary>
    /// <typeparam name="TSubdirectory">The type of the database entity.</typeparam>
    /// <param name="source1">The input database entity objects.</param>
    /// <param name="source2">The input OS subdirectory objects to match up with corresponding <paramref name="source2"/>.</param>
    /// <param name="unmatchedDb"><typeparamref name="TSubdirectory"/> objects where neither the last write time nor creation time matches the last write time or creation time of any <see cref="DirectoryInfo"/> objects.</param>
    /// <param name="unmatchedFs"><see cref="DirectoryInfo"/> objects where neither the last write time nor creation time matches the last write time or creation time of any <typeparamref name="TSubdirectory"/> objects.</param>
    /// <returns>A <c><see cref="List{T}">List</see>&lt;<see cref="ValueTuple{TSubdirectory, DirectoryInfo}" />&gt;</c> representing sets of multiple <typeparamref name="TSubdirectory"/> and <see cref="DirectoryInfo"/> objects that share the same name, last write time and creation time.</returns>
    public static List<(TSubdirectory Subdirectory, DirectoryInfo DirectoryInfo)> ToMatchedPairs<TSubdirectory>(this IEnumerable<TSubdirectory> source1, IEnumerable<DirectoryInfo> source2, out List<TSubdirectory> unmatchedDb, out List<DirectoryInfo> unmatchedFs)
        where TSubdirectory : class, Model.ISubdirectory
    {
        List<(TSubdirectory DbDir, DirectoryInfo DirectoryInfo)> matchingPairs = [];
        List<(List<TSubdirectory> DbDir, List<DirectoryInfo> DirectoryInfo)> partialMatches;
        foreach ((List<TSubdirectory> multiMatchDb, List<DirectoryInfo> multiMatchFs) in MatchByName(source1, source2, matchingPairs, StringComparer.InvariantCultureIgnoreCase, out partialMatches, out unmatchedDb, out unmatchedFs))
        {
            List<TSubdirectory> udb;
            List<DirectoryInfo> ufs;
            List<(List<TSubdirectory> DbDir, List<DirectoryInfo> DirectoryInfo)> pm;
            foreach ((List<TSubdirectory> mmDb, List<DirectoryInfo> mmFs) in MatchByName(multiMatchDb, multiMatchFs, matchingPairs, StringComparer.InvariantCulture, out pm, out udb, out ufs))
            {
                if (mmDb.Count > mmFs.Count)
                {
                    matchingPairs.AddRange(mmDb.Take(mmFs.Count).OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).Zip(mmFs.OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime)));
                    udb.AddRange(mmDb.Skip(mmFs.Count));
                }
                else if (mmDb.Count < mmFs.Count)
                {
                    matchingPairs.AddRange(mmDb.OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).Zip(mmFs.Take(mmDb.Count).OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime)));
                    ufs.AddRange(mmFs.Skip(mmDb.Count));
                }
                else
                    matchingPairs.AddRange(mmDb.OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).Zip(mmFs.OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime)));
            }
            partialMatches.AddRange(pm);
            if (udb.Count == 1 && ufs.Count == 1)
                matchingPairs.Add((udb[0], ufs[0]));
            else if (udb.Count > 0)
            {
                if (ufs.Count > 0)
                {
                    if (udb.Count > ufs.Count)
                    {
                        matchingPairs.AddRange(udb.Take(ufs.Count).OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).Zip(ufs.OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime)));
                        unmatchedDb.AddRange(udb.Skip(ufs.Count));
                    }
                    else if (udb.Count < ufs.Count)
                    {
                        matchingPairs.AddRange(udb.OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).Zip(ufs.Take(udb.Count).OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime)));
                        unmatchedFs.AddRange(ufs.Skip(udb.Count));
                    }
                    else
                        matchingPairs.AddRange(udb.OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).Zip(ufs.OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime)));
                }
                else
                    unmatchedDb.AddRange(udb);
            }
            else if (ufs.Count > 0)
                unmatchedFs.AddRange(ufs);
        }
        foreach ((List<TSubdirectory> db, List<DirectoryInfo> fs) in partialMatches)
        {
            if (db.Count > fs.Count)
            {
                matchingPairs.AddRange(db.Take(fs.Count).OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).Zip(fs.OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime)));
                unmatchedDb.AddRange(db.Skip(fs.Count));
            }
            else if (db.Count < fs.Count)
            {
                matchingPairs.AddRange(db.OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).Zip(fs.Take(db.Count).OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime)));
                unmatchedFs.AddRange(fs.Skip(db.Count));
            }
            else
                matchingPairs.AddRange(db.OrderBy(d => d.CreationTime).ThenBy(d => d.LastWriteTime).Zip(fs.OrderBy(f => f.CreationTime).ThenBy(f => f.LastWriteTime)));
        }
        return matchingPairs;
    }

    /// <summary>
    /// Determines whether 2 <see cref="Nullable{T}"/>, <see cref="IEquatable{T}"/> values are equal.
    /// </summary>
    /// <param name="x">The first nullable value.</param>
    /// <param name="y">The second nullable value.</param>
    /// <returns><see langword="true"/> if both values are <see langword="null"/> or both values are equal; otherwise, <see langword="false"/>.</returns>
    public static bool NullablesEqual<T>(T? x, T? y) where T : struct, IEquatable<T> => x.HasValue ? (y.HasValue && x.Value.Equals(y.Value)) : !y.HasValue;
}
