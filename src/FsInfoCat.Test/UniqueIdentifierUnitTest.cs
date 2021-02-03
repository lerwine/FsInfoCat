using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using FsInfoCat.Models.Accounts;
using FsInfoCat.Models.Crawl;
using FsInfoCat.Models.DB;
using FsInfoCat.Models.Volumes;
using FsInfoCat.Util;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using NUnit.Framework.Interfaces;

namespace FsInfoCat.Test
{
    [TestFixture]
    public class UniqueIdentifierUnitTest
    {
        private static readonly string _hostName;
        private static readonly string _systemDrivePath;
        private static readonly string _systemDriveUrl;
        private static readonly string _ipV2Address;
        private static readonly string _ipV6Address;

        static UniqueIdentifierUnitTest()
        {
            _hostName = Environment.MachineName;
            _systemDrivePath = Path.GetPathRoot(Environment.SystemDirectory);
            _systemDriveUrl = new Uri(_systemDrivePath).AbsoluteUri;
            IPAddress[] addressList = Dns.GetHostEntry(_hostName).AddressList;
            _ipV2Address = addressList.Where(a => a.AddressFamily == AddressFamily.InterNetwork).Select(a => a.ToString()).DefaultIfEmpty("127.0.0.1").First();
            _ipV6Address = addressList.Where(a => a.AddressFamily == AddressFamily.InterNetworkV6).Select(a => a.ToString()).DefaultIfEmpty("::1").First().Split('%')[0];
        }

        private static IEnumerable<Tuple<uint, string, string>> GetSerialNumberTestValues()
        {
            yield return new Tuple<uint, string, string>(0x3B518D4BU, "3B51-8D4B",
                "Value of 995,200,331");
            yield return new Tuple<uint, string, string>(0x9E497DE8U, "9E49-7DE8",
                "Bit-wise equivalent to signed integer -1,639,350,808");
            yield return new Tuple<uint, string, string>(0U, "0000-0000",
                "Zero value");
            yield return new Tuple<uint, string, string>(1U, "0000-0001",
                "Value of 1");
            yield return new Tuple<uint, string, string>(0x80000000U, "8000-0000",
                "Bit-wise equivalent to Int32.MinValue");
            yield return new Tuple<uint, string, string>(0x7FFFFFFFU, "7FFF-FFFF",
                "Bit-wise equivalent to Int32.MaxValue");
            yield return new Tuple<uint, string, string>(0xFFFFFFFFU, "FFFF-FFFF",
                "Bit-wise equivalent to signed integer -1");
        }

        private static IEnumerable<Tuple<byte, string, string>> GetOrdinalTestValues()
        {
            yield return new Tuple<byte, string, string>(0, "00",
                "zero value");
            yield return new Tuple<byte, string, string>(1, "1",
                "Value of 1");
            yield return new Tuple<byte, string, string>(10, "0a",
                "Value of 10");
            yield return new Tuple<byte, string, string>(Byte.MaxValue, "ff",
                "Byte.MaxValue");
        }

        public static IEnumerable<TestCaseData> GetGuidParameterTestCases()
        {
            Guid guid = Guid.NewGuid();
            string url = $"urn:uuid:{guid.ToString("d").ToLower()}";
            yield return new TestCaseData(guid)
                .SetName("Random new guid #1")
                .Returns(new IdValues(url.Substring(4), url));

            guid = Guid.NewGuid();
            url = $"urn:uuid:{guid.ToString("d").ToLower()}";
            yield return new TestCaseData(guid)
                .SetName("Random new guid #2")
                .Returns(new IdValues(url.Substring(4), url));

            guid = Guid.Empty;
            url = $"urn:uuid:{guid.ToString("d").ToLower()}";
            yield return new TestCaseData(guid)
                .SetName("Empty guid")
                .Returns(new IdValues(url.Substring(4), url));
        }

