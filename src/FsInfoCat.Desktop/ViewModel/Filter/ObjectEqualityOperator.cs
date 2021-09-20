using System;

namespace FsInfoCat.Desktop.ViewModel.Filter
{
    [Flags]
    public enum ObjectEqualityOperator : ushort
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
        /// Object is not null and not equal to another object (bits: 1000 0000).
        /// </summary>
        NotNullOrEqualTo = Filter.ComparisonValue_NotNullOrEqualTo,

        /// <summary>
        /// Object is null or equal to another object (bits: 0000 0001).
        /// </summary>
        NullOrEqualTo = Filter.ComparisonValue_NullOrEqualTo
    }
}
