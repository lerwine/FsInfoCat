using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    [XmlRoot(XmlElementName_FilePathTestData)]
    public class FilePathTestData : ISynchronized
    {
        public const string XmlElementName_FilePathTestData = "FilePathTestData";
        public const string XmlElementName_TestData = "TestData";
        internal object SyncRoot => _syncRoot;
        private readonly object _syncRoot = new object();
        object ISynchronized.SyncRoot => _syncRoot;
        private FilePathTestDataCollection _items;

        [XmlElement(XmlElementName_TestData, IsNullable = false)]
        public FilePathTestDataCollection Items
        {
            get => _items;
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value));
                lock (value.SyncRoot)
                {
                    lock (_syncRoot)
                    {
                        if (value.Owner is null)
                        {
                            if (_items is null)
                                value.Owner = this;
                            else
                                lock (_items.SyncRoot)
                                {
                                    if (ReferenceEquals(this, _items.Owner))
                                        _items.Owner = null;
                                    _items = null;
                                    value.Owner = this;
                                }
                            _items = value;
                        }
                        else if (!ReferenceEquals(value.Owner, this))
                            throw new InvalidOperationException();
                    }
                }
            }
        }

        public FilePathTestData()
        {
            _items = new FilePathTestDataCollection
            {
                Owner = this
            };
        }

        public static FilePathTestData Load()
        {
            using StringReader stringReader = new StringReader(Properties.Resources.FilePathTestData);
            using XmlReader xmlReader = XmlReader.Create(stringReader);
            return Load(xmlReader);
        }

        public static FilePathTestData Load(XmlReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FilePathTestData));
            return (FilePathTestData)serializer.Deserialize(reader);
        }

        public void Save(XmlWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FilePathTestData));
            serializer.Serialize(writer, this);
        }
    }
}
