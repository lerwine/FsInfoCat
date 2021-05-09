using FsInfoCat.Desktop.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading;

namespace FsInfoCat.Desktop.Model.ComponentSupport
{
    public class ValidationAttributeList : Collection<ValidationAttribute>
    {
        private readonly object _syncRoot = new object();

        public ValidationAttributeList() { }

        public void EnsureCustomValidator<T>(string methodName)
        {
            if (methodName is null)
                throw new ArgumentNullException(methodName);
            Type type = typeof(T);
            if (!Items.OfType<CustomValidationAttribute>().Any(a => type.Equals(a.ValidatorType) && methodName.Equals(a.Method)))
                Add(new CustomValidationAttribute(type, methodName));
        }

        public void SetMaxLength(int? value)
        {
            MaxLengthAttribute attribute = Items.OfType<MaxLengthAttribute>().FirstOrDefault();
            if (value.HasValue)
            {
                if (value.Value < 1)
                    throw new ArgumentOutOfRangeException(nameof(value));
                if (attribute is null || attribute.Length != value.Value)
                    Add(new MaxLengthAttribute(value.Value));
            }
            else if (!(attribute is null))
                Remove(attribute);
        }

        public void SetMaxLength(int? value, string errorMessage)
        {
            MaxLengthAttribute attribute = Items.OfType<MaxLengthAttribute>().FirstOrDefault();
            if (value.HasValue)
            {
                if (value.Value < 1)
                    throw new ArgumentOutOfRangeException(nameof(value));
                if (attribute is null || attribute.Length != value.Value || !string.Equals(errorMessage, attribute.ErrorMessage))
                    Add(new MaxLengthAttribute(value.Value));
            }
            else if (!(attribute is null))
                Remove(attribute);
        }

        public void SetMinLength(int? value)
        {
            MinLengthAttribute attribute = Items.OfType<MinLengthAttribute>().FirstOrDefault();
            if (value.HasValue)
            {
                if (value.Value < 1)
                    throw new ArgumentOutOfRangeException(nameof(value));
                if (attribute is null || attribute.Length != value.Value)
                    Add(new MinLengthAttribute(value.Value));
            }
            else if (!(attribute is null))
                Remove(attribute);
        }

        public void SetMinLength(int? value, string errorMessage)
        {
            MinLengthAttribute attribute = Items.OfType<MinLengthAttribute>().FirstOrDefault();
            if (value.HasValue)
            {
                if (value.Value < 1)
                    throw new ArgumentOutOfRangeException(nameof(value));
                if (attribute is null || attribute.Length != value.Value || !string.Equals(errorMessage, attribute.ErrorMessage))
                    Add(new MinLengthAttribute(value.Value));
            }
            else if (!(attribute is null))
                Remove(attribute);
        }

        public void EnsureRange(int minimum, int maximum)
        {
            if (!Items.OfType<RangeAttribute>().Any(a => a.Minimum is int min && min == minimum && a.Maximum is int max && max == maximum))
                Add(new RangeAttribute(minimum, maximum));
        }

        public void EnsureRange(int minimum, int maximum, string errorMessage)
        {
            if (!Items.OfType<RangeAttribute>().Any(a => a.Minimum is int min && min == minimum && a.Maximum is int max && max == maximum &&
                    string.Equals(a.ErrorMessage, errorMessage)))
                Add(new RangeAttribute(minimum, maximum));
        }

        public void EnsureRange(double minimum, double maximum)
        {
            if (!Items.OfType<RangeAttribute>().Any(a => a.Minimum is double min && min == minimum && a.Maximum is double max && max == maximum))
                Add(new RangeAttribute(minimum, maximum));
        }

        public void EnsureRange(double minimum, double maximum, string errorMessage)
        {
            if (!Items.OfType<RangeAttribute>().Any(a => a.Minimum is double min && min == minimum && a.Maximum is double max && max == maximum &&
                    string.Equals(a.ErrorMessage, errorMessage)))
                Add(new RangeAttribute(minimum, maximum));
        }

