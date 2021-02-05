using System.Xml.Serialization;

namespace TestHelper
{
    public abstract class ContentRef
    {
        private string _path = "";

        [XmlText]
        public string Path
        {
            get => _path;
            set => _path = (value is null) ? "" : value;
        }
    }
}
