using System;
using System.ComponentModel;

namespace FsInfoCat.Model
{
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    public sealed class DefaultValueDbSettingsAttribute : DefaultValueAttribute
    {
        public DefaultValueDbSettingsAttribute(string settingsName) : base(settingsName) { }

        public override object Value
        {
            get
            {
                string settingsName = base.Value as string;
                return string.IsNullOrWhiteSpace(settingsName) ? null : DBSettings.Default[settingsName];
            }
        }
    }
}
