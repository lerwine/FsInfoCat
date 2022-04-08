using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace DevUtil
{
    public static partial class PrimeNumberGenerator
    {
        public class CacheAccessor : IReadOnlyList<uint>, IList<uint>, IList
        {
            public uint this[int index]
            {
                get
                {
                    if (index < 0)
                        throw new ArgumentOutOfRangeException(nameof(index));
                    Monitor.Enter(_syncRoot);
                    try
                    {
                        if (index > CacheLength)
                            throw new ArgumentOutOfRangeException(nameof(index));
                        return _cache[index];
                    }
                    finally { Monitor.Exit(_syncRoot); }
                }
            }

            uint IList<uint>.this[int index] { get => this[index]; set => throw new NotSupportedException(); }

            object IList.this[int index] { get => this[index]; set => throw new NotSupportedException(); }

            public int Count => CacheLength;

            public object SyncRoot => _syncRoot;

            bool IList.IsFixedSize => false;

            bool IList.IsReadOnly => true;

            bool ICollection<uint>.IsReadOnly => true;

            bool ICollection.IsSynchronized => true;

            internal CacheAccessor() { }

            public IEnumerator<uint> GetEnumerator() => new Enumerator();

            IEnumerator IEnumerable.GetEnumerator() => new Enumerator();

            void ICollection<uint>.Add(uint item) => throw new NotSupportedException();

            int IList.Add(object value) => throw new NotSupportedException();

            void ICollection<uint>.Clear() => throw new NotSupportedException();

            void IList.Clear() => throw new NotSupportedException();
            
            public bool Contains(uint item) => ((ICollection<uint>)_cache).Contains(item);

            bool IList.Contains(object value) => value is uint i && Contains(i);

            public void CopyTo(uint[] array, int arrayIndex) => _cache.CopyTo(array, arrayIndex);

            void ICollection.CopyTo(Array array, int index) => _cache.CopyTo(array, index);

            public int IndexOf(uint item) => ((IList<uint>)_cache).IndexOf(item);

            int IList.IndexOf(object value) => (value is uint i) ? IndexOf(i) : -1;

            void IList<uint>.Insert(int index, uint item) => throw new NotSupportedException();

            void IList.Insert(int index, object value) => throw new NotSupportedException();

            bool ICollection<uint>.Remove(uint item) => throw new NotSupportedException();

            void IList.Remove(object value) => throw new NotSupportedException();

            void IList<uint>.RemoveAt(int index) => throw new NotSupportedException();

            void IList.RemoveAt(int index) => throw new NotSupportedException();

            public static void TruncateCache(int maxLength)
            {
                if (maxLength < 0)
                    throw new ArgumentOutOfRangeException(nameof(maxLength));
                Monitor.Enter(_syncRoot);
                try
                {
                    if (maxLength < CacheLength)
                    {
                        CacheLength = maxLength;
                        if (maxLength > _minArrayLength)
                            Array.Resize(ref _cache, maxLength);
                    }
                }
                finally { Monitor.Exit(_syncRoot); }
            }
        }
    }
}