        public static IEnumerable<TestCaseData> GetValidUUIDUriParameterTestCases()
        {
            Guid guid = Guid.NewGuid();
            string url = $"urn:uuid:{guid.ToString("B")}";
            string value = $"uuid:{guid.ToString("d").ToLower()}";
            string expected = $"urn:uuid:{guid.ToString("d").ToLower()}";
            yield return new TestCaseData(url)
                .SetName($"Random guid, brace format: {url}")
                .Returns(new UuidValues(guid, value, expected));

            url = $"urn:uuid:{guid.ToString("N")}";
            yield return new TestCaseData(url)
                .SetName($"Random guid, no-dash format: {url}")
                .Returns(new UuidValues(guid, value, expected));

            url = $"urn:uuid:{guid.ToString("P")}";
            yield return new TestCaseData(url)
                .SetName($"Random guid, parentheses format: {url}")
                .Returns(new UuidValues(guid, value, expected));

            url = $"urn:uuid:{guid.ToString("X")}";
            yield return new TestCaseData(url)
                .SetName($"Random guid, grouped hex format: {url}")
                .Returns(new UuidValues(guid, value, expected));

            url = $"urn:uuid:{guid.ToString("d").ToUpper()}";
            yield return new TestCaseData(url)
                .SetName($"Random upper-case guid: {url}")
                .Returns(new UuidValues(guid, value, expected));

            url = $"urn:uuid:{guid.ToString("d").ToLower()}";
            yield return new TestCaseData(url)
                .SetName($"Random new guid #1: {url}")
                .Returns(new UuidValues(guid, value, expected));

            url = $"urn:uuid:{guid.ToString("d").ToLower()}/";
            yield return new TestCaseData(url)
                .SetName($"Random new guid #1 with extraneous path: {url}")
                .Returns(new UuidValues(guid, value, expected));

            url = $"urn:uuid:{guid.ToString("d").ToLower()}#";
            yield return new TestCaseData(url)
                .SetName($"Random new guid #1 with empty fragment: {url}")
                .Returns(new UuidValues(guid, value, expected));

            url = $"urn:uuid:{guid.ToString("d").ToLower()}?";
            yield return new TestCaseData(url)
                .SetName($"Random new guid #1 with empty query: {url}")
                .Returns(new UuidValues(guid, value, expected));

            url = $"urn:uuid:{guid.ToString("d").ToLower()}/?#";
            yield return new TestCaseData(url)
                .SetName($"Random new guid #1 with extraneous path and empty query and fragment: {url}")
                .Returns(new UuidValues(guid, value, expected));

            guid = Guid.NewGuid();
            url = $"urn:uuid:{guid.ToString("d").ToLower()}";
            yield return new TestCaseData(url)
                .SetName($"Random new guid #2: {url}")
                .Returns(new UuidValues(guid, url.Substring(4), url));

            guid = Guid.Empty;
            url = $"urn:uuid:{guid.ToString("d").ToLower()}";
            yield return new TestCaseData(url)
                .SetName($"Empty guid: {url}")
                .Returns(new UuidValues(guid, url.Substring(4), url));
        }

        public static IEnumerable<TestCaseData> GetSerialNumberParameterTestCases() =>
            GetSerialNumberTestValues().Select(v => new {
                Arg = v.Item1,
                Url = $"urn::volume:id:{v.Item2}",
                Name = v.Item3
            }).Select(a => new TestCaseData(a.Arg)
                .SetName($"{a.Name}: {a.Arg}")
                .Returns(new IdValues(a.Url.Substring(4), a.Url)));

        public static IEnumerable<TestCaseData> GetSerialNumberAndOrdinalParametersTestCases() =>
            GetSerialNumberTestValues().Select(sn => new
            {
                SerialNumber = sn.Item1,
                ReturnsValue = sn.Item2.Replace("-", ""),
                Description = sn.Item3
            }).Select(a => new
            {
                SerialNumber = a.SerialNumber,
                ReturnsValue = a.ReturnsValue,
                ReturnsUri = $"urn:volume:id:{a.ReturnsValue}",
                Description = a.Description
            }).SelectMany(sn => GetOrdinalTestValues().Select(ord =>
                new TestCaseData(sn.SerialNumber, ord.Item1)
                    .SetName($"serialNumber = {sn.Description} ({sn.SerialNumber}), Ordinal = {ord.Item3} ({ord.Item1})")));

