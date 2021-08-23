namespace FsInfoCat.Local
{
    public class SubdirectoryListItemWithAncestorNames : SubdirectoryListItem, ILocalSubdirectoryListItemWithAncestorNames
    {
        private readonly IPropertyChangeTracker<string> _ancestorNames;

        public string AncestorNames { get => _ancestorNames.GetValue(); set => _ancestorNames.SetValue(value); }

        public SubdirectoryListItemWithAncestorNames()
        {
            _ancestorNames = AddChangeTracker(nameof(AncestorNames), "", NonNullStringCoersion.Default);
        }
    }
}
