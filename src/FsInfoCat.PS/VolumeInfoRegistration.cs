using FsInfoCat.Models;
using FsInfoCat.Models.Volumes;
using FsInfoCat.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Management.Automation;
using System.Threading;

namespace FsInfoCat.PS
{
    // TODO: Need to create base VolumeSetProvider class so it may be tested in NUnit tests
    public sealed class VolumeInfoRegistration : IVolumeSetProvider<VolumeInfoRegistration.RegisteredVolumeItem>
    {
        private readonly object _syncRoot = new object();
        private readonly Collection<RegisteredVolumeItem> _backingCollection = new Collection<RegisteredVolumeItem>();

        public RegisteredVolumeItem this[VolumeIdentifier key] => TryGetValue(key, out RegisteredVolumeItem result) ? result : null;

        public int Count => _backingCollection.Count;

        public IEnumerable<VolumeIdentifier> Keys => _backingCollection.Select(v => ((RegisteredVolumeInfo)v.BaseObject).Identifier);

        bool ICollection.IsSynchronized => true;

        bool ICollection<RegisteredVolumeItem>.IsReadOnly => false;

        object ICollection.SyncRoot => _syncRoot;

        IEnumerable<RegisteredVolumeItem> IReadOnlyDictionary<VolumeIdentifier, RegisteredVolumeItem>.Values => _backingCollection.AsEnumerable();

        public void Add(RegisteredVolumeItem item)
        {
            if (item is null)
                throw new ArgumentNullException(nameof(item));
            RegisteredVolumeInfo volumeInfo = (RegisteredVolumeInfo)item.BaseObject;
            Monitor.Enter(_syncRoot);
            try
            {
                FileUri key = GetUriKey(volumeInfo.RootUri, out RegisteredVolumeItem existing);
                if (!(existing is null))
                {
                    if (!ReferenceEquals(existing, item))
                        throw new ArgumentOutOfRangeException(nameof(item), $"Another volume with the same root URI ({volumeInfo.RootUri}) has already been added");
                }
                else if (TryGetValue(volumeInfo.Identifier, out existing))
                {
                    if (!ReferenceEquals(existing, item))
                        throw new ArgumentOutOfRangeException(nameof(item), $"Another volume with the same identifier ({volumeInfo.Identifier}) has already been added");
                }
                else if (TryGetValue(volumeInfo.VolumeName, out existing))
                {
                    if (!ReferenceEquals(existing, item))
                        throw new ArgumentOutOfRangeException(nameof(item), $"Another volume with the same volume name ({volumeInfo.VolumeName}) has already been added");
                }
                else
                {
                    try
                    {
                        volumeInfo.RootUri = key;
                    }
                    catch
                    {
                        if (ReferenceEquals(volumeInfo.RootUri, key))
                            _backingCollection.Add(item);
                        throw;
                    }
                    _backingCollection.Add(item);
                }
            }
            finally { Monitor.Exit(_syncRoot); }
        }

        public void Clear()
        {
            Monitor.Enter(_syncRoot);
            try { _backingCollection.Clear(); }
            finally { Monitor.Exit(_syncRoot); }
        }

        public bool Contains(RegisteredVolumeItem item) => _backingCollection.Contains(item);

        public bool ContainsKey(VolumeIdentifier key) => Keys.Contains(key);

        public bool ContainsRootUri(FileUri uri)
        {
            Monitor.Enter(_syncRoot);
            try
            {
                GetUriKey(uri, out RegisteredVolumeItem item);
                return !(item is null); }
            finally { Monitor.Exit(_syncRoot); }
        }

        public bool ContainsVolumeName(string name)
        {
            if (name is null)
                return false;
            return _backingCollection.Any(v => DynamicStringComparer.IgnoreCaseEquals(((RegisteredVolumeInfo)v.BaseObject).VolumeName, name));
        }

        public void CopyTo(RegisteredVolumeItem[] array, int arrayIndex) => _backingCollection.CopyTo(array, arrayIndex);

