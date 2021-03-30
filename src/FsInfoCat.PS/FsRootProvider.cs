using FsInfoCat.Models.Crawl;
using FsInfoCat.Models.Volumes;
using FsInfoCat.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;

namespace FsInfoCat.PS
{
    internal class FsRootProvider : IVolumeSetProvider<FsRoot>
    {
        private readonly VolumeInfoRegistration _registeredVolumes;
        private readonly object _syncRoot = new object();
        private readonly Collection<FsRoot> _backingCollection = new Collection<FsRoot>();

        public FsRootProvider(VolumeInfoRegistration registeredVolumes)
        {
            _registeredVolumes = registeredVolumes;
        }

        public FsRoot this[VolumeIdentifier key] => TryGetValue(key, out FsRoot result) ? result : null;

        public int Count => _backingCollection.Count;

        bool ICollection.IsSynchronized => true;

        object ICollection.SyncRoot => _syncRoot;

        bool ICollection<FsRoot>.IsReadOnly => false;

        public IEnumerable<VolumeIdentifier> Keys => _backingCollection.Select(r => r.Identifier);

        IEnumerable<FsRoot> IReadOnlyDictionary<VolumeIdentifier, FsRoot>.Values => _backingCollection.AsEnumerable();

        public void Add(FsRoot item)
        {
            Monitor.Enter(_syncRoot);
            try
            {
                if (TryGetValue(item.Identifier, out FsRoot existing))
                {
                    if (!ReferenceEquals(existing, item))
                        throw new ArgumentOutOfRangeException(nameof(item), $"Another volume with the same identifier ({item.Identifier}) has already been added");
                }
                else if (TryGetValue(item.VolumeName, out existing))
                {
                    if (!ReferenceEquals(existing, item))
                        throw new ArgumentOutOfRangeException(nameof(item), $"Another volume with the same volume name ({item.VolumeName}) has already been added");
                }
                else if (TryGetValue(item.RootUri, out existing))
                {
                    if (!ReferenceEquals(existing, item))
                        throw new ArgumentOutOfRangeException(nameof(item), $"Another volume with the same root URI ({item.RootUri}) has already been added");
                }
                else
                    _backingCollection.Add(item);
            }
            finally { Monitor.Exit(_syncRoot); }
        }

        public void Clear()
        {
            Monitor.Enter(_syncRoot);
            try { _backingCollection.Clear(); }
            finally { Monitor.Exit(_syncRoot); }
        }

        public bool Contains(FsRoot item) => _backingCollection.Contains(item);

        public bool ContainsKey(VolumeIdentifier key) => Keys.Contains(key);

        public bool ContainsRootUri(FileUri uri)
        {
            if (uri is null || uri.IsEmpty())
                return false;
            Monitor.Enter(_syncRoot);
            try { return _backingCollection.Any(item => Equals(item.RootUri, uri)); }
            finally { Monitor.Exit(_syncRoot); }
        }

        public bool ContainsVolumeName(string name)
        {
            Monitor.Enter(_syncRoot);
            try { return _backingCollection.Any(item => DynamicStringComparer.IgnoreCaseEquals(item.VolumeName, name)); }
            finally { Monitor.Exit(_syncRoot); }
        }

        void ICollection.CopyTo(Array array, int index)
        {
            Monitor.Enter(_syncRoot);
            try { ((ICollection)_backingCollection).CopyTo(array, index); }
            finally { Monitor.Exit(_syncRoot); }
        }

        public void CopyTo(FsRoot[] array, int arrayIndex)
        {
            Monitor.Enter(_syncRoot);
            try { _backingCollection.CopyTo(array, arrayIndex); }
            finally { Monitor.Exit(_syncRoot); }
        }

