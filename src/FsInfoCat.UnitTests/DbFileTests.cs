using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class DbFileTests
    {
#pragma warning disable IDE0052 // Remove unread private members
        private static TestContext _testContext;
#pragma warning restore IDE0052 // Remove unread private members

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod("new DbFile()"), Ignore]
        public void NewDbFileTestMethod()
        {
            using IServiceScope serviceScope = Hosting.ServiceProvider.CreateScope();
            using LocalDbContext dbContext = serviceScope.ServiceProvider.GetService<LocalDbContext>();
            DbFile target = new();

            EntityEntry<DbFile> entry = dbContext.Entry(target);
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.AreEqual(EntityState.Detached, entry.State);
            Assert.IsNotNull(target.Name);
            Assert.AreEqual("", target.Name);
            Assert.AreEqual(FileCrawlOptions.None, target.Options);
            Assert.AreEqual(FileCorrelationStatus.Dissociated, target.Status);
            Assert.IsNull(target.LastHashCalculation);
            Assert.IsNotNull(target.Notes);
            Assert.AreEqual("", target.Notes);
            Assert.AreEqual(Guid.Empty, target.ParentId);
            Assert.AreEqual(Guid.Empty, target.BinaryPropertySetId);
            Assert.IsNull(target.SummaryPropertySetId);
            Assert.IsNull(target.DocumentPropertySetId);
            Assert.IsNull(target.AudioPropertySetId);
            Assert.IsNull(target.DRMPropertySetId);
            Assert.IsNull(target.GPSPropertySetId);
            Assert.IsNull(target.ImagePropertySetId);
            Assert.IsNull(target.MediaPropertySetId);
            Assert.IsNull(target.MusicPropertySetId);
            Assert.IsNull(target.PhotoPropertySetId);
            Assert.IsNull(target.RecordedTVPropertySetId);
            Assert.IsNull(target.VideoPropertySetId);
            Assert.IsNull(target.BinaryProperties);
            Assert.IsNull(target.Parent);
            Assert.IsNull(target.Redundancy);
            Assert.IsNull(target.SummaryProperties);
            Assert.IsNull(target.DocumentProperties);
            Assert.IsNull(target.AudioProperties);
            Assert.IsNull(target.DRMProperties);
            Assert.IsNull(target.GPSProperties);
            Assert.IsNull(target.ImageProperties);
            Assert.IsNull(target.MediaProperties);
            Assert.IsNull(target.MusicProperties);
            Assert.IsNull(target.PhotoProperties);
            Assert.IsNull(target.RecordedTVProperties);
            Assert.IsNull(target.VideoProperties);
            Assert.IsNotNull(target.AccessErrors);
            Assert.AreEqual(0, target.AccessErrors.Count);
            Assert.IsNotNull(target.BaselineComparisons);
            Assert.AreEqual(0, target.BaselineComparisons.Count);
            Assert.IsNotNull(target.CorrelativeComparisons);
            Assert.AreEqual(0, target.CorrelativeComparisons.Count);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
            Assert.AreEqual(target.CreatedOn, target.LastAccessed);

            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for new DbFile()

            dbContext.Files.Add(target);
            Assert.AreNotEqual(Guid.Empty, target.Id);
            Assert.AreEqual(EntityState.Added, entry.State);
            Assert.IsNotNull(target.Name);
            Assert.AreEqual("", target.Name);
            Assert.AreEqual(FileCrawlOptions.None, target.Options);
            Assert.AreEqual(FileCorrelationStatus.Dissociated, target.Status);
            Assert.IsNull(target.LastHashCalculation);
            Assert.IsNotNull(target.Notes);
            Assert.AreEqual("", target.Notes);
            Assert.AreEqual(Guid.Empty, target.ParentId);
            Assert.AreEqual(Guid.Empty, target.BinaryPropertySetId);
            Assert.IsNull(target.SummaryPropertySetId);
            Assert.IsNull(target.DocumentPropertySetId);
            Assert.IsNull(target.AudioPropertySetId);
            Assert.IsNull(target.DRMPropertySetId);
            Assert.IsNull(target.GPSPropertySetId);
            Assert.IsNull(target.ImagePropertySetId);
            Assert.IsNull(target.MediaPropertySetId);
            Assert.IsNull(target.MusicPropertySetId);
            Assert.IsNull(target.PhotoPropertySetId);
            Assert.IsNull(target.RecordedTVPropertySetId);
            Assert.IsNull(target.VideoPropertySetId);
            Assert.IsNull(target.BinaryProperties);
            Assert.IsNull(target.Parent);
            Assert.IsNull(target.Redundancy);
            Assert.IsNull(target.SummaryProperties);
            Assert.IsNull(target.DocumentProperties);
            Assert.IsNull(target.AudioProperties);
            Assert.IsNull(target.DRMProperties);
            Assert.IsNull(target.GPSProperties);
            Assert.IsNull(target.ImageProperties);
            Assert.IsNull(target.MediaProperties);
            Assert.IsNull(target.MusicProperties);
            Assert.IsNull(target.PhotoProperties);
            Assert.IsNull(target.RecordedTVProperties);
            Assert.IsNull(target.VideoProperties);
            Assert.IsNotNull(target.AccessErrors);
            Assert.AreEqual(0, target.AccessErrors.Count);
            Assert.IsNotNull(target.BaselineComparisons);
            Assert.AreEqual(0, target.BaselineComparisons.Count);
            Assert.IsNotNull(target.CorrelativeComparisons);
            Assert.AreEqual(0, target.CorrelativeComparisons.Count);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
            Assert.AreEqual(target.CreatedOn, target.LastAccessed);
        }

        [TestMethod("Guid Id"), Ignore]
        public void IdTestMethod()
        {
            DbFile target = new();
            Guid expectedValue = Guid.NewGuid();
            target.Id = expectedValue;
            Guid actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
            target.Id = expectedValue;
            actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
            Assert.ThrowsException<InvalidOperationException>(() => target.Id = Guid.NewGuid());
        }

        [TestMethod("string Name"), Ignore]
        public void NameTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for string Name

            DbFile target = default; // TODO: Create and initialize DbFile instance
            string expectedValue = default;
            target.Name = default;
            string actualValue = target.Name;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("FileCrawlOptions Options"), Ignore]
        public void OptionsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for FileCrawlOptions Options

            DbFile target = default; // TODO: Create and initialize DbFile instance
            FileCrawlOptions expectedValue = default;
            target.Options = default;
            FileCrawlOptions actualValue = target.Options;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime LastAccessed"), Ignore]
        public void LastAccessedTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for DateTime LastAccessed

            DbFile target = default; // TODO: Create and initialize DbFile instance
            DateTime expectedValue = default;
            target.LastAccessed = default;
            DateTime actualValue = target.LastAccessed;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime? LastHashCalculation"), Ignore]
        public void LastHashCalculationTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for DateTime? LastHashCalculation

            DbFile target = default; // TODO: Create and initialize DbFile instance
            DateTime? expectedValue = default;
            target.LastHashCalculation = default;
            DateTime? actualValue = target.LastHashCalculation;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [DataTestMethod]
        [DataRow(null, "", DisplayName = "string Notes = null")]
        [DataRow("", "", DisplayName = "string Notes = \"\"")]
        [DataRow("\n\r", "", DisplayName = "string Notes = \"\\n\\r\"")]
        [DataRow("Test", "Test", DisplayName = "string Notes = \"Test\"")]
        [DataRow("\n Test \r", "\n Test \r", DisplayName = "string Notes = \"\\n Test \\r\"")]
        public void NotesTestMethod(string notes, string expected)
        {
            DbFile target = new();
            target.Notes = notes;
            string actualValue = target.Notes;
            Assert.IsNotNull(actualValue);
            Assert.AreEqual(expected, actualValue);
        }

        [TestMethod("FileCorrelationStatus Status"), Ignore]
        public void StatusTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for FileCorrelationStatus Status

            DbFile target = default; // TODO: Create and initialize DbFile instance
            FileCorrelationStatus expectedValue = default;
            target.Status = default;
            FileCorrelationStatus actualValue = target.Status;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid ParentId"), Ignore]
        public void ParentIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid ParentId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid expectedValue = default;
            target.ParentId = default;
            Guid actualValue = target.ParentId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid BinaryPropertySetId"), Ignore]
        public void BinaryPropertySetIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid BinaryPropertySetId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid expectedValue = default;
            target.BinaryPropertySetId = default;
            Guid actualValue = target.BinaryPropertySetId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? SummaryPropertySetId"), Ignore]
        public void SummaryPropertySetIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid? BinaryPropertySetId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.BinaryPropertySetId = default;
            Guid? actualValue = target.SummaryPropertySetId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? DocumentPropertySetId"), Ignore]
        public void DocumentPropertySetIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid? DocumentPropertySetId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.DocumentPropertySetId = default;
            Guid? actualValue = target.DocumentPropertySetId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? AudioPropertySetId"), Ignore]
        public void AudioPropertySetIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid? AudioPropertySetId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.AudioPropertySetId = default;
            Guid? actualValue = target.AudioPropertySetId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? DRMPropertySetId"), Ignore]
        public void DRMPropertySetIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid? DRMPropertySetId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.DRMPropertySetId = default;
            Guid? actualValue = target.DRMPropertySetId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? GPSPropertySetId"), Ignore]
        public void GPSPropertySetIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid? GPSPropertySetId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.GPSPropertySetId = default;
            Guid? actualValue = target.GPSPropertySetId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? ImagePropertySetId"), Ignore]
        public void ImagePropertySetIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid? ImagePropertySetId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.ImagePropertySetId = default;
            Guid? actualValue = target.ImagePropertySetId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? MediaPropertySetId"), Ignore]
        public void MediaPropertySetIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid? MediaPropertySetId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.MediaPropertySetId = default;
            Guid? actualValue = target.MediaPropertySetId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? MusicPropertySetId"), Ignore]
        public void MusicPropertySetIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid? MusicPropertySetId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.MusicPropertySetId = default;
            Guid? actualValue = target.MusicPropertySetId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? PhotoPropertySetId"), Ignore]
        public void PhotoPropertySetIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid? PhotoPropertySetId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.PhotoPropertySetId = default;
            Guid? actualValue = target.PhotoPropertySetId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? RecordedTVPropertySetId"), Ignore]
        public void RecordedTVPropertySetIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid? RecordedTVPropertySetId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.RecordedTVPropertySetId = default;
            Guid? actualValue = target.RecordedTVPropertySetId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? VideoPropertySetId"), Ignore]
        public void VideoPropertySetIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid? VideoPropertySetId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.VideoPropertySetId = default;
            Guid? actualValue = target.VideoPropertySetId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("BinaryProperties BinaryProperties"), Ignore]
        public void BinaryPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for BinaryProperties BinaryProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            BinaryPropertySet expectedValue = default;
            target.BinaryProperties = default;
            BinaryPropertySet actualValue = target.BinaryProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Subdirectory Parent"), Ignore]
        public void ParentTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Subdirectory Parent

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Subdirectory expectedValue = default;
            target.Parent = default;
            Subdirectory actualValue = target.Parent;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Redundancy Redundancy"), Ignore]
        public void RedundancyTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Redundancy Redundancy

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Redundancy expectedValue = default;
            target.Redundancy = default;
            Redundancy actualValue = target.Redundancy;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("SummaryPropertySet SummaryProperties"), Ignore]
        public void SummaryPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for SummaryPropertySet SummaryProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            SummaryPropertySet expectedValue = default;
            target.SummaryProperties = default;
            SummaryPropertySet actualValue = target.SummaryProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DocumentPropertySet DocumentProperties"), Ignore]
        public void DocumentPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for DocumentPropertySet DocumentProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            DocumentPropertySet expectedValue = default;
            target.DocumentProperties = default;
            DocumentPropertySet actualValue = target.DocumentProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("AudioPropertySet AudioProperties"), Ignore]
        public void AudioPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for AudioPropertySet AudioProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            AudioPropertySet expectedValue = default;
            target.AudioProperties = default;
            AudioPropertySet actualValue = target.AudioProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DRMPropertySet DRMProperties"), Ignore]
        public void DRMPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for DRMPropertySet DRMProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            DRMPropertySet expectedValue = default;
            target.DRMProperties = default;
            DRMPropertySet actualValue = target.DRMProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("GPSPropertySet GPSProperties"), Ignore]
        public void GPSPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for GPSPropertySet GPSProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            GPSPropertySet expectedValue = default;
            target.GPSProperties = default;
            GPSPropertySet actualValue = target.GPSProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("ImagePropertySet ImageProperties"), Ignore]
        public void ImagePropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for ImagePropertySet ImageProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            ImagePropertySet expectedValue = default;
            target.ImageProperties = default;
            ImagePropertySet actualValue = target.ImageProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("MediaPropertySet MediaProperties"), Ignore]
        public void MediaPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for MediaPropertySet MediaProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            MediaPropertySet expectedValue = default;
            target.MediaProperties = default;
            MediaPropertySet actualValue = target.MediaProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("MusicPropertySet MusicProperties"), Ignore]
        public void MusicPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for MusicPropertySet MusicProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            MusicPropertySet expectedValue = default;
            target.MusicProperties = default;
            MusicPropertySet actualValue = target.MusicProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("PhotoPropertySet PhotoProperties"), Ignore]
        public void PhotoPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for PhotoPropertySet PhotoProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            PhotoPropertySet expectedValue = default;
            target.PhotoProperties = default;
            PhotoPropertySet actualValue = target.PhotoProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("RecordedTVPropertySet RecordedTVProperties"), Ignore]
        public void RecordedTVPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for RecordedTVPropertySet RecordedTVProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            RecordedTVPropertySet expectedValue = default;
            target.RecordedTVProperties = default;
            RecordedTVPropertySet actualValue = target.RecordedTVProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("VideoPropertySet VideoProperties"), Ignore]
        public void VideoPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for VideoPropertySet VideoProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            VideoPropertySet expectedValue = default;
            target.VideoProperties = default;
            VideoPropertySet actualValue = target.VideoProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("HashSet<FileAccessError> AccessErrors"), Ignore]
        public void AccessErrorsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for HashSet<FileAccessError> AccessErrors

            DbFile target = default; // TODO: Create and initialize DbFile instance
            HashSet<FileAccessError> expectedValue = default;
            target.AccessErrors = default;
            HashSet<FileAccessError> actualValue = target.AccessErrors;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("HashSet<FileComparison> BaselineComparisons"), Ignore]
        public void BaselineComparisonsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for HashSet<FileComparison> BaselineComparisons

            DbFile target = default; // TODO: Create and initialize DbFile instance
            HashSet<FileComparison> expectedValue = default;
            target.BaselineComparisons = default;
            HashSet<FileComparison> actualValue = target.BaselineComparisons;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("HashSet<FileComparison> CorrelativeComparisons"), Ignore]
        public void CorrelativeComparisonsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for HashSet<FileComparison> CorrelativeComparisons

            DbFile target = default; // TODO: Create and initialize DbFile instance
            HashSet<FileComparison> expectedValue = default;
            target.CorrelativeComparisons = default;
            HashSet<FileComparison> actualValue = target.CorrelativeComparisons;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? UpstreamId"), Ignore]
        public void UpstreamIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Guid? UpstreamId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.UpstreamId = default;
            Guid? actualValue = target.UpstreamId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime? LastSynchronizedOn"), Ignore]
        [TestProperty(TestHelper.TestProperty_Description,
            "Volume.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        public void LastSynchronizedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for DateTime? LastSynchronizedOn

            DbFile target = default; // TODO: Create and initialize DbFile instance
            DateTime? expectedValue = default;
            target.LastSynchronizedOn = default;
            DateTime? actualValue = target.LastSynchronizedOn;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime CreatedOn"), Ignore]
        [Microsoft.VisualStudio.TestTools.UnitTesting.Description("BinaryProperties.CreatedOn: CreatedOn<=ModifiedOn")]
        public void CreatedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for DateTime CreatedOn

            DbFile target = default; // TODO: Create and initialize DbFile instance
            DateTime expectedValue = default;
            target.CreatedOn = default;
            DateTime actualValue = target.CreatedOn;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime ModifiedOn"), Ignore]
        public void ModifiedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for DateTime ModifiedOn

            DbFile target = default; // TODO: Create and initialize DbFile instance
            DateTime expectedValue = default;
            target.ModifiedOn = default;
            DateTime actualValue = target.ModifiedOn;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("string Item[string]"), Ignore]
        public void ItemColumnNameTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for string Item[string]

            string columnNameIndex = default;
            DbFile target = default; // TODO: Create and initialize DbFile instance
            string expectedValue = default;
            string actualValue = target[columnNameIndex];
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("IEnumerable<ValidationResult> Validate(ValidationContext)"), Ignore]
        public void ValidateValidationContextTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for IEnumerable<ValidationResult> Validate(ValidationContext)

            ValidationContext validationContextArg = default;
            DbFile target = default; // TODO: Create and initialize DbFile instance
            IEnumerable<ValidationResult> expectedReturnValue = default;
            IEnumerable<ValidationResult> actualReturnValue = target.Validate(validationContextArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("bool HasErrors()"), Ignore]
        public void HasErrorsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for bool HasErrors()

            DbFile target = default; // TODO: Create and initialize DbFile instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.HasErrors();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void AcceptChanges()"), Ignore]
        public void AcceptChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for void AcceptChanges()

            DbFile target = default; // TODO: Create and initialize DbFile instance
            target.AcceptChanges();
        }

        [TestMethod("bool IsChanged()"), Ignore]
        public void IsChangedTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for bool IsChanged()

            DbFile target = default; // TODO: Create and initialize DbFile instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.IsChanged();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void RejectChanges()"), Ignore]
        public void RejectChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for void RejectChanges()

            DbFile target = default; // TODO: Create and initialize DbFile instance
            target.RejectChanges();
        }

        [TestMethod("Type GetType()"), Ignore]
        public void GetTypeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for Type GetType()

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Type expectedReturnValue = default;
            Type actualReturnValue = target.GetType();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("string ToString()"), Ignore]
        public void ToStringTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for string ToString()

            DbFile target = default; // TODO: Create and initialize DbFile instance
            string expectedReturnValue = default;
            string actualReturnValue = target.ToString();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("bool Equals(object)"), Ignore]
        public void EqualsObjectTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for bool Equals(object)

            object objArg = default;
            DbFile target = default; // TODO: Create and initialize DbFile instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.Equals(objArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("int GetHashCode()"), Ignore]
        public void GetHashCodeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // DEFERRED: Implement test for int GetHashCode()

            DbFile target = default; // TODO: Create and initialize DbFile instance
            int expectedReturnValue = default;
            int actualReturnValue = target.GetHashCode();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }
    }
}
