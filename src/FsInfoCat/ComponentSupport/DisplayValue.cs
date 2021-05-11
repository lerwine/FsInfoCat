using System.ComponentModel;

namespace FsInfoCat.ComponentSupport
{
    internal class DisplayValue<TValue> : IDisplayValue<TValue>
    {
        internal DisplayValue(TValue sourceValue, string stringValue)
        {
            SourceValue = sourceValue;
            StringValue = stringValue ?? "";
        }

        public TValue SourceValue { get; }

        object IDisplayValue.SourceValue => SourceValue;

        public string StringValue { get; }
    }
}
