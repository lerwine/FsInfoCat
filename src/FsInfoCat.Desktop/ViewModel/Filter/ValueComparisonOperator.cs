using System;

namespace FsInfoCat.Desktop.ViewModel.Filter
{
    [Flags]
    public enum ValueComparisonOperator : ushort
    {
        /// <summary>
        /// Value is equal to another value (bits: 1000 0001).
        /// </summary>
        EqualTo = Filter.ComparisonValue_NotNullAndEqualTo,

        /// <summary>
        /// Value is not equal to another value (bits: 0000 0000).
        /// </summary>
        NotEqualTo = Filter.ComparisonValue_NullOrNotEqualTo,

        /// <summary>
        /// Value is less than another value (bits: 1000 0010).
        /// </summary>
        LessThan = Filter.ComparisonValue_NotNullAndLessThan,

        /// <summary>
        /// Value is greater than another value (bits: 1000 0100).
        /// </summary>
        GreaterThan = Filter.ComparisonValue_NotNullAndGreaterThan,

        /// <summary>
        /// Value is less than or equal another value (bits: 1000 0011).
        /// </summary>
        LessThanOrEqualTo = Filter.ComparisonValue_NotNullAndLessThanOrEqualTo,

        /// <summary>
        /// Value is greater than or equal to another value (bits: 1000 0101).
        /// </summary>
        GreaterThanOrEqualTo = Filter.ComparisonValue_NotNullAndGreaterThanOrEqualTo
    }
}
