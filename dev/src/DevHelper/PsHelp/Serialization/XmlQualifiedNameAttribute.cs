using System;

namespace DevHelper.PsHelp.Serialization
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class XmlQualifiedNameAttribute : Attribute
    {
        public NamespaceURI NamespaceURI { get; }

        public string LocalName { get; set; }

        public XmlQualifiedNameAttribute(NamespaceURI namespaceURI)
        {
            NamespaceURI = namespaceURI;
        }
    }
}