        public bool Equals(FileUri x, FileUri y, out FsRoot item)
        {
            if (FileUri.IsNullOrEmpty(x))
            {
                item = null;
                return FileUri.IsNullOrEmpty(y);
            }
            if (ReferenceEquals(x, y))
            {
                TryGetByChildURI(x, out item);
                return true;
            }
            if (x.PathSegmentCount != y.PathSegmentCount || !DynamicStringComparer.IgnoreCaseEquals(x.Host, y.Host))
            {
                item = null;
                return false;
            }
            if (TryGetValue(x, out item))
                return IsSameAsRootUri(y, item);
            else if (TryGetValue(y, out item))
                return false;
            if (TryGetByChildURI(x, out item))
                return TryGetByChildURI(y, out FsRoot r) && ReferenceEquals(r, item);
            item = null;
            if (TryGetByChildURI(y, out _))
                return false;
            while (DynamicStringComparer.CaseSensitiveEquals(x.Name, y.Name))
            {
                if ((x = x.Parent) is null)
                    return false;
                y = y.Parent;
            }
            return true;
        }

        public bool Equals(FileUri x, FileUri y)
        {
            if (FileUri.IsNullOrEmpty(x))
                return FileUri.IsNullOrEmpty(y);
            if (ReferenceEquals(x, y))
                return true;
            if (x.PathSegmentCount != y.PathSegmentCount || !DynamicStringComparer.IgnoreCaseEquals(x.Host, y.Host))
                return false;
            if (TryGetValue(x, out FsRoot item))
                return IsSameAsRootUri(y, item);
            else if (TryGetValue(y, out _))
                return false;
            if (TryGetByChildURI(x, out item))
                return TryGetByChildURI(y, out FsRoot r) && ReferenceEquals(r, item);
            if (TryGetByChildURI(y, out _))
                return false;
            while (DynamicStringComparer.CaseSensitiveEquals(x.Name, y.Name))
            {
                if ((x = x.Parent) is null)
                    return false;
                y = y.Parent;
            }
            return true;
        }

        public IEnumerator<FsRoot> GetEnumerator() => _backingCollection.GetEnumerator();

        IEnumerator<KeyValuePair<VolumeIdentifier, FsRoot>> IEnumerable<KeyValuePair<VolumeIdentifier, FsRoot>>.GetEnumerator() =>
            _backingCollection.Select(item => new KeyValuePair<VolumeIdentifier, FsRoot>(item.Identifier, item)).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_backingCollection).GetEnumerator();

        public int GetHashCode(FileUri obj)
        {
            if (FileUri.IsNullOrEmpty(obj))
                return 0;
            int endIndex = obj.PathSegmentCount;
            Stack<int> hashcodes = new Stack<int>();
            if (TryGetByChildURI(obj, out FsRoot item))
            {
                IEqualityComparer<string> nameComparer;
                do
                {
                    int startIndex = item.RootUri.PathSegmentCount;
                    nameComparer = item.GetNameComparer();
                    do
                    {
                        hashcodes.Push(nameComparer.GetHashCode(obj.Name));
                        obj = obj.Parent;
                    } while (--endIndex > startIndex);
                } while (TryGetByChildURI(obj, out item));
                do
                {
                    hashcodes.Push(nameComparer.GetHashCode(obj.Name));
                } while (!((obj = obj.Parent) is null));
            }
            else
            {
                do
                {
                    hashcodes.Push(DynamicStringComparer.CASE_SENSITIVE.GetHashCode(obj.Name));
                } while (!((obj = obj.Parent) is null));
            }

            return hashcodes.Aggregate((v, e) => v ^ e);
        }

        public bool Remove(FsRoot item)
        {
            Monitor.Enter(_syncRoot);
            try { return _backingCollection.Remove(item); }
            finally { Monitor.Exit(_syncRoot); }
        }

        public bool TryImportRoot(FileUri uri, out FsRoot value)
        {
            if (_registeredVolumes.TryGetByChildURI(uri, out IVolumeInfo registeredVolume))
            {
                value = new FsRoot(registeredVolume);
                Add(value);
                return true;
            }
            value = null;
            return false;
        }

