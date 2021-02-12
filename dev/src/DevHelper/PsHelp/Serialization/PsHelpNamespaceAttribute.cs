using System;

namespace DevHelper.PsHelp.Serialization
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class PsHelpNamespaceAttribute : Attribute
    {
        public string URI { get; }

        public string Prefix { get; set; }

        public bool IsCommandHelpNamespace { get; set; }

        public PsHelpNamespaceAttribute(string uri) { URI = uri; }
    }
}
