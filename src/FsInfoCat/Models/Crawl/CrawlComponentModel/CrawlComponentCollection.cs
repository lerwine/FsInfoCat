using System;
using System.Collections;
using System.Collections.Generic;

namespace FsInfoCat.Models.Crawl.CrawlComponentModel
{
    public partial class CrawlComponent
    {
        public partial class CrawlComponentContainer
        {
            public class CrawlComponentCollection : ICrawlComponentCollection
            {
                public INestedCrawlComponent this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
                object IList.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

                public bool IsFixedSize => throw new NotImplementedException();

                public bool IsReadOnly => throw new NotImplementedException();

                public int Count => throw new NotImplementedException();

                public bool IsSynchronized => throw new NotImplementedException();

                public object SyncRoot => throw new NotImplementedException();

                public void Add(INestedCrawlComponent value)
                {
                    throw new NotImplementedException();
                }

                public int Add(object value)
                {
                    throw new NotImplementedException();
                }

                public void Clear()
                {
                    throw new NotImplementedException();
                }

                public bool Contains(INestedCrawlComponent value)
                {
                    throw new NotImplementedException();
                }

                public bool Contains(object value)
                {
                    throw new NotImplementedException();
                }

                public void CopyTo(Array array, int index)
                {
                    throw new NotImplementedException();
                }

                public IEnumerator GetEnumerator()
                {
                    throw new NotImplementedException();
                }

                public int IndexOf(INestedCrawlComponent value)
                {
                    throw new NotImplementedException();
                }

                public int IndexOf(object value)
                {
                    throw new NotImplementedException();
                }

                public void Insert(int index, INestedCrawlComponent value)
                {
                    throw new NotImplementedException();
                }

                public void Insert(int index, object value)
                {
                    throw new NotImplementedException();
                }

                public void Remove(INestedCrawlComponent value)
                {
                    throw new NotImplementedException();
                }

                public void Remove(object value)
                {
                    throw new NotImplementedException();
                }

                public void RemoveAt(int index)
                {
                    throw new NotImplementedException();
                }
            }

            public class CrawlComponentCollection<TComponent> : CrawlComponentCollection,ICrawlComponentCollection<TComponent>
                where TComponent : INestedCrawlComponent
            {
                public TComponent this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

                public int Count => throw new NotImplementedException();

                protected internal object SyncRoot => throw new NotImplementedException();

                public void Add(TComponent item)
                {
                    throw new NotImplementedException();
                }

                public void Clear()
                {
                    throw new NotImplementedException();
                }

                public bool Contains(TComponent item)
                {
                    throw new NotImplementedException();
                }

                public void CopyTo(TComponent[] array, int arrayIndex)
                {
                    throw new NotImplementedException();
                }

                public IEnumerator<TComponent> GetEnumerator()
                {
                    throw new NotImplementedException();
                }

                public int IndexOf(TComponent item)
                {
                    throw new NotImplementedException();
                }

                public void Insert(int index, TComponent item)
                {
                    throw new NotImplementedException();
                }

                public bool Remove(TComponent item)
                {
                    throw new NotImplementedException();
                }

                public void RemoveAt(int index)
                {
                    throw new NotImplementedException();
                }

                INestedCrawlComponent ICrawlComponentCollection.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
                object IList.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
                bool ICollection<TComponent>.IsReadOnly => throw new NotImplementedException();
                bool IList.IsFixedSize => throw new NotImplementedException();
                bool IList.IsReadOnly => throw new NotImplementedException();
                bool ICollection.IsSynchronized => throw new NotImplementedException();
                object ICollection.SyncRoot => throw new NotImplementedException();

                IEnumerator IEnumerable.GetEnumerator()
                {
                    throw new NotImplementedException();
                }

                void ICrawlComponentCollection.Add(INestedCrawlComponent value)
                {
                    throw new NotImplementedException();
                }

                bool ICrawlComponentCollection.Contains(INestedCrawlComponent value)
                {
                    throw new NotImplementedException();
                }

                int ICrawlComponentCollection.IndexOf(INestedCrawlComponent value)
                {
                    throw new NotImplementedException();
                }

                void ICrawlComponentCollection.Insert(int index, INestedCrawlComponent value)
                {
                    throw new NotImplementedException();
                }

                void ICrawlComponentCollection.Remove(INestedCrawlComponent value)
                {
                    throw new NotImplementedException();
                }

                int IList.Add(object value)
                {
                    throw new NotImplementedException();
                }

                bool IList.Contains(object value)
                {
                    throw new NotImplementedException();
                }

                int IList.IndexOf(object value)
                {
                    throw new NotImplementedException();
                }

                void IList.Insert(int index, object value)
                {
                    throw new NotImplementedException();
                }

                void IList.Remove(object value)
                {
                    throw new NotImplementedException();
                }

                void ICollection.CopyTo(Array array, int index)
                {
                    throw new NotImplementedException();
                }
            }
        }
    }
}
