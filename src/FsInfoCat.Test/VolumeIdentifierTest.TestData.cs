using System;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using System.Net;
using System.IO;
using System.Net.Sockets;

namespace FsInfoCat.Test
{
    public partial class VolumeIdentifierTest
    {
        #region static fields

        private static readonly string _hostName;
        private static readonly string _systemDrivePath;
        private static readonly string _systemDriveUrl;
        private static readonly string _ipV2Address;
        private static readonly string _ipV6Address;

        static VolumeIdentifierTest()
        {
            _hostName = Environment.MachineName.ToLower();
            _systemDrivePath = Path.GetPathRoot(Environment.SystemDirectory);
            _systemDriveUrl = new Uri(_systemDrivePath, UriKind.Absolute).AbsoluteUri;
            IPAddress[] addressList = Dns.GetHostEntry(_hostName).AddressList;
            _ipV2Address = addressList.Where(a => a.AddressFamily == AddressFamily.InterNetwork)
                .Select(a => a.ToString()).DefaultIfEmpty("127.0.0.1").First();
            _ipV6Address = addressList.Where(a => a.AddressFamily == AddressFamily.InterNetworkV6)
                .Select(a => a.ToString()).DefaultIfEmpty("::1").First().Split('%')[0];
        }

        #endregion

        public static IEnumerable<TestCaseData> GetGuidToIdentifierStringTestCases() => UUIDTestValues.GetTestValues(false)
            .Select((t, i) => new TestCaseData(t.UUID)
                    .SetName($"GuidToIdentifierStringTest(Guid) // {i}. {t.UUID} {t.Description}")
                .Returns(t.StringValue.Substring(5)));

        public static IEnumerable<TestCaseData> GetSerialNumberToIdentifierStringTestCases() => SerialNumberTestValues.GetTestValues(false)
            .Select((t, i) => new TestCaseData(t.SerialNumber, (byte?)null)
                .SetName($"SerialNumberToIdentifierStringTest(uint, null) // {i}. {t.SerialNumber} {t.Description}")
                .Returns(t.StringValue.Substring(10)))
            .Concat(SerialNumberOrdinalTestValues.GetTestValues()
                .Select((t, i) => new TestCaseData(t.SerialNumber, t.Ordinal)
                    .SetName($"SerialNumberToIdentifierStringTest(uint, byte) // {i}. ({t.SerialNumber}, {t.Ordinal}) {t.Description}")
                    .Returns(t.StringValue.Substring(10))));

        public static IEnumerable<TestCaseData> GetGuidToUrnTestCases() => UUIDTestValues.GetTestValues(false)
            .Select((t, i) => new TestCaseData(t.UUID)
                .SetName($"GuidToUrnTest(Guid) // {i}. {t.UUID} {t.Description}")
                .Returns(t.Url.AbsoluteUri));

        public static IEnumerable<TestCaseData> GetSerialNumberToUrnTestCases() => SerialNumberTestValues.GetTestValues(false)
            .Select((t, i) => new TestCaseData(t.SerialNumber, (byte?)null)
                .SetName($"SerialNumberToUrnTest(uint) // {i}. {t.SerialNumber}) // {t.Description}")
                .Returns(t.Url.AbsoluteUri))
            .Concat(SerialNumberOrdinalTestValues.GetTestValues()
                .Select((t, i) => new TestCaseData(t.SerialNumber, t.Ordinal)
                    .SetName($"SerialNumberToUrnTest(uint, byte) // {i}. ({t.SerialNumber}, {t.Ordinal}) {t.Description}")
                    .Returns(t.Url.AbsoluteUri)));

        public static IEnumerable<TestCaseData> GetGuidParameterTestCases() => UUIDTestValues.GetTestValues(false)
            .Select((t, i) =>
                new TestCaseData(t.UUID)
                    .SetName($"FromGuidValueConstructorTest(Guid) // {i}. {t.UUID} {t.Description}")
                    .Returns(new IdValues(t.StringValue, t.Url.AbsoluteUri))
            );

