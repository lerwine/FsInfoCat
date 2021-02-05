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
            _hostName = Environment.MachineName.ToLower();
            _systemDrivePath = Path.GetPathRoot(Environment.SystemDirectory);
            _systemDriveUrl = new Uri(_systemDrivePath).AbsoluteUri;
            IPAddress[] addressList = Dns.GetHostEntry(_hostName).AddressList;
            _ipV2Address = addressList.Where(a => a.AddressFamily == AddressFamily.InterNetwork).Select(a => a.ToString()).DefaultIfEmpty("127.0.0.1").First();
            _ipV6Address = addressList.Where(a => a.AddressFamily == AddressFamily.InterNetworkV6).Select(a => a.ToString()).DefaultIfEmpty("::1").First().Split('%')[0];
        }

        class SerialNumberTestValues
        {
            private readonly ReadOnlyCollection<SerialNumberTestValues> _caseVariants;
            private readonly ReadOnlyCollection<SerialNumberTestValues> _dashCaseVariants;
            private readonly SerialNumberTestValues _dashVariant;
            internal uint SerialNumber { get; }
            internal string StringParam { get; }
            internal string StringValue { get; }
            internal string Description { get; }
            internal string UrnParam { get; }
            internal Uri Url { get; }
            protected SerialNumberTestValues(uint serialNumber, string description)
            {
                SerialNumber = serialNumber;
                StringParam = $"{(serialNumber >> 16).ToString("x4")}-{(serialNumber & 0xFFFFU).ToString("x4")}";
                StringValue = StringParam;
                Description = description;
                Url = new Uri($"urn:volume:id:{StringParam}", UriKind.Absolute);
                UrnParam = Url.AbsoluteUri;
                Collection<SerialNumberTestValues> caseVariants = new Collection<SerialNumberTestValues>();
                Collection<SerialNumberTestValues> dashCaseVariants = new Collection<SerialNumberTestValues>();
                _caseVariants = new ReadOnlyCollection<SerialNumberTestValues>(caseVariants);
                _dashCaseVariants = new ReadOnlyCollection<SerialNumberTestValues>(dashCaseVariants);
                _dashVariant = new SerialNumberTestValues(this, serialNumber.ToString("x8"), $"{description} w/o dash",
                    $"urn:volume:id:{serialNumber.ToString("x8")}");
                caseVariants.Add(this);
                dashCaseVariants.Add(this);
                dashCaseVariants.Add(_dashVariant);
                if (StringParam.ToUpper() != StringParam)
                {
                    caseVariants.Add(new SerialNumberTestValues(this, StringParam.ToUpper(),
                        $"Upper case {Description.Substring(0, 1).ToLower()}{Description.Substring(1)}",
                        $"urn:VOLUME:ID:{StringParam.ToUpper()}"));
                    caseVariants.Add(new SerialNumberTestValues(this, _dashVariant.StringParam.ToUpper(),
                        $"Upper case {_dashVariant.Description.Substring(0, 1).ToLower()}{_dashVariant.Description.Substring(1)}",
                        $"urn:VOLUME:ID:{_dashVariant.StringParam.ToUpper()}"));
                }
            }
            protected SerialNumberTestValues(SerialNumberTestValues dashVariant, string stringParam, string description, string urn)
            {
                _dashVariant = dashVariant;
                _caseVariants = dashVariant._caseVariants;
                _dashCaseVariants = dashVariant._dashCaseVariants;
                SerialNumber = dashVariant.SerialNumber;
                StringParam = stringParam;
                StringValue = dashVariant.StringValue;
                Description = description;
                Url = dashVariant.Url;
                UrnParam = urn;
            }

            private static IEnumerable<SerialNumberTestValues> _GetTestValues()
            {
                yield return new SerialNumberTestValues(0x3B518D4BU, "Value of 995,200,331");
                yield return new SerialNumberTestValues(0x9E497DE8U, "Bit-wise equivalent to signed integer -1,639,350,808");
                yield return new SerialNumberTestValues(0U, "Zero value");
                yield return new SerialNumberTestValues(1U, "Value of 1");
                yield return new SerialNumberTestValues(0x80000000U, "Bit-wise equivalent to Int32.MinValue");
                yield return new SerialNumberTestValues(0x7FFFFFFFU, "Bit-wise equivalent to Int32.MaxValue");
                yield return new SerialNumberTestValues(0xFFFFFFFFU, "Bit-wise equivalent to signed integer -1");
            }

            public static IEnumerable<SerialNumberTestValues> GetTestValues(bool includeCaseVariants = false, bool includeDashVariants = false)
            {
                if (includeCaseVariants)
                {
                    if (includeDashVariants)
                        return _GetTestValues().SelectMany(v => v._dashCaseVariants);
                    return _GetTestValues().SelectMany(v => v._caseVariants);
                }
                if (includeDashVariants)
                    return _GetTestValues().SelectMany(v => new SerialNumberTestValues[] { v, v._dashVariant });
                return _GetTestValues();
            }
        }

        class SerialNumberOrdinalTestValues
        {
            private readonly ReadOnlyCollection<SerialNumberOrdinalTestValues> _variants;
            internal uint SerialNumber { get; }
            internal byte Ordinal { get; }
            internal string StringParam { get; }
            internal string StringValue { get; }
            internal string Description { get; }
            internal string UrnParam { get; }
            internal Uri Url { get; }
            protected SerialNumberOrdinalTestValues(uint serialNumber, byte ordinal, string description)
            {
                SerialNumber = serialNumber;
                Ordinal = ordinal;
                StringParam = $"{serialNumber.ToString("x8")}-{ordinal.ToString("x2")}";
                StringValue = StringParam;
                Description = description;
                Url = new Uri($"urn:volume:id:{StringParam}", UriKind.Absolute);
                UrnParam = Url.AbsoluteUri;
                Collection<SerialNumberOrdinalTestValues> variants = new Collection<SerialNumberOrdinalTestValues>();
                _variants = new ReadOnlyCollection<SerialNumberOrdinalTestValues>(variants);
                variants.Add(this);
                if (StringParam.ToUpper() != StringParam)
                {
                    SerialNumberOrdinalTestValues caseVariant = new SerialNumberOrdinalTestValues(this, StringParam.ToUpper(),
                        $"Upper case {Description.Substring(0, 1).ToLower()}{Description.Substring(1)}",
                        $"urn:VOLUME:ID:{StringParam.ToUpper()}");
                    variants.Add(caseVariant);
                    if (ordinal < 16)
                    {
                        variants.Add(new SerialNumberOrdinalTestValues(this, $"{serialNumber.ToString("x8")}-{ordinal.ToString("x")}",
                            $"{Description} without leading zero",
                            $"urn:volume:id:{serialNumber.ToString("x8")}-{ordinal.ToString("x")}"));
                        variants.Add(new SerialNumberOrdinalTestValues(this, $"{serialNumber.ToString("X8")}-{ordinal.ToString("X")}",
                            $"{caseVariant.Description.Substring(1)} without leading zero",
                            $"urn:VOLUME:ID:{serialNumber.ToString("X8")}-{ordinal.ToString("X")}"));
                    }
                }
                else if (ordinal < 16)
                    variants.Add(new SerialNumberOrdinalTestValues(this, $"{serialNumber.ToString("x8")}-{ordinal.ToString("x")}",
                        $"{Description} without leading zero",
                        $"urn:volume:id:{serialNumber.ToString("x8")}-{ordinal.ToString("x")}"));
            }
            protected SerialNumberOrdinalTestValues(SerialNumberOrdinalTestValues masterVariant, string stringParam, string description, string urn)
            {
                _variants = masterVariant._variants;
                SerialNumber = masterVariant.SerialNumber;
                Ordinal = masterVariant.Ordinal;
                StringParam = stringParam;
                StringValue = masterVariant.StringValue;
                Description = description;
                Url = masterVariant.Url;
                UrnParam = urn;
            }

            private static IEnumerable<Tuple<byte, string>> _GetOrdinalValues()
            {
                yield return new Tuple<byte, string>(0, "zero value");
                yield return new Tuple<byte, string>(1, "Value of 1");
                yield return new Tuple<byte, string>(10, "Value of 10");
                yield return new Tuple<byte, string>(16, "Value of 16");
                yield return new Tuple<byte, string>(Byte.MaxValue, "Byte.MaxValue");
            }

            public static IEnumerable<SerialNumberOrdinalTestValues> GetTestValues(bool includeVariants = false)
            {
                if (includeVariants)
                    return GetTestValues(false).SelectMany(o => o._variants);
                return SerialNumberTestValues.GetTestValues(false, false).SelectMany(sn => _GetOrdinalValues().Select(ord =>
                    new SerialNumberOrdinalTestValues(sn.SerialNumber, ord.Item1, $"serialNumber = {sn.Description}; ordinal = {ord.Item2}")
                ));
            }
        }

        class UrlTestValues
        {
            private readonly ReadOnlyCollection<UrlTestValues> _variants;
            internal Uri NormalizedUrl { get; }
            internal string UrlParam { get; }
            internal string Description { get; }
            internal bool HasTrailingSlash { get; }
            public UrlTestValues(Uri url, string description)
            {
                string originalString = url.OriginalString;
                NormalizedUrl = UniqueIdentifier.NormalizeUri(url);
                Assert.That(NormalizedUrl.IsAbsoluteUri, Is.True, $"Invalid test data: {url.OriginalString}");
                Assert.That(NormalizedUrl.Query, Is.Empty, $"Invalid test data: {url.OriginalString}");
                Assert.That(NormalizedUrl.Fragment, Is.Empty, $"Invalid test data: {url.OriginalString}");
                if (originalString.EndsWith("#"))
                    originalString = originalString.Substring(0, originalString.Length - 1);
                if (originalString.EndsWith("?"))
                    originalString = originalString.Substring(0, originalString.Length - 1);
                UrlParam = originalString;
                Collection<UrlTestValues> variants = new Collection<UrlTestValues>();
                _variants = new ReadOnlyCollection<UrlTestValues>(variants);
                UrlTestValues pathVariant;
                HasTrailingSlash = NormalizedUrl.AbsolutePath.EndsWith('/');
                string noSlashUrl = (HasTrailingSlash) ? originalString.Substring(0, originalString.Length - 1) : $"{originalString}/";
                if (originalString != NormalizedUrl.AbsoluteUri)
                {
                    Description = $"Improper URL {description}";
                    variants.Add(this);
                    variants.Add(new UrlTestValues(this, $"{noSlashUrl}?", $"{Description} with empty query", false));
                    variants.Add(new UrlTestValues(this, $"{noSlashUrl}#", $"{Description} with empty fragment", false));
                    if (HasTrailingSlash)
                        pathVariant = new UrlTestValues(this, noSlashUrl, $"{Description} without trailing slash", false);
                    else
                        pathVariant = new UrlTestValues(this, $"{UrlParam}/", $"{Description} with trailing slash", true);
                    variants.Add(pathVariant);
                    variants.Add(new UrlTestValues(this, $"{pathVariant.UrlParam}?#", $"{pathVariant.Description} and empty query and fragment", true));
                    variants.Add(new UrlTestValues(this, NormalizedUrl.AbsoluteUri, description, HasTrailingSlash));
                }
                else
                {
                    Description = description;
                    variants.Add(this);
                }
                noSlashUrl = (HasTrailingSlash) ? NormalizedUrl.AbsolutePath.Substring(0, NormalizedUrl.AbsolutePath.Length - 1) : $"{NormalizedUrl.AbsolutePath}/";
                variants.Add(new UrlTestValues(this, $"{noSlashUrl}?", $"{description} with empty query", false));
                variants.Add(new UrlTestValues(this, $"{noSlashUrl}#", $"{description} with empty fragment", false));
                if (HasTrailingSlash)
                    pathVariant = new UrlTestValues(this, noSlashUrl, $"{description} without trailing slash", false);
                else
                    pathVariant = new UrlTestValues(this, $"{NormalizedUrl.AbsolutePath}/", $"{description} with trailing slash", true);
                variants.Add(pathVariant);
                variants.Add(new UrlTestValues(this, $"{pathVariant.UrlParam}?#", $"{pathVariant.Description} and empty query and fragment", true));
            }

            private UrlTestValues(UrlTestValues source, string urlParam, string description, bool hasTrailingSlash)
            {
                NormalizedUrl = source.NormalizedUrl;
                _variants = source._variants;
                UrlParam = urlParam;
                Description = description;
                HasTrailingSlash = hasTrailingSlash;
            }

            private static IEnumerable<UrlTestValues> _GetUrlTestValues()
            {
                yield return new UrlTestValues(new Uri(@"\\servicenowdiag479.file.core.windows.net\testazureshare\", UriKind.Absolute), "File UNC with fqdn");
                yield return new UrlTestValues(new Uri($@"\\{_hostName}\$Admin\", UriKind.Absolute), "File UNC");
                yield return new UrlTestValues(new Uri($@"\\{_ipV2Address}\Us&Them\", UriKind.Absolute), "File with IPV2");
                yield return new UrlTestValues(new Uri($@"\\[{_ipV6Address}]\100% Done", UriKind.Absolute), "File with IPV6");
                yield return new UrlTestValues(new Uri($"urn:uuid:{Guid.NewGuid().ToString("b").ToLower()}", UriKind.Absolute), "UUID urn");
            }

            public static IEnumerable<UrlTestValues> GetUrlTestValues(bool includeVariants = false)
            {
                if (includeVariants)
                    return _GetUrlTestValues().SelectMany(u => u._variants);
                return _GetUrlTestValues();
            }
        }

        class UUIDTestValues
        {
            private readonly ReadOnlyCollection<UUIDTestValues> _variants;
            internal Guid UUID { get; }
            internal string StringParam { get; }
            internal string StringValue { get; }
            internal string Description { get; }
            internal string UrnParam { get; }
            internal Uri Url { get; }
            protected UUIDTestValues(Guid uuid, string description)
            {
                UUID = uuid;
                StringParam = uuid.ToString("d").ToLower();
                StringValue = StringParam;
                Description = description;
                Url = new Uri($"urn:uuid:{StringParam}", UriKind.Absolute);
                UrnParam = Url.AbsoluteUri;
                Collection<UUIDTestValues> variants = new Collection<UUIDTestValues>();
                _variants = new ReadOnlyCollection<UUIDTestValues>(variants);
                variants.Add(this);
                variants.Add(new UUIDTestValues(this, uuid.ToString("b").ToLower(),
                    $"Brace format {Description.Substring(0, 1).ToLower()}{Description.Substring(1)}",
                    $"urn:uuid:{uuid.ToString("b").ToLower()}"));
                variants.Add(new UUIDTestValues(this, uuid.ToString("n").ToLower(),
                    $"No-dash format {Description.Substring(0, 1).ToLower()}{Description.Substring(1)}",
                    $"urn:uuid:{uuid.ToString("n").ToLower()}"));
                variants.Add(new UUIDTestValues(this, uuid.ToString("p").ToLower(),
                    $"Parenheses format {Description.Substring(0, 1).ToLower()}{Description.Substring(1)}",
                    $"urn:uuid:{uuid.ToString("p").ToLower()}"));
                variants.Add(new UUIDTestValues(this, uuid.ToString("x").ToLower(),
                    $"Grouped hex format {Description.Substring(0, 1).ToLower()}{Description.Substring(1)}",
                    $"urn:uuid:{uuid.ToString("x").ToLower()}"));
                if (StringParam.ToUpper() != StringParam)
                {
                    foreach (UUIDTestValues t in variants.ToArray())
                        variants.Add(new UUIDTestValues(this, t.StringParam.ToUpper(),
                            $"Upper case {t.Description.Substring(0, 1).ToLower()}{t.Description.Substring(1)}",
                            $"urn:UUID:{t.StringParam.ToUpper()}"));
                }
            }
            protected UUIDTestValues(UUIDTestValues masterVariant, string stringParam, string description, string urn)
            {
                _variants = masterVariant._variants;
                UUID = masterVariant.UUID;
                StringParam = stringParam;
                StringValue = masterVariant.StringValue;
                Description = description;
                Url = masterVariant.Url;
                UrnParam = urn;
            }

            public static IEnumerable<UUIDTestValues> GetTestValues(bool includeVariants)
            {
                if (includeVariants)
                {
                    Guid guid = Guid.NewGuid();
                    string s = guid.ToString("n").ToLower();
                    while (s.ToUpper() == s)
                        s = (guid = Guid.NewGuid()).ToString("n").ToLower();
                    return (new UUIDTestValues[] {
                        new UUIDTestValues(guid, "Random UUID"), new UUIDTestValues(Guid.Empty, "Empty UUID")
                    }).SelectMany(u => u._variants);
                }
                return new UUIDTestValues[]
                {
                    new UUIDTestValues(Guid.NewGuid(), "Random UUID #1"),
                    new UUIDTestValues(Guid.NewGuid(), "Random UUID #2"),
                    new UUIDTestValues(Guid.Empty, "Empty UUID")
                };
            }
        }

        public static IEnumerable<TestCaseData> GetGuidParameterTestCases() => UUIDTestValues.GetTestValues(false)
            .Select(t =>
                new TestCaseData(t.UUID)
                    .SetName($"{t.Description} FromGuidValueConstructorTest({t.UUID})")
                    .Returns(new IdValues(t.StringValue, t.Url.AbsoluteUri))
            );

        public static IEnumerable<TestCaseData> GetValidUUIDUriParameterTestCases() => UUIDTestValues.GetTestValues(true)
            .Select(t =>
                new TestCaseData(t.UrnParam)
                    .SetName($"{t.Description} FromValidUUIDUriConstructorTest(\"{t.UrnParam}\")")
                    .Returns(new UuidValues(t.UUID, t.StringValue, t.Url.AbsoluteUri))
            );

        public static IEnumerable<TestCaseData> GetSerialNumberParameterTestCases() =>
            SerialNumberTestValues.GetTestValues(false, false).Select(t =>
                new TestCaseData(t.SerialNumber)
                    .SetName($"{t.Description} FromSerialNumberValueConstructorTest({t.SerialNumber})")
                    .Returns(new IdValues(t.StringValue, t.Url.AbsoluteUri))
            );

        public static IEnumerable<TestCaseData> GetSerialNumberAndOrdinalParametersTestCases() => SerialNumberOrdinalTestValues.GetTestValues(false)
            .Select(t =>
                new TestCaseData(t.SerialNumber, t.Ordinal)
                    .SetName($"{t.Description} FromSerialNumberAndOrdinalValuesConstructorTest({t.SerialNumber}, {t.Ordinal})")
                    .Returns(new SnIdValues(t.SerialNumber, t.Ordinal, t.StringValue, t.Url.AbsoluteUri))
            );

        public static IEnumerable<TestCaseData> GetValidSerialNumberUriParameterTestCases() => SerialNumberTestValues.GetTestValues(true, true)
            .Select(t =>
                new TestCaseData(t.UrnParam)
                    .SetName($"{t.Description} FromValidSerialNumberAndOrdinalUriConstructorTest(\"{t.UrnParam}\")")
                    .Returns(new SnIdValues(t.SerialNumber, null, t.StringValue, t.Url.AbsoluteUri))
            ).Concat(SerialNumberOrdinalTestValues.GetTestValues(true).Select(t =>
                new TestCaseData(t.UrnParam)
                    .SetName($"{t.Description} FromValidSerialNumberAndOrdinalUriConstructorTest(\"{t.UrnParam}\")")
                    .Returns(new SnIdValues(t.SerialNumber, t.Ordinal, t.StringValue, t.Url.AbsoluteUri))
            ));

        public static IEnumerable<TestCaseData> GetValidFilePathParameterTestCases()
        {
            string path = @"\\servicenowdiag479.file.core.windows.net\testazureshare";
            string expected = $@"{path}\";
            string url = "file://servicenowdiag479.file.core.windows.net/testazureshare/";
            yield return new TestCaseData(path)
                .SetName($"URN with fully qualified machine name: FromValidFilePathConstructorTest(\"{path}\")")
                .Returns(new IdValues(expected, url));

            yield return new TestCaseData(expected)
                .SetName($"URN with fully qualified machine name and trailing backslash: FromValidFilePathConstructorTest(\"{expected}\")")
                .Returns(new IdValues(expected, url));

            path = $@"\\{_hostName}\$Admin";
            expected = $@"{path}\";
            url = $"file://{_hostName}/$Admin/";
            yield return new TestCaseData(path)
                .SetName($"URN with machine name only: FromValidFilePathConstructorTest(\"{path}\")")
                .Returns(new IdValues(expected, url));

            path = $@"\\{_hostName}\Admin$\";
            yield return new TestCaseData(expected)
                .SetName($"URN with machine name only and trailing backslash: FromValidFilePathConstructorTest(\"{expected}\")")
                .Returns(new IdValues(expected, url));

            path = $@"\\{_ipV2Address}\Us&Them";
            expected = $@"{path}\";
            url = $"file://{_ipV2Address}/Us&Them/";
            yield return new TestCaseData(path)
                .SetName($"URN with IPV2 address: FromValidFilePathConstructorTest(\"{path}\")")
                .Returns(new IdValues(expected, url));

            path = $@"\\{_ipV2Address}\Us&Them\";
            yield return new TestCaseData(expected)
                .SetName($"URN with IPV2 address and trailing backslash: FromValidFilePathConstructorTest(\"{expected}\")")
                .Returns(new IdValues(expected, url));

            path = $@"\\[{_ipV6Address}]\100% Done";
            expected = $@"{path}\";
            url = $"file://[{_ipV6Address}]/100%25%20Done/";
            yield return new TestCaseData(path)
                .SetName($"URN with IPV6 address: FromValidFilePathConstructorTest(\"{path}\")")
                .Returns(new IdValues(expected, url));

            path = $@"\\[{_ipV6Address}]\100% Done\";
            yield return new TestCaseData(expected)
                .SetName($"URN with IPV6 address and trailing backslash: FromValidFilePathConstructorTest(\"{expected}\")")
                .Returns(new IdValues(expected, url));
        }

        public static IEnumerable<TestCaseData> GetValidFileUrlParameterTestCases()
        {
            string url = "file://servicenowdiag479.file.core.windows.net/testazureshare";
            string expected = $"{url}/";
            string path = @"\\servicenowdiag479.file.core.windows.net\testazureshare\";
            yield return new TestCaseData(url)
                .SetName($"URN with fully qualified machine name: FromValidFileUrlConstructorTest(\"{url}\")")
                .Returns(new IdValues(path, expected));

            yield return new TestCaseData(expected)
                .SetName($"URN with fully qualified machine name and trailing slash: FromValidFileUrlConstructorTest(\"{expected}\")")
                .Returns(new IdValues(path, expected));

            string noTrailingSlash = url;
            url = $"{noTrailingSlash}?";
            yield return new TestCaseData(url)
                .SetName($"URN with fully qualified machine name and empty query: FromValidFileUrlConstructorTest(\"{url}\")")
                .Returns(new IdValues(path, expected));

            url = $"{expected}?";
            yield return new TestCaseData(url)
                .SetName($"URN with fully qualified machine name, trailing slash and empty query: FromValidFileUrlConstructorTest(\"{url}\")")
                .Returns(new IdValues(path, expected));

            url = $"{noTrailingSlash}#";
            yield return new TestCaseData(url)
                .SetName($"URN with fully qualified machine name and empty fragment: FromValidFileUrlConstructorTest(\"{url}\")")
                .Returns(new IdValues(path, expected));

            url = $"{expected}#";
            yield return new TestCaseData(url)
                .SetName($"URN with fully qualified machine name, trailing slash and empty fragment: FromValidFileUrlConstructorTest(\"{url}\")")
                .Returns(new IdValues(path, expected));

            url = $"{noTrailingSlash}?#";
            yield return new TestCaseData(url)
                .SetName($"URN with fully qualified machine name, empty query and empty fragment:0 FromValidFileUrlConstructorTest(\"{url}\")")
                .Returns(new IdValues(path, expected));

            url = $"{expected}?#";
            yield return new TestCaseData(url)
                .SetName($"URN with fully qualified machine name, trailing slash, empty query and empty fragment: FromValidFileUrlConstructorTest(\"{url}\")")
                .Returns(new IdValues(path, expected));

            url = $"file://{_hostName}/$Admin";
            expected = $"{url}/";
            path = $@"\\{_hostName}\$Admin\";
            yield return new TestCaseData(url)
                .SetName($"URN with machine name only: FromValidFileUrlConstructorTest(\"{url}\")")
                .Returns(new IdValues(path, expected));

            yield return new TestCaseData(expected)
                .SetName($"URN with machine name only and trailing slash: FromValidFileUrlConstructorTest(\"{expected}\")")
                .Returns(new IdValues(path, expected));

            url = $"file://{_ipV2Address}/Us&Them";
            expected = $"{url}/";
            path = $@"\\{_ipV2Address}\Us&Them\";
            yield return new TestCaseData(url)
                .SetName($"URN with IPV2 address: FromValidFileUrlConstructorTest(\"{url}\")")
                .Returns(new IdValues(path, expected));

            yield return new TestCaseData(expected)
                .SetName($"URN with IPV2 address and trailing slash: FromValidFileUrlConstructorTest(\"{expected}\")")
                .Returns(new IdValues(path, expected));

            url = $"file://[{_ipV6Address}]/100%25%20Done";
            expected = $"{url}/";
            path = $@"\\[{_ipV6Address}]\100% Done\";
            yield return new TestCaseData(url)
                .SetName($"URN with IPV6 address: FromValidFileUrlConstructorTest(\"{url}\")")
                .Returns(new IdValues(path, expected));

            yield return new TestCaseData(expected)
                .SetName($"URN with IPV6 address and trailing slash: FromValidFileUrlConstructorTest(\"{expected}\")")
                .Returns(new IdValues(path, expected));
        }

        public static IEnumerable<TestCaseData> GetNormalizeUriTestCases()
        {
            string rawUri = $"file://[{_ipV6Address}]/100% Done";
            Uri expected = new Uri(new Uri(rawUri).AbsoluteUri, UriKind.Absolute);
            string wellFormed = expected.AbsoluteUri;
            yield return new TestCaseData(new Uri(rawUri), (bool?)null, (bool?)null, (UniqueIdentifier.NormalizePathOption?)null)
                .SetName($"NormalizeUri(\"{rawUri}\")")
                .Returns(expected);
            yield return new TestCaseData(new Uri($"{rawUri}?"), (bool?)null, (bool?)null, (UniqueIdentifier.NormalizePathOption?)null)
                .SetName($"NormalizeUri(\"{rawUri}?\")")
                .Returns(expected);
            yield return new TestCaseData(new Uri($"{rawUri}#"), (bool?)null, (bool?)null, (UniqueIdentifier.NormalizePathOption?)null)
                .SetName($"NormalizeUri(\"{rawUri}#\")")
                .Returns(expected);
            yield return new TestCaseData(new Uri(wellFormed), (bool?)null, (bool?)null, (UniqueIdentifier.NormalizePathOption?)null)
                .SetName($"NormalizeUri(\"{wellFormed}\")")
                .Returns(expected);
            yield return new TestCaseData(new Uri($"{wellFormed}?"), (bool?)null, (bool?)null, (UniqueIdentifier.NormalizePathOption?)null)
                .SetName($"NormalizeUri(\"{wellFormed}?\")")
                .Returns(expected);
            yield return new TestCaseData(new Uri($"{wellFormed}#"), (bool?)null, (bool?)null, (UniqueIdentifier.NormalizePathOption?)null)
                .SetName($"NormalizeUri(\"{wellFormed}#\")")
                .Returns(expected);
            string trailingSlashRawUri = rawUri + "/";
            string trailingSlashWellFormed = wellFormed + "/";
            yield return new TestCaseData(new Uri(trailingSlashRawUri), false, false, UniqueIdentifier.NormalizePathOption.NoTrailingSlash)
                .SetName($"NormalizeUri(\"{trailingSlashRawUri}\", false, false, NoTrailingSlash)")
                .Returns(expected);
            yield return new TestCaseData(new Uri($"{trailingSlashRawUri}?"), false, false, UniqueIdentifier.NormalizePathOption.NoTrailingSlash)
                .SetName($"NormalizeUri(\"{trailingSlashRawUri}?\", false, false, NoTrailingSlash)")
                .Returns(expected);
            yield return new TestCaseData(new Uri($"{trailingSlashRawUri}#"), false, false, UniqueIdentifier.NormalizePathOption.NoTrailingSlash)
                .SetName($"NormalizeUri(\"{trailingSlashRawUri}#\", false, false, NoTrailingSlash)")
                .Returns(expected);
            yield return new TestCaseData(new Uri($"{trailingSlashRawUri}#"), false, false, UniqueIdentifier.NormalizePathOption.NoTrailingSlash)
                .SetName($"NormalizeUri(\"{trailingSlashRawUri}?#\", false, false, NoTrailingSlash)")
                .Returns(expected);
            yield return new TestCaseData(new Uri(trailingSlashWellFormed), false, false, UniqueIdentifier.NormalizePathOption.NoTrailingSlash)
                .SetName($"NormalizeUri(\"{trailingSlashWellFormed}\", false, false, NoTrailingSlash)")
                .Returns(expected);
            yield return new TestCaseData(new Uri($"{trailingSlashWellFormed}?"), false, false, UniqueIdentifier.NormalizePathOption.NoTrailingSlash)
                .SetName($"NormalizeUri(\"{trailingSlashWellFormed}?\", false, false, NoTrailingSlash)")
                .Returns(expected);
            yield return new TestCaseData(new Uri($"{trailingSlashWellFormed}#"), false, false, UniqueIdentifier.NormalizePathOption.NoTrailingSlash)
                .SetName($"NormalizeUri(\"{trailingSlashWellFormed}#\", false, false, NoTrailingSlash)")
                .Returns(expected);
            yield return new TestCaseData(new Uri($"{trailingSlashWellFormed}#"), false, false, UniqueIdentifier.NormalizePathOption.NoTrailingSlash)
                .SetName($"NormalizeUri(\"{trailingSlashWellFormed}?#\", false, false, NoTrailingSlash)")
                .Returns(expected);

            Uri slashExpected = new Uri(trailingSlashWellFormed, UriKind.Absolute);
            yield return new TestCaseData(new Uri(rawUri), false, false, UniqueIdentifier.NormalizePathOption.TrailingSlash)
                .SetName($"NormalizeUri(\"{rawUri}\")")
                .Returns(slashExpected);
            yield return new TestCaseData(new Uri($"{rawUri}?"), false, false, UniqueIdentifier.NormalizePathOption.TrailingSlash)
                .SetName($"NormalizeUri(\"{rawUri}?\", false, false, TrailingSlash)")
                .Returns(slashExpected);
            yield return new TestCaseData(new Uri($"{rawUri}#"), false, false, UniqueIdentifier.NormalizePathOption.TrailingSlash)
                .SetName($"NormalizeUri(\"{rawUri}#\", false, false, TrailingSlash)")
                .Returns(slashExpected);
            yield return new TestCaseData(new Uri(trailingSlashRawUri), false, false, UniqueIdentifier.NormalizePathOption.TrailingSlash)
                .SetName($"NormalizeUri(\"{trailingSlashRawUri}\", false, false, TrailingSlash)")
                .Returns(slashExpected);
            yield return new TestCaseData(new Uri($"{trailingSlashRawUri}?"), false, false, UniqueIdentifier.NormalizePathOption.TrailingSlash)
                .SetName($"NormalizeUri(\"{trailingSlashRawUri}?\", false, false, TrailingSlash)")
                .Returns(slashExpected);
            yield return new TestCaseData(new Uri($"{trailingSlashRawUri}#"), false, false, UniqueIdentifier.NormalizePathOption.TrailingSlash)
                .SetName($"NormalizeUri(\"{trailingSlashRawUri}#\", false, false, TrailingSlash)")
                .Returns(slashExpected);
            yield return new TestCaseData(new Uri($"{trailingSlashRawUri}#"), false, false, UniqueIdentifier.NormalizePathOption.TrailingSlash)
                .SetName($"NormalizeUri(\"{trailingSlashRawUri}?#\", false, false, TrailingSlash)")
                .Returns(slashExpected);
            yield return new TestCaseData(new Uri(wellFormed), false, false, UniqueIdentifier.NormalizePathOption.TrailingSlash)
                .SetName($"NormalizeUri(\"{wellFormed}\", false, false, TrailingSlash)")
                .Returns(slashExpected);
            yield return new TestCaseData(new Uri($"{wellFormed}?"), false, false, UniqueIdentifier.NormalizePathOption.TrailingSlash)
                .SetName($"NormalizeUri(\"{wellFormed}?\", false, false, TrailingSlash)")
                .Returns(slashExpected);
            yield return new TestCaseData(new Uri($"{wellFormed}#"), false, false, UniqueIdentifier.NormalizePathOption.TrailingSlash)
                .SetName($"NormalizeUri(\"{wellFormed}#\", false, false, TrailingSlash)")
                .Returns(slashExpected);
            yield return new TestCaseData(new Uri(trailingSlashWellFormed), false, false, UniqueIdentifier.NormalizePathOption.TrailingSlash)
                .SetName($"NormalizeUri(\"{trailingSlashWellFormed}\", false, false, TrailingSlash)")
                .Returns(slashExpected);
            yield return new TestCaseData(new Uri($"{trailingSlashWellFormed}?"), false, false, UniqueIdentifier.NormalizePathOption.TrailingSlash)
                .SetName($"NormalizeUri(\"{trailingSlashWellFormed}?\", false, false, TrailingSlash)")
                .Returns(slashExpected);
            yield return new TestCaseData(new Uri($"{trailingSlashWellFormed}#"), false, false, UniqueIdentifier.NormalizePathOption.TrailingSlash)
                .SetName($"NormalizeUri(\"{trailingSlashWellFormed}#\", false, false, TrailingSlash)")
                .Returns(slashExpected);
            yield return new TestCaseData(new Uri($"{trailingSlashWellFormed}#"), false, false, UniqueIdentifier.NormalizePathOption.TrailingSlash)
                .SetName($"NormalizeUri(\"{trailingSlashWellFormed}?#\", false, false, TrailingSlash)")
                .Returns(slashExpected);
        }

        public static IEnumerable<TestCaseData> GetGuidToIdentifierStringTestCases() => UUIDTestValues.GetTestValues(false)
            .Select(t => new TestCaseData(t.UUID)
                    .SetName($"{t.Description} ToNormalizedUriTest({t.UUID})")
                .Returns(t.StringValue));

        public static IEnumerable<TestCaseData> GetGuidToUrnTestCases() => UUIDTestValues.GetTestValues(false)
            .Select(t => new TestCaseData(t.UUID)
                .SetName($"{t.Description} GuidToUrnTest({t.UUID})")
                .Returns(t.Url.AbsoluteUri));

        public static IEnumerable<TestCaseData> GetSerialNumberToIdentifierStringTestCases() => SerialNumberTestValues.GetTestValues(false)
            .Select(t => new TestCaseData(t.SerialNumber, (byte?)null)
                .SetName($"{t.Description} SerialNumberToIdentifierStringTest({t.SerialNumber}, null)")
                .Returns(t.StringValue))
            .Concat(SerialNumberOrdinalTestValues.GetTestValues()
                .Select(t => new TestCaseData(t.SerialNumber, t.Ordinal)
                    .SetName($"{t.Description} SerialNumberToIdentifierStringTest({t.SerialNumber}, {t.Ordinal})")
                    .Returns(t.StringValue)));

        public static IEnumerable<TestCaseData> GetSerialNumberToUrnTestCases() => SerialNumberTestValues.GetTestValues(false)
            .Select(t => new TestCaseData(t.SerialNumber, (byte?)null)
                .SetName($"{t.Description} SerialNumberToUrnTest({t.SerialNumber})")
                .Returns(t.Url.AbsoluteUri))
            .Concat(SerialNumberOrdinalTestValues.GetTestValues()
                .Select(t => new TestCaseData(t.SerialNumber, t.Ordinal)
                    .SetName($"{t.Description} SerialNumberToUrnTest({t.SerialNumber}, {t.Ordinal})")
                    .Returns(t.Url.AbsoluteUri)));

        public static IEnumerable<TestCaseData> GetParseUrnTestCases()
        {
            return UUIDTestValues.GetTestValues(true).Select(t =>
                new TestCaseData(t.UrnParam.Substring(4))
                    .SetName($"{t.Description} ParseUrnTest(\"{t.UrnParam.Substring(4)}\")")
                    .Returns(new ParseResultValues(t.UUID, null, null))
            ).Concat(SerialNumberTestValues.GetTestValues().Take(2).Select(t =>
                new TestCaseData(t.UrnParam.Substring(4))
                    .SetName($"{t.Description} ParseUrnTest(\"{t.UrnParam.Substring(4)}\")")
                    .Returns(new ParseResultValues(null, t.SerialNumber, null))
            )).Concat(SerialNumberOrdinalTestValues.GetTestValues(true).Select(t =>
                new TestCaseData(t.UrnParam.Substring(4))
                    .SetName($"{t.Description} ParseUrnTest(\"{t.UrnParam.Substring(4)}\")")
                    .Returns(new ParseResultValues(null, t.SerialNumber, t.Ordinal))
            ));
        }

        [SetUp]
        public void Setup()
        {
        }

        [Test]
        [Property("Priority", 1)]
        [TestCaseSource("GetNormalizeUriTestCases")]
        public Uri NormalizeUriTest(Uri url, bool? stripQuery, bool? stripFragment, UniqueIdentifier.NormalizePathOption? pathOption)
        {
            if (stripQuery.HasValue)
            {
                if (stripFragment.HasValue)
                {
                    if (pathOption.HasValue)
                        return UniqueIdentifier.NormalizeUri(url, stripQuery.Value, stripFragment.Value, pathOption.Value);
                    return UniqueIdentifier.NormalizeUri(url, stripQuery.Value, stripFragment.Value);
                }
                return UniqueIdentifier.NormalizeUri(url, stripQuery.Value);
            }
            return UniqueIdentifier.NormalizeUri(url);
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
        [NUnit.Framework.Category("Working")]
        [TestCaseSource("GetParseUrnTestCases")]
        public ParseResultValues ParseUrnTest(string uriPath)
        {
            UniqueIdentifier.ParseUrnPath(uriPath, out Guid? uuid, out uint? serialNumber, out byte? ordinal);
            return new ParseResultValues(uuid, serialNumber, ordinal);
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
            return new IdValues(target.Value, target.URL.AbsoluteUri);
        }

        [Test]
        [Property("Priority", 3)]
        [TestCaseSource("GetSerialNumberAndOrdinalParametersTestCases")]
        public SnIdValues FromSerialNumberAndOrdinalValuesConstructorTest(uint serialNumber, byte ordinal)
        {
            UniqueIdentifier target = new UniqueIdentifier(serialNumber, ordinal);
            Assert.That(target.URL.IsAbsoluteUri, Is.True);
            Assert.That(target.URL.IsFile, Is.False);
            Assert.That(target.URL.Scheme, Is.EqualTo("urn"));
            Assert.That(target.URL.Fragment, Is.Empty);
            Assert.That(target.SerialNumber.HasValue, Is.True);
            Assert.That(target.SerialNumber.Value, Is.EqualTo(serialNumber));
            Assert.That(target.UUID.HasValue, Is.False);
            Assert.That(target.Ordinal.HasValue, Is.True);
            return new SnIdValues(target.SerialNumber, target.Ordinal, target.Value, target.URL.AbsoluteUri);
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
            Assert.That(target.SerialNumber.HasValue, Is.True);
            Assert.That(target.UUID.HasValue, Is.False);
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
            Assert.That(target.URL.Fragment, Is.Empty);
            Assert.That(target.SerialNumber.HasValue, Is.False);
            Assert.That(target.UUID.HasValue, Is.False);
            Assert.That(target.Ordinal.HasValue, Is.False);
            Assert.That(target.Value, Is.EqualTo(target.URL.LocalPath));
            return new IdValues(target.Value, target.URL.AbsoluteUri);
        }

        [Test]
        [Property("Priority", 3)]
        public void FromInvalidUriConstructorTest()
        {
            Assert.That(() => { new UniqueIdentifier(null); }, Throws.ArgumentNullException
                .With.Property("ParamName").EqualTo("url"));
            Assert.That(() => { new UniqueIdentifier(_systemDrivePath); }, Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("url"));
            Assert.That(() => { new UniqueIdentifier(_systemDriveUrl); }, Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("url"));
            Assert.That(() => { new UniqueIdentifier("file:///"); }, Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("url"));
        }

        [Test]
        [Property("Priority", 3)]
        public void FromInvalidUriFormatConstructorTest([Values("", " ", ".", "urn")] string uri)
        {
            Assert.That(() => { new UniqueIdentifier(uri); }, Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("url"));
        }

        [Test]
        [Property("Priority", 3)]
        public void FromInvalidUrnNamespaceConstructorTest([Values("urn:", "urn:id")] string uri)
        {
            Assert.That(() => { new UniqueIdentifier(uri); }, Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("uri"));
        }

        [Test]
        [Property("Priority", 3)]
        public void FromInvalidUUIDUrnNamespaceConstructorTest([Values(
            "urn:uuid", "urn:uuid:", "urn:uuid:fb4f6ff1-7ffe-4eef-840a-1df28bb2e178:2", "urn:uuid:fb4f6ff1-7ffe-4eef-840a-1df28bb2e17"
        )] string uri)
        {
            Assert.That(() => { new UniqueIdentifier(uri); }, Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("uri"));
        }

        [Test]
        [Property("Priority", 3)]
        public void FromInvalidVolumeIdUrnNamespaceConstructorTest([Values(
            "urn:volume", "urn:volume:", "urn:volume:id", "urn:volume:id:", "urn:volume:id:3B51-8D4B-1", "urn:volume:id:3B518D4B-"
        )] string uri)
        {
            Assert.That(() => { new UniqueIdentifier(uri); }, Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("uri"));
        }

        [Test]
        [Property("Priority", 3)]
        public void FromInvalidUriSchemeNamespaceConstructorTest([Values(
            "http://tempuri.org", "mailto:volume@id.3B51-8D4B"
        )] string uri)
        {
            Assert.That(() => { new UniqueIdentifier(uri); }, Throws.InstanceOf<ArgumentOutOfRangeException>()
                .With.Property("ParamName").EqualTo("url"));
        }

        public class IdValues : IEquatable<IdValues>
        {
            private Uri _absoluteUri;
            public string Value { get; }

            public string AbsoluteUri { get; }

            public IdValues(string value, string absoluteUri)
            {
                Value = value;
                AbsoluteUri = absoluteUri;
                if (null != absoluteUri && Uri.TryCreate(absoluteUri, UriKind.RelativeOrAbsolute, out Uri uri))
                    _absoluteUri = uri;
                else
                    _absoluteUri = null;
            }

            public bool Equals([AllowNull] IdValues other)
            {
                if (other is null)
                    return false;
                if (ReferenceEquals(this, other))
                    return true;
                if (null == _absoluteUri)
                {
                    if (null != other._absoluteUri)
                        return false;
                }
                else
                {
                    if (null == other._absoluteUri || !_absoluteUri.AuthorityCaseInsensitiveEquals(other._absoluteUri))
                        return false;
                }
                return Value == other.Value;
            }

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

        public class ParseResultValues : IEquatable<ParseResultValues>
        {
            public Guid? UUID { get; }
            public uint? SerialNumber { get; }
            public byte? Ordinal { get; }

            public ParseResultValues(Guid? uuid, uint? serialNumber, byte? ordinal)
            {
                UUID = uuid;
                SerialNumber = serialNumber;
                Ordinal = ordinal;
            }

            public bool Equals([AllowNull] ParseResultValues other) => null != other && (ReferenceEquals(this, other) ||
                (UUID == other.UUID && SerialNumber == other.SerialNumber && Ordinal == other.Ordinal));

            public override bool Equals(object obj) => Equals(obj as ParseResultValues);

            public override int GetHashCode() => HashCode.Combine(UUID, SerialNumber, Ordinal);

            public override string ToString()
            {
                return (UUID.HasValue) ?
                    (SerialNumber.HasValue) ?
                        ((Ordinal.HasValue) ?
                            $"{{ UUID = {UUID.Value.ToString("d")}, SerialNumber = {SerialNumber.Value.ToString("x4")}, Ordinal = {Ordinal.Value.ToString("x2")} }}"
                            : $"{{ UUID = {UUID.Value.ToString("d")}, SerialNumber = {SerialNumber.Value.ToString("x4")}, Ordinal = null }}"
                        )
                        : ((Ordinal.HasValue) ?
                            $"{{ UUID = {UUID.Value.ToString("d")}, SerialNumber = null, Ordinal = {Ordinal.Value.ToString("x2")} }}"
                            : $"{{ UUID = {UUID.Value.ToString("d")}, SerialNumber = null, Ordinal = null }}"
                        )
                    : (SerialNumber.HasValue) ?
                        ((Ordinal.HasValue) ?
                            $"{{ UUID = null, SerialNumber = {SerialNumber.Value.ToString("x4")}, Ordinal = {Ordinal.Value.ToString("x2")} }}"
                            : $"{{ UUID = null, SerialNumber = {SerialNumber.Value.ToString("x4")}, Ordinal = null }}"
                        )
                        : ((Ordinal.HasValue) ?
                            $"{{ UUID = null, SerialNumber = null, Ordinal = {Ordinal.Value.ToString("x2")} }}"
                            : "{ UUID = null, SerialNumber = null, Ordinal = null }"
                        );
            }

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
