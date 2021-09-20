using System;

namespace FsInfoCat.Desktop.ViewModel.Filter
{
    [Flags]
    public enum ObjectComparisonOperator : ushort
    {
        /// <summary>
        /// Object is not null and is equal to another object (bits: 1000 0001).
        /// </summary>
        EqualTo = Filter.ComparisonValue_NotNullAndEqualTo,

        /// <summary>
        /// Object is null or not equal to another object (bits: 0000 0000).
        /// </summary>
        NotEqualTo = Filter.ComparisonValue_NullOrNotEqualTo,

        /// <summary>
        /// Object is not null and is less than another object (bits: 1000 0010).
        /// </summary>
        LessThan = Filter.ComparisonValue_NotNullAndLessThan,

        /// <summary>
        /// Object is not null and is greater than another object (bits: 1000 0100).
        /// </summary>
        GreaterThan = Filter.ComparisonValue_NotNullAndGreaterThan,

        /// <summary>
        /// Object is not null and is less than or equal another object (bits: 1000 0011).
        /// </summary>
        LessThanOrEqualTo = Filter.ComparisonValue_NotNullAndLessThanOrEqualTo,

        /// <summary>
        /// Object is not null and is greater than or equal to another object (bits: 1000 0101).
        /// </summary>
        GreaterThanOrEqualTo = Filter.ComparisonValue_NotNullAndGreaterThanOrEqualTo,

        /// <summary>
        /// Object is not null and not equal to another object (bits: 1000 0000).
        /// </summary>
        NotNullOrEqualTo = Filter.ComparisonValue_NotNullOrEqualTo,

        /// <summary>
        /// Object is null or equal to another object (bits: 0000 0001).
        /// </summary>
        NullOrEqualTo = Filter.ComparisonValue_NullOrEqualTo,

        /// <summary>
        /// Object is null or less than another object (bits: 0000 0010).
        /// </summary>
        NullOrLessThan = Filter.ComparisonValue_NullOrLessThan,

        /// <summary>
        /// Object is null or greater than another object (bits: 0000 0100).
        /// </summary>
        NullOrGreaterThan = Filter.ComparisonValue_NullOrGreaterThan,

        /// <summary>
        /// Object is null, less than or equal to another object (bits: 0000 0011).
        /// </summary>
        NullLessThanOrEqualTo = Filter.ComparisonValue_NullLessThanOrEqualTo,

        /// <summary>
        /// Object is null, greater than or equal to another object (bits: 0000 0101).
        /// </summary>
        NullGreaterThanOrEqualTo = Filter.ComparisonValue_NullGreaterThanOrEqualTo
    }
}