        void ICollection.CopyTo(Array array, int index) => ((ICollection)_backingCollection).CopyTo(array, index);

        public FileUri GetUriKey(FileUri fileUri, out RegisteredVolumeItem result)
        {
            if (fileUri is null)
            {
                result = null;
                return null;
            }
            Monitor.Enter(_syncRoot);
            try
            {
                result = _backingCollection.FirstOrDefault(v => ReferenceEquals(((RegisteredVolumeInfo)v.BaseObject).RootUri, fileUri));
                if (!(result is null))
                    return fileUri;
                if (fileUri.Parent is null)
                {
                    string host = fileUri.Host;
                    string path = fileUri.ToString();
                    foreach (RegisteredVolumeItem item in _backingCollection)
                    {
                        RegisteredVolumeInfo i = (RegisteredVolumeInfo)item.BaseObject;
                        FileUri f = i.RootUri;
                        if (DynamicStringComparer.IgnoreCaseEquals(f.Host, host) && i.NameComparer.Equals(path, f.ToString()))
                        {
                            result = item;
                            return f;
                        }
                    }
                    result = null;
                    return fileUri;
                }
                FileUri parentUri = GetUriKey(fileUri.Parent, out result);
                if (result is null)
                {
                    string host = fileUri.Host;
                    string path = fileUri.ToString();
                    result = _backingCollection.FirstOrDefault(v =>
                    {
                        RegisteredVolumeInfo rvi = (RegisteredVolumeInfo)v.BaseObject;
                        FileUri f = rvi.RootUri;
                        return DynamicStringComparer.IgnoreCaseEquals(f.Host, host) && rvi.NameComparer.Equals(path, f.ToString());
                    });
                }
                else
                {
                    RegisteredVolumeInfo rvi = (RegisteredVolumeInfo)result.BaseObject;
                    FileUri parentRoot = rvi.RootUri;
                    string childName = fileUri.Name;
                    result = _backingCollection.FirstOrDefault(v =>
                    {
                        RegisteredVolumeInfo vi = (RegisteredVolumeInfo)v.BaseObject;
                        FileUri r = rvi.RootUri;
                        return ReferenceEquals(r.Parent, parentRoot) && vi.NameComparer.Equals(childName, r.Name) && vi.NameComparer.Equals(childName, r.Name);
                    });
                }
                return (ReferenceEquals(parentUri, fileUri.Parent)) ? fileUri : new FileUri(parentUri, fileUri.Name);
            }
            finally { Monitor.Exit(_syncRoot); }
        }

        public bool Equals(FileUri x, FileUri y, out RegisteredVolumeItem item)
        {
            if (x is null)
            {
                item = null;
                return y is null;
            }
            if (y is null)
            {
                item = null;
                return false;
            }
            if (ReferenceEquals(x, y))
            {
                GetUriKey(x, out item);
                return true;
            }
            FileUri u1 = GetUriKey(x, out item);
            FileUri u2 = GetUriKey(x, out RegisteredVolumeItem v2);
            if (!(item is null))
                return ReferenceEquals(u1, u2);
            if (v2 is null && Equals(x.Parent, y.Parent, out item))
            {
                if (item is null)
                    return x.Name == y.Name;
                return ((RegisteredVolumeInfo)item.BaseObject).NameComparer.Equals(x.Name, y.Name);
            }
            return false;
        }

        public bool Equals(FileUri x, FileUri y)
        {
            if (x is null)
                return y is null;
            if (y is null)
                return false;
            if (ReferenceEquals(x, y))
                return true;
            if (!DynamicStringComparer.IgnoreCaseEquals(x.Host, y.Host))
                return false;
            FileUri u1 = GetUriKey(x, out RegisteredVolumeItem v1);
            FileUri u2 = GetUriKey(x, out RegisteredVolumeItem v2);
            if (!(v1 is null))
                return ReferenceEquals(u1, u2);
            if (v2 is null && Equals(x.Parent, y.Parent, out v1))
            {
                if (v1 is null)
                    return x.Name == y.Name;
                return ((RegisteredVolumeInfo)v1.BaseObject).NameComparer.Equals(x.Name, y.Name);
            }
            return false;
        }

