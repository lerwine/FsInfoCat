using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace TestHelper
{
    [XmlRoot(ROOT_ELEMENT_NAME)]
    public class ContentFile : ContentItem, IEquatable<ContentFile>, IComparable<ContentFile>
    {
        public const string ROOT_ELEMENT_NAME = "File";
        private long _length = 0L;

        [XmlAttribute()]
        public long Length
        {
            get => _length;
            set => _length = (value < 0L) ? 0L : value;
        }

        internal static Collection<ContentFile> Merge(Collection<ContentFile> files1, Collection<ContentFile> files2)
        {
            if (files2.Count == 0)
                return files1;
            if (files1.Count == 0)
                return files2;
            Collection<ContentFile> result = new Collection<ContentFile>();
            foreach (ContentFile f in files1)
            {
                if (null != f && !result.Contains(f))
                    result.Add(f);
            }
            foreach (ContentFile f in files2)
            {
                if (null != f && !result.Contains(f))
                    result.Add(f);
            }
            return result;
        }

        public int CompareTo(ContentFile other)
        {
            if (other is null)
                return 1;
            if (ReferenceEquals(this, other))
                return 0;
            int result = base.CompareTo(other);
            return (result == 0) ? Length.CompareTo(other.Length) : result;
        }

        public override int CompareTo(ContentItem other)
        {
            return (null != other && other is ContentFile) ? CompareTo((ContentFile)other) : -1;
        }

        public bool Equals(ContentFile other)
        {
            return null != other && (ReferenceEquals(this, other) || (base.Equals(this) && Length == other.Length));
        }

        public override bool Equals(ContentItem other)
        {
            return null != other && other is ContentFile && Equals((ContentFile)other);
        }
    }
}
