using System;

namespace DevHelper.PsHelp.Serialization
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class DefaultPrefixAttribute : Attribute
    {
        public string Value { get; }
        public DefaultPrefixAttribute(string value) { Value = value; }
    }
}
