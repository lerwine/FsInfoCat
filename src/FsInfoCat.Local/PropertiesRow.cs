using System;

namespace FsInfoCat.Local
{
    public class PropertiesRow : LocalDbEntity, IHasSimpleIdentifier
    {
        private readonly IPropertyChangeTracker<Guid> _id;

        public Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        public PropertiesRow()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
        }
    }
}
