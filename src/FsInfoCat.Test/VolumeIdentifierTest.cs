using System;
using NUnit.Framework;
using FsInfoCat.Models.Volumes;

namespace FsInfoCat.Test
{
    [TestFixture]
    public partial class VolumeIdentifierTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test, Property("Priority", 1), NUnit.Framework.Category("Working")]
        [TestCaseSource("GetGuidToIdentifierStringTestCases")]
        public string GuidToIdentifierStringTest(Guid guid)
        {
            return VolumeIdentifier.ToIdentifierString(guid);
        }

        [Test, Property("Priority", 1), NUnit.Framework.Category("Working")]
        [TestCaseSource("GetSerialNumberToIdentifierStringTestCases")]
        public string SerialNumberToIdentifierStringTest(uint serialNumber, byte? ordinal)
        {
            if (ordinal.HasValue)
                return VolumeIdentifier.ToIdentifierString(serialNumber, ordinal.Value);
            return VolumeIdentifier.ToIdentifierString(serialNumber);
        }

        [Test, Property("Priority", 2)]
        [TestCaseSource("GetGuidToUrnTestCases")]
        public string GuidToUrnTest(Guid guid)
        {
            return VolumeIdentifier.ToUrn(guid);
        }

        [Test, Property("Priority", 3)]
        [TestCaseSource("GetSerialNumberToUrnTestCases")]
        public string SerialNumberToUrnTest(uint serialNumber, byte? ordinal)
        {
            if (ordinal.HasValue)
                return VolumeIdentifier.ToUrn(serialNumber, ordinal.Value);
            return VolumeIdentifier.ToUrn(serialNumber);
        }

        [Test, Property("Priority", 3)]
        [TestCaseSource("GetGuidParameterTestCases")]
        public IdValues FromGuidValueConstructorTest(Guid guid)
        {
            VolumeIdentifier target = new VolumeIdentifier(guid);
            Assert.That(target.Location.IsAbsoluteUri, Is.True);
            Assert.That(target.Location.IsFile, Is.False);
            Assert.That(target.Location.Scheme, Is.EqualTo("urn"));
            Assert.That(target.Location.Fragment, Is.Empty);
            Assert.That(target.UUID.HasValue, Is.True);
            Assert.That(target.UUID.Value, Is.EqualTo(guid));
            Assert.That(target.SerialNumber.HasValue, Is.False);
            Assert.That(target.Ordinal.HasValue, Is.False);
            return new IdValues(target.Location.AbsolutePath, target.Location.AbsoluteUri);
        }

        [Test, Property("Priority", 3)]
        [TestCaseSource("GetValidUUIDUriParameterTestCases")]
        public UuidValues FromValidUUIDUriConstructorTest(string uri)
        {
            VolumeIdentifier target = new VolumeIdentifier(uri);
            Assert.That(target.Location.IsAbsoluteUri, Is.True);
            Assert.That(target.Location.IsFile, Is.False);
            Assert.That(target.Location.Scheme, Is.EqualTo("urn"));
            Assert.That(target.Location.Fragment, Is.Empty);
            return new UuidValues(target.UUID, target.Location.AbsolutePath, target.Location.AbsoluteUri);
        }

        [Test, Property("Priority", 3)]
        [TestCaseSource("GetSerialNumberParameterTestCases")]
        public IdValues FromSerialNumberValueConstructorTest(uint serialNumber)
        {
            VolumeIdentifier target = new VolumeIdentifier(serialNumber);
            Assert.That(target.Location.IsAbsoluteUri, Is.True);
            Assert.That(target.Location.IsFile, Is.False);
            Assert.That(target.Location.Scheme, Is.EqualTo("urn"));
            Assert.That(target.Location.Fragment, Is.Empty);
            Assert.That(target.SerialNumber.HasValue, Is.True);
            Assert.That(target.SerialNumber.Value, Is.EqualTo(serialNumber));
            Assert.That(target.UUID.HasValue, Is.False);
            Assert.That(target.Ordinal.HasValue, Is.False);
            return new IdValues(target.Location.AbsolutePath, target.Location.AbsoluteUri);
        }

        [Test, Property("Priority", 3)]
        [TestCaseSource("GetSerialNumberAndOrdinalParametersTestCases")]
        public SnIdValues FromSerialNumberAndOrdinalValuesConstructorTest(uint serialNumber, byte ordinal)
        {
            VolumeIdentifier target = new VolumeIdentifier(serialNumber, ordinal);
            Assert.That(target.Location.IsAbsoluteUri, Is.True);
            Assert.That(target.Location.IsFile, Is.False);
            Assert.That(target.Location.Scheme, Is.EqualTo("urn"));
            Assert.That(target.Location.Fragment, Is.Empty);
            Assert.That(target.SerialNumber.HasValue, Is.True);
            Assert.That(target.SerialNumber.Value, Is.EqualTo(serialNumber));
            Assert.That(target.UUID.HasValue, Is.False);
            Assert.That(target.Ordinal.HasValue, Is.True);
            return new SnIdValues(target.SerialNumber, target.Ordinal, target.Location.AbsolutePath, target.Location.AbsoluteUri);
        }

        [Test, Property("Priority", 3)]
        [TestCaseSource("GetValidSerialNumberUriParameterTestCases")]
        public SnIdValues FromValidSerialNumberAndOrdinalUriConstructorTest(string uri)
        {
            VolumeIdentifier target = new VolumeIdentifier(uri);
            Assert.That(target.Location.IsAbsoluteUri, Is.True);
            Assert.That(target.Location.IsFile, Is.False);
            Assert.That(target.Location.Scheme, Is.EqualTo("urn"));
            Assert.That(target.Location.Fragment, Is.Empty);
            Assert.That(target.SerialNumber.HasValue, Is.True);
            Assert.That(target.UUID.HasValue, Is.False);
            return new SnIdValues(target.SerialNumber, target.Ordinal, target.Location.AbsolutePath, target.Location.AbsoluteUri);
        }

        [Test, Property("Priority", 3)]
        [TestCaseSource("GetValidFilePathParameterTestCases")]
        public IdValues FromValidFilePathConstructorTest(string path)
        {
            VolumeIdentifier target = new VolumeIdentifier(path);
            Assert.That(target.Location.IsAbsoluteUri, Is.True);
            Assert.That(target.Location.IsFile, Is.True);
            Assert.That(target.Location.Scheme, Is.EqualTo("file"));
            Assert.That(target.Location.Fragment, Is.Empty);
            Assert.That(target.SerialNumber.HasValue, Is.False);
            Assert.That(target.UUID.HasValue, Is.False);
            Assert.That(target.Ordinal.HasValue, Is.False);
            return new IdValues(target.Location.AbsolutePath, target.Location.AbsoluteUri);
        }

        [Test, Property("Priority", 3)]
        [TestCaseSource("GetValidFileUrlParameterTestCases")]
        public IdValues FromValidFileUrlConstructorTest(string uri)
        {
            VolumeIdentifier target = new VolumeIdentifier(uri);
            Assert.That(target.Location.IsAbsoluteUri, Is.True);
            Assert.That(target.Location.IsFile, Is.True);
            Assert.That(target.Location.Scheme, Is.EqualTo("file"));
            Assert.That(target.Location.Fragment, Is.Empty);
            Assert.That(target.SerialNumber.HasValue, Is.False);
            Assert.That(target.UUID.HasValue, Is.False);
            Assert.That(target.Ordinal.HasValue, Is.False);
            Assert.That(target.Location.AbsolutePath, Is.EqualTo(target.Location.LocalPath));
            return new IdValues(target.Location.AbsolutePath, target.Location.AbsoluteUri);
        }

        [Test, Property("Priority", 3)]
        public void FromInvalidUriConstructorTest()
        {
            Assert.That(() => { new VolumeIdentifier(null); }, Throws.ArgumentNullException
                .With.Property("ParamName").EqualTo("url"));
            Assert.That(() => { new VolumeIdentifier(_systemDrivePath); }, Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("url"));
            Assert.That(() => { new VolumeIdentifier(_systemDriveUrl); }, Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("url"));
            Assert.That(() => { new VolumeIdentifier("file:///"); }, Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("url"));
        }

        [Test, Property("Priority", 3)]
        public void FromInvalidUriFormatConstructorTest([Values("", " ", ".", "urn")] string uri)
        {
            Assert.That(() => { new VolumeIdentifier(uri); }, Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("url"));
        }

        [Test, Property("Priority", 3)]
        public void FromInvalidUrnNamespaceConstructorTest([Values("urn:", "urn:id")] string uri)
        {
            Assert.That(() => { new VolumeIdentifier(uri); }, Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("uri"));
        }

        [Test, Property("Priority", 3)]
        public void FromInvalidUUIDUrnNamespaceConstructorTest([Values(
            "urn:uuid", "urn:uuid:", "urn:uuid:fb4f6ff1-7ffe-4eef-840a-1df28bb2e178:2", "urn:uuid:fb4f6ff1-7ffe-4eef-840a-1df28bb2e17"
        )] string uri)
        {
            Assert.That(() => { new VolumeIdentifier(uri); }, Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("uri"));
        }

        [Test, Property("Priority", 3)]
        public void FromInvalidVolumeIdUrnNamespaceConstructorTest([Values(
            "urn:volume", "urn:volume:", "urn:volume:id", "urn:volume:id:", "urn:volume:id:3B51-8D4B-1", "urn:volume:id:3B518D4B-"
        )] string uri)
        {
            Assert.That(() => { new VolumeIdentifier(uri); }, Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("uri"));
        }

        [Test, Property("Priority", 3)]
        public void FromInvalidUriSchemeNamespaceConstructorTest([Values(
            "http://tempuri.org", "mailto:volume@id.3B51-8D4B"
        )] string uri)
        {
            Assert.That(() => { new VolumeIdentifier(uri); }, Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("url"));
        }
    }
}
