using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class DRMPropertiesListItem : DRMPropertiesRow, ILocalDRMPropertiesListItem, IEquatable<DRMPropertiesListItem>
    {
        public const string VIEW_NAME = "vDRMPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<DRMPropertiesListItem> builder) => builder.ToView(VIEW_NAME).HasKey(nameof(Id));

        protected bool ArePropertiesEqual([DisallowNull] ILocalDRMPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IDRMPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(DRMPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IDRMPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IDRMProperties other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            throw new NotImplementedException();
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            throw new NotImplementedException();
        }
    }
}