        public static IEnumerable<TestCaseData> GetValidSerialNumberUriParameterTestCases()
        {
            var snTestValues = GetSerialNumberTestValues().Select(t => new
            {
                Arg = $"urn:volume:id:{t.Item2}",
                Description = $"serialNumber: {t.Item3}",
                Returns = new SnIdValues(t.Item1, null, t.Item2, $"urn:volume:id:{t.Item2}")
            }).SelectMany(sn => (new[] { sn }).Concat(GetOrdinalTestValues().Select(t => new
            {
                Arg = $"{sn.Arg.Replace("-", "")}-{t.Item2}",
                Description = $"{sn.Description}, ordinal: {t.Item3}",
                Returns = new SnIdValues(sn.Returns.SerialNumber, t.Item1, $"{sn.Returns.Value.Replace("-", "")}-{t.Item2}",
                    $"{sn.Returns.AbsoluteUri.Replace("-", "")}-{t.Item2}")
            }))).Concat(GetSerialNumberTestValues().Where(t => t.Item2.ToUpper() != t.Item2.ToLower()).Take(2).Select(t => new
            {
                Arg = $"urn:VOLUME:ID:{t.Item2.ToUpper()}",
                Description = $"(upper case) serialNumber: {t.Item3}",
                Returns = new SnIdValues(t.Item1, null, t.Item2, $"urn:volume:id:{t.Item2}")
            }).SelectMany(sn => (new[] { sn }).Concat(GetOrdinalTestValues().Where(t => t.Item2.ToUpper() != t.Item2.ToLower()).Take(2).Select(t => new
            {
                Arg = $"{sn.Arg.Replace("-", "")}-{t.Item2.ToUpper()}",
                Description = $"{sn.Description}, ordinal: {t.Item3}",
                Returns = new SnIdValues(sn.Returns.SerialNumber, t.Item1, $"{sn.Returns.Value.Replace("-", "")}-{t.Item2}",
                    $"{sn.Returns.AbsoluteUri.Replace("-", "")}-{t.Item2}")
            }))));

            return snTestValues.Concat(snTestValues.GroupBy(t => t.Returns.SerialNumber).SelectMany(g =>
                g.Where(s => !s.Returns.Ordinal.HasValue && !s.Description.StartsWith("(")).Take(1)
                    .Concat(g.Where(s => s.Returns.Ordinal.HasValue && !s.Description.StartsWith("(")).Take(1))
                    .Concat(g.Where(s => !s.Returns.Ordinal.HasValue && s.Description.StartsWith("(")).Take(1))
                    .Concat(g.Where(s => s.Returns.Ordinal.HasValue && s.Description.StartsWith("(")).Take(1))
            ).Select(a => (a.Description.StartsWith("(upper case)")) ?
                new
                {
                    Arg = a.Arg,
                    DescriptionStart = "(upper case; ",
                    Description = a.Description.Substring(11),
                    Returns = a.Returns
                }
                : new
                {
                    Arg = a.Arg,
                    DescriptionStart = "(",
                    Description = $") {a.Description}",
                    Returns = a.Returns
                }
            ).SelectMany(a => new[]
            {
                new
                {
                    Arg = $"{a.Arg}/",
                    Description = $"{a.DescriptionStart}trailing slash{a.Description}",
                    Returns = new SnIdValues(a.Returns.SerialNumber, a.Returns.Ordinal, a.Returns.Value, a.Returns.AbsoluteUri)
                },
                new
                {
                    Arg = $"{a.Arg}?",
                    Description = $"{a.DescriptionStart}empty query{a.Description}",
                    Returns = new SnIdValues(a.Returns.SerialNumber, a.Returns.Ordinal, a.Returns.Value, a.Returns.AbsoluteUri)
                },
                new
                {
                    Arg = $"{a.Arg}?",
                    Description = $"{a.DescriptionStart}empty fragment{a.Description}",
                    Returns = new SnIdValues(a.Returns.SerialNumber, a.Returns.Ordinal, a.Returns.Value, a.Returns.AbsoluteUri)
                },
                new
                {
                    Arg = $"{a.Arg}/?#",
                    Description = $"{a.DescriptionStart}trailing slash; empty query and fragment{a.Description}",
                    Returns = new SnIdValues(a.Returns.SerialNumber, a.Returns.Ordinal, a.Returns.Value, a.Returns.AbsoluteUri)
                },
            })).Select(a => new TestCaseData(a.Arg).SetName(a.Description).Returns(a.Returns));
        }

        public static IEnumerable<TestCaseData> GetValidFilePathParameterTestCases()
        {
            string path = @"\\servicenowdiag479.file.core.windows.net\testWshare";
            string expected = $@"{path}\";
            string url = "file://servicenowdiag479.file.core.windows.net/testazureshare/";
            yield return new TestCaseData(path)
                .SetName($"URN with fully qualified machine name: {path}")
                .Returns(new IdValues(expected, url));

            yield return new TestCaseData(expected)
                .SetName($"URN with fully qualified machine name and trailing backslash: {expected}")
                .Returns(new IdValues(expected, url));

            path = $@"\\{_hostName}\$Admin";
            expected = $@"{path}\";
            url = $"file://{_hostName}/$Admin/";
            yield return new TestCaseData(path)
                .SetName($"URN with machine name only: {path}")
                .Returns(new IdValues(expected, url));

            path = $@"\\{_hostName}\Admin$\";
            yield return new TestCaseData(expected)
                .SetName($"URN with machine name only and trailing backslash: {expected}")
                .Returns(new IdValues(expected, url));

            path = $@"\\{_ipV2Address}\Us&Them";
            expected = $@"{path}\";
            url = $"file://{_ipV2Address}/Us&Them/";
            yield return new TestCaseData(path)
                .SetName($"URN with IPV2 address: {path}")
                .Returns(new IdValues(expected, url));

            path = $@"\\{_ipV2Address}\Us&Them\";
            yield return new TestCaseData(expected)
                .SetName($"URN with IPV2 address and trailing backslash: {expected}")
                .Returns(new IdValues(expected, url));

            path = $@"\\[{_ipV6Address}]\100% Done";
            expected = $@"{path}\";
            url = $"file://[{_ipV6Address}]/100%25%20Done/";
            yield return new TestCaseData(path)
                .SetName($"URN with IPV6 address: {path}")
                .Returns(new IdValues(expected, url));

            path = $@"\\[{_ipV6Address}]\100% Done\";
            yield return new TestCaseData(expected)
                .SetName($"URN with IPV6 address and trailing backslash: {expected}")
                .Returns(new IdValues(expected, url));
        }

