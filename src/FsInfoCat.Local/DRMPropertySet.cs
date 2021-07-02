using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Local
{
    public class DRMPropertySet : LocalDbEntity, ILocalDRMPropertySet
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

        public DRMPropertySet()
        {
            _id = AddChangeTracker(nameof(Id), Guid.Empty);
            _datePlayExpires = AddChangeTracker<System.DateTime?>(nameof(DatePlayExpires), null);
            _datePlayStarts = AddChangeTracker<System.DateTime?>(nameof(DatePlayStarts), null);
            _description = AddChangeTracker(nameof(Description), null, FilePropertiesComparer.StringValueCoersion);
            _isProtected = AddChangeTracker<bool?>(nameof(IsProtected), null);
            _playCount = AddChangeTracker<uint?>(nameof(PlayCount), null);
        }

        internal static async Task RefreshAsync(EntityEntry<DbFile> entry, IFileDetailProvider fileDetailProvider, CancellationToken cancellationToken)
        {
            DRMPropertySet oldDRMPropertySet = entry.Entity.DRMPropertySetId.HasValue ? await entry.GetRelatedReferenceAsync(f => f.DRMProperties, cancellationToken) : null;
            IDRMProperties currentDRMProperties = await fileDetailProvider.GetDRMPropertiesAsync(cancellationToken);
            // TODO: Implement RefreshAsync(EntityEntry<DbFile>, IFileDetailProvider, CancellationToken)
            throw new NotImplementedException();
        }
    }
}