        public void EnsureRange<T>(string minimum, string maximum)
        {
            Type t = typeof(T);
            if (!Items.OfType<RangeAttribute>().Any(a => t.Equals(a.OperandType) && a.Minimum is string min && min == minimum && a.Maximum is string max && max == maximum))
                Add(new RangeAttribute(t, minimum, maximum));
        }

        public void EnsureRange<T>(string minimum, string maximum, string errorMessage)
        {
            Type t = typeof(T);
            if (!Items.OfType<RangeAttribute>().Any(a => t.Equals(a.OperandType) && a.Minimum is string min && min == minimum &&
                    a.Maximum is string max && max == maximum && string.Equals(a.ErrorMessage, errorMessage)))
                Add(new RangeAttribute(t, minimum, maximum));
        }

        public void EnsureStringLength(int maximumLength, int minimumLength = 0)
        {
            if (maximumLength < minimumLength)
                throw new ArgumentOutOfRangeException(nameof(maximumLength));
            if (minimumLength < 0)
                throw new ArgumentOutOfRangeException(nameof(minimumLength));
            if (!Items.OfType<StringLengthAttribute>().Any(a => a.MinimumLength == minimumLength && a.MaximumLength == maximumLength))
                Add(new StringLengthAttribute(maximumLength) { MinimumLength = minimumLength });
        }

        public void EnsureStringLength(int maximumLength, string errorMessage) => EnsureStringLength(maximumLength, 0, errorMessage);

        public void EnsureStringLength(int maximumLength, int minimumLength, string errorMessage)
        {
            if (maximumLength < minimumLength)
                throw new ArgumentOutOfRangeException(nameof(maximumLength));
            if (minimumLength < 0)
                throw new ArgumentOutOfRangeException(nameof(minimumLength));
            if (!Items.OfType<StringLengthAttribute>().Any(a => a.MinimumLength == minimumLength && a.MaximumLength == maximumLength &&
                    string.Equals(a.ErrorMessage, errorMessage)))
                Add(new StringLengthAttribute(maximumLength) { MinimumLength = minimumLength });
        }

        public void EnsureRegularExpression(string pattern, int matchTimeoutInMilliseconds = 0)
        {
            if (string.IsNullOrEmpty(pattern))
                throw new ArgumentOutOfRangeException(nameof(pattern));
            if (matchTimeoutInMilliseconds < 0)
                throw new ArgumentOutOfRangeException(nameof(matchTimeoutInMilliseconds));
            if (!Items.OfType<RegularExpressionAttribute>().Any(a => pattern.Equals(a.Pattern) && a.MatchTimeoutInMilliseconds == matchTimeoutInMilliseconds))
                Add(new RegularExpressionAttribute(pattern) { MatchTimeoutInMilliseconds = matchTimeoutInMilliseconds });
        }

        public void EnsureRegularExpression(string pattern, string errorMessage) => EnsureRegularExpression(pattern, 0, errorMessage);

        public void EnsureRegularExpression(string pattern, int matchTimeoutInMilliseconds, string errorMessage)
        {
            if (string.IsNullOrEmpty(pattern))
                throw new ArgumentOutOfRangeException(nameof(pattern));
            if (matchTimeoutInMilliseconds < 0)
                throw new ArgumentOutOfRangeException(nameof(matchTimeoutInMilliseconds));
            if (!Items.OfType<RegularExpressionAttribute>().Any(a => pattern.Equals(a.Pattern) &&
                    a.MatchTimeoutInMilliseconds == matchTimeoutInMilliseconds && string.Equals(a.ErrorMessage, errorMessage)))
                Add(new RegularExpressionAttribute(pattern) { MatchTimeoutInMilliseconds = matchTimeoutInMilliseconds });
        }

