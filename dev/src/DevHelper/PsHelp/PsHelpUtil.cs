using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Xml;
using DevHelper.PsHelp.Serialization;

namespace DevHelper.PsHelp
{
    public static class PsHelpUtil
    {
        public const string NAMESPACE_URI_xmlns = "http://www.w3.org/2000/xmlns/";
        public const string Namespace_URI_xsi = "http://www.w3.org/2001/XMLSchema-instance";
        public const string Namespace_URI_msh = "http://msh";
        public const string Namespace_URI_maml = "http://schemas.microsoft.com/maml/2004/10";
        public const string Namespace_URI_command = "http://schemas.microsoft.com/maml/dev/command/2004/10";
        public const string Namespace_URI_dev = "http://schemas.microsoft.com/maml/dev/2004/10";

        public static IEnumerable<TAttribute> GetFieldAttributes<TAttribute>(this Type type, string name)
            where TAttribute : Attribute
        {
            BindingFlags flags = BindingFlags.GetField | BindingFlags.Public | BindingFlags.Static;
            FieldInfo fieldInfo = type.GetField(name, flags);
            if (fieldInfo is null && (fieldInfo = type.GetField(name, flags | BindingFlags.IgnoreCase)) is null)
                return new TAttribute[0];
            return fieldInfo.GetCustomAttributes(typeof(TAttribute), false).Cast<TAttribute>();
        }

        public static string GetStringValue(this NamespaceURI value, out string defaultPrefix)
        {
            Type t = value.GetType();
            string name = Enum.GetName(t, value);
            defaultPrefix = t.GetFieldAttributes<DefaultPrefixAttribute>(name).Select(a => a.Value).DefaultIfEmpty(name).First();
            return t.GetFieldAttributes<PsHelpNamespaceAttribute>(name).Select(a => a.URI).DefaultIfEmpty("").First();
        }

        public static string URL(this NamespaceURI value)
        {
            Type t = value.GetType();
            string name = Enum.GetName(t, value);
            return t.GetFieldAttributes<PsHelpNamespaceAttribute>(name).Select(a => a.URI).DefaultIfEmpty("").First();
        }

        public static bool TryGetCommandHelpNamespace(this NamespaceURI value, out string commandNs)
        {
            Type t = value.GetType();
            string name = Enum.GetName(t, value);
            PsHelpNamespaceAttribute ns = t.GetFieldAttributes<PsHelpNamespaceAttribute>(name).FirstOrDefault();
            if (ns is null || !ns.IsCommandHelpNamespace)
            {
                commandNs = null;
                return false;
            }
            commandNs = ns.URI;
            return true;
        }

        public static string GetDefaultPrefix(this NamespaceURI value)
        {
            Type t = value.GetType();
            string name = Enum.GetName(t, value);
            return t.GetFieldAttributes<DefaultPrefixAttribute>(name).Select(a => a.Value).DefaultIfEmpty(name).First();
        }

        public static string CalculatePrefix(this XmlNode contextNode, string namespaceURI, string defaultPrefix = null)
        {
            string prefix;
            string ns;
            if (string.IsNullOrEmpty(namespaceURI))
            {
                namespaceURI = "";
                prefix = contextNode.GetPrefixOfNamespace("");
                if (string.IsNullOrEmpty(prefix) && string.IsNullOrEmpty(contextNode.GetNamespaceOfPrefix("")))
                    return prefix;
                if (!string.IsNullOrEmpty(defaultPrefix))
                {
                    if (string.IsNullOrEmpty(contextNode.GetNamespaceOfPrefix(defaultPrefix)))
                        return defaultPrefix;
                    prefix = defaultPrefix;
                }
            }
            else
            {
                prefix = contextNode.GetPrefixOfNamespace(namespaceURI);
                if (!string.IsNullOrEmpty(prefix))
                    return prefix;
                ns = contextNode.GetNamespaceOfPrefix(prefix);
                if (namespaceURI.Equals(ns))
                    return prefix;
                if (!string.IsNullOrEmpty(defaultPrefix))
                {
                    ns = contextNode.GetNamespaceOfPrefix(defaultPrefix);
                    if (namespaceURI.Equals(ns) || (string.IsNullOrEmpty(namespaceURI) && string.IsNullOrEmpty(contextNode.GetPrefixOfNamespace(""))))
                        return defaultPrefix;
                    prefix = defaultPrefix;
                }
            }
            if (string.IsNullOrEmpty(prefix))
                prefix = "ns";
            int i = 1;
            string p = "";
            do
            {
                p = $"{prefix}{i}";
                ns = contextNode.GetNamespaceOfPrefix(p);
            } while (!(namespaceURI.Equals(ns) || (string.IsNullOrEmpty(ns) && string.IsNullOrEmpty(contextNode.GetPrefixOfNamespace("")))));
            return p;
        }

