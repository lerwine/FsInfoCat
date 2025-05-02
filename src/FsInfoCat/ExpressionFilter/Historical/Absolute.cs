using System;

namespace FsInfoCat.ExpressionFilter.Historical
{
    // TODO: Document Absolute class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Absolute : HistoricalTimeReference, IAbsoluteTimeReference
    {
        private readonly IPropertyChangeTracker<DateTime> _value;

        public DateTime Value { get => _value.GetValue(); set => _value.SetValue(value); }

        public Absolute()
        {
            _value = AddChangeTracker(nameof(Value), DateTime.Now);
        }

        public override DateTime ToDateTime() => Value;

        protected override int CompareTo(DateTime other) => Value.CompareTo(other);
    }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
}
