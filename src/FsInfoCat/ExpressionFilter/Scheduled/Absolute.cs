using System;

namespace FsInfoCat.ExpressionFilter.Scheduled
{
    // TODO: Document Absolute class
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    public class Absolute : SchedulableTimeReference, IAbsoluteTimeReference
    {
        private readonly IPropertyChangeTracker<DateTime> _value;

        public DateTime Value { get => _value.GetValue(); set => _value.SetValue(value); }

        public Absolute()
        {
            _value = AddChangeTracker(nameof(Value), default(DateTime));
        }

        public override DateTime ToDateTime() => Value;

        protected override int CompareTo(DateTime other) => Value.CompareTo(other);
    }
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
}
