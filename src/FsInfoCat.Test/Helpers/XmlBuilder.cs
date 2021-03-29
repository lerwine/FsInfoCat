using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;

namespace FsInfoCat.Test.Helpers
{
    /// <summary>
    /// Extension methods for building <seealso cref="XmlNode">XmlNodes</seealso> using method chaining.
    /// </summary>
    public static class XmlBuilder
    {
        public static readonly Regex XPathEncodableS = new Regex(@"[&'<>\p{Zl}\p{Zp}\p{C}]", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        public static readonly Regex XPathEncodableD = new Regex(@"[""&<>\p{Zl}\p{Zp}\p{C}]", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public static string ToXPathString(string text)
        {
            if (string.IsNullOrEmpty(text))
                return "";
            if (text.Contains('\''))
            {
                if (text.Contains('"'))
                    text = XPathEncodableS.Replace(text, match =>
                    {
                        char c = match.Value[0];
                        switch (c)
                        {
                            case '&':
                                return "&amp;";
                            case '<':
                                return "&lt;";
                            case '>':
                                return "&gt;";
                            case '\'':
                                return "&apos;";
                            default:
                                int n = c;
                                if (n > 0xff)
                                    return $@"&#x{n:x4};";
                                return $@"&#x{n:x2};";
                        }
                    });
                else
                {
                    if (XPathEncodableD.IsMatch(text))
                        text = XPathEncodableD.Replace(text, match =>
                        {
                            char c = match.Value[0];
                            switch (c)
                            {
                                case '&':
                                    return "&amp;";
                                case '<':
                                    return "&lt;";
                                case '>':
                                    return "&gt;";
                                case '"':
                                    return "&quot;";
                                default:
                                    int n = c;
                                    if (n > 0xff)
                                        return $@"&#x{n:x4};";
                                    return $@"&#x{n:x2};";
                            }
                        });
                    return $"\"{text}\"";
                }
            }
            else if (XPathEncodableS.IsMatch(text))
                text = XPathEncodableS.Replace(text, match =>
                {
                    char c = match.Value[0];
                    switch (c)
                    {
                        case '&':
                            return "&amp;";
                        case '<':
                            return "&lt;";
                        case '>':
                            return "&gt;";
                        case '\'':
                            return "&apos;";
                        default:
                            int n = c;
                            if (n > 0xff)
                                return $@"&#x{n:x4};";
                            return $@"&#x{n:x2};";
                    }
                });
            return $"'{text}'";
        }

        public static bool TryGetObjectStringValue(this XObject node, out string result)
        {
            if (node is null)
            {
                result = null;
                return false;
            }
            try
            {
                if (node is XAttribute attribute)
                    result = attribute.Value;
                else if (node is XElement element)
                    result = (element.IsEmpty) ? null : element.Value;
                else
                {
                    result = null;
                    return false;
                }
            }
            catch
            {
                result = null;
                return false;
            }
            return TryGetObjectStringValue(node.Parent, out result);
        }

        public static bool TryGetNamedObjectStringValue(this XObject node, XName name, out string result)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (node is null)
            {
                result = null;
                return false;
            }
            try
            {
                if (node is XAttribute attribute)
                {
                    if (attribute.Name == name)
                    {
                        result = attribute.Value;
                        return true;
                    }
                    if ((node = attribute.Parent) is null)
                    {
                        result = null;
                        return false;
                    }
                }
                if (node is XElement element)
                {
                    XAttribute a = element.Attribute(name);
                    if (a is null)
                    {
                        XElement e = element.Element(name);
                        if (e is null)
                        {
                            if (element.Name != name)
                            {
                                result = null;
                                return false;
                            }
                            e = element;
                        }
                        result = (e.IsEmpty) ? null : e.Value;
                        return true;
                    }
                    result = a.Value;
                    return true;
                }
            }
            catch
            {
                result = null;
                return false;
            }
            return node.Parent.TryGetNamedObjectStringValue(name, out result);
        }

        private static bool TryInvoke<TIn, TOut>(TIn value, Func<TIn, TOut> parseFunc, out TOut result)
        {
            try
            {
                result = parseFunc(value);
                return true;
            }
            catch { /* okay to ignore */ }
            result = default;
            return false;
        }

        public delegate bool TryParseValue<TIn, TOut>(TIn source, out TOut result);

        public static IEnumerable<TObject> WhereValueOf<TValue, TObject>(IEnumerable<TObject> source, TryParseValue<string, TValue> parseFunc, TValue value, IEqualityComparer<TValue> comparer = null)
            where TObject : XObject
        {
            if (source is null)
                return Array.Empty<TObject>();
            if (comparer is null)
                comparer = EqualityComparer<TValue>.Default;
            return source.Where(o =>
            {
                if (o is null)
                    return false;
                return TryGetObjectStringValue(o, out string rawValue) && parseFunc(rawValue, out TValue v) && comparer.Equals(v, value);
            });
        }

        public static IEnumerable<TObject> WhereValueOf<TValue, TObject>(IEnumerable<TObject> source, Func<string, TValue> parseFunc, TValue value, IEqualityComparer<TValue> comparer = null)
            where TObject : XObject
        {
            if (parseFunc is null)
                throw new ArgumentNullException(nameof(parseFunc));
            return WhereValueOf(source, (string value, out TValue result) => TryInvoke(value, parseFunc, out result), value, comparer);
        }

        public static bool TryGetParsedValue<T>(this XObject node, TryParseValue<string, T> parseFunc, out T result)
        {
            if (parseFunc is null)
                throw new ArgumentNullException(nameof(parseFunc));
            if (node.TryGetObjectStringValue(out string rawValue))
                return parseFunc(rawValue, out result);
            result = default;
            return false;
        }

        public static bool TryGetParsedValue<T>(this XObject node, Func<string, T> parseFunc, out T result)
        {
            if (parseFunc is null)
                throw new ArgumentNullException(nameof(parseFunc));
            return node.TryGetParsedValue((string value, out T r) => TryInvoke(value, parseFunc, out r), out result);
        }

        public static bool TryGetParsedValue<T>(this XObject node, XName name, TryParseValue<string, T> parseFunc, out T result)
        {
            if (parseFunc is null)
                throw new ArgumentNullException(nameof(parseFunc));
            if (node.TryGetNamedObjectStringValue(name, out string rawValue))
                return parseFunc(rawValue, out result);
            result = default;
            return false;
        }

        public static bool TryGetParsedValue<T>(this XObject node, XName name, Func<string, T> parseFunc, out T result)
        {
            if (parseFunc is null)
                throw new ArgumentNullException(nameof(parseFunc));
            return node.TryGetParsedValue(name, (string value, out T r) => TryInvoke(value, parseFunc, out r), out result);
        }

        public static bool TryGetParsedAttributeValue<T>(this XObject node, XName name, TryParseValue<string, T> parseFunc, out T result)
        {
            if (parseFunc is null)
                throw new ArgumentNullException(nameof(parseFunc));
            if (node.TryGetAttributeStringValue(name, out string rawValue))
                return parseFunc(rawValue, out result);
            result = default;
            return false;
        }

        public static bool TryGetParsedAttributeValue<T>(this XObject node, XName name, Func<string, T> parseFunc, out T result)
        {
            if (parseFunc is null)
                throw new ArgumentNullException(nameof(parseFunc));
            return node.TryGetParsedAttributeValue(name, (string value, out T r) => TryInvoke(value, parseFunc, out r), out result);
        }

        public static bool TryGetParsedElementValue<T>(this XObject node, XName name, TryParseValue<string, T> parseFunc, out T result)
        {
            if (parseFunc is null)
                throw new ArgumentNullException(nameof(parseFunc));
            if (node.TryGetElementStringValue(name, out string rawValue))
                return parseFunc(rawValue, out result);
            result = default;
            return false;
        }

        public static bool TryGetParsedElementValue<T>(this XObject node, XName name, Func<string, T> parseFunc, out T result)
        {
            if (parseFunc is null)
                throw new ArgumentNullException(nameof(parseFunc));
            return node.TryGetParsedElementValue(name, (string value, out T r) => TryInvoke(value, parseFunc, out r), out result);
        }

        public static bool TryGetAttributeStringValue(this XObject node, XName name, out string result)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (node is null)
            {
                result = null;
                return false;
            }
            try
            {
                if (node is XAttribute attribute)
                {
                    if (attribute.Name == name)
                    {
                        result = attribute.Value;
                        return true;
                    }
                    if ((node = attribute.Parent) is null)
                    {
                        result = null;
                        return false;
                    }
                }
                if (node is XElement element)
                {
                    XAttribute a = element.Attribute(name);
                    if (a is null)
                    {
                        result = null;
                        return false;
                    }
                    result = a.Value;
                    return true;
                }
            }
            catch
            {
                result = null;
                return false;
            }
            return node.Parent.TryGetAttributeStringValue(name, out result);
        }

