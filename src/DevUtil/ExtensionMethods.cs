using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace DevUtil
{
    public static class ExtensionMethods
    {
        public static XElement FindFirstMatchingAttribute(this XElement element, XName elementName, XName attributeName, string value, IEqualityComparer<string> comparer = null)
        {
            if (value is null) throw new ArgumentNullException(nameof(value));
            if (elementName is null) throw new ArgumentNullException(nameof(elementName));
            if (attributeName is null) throw new ArgumentNullException(nameof(attributeName));
            if (element is null) return null;
            if (comparer is null) comparer = StringComparer.InvariantCulture;
            return element.Elements(elementName).Attributes(attributeName).FirstOrDefault(a => comparer.Equals(a.Value, value))?.Parent;
        }

        public static XElement FindFirstMatchingAttributeGuid(this XElement element, XName elementName, XName attributeName, Guid value)
        {
            if (elementName is null) throw new ArgumentNullException(nameof(elementName));
            if (attributeName is null) throw new ArgumentNullException(nameof(attributeName));
            if (element is null) return null;
            return element.Elements(elementName).Attributes(attributeName).FirstOrDefault(a =>
            {
                try { return XmlConvert.ToGuid(a.Value).Equals(value); }
                catch { return false; }
            })?.Parent;
        }

        public static XElement FindFirstMatchingAttributeBoolean(this XElement element, XName elementName, XName attributeName, bool value)
        {
            if (elementName is null) throw new ArgumentNullException(nameof(elementName));
            if (attributeName is null) throw new ArgumentNullException(nameof(attributeName));
            if (element is null) return null;
            return element.Elements(elementName).Attributes(attributeName).FirstOrDefault(a =>
            {
                try { return XmlConvert.ToBoolean(a.Value) == value; }
                catch { return false; }
            })?.Parent;
        }

        public static XElement FindFirstMatchingAttributeInt32(this XElement element, XName elementName, XName attributeName, int value)
        {
            if (elementName is null) throw new ArgumentNullException(nameof(elementName));
            if (attributeName is null) throw new ArgumentNullException(nameof(attributeName));
            if (element is null) return null;
            return element.Elements(elementName).Attributes(attributeName).FirstOrDefault(a =>
            {
                try { return XmlConvert.ToInt32(a.Value) == value; }
                catch { return false; }
            })?.Parent;
        }

        public static Exception GetCausalException(this Exception source)
        {
            if (source is null) return null;
            if ((source is MethodInvocationException || source is GetValueInvocationException || source is SetValueInvocationException) && source.InnerException is not null)
                source = source.InnerException;
            return (source is AggregateException aggregateException && aggregateException.InnerExceptions.Count == 1) ? aggregateException.InnerException : source;
        }
    }
}