        public bool TryGetByChildURI(FileUri uri, out FsRoot value)
        {
            if (uri is null)
            {
                value = null;
                return false;
            }
            Monitor.Enter(_syncRoot);
            try
            {
                while (!TryGetValue(uri, out value))
                {
                    if ((uri = uri.Parent) is null)
                        return false;
                }
            }
            finally { Monitor.Exit(_syncRoot); }
            return true;
        }

        bool IVolumeSetProvider.TryGetByChildURI(FileUri uri, out IVolumeInfo value)
        {
            if (TryGetByChildURI(uri, out FsRoot result))
            {
                value = result;
                return true;
            }
            value = null;
            return false;
        }

        public bool TryGetValue(string volumeName, out FsRoot value)
        {
            if (volumeName is null)
            {
                value = null;
                return false;
            }
            Monitor.Enter(_syncRoot);
            try { value = _backingCollection.FirstOrDefault(item => DynamicStringComparer.IgnoreCaseEquals(item.VolumeName, volumeName)); }
            finally { Monitor.Exit(_syncRoot); }
            return !(value is null);
        }

        bool IVolumeSetProvider.TryGetValue(string volumeName, out IVolumeInfo value)
        {
            if (TryGetValue(volumeName, out FsRoot result))
            {
                value = result;
                return true;
            }
            value = null;
            return false;
        }

        private bool IsSameAsRootUri(FileUri uri, FsRoot root)
        {
            if (uri is null || root is null)
                return false;
            FileUri r = root.RootUri;
            if (ReferenceEquals(r, uri))
                return true;
            if (uri.IsEmpty())
                return r.IsEmpty();
            if (r.IsEmpty() || uri.PathSegmentCount != r.PathSegmentCount || !DynamicStringComparer.IgnoreCaseEquals(uri.Host, r.Host))
                return false;
            if (TryGetByChildURI(r.Parent, out FsRoot mountParentVol))
            {
                int startIndex = mountParentVol.RootUri.PathSegmentCount;
                return FileUri.AreSegmentsEqual(uri, r, startIndex, r.PathSegmentCount, root.GetNameComparer()) && uri.TryGetAtSegmentCount(startIndex, out uri) &&
                    IsSameAsRootUri(uri, mountParentVol);
            }
            return FileUri.AreSegmentsEqual(uri, r, 0, r.PathSegmentCount, root.GetNameComparer());
        }

        public bool TryGetValue(FileUri uri, out FsRoot value)
        {
            if (uri is null)
            {
                value = null;
                return false;
            }
            Monitor.Enter(_syncRoot);
            try
            {
                if (uri.IsEmpty())
                    value = _backingCollection.FirstOrDefault(item => item.RootUri.IsEmpty());
                else
                    value = _backingCollection.FirstOrDefault(item => IsSameAsRootUri(uri, item));
            }
            finally { Monitor.Exit(_syncRoot); }
            return !(value is null);
        }

        bool IVolumeSetProvider.TryGetValue(FileUri uri, out IVolumeInfo value)
        {
            if (TryGetValue(uri, out FsRoot result))
            {
                value = result;
                return true;
            }
            value = null;
            return false;
        }

        public bool TryFindMatching(IVolumeInfo item, out FsRoot actual)
        {
            if (item is null)
            {
                actual = null;
                return false;
            }
            Monitor.Enter(_syncRoot);
            try
            {
                return !((actual = _backingCollection.FirstOrDefault(v => v.IsEqualTo(item))) is null);
            }
            finally { Monitor.Exit(_syncRoot); }
        }

        bool IVolumeSetProvider.TryFindMatching(IVolumeInfo item, out IVolumeInfo actual)
        {
            if (TryFindMatching(item, out FsRoot result))
            {
                actual = result;
                return true;
            }
            actual = null;
            return false;
        }

        public bool TryGetValue(VolumeIdentifier key, out FsRoot value)
        {
            Monitor.Enter(_syncRoot);
            try { value = _backingCollection.FirstOrDefault(item => item.Identifier.Equals(key)); }
            finally { Monitor.Exit(_syncRoot); }
            return !(value is null);
        }
    }
}