        public static IEnumerable<TestCaseData> GetValidUUIDUriParameterTestCases() => UUIDTestValues.GetTestValues(true)
            .Select((t, i) =>
                new TestCaseData(t.UrnParam)
                    .SetName($"FromValidUUIDUriConstructorTest(urn:uuid<guid>) // {i}. \"{t.UrnParam}\" {t.Description}")
                    .Returns(new UuidValues(t.UUID, t.StringValue, t.Url.AbsoluteUri))
            );

        public static IEnumerable<TestCaseData> GetSerialNumberParameterTestCases() =>
            SerialNumberTestValues.GetTestValues(false, false).Select((t, i) =>
                new TestCaseData(t.SerialNumber)
                    .SetName($"FromSerialNumberValueConstructorTest(uint) // {i}. {t.SerialNumber} {t.Description}")
                    .Returns(new IdValues(t.StringValue, t.Url.AbsoluteUri))
            );

        public static IEnumerable<TestCaseData> GetSerialNumberAndOrdinalParametersTestCases() => SerialNumberOrdinalTestValues.GetTestValues(false)
            .Select((t, i) =>
                new TestCaseData(t.SerialNumber, t.Ordinal)
                    .SetName($"FromSerialNumberAndOrdinalValuesConstructorTest(uint, byte) // {i}. ({t.SerialNumber}, {t.Ordinal}) // {t.Description}")
                    .Returns(new SnIdValues(t.SerialNumber, t.Ordinal, t.StringValue, t.Url.AbsoluteUri))
            );

        public static IEnumerable<TestCaseData> GetValidSerialNumberUriParameterTestCases() => SerialNumberTestValues.GetTestValues(true, true)
            .Select((t, i) =>
                new TestCaseData(t.UrnParam)
                    .SetName($"FromValidSerialNumberAndOrdinalUriConstructorTest(urn:volume:id:<uint>) // {i}. \"{t.UrnParam}\" {t.Description}")
                    .Returns(new SnIdValues(t.SerialNumber, null, t.StringValue, t.Url.AbsoluteUri))
            ).Concat(SerialNumberOrdinalTestValues.GetTestValues(true).Select((t, i) =>
                new TestCaseData(t.UrnParam)
                    .SetName($"FromValidSerialNumberAndOrdinalUriConstructorTest(urn:volume:id:<uint>-<byte>) // {i}. \"{t.UrnParam}\" // {t.Description}")
                    .Returns(new SnIdValues(t.SerialNumber, t.Ordinal, t.StringValue, t.Url.AbsoluteUri))
            ));

        public static IEnumerable<TestCaseData> GetValidFilePathParameterTestCases()
        {
            string path = @"\\servicenowdiag479.file.core.windows.net\testazureshare";
            string expected = $@"{path}\";
            string url = "file://servicenowdiag479.file.core.windows.net/testazureshare/";
            yield return new TestCaseData(path)
                .SetName($"FromValidFilePathConstructorTest(file://<fqdn>) // \"{path}\"")
                .Returns(new IdValues(expected, url));

            yield return new TestCaseData(expected)
                .SetName($"FromValidFilePathConstructorTest(file://<fqdn>/) // \"{expected}\"")
                .Returns(new IdValues(expected, url));

            path = $@"\\{_hostName}\$Admin";
            expected = $@"{path}\";
            url = $"file://{_hostName}/$Admin/";
            yield return new TestCaseData(path)
                .SetName($"FromValidFilePathConstructorTest(file://<name>) // \"{path}\"")
                .Returns(new IdValues(expected, url));

            path = $@"\\{_hostName}\Admin$\";
            yield return new TestCaseData(expected)
                .SetName($"FromValidFilePathConstructorTest(file://<name>/) // \"{expected}\"")
                .Returns(new IdValues(expected, url));

            path = $@"\\{_ipV2Address}\Us&Them";
            expected = $@"{path}\";
            url = $"file://{_ipV2Address}/Us&Them/";
            yield return new TestCaseData(path)
                .SetName($"FromValidFilePathConstructorTest(file://<ipV2>) // \"{path}\"")
                .Returns(new IdValues(expected, url));

            path = $@"\\{_ipV2Address}\Us&Them\";
            yield return new TestCaseData(expected)
                .SetName($"FromValidFilePathConstructorTest(file://<ipV2>/) // \"{expected}\"")
                .Returns(new IdValues(expected, url));

            path = $@"\\[{_ipV6Address}]\100% Done";
            expected = $@"{path}\";
            url = $"file://[{_ipV6Address}]/100%25%20Done/";
            yield return new TestCaseData(path)
                .SetName($"FromValidFilePathConstructorTest(file://<ipV6>) // \"{path}\"")
                .Returns(new IdValues(expected, url));

            path = $@"\\[{_ipV6Address}]\100% Done\";
            yield return new TestCaseData(expected)
                .SetName($"FromValidFilePathConstructorTest(file://<ipV6>/) // \"{expected}\"")
                .Returns(new IdValues(expected, url));
        }

