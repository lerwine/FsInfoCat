using FsInfoCat.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace FsInfoCat
{
    // TODO: Document EnumerableCoersion class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class EnumerableCoersion<T> : ICoersion<IEnumerable<T>>
    {
        public static readonly EnumerableCoersion<T> Default = new();

        private readonly ICoersion<T> _backingCoersion;

        Type ICoersion.ValueType => typeof(IEnumerable<T>);

        public EnumerableCoersion(ICoersion<T> elementCoersion)
        {
            _backingCoersion = elementCoersion ?? Coersion<T>.Default;
        }

        public EnumerableCoersion() : this(null) { }

        public virtual IEnumerable<T> Cast(object obj) => (IEnumerable<T>)obj;

        public virtual IEnumerable<T> Coerce(object obj)
        {
            if (TryCast(obj, out IEnumerable<T> result))
                return result;
            if (_backingCoersion.TryCast(obj, out T e))
                return new T[] { e };
            if (obj is IEnumerable g)
                return g.Cast<object>().Select(_backingCoersion.Coerce);
            return new T[] { _backingCoersion.Coerce(obj) };
        }

        public virtual IEnumerable<T> Normalize(IEnumerable<T> obj) => obj;

        object ICoersion.Normalize(object obj) => Normalize((IEnumerable<T>)obj);

        public virtual bool Equals(IEnumerable<T> x, IEnumerable<T> y)
        {
            if (x is null)
                return y is null;
            if (y is null)
                return false;
            return ReferenceEquals(x, y) || x.SequenceEqual(y, _backingCoersion);
        }

        public virtual int GetHashCode(IEnumerable<T> obj) => obj.GetAggregateHashCode(_backingCoersion);

        public virtual bool TryCast(object obj, out IEnumerable<T> result)
        {
            if (obj is null)
                result = null;
            else if (obj is IEnumerable<T> t)
                result = t;
            else
            {
                result = null;
                return false;
            }
            return true;
        }

        public virtual bool TryCoerce(object obj, out IEnumerable<T> result)
        {
            if (TryCast(obj, out result))
                return true;
            if (_backingCoersion.TryCast(obj, out T e))
                result = new T[] { e };
            else if (obj is IEnumerable g)
            {
                LinkedList<T> items = new();
                if (g.Cast<object>().All(o =>
                {
                    if (_backingCoersion.TryCoerce(o, out e))
                    {
                        _ = items.AddLast(e);
                        return true;
                    }
                    return false;
                }))
                    result = items;
                else
                {
                    result = null;
                    return false;
                }
            }
            else if (_backingCoersion.TryCoerce(obj, out e))
                result = new T[] { e };
            else
            {
                result = null;
                return false;
            }
            return true;
        }

        object ICoersion.Cast(object obj) => Cast(obj);

        object ICoersion.Coerce(object obj) => Cast(obj);

        bool ICoersion.TryCast(object obj, out object result)
        {
            bool r = TryCast(obj, out IEnumerable<T> e);
            result = e;
            return r;
        }

        bool ICoersion.TryCoerce(object obj, out object result)
        {
            bool r = TryCoerce(obj, out IEnumerable<T> e);
            result = e;
            return r;
        }

        bool IEqualityComparer.Equals(object x, object y) => TryCast(x, out IEnumerable<T> a) && TryCast(y, out IEnumerable<T> b) ? Equals(a, b) :
            Equals(x, y);

        int IEqualityComparer.GetHashCode(object obj) => (obj is null) ? 0 : (TryCast(obj, out IEnumerable<T> t) ? GetHashCode(t) : obj.GetHashCode());
    }

    public abstract class EnumerableCoersion<TElement, TEnumerable> : ICoersion<TEnumerable>
        where TEnumerable : class, IEnumerable<TElement>
    {
        private readonly EnumerableCoersion<TElement> _backingCoersion;

        Type ICoersion.ValueType => typeof(TEnumerable);

        public EnumerableCoersion(ICoersion<TElement> elementCoersion)
        {
            _backingCoersion = new EnumerableCoersion<TElement>(elementCoersion);
        }

        public EnumerableCoersion() : this(null) { }

        public virtual TEnumerable Cast(object obj) => (TEnumerable)obj;

        public virtual TEnumerable Coerce(object obj)
        {
            if (TryCast(obj, out TEnumerable result))
                return result;
            return CreateFromEnumerable(_backingCoersion.Coerce(obj));
        }

        public virtual TEnumerable Normalize(TEnumerable obj) => obj;

        object ICoersion.Normalize(object obj) => Normalize((TEnumerable)obj);

        public virtual bool TryCast(object obj, out TEnumerable result)
        {
            {
                if (obj is null)
                    result = null;
                else if (obj is TEnumerable t)
                    result = t;
                else
                {
                    result = null;
                    return false;
                }
                return true;
            }
        }

        public virtual bool TryCoerce(object obj, out TEnumerable result)
        {
            if (TryCast(obj, out result))
                return true;
            if (_backingCoersion.TryCoerce(obj, out IEnumerable<TElement> e))
                return TryCreateFromEnumerable(e, out result);
            result = default;
            return false;
        }

        protected abstract TEnumerable CreateFromEnumerable([AllowNull] IEnumerable<TElement> elements);

        protected abstract bool TryCreateFromEnumerable([AllowNull] IEnumerable<TElement> elements, out TEnumerable result);

        public virtual bool Equals(TEnumerable x, TEnumerable y) => _backingCoersion.Equals(x, y);

        public virtual int GetHashCode(TEnumerable obj) => _backingCoersion.GetHashCode(obj);

        object ICoersion.Cast(object obj) => Cast(obj);

        object ICoersion.Coerce(object obj) => Coerce(obj);

        bool ICoersion.TryCast(object obj, out object result)
        {
            bool r = TryCast(obj, out TEnumerable e);
            result = e;
            return r;
        }

        bool ICoersion.TryCoerce(object obj, out object result)
        {
            bool r = TryCast(obj, out TEnumerable e);
            result = e;
            return r;
        }

        bool IEqualityComparer.Equals(object x, object y) => ((IEqualityComparer)_backingCoersion).Equals(x, y);

        int IEqualityComparer.GetHashCode(object obj) => ((IEqualityComparer)_backingCoersion).GetHashCode(obj);
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
