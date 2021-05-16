using FsInfoCat.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace FsInfoCat.Internal
{
    internal class CollectionsService : ICollectionsService
    {
        // TODO: Implement methods
        public IReadOnlyCollection<T> AsReadOnlyCollection<T>(IReadOnlyCollection<T> source)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<T> AsReadOnlyCollection<T>(ICollection<T> source)
        {
            throw new NotImplementedException();
        }

        public IObservable<IReadOnlyCollection<T>> AsReadOnlyCollection<T>(IObservable<IReadOnlyCollection<T>> source)
        {
            throw new NotImplementedException();
        }

        public IObservable<IReadOnlyCollection<T>> AsReadOnlyCollection<T>(IObservable<ICollection<T>> source)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyDictionary<TKey, TValue> AsReadOnlyDictionary<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> source)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyDictionary<TKey, TValue> AsReadOnlyDictionary<TKey, TValue>(IDictionary<TKey, TValue> source)
        {
            throw new NotImplementedException();
        }

        public IObservable<IReadOnlyDictionary<TKey, TValue>> AsReadOnlyDictionary<TKey, TValue>(IObservable<IReadOnlyDictionary<TKey, TValue>> source)
        {
            throw new NotImplementedException();
        }

        public IObservable<IReadOnlyDictionary<TKey, TValue>> AsReadOnlyDictionary<TKey, TValue>(IObservable<IDictionary<TKey, TValue>> source)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<T> AsReadOnlyList<T>(IReadOnlyList<T> source)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<T> AsReadOnlyList<T>(IList<T> source)
        {
            throw new NotImplementedException();
        }

        public IObservable<IReadOnlyList<T>> AsReadOnlyList<T>(IObservable<IReadOnlyList<T>> source)
        {
            throw new NotImplementedException();
        }

        public IObservable<IReadOnlyList<T>> AsReadOnlyList<T>(IObservable<IList<T>> source)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TTarget> CastCollection<TSource, TTarget>(Func<IReadOnlyCollection<TSource>> accessor) where TSource : TTarget
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TTarget> CastCollection<TSource, TTarget>(Func<IReadOnlyCollection<TSource>> accessor, IEqualityComparer<TTarget> itemComparer) where TSource : TTarget
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TTarget> CastCollection<TSource, TTarget>(Func<ICollection<TSource>> accessor) where TSource : TTarget
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<TTarget> CastCollection<TSource, TTarget>(Func<ICollection<TSource>> accessor, IEqualityComparer<TTarget> itemComparer) where TSource : TTarget
        {
            throw new NotImplementedException();
        }

        public IReadOnlyDictionary<TKey, TTarget> CastDictionary<TKey, TSource, TTarget>(Func<IReadOnlyDictionary<TKey, TSource>> accessor) where TSource : TTarget
        {
            throw new NotImplementedException();
        }

        public IReadOnlyDictionary<TKey, TTarget> CastDictionary<TKey, TSource, TTarget>(Func<IReadOnlyDictionary<TKey, TSource>> accessor, IEqualityComparer<TTarget> valueComparer) where TSource : TTarget
        {
            throw new NotImplementedException();
        }

        public IReadOnlyDictionary<TKey, TTarget> CastDictionary<TKey, TSource, TTarget>(Func<IDictionary<TKey, TSource>> accessor) where TSource : TTarget
        {
            throw new NotImplementedException();
        }

        public IReadOnlyDictionary<TKey, TTarget> CastDictionary<TKey, TSource, TTarget>(Func<IDictionary<TKey, TSource>> accessor, IEqualityComparer<TTarget> valueComparer) where TSource : TTarget
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<TTarget> CastList<TSource, TTarget>(Func<IReadOnlyList<TSource>> accessor) where TSource : TTarget
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<TTarget> CastList<TSource, TTarget>(Func<IReadOnlyList<TSource>> accessor, IEqualityComparer<TTarget> itemComparer) where TSource : TTarget
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<TTarget> CastList<TSource, TTarget>(Func<IList<TSource>> accessor) where TSource : TTarget
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<TTarget> CastList<TSource, TTarget>(Func<IList<TSource>> accessor, IEqualityComparer<TTarget> itemComparer) where TSource : TTarget
        {
            throw new NotImplementedException();
        }

        public ICollection<T> Collection<T>(params T[] items)
        {
            throw new NotImplementedException();
        }

        public ICollection<T> Collection<T>(IEqualityComparer<T> itemComparer, params T[] item)
        {
            throw new NotImplementedException();
        }

        public IObservable<T> CreatePropertyChangeObservable<T>(INotifyPropertyChanged source, Func<T> accessor, string propertyName)
        {
            throw new NotImplementedException();
        }

        public ICollection<T> EmptyCollection<T>()
        {
            throw new NotImplementedException();
        }

        public ICollection<T> EmptyCollection<T>(IEqualityComparer<T> itemComparer)
        {
            throw new NotImplementedException();
        }

        public IDictionary<TKey, TValue> EmptyDictionary<TKey, TValue>()
        {
            throw new NotImplementedException();
        }

        public IDictionary<TKey, TValue> EmptyDictionary<TKey, TValue>(IEqualityComparer<TKey> keyComparer)
        {
            throw new NotImplementedException();
        }

        public IDictionary<TKey, TValue> EmptyDictionary<TKey, TValue>(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer)
        {
            throw new NotImplementedException();
        }

        public ICollection EmptyGenericCollection()
        {
            throw new NotImplementedException();
        }

        public IDictionary EmptyGenericDictionary()
        {
            throw new NotImplementedException();
        }

        public IList EmptyGenericList()
        {
            throw new NotImplementedException();
        }

        public IList<T> EmptyList<T>()
        {
            throw new NotImplementedException();
        }

        public IList<T> EmptyList<T>(IEqualityComparer<T> itemComparer)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<T> EmptyReadOnlyCollection<T>()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<T> EmptyReadOnlyCollection<T>(IEqualityComparer<T> itemComparer)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<T> EmptyReadOnlyList<T>()
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<T> EmptyReadOnlyList<T>(IEqualityComparer<T> itemComparer)
        {
            throw new NotImplementedException();
        }

        public IList<T> List<T>(params T[] items)
        {
            throw new NotImplementedException();
        }

        public IList<T> List<T>(IEqualityComparer<T> itemComparer, params T[] items)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<T> ReadOnlyCollection<T>(params T[] items)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyCollection<T> ReadOnlyCollection<T>(IEqualityComparer<T> itemComparer, params T[] items)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<T> ReadOnlyList<T>(params T[] items)
        {
            throw new NotImplementedException();
        }

        public IReadOnlyList<T> ReadOnlyList<T>(IEqualityComparer<T> itemComparer, params T[] items)
        {
            throw new NotImplementedException();
        }
    }
}
