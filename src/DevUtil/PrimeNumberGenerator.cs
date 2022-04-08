using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DevUtil
{
    public class PrimeNumberGenerator : IReadOnlyList<uint>, IList<uint>, IList
    {
        private static readonly object _syncRoot = new();
        private static uint[] _values = new uint[] { 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79, 83, 89, 97 };
        private static readonly int _minArrayLength;
        private static int _count;
        static PrimeNumberGenerator() => _minArrayLength = _values.Length;

        public static readonly PrimeNumberGenerator Instance = new();

        public uint this[int index]
        {
            get
            {
                if (index < 0)
                    throw new ArgumentOutOfRangeException(nameof(index));
                Monitor.Enter(_syncRoot);
                try
                {
                    if (index > _count)
                        throw new ArgumentOutOfRangeException(nameof(index));
                    return _values[index];
                }
                finally { Monitor.Exit(_syncRoot); }
            }
        }

        uint IList<uint>.this[int index] { get => this[index]; set => throw new NotSupportedException(); }

        object IList.this[int index] { get => this[index]; set => throw new NotSupportedException(); }

        public int Count => _count;

        public object SyncRoot => _syncRoot;

        bool IList.IsFixedSize => false;

        bool IList.IsReadOnly => true;

        bool ICollection<uint>.IsReadOnly => true;

        bool ICollection.IsSynchronized => true;

        private PrimeNumberGenerator() => _count = _minArrayLength;

        public static bool IsPrimeNumber(uint value)
        {
            if (value < 2 || (value & 1) == 0)
                return false;
            Monitor.Enter(_syncRoot);
            try
            {
                int e = ((_count < _minArrayLength) ? _minArrayLength : _count) - 1;
                uint v = _values[e];
                if (value == v)
                    return true;
                if (value < v)
                {
                    for (int i = 1; i < e; i++)
                    {
                        if (value == _values[i])
                            return true;
                    }
                    return false;
                }
            }
            finally { Monitor.Exit(_syncRoot); }
            return TestPrimeNumber(value);
        }

        private static bool TestPrimeNumber(uint value)
        {
            uint end = value >> 1;
            for (uint d = 2; d <= end; d++)
            {
                if (value % d == 0)
                    return false;
            }
            return true;
        }

        public static uint Get(int index)
        {
            Monitor.Enter(_syncRoot);
            try
            {
                if (index < _values.Length)
                {
                    if (index >= _count)
                        _count = index + 1;
                    return _values[index];
                }
                uint[] values = new uint[index];
                Array.Copy(_values, values, values.Length);
                uint i = _values[_count - 1] + 2;
                while (!TestPrimeNumber(i)) i += 2;
                int position = _count;
                _values[position] = i;
                while (++position < index)
                {
                    i += 2;
                    while (!TestPrimeNumber(i)) i += 2;
                    _values[position] = i;
                }
                _count = index + 1;
                return i;
            }
            finally { Monitor.Exit(_syncRoot); }
        }

        public static void Truncate(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));
            Monitor.Enter(_syncRoot);
            try
            {
                if (_count < count)
                    throw new ArgumentOutOfRangeException(nameof(count));
                if (count < _count)
                {
                    _count = count;
                    if (count > _minArrayLength)
                        Array.Resize(ref _values, count);
                }
            }
            finally { Monitor.Exit(_syncRoot); }
        }

        public IEnumerator<uint> GetEnumerator() => new Enumerator();

        IEnumerator IEnumerable.GetEnumerator() => new Enumerator();

        int IList.Add(object value) => throw new NotSupportedException();

        void IList.Clear() => throw new NotSupportedException();

        bool IList.Contains(object value) => value is uint i && Contains(i);

        int IList.IndexOf(object value) => (value is uint i) ? IndexOf(i) : -1;

        void IList.Insert(int index, object value) => throw new NotSupportedException();

        void IList.Remove(object value) => throw new NotSupportedException();

        void IList.RemoveAt(int index) => throw new NotSupportedException();

        void ICollection.CopyTo(Array array, int index) => _values.CopyTo(array, index);

        public int IndexOf(uint item) => ((IList<uint>)_values).IndexOf(item);

        public bool Contains(uint item) => ((ICollection<uint>)_values).Contains(item);

        public void CopyTo(uint[] array, int arrayIndex) => _values.CopyTo(array, arrayIndex);

        void IList<uint>.Insert(int index, uint item) => throw new NotSupportedException();

        void IList<uint>.RemoveAt(int index) => throw new NotSupportedException();

        void ICollection<uint>.Add(uint item) => throw new NotSupportedException();

        void ICollection<uint>.Clear() => throw new NotSupportedException();

        bool ICollection<uint>.Remove(uint item) => throw new NotSupportedException();

        class Enumerator : IEnumerator<uint>
        {
            private readonly object _syncRoot = new object();
            private readonly int _count;
            private int _index = -1;

            public uint Current
            {
                get
                {
                    Monitor.Enter(_syncRoot);
                    try
                    {
                        switch (_index)
                        {
                            case -2:
                                throw new ObjectDisposedException(GetType().FullName);
                            case -1:
                                throw new InvalidOperationException("Enumerator not advanced to first element");
                            default:
                                if (_index == _count)
                                    throw new InvalidOperationException("Enumerator was advanced past the last element");
                                break;
                        }
                        Monitor.Enter(PrimeNumberGenerator._syncRoot);
                        try
                        {
                            if (_count != PrimeNumberGenerator._count)
                                throw new InvalidOperationException("Source list has changed");
                            return _values[_index];
                        }
                        finally { Monitor.Exit(PrimeNumberGenerator._syncRoot); }
                    }
                    finally { Monitor.Exit(_syncRoot); }
                }
            }

            object IEnumerator.Current => Current;

            internal Enumerator() => _count = PrimeNumberGenerator._count;

            public void Dispose()
            {
                Monitor.Enter(_syncRoot);
                try { _index = -2; }
                finally { Monitor.Exit(_syncRoot); }
            }

            public bool MoveNext()
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    if (_index == -2)
                        throw new ObjectDisposedException(GetType().FullName);
                    else if (_index == _count)
                        return false;
                    Monitor.Enter(PrimeNumberGenerator._syncRoot);
                    try
                    {
                        if (_count != PrimeNumberGenerator._count)
                            throw new InvalidOperationException("Source list has changed");
                        return ++_index < _count;
                    }
                    finally { Monitor.Exit(PrimeNumberGenerator._syncRoot); }
                }
                finally { Monitor.Exit(_syncRoot); }
            }

            public void Reset()
            {
                Monitor.Enter(_syncRoot);
                try
                {
                    if (_index == -2)
                        throw new ObjectDisposedException(GetType().FullName);
                    Monitor.Enter(PrimeNumberGenerator._syncRoot);
                    try
                    {
                        if (_count != PrimeNumberGenerator._count)
                            throw new InvalidOperationException("Source list has changed");
                        _index = -1;
                    }
                    finally { Monitor.Exit(PrimeNumberGenerator._syncRoot); }
                }
                finally { Monitor.Exit(_syncRoot); }
            }
        }
    }
}