        public static string XPathName(this XmlNode contextNode, ElementName value)
        {
            Type t = value.GetType();
            string name = Enum.GetName(t, value);
            NamespaceURI ns = t.GetFieldAttributes<NamespaceDefinitionAttribute>(name).Select(a => a.Value).FirstOrDefault();
            string namespaceURI = ns.GetStringValue(out string defaultPrefix);
            string prefix = contextNode.CalculatePrefix(namespaceURI, defaultPrefix);
            string localName = EnsureNCName(t.GetFieldAttributes<LocalNameAttribute>(name).Select(a => a.Value).DefaultIfEmpty(name).First());
            if (string.IsNullOrEmpty(prefix))
                return localName;
            return $"{prefix}:{localName}";
        }

        public static string LocalName(this ElementName value, out NamespaceURI ns)
        {
            Type t = value.GetType();
            string name = Enum.GetName(t, value);
            ns = t.GetFieldAttributes<NamespaceDefinitionAttribute>(name).Select(a => a.Value).FirstOrDefault();
            return t.GetFieldAttributes<PsHelpNamespaceAttribute>(name).Select(a => a.URI).DefaultIfEmpty("").First();
        }

        public static NamespaceURI NamespaceDefinition(this ElementName value)
        {
            Type t = value.GetType();
            string name = Enum.GetName(t, value);
            return t.GetFieldAttributes<NamespaceDefinitionAttribute>(name).Select(a => a.Value).FirstOrDefault();
        }

        public static string LocalName(this ElementName value, out string namespaceURI, out string defaultPrefix)
        {
            Type t = value.GetType();
            string name = Enum.GetName(t, value);
            defaultPrefix = t.GetFieldAttributes<DefaultPrefixAttribute>(name).Select(a => a.Value).DefaultIfEmpty(name).First();
            namespaceURI = t.GetFieldAttributes<PsHelpNamespaceAttribute>(name).Select(a => a.URI).DefaultIfEmpty("").First();
            return t.GetFieldAttributes<PsHelpNamespaceAttribute>(name).Select(a => a.URI).DefaultIfEmpty(name).First();
        }

        public static string LocalName(this ElementName value)
        {
            Type t = value.GetType();
            string name = Enum.GetName(t, value);
            return t.GetFieldAttributes<LocalNameAttribute>(name).Select(a => a.Value).DefaultIfEmpty(name).First();
        }

        public static string ToCName(this ElementName value)
        {
            Type t = value.GetType();
            string name = Enum.GetName(t, value);
            string defaultPrefix = t.GetFieldAttributes<NamespaceDefinitionAttribute>(name).Select(a => a.Value).FirstOrDefault().GetDefaultPrefix();
            name = t.GetFieldAttributes<LocalNameAttribute>(name).Select(a => a.Value).DefaultIfEmpty(name).First();
            if (string.IsNullOrEmpty(defaultPrefix))
                return name.EnsureNCName();
            return $"{defaultPrefix.EnsureNCName()}:{name.EnsureNCName()}";
        }

        public static bool IsXmlNCName(this string value) => !string.IsNullOrEmpty(value) && XmlConvert.IsStartNCNameChar(value[0]) &&
            (value.Length == 1 || value.ToCharArray().Skip(1).All(c => XmlConvert.IsNCNameChar(c)));

        public static bool IsXmlName(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;
            char[] ca = value.ToCharArray();
            if (!XmlConvert.IsStartNCNameChar(ca[0]))
                return false;
            if (ca.Length == 1)
                return true;
            int i = ca.Skip(1).TakeWhile(c => XmlConvert.IsNCNameChar(c)).Count() + 1;
            if (i == ca.Length)
                return true;
            if (ca[i] != ':' || ++i == ca.Length || !XmlConvert.IsStartNCNameChar(ca[i]))
                return false;
            return ca.Skip(i + 1).All(c => XmlConvert.IsNCNameChar(c));
        }

        public static string EnsureNCName(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return "";
            return (value.IsXmlNCName()) ? value : XmlConvert.EncodeLocalName(value);
        }

    }
}
