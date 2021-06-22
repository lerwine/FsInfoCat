using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.Test.ComponentSupport
{
    public class TestGenericEquatableComponent : TestComponent
    {
        private int _genericEqualsInvocationCount = 0;
        public TestGenericEquatableComponent(int numerator, int denominator) : base(numerator, denominator) { }
        public override bool Equals(object obj)
        {
            _genericEqualsInvocationCount++;
            if (obj == null || obj is TestComponent)
                return BaseEquals((TestComponent)obj);
            if (obj is double)
                return Value.Equals((double)obj);
            TypeConverter typeConverter = TypeDescriptor.GetConverter(obj);
            if (typeConverter.CanConvertTo(typeof(double)))
                return Value.Equals((double)typeConverter.ConvertTo(obj, typeof(double)));
            return false;
        }
        public override int GetGenericEqualsInvocationCount()
        {
            return _genericEqualsInvocationCount;
        }
        public override int GetTypedEqualsInvocationCount()
        {
            return 0;
        }
        public override int GetHashCode() { return BaseGetHashCode(); }
        public override string ToString() { return BaseToString(); }
    }

    public class TestBaseEquatableComponent : TestComponent, IEquatable<TestComponent>
    {
        public TestBaseEquatableComponent(int numerator, int denominator) : base(numerator, denominator) { }
        public bool Equals(TestComponent other) { return BaseEquals(other); }
        public override int GetHashCode() { return BaseGetHashCode(); }
        public override string ToString() { return BaseToString(); }
    }

    public class TestEquatableComponent : TestComponent, IEquatable<TestEquatableComponent>
    {
        public TestEquatableComponent(int numerator, int denominator) : base(numerator, denominator) { }
        public bool Equals(TestEquatableComponent other) { return BaseEquals(other); }
        public override int GetHashCode() { return BaseGetHashCode(); }
        public override string ToString() { return BaseToString(); }
    }

    public class TestGenericComparableComponent : TestComponent, IComparable
    {
        private int _comparisonInvocationCount = 0;
        public TestGenericComparableComponent(int numerator, int denominator) : base(numerator, denominator) { }

        public int CompareTo(object obj)
        {
            _comparisonInvocationCount++;
            if (obj == null || obj is TestComponent)
                return BaseCompareTo((TestComponent)obj);
            if (obj is double)
                return Value.CompareTo((double)obj);
            TypeConverter typeConverter = TypeDescriptor.GetConverter(obj);
            if (typeConverter.CanConvertTo(typeof(double)))
                return Value.CompareTo((double)typeConverter.ConvertTo(obj, typeof(double)));
            return -1;
        }

        public override int GetComparisonInvocationCount()
        {
            return _comparisonInvocationCount;
        }
    }

    public class TestBaseComparableComponent : TestComponent, IComparable<TestComponent>
    {
        public TestBaseComparableComponent(int numerator, int denominator) : base(numerator, denominator) { }

        public int CompareTo(TestComponent other) { return BaseCompareTo(other); }
    }

    public class TestComparableComponent : TestComponent, IComparable<TestComparableComponent>
    {
        public TestComparableComponent(int numerator, int denominator) : base(numerator, denominator) { }

        public int CompareTo(TestComparableComponent other) { return BaseCompareTo(other); }
        public override int GetHashCode() { return BaseGetHashCode(); }
        public override string ToString() { return BaseToString(); }
    }

    public class TestValidator
    {
        public static bool IsValidNumerator(int numerator, ValidationContext context, out ValidationResult result)
        {
            if (numerator < 0)
            {
                result = new ValidationResult("Numerator cannot be negative");
                return false;
            }
            result = null;
            return true;
        }

        public static bool IsFraction(int numerator, ValidationContext context, out ValidationResult result)
        {
            if (context.ObjectInstance is TestComponent testComponent && testComponent.Denominator < numerator)
            {
                result = new ValidationResult("Numerator cannot be greater than the Denominator");
                return false;
            }
            result = null;
            return true;
        }

        public static bool IsValidDenominator(int denominator, ValidationContext context, out ValidationResult result)
        {
            if (denominator == 0)
            {
                if (!(context.ObjectInstance is TestComponent testComponent && testComponent.Numerator == 0))
                {
                    result = new ValidationResult("Denominator cannot be zero");
                    return false;
                }
            }
            else if (denominator < 0)
            {
                result = new ValidationResult("Denominator cannot be negative");
                return false;
            }
            result = null;
            return true;
        }
    }

    public class TestComponent
    {
        public const string DISPLAY_NAME_VALUE = "Dividend";
        public const string DESCRIPTION_DENOMINATOR = "Fractional Denominator";
        private int _numeratorValue;
        private int _numeratorAccessorCount = 0;
        private int _numeratorMutatorCount = 0;
        private int _denominatorValue;
        private int _denominatorAccessorCount = 0;
        private int _denominatorMutatorCount = 0;
        private int _valueAccessorCount = 0;
        private int _hashCodeInvocationCount = 0;
        private int _stringInvocationCount = 0;
        private int _comparisonInvocationCount = 0;
        private int _typedEqualsInvocationCount = 0;
        private int _genericEqualsInvocationCount = 0;

        public int GetNumeratorAccessCount() { return _numeratorAccessorCount; }

        public int GetNumeratorMutateCount() { return _numeratorMutatorCount; }

        public int GetDenominatorAccessCount() { return _denominatorAccessorCount; }

        public int GetDenominatorMutateCount() { return _denominatorMutatorCount; }

        public int GetValueAccessCount() { return _valueAccessorCount; }

        public virtual int GetHashCodeInvocationCount() { return _hashCodeInvocationCount; }

        public virtual int GetStringInvocationCount() { return _stringInvocationCount; }

        public virtual int GetComparisonInvocationCount() { return _comparisonInvocationCount; }

        public virtual int GetTypedEqualsInvocationCount() { return _typedEqualsInvocationCount; }

        public virtual int GetGenericEqualsInvocationCount() { return _genericEqualsInvocationCount; }

        protected int BaseCompareTo(TestComponent other)
        {
            _comparisonInvocationCount++;
            if (other == null)
                return 1;
            if (ReferenceEquals(this, other))
                return 0;
            if (Denominator == other.Denominator)
                return Numerator.CompareTo(other.Numerator);
            return Value.CompareTo(other.Value);
        }

        protected bool BaseEquals(TestComponent other)
        {
            _typedEqualsInvocationCount++;
            if (other == null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (Denominator == other.Denominator)
                return Numerator.Equals(other.Numerator);
            return Value.Equals(other.Value);
        }

        public override bool Equals(object obj)
        {
            _genericEqualsInvocationCount++;
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            _hashCodeInvocationCount++;
            return base.GetHashCode();
        }

        public override string ToString()
        {
            _stringInvocationCount++;
            return base.ToString();
        }

        [Category("Behavior")]
        public int Numerator
        {
            get
            {
                _numeratorAccessorCount++;
                return _numeratorValue;
            }
            set
            {
                _numeratorMutatorCount++;
                _numeratorValue = value;
            }
        }

        [ReadOnly(true)]
        [Description(DESCRIPTION_DENOMINATOR)]
        [CustomValidation(typeof(TestValidator), nameof(TestValidator.IsValidDenominator))]
        public int Denominator
        {
            get
            {
                _denominatorAccessorCount++;
                return _denominatorValue;
            }
            set
            {
                _denominatorMutatorCount++;
                _denominatorValue = value;
            }
        }

        [DisplayName(DISPLAY_NAME_VALUE)]
        public double Value
        {
            get
            {
                _valueAccessorCount++;
                return System.Convert.ToDouble(_numeratorValue) / System.Convert.ToDouble(_denominatorValue);
            }
        }

        public TestComponent(int numerator, int denominator)
        {
            _numeratorValue = numerator;
            _denominatorValue = denominator;
        }

        protected int BaseGetHashCode()
        {
            _hashCodeInvocationCount++;
            return (((long)Denominator * (long)Numerator) + (long)Numerator).GetHashCode();
        }

        protected string BaseToString()
        {
            _stringInvocationCount++;
            return base.ToString();
        }
    }
}