        public static IEnumerable<TestCaseData> GetValidFileUrlParameterTestCases()
        {
            string url = "file://servicenowdiag479.file.core.windows.net/testazureshare";
            string expected = $"{url}/";
            string path = @"\\\servicenowdiag479.file.core.windows.net\testazureshare\";
            yield return new TestCaseData(url)
                .SetName($"URN with fully qualified machine name: {url}")
                .Returns(new IdValues(path, expected));

            yield return new TestCaseData(expected)
                .SetName($"URN with fully qualified machine name and trailing slash: {expected}")
                .Returns(new IdValues(path, expected));

            string noTrailingSlash = url;
            url = $"{noTrailingSlash}?";
            yield return new TestCaseData(url)
                .SetName($"URN with fully qualified machine name and empty query: {url}")
                .Returns(new IdValues(path, expected));

            url = $"{expected}?";
            yield return new TestCaseData(url)
                .SetName($"URN with fully qualified machine name, trailing slash and empty query: {url}")
                .Returns(new IdValues(path, expected));

            url = $"{noTrailingSlash}#";
            yield return new TestCaseData(url)
                .SetName($"URN with fully qualified machine name and empty fragment: {url}")
                .Returns(new IdValues(path, expected));

            url = $"{expected}#";
            yield return new TestCaseData(url)
                .SetName($"URN with fully qualified machine name, trailing slash and empty fragment: {url}")
                .Returns(new IdValues(path, expected));

            url = $"{noTrailingSlash}?#";
            yield return new TestCaseData(url)
                .SetName($"URN with fully qualified machine name, empty query and empty fragment: {url}")
                .Returns(new IdValues(path, expected));

            url = $"{expected}?#";
            yield return new TestCaseData(url)
                .SetName($"URN with fully qualified machine name, trailing slash, empty query and empty fragment: {url}")
                .Returns(new IdValues(path, expected));

            url = $"file://{_hostName}/$Admin";
            expected = $"{url}/";
            path = $@"\\{_hostName}\$Admin\";
            yield return new TestCaseData(url)
                .SetName($"URN with machine name only: {url}")
                .Returns(new IdValues(path, expected));

            yield return new TestCaseData(expected)
                .SetName($"URN with machine name only and trailing slash: {expected}")
                .Returns(new IdValues(path, expected));

            url = $"file://{_ipV2Address}/Us&Them";
            expected = $"{url}/";
            path = $@"\\{_ipV2Address}\Us&Them\";
            yield return new TestCaseData(url)
                .SetName($"URN with IPV2 address: {url}")
                .Returns(new IdValues(path, expected));

            yield return new TestCaseData(expected)
                .SetName($"URN with IPV2 address and trailing slash: {expected}")
                .Returns(new IdValues(path, expected));

            url = $"file://[{_ipV6Address}]/100%25%20Done";
            expected = $"{url}/";
            path = $@"\\{_hostName}\100% Done\";
            yield return new TestCaseData(url)
                .SetName($"URN with IPV6 address: {url}")
                .Returns(new IdValues(path, expected));

            yield return new TestCaseData(expected)
                .SetName($"URN with IPV6 address and trailing slash: {expected}")
                .Returns(new IdValues(path, expected));
        }

        [SetUp]
        public void Setup()
        {
        }

        public static IEnumerable<TestCaseData> GetToNormalizedUriTestCases()
        {
            string expected = $"file://[{_ipV6Address}]/100%25%20Done";
            yield return new TestCaseData(new Uri(expected))
                .SetName($"File URN with no trailing slash and noTrailingSlash = true: {expected}")
                .Returns(new Uri(expected));
#warning Need to implmeent more test data
        }

        public static IEnumerable<TestCaseData> GetGuidToIdentifierStringTestCases()
        {
            Guid guid = Guid.NewGuid();
            yield return new TestCaseData(guid)
                .SetName($"Random Guid: {guid}")
                .Returns(guid.ToString("d").ToLower());

            guid = Guid.Empty;
            yield return new TestCaseData(guid)
                .SetName($"Empty Guid: {guid}")
                .Returns(guid.ToString("d").ToLower());
        }