        public void EnsureCompare(string otherProperty)
        {
            if (string.IsNullOrEmpty(otherProperty))
                throw new ArgumentOutOfRangeException(nameof(otherProperty));
            if (!Items.OfType<CompareAttribute>().Any(a => otherProperty.Equals(a.OtherProperty)))
                Add(new CompareAttribute(otherProperty));
        }

        public void EnsureCompare(string otherProperty, string errorMessage)
        {
            if (string.IsNullOrEmpty(otherProperty))
                throw new ArgumentOutOfRangeException(nameof(otherProperty));
            if (!Items.OfType<CompareAttribute>().Any(a => otherProperty.Equals(a.OtherProperty) && string.Equals(a.ErrorMessage, errorMessage)))
                Add(new CompareAttribute(otherProperty));
        }

        public void SetRequired(bool isRequired, bool allowEmptyStrings = false)
        {
            RequiredAttribute attribute = Items.OfType<RequiredAttribute>().FirstOrDefault();
            if (attribute is null)
            {
                if (!isRequired)
                    return;
            }
            else if (isRequired)
            {
                if (attribute.AllowEmptyStrings == allowEmptyStrings)
                    return;
            }
            else
            {
                Remove(attribute);
                return;
            }
            Add(new RequiredAttribute() { AllowEmptyStrings = allowEmptyStrings });
        }

        public void SetRequired(bool isRequired, string errorMessage) => SetRequired(isRequired, false, errorMessage);

        public void SetRequired(bool isRequired, bool allowEmptyStrings, string errorMessage)
        {
            RequiredAttribute attribute = Items.OfType<RequiredAttribute>().FirstOrDefault();
            if (attribute is null)
            {
                if (!isRequired)
                    return;
            }
            else if (isRequired)
            {
                if (attribute.AllowEmptyStrings == allowEmptyStrings && string.Equals(attribute.ErrorMessage, errorMessage))
                    return;
            }
            else
            {
                Remove(attribute);
                return;
            }
            Add(new RequiredAttribute() { AllowEmptyStrings = allowEmptyStrings });
        }

        private int Find(ValidationAttribute item)
        {
            Monitor.Enter(_syncRoot);
            try
            {
                AttributeComparer<ValidationAttribute> comparer = AttributeComparer<ValidationAttribute>.Instance;
                int index = -1;
                foreach (ValidationAttribute attribute in Items)
                {
                    index++;
                    if (comparer.Equals(item, attribute))
                        return index;
                }
            }
            finally { Monitor.Exit(_syncRoot); }
            return -1;
        }

        protected override void InsertItem(int index, ValidationAttribute item)
        {
            if (item is null)
                throw new ArgumentNullException();
            Monitor.Enter(_syncRoot);
            try
            {
                int oldIndex = Find(item);
                if (oldIndex < 0)
                    base.InsertItem(index, item);
                else
                {
                    base.InsertItem(index, item);
                    base.RemoveItem((oldIndex < index) ? oldIndex : oldIndex + 1);
                }
            }
            finally { Monitor.Exit(_syncRoot); }
        }

        protected override void SetItem(int index, ValidationAttribute item)
        {
            if (item is null)
                throw new ArgumentNullException();
            Monitor.Enter(_syncRoot);
            try
            {
                int oldIndex = IndexOf(item);
                base.SetItem(index, item);
                if (oldIndex >= 0 && index != oldIndex)
                    base.RemoveItem(oldIndex);
            }
            finally { Monitor.Exit(_syncRoot); }
        }

        protected override void ClearItems()
        {
            Monitor.Enter(_syncRoot);
            try { base.ClearItems(); }
            finally { Monitor.Exit(_syncRoot); }
        }

        protected override void RemoveItem(int index)
        {
            Monitor.Enter(_syncRoot);
            try { base.RemoveItem(index); }
            finally { Monitor.Exit(_syncRoot); }
        }
    }
}
