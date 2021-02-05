using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace TestHelper
{
    [XmlRoot(ROOT_ELEMENT_NAME)]
    public class ContentTemplate
    {
        public const string ROOT_ELEMENT_NAME = "Template";
        private string _fileName = "";
        private int _maxDepth = 0;
        private int _id = -1;
        private Collection<ContentFile> _files = null;
        private Collection<ContentFolder> _folders = null;

        [XmlAttribute()]
        public int ID
        {
            get => _id;
            set => _id = (value  < -1) ? -1 : value;
        }

        [XmlAttribute()]
        public string FileName
        {
            get => _fileName;
            set => _fileName = (value is null) ? "" : value;
        }

        [XmlAttribute()]
        public int MaxDepth
        {
            get => _maxDepth;
            set => _maxDepth = (value < 0) ? 0 : value;
        }

        [XmlArray()]
        [XmlArrayItem(ContentFile.ROOT_ELEMENT_NAME)]
        public Collection<ContentFile> Files
        {
            get
            {
                Collection<ContentFile> items = _files;
                if (items is null)
                    _files = items = new Collection<ContentFile>();
                return items;
            }
            set => _files = value;
        }

        [XmlArray()]
        [XmlArrayItem(ContentFolder.ROOT_ELEMENT_NAME)]
        public Collection<ContentFolder> Folders
        {
            get
            {
                Collection<ContentFolder> items = _folders;
                if (items is null)
                    _folders = items = new Collection<ContentFolder>();
                return items;
            }
            set => _folders = value;
        }
    }
}