        public static bool TryGetElementStringValue(this XObject node, XName name, out string result)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (node is null)
            {
                result = null;
                return false;
            }
            try
            {
                if (node is XAttribute attribute && (node = node.Parent) is null)
                {
                    result = null;
                    return false;
                }
                if (node is XElement element)
                {
                    XElement e = element.Element(name);
                    if (e is null)
                    {
                        if (element.Name != name)
                        {
                            result = null;
                            return false;
                        }
                        e = element;
                    }
                    result = e.IsEmpty ? null : e.Value;
                    return true;
                }
            }
            catch
            {
                result = null;
                return false;
            }
            return node.Parent.TryGetElementStringValue(name, out result);
        }

        private static bool BooleanValueTryGet(string value, out bool result)
        {
            if (!string.IsNullOrWhiteSpace(value))
                try
                {
                    result = XmlConvert.ToBoolean(value);
                    return true;
                }
                catch { /* Okay to ignore */ }
            result = default;
            return false;
        }

        private static bool IntegerValueTryGet(string value, out int result)
        {
            if (!string.IsNullOrWhiteSpace(value))
                try
                {
                    result = XmlConvert.ToInt32(value);
                    return true;
                }
                catch { /* Okay to ignore */ }
            result = default;
            return false;
        }

        public static bool TryGetBooleanValue(this XAttribute attribute, out bool result) => attribute.TryGetParsedValue(BooleanValueTryGet, out result);

        public static bool TryGetIntegerValue(this XAttribute attribute, out int result) => attribute.TryGetParsedValue(IntegerValueTryGet, out result);

        public static bool TryGetAttributeBooleanValue(this XObject node, XName name, out bool result) =>
            node.TryGetParsedAttributeValue(name, BooleanValueTryGet, out result);

        public static bool TryGetAttributeIntegerValue(this XObject node, XName name, out int result) =>
            node.TryGetParsedAttributeValue(name, IntegerValueTryGet, out result);