        public IEnumerator<RegisteredVolumeItem> GetEnumerator() => _backingCollection.GetEnumerator();

        IEnumerator<KeyValuePair<VolumeIdentifier, RegisteredVolumeItem>> IEnumerable<KeyValuePair<VolumeIdentifier, RegisteredVolumeItem>>.GetEnumerator() =>
            _backingCollection.Select(item => new KeyValuePair<VolumeIdentifier, RegisteredVolumeItem>(((RegisteredVolumeInfo)item.BaseObject).Identifier, item)).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_backingCollection).GetEnumerator();

        private bool GetSegmentHashCodes(FileUri fileUri, Stack<int> values)
        {
            List<string> segments = new List<string>
            {
                fileUri.Name
            };
            IVolumeInfo volume;
            FileUri u = fileUri;
            while (!TryGetValue(u, out volume))
            {
                if ((u = u.Parent) is null)
                    return false;
                segments.Add(u.Name);
            }
            IEqualityComparer<string> nameComparer = volume.GetNameComparer();
            foreach (string n in segments)
                values.Push(nameComparer.GetHashCode(n));
            if ((u = u.Parent) is null)
                return true;
            if (!GetSegmentHashCodes(u, values))
                do
                {
                    values.Push(nameComparer.GetHashCode(u.Name));
                } while (!((u = u.Parent) is null));
            return true;
        }

        public int GetHashCode(FileUri obj)
        {
            if (obj is null || obj.IsEmpty())
                return 0;
            Stack<int> hashCodes = new Stack<int>();
            if (!GetSegmentHashCodes(obj, hashCodes))
            {
                FileUri u = obj;
                do
                {
                    hashCodes.Push(DynamicStringComparer.IGNORE_CASE.GetHashCode(u.Name ?? ""));
                } while (!((u = u.Parent) is null));
            }
            hashCodes.Push(DynamicStringComparer.IGNORE_CASE.GetHashCode(obj.Host));
            return hashCodes.Aggregate(0, (x, y) => x ^ y);
        }

        /// <summary>
        /// Removes (unregisters) a <seealso cref="RegisteredVolumeItem"/>.
        /// </summary>
        /// <param name="item">The <seealso cref="RegisteredVolumeItem"/> to remove.</param>
        /// <returns><see langword="true"/> if the this contained the referened <paramref name="item"/> and it was removed; otherwise, <see langword="false"/>.</returns>
        public bool Remove(RegisteredVolumeItem item)
        {
            Monitor.Enter(_syncRoot);
            try { return _backingCollection.Remove(item); }
            finally { Monitor.Exit(_syncRoot); }
        }

        /// <summary>
        /// Looks for a <seealso cref="RegisteredVolumeItem"/> whose <seealso cref="IVolumeInfo.RootUri"/> is equal to or is a parent of the specified <seealso cref="FileUri"/>.
        /// </summary>
        /// <param name="uri">A <seealso cref="FileUri"/> that is a hierarchical member of a volume.</param>
        /// <param name="value">Returns he matching <seealso cref="RegisteredVolumeItem"/>.</param>
        /// <returns><see langword="true"/> if a matching <seealso cref="RegisteredVolumeItem"/> was found; otherwise, <see langword="false"/>.</returns>
        public bool TryGetByChildURI(FileUri uri, out RegisteredVolumeItem value)
        {
            Monitor.Enter(_syncRoot);
            try
            {
                FileUri key = GetUriKey(uri, out value);
                if (!(value is null))
                    return true;
                if (ReferenceEquals(key, uri))
                    return false;
                while (!((key = key.Parent) is null))
                {
                    if (!((value = _backingCollection.FirstOrDefault(v => ReferenceEquals(((RegisteredVolumeInfo)v.BaseObject).RootUri, key))) is null))
                        return true;
                }
            }
            finally { Monitor.Exit(_syncRoot); }
            return false;
        }

