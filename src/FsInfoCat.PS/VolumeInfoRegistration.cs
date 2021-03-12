using FsInfoCat.Models;
using FsInfoCat.Models.Crawl;
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
    public sealed class VolumeInfoRegistration : IVolumeSetProvider<VolumeInfoRegistration.RegisteredVolumeItem>
    {
        public static readonly StringComparer CASE_IGNORED_NAME_COMPARER = StringComparer.InvariantCultureIgnoreCase;
        public static readonly StringComparer CASE_SENSITIVE_NAME_COMPARER = StringComparer.InvariantCulture;

        private object _syncRoot = new object();
        private Collection<RegisteredVolumeItem> _backingCollection = new Collection<RegisteredVolumeItem>();

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
                FileUri key = CreateKey(volumeInfo.RootUri, out RegisteredVolumeItem existing);
                if (!(existing is null))
                {
                    if (!ReferenceEquals(existing, item))
                        throw new ArgumentOutOfRangeException(nameof(volumeInfo), $"Another volume with the same root URI ({volumeInfo.RootUri}) has already been added");
                }
                else if (TryGetValue(volumeInfo.Identifier, out existing))
                {
                    if (!ReferenceEquals(existing, item))
                        throw new ArgumentOutOfRangeException(nameof(volumeInfo), $"Another volume with the same identifier ({volumeInfo.Identifier}) has already been added");
                }
                else if (TryFindByName(volumeInfo.VolumeName, out existing))
                {
                    if (!ReferenceEquals(existing, item))
                        throw new ArgumentOutOfRangeException(nameof(volumeInfo), $"Another volume with the same volume name ({volumeInfo.VolumeName}) has already been added");
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

        public void CopyTo(RegisteredVolumeItem[] array, int arrayIndex) => _backingCollection.CopyTo(array, arrayIndex);

        void ICollection.CopyTo(Array array, int index) => ((ICollection)_backingCollection).CopyTo(array, index);

        public FileUri CreateKey(FileUri fileUri, out RegisteredVolumeItem result)
        {
            if (fileUri is null)
            {
                result = null;
                return null;
            }
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
                    if (CASE_IGNORED_NAME_COMPARER.Equals(f.Host, host) && i.PathComparer.Equals(path, f.ToString()))
                    {
                        result = item;
                        return f;
                    }
                }
                result = null;
                return fileUri;
            }
            FileUri parentUri = CreateKey(fileUri.Parent, out result);
            if (result is null)
            {
                string host = fileUri.Host;
                string path = fileUri.ToString();
                result = _backingCollection.FirstOrDefault(v =>
                {
                    RegisteredVolumeInfo rvi = (RegisteredVolumeInfo)v.BaseObject;
                    FileUri f = rvi.RootUri;
                    return CASE_IGNORED_NAME_COMPARER.Equals(f.Host, host) && rvi.PathComparer.Equals(path, f.ToString());
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
                    return ReferenceEquals(r.Parent, parentRoot) && vi.PathComparer.Equals(childName, r.Name) && vi.PathComparer.Equals(childName, r.Name);
                });
            }
            return (ReferenceEquals(parentUri, fileUri.Parent)) ? fileUri : new FileUri(parentUri, fileUri.Name);
        }

        public bool Equals(FileUri x, FileUri y)
        {
            if (x is null)
                return y is null;
            if (y is null)
                return false;
            if (ReferenceEquals(x, y))
                return true;
            x = CreateKey(x, out RegisteredVolumeItem a);
            y = CreateKey(y, out RegisteredVolumeItem b);
            if (ReferenceEquals(x, y))
                return true;
            throw new NotImplementedException();
        }

        public IEnumerator<RegisteredVolumeItem> GetEnumerator() => _backingCollection.GetEnumerator();

        IEnumerator<KeyValuePair<VolumeIdentifier, RegisteredVolumeItem>> IEnumerable<KeyValuePair<VolumeIdentifier, RegisteredVolumeItem>>.GetEnumerator() =>
            _backingCollection.Select(item => new KeyValuePair<VolumeIdentifier, RegisteredVolumeItem>(((RegisteredVolumeInfo)item.BaseObject).Identifier, item)).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)_backingCollection).GetEnumerator();

        public int GetHashCode(FileUri obj)
        {
            throw new NotImplementedException();
        }

        public bool Remove(RegisteredVolumeItem item)
        {
            Monitor.Enter(_syncRoot);
            try { return _backingCollection.Remove(item); }
            finally { Monitor.Exit(_syncRoot); }
        }

        public bool TryFind(FileUri uri, out RegisteredVolumeItem value)
        {
            Monitor.Enter(_syncRoot);
            try
            {
                FileUri key = CreateKey(uri, out value);
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

        public bool TryFind(FileUri uri, out IVolumeInfo value) => TryFind(uri, out value);

        public bool TryFindByRootURI(FileUri uri, out RegisteredVolumeItem value)
        {
            Monitor.Enter(_syncRoot);
            try { CreateKey(uri, out value); }
            finally { Monitor.Exit(_syncRoot); }
            return !(value is null);
        }

        public bool TryFindByRootURI(FileUri uri, out IVolumeInfo value) => TryFindByRootURI(uri, out value);

        public bool TryGetValue(VolumeIdentifier key, out RegisteredVolumeItem value)
        {
            value = _backingCollection.FirstOrDefault(v => ((RegisteredVolumeInfo)v.BaseObject).Identifier.Equals(key));
            return !(value is null);
        }

        public bool TryFindByName(string volumeName, out RegisteredVolumeItem value)
        {
            if (volumeName is null)
            {
                value = null;
                return false;
            }
            value = _backingCollection.FirstOrDefault(v => CASE_IGNORED_NAME_COMPARER.Equals(((RegisteredVolumeInfo)v.BaseObject).VolumeName, volumeName));
            return !(value is null);
        }

        public bool TryFindByName(string volumeName, out IVolumeInfo value) => TryFindByName(volumeName, out value);

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
            IEqualityComparer<string> IVolumeInfo.GetPathComparer() => ((RegisteredVolumeInfo)BaseObject).PathComparer;
            event PropertyValueChangeEventHandler INotifyPropertyValueChanging.PropertyValueChanging
            {
                add => ((RegisteredVolumeInfo)BaseObject).PropertyValueChanging += value;
                remove => ((RegisteredVolumeInfo) BaseObject).PropertyValueChanging -= value;
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
            public IEqualityComparer<string> GetPathComparer() => PathComparer;
            public IEqualityComparer<string> PathComparer { get; }
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
