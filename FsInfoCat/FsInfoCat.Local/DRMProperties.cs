using System;
using System.Collections.Generic;
using System.Linq;

namespace FsInfoCat.Local
{
    public class DRMProperties : LocalDbEntity, ILocalDRMProperties
    {
        #region Fields

        private readonly IPropertyChangeTracker<Guid> _id;
        private readonly IPropertyChangeTracker<DateTime?> _datePlayExpires;
        private readonly IPropertyChangeTracker<DateTime?> _datePlayStarts;
        private readonly IPropertyChangeTracker<string> _description;
        private readonly IPropertyChangeTracker<bool?> _isProtected;
        private readonly IPropertyChangeTracker<uint?> _playCount;
        private HashSet<DbFile> _files = new();

        #endregion

        #region Properties

        public Guid Id { get => _id.GetValue(); set => _id.SetValue(value); }

        public DateTime? DatePlayExpires { get => _datePlayExpires.GetValue(); set => _datePlayExpires.SetValue(value); }
        public DateTime? DatePlayStarts { get => _datePlayStarts.GetValue(); set => _datePlayStarts.SetValue(value); }
        public string Description { get => _description.GetValue(); set => _description.SetValue(value); }
        public bool? IsProtected { get => _isProtected.GetValue(); set => _isProtected.SetValue(value); }
        public uint? PlayCount { get => _playCount.GetValue(); set => _playCount.SetValue(value); }

        public HashSet<DbFile> Files
        {
            get => _files;
            set => CheckHashSetChanged(_files, value, h => _files = h);
        }

        #endregion

        #region Explicit Members

        IEnumerable<ILocalFile> ILocalPropertySet.Files => Files.Cast<ILocalFile>();

        IEnumerable<IFile> IPropertySet.Files => Files.Cast<IFile>();

        #endregion

        public DRMProperties()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _datePlayExpires = AddChangeTracker<System.DateTime?>(nameof(DatePlayExpires), null);
            _datePlayStarts = AddChangeTracker<System.DateTime?>(nameof(DatePlayStarts), null);
            _description = AddChangeTracker<string>(nameof(Description), null);
            _isProtected = AddChangeTracker<bool?>(nameof(IsProtected), null);
            _playCount = AddChangeTracker<uint?>(nameof(PlayCount), null);
        }
    }
}
