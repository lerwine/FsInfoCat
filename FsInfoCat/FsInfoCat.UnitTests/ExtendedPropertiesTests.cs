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
    public class ExtendedPropertiesTests
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [TestMethod("new ExtendedProperties()")]
        public void NewExtendedPropertiesTestMethod()
        {
            using var dbContext = Services.ServiceProvider.GetService<LocalDbContext>();
            ExtendedProperties target = new();

            EntityEntry<ExtendedProperties> entry = dbContext.Entry(target);
            Assert.AreEqual(Guid.Empty, target.Id);
            Assert.AreEqual(EntityState.Detached, entry.State);
            Assert.AreEqual(0, target.Width);
            Assert.AreEqual(0, target.Height);
            Assert.IsNull(target.Duration);
            Assert.IsNull(target.FrameCount);
            Assert.IsNull(target.TrackNumber);
            Assert.IsNull(target.Bitrate);
            Assert.IsNull(target.FrameRate);
            Assert.IsNull(target.SamplesPerPixel);
            Assert.IsNull(target.PixelPerUnitX);
            Assert.IsNull(target.PixelPerUnitY);
            Assert.IsNull(target.Compression);
            Assert.IsNull(target.XResNumerator);
            Assert.IsNull(target.XResDenominator);
            Assert.IsNull(target.YResNumerator);
            Assert.IsNull(target.YResDenominator);
            Assert.IsNull(target.ResolutionXUnit);
            Assert.IsNull(target.ResolutionYUnit);
            Assert.IsNull(target.JPEGProc);
            Assert.IsNull(target.JPEGQuality);
            Assert.IsNull(target.DateTime);
            Assert.IsNull(target.Title);
            Assert.IsNull(target.Description);
            Assert.IsNull(target.Copyright);
            Assert.IsNull(target.SoftwareUsed);
            Assert.IsNull(target.Artist);
            Assert.IsNull(target.HostComputer);
            Assert.IsNotNull(target.Files);
            Assert.AreEqual(0, target.Files.Count);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);

            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for new ExtendedProperties()

            dbContext.ExtendedProperties.Add(target);
            Assert.AreNotEqual(Guid.Empty, target.Id);
            Assert.AreEqual(EntityState.Added, entry.State);
            Assert.AreEqual(0, target.Width);
            Assert.AreEqual(0, target.Height);
            Assert.IsNull(target.Duration);
            Assert.IsNull(target.FrameCount);
            Assert.IsNull(target.TrackNumber);
            Assert.IsNull(target.Bitrate);
            Assert.IsNull(target.FrameRate);
            Assert.IsNull(target.SamplesPerPixel);
            Assert.IsNull(target.PixelPerUnitX);
            Assert.IsNull(target.PixelPerUnitY);
            Assert.IsNull(target.Compression);
            Assert.IsNull(target.XResNumerator);
            Assert.IsNull(target.XResDenominator);
            Assert.IsNull(target.YResNumerator);
            Assert.IsNull(target.YResDenominator);
            Assert.IsNull(target.ResolutionXUnit);
            Assert.IsNull(target.ResolutionYUnit);
            Assert.IsNull(target.JPEGProc);
            Assert.IsNull(target.JPEGQuality);
            Assert.IsNull(target.DateTime);
            Assert.IsNull(target.Title);
            Assert.IsNull(target.Description);
            Assert.IsNull(target.Copyright);
            Assert.IsNull(target.SoftwareUsed);
            Assert.IsNull(target.Artist);
            Assert.IsNull(target.HostComputer);
            Assert.IsNotNull(target.Files);
            Assert.AreEqual(0, target.Files.Count);
            Assert.IsNull(target.UpstreamId);
            Assert.IsNull(target.LastSynchronizedOn);
            Assert.AreEqual(target.CreatedOn, target.ModifiedOn);
        }

        [TestMethod("Guid Id")]
        [Ignore]
        public void IdTestMethod()
        {
            ExtendedProperties target = new();
            Guid expectedValue = Guid.NewGuid();
            target.Id = expectedValue;
            Guid actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
            target.Id = expectedValue;
            actualValue = target.Id;
            Assert.AreEqual(expectedValue, actualValue);
            Assert.ThrowsException<InvalidOperationException>(() => target.Id = Guid.NewGuid());
        }

        [TestMethod("string Kind")]
        public void KindTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for ushort Width

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            string expectedValue = default;
            target.Width = default;
            string actualValue = target.Kind;
            Assert.AreEqual(expectedValue, actualValue);
        }


        [TestMethod("ushort Width")]
        public void WidthTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for ushort Width

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            ushort? expectedValue = default;
            target.Width = default;
            ushort? actualValue = target.Width;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("ushort Height")]
        public void HeightTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for ushort Height

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            ushort? expectedValue = default;
            target.Height = default;
            ushort? actualValue = target.Height;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("ulong? Duration")]
        public void DurationTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for ulong? Duration

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            ulong? expectedValue = default;
            target.Duration = default;
            ulong? actualValue = target.Duration;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("uint? FrameCount")]
        public void FrameCountTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for uint? FrameCount

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            uint? expectedValue = default;
            target.FrameCount = default;
            uint? actualValue = target.FrameCount;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("uint? TrackNumber")]
        public void TrackNumberTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for uint? TrackNumber

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            uint? expectedValue = default;
            target.TrackNumber = default;
            uint? actualValue = target.TrackNumber;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("uint? Bitrate")]
        public void BitrateTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for uint? Bitrate

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            uint? expectedValue = default;
            target.Bitrate = default;
            uint? actualValue = target.Bitrate;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("uint? FrameRate")]
        public void FrameRateTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for uint? FrameRate

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            uint? expectedValue = default;
            target.FrameRate = default;
            uint? actualValue = target.FrameRate;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("ushort? SamplesPerPixel")]
        public void SamplesPerPixelTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for ushort? SamplesPerPixel

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            ushort? expectedValue = default;
            target.SamplesPerPixel = default;
            ushort? actualValue = target.SamplesPerPixel;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("uint? PixelPerUnitX")]
        public void PixelPerUnitXTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for uint? PixelPerUnitX

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            uint? expectedValue = default;
            target.PixelPerUnitX = default;
            uint? actualValue = target.PixelPerUnitX;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("uint? PixelPerUnitY")]
        public void PixelPerUnitYTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for uint? PixelPerUnitY

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            uint? expectedValue = default;
            target.PixelPerUnitY = default;
            uint? actualValue = target.PixelPerUnitY;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("ushort? Compression")]
        public void CompressionTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for ushort? Compression

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            ushort? expectedValue = default;
            target.Compression = default;
            ushort? actualValue = target.Compression;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("uint? XResNumerator")]
        public void XResNumeratorTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for uint? XResNumerator

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            uint? expectedValue = default;
            target.XResNumerator = default;
            uint? actualValue = target.XResNumerator;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("uint? XResDenominator")]
        public void XResDenominatorTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for uint? XResDenominator

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            uint? expectedValue = default;
            target.XResDenominator = default;
            uint? actualValue = target.XResDenominator;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("uint? YResNumerator")]
        public void YResNumeratorTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for uint? YResNumerator

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            uint? expectedValue = default;
            target.YResNumerator = default;
            uint? actualValue = target.YResNumerator;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("uint? YResDenominator")]
        public void YResDenominatorTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for uint? YResDenominator

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            uint? expectedValue = default;
            target.YResDenominator = default;
            uint? actualValue = target.YResDenominator;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("ushort? ResolutionXUnit")]
        public void ResolutionXUnitTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for ushort? ResolutionXUnit

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            ushort? expectedValue = default;
            target.ResolutionXUnit = default;
            ushort? actualValue = target.ResolutionXUnit;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("ushort? ResolutionYUnit")]
        public void ResolutionYUnitTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for ushort? ResolutionYUnit

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            ushort? expectedValue = default;
            target.ResolutionYUnit = default;
            ushort? actualValue = target.ResolutionYUnit;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("ushort? JPEGProc")]
        public void JPEGProcTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for ushort? JPEGProc

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            ushort? expectedValue = default;
            target.JPEGProc = default;
            ushort? actualValue = target.JPEGProc;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("ushort? JPEGQuality")]
        public void JPEGQualityTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for ushort? JPEGQuality

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            ushort? expectedValue = default;
            target.JPEGQuality = default;
            ushort? actualValue = target.JPEGQuality;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("DateTime? DateTime")]
        public void DateTimeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for DateTime? DateTime

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            DateTime? expectedValue = default;
            target.DateTime = default;
            DateTime? actualValue = target.DateTime;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("string Title")]
        public void TitleTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string Title

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            string expectedValue = default;
            target.Title = default;
            string actualValue = target.Title;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("string Description")]
        public void DescriptionTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string Description

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            string expectedValue = default;
            target.Description = default;
            string actualValue = target.Description;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("string Copyright")]
        public void CopyrightTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string Copyright

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            string expectedValue = default;
            target.Copyright = default;
            string actualValue = target.Copyright;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("string SoftwareUsed")]
        public void SoftwareUsedTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string SoftwareUsed

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            string expectedValue = default;
            target.SoftwareUsed = default;
            string actualValue = target.SoftwareUsed;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("string Artist")]
        public void ArtistTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string Artist

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            string expectedValue = default;
            target.Artist = default;
            string actualValue = target.Artist;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("string HostComputer")]
        public void HostComputerTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string HostComputer

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            string expectedValue = default;
            target.HostComputer = default;
            string actualValue = target.HostComputer;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("HashSet<DbFile> Files")]
        public void FilesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for HashSet<DbFile> Files

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            HashSet<DbFile> expectedValue = default;
            target.Files = default;
            HashSet<DbFile> actualValue = target.Files;
            Assert.AreEqual(expectedValue, actualValue);
        }

        [TestMethod("Guid? UpstreamId")]
        public void UpstreamIdTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Guid? UpstreamId

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
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

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
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

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
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

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
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
            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
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
            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            IEnumerable<ValidationResult> expectedReturnValue = default;
            IEnumerable<ValidationResult> actualReturnValue = target.Validate(validationContextArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("bool HasErrors()")]
        public void HasErrorsTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool HasErrors()

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.HasErrors();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void AcceptChanges()")]
        public void AcceptChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for void AcceptChanges()

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            target.AcceptChanges();
        }

        [TestMethod("bool IsChanged()")]
        public void IsChangedTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for bool IsChanged()

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.IsChanged();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("void RejectChanges()")]
        public void RejectChangesTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for void RejectChanges()

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            target.RejectChanges();
        }

        [TestMethod("Type GetType()")]
        public void GetTypeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for Type GetType()

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            Type expectedReturnValue = default;
            Type actualReturnValue = target.GetType();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("string ToString()")]
        public void ToStringTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for string ToString()

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
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
            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            bool expectedReturnValue = default;
            bool actualReturnValue = target.Equals(objArg);
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }

        [TestMethod("int GetHashCode()")]
        public void GetHashCodeTestMethod()
        {
            Assert.Inconclusive("Test not implemented");
            // TODO: Implement test for int GetHashCode()

            ExtendedProperties target = default; // TODO: Create and initialize ExtendedProperties instance
            int expectedReturnValue = default;
            int actualReturnValue = target.GetHashCode();
            Assert.AreEqual(expectedReturnValue, actualReturnValue);
        }
    }
}