        /// <summary>
        /// Looks for a <seealso cref="IVolumeInfo"/> whose <seealso cref="IVolumeInfo.RootUri"/> is equal to or is a parent of the specified <seealso cref="FileUri"/>.
        /// </summary>
        /// <param name="uri">A <seealso cref="FileUri"/> that is a hierarchical member of a volume.</param>
        /// <param name="value">Returns he matching <seealso cref="IVolumeInfo"/>.</param>
        /// <returns><see langword="true"/> if a matching <seealso cref="IVolumeInfo"/> was found; otherwise, <see langword="false"/>.</returns>
        public bool TryGetByChildURI(FileUri uri, out IVolumeInfo value) => TryGetByChildURI(uri, out value);

        /// <summary>
        /// Looks for a <seealso cref="RegisteredVolumeItem"/> where the <seealso cref="IVolumeInfo.RootUri"/> is equal to the specified <seealso cref="FileUri"/>.
        /// </summary>
        /// <param name="uri">The <seealso cref="FileUri"/> which represents the volume root path.</param>
        /// <param name="value">Returns he matching <seealso cref="RegisteredVolumeItem"/>.</param>
        /// <returns><see langword="true"/> if a matching <seealso cref="RegisteredVolumeItem"/> was found; otherwise, <see langword="false"/>.</returns>
        public bool TryGetValue(FileUri uri, out RegisteredVolumeItem value)
        {
            Monitor.Enter(_syncRoot);
            try { GetUriKey(uri, out value); }
            finally { Monitor.Exit(_syncRoot); }
            return !(value is null);
        }

        /// <summary>
        /// Looks for a <seealso cref="IVolumeInfo"/> where the <seealso cref="IVolumeInfo.RootUri"/> is equal to the specified <seealso cref="FileUri"/>.
        /// </summary>
        /// <param name="uri">The <seealso cref="FileUri"/> which represents the volume root path.</param>
        /// <param name="value">Returns he matching <seealso cref="IVolumeInfo"/>.</param>
        /// <returns><see langword="true"/> if a matching <seealso cref="IVolumeInfo"/> was found; otherwise, <see langword="false"/>.</returns>
        public bool TryGetValue(FileUri uri, out IVolumeInfo value) => TryGetValue(uri, out value);

        /// <summary>
        /// Looks up a <seealso cref="RegisteredVolumeItem"/> by its <seealso cref="VolumeIdentifier"/>.
        /// </summary>
        /// <param name="key">The <seealso cref="VolumeIdentifier"/> to look for.</param>
        /// <param name="value">Returns a <seealso cref="RegisteredVolumeItem"/> whose <seealso cref="IVolumeInfo.Identifier"/> matches given
        /// <seealso cref="VolumeIdentifier"/> <paramref name="key"/>.</param>
        /// <returns><see langword="true"/> if a matching <seealso cref="RegisteredVolumeItem"/> was found; otherwise, <see langword="false"/>.</returns>
        public bool TryGetValue(VolumeIdentifier key, out RegisteredVolumeItem value)
        {
            value = _backingCollection.FirstOrDefault(v => ((RegisteredVolumeInfo)v.BaseObject).Identifier.Equals(key));
            return !(value is null);
        }

