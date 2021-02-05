using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace TestHelper
{
    public abstract class ContentItem : IEquatable<ContentItem>, IComparable<ContentItem>
    {
        private int _id = -1;
        private string _name = "";
        private int _attributes = 0;
        private int _depth = 0;

        [XmlAttribute()]
        public int ID
        {
            get => _id;
            set => _id = (value  < -1) ? -1 : value;
        }

        [XmlAttribute()]
        public string Name
        {
            get => _name;
            set => _name = (value is null) ? "" : value;
        }

        [XmlAttribute()]
        public int Attributes
        {
            get => _attributes;
            set => _attributes = (value < 0) ? 0 : value;
        }

        [XmlAttribute()]
        public int Depth
        {
            get => _depth;
            set => _depth = (value < 0) ? 0 : value;
        }

        [XmlAttribute()]
        public DateTime CreationTime { get; set; }

        [XmlAttribute()]
        public DateTime LastWriteTime { get; set; }

        public virtual int CompareTo(ContentItem other)
        {
            if (other is null)
                return 1;
            if (ReferenceEquals(this, other))
                return 0;
            int result = Depth - other.Depth;
            if (result == 0 && (result = CompareNames(Name, other.Name)) == 0 &&
                (result = CreationTime.CompareTo(other.CreationTime)) == 0 && (result = LastWriteTime.CompareTo(other.LastWriteTime)) == 0 &&
                (result = Attributes - other.Attributes) == 0)
                return ID - other.ID;
            return result;
        }

        public virtual bool Equals(ContentItem other)
        {
            return null != other && (ReferenceEquals(this, other) || (ID == other.ID && Attributes == other.Attributes &&
                Depth == other.Depth && Name.Equals(other.Name, StringComparison.InvariantCultureIgnoreCase) &&
                CreationTime.Equals(other.CreationTime) && LastWriteTime.Equals(other.LastWriteTime)));
        }

        public override bool Equals(object obj)
        {
            return null != obj && obj is ContentItem && Equals((ContentItem)obj);
        }

        public override int GetHashCode()
        {
            return ID;
        }

        public override string ToString()
        {
            return Name;
        }

        public static ContentFile FindFileByID(int id, IEnumerable<ContentTemplate> templates)
        {
            return templates.Select(t =>
            {
                ContentFile f = t.Files.FirstOrDefault(i => i._id == id);
                return (f is null) ? FindFileByID(id, t.Folders) : f;
            }).FirstOrDefault(i => null != i);
        }

        public static ContentFolder FindFolderByID(int id, IEnumerable<ContentTemplate> templates)
        {
            return templates.Select(t =>
            {
                ContentFolder f = t.Folders.FirstOrDefault(i => i._id == id);
                return (f is null) ? FindFolderByID(id, t.Folders) : f;
            }).FirstOrDefault(i => null != i);
        }

        private static ContentFile FindFileByID(int id, IEnumerable<ContentFolder> folders)
        {
            return folders.Select(t =>
            {
                ContentFile f = t.Files.FirstOrDefault(i => i._id == id);
                return (f is null) ? FindFileByID(id, t.Folders) : f;
            }).FirstOrDefault(i => null != i);
        }

        internal static ContentFolder FindFolderByID(int id, IEnumerable<ContentFolder> folders)
        {
            return folders.Select(t =>
            {
                ContentFolder f = t.Folders.FirstOrDefault(i => i._id == id);
                return (f is null) ? FindFolderByID(id, t.Folders) : f;
            }).FirstOrDefault(i => null != i);
        }

        public static int CompareNames(string x, string y)
        {
            if (x is null)
                return (y is null) ? 0 : 1;
            if (y is null)
                return -1;
            if (x.Length == 0)
                return (y.Length == 0) ? 0 : -1;
            if (y.Length == 0)
                return 1;

            using (IEnumerator<string> enumeratorA = x.Split('.').Cast<string>().GetEnumerator())
            {
                using (IEnumerator<string> enumeratorB = y.Split('.').Cast<string>().GetEnumerator())
                {
                    while (enumeratorA.MoveNext())
                    {
                        if (enumeratorB.MoveNext())
                        {
                            string a = enumeratorA.Current;
                            string b = enumeratorB.Current;
                            int result = result = StringComparer.OrdinalIgnoreCase.Compare(a, b);
                            if (result != 0 || (a.Length > 0 && b.Length > 0 &&
                                (result = StringComparer.InvariantCultureIgnoreCase.Compare(a, b)) != 0))
                                return result;
                        }
                        else
                            return 1;
                    }
                    if (enumeratorB.MoveNext())
                        return -1;
                }
            }
            return 0;
        }
    }
}
