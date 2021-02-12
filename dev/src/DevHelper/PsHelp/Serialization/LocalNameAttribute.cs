using System;

namespace DevHelper.PsHelp.Serialization
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public sealed class LocalNameAttribute : Attribute
    {
        public string Value { get; }
        public LocalNameAttribute(string value) { Value = value; }
    }
}