        public static string StringAttributeValue(this XObject node, XName name, string defaultValue = null) =>
            node.TryGetAttributeStringValue(name, out string value) ? value : defaultValue;

        public static string StringElementValue(this XObject node, XName name, string defaultValue = null) =>
            node.TryGetElementStringValue(name, out string value) ? value : defaultValue;

        public static bool BooleanAttributeValue(this XObject node, XName name, bool defaultValue = false) =>
            node.TryGetAttributeBooleanValue(name, out bool value) ? value : defaultValue;

        public static int IntegerAttributeValue(this XObject node, XName name, int defaultValue = default) =>
            node.TryGetAttributeIntegerValue(name, out int value) ? value : defaultValue;

        public static bool AttributeEquals(this XElement source, XName name, string value) => source.TryGetAttributeStringValue(name, out string v) && value == v;

        public static bool AttributeNotEquals(this XElement source, XName name, string value) => source.TryGetAttributeStringValue(name, out string v) && value != v;

        public static bool AttributeEqualsAny(this XElement source, XName name, params string[] values) =>
            !(values is null) && values.Length > 0 && source.TryGetAttributeStringValue(name, out string v) && values.Contains(v);

        public static bool AttributeNotEqualsAny(this XElement source, XName name, params string[] values) =>
            !(values is null) && values.Length > 0 && source.TryGetAttributeStringValue(name, out string v) && !values.Contains(v);

        public static bool AttributeEquals(this XElement source, XName name, bool value) => source.TryGetAttributeBooleanValue(name, out bool v) && value == v;

        public static bool AttributeNotEquals(this XElement source, XName name, bool value) => source.TryGetAttributeBooleanValue(name, out bool v) && value != v;

        public static bool AttributeEqualsAny(this XElement source, XName name, params int[] values) =>
            !(values is null) && values.Length > 0 && source.TryGetAttributeIntegerValue(name, out int v) && values.Contains(v);

        public static IEnumerable<XElement> WhereAttributeExists(this IEnumerable<XElement> source, XName name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (source is null)
                return Array.Empty<XElement>();
            return source.Where(e => e.Attributes(name).Any());
        }

