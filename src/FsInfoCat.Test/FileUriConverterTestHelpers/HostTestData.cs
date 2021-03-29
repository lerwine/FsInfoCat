using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    [XmlRoot("HostTestData")]
    public class HostTestData
    {
        private HostTestDataCollection _items;

        [XmlElement("IPV4", IsNullable = false, Type = typeof(IPV4HostAddress))]
        [XmlElement("IPV6", IsNullable = false, Type = typeof(IPV6HostAddress))]
        [XmlElement("HostName", IsNullable = false, Type = typeof(DnsOrBasicHostName))]
        [XmlElement("Invalid", IsNullable = false, Type = typeof(InvalidHostAddress))]
        public HostTestDataCollection Items
        {
            get => _items;
            set
            {
                if (value is null)
                    throw new ArgumentNullException(nameof(value));
                _items = value;
            }
        }

        public HostTestData()
        {
            _items = new HostTestDataCollection();
        }

        public HostTestData(params BaseHostType[] items)
        {
            _items = new HostTestDataCollection(items);
        }

        public HostTestData(IList<BaseHostType> list)
        {
            _items = new HostTestDataCollection(list);
        }

        public static HostTestData Load()
        {
            using StringReader stringReader = new StringReader(Properties.Resources.HostTestData);
            using XmlReader xmlReader = XmlReader.Create(stringReader);
            return Load(xmlReader);
        }

        public static HostTestData Load(XmlReader reader)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(HostTestData));
            return (HostTestData)serializer.Deserialize(reader);
        }

        public void Save(XmlWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(HostTestData));
            serializer.Serialize(writer, this);
        }
    }
}
