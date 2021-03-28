using FsInfoCat.Test.Helpers;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public sealed class FilePathTestDataItem : ISynchronized, IOwnable<FilePathTestData>, IEquatable<FilePathTestDataItem>
    {
        internal object SyncRoot => _syncRoot;
        private readonly object _syncRoot = new object();
        object ISynchronized.SyncRoot => _syncRoot;
        private FilePathTestDataCollection _ownerCollection;
        ISynchronized IOwnable.Owner => Owner;
        FilePathTestData IOwnable<FilePathTestData>.Owner => Owner;

        [XmlIgnore]
        internal FilePathTestDataCollection OwnerCollection
        {
            get => _ownerCollection;
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value));
                lock (_syncRoot)
                    _ownerCollection = value;
            }
        }

        [XmlIgnore]
        internal FilePathTestData Owner => _ownerCollection?.Owner;

        private string _inputString = "";
        [XmlAttribute]
        public string InputString
        {
            get => _inputString;
            set => _inputString = value ?? "";
        }

        private PlatformPath _windows;
        [XmlElement(IsNullable = false, Order = 0)]
        public PlatformPath Windows
        {
            get => _windows;
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value));
                lock (value.SyncRoot)
                {
                    lock (_syncRoot)
                    {
                        if (ReferenceEquals(value, _linux))
                            throw new InvalidOperationException();
                        if (value.Owner is null)
                        {
                            if (_windows is null)
                                value.Owner = this;
                            else
                                lock (_windows.SyncRoot)
                                {
                                    if (ReferenceEquals(this, _windows.Owner))
                                        _windows.Owner = null;
                                    _windows = null;
                                    value.Owner = this;
                                    _windows = value;
                                }
                            _windows = value;
                        }
                        else if (!ReferenceEquals(value.Owner, this))
                            throw new InvalidOperationException();
                    }
                }
            }
        }

        private PlatformPath _linux;
        [XmlElement(IsNullable = false, Order = 1)]
        public PlatformPath Linux
        {
            get => _linux;
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value));
                lock (value.SyncRoot)
                {
                    lock (_syncRoot)
                    {
                        if (ReferenceEquals(value, _windows))
                            throw new InvalidOperationException();
                        if (value.Owner is null)
                        {
                            if (_linux is null)
                                value.Owner = this;
                            else
                                lock (_linux.SyncRoot)
                                {
                                    if (ReferenceEquals(this, _linux.Owner))
                                        _linux.Owner = null;
                                    _linux = null;
                                    value.Owner = this;
                                }
                            _linux = value;
                        }
                        else if (!ReferenceEquals(value.Owner, this))
                            throw new InvalidOperationException();
                    }
                }
            }
        }

        public FilePathTestDataItem()
        {
            _windows = new PlatformPath();
            _linux = new PlatformPath();
            _windows.Owner = _linux.Owner = this;
        }

        public bool Equals([AllowNull] FilePathTestDataItem other) => !(other is null) && (ReferenceEquals(this, other) || _windows.Equals(other._windows) && _linux.Equals(other._linux));

        public override bool Equals(object obj) => Equals(obj as FilePathTestDataItem);

        public override int GetHashCode() => HashCode.Combine(_inputString, _windows, _linux);

        public string GetXPath() => (Owner is null) ? FilePathTestData.XmlElementName_TestData :
            $"/{FilePathTestData.XmlElementName_FilePathTestData}/{FilePathTestData.XmlElementName_TestData}[@{nameof(InputString)}={XmlBuilder.ToXPathString(InputString)}]";
    }
}