        public static IEnumerable<TestCaseData> GetValidFileUrlParameterTestCases()
        {
            string url = "file://servicenowdiag479.file.core.windows.net/testazureshare";
            string expected = $"{url}/";
            string path = @"\\servicenowdiag479.file.core.windows.net\testazureshare\";
            yield return new TestCaseData(url)
                .SetName($"FromValidFileUrlConstructorTest(urn:fqdn) // \"{url}\"")
                .Returns(new IdValues(path, expected));

            yield return new TestCaseData(expected)
                .SetName($"FromValidFileUrlConstructorTest(urn:fqdn/)) // \"{expected}\"")
                .Returns(new IdValues(path, expected));

            string noTrailingSlash = url;
            url = $"{noTrailingSlash}?";
            yield return new TestCaseData(url)
                .SetName($"FromValidFileUrlConstructorTest(urn:fqdn?) // \"{url}\"")
                .Returns(new IdValues(path, expected));

            url = $"{expected}?";
            yield return new TestCaseData(url)
                .SetName($"FromValidFileUrlConstructorTest(urn:fqdn/?) // \"{url}\"")
                .Returns(new IdValues(path, expected));

            url = $"{noTrailingSlash}#";
            yield return new TestCaseData(url)
                .SetName($"FromValidFileUrlConstructorTest(urn:fqdn#) // \"{url}\"")
                .Returns(new IdValues(path, expected));

            url = $"{expected}#";
            yield return new TestCaseData(url)
                .SetName($"FromValidFileUrlConstructorTest(urn:fqdn/#) // \"{url}\")")
                .Returns(new IdValues(path, expected));

            url = $"{noTrailingSlash}?#";
            yield return new TestCaseData(url)
                .SetName($"FromValidFileUrlConstructorTest(urn:fqdn?#) // \"{url}\")")
                .Returns(new IdValues(path, expected));

            url = $"{expected}?#";
            yield return new TestCaseData(url)
                .SetName($"FromValidFileUrlConstructorTest(urn:fqdn/?#) // \"{url}\")")
                .Returns(new IdValues(path, expected));

            url = $"file://{_hostName}/$Admin";
            expected = $"{url}/";
            path = $@"\\{_hostName}\$Admin\";
            yield return new TestCaseData(url)
                .SetName($"FromValidFileUrlConstructorTest(urn:name) // \"{url}\"")
                .Returns(new IdValues(path, expected));

            yield return new TestCaseData(expected)
                .SetName($"FromValidFileUrlConstructorTest(urn:name/) // \"{expected}\"")
                .Returns(new IdValues(path, expected));

            url = $"file://{_ipV2Address}/Us&Them";
            expected = $"{url}/";
            path = $@"\\{_ipV2Address}\Us&Them\";
            yield return new TestCaseData(url)
                .SetName($"FromValidFileUrlConstructorTest(urn:ipV2) // \"{url}\"")
                .Returns(new IdValues(path, expected));

            yield return new TestCaseData(expected)
                .SetName($"FromValidFileUrlConstructorTest(urn:ipV2/) // \"{expected}\"")
                .Returns(new IdValues(path, expected));

            url = $"file://[{_ipV6Address}]/100%25%20Done";
            expected = $"{url}/";
            path = $@"\\[{_ipV6Address}]\100% Done\";
            yield return new TestCaseData(url)
                .SetName($"FromValidFileUrlConstructorTest(urn:ipV6) // \"{url}\"")
                .Returns(new IdValues(path, expected));

            yield return new TestCaseData(expected)
                .SetName($"FromValidFileUrlConstructorTest(urn:ipV6/) // \"{expected}\"")
                .Returns(new IdValues(path, expected));
        }
    }
}
