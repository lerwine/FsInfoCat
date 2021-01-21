using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace TestHelper
{
    [XmlRoot(ROOT_ELEMENT_NAME)]
    public class ExpectedResult
    {
        public const string ROOT_ELEMENT_NAME = "Expected";
        private int _fileCount = 0;
        private int _folderCount = 0;
        private int _rootID = -1;
        private Collection<ContentRef> _items = null;

        [XmlAttribute()]
        public int RootID
        {
            get => _rootID;
            set => _rootID = (value  < -1) ? -1 : value;
        }

        [XmlAttribute()]
        public int FileCount
        {
            get => _fileCount;
            set => _fileCount = (value  < 0) ? 0 : value;
        }

        [XmlAttribute()]
        public int FolderCount
        {
            get => _folderCount;
            set => _folderCount = (value  < 0) ? 0 : value;
        }

        [XmlElement(FileRef.ROOT_ELEMENT_NAME, typeof(FileRef))]
        [XmlElement(FolderRef.ROOT_ELEMENT_NAME, typeof(FolderRef))]
        public Collection<ContentRef> Items
        {
            get
            {
                Collection<ContentRef> items = _items;
                if (null == items)
                    _items = items = new Collection<ContentRef>();
                return items;
            }
            set => _items = value;
        }
    }
}