        public static IEnumerable<XElement> WhereAttributeNotExists(this IEnumerable<XElement> source, XName name)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (source is null)
                return Array.Empty<XElement>();
            return source.Where(e => !e.Attributes(name).Any());
        }

        public static IEnumerable<XElement> WhereAttributeEquals(this IEnumerable<XElement> source, XName name, string value)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (source is null)
                return Array.Empty<XElement>();
            if (value is null)
                return source.Where(e => !e.Attributes(name).Any());
            return source.Where(e => e.Attributes(name).Any(a => a.Value == value));
        }

        public static IEnumerable<XElement> WhereAttributeNotEquals(this IEnumerable<XElement> source, XName name, string value)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (source is null)
                return Array.Empty<XElement>();
            if (value is null)
                return source.Where(e => e.Attributes(name).Any());
            return source.Where(e => e.Attributes(name).Any(a => a.Value != value));
        }

        public static IEnumerable<XElement> WhereAttributeEquals(this IEnumerable<XElement> source, XName name, bool value)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (source is null)
                return Array.Empty<XElement>();
            return source.Attributes(name).Where(a => a.TryGetBooleanValue(out bool b) && b == value).Select(a => a.Parent);
        }

        public static IEnumerable<XElement> WhereAttributeNotEquals(this IEnumerable<XElement> source, XName name, bool value)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (source is null)
                return Array.Empty<XElement>();
            return source.Attributes(name).Where(a => a.TryGetBooleanValue(out bool b) && b != value).Select(a => a.Parent);
        }

        public static IEnumerable<XElement> WhereAttributeEquals(this IEnumerable<XElement> source, XName name, int value)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (source is null)
                return Array.Empty<XElement>();
            return source.Attributes(name).Where(a => a.TryGetIntegerValue(out int i) && i == value).Select(a => a.Parent);
        }

        public static IEnumerable<XElement> WhereAttributeNotEquals(this IEnumerable<XElement> source, XName name, int value)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (source is null)
                return Array.Empty<XElement>();
            return source.Attributes(name).Where(a => a.TryGetIntegerValue(out int i) && i != value).Select(a => a.Parent);
        }

        public static IEnumerable<XElement> WhereAttributeEqualsAny(this IEnumerable<XElement> source, XName name, params string[] values)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (source is null || values is null || values.Length == 0)
                return Array.Empty<XElement>();
            int nullCount = values.Count(v => v is null);
            if (nullCount == values.Length)
                return source.Where(e => !e.Attributes(name).Any());
            if (nullCount > 0)
            {
                values = values.Where(v => !(v is null)).ToArray();
                return source.Where(e => !e.Attributes(name).Any() || e.Attributes(name).Any(a => values.Contains(a.Value)));
            }
            return source.Where(e => e.Attributes(name).Any(a => values.Contains(a.Value)));
        }

        public static IEnumerable<XElement> WhereAttributeNotEqualsAny(this IEnumerable<XElement> source, XName name, params string[] values)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (source is null || values is null || values.Length == 0)
                return Array.Empty<XElement>();
            if (values.Any(v => v is null))
                return source.Where(e => e.Attributes(name).Any());
            return source.Where(e => e.Attributes(name).Any(a => values.Contains(a.Value)));
        }

        public static IEnumerable<XElement> WhereAttributeEqualsAny(this IEnumerable<XElement> source, XName name, params int[] values)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (source is null || values is null || values.Length == 0)
                return Array.Empty<XElement>();
            return source.Attributes(name).Where(a => a.TryGetIntegerValue(out int i) && values.Contains(i)).Select(a => a.Parent);
        }

        public static IEnumerable<XElement> WhereAttributeNotEqualsAny(this IEnumerable<XElement> source, XName name, params int[] values)
        {
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            if (source is null || values is null || values.Length == 0)
                return Array.Empty<XElement>();
            return source.Attributes(name).Where(a => a.TryGetIntegerValue(out int i) && !values.Contains(i)).Select(a => a.Parent);
        }

        public static XElement ApplyValue(this XElement target, object value)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            target.SetValue(value);
            return target;
        }

        public static XElement ApplyAttributeValue(this XElement target, XName name, object value)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            target.SetAttributeValue(name, value);
            return target;
        }

        public static XElement ApplyElementValue(this XElement target, XName name, object value)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            if (name is null)
                throw new ArgumentNullException(nameof(name));
            target.SetElementValue(name, value);
            return target;
        }

        public static XElement Append(this XElement target, object content)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            target.Add(content);
            return target;
        }

        public static XElement Append(this XElement target, object[] content)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            target.Add(content);
            return target;
        }

        public static XElement ApplyFirst(this XElement target, object[] content)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            target.AddFirst(content);
            return target;
        }

        public static XElement ApplyFirst(this XElement target, object content)
        {
            if (target is null)
                throw new ArgumentNullException(nameof(target));
            target.AddFirst(content);
            return target;
        }

        public static T NextOfType<T>(this XmlNode node)
            where T : XmlNode
        {
            if (node is null)
                return null;
            for (XmlNode n = node.NextSibling; !(n is null); n = n.NextSibling)
            {
                if (n is T t)
                    return t;
            }
            return null;
        }

        public static T PreviousOfType<T>(this XmlNode node)
            where T : XmlNode
        {
            if (node is null)
                return null;
            for (XmlNode n = node.PreviousSibling; !(n is null); n = n.PreviousSibling)
            {
                if (n is T t)
                    return t;
            }
            return null;
        }

        public static IEnumerable<T> SiblingsOfType<T>(this XmlNode node)
            where T : XmlNode
        {
            for (T t = NextOfType<T>(node); !(t is null); t = NextOfType<T>(t))
                yield return t;
            for (T t = PreviousOfType<T>(node); !(t is null); t = PreviousOfType<T>(t))
                yield return t;
        }

        public static IEnumerable<T> FollowingOfType<T>(this XmlNode node)
            where T : XmlNode
        {
            for (T t = NextOfType<T>(node); !(t is null); t = NextOfType<T>(t))
                yield return t;
        }

        public static IEnumerable<T> PrecedingOfType<T>(this XmlNode node)
            where T : XmlNode
        {
            for (T t = PreviousOfType<T>(node); !(t is null); t = PreviousOfType<T>(t))
                yield return t;
        }

        private static string ThrowIfNotNCName([AllowNull] string name, [NotNull] string paramName)
        {
            if (name is null)
                throw new ArgumentNullException(paramName);
            if (name.Length == 0)
                throw new ArgumentOutOfRangeException(paramName, name);
            try { return XmlConvert.VerifyNCName(name); }
            catch (Exception exc)
            {
                throw new ArgumentOutOfRangeException(paramName, name, exc.Message);
            }
        }

        public static XmlElement NewDocument(string prefix, [NotNull] string localName, string namespaceURI)
        {
            XmlDocument ownerDocument = new XmlDocument();
            if (string.IsNullOrEmpty(prefix))
                return (XmlElement)ownerDocument.AppendChild(ownerDocument.CreateElement(ThrowIfNotNCName(localName, nameof(localName)), namespaceURI ?? ""));
            return (XmlElement)ownerDocument.AppendChild(ownerDocument.CreateElement(ThrowIfNotNCName(prefix, nameof(prefix)),
                ThrowIfNotNCName(localName, nameof(localName)), namespaceURI ?? ""));
        }

        public static XmlElement NewDocument([NotNull] string localName, string namespaceURI) => NewDocument(null, localName, namespaceURI);

        public static XmlElement NewDocument([NotNull] string localName) => NewDocument(localName, null);

        /// <summary>
        /// Appends a new <seealso cref="XmlElement"/> to a parent <seealso cref="XmlElement"/>.
        /// </summary>
        /// <param name="parent">The target <seealso cref="XmlElement"/>.</param>
        /// <param name="prefix">The default name prefix.</param>
        /// <param name="localName">The local name of the new <seealso cref="XmlElement"/>.</param>
        /// <param name="namespaceURI">The namespace URI of the new <seealso cref="XmlElement"/>.</param>
        /// <param name="elementAction">The callback method to invoke after the new <seealso cref="XmlElement"/> is appended.</param>
        /// <returns>The <paramref name="parent"/> <seealso cref="XmlElement"/>, which can be used for function chaining.</returns>
        public static XmlElement AppendElement([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, [NotNull] Action<XmlElement> elementAction)
        {
            if (parent is null)
                throw new ArgumentNullException(nameof(parent));

            if (string.IsNullOrEmpty(prefix))
                elementAction((XmlElement)parent.AppendChild(parent.OwnerDocument.CreateElement(ThrowIfNotNCName(localName, nameof(localName)), namespaceURI ?? "")));
            else
            {
                string p = parent.GetPrefixOfNamespace(ThrowIfNotNCName(prefix, nameof(prefix)));
                elementAction((XmlElement)parent.AppendChild(parent.OwnerDocument.CreateElement((string.IsNullOrEmpty(p)) ? prefix : p,
                    ThrowIfNotNCName(localName, nameof(localName)), namespaceURI ?? "")));
            }
            return parent;
        }

        /// <summary>
        /// Appends a new <seealso cref="XmlElement"/> to a parent <seealso cref="XmlElement"/>.
        /// </summary>
        /// <param name="parent">The target <seealso cref="XmlElement"/>.</param>
        /// <param name="localName">The local name of the new <seealso cref="XmlElement"/>.</param>
        /// <param name="namespaceURI">The namespace URI of the new <seealso cref="XmlElement"/>.</param>
        /// <param name="elementAction">The callback method to invoke after the new <seealso cref="XmlElement"/> is appended.</param>
        /// <returns>The <paramref name="parent"/> <seealso cref="XmlElement"/>, which can be used for function chaining.</returns>
        public static XmlElement AppendElement([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, [NotNull] Action<XmlElement> elementAction)
            => AppendElement(parent, null, localName, namespaceURI, elementAction);

        /// <summary>
        /// Appends a new <seealso cref="XmlElement"/> to a parent <seealso cref="XmlElement"/>.
        /// </summary>
        /// <param name="parent">The target <seealso cref="XmlElement"/>.</param>
        /// <param name="localName">The local name of the new <seealso cref="XmlElement"/>.</param>
        /// <param name="elementAction">The callback method to invoke after the new <seealso cref="XmlElement"/> is appended.</param>
        /// <returns>The <paramref name="parent"/> <seealso cref="XmlElement"/>, which can be used for function chaining.</returns>
        public static XmlElement AppendElement([NotNull] this XmlElement parent, [NotNull] string localName, [NotNull] Action<XmlElement> elementAction)
            => AppendElement(parent, localName, null, elementAction);

        #region WithElement overloads

        /// <summary>
        /// Invokes an <seealso cref="Action{T}">Action</seealso><c>&lt;<seealso cref="XmlElement"/>&gt;</c> on an element matching the specified qualified name,
        /// appending a new element if no element already exists that matches the specified name.
        /// </summary>
        /// <param name="parent">The target <seealso cref="XmlElement"/>.</param>
        /// <param name="prefix">The default name prefix to use if a new element is appended.</param>
        /// <param name="localName">The local name of the <seealso cref="XmlElement"/>.</param>
        /// <param name="namespaceURI">The namespace URI of the <seealso cref="XmlElement"/>.</param>
        /// <param name="elementAction">The callback method to invoke for the matching or appended <seealso cref="XmlElement"/>.</param>
        /// <returns>The <paramref name="parent"/> <seealso cref="XmlElement"/>, which can be used for function chaining.</returns>
        public static XmlElement WithElement([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, [NotNull] Action<XmlElement> elementAction)
        {
            if (parent is null)
                throw new ArgumentNullException(nameof(parent));

            XmlElement element = (parent.IsEmpty) ? null : ((string.IsNullOrEmpty(namespaceURI)) ?
                parent.ChildNodes.OfType<XmlElement>().FirstOrDefault(e => e.LocalName.Equals(localName) && e.NamespaceURI.Length == 0) :
                parent.ChildNodes.OfType<XmlElement>().FirstOrDefault(e => e.LocalName.Equals(localName) && e.NamespaceURI.Equals(namespaceURI)));
            if (element is null)
                return AppendElement(parent, prefix, localName, namespaceURI, elementAction);
            elementAction(element);
            return parent;
        }

        public static XmlElement WithElement([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, string value)
        {
            if (parent is null)
                throw new ArgumentNullException(nameof(parent));

            XmlElement element = (parent.IsEmpty) ? null : ((string.IsNullOrEmpty(namespaceURI)) ?
                parent.ChildNodes.OfType<XmlElement>().FirstOrDefault(e => e.LocalName.Equals(localName) && e.NamespaceURI.Length == 0) :
                parent.ChildNodes.OfType<XmlElement>().FirstOrDefault(e => e.LocalName.Equals(localName) && e.NamespaceURI.Equals(namespaceURI)));
            if (element is null)
            {
                if (value is null)
                    return parent;
                return AppendElement(parent, prefix, localName, namespaceURI, e => e.InnerText = value);
            }
            if (value is null)
            {
                parent.RemoveChild(element);
                return parent;
            }
            return WithInnerText(element, value);
        }

        public static XmlElement WithElement([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, bool? value) =>
            WithElement(parent, prefix, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, Guid? value) =>
            WithElement(parent, prefix, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, int? value) =>
            WithElement(parent, prefix, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, long? value) =>
            WithElement(parent, prefix, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, uint? value) =>
            WithElement(parent, prefix, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, ulong? value) =>
            WithElement(parent, prefix, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, byte? value) =>
            WithElement(parent, prefix, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, sbyte? value) =>
            WithElement(parent, prefix, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, float? value) =>
            WithElement(parent, prefix, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, double? value) =>
            WithElement(parent, prefix, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, char? value) =>
            WithElement(parent, prefix, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, DateTime? value, XmlDateTimeSerializationMode dateTimeOption) =>
            WithElement(parent, prefix, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value, dateTimeOption) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, Uri value) =>
            WithElement(parent, prefix, localName, namespaceURI, (value is null) ? null : ((value.IsAbsoluteUri) ? value.AbsoluteUri : value.OriginalString));

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, [NotNull] Action<XmlElement> elementAction) =>
            WithElement(parent, null, localName, namespaceURI, elementAction);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, string value) =>
            WithElement(parent, null, localName, namespaceURI, value);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, bool? value) =>
            WithElement(parent, null, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, Guid? value) =>
            WithElement(parent, null, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, int? value) =>
            WithElement(parent, null, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, long? value) =>
            WithElement(parent, null, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, uint? value) =>
            WithElement(parent, null, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, ulong? value) =>
            WithElement(parent, null, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, byte? value) =>
            WithElement(parent, null, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, sbyte? value) =>
            WithElement(parent, null, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, float? value) =>
            WithElement(parent, null, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, double? value) =>
            WithElement(parent, null, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, char? value) =>
            WithElement(parent, null, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, DateTime? value, XmlDateTimeSerializationMode dateTimeOption) =>
            WithElement(parent, null, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value, dateTimeOption) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, Uri value) =>
            WithElement(parent, null, localName, namespaceURI, (value is null) ? null : ((value.IsAbsoluteUri) ? value.AbsoluteUri : value.OriginalString));

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, [NotNull] Action<XmlElement> elementAction) =>
            WithElement(parent, localName, null, elementAction);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, string value) =>
            WithElement(parent, localName, null, value);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, bool? value) =>
            WithElement(parent, localName, null, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, Guid? value) =>
            WithElement(parent, localName, null, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, int? value) =>
            WithElement(parent, localName, null, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, long? value) =>
            WithElement(parent, localName, null, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, uint? value) =>
            WithElement(parent, localName, null, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, ulong? value) =>
            WithElement(parent, localName, null, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, byte? value) =>
            WithElement(parent, localName, null, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, sbyte? value) =>
            WithElement(parent, localName, null, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, float? value) =>
            WithElement(parent, localName, null, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, double? value) =>
            WithElement(parent, localName, null, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, char? value) =>
            WithElement(parent, localName, null, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, DateTime? value, XmlDateTimeSerializationMode dateTimeOption) =>
            WithElement(parent, localName, null, (value.HasValue) ? XmlConvert.ToString(value.Value, dateTimeOption) : null);

        public static XmlElement WithElement([NotNull] this XmlElement parent, [NotNull] string localName, Uri value) =>
            WithElement(parent, localName, null, (value is null) ? null : ((value.IsAbsoluteUri) ? value.AbsoluteUri : value.OriginalString));

        #endregion

        #region WithAttribute overloads

        /// <summary>
        /// Invokes an <seealso cref="Action{T}">Action</seealso><c>&lt;<seealso cref="XmlAttribute"/>&gt;</c> on an attribute matching the specified qualified name,
        /// appending a new attribute if no attribute already exists that matches the specified name.
        /// </summary>
        /// <param name="parent">The target <seealso cref="XmlElement"/>.</param>
        /// <param name="prefix">The default name prefix to use if a new attribute is appended.</param>
        /// <param name="localName">The local name of the <seealso cref="XmlAttribute"/>.</param>
        /// <param name="namespaceURI">The namespace URI of the <seealso cref="XmlAttribute"/>.</param>
        /// <param name="attributeAction">The callback method to invoke for the matching or appended <seealso cref="XmlAttribute"/>.</param>
        /// <returns>The <paramref name="parent"/> <seealso cref="XmlElement"/>, which can be used for function chaining.</returns>
        public static XmlElement WithAttribute([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, Action<XmlAttribute> attributeAction)
        {
            XmlAttribute attribute = parent.GetAttributeNode(ThrowIfNotNCName(localName, nameof(localName)), namespaceURI ?? "");
            if (attribute is null)
            {
                if (string.IsNullOrEmpty(prefix))
                    attributeAction(parent.Attributes.Append(parent.OwnerDocument.CreateAttribute(ThrowIfNotNCName(localName, nameof(localName)), namespaceURI ?? "")));
                else
                {
                    string p = parent.GetPrefixOfNamespace(ThrowIfNotNCName(prefix, nameof(prefix)));
                    attributeAction(parent.Attributes.Append(parent.OwnerDocument.CreateAttribute((string.IsNullOrEmpty(p)) ? prefix : p, ThrowIfNotNCName(localName, nameof(localName)),
                        namespaceURI ?? "")));
                }
            }
            else
                attributeAction(attribute);
            return parent;
        }

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, string value)
        {
            if (parent is null)
                throw new ArgumentNullException(nameof(parent));

            XmlAttribute attribute = parent.GetAttributeNode(localName);
            if (attribute is null)
            {
                if (value is null)
                    return parent;
                if (string.IsNullOrEmpty(prefix))
                    parent.Attributes.Append(parent.OwnerDocument.CreateAttribute(ThrowIfNotNCName(localName, nameof(localName)), namespaceURI ?? "")).Value = value;
                else
                {
                    string p = parent.GetPrefixOfNamespace(ThrowIfNotNCName(prefix, nameof(prefix)));
                    parent.Attributes.Append(parent.OwnerDocument.CreateAttribute((string.IsNullOrEmpty(p)) ? prefix : p, ThrowIfNotNCName(localName, nameof(localName)), namespaceURI ?? "")).Value = value;
                }
            }
            else if (value is null)
                parent.Attributes.Remove(attribute);
            else
                attribute.Value = value;
            return parent;
        }

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, bool? value) =>
            WithAttribute(parent, prefix, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, Guid? value) =>
            WithAttribute(parent, prefix, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, int? value) =>
            WithAttribute(parent, prefix, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, long? value) =>
            WithAttribute(parent, prefix, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, uint? value) =>
            WithAttribute(parent, prefix, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, ulong? value) =>
            WithAttribute(parent, prefix, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, byte? value) =>
            WithAttribute(parent, prefix, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, sbyte? value) =>
            WithAttribute(parent, prefix, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, float? value) =>
            WithAttribute(parent, prefix, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, double? value) =>
            WithAttribute(parent, prefix, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, char? value) =>
            WithAttribute(parent, prefix, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, DateTime? value, XmlDateTimeSerializationMode dateTimeOption) =>
            WithAttribute(parent, prefix, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value, dateTimeOption) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, string prefix, [NotNull] string localName, string namespaceURI, Uri value) =>
            WithAttribute(parent, prefix, localName, namespaceURI, (value is null) ? null : ((value.IsAbsoluteUri) ? value.AbsoluteUri : value.OriginalString));

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, Action<XmlAttribute> attributeAction) =>
            WithAttribute(parent, null, localName, namespaceURI, attributeAction);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, string value) =>
            WithAttribute(parent, null, localName, namespaceURI, value);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, bool? value) =>
            WithAttribute(parent, null, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, Guid? value) =>
            WithAttribute(parent, null, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, int? value) =>
            WithAttribute(parent, null, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, long? value) =>
            WithAttribute(parent, null, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, uint? value) =>
            WithAttribute(parent, null, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, ulong? value) =>
            WithAttribute(parent, null, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, byte? value) =>
            WithAttribute(parent, null, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, sbyte? value) =>
            WithAttribute(parent, null, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, float? value) =>
            WithAttribute(parent, null, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, double? value) =>
            WithAttribute(parent, null, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, char? value) =>
            WithAttribute(parent, null, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, DateTime? value, XmlDateTimeSerializationMode dateTimeOption) =>
            WithAttribute(parent, null, localName, namespaceURI, (value.HasValue) ? XmlConvert.ToString(value.Value, dateTimeOption) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, Uri value) =>
            WithAttribute(parent, null, localName, namespaceURI, (value is null) ? null : ((value.IsAbsoluteUri) ? value.AbsoluteUri : value.OriginalString));

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, Action<XmlAttribute> attributeAction) =>
            WithAttribute(parent, localName, null, attributeAction);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, string value) =>
            WithAttribute(parent, localName, null, value);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, bool? value) =>
            WithAttribute(parent, localName, null, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, Guid? value) =>
            WithAttribute(parent, localName, null, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, int? value) =>
            WithAttribute(parent, localName, null, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, long? value) =>
            WithAttribute(parent, localName, null, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, uint? value) =>
            WithAttribute(parent, localName, null, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, ulong? value) =>
            WithAttribute(parent, localName, null, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, byte? value) =>
            WithAttribute(parent, localName, null, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, sbyte? value) =>
            WithAttribute(parent, localName, null, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, float? value) =>
            WithAttribute(parent, localName, null, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, double? value) =>
            WithAttribute(parent, localName, null, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, char? value) =>
            WithAttribute(parent, localName, null, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, DateTime? value, XmlDateTimeSerializationMode dateTimeOption) =>
            WithAttribute(parent, localName, null, (value.HasValue) ? XmlConvert.ToString(value.Value, dateTimeOption) : null);

        public static XmlElement WithAttribute([NotNull] this XmlElement parent, [NotNull] string localName, Uri value) =>
            WithAttribute(parent, localName, null, (value is null) ? null : ((value.IsAbsoluteUri) ? value.AbsoluteUri : value.OriginalString));

        #endregion

        public static XmlElement IfAttribute([NotNull] this XmlElement parent, [NotNull] string localName, string namespaceURI, Action<XmlAttribute> ifAttributeExists)
        {
            XmlAttribute attribute = parent.GetAttributeNode(localName, namespaceURI ?? "");
            if (!(attribute is null))
                ifAttributeExists(attribute);
            return parent;
        }

        public static XmlElement IfAttribute([NotNull] this XmlElement parent, [NotNull] string localName, Action<XmlAttribute> ifAttributeExists)
            => IfAttribute(parent, localName, null, ifAttributeExists);

        public static readonly Regex WhitespaceRegex = new Regex(@"^[^\S\r\n]*(?<s>(\r\n?|\n)[^\S\r\n]*(\r\n?|\n))?[\s\r\n]*$");

        #region WithInnerText overloads

        public static XmlElement WithInnerText([NotNull] this XmlElement parent, string value)
        {
            if (parent.IsEmpty)
            {
                if (!(value is null))
                    parent.InnerText = value;
                return parent;
            }
            XmlCharacterData[] textNodes = parent.ChildNodes.OfType<XmlCharacterData>().Where(n => !(n is XmlComment)).ToArray();
            int tc = textNodes.Length;
            if (tc == parent.ChildNodes.Count)
            {
                if (value is null)
                    parent.IsEmpty = true;
                else
                    parent.InnerText = value;
                return parent;
            }
            if (value is null)
            {
                foreach (XmlCharacterData d in textNodes)
                    parent.RemoveChild(d);
                return parent;
            }
            if (value.Length == 0)
            {
                foreach (XmlCharacterData n in textNodes.Where(n => !(n is XmlWhitespace || n.InnerText.Length == 0)).ToArray())
                    parent.RemoveChild(n);
                return parent;
            }
            XmlCharacterData[] nwsNodes = textNodes.Where(n => n.InnerText.Any(c => !char.IsWhiteSpace(c))).ToArray();
            int nc = nwsNodes.Length - 1;
            Match match = WhitespaceRegex.Match(value);
            XmlCharacterData cdn;
            if (nc > -1)
            {
                // At least one has non-whitespace
                cdn = nwsNodes[nc];
                if (match.Success)
                    parent.ReplaceChild((match.Groups["s"].Success) ? (XmlNode)parent.OwnerDocument.CreateSignificantWhitespace(value) :
                        parent.OwnerDocument.CreateWhitespace(value), cdn);
                else if (cdn is XmlCDataSection cDataSection)
                    cDataSection.InnerText = value;
                else if (cdn is XmlText xmlText)
                    xmlText.InnerText = value;
                else
                    parent.ReplaceChild(parent.OwnerDocument.CreateTextNode(value), cdn);
                if (nc > 0)
                    foreach (XmlCDataSection n in nwsNodes.Take(nc))
                        parent.RemoveChild(n);
                return parent;
            }

            if ((nc = (nwsNodes = textNodes.Where(n => !(n is XmlWhitespace)).ToArray()).Length - 1) < 0)
            {
                parent.AppendChild(
                    (match.Success) ? ((match.Groups["s"].Success) ? (XmlNode)parent.OwnerDocument.CreateSignificantWhitespace(value) : parent.OwnerDocument.CreateWhitespace(value))
                    : parent.OwnerDocument.CreateTextNode(value)
                );
                return parent;
            }
            cdn = nwsNodes[nc];
            if (match.Success)
            {
                if (match.Groups["s"].Success)
                {
                    if (cdn is XmlSignificantWhitespace significantWhitespace)
                        significantWhitespace.InnerText = value;
                    else
                        parent.ReplaceChild((XmlNode)parent.OwnerDocument.CreateSignificantWhitespace(value), cdn);
                }
                else if (cdn is XmlWhitespace xmlWhitespace)
                    xmlWhitespace.InnerText = value;
                else
                    parent.ReplaceChild((XmlNode)parent.OwnerDocument.CreateWhitespace(value), cdn);
            }
            else
                parent.ReplaceChild((XmlNode)parent.OwnerDocument.CreateTextNode(value), cdn);
            if (nc > 0)
                foreach (XmlCDataSection n in nwsNodes.Take(nc))
                    parent.RemoveChild(n);
            return parent;
        }

        public static XmlElement WithInnerText([NotNull] this XmlElement parent, [NotNull] Action<IEnumerable<XmlCharacterData>> innerTextNodes)
        {
            if (parent.IsEmpty)
                innerTextNodes(Array.Empty<XmlCharacterData>());
            IEnumerable<XmlCharacterData> textNodes = parent.ChildNodes.OfType<XmlCharacterData>().Where(n => !(n is XmlComment));
            if (textNodes.Skip(1).Any())
            {
                if (textNodes.Any(n => n.InnerText.Trim().Length > 0))
                    innerTextNodes(textNodes.Where(n => n.InnerText.Trim().Length > 0));
                else if (!textNodes.All(n => n is XmlWhitespace))
                    innerTextNodes(textNodes.Where(n => !(n is XmlWhitespace)));
            }
            else
                innerTextNodes(textNodes);
            return parent;
        }

        public static XmlElement WithInnerText([NotNull] this XmlElement parent, bool? value) =>
            WithInnerText(parent, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithInnerText([NotNull] this XmlElement parent, Guid? value) =>
            WithInnerText(parent, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithInnerText([NotNull] this XmlElement parent, int? value) =>
            WithInnerText(parent, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithInnerText([NotNull] this XmlElement parent, long? value) =>
            WithInnerText(parent, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithInnerText([NotNull] this XmlElement parent, uint? value) =>
            WithInnerText(parent, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithInnerText([NotNull] this XmlElement parent, ulong? value) =>
            WithInnerText(parent, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithInnerText([NotNull] this XmlElement parent, byte? value) =>
            WithInnerText(parent, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithInnerText([NotNull] this XmlElement parent, sbyte? value) =>
            WithInnerText(parent, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithInnerText([NotNull] this XmlElement parent, float? value) =>
            WithInnerText(parent, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithInnerText([NotNull] this XmlElement parent, double? value) =>
            WithInnerText(parent, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithInnerText([NotNull] this XmlElement parent, char? value) =>
            WithInnerText(parent, (value.HasValue) ? XmlConvert.ToString(value.Value) : null);

        public static XmlElement WithInnerText([NotNull] this XmlElement parent, DateTime? value, XmlDateTimeSerializationMode dateTimeOption) =>
            WithInnerText(parent, (value.HasValue) ? XmlConvert.ToString(value.Value, dateTimeOption) : null);

        public static XmlElement WithInnerText([NotNull] this XmlElement parent, Uri value) =>
            WithInnerText(parent, (value is null) ? null : ((value.IsAbsoluteUri) ? value.AbsoluteUri : value.OriginalString));

        #endregion
    }
}
