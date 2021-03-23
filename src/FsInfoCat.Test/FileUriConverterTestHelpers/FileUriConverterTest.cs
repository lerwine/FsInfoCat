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
using FsInfoCat.Test.FileUriConverterTestHelpers;

namespace FsInfoCat.Test
{
    [TestFixture()]
    public class FileUriConverterTest
    {
        internal static readonly XDocument HostTestData;
        internal static readonly XDocument FilePathTestDataXML;
        private ILogger<FileUriConverterTest> _logger;
        private FilePathTestData _testItems;

        [SetUp()]
        public void Init()
        {
            _logger = TestLogger.Create<FileUriConverterTest>();

            try
            {
                _testItems = FilePathTestData.Load();
            }
            catch (Exception exc)
            {
                _logger.LogCritical(exc, "Failed to load test items");
                throw;
            }
        }
        static FileUriConverterTest()
        {
            HostTestData = XDocument.Parse(Properties.Resources.HostTestData);
            FilePathTestDataXML = XDocument.Parse(Properties.Resources.FilePathTestData);
        }

        
        [Test, Property("Priority", 1)]
        public void HostTestDataChecks()
        {
            Assert.That(HostTestData, Is.Not.Null, "HostTestData is null");
            Assert.That(HostTestData.Root, Is.Not.Null, "HostTestData.Root is null");
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

        [Test, Property("Priority", 1)]
        public void FilePathTestDataChecks()
        {
            Assert.That(FilePathTestDataXML, Is.Not.Null, "FilePathTestData is null");
            Assert.That(FilePathTestDataXML.Root, Is.Not.Null, "FilePathTestData.Root is null");
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

        public static IEnumerable<TestCaseData> GetHostNameRegexTestCases()
        {
            return HostTestData.Root.Elements().Select(testData =>
            {
                switch (testData.Name.LocalName)
                {
                    case "HostName":
                        return new TestCaseData(testData.Attribute("Address").Value)
                            .Returns(
                                new XElement("HostNameRegex",
                                    new XAttribute("Success", true),
                                    new XElement("Group", new XAttribute("Name", "ipv4"), new XAttribute("Success", false)),
                                    new XElement("Group", new XAttribute("Name", "ipv6"), new XAttribute("Success", false)),
                                    new XElement("Group", new XAttribute("Name", "d"), new XAttribute("Success", false)),
                                    new XElement("Group", new XAttribute("Name", "dns"), testData.Value, new XAttribute("Success", true))
                                ).ToTestResultString()
                            );
                    case "IPV4":
                        return new TestCaseData(testData.Attribute("Address").Value)
                            .Returns(
                                new XElement("HostNameRegex",
                                    new XAttribute("Success", true),
                                    new XElement("Group", new XAttribute("Name", "ipv4"), testData.Value, new XAttribute("Success", true)),
                                    new XElement("Group", new XAttribute("Name", "ipv6"), new XAttribute("Success", false)),
                                    new XElement("Group", new XAttribute("Name", "d"), new XAttribute("Success", false)),
                                    new XElement("Group", new XAttribute("Name", "dns"), new XAttribute("Success", false))
                                ).ToTestResultString()
                            );
                    case "IPV6":
                        if (testData.Attribute("Type").Value == "UNC")
                        {
                            if (testData.Attribute("IsDns").Value == "true")
                                return new TestCaseData(testData.Attribute("Address").Value)
                                    .Returns(
                                        new XElement("HostNameRegex",
                                            new XAttribute("Success", true),
                                            new XElement("Group", new XAttribute("Name", "ipv4"), new XAttribute("Success", false)),
                                            new XElement("Group", new XAttribute("Name", "ipv6"), new XAttribute("Success", false)),
                                            new XElement("Group", new XAttribute("Name", "d"), new XAttribute("Success", false)),
                                            new XElement("Group", new XAttribute("Name", "dns"), testData.Value, new XAttribute("Success", true))
                                        ).ToTestResultString()
                                    );
                            return new TestCaseData(testData.Attribute("Address").Value)
                                .Returns(
                                    new XElement("HostNameRegex",
                                        new XAttribute("Success", false)
                                    ).ToTestResultString()
                                );

                        }
                        return new TestCaseData(testData.Attribute("Address").Value)
                            .Returns(
                                new XElement("HostNameRegex",
                                    new XAttribute("Success", true),
                                    new XElement("Group", new XAttribute("Name", "ipv4"), new XAttribute("Success", false)),
                                    new XElement("Group", new XAttribute("Name", "ipv6"), testData.Value, new XAttribute("Success", true)),
                                    new XElement("Group", new XAttribute("Name", "d"), new XAttribute("Success", false)),
                                    new XElement("Group", new XAttribute("Name", "dns"), new XAttribute("Success", false))
                                ).ToTestResultString()
                            );
                    default:
                        return new TestCaseData(testData.Attribute("Address").Value)
                            .Returns(
                                new XElement("HostNameRegex",
                                    new XAttribute("Success", false)
                                ).ToTestResultString()
                            );
                }
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetHostNameRegexTestCases))]
        public string HostNameRegexTest(string input)
        {
            Match match = FileUriConverter.HOST_NAME_OR_ADDRESS_FOR_URI_REGEX.Match(input);
            return UrlHelperTest.ToTestReturnValueXml(match, "HostNameRegex", "ipv4", "ipv6", "d", "dns");
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

        public static IEnumerable<TestCaseData> GetPatternHostNameTestCases()
        {
            return HostTestData.Root.Elements("Invalid").Select(element =>
                new TestCaseData(element.Attribute("Address").Value)
                    .Returns(TestResultBuilder.CreateMatchResult(false).ToTestResultString())
            )
            .Concat(
                HostTestData.Root.Elements("HostName").Concat(HostTestData.Root.Elements("IPV4")).Concat(HostTestData.Root.Elements("IPV6"))
                .Select(element =>
                    new TestCaseData(element.StringAttributeValue("Address", ""))
                        .Returns(TestResultBuilder.CreateMatchResult(element.StringAttributeValue("Address", ""))
                        .ToTestResultString())
                )
            );
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetPatternHostNameTestCases))]
        public string PatternHostNameTest(string input)
        {
            Match match = Regex.Match(input, FileUriConverter.PATTERN_HOST_NAME_OR_ADDRESS);
            return match.CreateTestResult().ToTestResultString();
        }

        public static IEnumerable<TestCaseData> GetIpv6AddressRegexTestCases()
        {
            return HostTestData.Root.Elements().Select(hostElement =>
            {
                XElement expected;
                if (hostElement.Name.LocalName == "IPV6" && hostElement.Attribute("Type").Value == "Normal")
                    expected = TestResultBuilder.CreateMatchResult(hostElement.Value);
                else
                    expected = TestResultBuilder.CreateMatchResult(false);
                return new TestCaseData(hostElement.Attribute("Address").Value)
                    .Returns(expected.ToTestResultString());
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetIpv6AddressRegexTestCases))]
        public string Ipv6AddressRegexTest(string input)
        {
            Match match = FileUriConverter.IPV6_ADDRESS_REGEX.Match(input);
            return match.CreateTestResult().ToTestResultString();
        }

        public static IEnumerable<TestCaseData> GetPatternDnsNameTestCases()
        {
            return HostTestData.Root.Elements().Select(hostElement =>
            {
                XElement expected;
                if (hostElement.Name.LocalName == "HostName" || hostElement.Name.LocalName == "IPV4" ||
                    hostElement.Attributes("IsDns").Any(a => XmlConvert.ToBoolean(a.Value)))
                    expected = TestResultBuilder.CreateMatchResult(hostElement.Value);
                else
                    expected = TestResultBuilder.CreateMatchResult(false);
                return new TestCaseData(hostElement.Attribute("Address").Value)
                    .Returns(expected.ToTestResultString());
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetPatternDnsNameTestCases))]
        public string PatternDnsNameTest(string input)
        {
            Match match = Regex.Match(input, FileUriConverter.PATTERN_BASIC_OR_DNS_NAME);
            return match.CreateTestResult().ToTestResultString();
        }

        public static IEnumerable<XElement> GetWindowsAbsoluteToFileSystemPathTestElements() =>
            FilePathTestDataXML.WindowsElements().AbsoluteUrlElements(e => e.IsWellFormed() && e.IsFileScheme());

        public static IEnumerable<XElement> GetWindowsRelativeToFileSystemPathTestElements() =>
            FilePathTestDataXML.WindowsElements(w => !w.AbsoluteUrlElement().IsWellFormed()).RelativeUrlElements(e => e.IsWellFormed());

        public static IEnumerable<XElement> GetLinuxAbsoluteToFileSystemPathTestElements() =>
            FilePathTestDataXML.LinuxElements().AbsoluteUrlElements(e => e.IsWellFormed() && e.IsFileScheme());

        public static IEnumerable<XElement> GetLinuxRelativeToFileSystemPathTestElements() =>
            FilePathTestDataXML.LinuxElements(w => !w.AbsoluteUrlElement().IsWellFormed()).RelativeUrlElements(e => e.IsWellFormed());

        // TODO: Fix it so it's not generating hosts with port numbers
        public static IEnumerable<TestCaseData> GetToFileSystemPathTestCases()
        {
            return GetWindowsAbsoluteToFileSystemPathTestElements()
                .Select(e =>
                    new TestCaseData(
                        e.HostElement().StringAttributeValue(FilePathTestDataExtensions.AttributeNameMatch, ""),
                        e.PathElement().StringAttributeValue(FilePathTestDataExtensions.AttributeNameMatch, ""),
                        PlatformType.Windows,
                        false
                        ).Returns(e.StringElementValue(FilePathTestDataExtensions.LocalPathElementName, ""))
                )
                .Concat(
                    GetWindowsRelativeToFileSystemPathTestElements()
                    .Select(e =>
                        new TestCaseData(
                            "",
                        e.PathElement().StringAttributeValue(FilePathTestDataExtensions.AttributeNameMatch, ""),
                            PlatformType.Windows,
                            false
                        ).Returns(e.StringElementValue(FilePathTestDataExtensions.LocalPathElementName, ""))
                    )
                )
                .Concat(
                    GetLinuxAbsoluteToFileSystemPathTestElements()
                    .Select(e =>
                        new TestCaseData(
                        e.HostElement().StringAttributeValue(FilePathTestDataExtensions.AttributeNameMatch, ""),
                        e.PathElement().StringAttributeValue(FilePathTestDataExtensions.AttributeNameMatch, ""),
                            PlatformType.Linux,
                            false
                        ).Returns(e.StringElementValue(FilePathTestDataExtensions.LocalPathElementName, ""))
                    )
                )
                .Concat(
                    GetLinuxRelativeToFileSystemPathTestElements()
                    .Select(e =>
                        new TestCaseData(
                            "",
                        e.PathElement().StringAttributeValue(FilePathTestDataExtensions.AttributeNameMatch, ""),
                            PlatformType.Windows,
                            false
                        ).Returns(e.StringElementValue(FilePathTestDataExtensions.LocalPathElementName, ""))
                    )
                );
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetToFileSystemPathTestCases))]
        public string ToFileSystemPathTest(string host, string path, PlatformType platform, bool allowAlt)
        {
            string result = FileUriConverter.ToFileSystemPath(host, path, platform, allowAlt);
            return result;
        }
    }
}
