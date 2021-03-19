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
    [TestFixture]
    public class FileUriConverterTest
    {
        internal static readonly XDocument HostTestData;
        internal static readonly XDocument FilePathTestData;
        private static readonly ILogger<FileUriConverterTest> _logger;

        static FileUriConverterTest()
        {
            HostTestData = XDocument.Parse(Properties.Resources.HostTestData);
            FilePathTestData = XDocument.Parse(Properties.Resources.FilePathTestData);
            _logger = TestLogger.Create<FileUriConverterTest>();
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
            Assert.That(FilePathTestData, Is.Not.Null, "FilePathTestData is null");
            Assert.That(FilePathTestData.Root, Is.Not.Null, "FilePathTestData.Root is null");
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
            return HostTestData.Root.Elements().Select(hostElement =>
            {
                XElement expected;
                if (hostElement.Name.LocalName == "Invalid")
                    expected = new XElement("PatternHostName", new XAttribute("Success", false));
                else
                    expected = new XElement("PatternHostName", new XAttribute("Success", true), hostElement.Attribute("Address").Value);
                return new TestCaseData(hostElement.Attribute("Address").Value)
                    .Returns(expected.ToString(SaveOptions.DisableFormatting));
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetPatternHostNameTestCases))]
        public string PatternHostNameTest(string input)
        {
            Match match = Regex.Match(input, FileUriConverter.PATTERN_HOST_NAME_OR_ADDRESS);
            return UrlHelperTest.ToTestReturnValueXml(match, "PatternHostName");
        }

        public static IEnumerable<TestCaseData> GetIpv6AddressRegexTestCases()
        {
            return HostTestData.Root.Elements().Select(hostElement =>
            {
                XElement expected;
                if (hostElement.Name.LocalName == "IPV6" && hostElement.Attribute("Type").Value == "Normal")
                    expected = new XElement("Ipv6AddressRegex",
                        new XAttribute("Success", true),
                        new XElement("Group",
                            new XAttribute("Name", "ipv6"),
                            new XAttribute("Success", true),
                            hostElement.Value
                        )
                    );
                else
                    expected = new XElement("Ipv6AddressRegex", new XAttribute("Success", false));
                return new TestCaseData(hostElement.Attribute("Address").Value)
                    .Returns(expected.ToString(SaveOptions.DisableFormatting));
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetIpv6AddressRegexTestCases))]
        public string Ipv6AddressRegexTest(string input)
        {
            Match match = FileUriConverter.IPV6_ADDRESS_REGEX.Match(input);
            return UrlHelperTest.ToTestReturnValueXml(match, "Ipv6AddressRegex");
        }

        public static IEnumerable<TestCaseData> GetPatternDnsNameTestCases()
        {
            return HostTestData.Root.Elements().Select(hostElement =>
            {
                XElement expected;
                if (hostElement.Name.LocalName == "HostName" || hostElement.Name.LocalName == "IPV4" ||
                    hostElement.Attributes("IsDns").Any(a => XmlConvert.ToBoolean(a.Value)))
                    expected = new XElement("PatternDnsName", new XAttribute("Success", true), hostElement.Value);
                else
                    expected = new XElement("PatternDnsName", new XAttribute("Success", false));
                return new TestCaseData(hostElement.Attribute("Address").Value)
                    .Returns(expected.ToString(SaveOptions.DisableFormatting));
            });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetPatternDnsNameTestCases))]
        public string PatternDnsNameTest(string input)
        {
            Match match = Regex.Match(input, FileUriConverter.PATTERN_BASIC_OR_DNS_NAME);
            return UrlHelperTest.ToTestReturnValueXml(match, "PatternDnsName");
        }

        // TODO: Fix it so it's not generating hosts with port numbers
        public static IEnumerable<TestCaseData> GetToFileSystemPathTestCases()
        {
            return FilePathTestData.Root.Elements("TestData").Elements("Windows").Elements("AbsoluteUrl").Where(e => e.AttributeEquals("IsWellFormed", true) &&
                    e.Elements("Authority").Elements("Scheme").WhereAttributeEquals("Name", "file").Any())
                .Select(absoluteUrlElement =>
                    new TestCaseData(
                        absoluteUrlElement.Elements("Authority").Elements("Host").Attributes("Match").Select(a => a.Value).DefaultIfEmpty("").First(),
                        absoluteUrlElement.Elements("Path").Attributes("Match").Select(a => a.Value).DefaultIfEmpty("").First(),
                        PlatformType.Windows,
                        false
                    ).Returns(absoluteUrlElement.Elements("LocalPath").Select(e => e.Value).DefaultIfEmpty("").First())
                )
                .Concat(
                    FilePathTestData.Root.Elements("TestData").Elements("Windows").Where(e =>
                        !e.Elements("AbsoluteUrl").WhereAttributeEquals("IsWellFormed", true).Any()).Elements("RelativeUrl").WhereAttributeEquals("IsWellFormed", true)
                    .Select(e =>
                        new TestCaseData(
                            e.Elements("Authority").Elements("Host").Attributes("Match").Select(a => a.Value).DefaultIfEmpty("").First(),
                            e.Elements("Path").Attributes("Match").Select(a => a.Value).DefaultIfEmpty("").First(),
                            PlatformType.Windows,
                            false
                        ).Returns(e.Elements("LocalPath").Select(e => e.Value).DefaultIfEmpty("").First())
                    )
                )
                .Concat(
                    FilePathTestData.Root.Elements("TestData").Elements("Linux").Elements("AbsoluteUrl").WhereAttributeEquals("IsWellFormed", true)
                    .Select(e =>
                        new TestCaseData(
                            e.Elements("Authority").Elements("Host").Attributes("Match").Select(a => a.Value).DefaultIfEmpty("").First(),
                            e.Elements("Path").Attributes("Match").Select(a => a.Value).DefaultIfEmpty("").First(),
                            PlatformType.Linux,
                            false
                        ).Returns(e.Elements("LocalPath").Select(e => e.Value).DefaultIfEmpty("").First())
                    )
                )
                .Concat(
                    FilePathTestData.Root.Elements("TestData").Elements("Linux").Where(e =>
                        !e.Elements("AbsoluteUrl").WhereAttributeEquals("IsWellFormed", true).Any()).Elements("RelativeUrl").WhereAttributeEquals("IsWellFormed", true)
                    .Select(e =>
                        new TestCaseData(
                            e.Elements("Authority").Elements("Host").Attributes("Match").Select(a => a.Value).DefaultIfEmpty("").First(),
                            e.Elements("Path").Attributes("Match").Select(a => a.Value).DefaultIfEmpty("").First(),
                            PlatformType.Windows,
                            false
                        ).Returns(e.Elements("LocalPath").Select(e => e.Value).DefaultIfEmpty("").First())
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
