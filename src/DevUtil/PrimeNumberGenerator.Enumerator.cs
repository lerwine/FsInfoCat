using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace DevUtil
{
    public static partial class PrimeNumberGenerator
    {
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
                            if (_count != PrimeNumberGenerator.CacheLength)
                                throw new InvalidOperationException("Source list has changed");
                            return _cache[_index];
                        }
                        finally { Monitor.Exit(PrimeNumberGenerator._syncRoot); }
                    }
                    finally { Monitor.Exit(_syncRoot); }
                }
            }

            object IEnumerator.Current => Current;

            internal Enumerator() => _count = PrimeNumberGenerator.CacheLength;

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
                        if (_count != PrimeNumberGenerator.CacheLength)
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
                        if (_count != PrimeNumberGenerator.CacheLength)
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
