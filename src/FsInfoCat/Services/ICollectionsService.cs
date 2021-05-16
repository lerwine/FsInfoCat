using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace FsInfoCat.Services
{
    public interface ICollectionsService
    {
        IReadOnlyList<T> AsReadOnlyList<T>(IReadOnlyList<T> source);

        IReadOnlyList<T> AsReadOnlyList<T>(IList<T> source);

        IObservable<IReadOnlyList<T>> AsReadOnlyList<T>(IObservable<IReadOnlyList<T>> source);

        IObservable<IReadOnlyList<T>> AsReadOnlyList<T>(IObservable<IList<T>> source);

        IReadOnlyCollection<T> AsReadOnlyCollection<T>(IReadOnlyCollection<T> source);

        IReadOnlyCollection<T> AsReadOnlyCollection<T>(ICollection<T> source);

        IObservable<IReadOnlyCollection<T>> AsReadOnlyCollection<T>(IObservable<IReadOnlyCollection<T>> source);

        IObservable<IReadOnlyCollection<T>> AsReadOnlyCollection<T>(IObservable<ICollection<T>> source);

        IReadOnlyDictionary<TKey, TValue> AsReadOnlyDictionary<TKey, TValue>(IReadOnlyDictionary<TKey, TValue> source);

        IReadOnlyDictionary<TKey, TValue> AsReadOnlyDictionary<TKey, TValue>(IDictionary<TKey, TValue> source);

        IObservable<IReadOnlyDictionary<TKey, TValue>> AsReadOnlyDictionary<TKey, TValue>(IObservable<IReadOnlyDictionary<TKey, TValue>> source);

        IObservable<IReadOnlyDictionary<TKey, TValue>> AsReadOnlyDictionary<TKey, TValue>(IObservable<IDictionary<TKey, TValue>> source);

        IObservable<T> CreatePropertyChangeObservable<T>(INotifyPropertyChanged source, Func<T> accessor, string propertyName);

        IReadOnlyList<TTarget> CastList<TSource, TTarget>(Func<IReadOnlyList<TSource>> accessor) where TSource : TTarget;

        IReadOnlyList<TTarget> CastList<TSource, TTarget>(Func<IReadOnlyList<TSource>> accessor, IEqualityComparer<TTarget> itemComparer) where TSource : TTarget;

        IReadOnlyList<TTarget> CastList<TSource, TTarget>(Func<IList<TSource>> accessor) where TSource : TTarget;

        IReadOnlyList<TTarget> CastList<TSource, TTarget>(Func<IList<TSource>> accessor, IEqualityComparer<TTarget> itemComparer) where TSource : TTarget;

        IReadOnlyCollection<TTarget> CastCollection<TSource, TTarget>(Func<IReadOnlyCollection<TSource>> accessor) where TSource : TTarget;

        IReadOnlyCollection<TTarget> CastCollection<TSource, TTarget>(Func<IReadOnlyCollection<TSource>> accessor, IEqualityComparer<TTarget> itemComparer) where TSource : TTarget;

        IReadOnlyCollection<TTarget> CastCollection<TSource, TTarget>(Func<ICollection<TSource>> accessor) where TSource : TTarget;

        IReadOnlyCollection<TTarget> CastCollection<TSource, TTarget>(Func<ICollection<TSource>> accessor, IEqualityComparer<TTarget> itemComparer) where TSource : TTarget;

        IReadOnlyDictionary<TKey, TTarget> CastDictionary<TKey, TSource, TTarget>(Func<IReadOnlyDictionary<TKey, TSource>> accessor) where TSource : TTarget;

        IReadOnlyDictionary<TKey, TTarget> CastDictionary<TKey, TSource, TTarget>(Func<IReadOnlyDictionary<TKey, TSource>> accessor, IEqualityComparer<TTarget> valueComparer) where TSource : TTarget;

        IReadOnlyDictionary<TKey, TTarget> CastDictionary<TKey, TSource, TTarget>(Func<IDictionary<TKey, TSource>> accessor) where TSource : TTarget;

        IReadOnlyDictionary<TKey, TTarget> CastDictionary<TKey, TSource, TTarget>(Func<IDictionary<TKey, TSource>> accessor, IEqualityComparer<TTarget> valueComparer) where TSource : TTarget;

        ICollection<T> Collection<T>(params T[] items);

        ICollection<T> Collection<T>(IEqualityComparer<T> itemComparer, params T[] item);

        IDictionary<TKey, TValue> EmptyDictionary<TKey, TValue>();

        IDictionary<TKey, TValue> EmptyDictionary<TKey, TValue>(IEqualityComparer<TKey> keyComparer);

        IDictionary<TKey, TValue> EmptyDictionary<TKey, TValue>(IEqualityComparer<TKey> keyComparer, IEqualityComparer<TValue> valueComparer);

        ICollection EmptyGenericCollection();

        IDictionary EmptyGenericDictionary();

        IList EmptyGenericList();

        ICollection<T> EmptyCollection<T>();

        ICollection<T> EmptyCollection<T>(IEqualityComparer<T> itemComparer);

        IList<T> EmptyList<T>();

        IList<T> EmptyList<T>(IEqualityComparer<T> itemComparer);

        IReadOnlyCollection<T> EmptyReadOnlyCollection<T>();

        IReadOnlyCollection<T> EmptyReadOnlyCollection<T>(IEqualityComparer<T> itemComparer);

        IReadOnlyList<T> EmptyReadOnlyList<T>();

        IReadOnlyList<T> EmptyReadOnlyList<T>(IEqualityComparer<T> itemComparer);

        IList<T> List<T>(params T[] items);

        IList<T> List<T>(IEqualityComparer<T> itemComparer, params T[] items);

        IReadOnlyCollection<T> ReadOnlyCollection<T>(params T[] items);

        IReadOnlyCollection<T> ReadOnlyCollection<T>(IEqualityComparer<T> itemComparer, params T[] items);

        IReadOnlyList<T> ReadOnlyList<T>(params T[] items);

        IReadOnlyList<T> ReadOnlyList<T>(IEqualityComparer<T> itemComparer, params T[] items);
    }
}
