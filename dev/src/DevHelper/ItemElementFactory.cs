using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace DevHelper
{
    public abstract class ItemElementFactory<T> : ICollection<T>, ICollection
        where T : BaseElement
    {
        protected abstract XmlElement EnsureParentElement();

        protected abstract T CreateItem(XmlElement e);

        protected abstract PsHelpNodeName ItemName { get; }

        public int Count => EnsureParentElement().SelectNodes($"{ItemName.Prefix()}:{ItemName.LocalName()}").Count;

        bool ICollection<T>.IsReadOnly => false;

        bool ICollection.IsSynchronized => false;

        object ICollection.SyncRoot => throw new NotSupportedException();

        public void Add(T item) => BaseElement.AddTo(item, EnsureParentElement());

        public void Clear()
        {
            XmlElement parentElement = EnsureParentElement();
            foreach (XmlElement element in EnsureParentElement().SelectNodes($"{ItemName.Prefix()}:{ItemName.LocalName()}").Cast<XmlElement>().ToArray())
                parentElement.RemoveChild(element);
        }

        public bool Contains(T item) => BaseElement.IsContainedBy(item, EnsureParentElement());

        public void CopyTo(T[] array, int arrayIndex) => GetAll().ToArray().CopyTo(array, arrayIndex);

        void ICollection.CopyTo(Array array, int index) => GetAll().ToArray().CopyTo(array, index);

        public IEnumerable<T> GetAll() => EnsureParentElement().SelectNodes($"{ItemName.Prefix()}:{ItemName.LocalName()}").Cast<XmlElement>().Select(e => CreateItem(e));

        public IEnumerator<T> GetEnumerator() => GetAll().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)GetAll()).GetEnumerator();

        public bool Remove(T item) => BaseElement.RemoveFrom(item, EnsureParentElement());
    }
}
