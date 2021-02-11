using System.Collections.ObjectModel;
using System.Threading;

namespace DevHelper
{
    [System.AttributeUsage(System.AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class XmlQNameAttribute : System.Attribute
    {
        public XmlDocumentNamespace Prefix { get; }

        public string LocalName { get; set; }

        public XmlQNameAttribute(XmlDocumentNamespace prefix) { Prefix = prefix; }
    }
}