        /// <summary>
        /// Looks up a <seealso cref="RegisteredVolumeItem"/> by its <seealso cref="IVolumeInfo.VolumeName"/>.
        /// </summary>
        /// <param name="volumeName">The case-insensitive volume name to look up.</param>
        /// <param name="value">Returns a <seealso cref="RegisteredVolumeItem"/> whose <seealso cref="IVolumeInfo.VolumeName"/> matches given
        /// <seealso cref="VolumeIdentifier"/> <paramref name="key"/>.</param>
        /// <returns><see langword="true"/> if a matching <seealso cref="RegisteredVolumeItem"/> was found; otherwise, <see langword="false"/>.</returns>
        public bool TryGetValue(string volumeName, out RegisteredVolumeItem value)
        {
            if (volumeName is null)
            {
                value = null;
                return false;
            }
            value = _backingCollection.FirstOrDefault(v => DynamicStringComparer.IgnoreCaseEquals(((RegisteredVolumeInfo)v.BaseObject).VolumeName, volumeName));
            return !(value is null);
        }

        /// <summary>
        /// Looks up a <seealso cref="IVolumeInfo"/> by its <seealso cref="IVolumeInfo.VolumeName"/>.
        /// </summary>
        /// <param name="volumeName">The case-insensitive volume name to look up.</param>
        /// <param name="value">Returns a <seealso cref="IVolumeInfo"/> whose <seealso cref="IVolumeInfo.VolumeName"/> matches given
        /// <seealso cref="VolumeIdentifier"/> <paramref name="key"/>.</param>
        /// <returns><see langword="true"/> if a matching <seealso cref="IVolumeInfo"/> was found; otherwise, <see langword="false"/>.</returns>
        public bool TryGetValue(string volumeName, out IVolumeInfo value) => TryGetValue(volumeName, out value);

        public bool TryFindMatching(IVolumeInfo item, out RegisteredVolumeItem actual)
        {
            if (item is null || _backingCollection.Count == 0)
            {
                actual = null;
                return false;
            }

            if ((item is RegisteredVolumeItem && !((actual = _backingCollection.FirstOrDefault(r => ReferenceEquals(r, item))) is null)) ||
                (item is RegisteredVolumeInfo && !((actual = _backingCollection.FirstOrDefault(r => ReferenceEquals(r.BaseObject, item))) is null)))
                return true;
            if (TryGetValue(item.Identifier, out actual))
            {
                RegisteredVolumeInfo volumeInfo = (RegisteredVolumeInfo)actual.BaseObject;
                return volumeInfo.CaseSensitive == item.CaseSensitive && DynamicStringComparer.IgnoreCaseEquals(volumeInfo.VolumeName, item.VolumeName) &&
                    volumeInfo.RootUri.ToString() == item.RootUri.ToString() && volumeInfo.DriveFormat == item.DriveFormat;
            }
            return false;
        }

        public bool TryFindMatching(IVolumeInfo item, out IVolumeInfo actual)
        {
            if (TryFindMatching(item, out RegisteredVolumeItem result))
            {
                actual = result;
                return true;
            }
            actual = null;
            return false;
        }

        /// <summary>
        /// Wrapper class to allow PowerShell scripts to associate additional custom properties.
        /// </summary>
        public class RegisteredVolumeItem : PSObject, IVolumeInfo
        {
            internal RegisteredVolumeItem(RegisteredVolumeInfo volumeInfo) : base(volumeInfo) { }
            FileUri IVolumeInfo.RootUri { get => ((RegisteredVolumeInfo)BaseObject).RootUri; set => throw new NotSupportedException(); }
            string IVolumeInfo.RootPathName => ((RegisteredVolumeInfo)BaseObject).RootPathName;
            string IVolumeInfo.VolumeName { get => ((RegisteredVolumeInfo)BaseObject).VolumeName; set => throw new NotSupportedException(); }
            string IVolumeInfo.DriveFormat { get => ((RegisteredVolumeInfo)BaseObject).DriveFormat; set => throw new NotSupportedException(); }
            VolumeIdentifier IVolumeInfo.Identifier { get => ((RegisteredVolumeInfo)BaseObject).Identifier; set => throw new NotSupportedException(); }
            bool IVolumeInfo.CaseSensitive { get => ((RegisteredVolumeInfo)BaseObject).CaseSensitive; set => throw new NotSupportedException(); }
            IEqualityComparer<string> IVolumeInfo.GetNameComparer() => ((RegisteredVolumeInfo)BaseObject).NameComparer;
            event PropertyValueChangeEventHandler INotifyPropertyValueChanging.PropertyValueChanging
            {
                add => ((RegisteredVolumeInfo)BaseObject).PropertyValueChanging += value;
                remove => ((RegisteredVolumeInfo)BaseObject).PropertyValueChanging -= value;
            }

