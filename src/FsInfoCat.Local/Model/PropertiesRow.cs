using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading;

namespace FsInfoCat.Local.Model
{
    /// <summary>
    /// Base class for entities containing extended file properties.
    /// </summary>
    /// <seealso cref="AudioPropertiesRow" />
    /// <seealso cref="DRMPropertiesRow" />
    /// <seealso cref="DbFileRow" />
    /// <seealso cref="DocumentPropertiesRow" />
    /// <seealso cref="GPSPropertiesRow" />
    /// <seealso cref="ImagePropertiesRow" />
    /// <seealso cref="MediaPropertiesRow" />
    /// <seealso cref="MusicPropertiesRow" />
    /// <seealso cref="PhotoPropertiesRow" />
    /// <seealso cref="RecordedTVPropertiesRow" />
    /// <seealso cref="SummaryPropertiesRow" />
    /// <seealso cref="VideoPropertiesRow" />
    /// <seealso cref="LocalDbEntity" />
    public abstract class PropertiesRow : LocalDbEntity, ILocalPropertiesRow
    {
        private Guid? _id;

        /// <summary>
        /// Gets the primary key value.
        /// </summary>
        /// <value>The <see cref="Guid">unique identifier</see> used as the current entity's primary key the database.</value>
        [Key]
        [BackingField(nameof(_id))]
        [Display(Name = nameof(FsInfoCat.Properties.Resources.UniqueIdentifier), ResourceType = typeof(FsInfoCat.Properties.Resources))]
        public virtual Guid Id
        {
            get => _id ?? Guid.Empty;
            set
            {
                Monitor.Enter(SyncRoot);
                try
                {
                    if (_id.HasValue)
                    {
                        if (!_id.Value.Equals(value))
                            throw new InvalidOperationException();
                    }
                    else if (value.Equals(Guid.Empty))
                        return;
                    _id = value;
                }
                finally { Monitor.Exit(SyncRoot); }
            }
        }

        /// <summary>
        /// Gets the unique identifier of the current entity if it has been assigned.
        /// </summary>
        /// <param name="result">Receives the unique identifier value.</param>
        /// <returns><see langword="true" /> if the <see cref="Id" /> property has been set; otherwise, <see langword="false" />.</returns>
        public bool TryGetId(out Guid result)
        {
            Guid? id = _id;
            if (id.HasValue)
            {
                result = id.Value;
                return true;
            }
            result = Guid.Empty;
            return false;
        }
    }
}
