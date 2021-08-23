namespace FsInfoCat.Local
{
    public class RedundantSetListItem : RedundantSetRow, ILocalRedundantSetListItem
    {
        private readonly IPropertyChangeTracker<long> _length;
        private readonly IPropertyChangeTracker<MD5Hash?> _hash;
        private readonly IPropertyChangeTracker<long> _redundancyCount;

        public long Length { get => _length.GetValue(); set => _length.SetValue(value); }

        public MD5Hash? Hash { get => _hash.GetValue(); set => _hash.SetValue(value); }

        public long RedundancyCount { get => _redundancyCount.GetValue(); set => _redundancyCount.SetValue(value); }

        public RedundantSetListItem()
        {
            _length = AddChangeTracker(nameof(Length), 0L);
            _hash = AddChangeTracker<MD5Hash?>(nameof(Hash), null);
            _redundancyCount = AddChangeTracker(nameof(RedundancyCount), 0L);
        }
    }
}
