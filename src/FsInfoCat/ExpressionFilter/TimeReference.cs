using System;
using System.Collections;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Windows;

namespace FsInfoCat.ExpressionFilter
{
    public abstract class TimeReference : NotifyDataErrorInfo, ITimeReference
    {
        private readonly IPropertyChangeTracker<bool> _isExclusive;
        private readonly IPropertyChangeTracker<bool> _includeNull;

        public bool IsExclusive { get => _isExclusive.GetValue(); set => _isExclusive.SetValue(value); }

        public bool IncludeNull { get => _includeNull.GetValue(); set => _includeNull.SetValue(value); }

        public TimeReference()
        {
            _isExclusive = AddChangeTracker(nameof(IsExclusive), false);
            _includeNull = AddChangeTracker(nameof(IncludeNull), false);
        }

        protected abstract int CompareTo(DateTime other);

        public int CompareTo(DateTime? other) => other.HasValue ? CompareTo(other.Value) : IncludeNull ? 0 : 1;

        public abstract DateTime ToDateTime();
    }
}
