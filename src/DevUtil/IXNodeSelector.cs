using System.Collections.Generic;
using System.Xml.Linq;

namespace DevUtil
{
    public interface IXNodeSelector<T> : IXObjectSelector<T> where T : XNode
    {
        new IEnumerable<IXNodeSelector<T>> SingleFromEach();

        XElementSelector Ancestors();

        XElementSelector Ancestors(string localName);

        XElementSelector Ancestors(string localName, string namespaceURI);

        XElementSelector Ancestors(XName name);

        IXNodeSelector<T> InDocumentOrder();

        void Remove();
    }
}
