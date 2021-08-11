using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading;

namespace FsInfoCat
{
    partial class DeferredDelegationService : IDeferredDelegationService
    {
        private readonly LinkedList<IDeferenceCollection> _deferences = new();

        [ServiceBuilderHandler]
#pragma warning disable IDE0051 // Remove unused private members
        private static void ConfigureServices(IServiceCollection services)
#pragma warning restore IDE0051 // Remove unused private members
        {
            services.AddSingleton<IDeferredDelegationService, DeferredDelegationService>();
        }

        /// <summary>
        /// Tries the get value <see cref="ICollection.SyncRoot"/> property.
        /// </summary>
        /// <param name="collection">The target collection.</param>
        /// <param name="syncRoot">The value if the value <see cref="ICollection.SyncRoot"/> property or <see langword="null"/> if the collection is not synchronzied.</param>
        /// <returns><see langword="true"/> if <see cref="ICollection.IsSynchronized"/> is <see langword="true"/>;
        /// otherwise, <see langword="false"/> if <paramref name="collection"/> is null, <see cref="ICollection.IsSynchronized"/> is <see langword="null"/>,
        /// or <see cref="ICollection.SyncRoot"/> is null or could not be accessed.</returns>
        public static bool TryGetSyncRoot([AllowNull] ICollection collection, out object syncRoot)
        {
            if (collection is not null && collection.IsSynchronized)
            {
                try
                {
                    if ((syncRoot = collection.SyncRoot) is not null)
                        return true;
                }
                catch { /* Makes no difference in this context if exception was thrown */ }
            }
            syncRoot = null;
            return false;
        }

        /// <summary>
        /// Tries the get value <see cref="ICollection.SyncRoot"/> property.
        /// </summary>
        /// <param name="target">The target collection.</param>
        /// <param name="syncRoot">The value if the value <see cref="ICollection.SyncRoot"/> property or <see langword="null"/> if the collection is not synchronzied.</param>
        /// <returns><see langword="true"/> if <see cref="ICollection.IsSynchronized"/> is <see langword="true"/>;
        /// otherwise, <see langword="false"/> if <paramref name="target"/> is null, <see cref="ICollection.IsSynchronized"/> is <see langword="null"/>,
        /// or <see cref="ICollection.SyncRoot"/> is null or could not be accessed.</returns>
        public static bool TryGetSyncRoot([AllowNull] ISynchronizable target, out object syncRoot)
        {
            if (target is not null)
            {
                try
                {
                    if ((syncRoot = target.SyncRoot) is not null)
                        return true;
                }
                catch { /* Makes no difference in this context if exception was thrown */ }
            }
            syncRoot = null;
            return false;
        }

        bool TryFind<T>(T instance, out DelegateDeference<T>.DeferenceCollection collection)
            where T : class
        {
            Monitor.Enter(_deferences);
            try
            {
                LinkedListNode<IDeferenceCollection> node = _deferences.First;
                while (node is not null)
                {
                    if (node.Value is DelegateDeference<T>.DeferenceCollection deferenceCollection && ReferenceEquals(deferenceCollection.Target, instance))
                    {
                        LinkedListNode<IDeferenceCollection> next = node;
                        if (deferenceCollection.Verify())
                        {
                            collection = deferenceCollection;
                            return true;
                        }
                        break;
                    }
                    node = node.Next;
                }
            }
            finally { Monitor.Exit(_deferences); }
            collection = null;
            return false;
        }

        public bool IsDeferred<T>(T target) where T : class => TryFind(target, out _);

        public IDelegateDeference<T> Enter<T>([DisallowNull] T target) where T : class
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                return DelegateDeference<T>.Enter(collection);
            return DelegateDeference<T>.DeferenceCollection.EnterNew(this, target, new());
        }

