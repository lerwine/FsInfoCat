namespace FsInfoCat.Desktop.ViewModel.Filter
{
    public enum ValueEqualityOperator : ushort
    {
        /// <summary>
        /// Value is equal to another object (bits: 1000 0001).
        /// </summary>
        EqualTo = Filter.ComparisonValue_NotNullAndEqualTo,

        /// <summary>
        /// Value is not equal to another value (bits: 0000 0000).
        /// </summary>
        NotEqualTo = Filter.ComparisonValue_NullOrNotEqualTo
    }
}