        public static IEnumerable<TestCaseData> GetGuidToUrnTestCases()
        {
            Guid guid = Guid.NewGuid();
            yield return new TestCaseData(guid)
                .SetName($"Random Guid: {guid}")
                .Returns($"urn:uuid:{guid.ToString("d").ToLower()}");

            guid = Guid.Empty;
            yield return new TestCaseData(guid)
                .SetName($"Empty Guid: {guid}")
                .Returns($"urn:uuid:{guid.ToString("d").ToLower()}");
        }

        public static IEnumerable<TestCaseData> GetSerialNumberToIdentifierStringTestCases() => GetSerialNumberTestValues()
            .Select(t => new TestCaseData(t.Item1, (byte?)null).SetName($"serialNumber: {t.Item3}")
                .Returns(t.Item2)).Concat(GetSerialNumberTestValues().SelectMany(sn =>
                GetOrdinalTestValues().Select(ord => new TestCaseData(sn.Item1, ord.Item1)
                    .SetName($"serialNumber: {sn.Item3}, ordinal: {ord.Item3}")
                    .Returns($"{sn.Item2.Replace("-", "")}-{ord.Item2}"))
            ));

        public static IEnumerable<TestCaseData> GetSerialNumberToUrnTestCases() => GetSerialNumberTestValues()
            .Select(t => new TestCaseData(t.Item1, (byte?)null).SetName($"serialNumber: {t.Item3}")
                .Returns($"urn:volume:id:{t.Item2}")).Concat(GetSerialNumberTestValues().SelectMany(sn =>
                GetOrdinalTestValues().Select(ord => new TestCaseData(sn.Item1, ord.Item1)
                    .SetName($"serialNumber: {sn.Item3}, ordinal: {ord.Item3}")
                    .Returns($"urn:volume:id:{sn.Item2.Replace("-", "")}-{ord.Item2}"))
            ));

        [Test]
        [Property("Priority", 1)]
        [TestCaseSource("GetToNormalizedUriTestCases")]
        public Uri ToNormalizedUriTest(Uri url, bool noTrailingSlash)
        {
            return UniqueIdentifier.ToNormalizedUri(url, noTrailingSlash);
        }

        [Test]
        [Property("Priority", 1)]
        [TestCaseSource("GetGuidToIdentifierStringTestCases")]
        public string GuidToIdentifierStringTest(Guid guid)
        {
            return UniqueIdentifier.ToIdentifierString(guid);
        }

        [Test]
        [Property("Priority", 1)]
        [TestCaseSource("GetSerialNumberToIdentifierStringTestCases")]
        public string SerialNumberToIdentifierStringTest(uint serialNumber, byte? ordinal)
        {
            if (ordinal.HasValue)
                return UniqueIdentifier.ToIdentifierString(serialNumber, ordinal.Value);
            return UniqueIdentifier.ToIdentifierString(serialNumber);
        }

        [Test]
        [Property("Priority", 2)]
        [TestCaseSource("GetGuidToUrnTestCases")]
        public string GuidToUrnTest(Guid guid)
        {
            return UniqueIdentifier.ToUrn(guid);
        }

        [Test]
        [Property("Priority", 2)]
        [TestCaseSource("GetSerialNumberToUrnTestCases")]
        public string SerialNumberToUrnTest(uint serialNumber, byte? ordinal)
        {
            if (ordinal.HasValue)
                return UniqueIdentifier.ToUrn(serialNumber, ordinal.Value);
            return UniqueIdentifier.ToUrn(serialNumber);
        }

        [Test]
        [Property("Priority", 1)]
        [TestCaseSource("GetParseUrnTestCases")]
        public IdValues ParseUrnTest(Guid guid)
        {
#warning Need to implement ParseUrnTest(Guid)
            throw new NotImplementedException();
        }

        [Test]
        [Property("Priority", 3)]
        [TestCaseSource("GetGuidParameterTestCases")]
        public IdValues FromGuidValueConstructorTest(Guid guid)
        {
            UniqueIdentifier target = new UniqueIdentifier(guid);
            Assert.That(target.URL.IsAbsoluteUri, Is.True);
            Assert.That(target.URL.IsFile, Is.False);
            Assert.That(target.URL.Scheme, Is.EqualTo("urn"));
            Assert.That(target.URL.Fragment, Is.Empty);
            Assert.That(target.UUID.HasValue, Is.True);
            Assert.That(target.UUID.Value, Is.EqualTo(guid));
            Assert.That(target.SerialNumber.HasValue, Is.False);
            Assert.That(target.Ordinal.HasValue, Is.False);
            // Assert.That(target.URL.AbsolutePath, Is.EqualTo(expected));
            // Assert.That(target.URL.PathAndQuery, Is.EqualTo(expected));
            return new IdValues(target.Value, target.URL.AbsoluteUri);
        }

