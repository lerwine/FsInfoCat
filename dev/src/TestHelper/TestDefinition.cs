using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml;
using System.Xml.Serialization;

namespace TestHelper
{
    [XmlRoot(ROOT_ELEMENT_NAME)]
    public class TestDefinition
    {
        public const string ROOT_ELEMENT_NAME = "Test";
        private int? _maxDepth = null;
        private int? _maxItems = null;
        private Collection<ExpectedResult> _expected = null;

        [XmlAttribute("MaxDepth")]
        public string __MaxDepthXml
        {
            get
            {
                int? i = _maxDepth;
                return (i.HasValue) ? XmlConvert.ToString(i.Value) : null;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _maxDepth = null;
                else
                    _maxDepth = XmlConvert.ToInt32(value.Trim());
            }
        }

        [XmlIgnore]
        public int? MaxDepth
        {
            get => _maxDepth;
            set => _maxDepth = (value  < 0) ? 0 : value;
        }

        [XmlElement(ExpectedResult.ROOT_ELEMENT_NAME)]
        public Collection<ExpectedResult> Expected
        {
            get
            {
                Collection<ExpectedResult> items = _expected;
                if (null == items)
                    _expected = items = new Collection<ExpectedResult>();
                return items;
            }
            set => _expected = value;
        }

        [XmlAttribute("MaxItems")]
        public string __MaxItemsXml
        {
            get
            {
                int? i = _maxItems;
                return (i.HasValue) ? XmlConvert.ToString(i.Value) : null;
            }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    _maxItems = null;
                else
                    _maxItems = XmlConvert.ToInt32(value.Trim());
            }
        }

        [XmlIgnore]
        public int? MaxItems
        {
            get => _maxItems;
            set => _maxItems = (value  < 0) ? 0 : value;
        }
    }
}
