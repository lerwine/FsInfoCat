using FsInfoCat;
using FsInfoCat.Local;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class DbFileTests
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod("new DbFile()")]
        public void NewDbFileTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<LocalDbContext>();
            DbFile target = new();

            EntityEntry<DbFile> entry = dbContext.Entry(target);
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.AreEqual(EntityState.Detached, entry.State);
            Assert.IsNotNull(target.Name);
            Assert.AreEqual("", target.Name);
            Assert.AreEqual(FileCrawlOptions.None, target.Options);
            Assert.IsNull(target.LastHashCalculation);
            Assert.IsNotNull(target.Notes);
            Assert.AreEqual("", target.Notes);
            Assert.IsFalse(target.Deleted);
            Assert.AreEqual(Guid.Empty, target.ParentId);
            Assert.AreEqual(Guid.Empty, target.BinaryPropertiesId);
            Assert.IsNull(target.SummaryPropertiesId);
            Assert.IsNull(target.DocumentPropertiesId);
            Assert.IsNull(target.AudioPropertiesId);
            Assert.IsNull(target.DRMPropertiesId);
            Assert.IsNull(target.GPSPropertiesId);
            Assert.IsNull(target.ImagePropertiesId);
            Assert.IsNull(target.MediaPropertiesId);
            Assert.IsNull(target.MusicPropertiesId);
            Assert.IsNull(target.PhotoPropertiesId);
            Assert.IsNull(target.RecordedTVPropertiesId);
            Assert.IsNull(target.VideoPropertiesId);
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
            Assert.IsNotNull(target.ComparisonSources);
            Assert.AreEqual(0, target.ComparisonSources.Count);
            Assert.IsNotNull(target.ComparisonTargets);
            Assert.AreEqual(0, target.ComparisonTargets.Count);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
            Assert.AreEqual(target.CreatedOn, target.LastAccessed);

            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for new DbFile()

            dbContext.Files.Add(target);
            Assert.AreNotEqual(Guid.Empty, target.Id);
            Assert.AreEqual(EntityState.Added, entry.State);
            Assert.IsNotNull(target.Name);
            Assert.AreEqual("", target.Name);
            Assert.AreEqual(FileCrawlOptions.None, target.Options);
            Assert.IsNull(target.LastHashCalculation);
            Assert.IsNotNull(target.Notes);
            Assert.AreEqual("", target.Notes);
            Assert.IsFalse(target.Deleted);
            Assert.AreEqual(Guid.Empty, target.ParentId);
            Assert.AreEqual(Guid.Empty, target.BinaryPropertiesId);
            Assert.IsNull(target.SummaryPropertiesId);
            Assert.IsNull(target.DocumentPropertiesId);
            Assert.IsNull(target.AudioPropertiesId);
            Assert.IsNull(target.DRMPropertiesId);
            Assert.IsNull(target.GPSPropertiesId);
            Assert.IsNull(target.ImagePropertiesId);
            Assert.IsNull(target.MediaPropertiesId);
            Assert.IsNull(target.MusicPropertiesId);
            Assert.IsNull(target.PhotoPropertiesId);
            Assert.IsNull(target.RecordedTVPropertiesId);
            Assert.IsNull(target.VideoPropertiesId);
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
            Assert.IsNotNull(target.ComparisonSources);
            Assert.AreEqual(0, target.ComparisonSources.Count);
            Assert.IsNotNull(target.ComparisonTargets);
            Assert.AreEqual(0, target.ComparisonTargets.Count);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
            Assert.AreEqual(target.CreatedOn, target.LastAccessed);
        }

        [TestMethod("Guid Id")]
        [Ignore]
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

        [TestMethod("string Name")]
        public void NameTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string Name

            DbFile target = default; // TODO: Create and initialize DbFile instance
            string expectedValue = default;
            target.Name = default;
            string actualValue = target.Name;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("FileCrawlOptions Options")]
        public void OptionsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for FileCrawlOptions Options

            DbFile target = default; // TODO: Create and initialize DbFile instance
            FileCrawlOptions expectedValue = default;
            target.Options = default;
            FileCrawlOptions actualValue = target.Options;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime LastAccessed")]
        public void LastAccessedTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DateTime LastAccessed

            DbFile target = default; // TODO: Create and initialize DbFile instance
            DateTime expectedValue = default;
            target.LastAccessed = default;
            DateTime actualValue = target.LastAccessed;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime? LastHashCalculation")]
        public void LastHashCalculationTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DateTime? LastHashCalculation

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

        [TestMethod("bool Deleted")]
        public void DeletedTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool Deleted

            DbFile target = default; // TODO: Create and initialize DbFile instance
            bool expectedValue = default;
            target.Deleted = default;
            bool actualValue = target.Deleted;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid ParentId")]
        public void ParentIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid ParentId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid expectedValue = default;
            target.ParentId = default;
            Guid actualValue = target.ParentId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid BinaryPropertiesId")]
        public void BinaryPropertiesIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid BinaryPropertiesId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid expectedValue = default;
            target.BinaryPropertiesId = default;
            Guid actualValue = target.BinaryPropertiesId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? SummaryPropertiesId")]
        public void SummaryPropertiesIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid? SummaryPropertiesId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.SummaryPropertiesId = default;
            Guid? actualValue = target.SummaryPropertiesId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? DocumentPropertiesId")]
        public void DocumentPropertiesIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid? DocumentPropertiesId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.DocumentPropertiesId = default;
            Guid? actualValue = target.DocumentPropertiesId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? AudioPropertiesId")]
        public void AudioPropertiesIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid? AudioPropertiesId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.AudioPropertiesId = default;
            Guid? actualValue = target.AudioPropertiesId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? DRMPropertiesId")]
        public void DRMPropertiesIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid? DRMPropertiesId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.DRMPropertiesId = default;
            Guid? actualValue = target.DRMPropertiesId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? GPSPropertiesId")]
        public void GPSPropertiesIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid? GPSPropertiesId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.GPSPropertiesId = default;
            Guid? actualValue = target.GPSPropertiesId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? ImagePropertiesId")]
        public void ImagePropertiesIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid? ImagePropertiesId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.ImagePropertiesId = default;
            Guid? actualValue = target.ImagePropertiesId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? MediaPropertiesId")]
        public void MediaPropertiesIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid? MediaPropertiesId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.MediaPropertiesId = default;
            Guid? actualValue = target.MediaPropertiesId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? MusicPropertiesId")]
        public void MusicPropertiesIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid? MusicPropertiesId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.MusicPropertiesId = default;
            Guid? actualValue = target.MusicPropertiesId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? PhotoPropertiesId")]
        public void PhotoPropertiesIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid? PhotoPropertiesId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.PhotoPropertiesId = default;
            Guid? actualValue = target.PhotoPropertiesId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? RecordedTVPropertiesId")]
        public void RecordedTVPropertiesIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid? RecordedTVPropertiesId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.RecordedTVPropertiesId = default;
            Guid? actualValue = target.RecordedTVPropertiesId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? VideoPropertiesId")]
        public void VideoPropertiesIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid? VideoPropertiesId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.VideoPropertiesId = default;
            Guid? actualValue = target.VideoPropertiesId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("BinaryProperties BinaryProperties")]
        public void BinaryPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for BinaryProperties BinaryProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            BinaryProperties expectedValue = default;
            target.BinaryProperties = default;
            BinaryProperties actualValue = target.BinaryProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Subdirectory Parent")]
        public void ParentTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Subdirectory Parent

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Subdirectory expectedValue = default;
            target.Parent = default;
            Subdirectory actualValue = target.Parent;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Redundancy Redundancy")]
        public void RedundancyTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Redundancy Redundancy

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Redundancy expectedValue = default;
            target.Redundancy = default;
            Redundancy actualValue = target.Redundancy;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("SummaryProperties SummaryProperties")]
        public void SummaryPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for SummaryProperties SummaryProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            SummaryProperties expectedValue = default;
            target.SummaryProperties = default;
            SummaryProperties actualValue = target.SummaryProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DocumentProperties DocumentProperties")]
        public void DocumentPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DocumentProperties DocumentProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            DocumentProperties expectedValue = default;
            target.DocumentProperties = default;
            DocumentProperties actualValue = target.DocumentProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("AudioProperties AudioProperties")]
        public void AudioPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for AudioProperties AudioProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            AudioProperties expectedValue = default;
            target.AudioProperties = default;
            AudioProperties actualValue = target.AudioProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DRMProperties DRMProperties")]
        public void DRMPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DRMProperties DRMProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            DRMProperties expectedValue = default;
            target.DRMProperties = default;
            DRMProperties actualValue = target.DRMProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("GPSProperties GPSProperties")]
        public void GPSPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for GPSProperties GPSProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            GPSProperties expectedValue = default;
            target.GPSProperties = default;
            GPSProperties actualValue = target.GPSProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("ImageProperties ImageProperties")]
        public void ImagePropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for ImageProperties ImageProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            ImageProperties expectedValue = default;
            target.ImageProperties = default;
            ImageProperties actualValue = target.ImageProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("MediaProperties MediaProperties")]
        public void MediaPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for MediaProperties MediaProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            MediaProperties expectedValue = default;
            target.MediaProperties = default;
            MediaProperties actualValue = target.MediaProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("MusicProperties MusicProperties")]
        public void MusicPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for MusicProperties MusicProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            MusicProperties expectedValue = default;
            target.MusicProperties = default;
            MusicProperties actualValue = target.MusicProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("PhotoProperties PhotoProperties")]
        public void PhotoPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for PhotoProperties PhotoProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            PhotoProperties expectedValue = default;
            target.PhotoProperties = default;
            PhotoProperties actualValue = target.PhotoProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("RecordedTVProperties RecordedTVProperties")]
        public void RecordedTVPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for RecordedTVProperties RecordedTVProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            RecordedTVProperties expectedValue = default;
            target.RecordedTVProperties = default;
            RecordedTVProperties actualValue = target.RecordedTVProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("VideoProperties VideoProperties")]
        public void VideoPropertiesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for VideoProperties VideoProperties

            DbFile target = default; // TODO: Create and initialize DbFile instance
            VideoProperties expectedValue = default;
            target.VideoProperties = default;
            VideoProperties actualValue = target.VideoProperties;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("HashSet<FileAccessError> AccessErrors")]
        public void AccessErrorsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for HashSet<FileAccessError> AccessErrors

            DbFile target = default; // TODO: Create and initialize DbFile instance
            HashSet<FileAccessError> expectedValue = default;
            target.AccessErrors = default;
            HashSet<FileAccessError> actualValue = target.AccessErrors;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("HashSet<FileComparison> ComparisonSources")]
        public void ComparisonSourcesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for HashSet<FileComparison> ComparisonSources

            DbFile target = default; // TODO: Create and initialize DbFile instance
            HashSet<FileComparison> expectedValue = default;
            target.ComparisonSources = default;
            HashSet<FileComparison> actualValue = target.ComparisonSources;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("HashSet<FileComparison> ComparisonTargets")]
        public void ComparisonTargetsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for HashSet<FileComparison> ComparisonTargets

            DbFile target = default; // TODO: Create and initialize DbFile instance
            HashSet<FileComparison> expectedValue = default;
            target.ComparisonTargets = default;
            HashSet<FileComparison> actualValue = target.ComparisonTargets;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? UpstreamId")]
        public void UpstreamIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid? UpstreamId

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Guid? expectedValue = default;
            target.UpstreamId = default;
            Guid? actualValue = target.UpstreamId;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime? LastSynchronizedOn")]
        [TestProperty(TestHelper.TestProperty_Description,
            "Volume.LastSynchronizedOn: (UpstreamId IS NULL OR LastSynchronizedOn IS NOT NULL) AND LastSynchronizedOn>=CreatedOn AND LastSynchronizedOn<=ModifiedOn")]
        public void LastSynchronizedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DateTime? LastSynchronizedOn

            DbFile target = default; // TODO: Create and initialize DbFile instance
            DateTime? expectedValue = default;
            target.LastSynchronizedOn = default;
            DateTime? actualValue = target.LastSynchronizedOn;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime CreatedOn")]
        [TestProperty(TestHelper.TestProperty_Description, "BinaryProperties.CreatedOn: CreatedOn<=ModifiedOn")]
        public void CreatedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DateTime CreatedOn

            DbFile target = default; // TODO: Create and initialize DbFile instance
            DateTime expectedValue = default;
            target.CreatedOn = default;
            DateTime actualValue = target.CreatedOn;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime ModifiedOn")]
        public void ModifiedOnTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DateTime ModifiedOn

            DbFile target = default; // TODO: Create and initialize DbFile instance
            DateTime expectedValue = default;
            target.ModifiedOn = default;
            DateTime actualValue = target.ModifiedOn;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("string Item[string]")]
        public void ItemColumnNameTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string Item[string]

            string columnNameIndex = default;
            DbFile target = default; // TODO: Create and initialize DbFile instance
            string expectedValue = default;
            string actualValue = target[columnNameIndex];
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("IEnumerable<ValidationResult> Validate(ValidationContext)")]
        public void ValidateValidationContextTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for IEnumerable<ValidationResult> Validate(ValidationContext)

            ValidationContext validationContextArg = default;
            DbFile target = default; // TODO: Create and initialize DbFile instance
            IEnumerable<ValidationResult> expectedReturnValue = default;
            IEnumerable<ValidationResult> actualReturnValue = target.Validate(validationContextArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("bool HasErrors()")]
        public void HasErrorsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool HasErrors()

            DbFile target = default; // TODO: Create and initialize DbFile instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.HasErrors();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void AcceptChanges()")]
        public void AcceptChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for void AcceptChanges()

            DbFile target = default; // TODO: Create and initialize DbFile instance
            target.AcceptChanges();
        }

        [TestMethod("bool IsChanged()")]
        public void IsChangedTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool IsChanged()

            DbFile target = default; // TODO: Create and initialize DbFile instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.IsChanged();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void RejectChanges()")]
        public void RejectChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for void RejectChanges()

            DbFile target = default; // TODO: Create and initialize DbFile instance
            target.RejectChanges();
        }

        [TestMethod("Type GetType()")]
        public void GetTypeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Type GetType()

            DbFile target = default; // TODO: Create and initialize DbFile instance
            Type expectedReturnValue = default;
            Type actualReturnValue = target.GetType();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("string ToString()")]
        public void ToStringTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string ToString()

            DbFile target = default; // TODO: Create and initialize DbFile instance
            string expectedReturnValue = default;
            string actualReturnValue = target.ToString();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("bool Equals(object)")]
        public void EqualsObjectTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool Equals(object)

            object objArg = default;
            DbFile target = default; // TODO: Create and initialize DbFile instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.Equals(objArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("int GetHashCode()")]
        public void GetHashCodeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for int GetHashCode()

            DbFile target = default; // TODO: Create and initialize DbFile instance
            int expectedReturnValue = default;
            int actualReturnValue = target.GetHashCode();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }
    }
}
