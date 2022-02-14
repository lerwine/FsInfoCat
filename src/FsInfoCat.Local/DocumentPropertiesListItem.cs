using FsInfoCat.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public class DocumentPropertiesListItem : DocumentPropertiesRow, ILocalDocumentPropertiesListItem, IEquatable<DocumentPropertiesListItem>
    {
        public const string VIEW_NAME = "vDocumentPropertiesListing";

        public long ExistingFileCount { get; set; }

        public long TotalFileCount { get; set; }

        internal static void OnBuildEntity(EntityTypeBuilder<DocumentPropertiesListItem> builder)
        {
            _ = (builder ?? throw new ArgumentOutOfRangeException(nameof(builder))).ToView(VIEW_NAME).Property(nameof(Contributor))
                .HasConversion(MultiStringValue.Converter);
        }

        protected bool ArePropertiesEqual([DisallowNull] ILocalDocumentPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        protected bool ArePropertiesEqual([DisallowNull] IDocumentPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(DocumentPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public bool Equals(IDocumentPropertiesListItem other)
        {
            throw new NotImplementedException();
        }

        public override bool Equals(IDocumentProperties other)
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
