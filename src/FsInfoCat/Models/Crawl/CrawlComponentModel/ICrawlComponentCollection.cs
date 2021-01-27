using System;
using System.Collections;
using System.Collections.Generic;

namespace FsInfoCat.Models.Crawl.CrawlComponentModel
{
    public interface ICrawlComponentCollection : ICollection, IList
    {
        new INestedCrawlComponent this[int index] { get; set; }
        void Add(INestedCrawlComponent value);
        bool Contains(INestedCrawlComponent value);
        int IndexOf(INestedCrawlComponent value);
        void Insert(int index, INestedCrawlComponent value);
        void Remove(INestedCrawlComponent value);
    }

    public interface ICrawlComponentCollection<TComponent> : ICollection<TComponent>, IEnumerable<TComponent>, IEnumerable, IList<TComponent>, IReadOnlyCollection<TComponent>, IReadOnlyList<TComponent>, ICrawlComponentCollection
        where TComponent : INestedCrawlComponent
    {
    }

    public abstract class CrawlComponentCollection : ICrawlComponentCollection
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

    public abstract class CrawlComponentCollection<TComponent> : CrawlComponentCollection, ICrawlComponentCollection<TComponent>
        where TComponent : INestedCrawlComponent
    {
        TComponent IList<TComponent>.this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        TComponent IReadOnlyList<TComponent>.this[int index] => throw new NotImplementedException();

        public void Add(TComponent item)
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

        IEnumerator<TComponent> IEnumerable<TComponent>.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
