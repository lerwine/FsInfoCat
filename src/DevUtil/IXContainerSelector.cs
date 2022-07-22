using System.Collections.Generic;
using System.Xml.Linq;

namespace DevUtil
{
    public interface IXContainerSelector<T> : IXNodeSelector<T> where T : XContainer
    {
        XNodeSelector DescendantNodes();

        XElementSelector Descendants();

        XElementSelector Descendants(string localName);

        XElementSelector Descendants(string localName, string namespaceURI);

        XElementSelector Descendants(XName name);

        XElementSelector Elements();

        XElementSelector Elements(string localName);

        XElementSelector Elements(string localName, string namespaceURI);

        XElementSelector Elements(XName name);

        XElementSelector ElementsWithAttribute(string attributeName, string value);

        XElementSelector ElementsWithAttribute(string attributeName, string value, IEqualityComparer<string> comparer);

        XElementSelector ElementsWithAttribute(string elementName, string attributeName, string value);

        XElementSelector ElementsWithAttribute(string elementName, string attributeName, string value, IEqualityComparer<string> comparer);

        XElementSelector ElementsWithAttribute(string elementName, string namespaceURI, string attributeName, string value);

        XElementSelector ElementsWithAttribute(string elementName, string namespaceURI, string attributeName, string value, IEqualityComparer<string> comparer);

        XElementSelector ElementsWithAttribute(string elementName, string elementNamespace, string attributeName, string attributeNamespace, string value);

        XElementSelector ElementsWithAttribute(string elementName, string elementNamespace, string attributeName, string attributeNamespace, string value, IEqualityComparer<string> comparer);

        XElementSelector ElementsWithAttribute(XName name, string value);

        XElementSelector ElementsWithAttribute(XName name, string value, IEqualityComparer<string> comparer);

        XElementSelector ElementsWithAttribute(XName elementName, XName attributeName, string value);

        XElementSelector ElementsWithAttribute(XName elementName, XName attributeName, string value, IEqualityComparer<string> comparer);

        XElementSelector ElementsWithChildElement(string childElementName, string value);

        XElementSelector ElementsWithChildElement(string childElementName, string value, IEqualityComparer<string> comparer);

        XElementSelector ElementsWithChildElement(string selectedElementName, string childElementName, string value);

        XElementSelector ElementsWithChildElement(string selectedElementName, string childElementName, string value, IEqualityComparer<string> comparer);

        XElementSelector ElementsWithChildElement(string selectedElementName, string namespaceURI, string childElementName, string value);

        XElementSelector ElementsWithChildElement(string selectedElementName, string namespaceURI, string childElementName, string value, IEqualityComparer<string> comparer);

        XElementSelector ElementsWithChildElement(string selectedElementName, string selectedElementNamespace, string childElementName, string childElementNamespace, string value);

        XElementSelector ElementsWithChildElement(string selectedElementName, string selectedElementNamespace, string childElementName, string childElementNamespace, string value, IEqualityComparer<string> comparer);

        XElementSelector ElementsWithChildElement(XName childElementName, string value);

        XElementSelector ElementsWithChildElement(XName childElementName, string value, IEqualityComparer<string> comparer);

        XElementSelector ElementsWithChildElement(XName selectedElementName, XName childElementName, string value);

        XElementSelector ElementsWithChildElement(XName selectedElementName, XName childElementName, string value, IEqualityComparer<string> comparer);

        XNodeSelector Nodes();
    }
}
