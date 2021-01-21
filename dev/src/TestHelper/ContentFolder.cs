using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml.Serialization;

namespace TestHelper
{
    [XmlRoot(ROOT_ELEMENT_NAME)]
    public class ContentFolder : ContentItem, IEquatable<ContentFolder>, IComparable<ContentFolder>
    {
        public const string ROOT_ELEMENT_NAME = "Folder";
        private Collection<ContentFile> _files = null;
        private Collection<ContentFolder> _folders = null;

        [XmlArray()]
        [XmlArrayItem(ContentFile.ROOT_ELEMENT_NAME)]
        public Collection<ContentFile> Files
        {
            get
            {
                Collection<ContentFile> items = _files;
                if (null == items)
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
                if (null == items)
                    _folders = items = new Collection<ContentFolder>();
                return items;
            }
            set => _folders = value;
        }

        internal static Collection<ContentFolder> Merge(Collection<ContentFolder> folders1, Collection<ContentFolder> folders2)
        {
            if (folders2.Count == 0)
                return folders1;
            if (folders1.Count == 0)
                return folders2;
            Collection<ContentFolder> result = new Collection<ContentFolder>();
            foreach (ContentFolder f in folders1.Concat(folders2).OrderBy(a => a))
            {
                if (null == f)
                    continue;
                int index = result.IndexOf(f);
                if (index < 0)
                    result.Add(f);
                else
                {
                    ContentFolder other = result[index];
                    if (!ReferenceEquals(f, other))
                    {
                        ContentFolder item = new ContentFolder();
                        item.Files = ContentFile.Merge(f.Files, other.Files);
                        item.Folders = Merge(f.Folders, other.Folders);
                        result[index] = item;
                    }
                }
            }
            return result;
        }

        public int CompareTo(ContentFolder other)
        {
            if (null == other)
                return 1;
            if (ReferenceEquals(this, other))
                return 0;
            return CompareTo(other, this, other);
        }

        private int CompareTo(ContentFolder other, ContentFolder syncA, ContentFolder syncB)
        {
            if (ReferenceEquals(this, syncA))
                return (ReferenceEquals(other, syncB)) ? 0 : base.CompareTo(other);
            if (ReferenceEquals(this, syncB))
                return (ReferenceEquals(other, syncA)) ? 0 : base.CompareTo(other);
            if (ReferenceEquals(other, syncA) || ReferenceEquals(other, syncB))
                return base.CompareTo(other);
            int result = base.CompareTo(other);
            if (result != 0)
                return result;
            using (IEnumerator<ContentFile> enumeratorA = Files.GetEnumerator())
            {
                using (IEnumerator<ContentFile> enumeratorB = Files.GetEnumerator())
                {
                    while (enumeratorA.MoveNext())
                    {
                        if (enumeratorB.MoveNext())
                        {
                            if ((result = enumeratorA.Current.CompareTo(enumeratorB.Current)) != 0)
                                return result;
                        }
                        else
                            return 1;
                    }
                    if (enumeratorB.MoveNext())
                        return -1;
                }
            }
            using (IEnumerator<ContentFolder> enumeratorA = Folders.GetEnumerator())
            {
                using (IEnumerator<ContentFolder> enumeratorB = Folders.GetEnumerator())
                {
                    while (enumeratorA.MoveNext())
                    {
                        if (enumeratorB.MoveNext())
                        {
                            if (null == enumeratorA.Current)
                            {
                                if (null != enumeratorB.Current)
                                    return 1;
                            }
                            else
                            {
                                if (null == enumeratorB.Current)
                                    return -1;
                                if (!ReferenceEquals(enumeratorA.Current, enumeratorB.Current) &&
                                    (result = enumeratorA.Current.CompareTo(enumeratorB.Current, syncA, syncB)) != 0)
                                    return result;
                            }
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

        public override int CompareTo(ContentItem other)
        {
            return (null != other && other is ContentFolder) ? CompareTo((ContentFolder)other) : 1;
        }

        private bool Equals(ContentFolder other, ContentFolder syncA, ContentFolder syncB)
        {
            if (ReferenceEquals(this, syncA))
                return ReferenceEquals(other, syncB);
            if (ReferenceEquals(this, syncB))
                return ReferenceEquals(other, syncA);
            if (ReferenceEquals(other, syncA) || ReferenceEquals(other, syncB))
                return false;
            if (!base.Equals(other) || !Files.SequenceEqual(other.Files))
                return false;
            using (IEnumerator<ContentFolder> enumeratorA = Folders.GetEnumerator())
            {
                using (IEnumerator<ContentFolder> enumeratorB = Folders.GetEnumerator())
                {
                    if (enumeratorA.MoveNext())
                    {
                        if (enumeratorB.MoveNext())
                        {
                            if (null == enumeratorA.Current)
                            {
                                if (null != enumeratorB.Current)
                                    return false;
                            }
                            else
                            {
                                if (null == enumeratorB.Current || !(ReferenceEquals(enumeratorA.Current, enumeratorB.Current) || enumeratorA.Current.Equals(enumeratorB.Current, syncA, syncB)))
                                    return false;
                            }
                        }
                        else
                            return false;
                    }
                    else if (enumeratorB.MoveNext())
                        return false;
                }
            }
            return true;
        }

        public bool Equals(ContentFolder other)
        {
            return null != other && (ReferenceEquals(this, other) || (base.Equals(other) && Files.SequenceEqual(other.Files) &&
                Folders.SequenceEqual(other.Folders)));
        }

        public override bool Equals(ContentItem other)
        {
            return null != other && other is ContentFolder && Equals((ContentFolder)other);
        }
    }
}