        [Test]
        [Property("Priority", 3)]
        [TestCaseSource("GetValidUUIDUriParameterTestCases")]
        public UuidValues FromValidUUIDUriConstructorTest(string uri)
        {
            UniqueIdentifier target = new UniqueIdentifier(uri);
            Assert.That(target.URL.IsAbsoluteUri, Is.True);
            Assert.That(target.URL.IsFile, Is.False);
            Assert.That(target.URL.Scheme, Is.EqualTo("urn"));
            Assert.That(target.URL.Fragment, Is.Empty);
            // string expected = uri.Split('/', '?', '#')[0].ToLower();
            // Assert.That(target.URL.AbsoluteUri, Is.EqualTo(expected));
            Assert.That(target.SerialNumber.HasValue, Is.False);
            Assert.That(target.Ordinal.HasValue, Is.False);
            // expected = expected.Substring(4);
            // Assert.That(Guid.TryParse(expected.Substring(5), out Guid guid), Is.True, $"Failed to parse '{expected}'");
            // expected = $"uuid:{guid.ToString("d")}";
            // Assert.That(target.URL.AbsolutePath, Is.EqualTo(expected));
            // Assert.That(target.URL.PathAndQuery, Is.EqualTo(expected));
            // Assert.That(target.Value, Is.EqualTo(expected.Substring(5)));
            return new UuidValues(target.UUID, target.Value, target.URL.AbsoluteUri);
        }

        [Test]
        [Property("Priority", 3)]
        [TestCaseSource("GetSerialNumberParameterTestCases")]
        public IdValues FromSerialNumberValueConstructorTest(uint serialNumber)
        {
            UniqueIdentifier target = new UniqueIdentifier(serialNumber);
            Assert.That(target.URL.IsAbsoluteUri, Is.True);
            Assert.That(target.URL.IsFile, Is.False);
            Assert.That(target.URL.Scheme, Is.EqualTo("urn"));
            Assert.That(target.URL.Fragment, Is.Empty);
            Assert.That(target.SerialNumber.HasValue, Is.True);
            Assert.That(target.SerialNumber.Value, Is.EqualTo(serialNumber));
            Assert.That(target.UUID.HasValue, Is.False);
            Assert.That(target.Ordinal.HasValue, Is.False);
            // string expected = serialNumber.ToString("x8");
            // expected = $"{expected.Substring(0, 4)}-{expected.Substring(4)}";
            // Assert.That(target.Value, Is.EqualTo(expected));
            // expected = $"volume:id:{expected}";
            // Assert.That(target.URL.AbsolutePath, Is.EqualTo(expected));
            // Assert.That(target.URL.PathAndQuery, Is.EqualTo(expected));
            return new IdValues(target.Value, target.URL.AbsoluteUri);
        }

        [Test]
        [Property("Priority", 3)]
        [TestCaseSource("GetSerialNumberAndOrdinalParametersTestCases")]
        public IdValues FromSerialNumberAndOrdinalValuesConstructorTest(uint serialNumber, byte ordinal)
        {
            UniqueIdentifier target = new UniqueIdentifier(serialNumber, ordinal);
            Assert.That(target.URL.IsAbsoluteUri, Is.True);
            Assert.That(target.URL.IsFile, Is.False);
            Assert.That(target.URL.Scheme, Is.EqualTo("urn"));
            Assert.That(target.URL.Fragment, Is.Empty);
            Assert.That(target.SerialNumber.HasValue, Is.True);
            Assert.That(target.SerialNumber.Value, Is.EqualTo(serialNumber));
            Assert.That(target.UUID.HasValue, Is.False);
            Assert.That(target.Ordinal.HasValue, Is.False);
            // string expected = $"{serialNumber.ToString("x8")}-{ordinal}";
            // Assert.That(target.Value, Is.EqualTo(expected));
            // expected = $"volume:id:{expected}";
            // Assert.That(target.URL.AbsolutePath, Is.EqualTo(expected));
            // Assert.That(target.URL.PathAndQuery, Is.EqualTo(expected));
            return new IdValues(target.Value, target.URL.AbsoluteUri);
        }

        [Test]
        [Property("Priority", 3)]
        [TestCaseSource("GetValidSerialNumberUriParameterTestCases")]
        public SnIdValues FromValidSerialNumberAndOrdinalUriConstructorTest(string uri)
        {
            UniqueIdentifier target = new UniqueIdentifier(uri);
            Assert.That(target.URL.IsAbsoluteUri, Is.True);
            Assert.That(target.URL.IsFile, Is.False);
            Assert.That(target.URL.Scheme, Is.EqualTo("urn"));
            Assert.That(target.URL.Fragment, Is.Empty);
            // string expected = uri.Split('/', '?', '#')[0];
            // Assert.That(target.URL.AbsoluteUri, Is.EqualTo(expected));
            Assert.That(target.SerialNumber.HasValue, Is.True);
            Assert.That(target.UUID.HasValue, Is.False);
            // expected = expected.Substring(4);
            // Assert.That(target.URL.AbsolutePath, Is.EqualTo(expected));
            // Assert.That(target.URL.PathAndQuery, Is.EqualTo(expected));
            // Assert.That(target.Value, Is.EqualTo(expected.Substring(10)));
            return new SnIdValues(target.SerialNumber, target.Ordinal, target.Value, target.URL.AbsoluteUri);
        }

