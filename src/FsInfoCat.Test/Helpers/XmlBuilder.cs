using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace FsInfoCat.Test.Helpers
{
    /// <summary>
    /// Extension methods for building <seealso cref="XmlNode">XmlNodes</seealso> using method chaining.
    /// </summary>
    public static class XmlBuilder
    {
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
                innerTextNodes(new XmlCharacterData[0]);
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