        public IDelegateDeference<T> Enter<T>([DisallowNull] T target, ref bool lockTaken) where T : class
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                return DelegateDeference<T>.Enter(collection, ref lockTaken);
            return DelegateDeference<T>.DeferenceCollection.EnterNew(this, target, new(), ref lockTaken);
        }

        public IDelegateDeference<T> EnterSynchronized<T>([DisallowNull] T target) where T : class, ICollection
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                return DelegateDeference<T>.Enter(collection);
            if (TryGetSyncRoot(target, out object syncRoot))
                return DelegateDeference<T>.DeferenceCollection.EnterNew(this, target, syncRoot);
            throw new ArgumentOutOfRangeException(nameof(target));
        }

        public IDelegateDeference<T> EnterSynchronized<T>([DisallowNull] T target, ref bool lockTaken) where T : class, ICollection
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                return DelegateDeference<T>.Enter(collection, ref lockTaken);
            if (TryGetSyncRoot(target, out object syncRoot))
                return DelegateDeference<T>.DeferenceCollection.EnterNew(this, target, syncRoot, ref lockTaken);
            throw new ArgumentOutOfRangeException(nameof(target));
        }

        public bool TryEnterSynchronized<T>([DisallowNull] T target, out IDelegateDeference<T> deference) where T : class, ICollection
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                return DelegateDeference<T>.TryEnter(collection, out deference);
            if (TryGetSyncRoot(target, out object syncRoot))
                return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, out deference);
            throw new ArgumentOutOfRangeException(nameof(target));
        }

        public bool TryEnterSynchronized<T>([DisallowNull] T target, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ICollection
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                return DelegateDeference<T>.TryEnter(collection, ref lockTaken, out deference);
            if (TryGetSyncRoot(target, out object syncRoot))
                return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, ref lockTaken, out deference);
            throw new ArgumentOutOfRangeException(nameof(target));
        }

        public bool TryEnterSynchronized<T>([DisallowNull] T target, int millisecondsTimeout, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ICollection
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                return DelegateDeference<T>.TryEnter(collection, millisecondsTimeout, ref lockTaken, out deference);
            if (TryGetSyncRoot(target, out object syncRoot))
                return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, millisecondsTimeout, ref lockTaken, out deference);
            throw new ArgumentOutOfRangeException(nameof(target));
        }

        public bool TryEnterSynchronized<T>([DisallowNull] T target, TimeSpan timeout, out IDelegateDeference<T> deference) where T : class, ICollection
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                return DelegateDeference<T>.TryEnter(collection, timeout, out deference);
            if (TryGetSyncRoot(target, out object syncRoot))
                return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, timeout, out deference);
            throw new ArgumentOutOfRangeException(nameof(target));
        }

        public bool TryEnterSynchronized<T>([DisallowNull] T target, TimeSpan timeout, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ICollection
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                return DelegateDeference<T>.TryEnter(collection, timeout, ref lockTaken, out deference);
            if (TryGetSyncRoot(target, out object syncRoot))
                return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, timeout, ref lockTaken, out deference);
            throw new ArgumentOutOfRangeException(nameof(target));
        }

        public bool TryEnterSynchronized<T>([DisallowNull] T target, int millisecondsTimeout, out IDelegateDeference<T> deference) where T : class, ICollection
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                return DelegateDeference<T>.TryEnter(collection, millisecondsTimeout, out deference);
            if (TryGetSyncRoot(target, out object syncRoot))
                return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, millisecondsTimeout, out deference);
            throw new ArgumentOutOfRangeException(nameof(target));
        }

        public IDelegateDeference<T> EnterSynchronizable<T>([DisallowNull] T target) where T : class, ISynchronizable
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                return DelegateDeference<T>.Enter(collection);
            if (TryGetSyncRoot(target, out object syncRoot))
                return DelegateDeference<T>.DeferenceCollection.EnterNew(this, target, syncRoot);
            throw new ArgumentOutOfRangeException(nameof(target));
        }

        public IDelegateDeference<T> EnterSynchronizable<T>([DisallowNull] T target, ref bool lockTaken) where T : class, ISynchronizable
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                return DelegateDeference<T>.Enter(collection, ref lockTaken);
            if (TryGetSyncRoot(target, out object syncRoot))
                return DelegateDeference<T>.DeferenceCollection.EnterNew(this, target, syncRoot, ref lockTaken);
            throw new ArgumentOutOfRangeException(nameof(target));
        }

        public bool TryEnterSynchronizable<T>([DisallowNull] T target, out IDelegateDeference<T> deference) where T : class, ISynchronizable
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                return DelegateDeference<T>.TryEnter(collection, out deference);
            if (TryGetSyncRoot(target, out object syncRoot))
                return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, out deference);
            throw new ArgumentOutOfRangeException(nameof(target));
        }

        public bool TryEnterSynchronizable<T>([DisallowNull] T target, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ISynchronizable
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                return DelegateDeference<T>.TryEnter(collection, ref lockTaken, out deference);
            if (TryGetSyncRoot(target, out object syncRoot))
                return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, ref lockTaken, out deference);
            throw new ArgumentOutOfRangeException(nameof(target));
        }

        public bool TryEnterSynchronizable<T>([DisallowNull] T target, int millisecondsTimeout, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ISynchronizable
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                return DelegateDeference<T>.TryEnter(collection, millisecondsTimeout, ref lockTaken, out deference);
            if (TryGetSyncRoot(target, out object syncRoot))
                return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, millisecondsTimeout, ref lockTaken, out deference);
            throw new ArgumentOutOfRangeException(nameof(target));
        }

        public bool TryEnterSynchronizable<T>([DisallowNull] T target, TimeSpan timeout, out IDelegateDeference<T> deference) where T : class, ISynchronizable
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                return DelegateDeference<T>.TryEnter(collection, out deference);
            if (TryGetSyncRoot(target, out object syncRoot))
                return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, out deference);
            throw new ArgumentOutOfRangeException(nameof(target));
        }

        public bool TryEnterSynchronizable<T>([DisallowNull] T target, TimeSpan timeout, ref bool lockTaken, out IDelegateDeference<T> deference) where T : class, ISynchronizable
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                return DelegateDeference<T>.TryEnter(collection, timeout, ref lockTaken, out deference);
            if (TryGetSyncRoot(target, out object syncRoot))
                return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, timeout, ref lockTaken, out deference);
            throw new ArgumentOutOfRangeException(nameof(target));
        }

        public bool TryEnterSynchronizable<T>([DisallowNull] T target, int millisecondsTimeout, out IDelegateDeference<T> deference) where T : class, ISynchronizable
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (TryFind(target, out DelegateDeference<T>.DeferenceCollection collection))
                return DelegateDeference<T>.TryEnter(collection, millisecondsTimeout, out deference);
            if (TryGetSyncRoot(target, out object syncRoot))
                return DelegateDeference<T>.DeferenceCollection.TryEnterNew(this, target, syncRoot, millisecondsTimeout, out deference);
            throw new ArgumentOutOfRangeException(nameof(target));
        }
    }

    interface IDeferenceCollection : IDelegateDeference
    {

    }
}