        [Test]
        [Property("Priority", 3)]
        [TestCaseSource("GetValidFilePathParameterTestCases")]
        public IdValues FromValidFilePathConstructorTest(string path)
        {
            UniqueIdentifier target = new UniqueIdentifier(path);
            Assert.That(target.URL.IsAbsoluteUri, Is.True);
            Assert.That(target.URL.IsFile, Is.True);
            Assert.That(target.URL.Scheme, Is.EqualTo("file"));
            Assert.That(target.URL.Fragment, Is.Empty);
            Assert.That(target.SerialNumber.HasValue, Is.False);
            Assert.That(target.UUID.HasValue, Is.False);
            Assert.That(target.Ordinal.HasValue, Is.False);
            // string expected = (path.EndsWith("\\")) ? path : path + "\\";
            // Assert.That(target.Value, Is.EqualTo(expected));
            // expected = new Uri(expected).AbsolutePath;
            // Assert.That(target.URL.AbsolutePath, Is.EqualTo(expected));
            // Assert.That(target.URL.PathAndQuery, Is.EqualTo(expected));
            return new IdValues(target.Value, target.URL.AbsoluteUri);
        }

        [Test]
        [Property("Priority", 3)]
        [TestCaseSource("GetValidFileUrlParameterTestCases")]
        public IdValues FromValidFileUrlConstructorTest(string uri)
        {
            UniqueIdentifier target = new UniqueIdentifier(uri);
            Assert.That(target.URL.IsAbsoluteUri, Is.True);
            Assert.That(target.URL.IsFile, Is.True);
            Assert.That(target.URL.Scheme, Is.EqualTo("file"));
            // string expected = uri.Split('?', '#')[0];
            // if (!uri.EndsWith("/"))
            //     uri += "/";
            // Assert.That(target.URL.AbsoluteUri, Is.EqualTo(expected));
            Assert.That(target.URL.Fragment, Is.Empty);
            Assert.That(target.SerialNumber.HasValue, Is.False);
            Assert.That(target.UUID.HasValue, Is.False);
            Assert.That(target.Ordinal.HasValue, Is.False);
            // Uri url = new Uri(uri);
            // expected = url.AbsolutePath;
            // Assert.That(target.URL.AbsolutePath, Is.EqualTo(expected));
            // Assert.That(target.URL.PathAndQuery, Is.EqualTo(expected));
            Assert.That(target.Value, Is.EqualTo(target.URL.LocalPath));
            return new IdValues(target.Value, target.URL.AbsolutePath);
        }

        [Test]
        [Property("Priority", 3)]
        public void FromInvalidUriConstructorTest()
        {
            Assert.That(() => { new UniqueIdentifier(null); }, Throws.ArgumentNullException
                .With.Property("ParamName").EqualTo("uriString"));
            Assert.That(() => { new UniqueIdentifier(_systemDrivePath); }, Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("uriString")
                .With.Property("Message").EqualTo("Invalid host name or path type (Parameter 'uriString')"));
            Assert.That(() => { new UniqueIdentifier(_systemDriveUrl); }, Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("uriString")
                .With.Property("Message").EqualTo("Invalid host name or path type (Parameter 'uriString')"));
            Assert.That(() => { new UniqueIdentifier("file:///"); }, Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("uriString")
                .With.Property("Message").EqualTo("Invalid host name or path type (Parameter 'uriString')"));
        }

        [Test]
        [Property("Priority", 3)]
        public void FromInvalidUriFormatConstructorTest([Values("", " ", ".", "urn")] string uri)
        {
            Assert.That(() => { new UniqueIdentifier(uri); }, Throws.InstanceOf<UriFormatException>());
        }

        [Test]
        [Property("Priority", 3)]
        public void FromInvalidUrnNamespaceConstructorTest([Values("urn:", "urn:id")] string uri)
        {
            Assert.That(() => { new UniqueIdentifier(uri); }, Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("uriString")
                .With.Property("Message").EqualTo("Unsupported URN namespace (Parameter 'uriString')"));
        }

