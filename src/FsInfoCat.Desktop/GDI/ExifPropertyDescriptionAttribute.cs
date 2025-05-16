using System;

namespace FsInfoCat.Desktop.GDI
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = false)]
    public sealed class ExifPropertyDescriptionAttribute(string displayName) : Attribute
    {
        private readonly string _displayName = displayName;
        private string _summary = "";
        private string _remarks = "";

        public string DisplayName { get { return _displayName; } }

        public string Summary { get { return _summary; } set { _summary = (value == null) ? "" : value.Trim(); } }

        public string Remarks { get { return _remarks; } set { _remarks = (value == null) ? "" : value.Trim(); } }
    }
}
