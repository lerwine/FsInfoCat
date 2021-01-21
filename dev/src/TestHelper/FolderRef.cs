using System.Xml.Serialization;

namespace TestHelper
{
    [XmlRoot(ROOT_ELEMENT_NAME)]
    public class FolderRef : ContentRef
    {
        public const string ROOT_ELEMENT_NAME = "FolderRef";
        private int _folderID = -1;

        [XmlAttribute()]
        public int FolderID
        {
            get => _folderID;
            set => _folderID = (value  < -1) ? -1 : value;
        }
    }
}
