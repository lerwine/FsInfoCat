using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class VideoPropertiesListItem : VideoPropertiesRow, ILocalVideoPropertiesListItem, IEquatable<VideoPropertiesListItem>
    {
        public const string VIEW_NAME = "vVideoPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<VideoPropertiesListItem> builder)
        {
            _ = builder.ToView(VIEW_NAME).HasKey(nameof(Id));
            _ = builder.Property(nameof(Director)).HasConversion(MultiStringValue.Converter);
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalVideoPropertiesListItem other) => ArePropertiesEqual((IVideoPropertySet)other) &&
            EqualityComparer<Guid?>.Default.Equals(UpstreamId, other.UpstreamId) &&
            LastSynchronizedOn == other.LastSynchronizedOn;

        public bool Equals(VideoPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IVideoPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IVideoPropertiesRow other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IVideoProperties other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(object obj)
        {
            if (obj is null) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj is VideoPropertiesListItem videoPropertiesListItem) return Equals(videoPropertiesListItem);
            if (obj is ILocalVideoPropertiesListItem localVideoPropertiesListItem)
            {
                if (Id.Equals(Guid.Empty)) return localVideoPropertiesListItem.Id.Equals(Guid.Empty) && ArePropertiesEqual(localVideoPropertiesListItem);
                return Id.Equals(localVideoPropertiesListItem.Id);
            }
            if (obj is IVideoPropertySet iVideoPropertySet)
            {
                if (Id.Equals(Guid.Empty)) return iVideoPropertySet.Id.Equals(Guid.Empty) && ArePropertiesEqual(iVideoPropertySet);
                return Id.Equals(iVideoPropertySet.Id);
            }
            if (obj is ILocalVideoPropertySet localVideoPropertySet)
            {
                if (Id.Equals(Guid.Empty)) return localVideoPropertySet.Id.Equals(Guid.Empty) && ArePropertiesEqual(localVideoPropertySet);
                return Id.Equals(localVideoPropertySet.Id);
            }
            return obj is IVideoProperties videoProperties && ArePropertiesEqual(videoProperties);
        }
    }
}