            event PropertyChangingEventHandler INotifyPropertyChanging.PropertyChanging
            {
                add => ((RegisteredVolumeInfo)BaseObject).PropertyChanging += value;
                remove => ((RegisteredVolumeInfo)BaseObject).PropertyChanging -= value;
            }

            event PropertyValueChangeEventHandler INotifyPropertyValueChanged.PropertyValueChanged
            {
                add => ((RegisteredVolumeInfo)BaseObject).PropertyValueChanged += value;
                remove => ((RegisteredVolumeInfo)BaseObject).PropertyValueChanged -= value;
            }

            event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
            {
                add => ((RegisteredVolumeInfo)BaseObject).PropertyChanged += value;
                remove => ((RegisteredVolumeInfo)BaseObject).PropertyChanged -= value;
            }
        }

        public class RegisteredVolumeInfo : IVolumeInfo
        {
            private FileUri _rootUri;
            public RegisteredVolumeInfo(FileUri rootUri, VolumeIdentifier identifier, string volumeName, bool caseSensitive, string driveFormat)
            {
                _rootUri = rootUri ?? throw new ArgumentNullException(nameof(rootUri));
                RootPathName = _rootUri.ToLocalPath();
                Identifier = identifier;
                VolumeName = volumeName ?? "";
                DriveFormat = driveFormat ?? "";
                CaseSensitive = caseSensitive;
                NameComparer = caseSensitive ? DynamicStringComparer.CASE_SENSITIVE : DynamicStringComparer.IGNORE_CASE;
            }
            public FileUri RootUri
            {
                get => _rootUri;
                internal set
                {
                    PropertyValueChangingEventArgs<FileUri> changingArgs = new PropertyValueChangingEventArgs<FileUri>(nameof(RootUri), _rootUri, value ?? throw new ArgumentNullException(nameof(value)));
                    PropertyValueChanging?.Invoke(this, changingArgs);
                    PropertyChanging?.Invoke(this, changingArgs);
                    FileUri oldValue = _rootUri;
                    _rootUri = value;
                    PropertyValueChangedEventArgs<FileUri> changedArgs = new PropertyValueChangedEventArgs<FileUri>(nameof(RootUri), oldValue, _rootUri);
                    try { PropertyValueChanged?.Invoke(this, changedArgs); }
                    finally { PropertyChanged?.Invoke(this, changedArgs); }
                }
            }
            public string RootPathName { get; }
            public string VolumeName { get; }
            public string DriveFormat { get; }
            public VolumeIdentifier Identifier { get; }
            public bool CaseSensitive { get; }
            public IEqualityComparer<string> GetNameComparer() => NameComparer;
            public IEqualityComparer<string> NameComparer { get; }
            FileUri IVolumeInfo.RootUri { get => RootUri; set => throw new NotSupportedException(); }
            string IVolumeInfo.VolumeName { get => VolumeName; set => throw new NotSupportedException(); }
            string IVolumeInfo.DriveFormat { get => DriveFormat; set => throw new NotSupportedException(); }
            VolumeIdentifier IVolumeInfo.Identifier { get => Identifier; set => throw new NotSupportedException(); }
            bool IVolumeInfo.CaseSensitive { get => CaseSensitive; set => throw new NotSupportedException(); }
            public event PropertyValueChangeEventHandler PropertyValueChanging;
            public event PropertyChangingEventHandler PropertyChanging;
            public event PropertyValueChangeEventHandler PropertyValueChanged;
            public event PropertyChangedEventHandler PropertyChanged;
        }
    }
}
