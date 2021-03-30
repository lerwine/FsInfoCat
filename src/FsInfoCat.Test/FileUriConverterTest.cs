using FsInfoCat.Test.FileUriConverterTestHelpers;
using FsInfoCat.Test.Helpers;
using FsInfoCat.Util;
using Microsoft.Extensions.Logging;
using NUnit.Framework;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace FsInfoCat.Test
{
    [TestFixture()]
    public class FileUriConverterTest
    {
        private static readonly ILogger<FileUriConverterTest> _logger;
        private static readonly FilePathTestData _testItems;
        private static readonly HostTestData _hostTestData;

        static FileUriConverterTest()
        {
            _logger = TestLogger.Create<FileUriConverterTest>();

            try
            {
                _testItems = FilePathTestData.Load();
                _hostTestData = HostTestData.Load();
            }
            catch (Exception exc)
            {
                _logger.LogCritical(exc, "Failed to load test items");
                throw;
            }
        }


        [Test, Property("Priority", 1), Ignore("Not necessary to always test")]
        public void HostTestDataChecks()
        {
            XDocument document = XDocument.Parse(Properties.Resources.HostTestData);
            Assert.That(document, Is.Not.Null, "HostTestData is null");
            Assert.That(document.Root, Is.Not.Null, "HostTestData.Root is null");
            XmlReaderSettings settings = new XmlReaderSettings
            {
                CheckCharacters = true,
                ConformanceLevel = ConformanceLevel.Document,
                DtdProcessing = DtdProcessing.Ignore,
                Schemas = new XmlSchemaSet(),
                ValidationFlags = XmlSchemaValidationFlags.ProcessIdentityConstraints | XmlSchemaValidationFlags.ReportValidationWarnings,
                ValidationType = ValidationType.Schema
            };
            ConcurrentQueue<string> validationErrors = new ConcurrentQueue<string>();
            settings.ValidationEventHandler += (object sender, ValidationEventArgs e) =>
            {
                if (e.Severity == XmlSeverityType.Warning)
                {
                    if (e.Exception is null)
                        _logger.LogWarning(e.Message);
                    else
                        _logger.LogWarning(e.Exception, e.Message);
                }
                else
                {
                    if (e.Exception is null)
                        validationErrors.Enqueue((string.IsNullOrWhiteSpace(e.Message)) ? "An unexpected validation exception has occurred" : e.Message);
                    else
                    {
                        string message = (string.IsNullOrEmpty(e.Message)) ? e.Exception.ToString() : e.Message;
                        validationErrors.Enqueue(message);
                        _logger.LogError(e.Exception, message);
                    }
                }
            };
            using (StringReader stringReader = new StringReader(Properties.Resources.HostTestDataSchema))
            {
                using XmlReader xmlReader = XmlReader.Create(stringReader);
                settings.Schemas.Add(null, xmlReader);
            }
            using (StringReader stringReader = new StringReader(Properties.Resources.HostTestData))
            {
                using XmlReader xmlReader = XmlReader.Create(stringReader, settings);
                try
                {
                    while (xmlReader.Read()) ;
                }
                catch (Exception exc)
                {
                    string message = (string.IsNullOrEmpty(exc.Message)) ? exc.ToString() : exc.Message;
                    validationErrors.Enqueue(message);
                    _logger.LogCritical(exc, "An unexpected exception was thrown.");
                }
            }
            Assert.That(validationErrors.IsEmpty, Is.True, () => string.Join($"{Environment.NewLine}", validationErrors.Select((s, i) => $"{i + 1}. {s}").ToArray()));
        }

        [Test, Property("Priority", 1), Ignore("Not necessary to always test")]
        public void FilePathTestDataChecks()
        {
            XDocument document = XDocument.Parse(Properties.Resources.FilePathTestData);
            Assert.That(document, Is.Not.Null, "FilePathTestData is null");
            Assert.That(document.Root, Is.Not.Null, "FilePathTestData.Root is null");
            XmlReaderSettings settings = new XmlReaderSettings
            {
                CheckCharacters = true,
                ConformanceLevel = ConformanceLevel.Document,
                DtdProcessing = DtdProcessing.Ignore,
                Schemas = new XmlSchemaSet(),
                ValidationFlags = XmlSchemaValidationFlags.ProcessIdentityConstraints | XmlSchemaValidationFlags.ReportValidationWarnings,
                ValidationType = ValidationType.Schema
            };
            ConcurrentQueue<string> validationErrors = new ConcurrentQueue<string>();
            settings.ValidationEventHandler += (object sender, ValidationEventArgs e) =>
            {
                if (e.Severity == XmlSeverityType.Warning)
                {
                    if (e.Exception is null)
                        _logger.LogWarning(e.Message);
                    else
                        _logger.LogWarning(e.Exception, e.Message);
                }
                else
                {
                    if (e.Exception is null)
                        validationErrors.Enqueue((string.IsNullOrWhiteSpace(e.Message)) ? "An unexpected validation exception has occurred" : e.Message);
                    else
                    {
                        string message = (string.IsNullOrEmpty(e.Message)) ? e.Exception.ToString() : e.Message;
                        validationErrors.Enqueue(message);
                        _logger.LogError(e.Exception, message);
                    }
                }
            };
            using (StringReader stringReader = new StringReader(Properties.Resources.FilePathTestDataSchema))
            {
                using XmlReader xmlReader = XmlReader.Create(stringReader);
                settings.Schemas.Add(null, xmlReader);
            }
            using (StringReader stringReader = new StringReader(Properties.Resources.FilePathTestData))
            {
                using XmlReader xmlReader = XmlReader.Create(stringReader, settings);
                try
                {
                    while (xmlReader.Read()) ;
                }
                catch (Exception exc)
                {
                    string message = (string.IsNullOrEmpty(exc.Message)) ? exc.ToString() : exc.Message;
                    validationErrors.Enqueue(message);
                    _logger.LogCritical(exc, "An unexpected exception was thrown.");
                }
            }
            Assert.That(validationErrors.IsEmpty, Is.True, () => string.Join($"{Environment.NewLine}", validationErrors.Select((s, i) => $"{i + 1}. {s}").ToArray()));
        }

        public static IEnumerable<TestCaseData> GetHostNameOrAddressForUriRegexTestCases() => _hostTestData.Items.OfType<DnsOrBasicHostName>().Select(testData =>
            new TestCaseData(testData.Address)
                .Returns(TestResultBuilder.CreateMatchResult(true)
                    .AppendMatchGroupFailed("ipv4")
                    .AppendMatchGroupFailed("ipv6")
                    .AppendMatchGroupResult("dns", testData.Value).ToTestResultString()))
            .Concat(_hostTestData.Items.OfType<IPV4HostAddress>().Select(testData => new TestCaseData(testData.Address)
                .Returns(TestResultBuilder.CreateMatchResult(true)
                    .AppendMatchGroupResult("ipv4", testData.Value)
                    .AppendMatchGroupFailed("ipv6")
                    .AppendMatchGroupFailed("dns").ToTestResultString())))
            .Concat(_hostTestData.Items.OfType<IPV6HostAddress>()
                .Select(testData =>
                    new TestCaseData(testData.Address)
                    .Returns(
                        (
                            (testData.IsUnc) ?
                                (testData.IsDns ?
                                    TestResultBuilder.CreateMatchResult(true)
                                        .AppendMatchGroupFailed("ipv4")
                                        .AppendMatchGroupFailed("ipv6")
                                        .AppendMatchGroupResult("dns", testData.Value) :
                                    TestResultBuilder.CreateMatchResult(false)
                                ) :
                                TestResultBuilder.CreateMatchResult(true)
                                    .AppendMatchGroupFailed("ipv4")
                                    .AppendMatchGroupResult("ipv6", testData.Value)
                                    .AppendMatchGroupFailed("dns")
                        ).ToTestResultString()
                    )
                )
            )
            .Concat(_hostTestData.Items.OfType<InvalidHostAddress>().Select(testData => new TestCaseData(testData.Address)
                .Returns(TestResultBuilder.CreateMatchResult(false).ToTestResultString())));

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetHostNameOrAddressForUriRegexTestCases))]
        public string HostNameOrAddressForUriRegexTest(string input)
        {
            Match match = FileUriConverter.HOST_NAME_OR_ADDRESS_FOR_URI_REGEX.Match(input);
            return match.CreateTestResultWithGroups("ipv4", "ipv6", "dns").ToTestResultString();
        }

        public static IEnumerable<TestCaseData> GetUriPathSegmentRegexTestCases()
        {
            return (new string[][]
            {
                new string[] { "/" },
                new string[] { "/", "Test" },
                new string[] { "/", "Test/" },
                new string[] { "/", "Test/", "Two" },
                new string[] { "/", "Test/", "Two/" },
                new string[] { "Test" },
                new string[] { "Test/" },
                new string[] { "Test/", "Two" },
                new string[] { "Test/", "Two/" }
            }).Select(expected => new TestCaseData(string.Join("", expected))
                .SetDescription($"\"{string.Join("\" + \"", expected)}\"")
                .Returns(expected)).Concat(new TestCaseData[]
                {
                    new TestCaseData("")
                        .SetDescription($"\"\"")
                        .Returns(Array.Empty<string>())
                });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetUriPathSegmentRegexTestCases))]
        public string[] UriPathSegmentRegexTest(string input)
        {
            MatchCollection matches = FileUriConverter.URI_PATH_SEGMENT_REGEX.Matches(input);
            return matches.Select(m => m.Value).ToArray();
        }

        public static IEnumerable<TestCaseData> GetPatternHostNameOrAddressTestCases() => _hostTestData.Items.OfType<DnsOrBasicHostName>().Select(testData =>
            new TestCaseData(testData.Address)
                .Returns(TestResultBuilder.CreateMatchResult(testData.Address).ToTestResultString()))
            .Concat(_hostTestData.Items.OfType<IPV4HostAddress>().Select(testData => new TestCaseData(testData.Address)
                .Returns(TestResultBuilder.CreateMatchResult(testData.Address).ToTestResultString())))
            .Concat(_hostTestData.Items.OfType<IPV6HostAddress>().Select(testData => new TestCaseData(testData.Address)
                .Returns(TestResultBuilder.CreateMatchResult(testData.Address).ToTestResultString())))
            .Concat(_hostTestData.Items.OfType<InvalidHostAddress>().Select(testData => new TestCaseData(testData.Address)
                .Returns(TestResultBuilder.CreateMatchResult(false).ToTestResultString())));

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetPatternHostNameOrAddressTestCases))]
        public string PatternHostNameOrAddressTest(string input)
        {
            Match match = Regex.Match(input, FileUriConverter.PATTERN_HOST_NAME_OR_ADDRESS);
            return match.CreateTestResult().ToTestResultString();
        }

        public static IEnumerable<TestCaseData> GetIpv6AddressRegexTestCases() => _hostTestData.Items.OfType<DnsOrBasicHostName>().Select(testData =>
            new TestCaseData(testData.Address)
                .Returns(TestResultBuilder.CreateMatchResult(false).ToTestResultString()))
            .Concat(_hostTestData.Items.OfType<IPV4HostAddress>().Select(testData => new TestCaseData(testData.Address)
                .Returns(TestResultBuilder.CreateMatchResult(false).ToTestResultString())))
            .Concat(_hostTestData.Items.OfType<IPV6HostAddress>().Select(testData => new TestCaseData(testData.Address)
                .Returns((!(testData.IsBracketed || testData.IsUnc) ? TestResultBuilder.CreateMatchResult(testData.Value) :
                    TestResultBuilder.CreateMatchResult(false)).ToTestResultString())))
            .Concat(_hostTestData.Items.OfType<InvalidHostAddress>().Select(testData => new TestCaseData(testData.Address)
                .Returns(TestResultBuilder.CreateMatchResult(false).ToTestResultString())));

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetIpv6AddressRegexTestCases))]
        public string Ipv6AddressRegexTest(string input)
        {
            Match match = FileUriConverter.IPV6_ADDRESS_REGEX.Match(input);
            return match.CreateTestResult().ToTestResultString();
        }

        public static IEnumerable<TestCaseData> GetPatternDnsNameTestCases() => _hostTestData.Items.OfType<DnsOrBasicHostName>().Select(testData =>
            new TestCaseData(testData.Address)
                .Returns(TestResultBuilder.CreateMatchResult(testData.Value).ToTestResultString()))
            .Concat(_hostTestData.Items.OfType<IPV4HostAddress>().Select(testData => new TestCaseData(testData.Address)
                .Returns(TestResultBuilder.CreateMatchResult(testData.Value).ToTestResultString())))
            .Concat(_hostTestData.Items.OfType<IPV6HostAddress>().Select(testData => new TestCaseData(testData.Address)
                .Returns(((testData.IsDns) ? TestResultBuilder.CreateMatchResult(testData.Value) :
                    TestResultBuilder.CreateMatchResult(false)).ToTestResultString())))
            .Concat(_hostTestData.Items.OfType<InvalidHostAddress>().Select(testData => new TestCaseData(testData.Address)
                .Returns(TestResultBuilder.CreateMatchResult(false).ToTestResultString())));

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetPatternDnsNameTestCases))]
        public string PatternDnsNameTest(string input)
        {
            Match match = Regex.Match(input, FileUriConverter.PATTERN_BASIC_OR_DNS_NAME);
            return match.CreateTestResult().ToTestResultString();
        }

        public static IEnumerable<TestCaseData> GetToFileSystemPathTestCases() => _testItems.Items.Select(testDataItem => testDataItem.Windows.AbsoluteUrl)
            .Where(u => !(u is null) && u.IsWellFormed && u.IsFileScheme())
            .Select(url => new TestCaseData(url.Owner.Owner.InputString, PlatformType.Windows, false) // Windows.AbsoluteUrl => PlatformType.Windows
                .SetArgDisplayNames($"fileUriString: {TestHelperExtensions.ToCsEscapedString(url.Owner.Owner.InputString)}", "platform: Windows", "allowAlt: false")
                .Returns(url.LocalPath.Path))
            .Concat(_testItems.Items.Select(testDataItem => testDataItem.Linux.AbsoluteUrl)
                .Where(u => !(u is null) && u.IsWellFormed && u.IsFileScheme())
                .Select(url => new TestCaseData(url.Owner.Owner.InputString, PlatformType.Linux, false) // Linux.AbsoluteUrl => PlatformType.Linux
                    .SetArgDisplayNames($"fileUriString: {TestHelperExtensions.ToCsEscapedString(url.Owner.Owner.InputString)}", "platform: Linux", "allowAlt: false")
                    .Returns(url.LocalPath.Path)))
            .Concat(_testItems.Items.Select(testDataItem => testDataItem.Windows.RelativeUrl)
                .Where(u => !(u is null) && u.IsWellFormed && u.GetAbsoluteUrl(true) is null)
                .Select(url => new TestCaseData(url.Owner.Owner.InputString, PlatformType.Windows, false) // Windows.RelativeUrl => PlatformType.Windows
                    .SetArgDisplayNames($"fileUriString: {TestHelperExtensions.ToCsEscapedString(url.Owner.Owner.InputString)}", "platform: Windows", "allowAlt: false")
                    .Returns(url.LocalPath.Path)))
            .Concat(_testItems.Items.Select(testDataItem => testDataItem.Linux.RelativeUrl)
                .Where(u => !(u is null) && u.IsWellFormed && u.GetAbsoluteUrl(true) is null)
                .Select(url => new TestCaseData(url.Owner.Owner.InputString, PlatformType.Linux, false) // Linux.RelativeUrl => PlatformType.Linux
                    .SetArgDisplayNames($"fileUriString: {TestHelperExtensions.ToCsEscapedString(url.Owner.Owner.InputString)}", "platform: Linux", "allowAlt: false")
                    .Returns(url.LocalPath.Path)))
            .Concat(_testItems.Items.Select(testDataItem => testDataItem.Windows.AbsoluteUrl)
                .Where(u => !(u is null) && u.IsWellFormed && u.IsFileScheme() && u.GetAltUrl(true) is null)
                .Select(url => new TestCaseData(url.Owner.Owner.InputString, PlatformType.Linux, true) // Windows.AbsoluteUrl => PlatformType.Linux/Windows
                    .SetArgDisplayNames($"fileUriString: {TestHelperExtensions.ToCsEscapedString(url.Owner.Owner.InputString)}", "platform: Linux", "allowAlt: true")
                    .Returns(url.LocalPath.Path)))
            .Concat(_testItems.Items.Select(testDataItem => testDataItem.Windows.RelativeUrl)
                .Where(u => !(u is null) && u.IsWellFormed && u.GetAltUrl(true) is null)
                .Select(url => new TestCaseData(url.Owner.Owner.InputString, PlatformType.Linux, true) // Windows.RelativeUrl => PlatformType.Linux/Windows
                    .SetArgDisplayNames($"fileUriString: {TestHelperExtensions.ToCsEscapedString(url.Owner.Owner.InputString)}", "platform: Linux", "allowAlt: true")
                    .Returns(url.LocalPath.Path)))
            .Concat(_testItems.Items.Select(testDataItem => testDataItem.Linux.AbsoluteUrl)
                .Where(u => !(u is null) && u.IsWellFormed && u.IsFileScheme() && u.GetAltUrl(true) is null)
                .Select(url => new TestCaseData(url.Owner.Owner.InputString, PlatformType.Windows, true) // Linux.AbsoluteUrl => PlatformType.Windows/Linux
                    .SetArgDisplayNames($"fileUriString: {TestHelperExtensions.ToCsEscapedString(url.Owner.Owner.InputString)}", "platform: Windows", "allowAlt: true")
                    .Returns(url.LocalPath.Path)))
            .Concat(_testItems.Items.Select(testDataItem => testDataItem.Linux.RelativeUrl)
                .Where(u => !(u is null) && u.IsWellFormed && u.GetAltUrl(true) is null)
                .Select(url => new TestCaseData(url.Owner.Owner.InputString, PlatformType.Windows, true) // Linux.RelativeUrl => PlatformType.Windows/Linux
                    .SetArgDisplayNames($"fileUriString: {TestHelperExtensions.ToCsEscapedString(url.Owner.Owner.InputString)}", "platform: Windows", "allowAlt: true")
                    .Returns(url.LocalPath.Path)));

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetToFileSystemPathTestCases))]
        public string ToFileSystemPathTest(string fileUriString, PlatformType platform, bool allowAlt)
        {
            string result = FileUriConverter.ToFileSystemPath(fileUriString, platform, allowAlt);
            return result;
        }
    }
}
