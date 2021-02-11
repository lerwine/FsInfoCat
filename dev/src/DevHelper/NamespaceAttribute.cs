using System;

namespace DevHelper
{
    [AttributeUsage(System.AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class NamespaceAttribute : System.Attribute
    {
        public NamespaceAttribute(string absoluteUri)
        {
            URI = (Uri.TryCreate(absoluteUri, UriKind.Absolute, out Uri uri)) ? uri.AbsolutePath : "";
        }

        public string URI { get; }

        public string SchemaLocation { get; set; }

        public string Prefix { get; set; }

        public bool IsCommandNS { get; set; }

        public bool ElementFormQualified { get; set; }
    }
}