        [Test]
        [Property("Priority", 3)]
        public void FromInvalidUUIDUrnNamespaceConstructorTest([Values(
            "urn:uuid", "urn:uuid:", "urn:uuid:fb4f6ff1-7ffe-4eef-840a-1df28bb2e178:2", "urn:uuid:fb4f6ff1-7ffe-4eef-840a-1df28bb2e17"
        )] string uri)
        {
            Assert.That(() => { new UniqueIdentifier(uri); }, Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("uriString")
                .With.Property("Message").EqualTo("Invalid UUID URI format (Parameter 'uriString')"));
        }

        [Test]
        [Property("Priority", 3)]
        public void FromInvalidVolumeIdUrnNamespaceConstructorTest([Values(
            "urn:volume", "urn:volume:", "urn:volume:id", "urn:volume:id:", "urn:volume:id:3B51-8D4B-1", "urn:volume:id:3B518D4B-"
        )] string uri)
        {
            Assert.That(() => { new UniqueIdentifier(uri); }, Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("uriString")
                .With.Property("Message").EqualTo("Invalid volume identifier URI format (Parameter 'uriString')"));
        }

        [Test]
        [Property("Priority", 3)]
        public void FromInvalidUriSchemeNamespaceConstructorTest([Values(
            "http://tempuri.org", "mailto:volume@id.3B51-8D4B"
        )] string uri)
        {
            Assert.That(() => { new UniqueIdentifier(uri); }, Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("uriString")
                .With.Property("Message").EqualTo("Unsupported URI scheme (Parameter 'uriString')"));
        }

        public class IdValues : IEquatable<IdValues>
        {
            public string Value { get; }

            public string AbsoluteUri { get; }

            public IdValues(string value, string absoluteUri)
            {
                Value = value;
                AbsoluteUri = absoluteUri;
            }

            public bool Equals([AllowNull] IdValues other) => null != other && (ReferenceEquals(this, other) ||
                (AbsoluteUri == other.AbsoluteUri && Value == other.Value));

            public override bool Equals(object obj) => Equals(obj as IdValues);

            public override int GetHashCode() => HashCode.Combine(AbsoluteUri, Value);

            public override string ToString() => (AbsoluteUri is null) ?
                ((Value is null) ?
                    "{ Value = null, AbsoluteUri = null }"
                    : $"{{ Value = \"{Value}\", AbsoluteUri = null }}"
                )
                : ((Value is null) ?
                    $"{{ Value = null, AbsoluteUri = \"{AbsoluteUri}\" }}"
                    : $"{{ Value = \"{Value}\", AbsoluteUri = \"{AbsoluteUri}\" }}"
                );
        }

        public class SnIdValues : IdValues, IEquatable<SnIdValues>
        {
            public uint? SerialNumber { get; }
            public byte? Ordinal { get; }

            public SnIdValues(uint? serialNumber, byte? ordinal, string value, string absoluteUri) : base(value, absoluteUri)
            {
                SerialNumber = serialNumber;
                Ordinal = ordinal;
            }

            public bool Equals([AllowNull] SnIdValues other) => null != other && (ReferenceEquals(this, other) ||
                (base.Equals(this) && SerialNumber == other.SerialNumber && Ordinal == other.Ordinal));

            public override bool Equals(object obj) => Equals(obj as SnIdValues);

            public override int GetHashCode() => HashCode.Combine(SerialNumber, Ordinal, AbsoluteUri, Value);

            public override string ToString()
            {
                return (SerialNumber.HasValue) ?
                    ((Ordinal.HasValue) ?
                        $"{{ SerialNumber = {SerialNumber.Value.ToString("x4")}, Ordinal = {Ordinal.Value.ToString("x2")},{base.ToString().Substring(1)}"
                        : $"{{ SerialNumber = {SerialNumber.Value.ToString("x4")}, Ordinal = null,{base.ToString().Substring(1)}"
                    )
                    : ((Ordinal.HasValue) ?
                        $"{{ SerialNumber = null, Ordinal = {Ordinal.Value.ToString("x2")},{base.ToString().Substring(1)}"
                        : $"{{ SerialNumber = null, Ordinal = null,{base.ToString().Substring(1)}"
                    );
            }
        }

        public class UuidValues : IdValues, IEquatable<UuidValues>
        {
            public Guid? UUID { get; }

            public UuidValues(Guid? uuid, string value, string absoluteUri) : base(value, absoluteUri)
            {
                UUID = uuid;
            }

            public bool Equals([AllowNull] UuidValues other) => null != other && (ReferenceEquals(this, other) ||
                (base.Equals(this) && UUID == other.UUID));

            public override bool Equals(object obj) => Equals(obj as SnIdValues);

            public override int GetHashCode() => HashCode.Combine(UUID, AbsoluteUri, Value);

            public override string ToString()
            {
                return (UUID.HasValue) ?
                    $"{{ UUID = {UUID.Value.ToString("d").ToLower()},{base.ToString().Substring(1)}"
                    : $"{{ UUID = null,{base.ToString().Substring(1)}";
            }
        }

    }
}
