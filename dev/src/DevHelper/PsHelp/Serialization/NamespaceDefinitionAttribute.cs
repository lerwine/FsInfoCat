using System;

namespace DevHelper.PsHelp.Serialization
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class NamespaceDefinitionAttribute : Attribute
    {
        public NamespaceURI Value { get; }
        public NamespaceDefinitionAttribute(NamespaceURI value) { Value = value; }
    }
}
