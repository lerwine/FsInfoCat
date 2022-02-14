using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
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

        protected bool ArePropertiesEqual([DisallowNull] ILocalVideoPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IVideoPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(VideoPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IVideoPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IVideoProperties other)
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
