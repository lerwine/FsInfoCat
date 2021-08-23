namespace FsInfoCat.Local
{
    public class FileWithBinaryPropertiesAndAncestorNames : FileWithBinaryProperties, ILocalFileListItemWithBinaryPropertiesAndAncestorNames
    {
        private readonly IPropertyChangeTracker<string> _ancestorNames;

        public string AncestorNames { get => _ancestorNames.GetValue(); set => _ancestorNames.SetValue(value); }

        public FileWithBinaryPropertiesAndAncestorNames()
        {
            _ancestorNames = AddChangeTracker(nameof(AncestorNames), "", NonNullStringCoersion.Default);
        }
    }
}
