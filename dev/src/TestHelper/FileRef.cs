using System.Xml.Serialization;

namespace TestHelper
{
    [XmlRoot(ROOT_ELEMENT_NAME)]
    public class FileRef : ContentRef
    {
        public const string ROOT_ELEMENT_NAME = "FileRef";
        private int _fileID = -1;

        [XmlAttribute()]
        public int FileID
        {
            get => _fileID;
            set => _fileID = (value  < -1) ? -1 : value;
        }
    }
}
