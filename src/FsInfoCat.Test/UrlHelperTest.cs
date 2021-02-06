using System;
using NUnit.Framework;
using FsInfoCat.Util;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace FsInfoCat.Test
{
    [TestFixture]
    public class UrlHelperTest
    {
        [SetUp]
        public void Setup()
        {
        }


        public static IEnumerable<TestCaseData> GetAuthorityCaseInsensitiveEqualsTestCases()
        {
            yield return new TestCaseData(new Uri("", UriKind.Relative), new Uri("", UriKind.Relative))
                .SetName("AuthorityCaseInsensitiveEqualsTest: 2 Empty Uri values")
                .Returns(true);
            yield return new TestCaseData(new Uri(@"\\SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET\testazureshare", UriKind.Absolute),
                    new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute))
                .SetName("AuthorityCaseInsensitiveEqualsTest: 2 different original strings and case, same absolute URI")
                .Returns(true);
            yield return new TestCaseData(new Uri("http://tempuri.org/TEST", UriKind.Absolute), new Uri("http://tempuri.org/test", UriKind.Absolute))
                .SetName("AuthorityCaseInsensitiveEqualsTest: Same host, different path case")
                .Returns(false);
            yield return new TestCaseData(new Uri("http://tempuri.org/TEST", UriKind.Absolute), new Uri("HTTP://TEMPURI.ORG/test", UriKind.Absolute))
                .SetName("AuthorityCaseInsensitiveEqualsTest: Alternating cases")
                .Returns(false);
            yield return new TestCaseData(new Uri("http://tempuri.org:80", UriKind.Absolute), new Uri("http://tempuri.org:75", UriKind.Absolute))
                .SetName("AuthorityCaseInsensitiveEqualsTest: Same host, different port")
                .Returns(false);
            yield return new TestCaseData(new Uri("http://tempuri.org/", UriKind.Absolute), new Uri("http://tempuri.org", UriKind.Absolute))
                .SetName("AuthorityCaseInsensitiveEqualsTest: Same host, one  omitting root slash")
                .Returns(true);
            yield return new TestCaseData(new Uri("HTTP://tempuri.org/", UriKind.Absolute), new Uri("http://tempuri.org", UriKind.Absolute))
                .SetName("AuthorityCaseInsensitiveEqualsTest: Different case scheme")
                .Returns(true);
            yield return new TestCaseData(new Uri("http://tempuri.org/", UriKind.Absolute), new Uri("https://tempuri.org", UriKind.Absolute))
                .SetName("AuthorityCaseInsensitiveEqualsTest: Alternating scheme")
                .Returns(false);
            yield return new TestCaseData(new Uri("http://user@tempuri.org/", UriKind.Absolute), new Uri("http://used@tempuri.org", UriKind.Absolute))
                .SetName("AuthorityCaseInsensitiveEqualsTest: Alternating username")
                .Returns(false);
            yield return new TestCaseData(new Uri("http://tempuri.org/?test=one", UriKind.Absolute), new Uri("http://tempuri.org?test=two", UriKind.Absolute))
                .SetName("AuthorityCaseInsensitiveEqualsTest: Alternating query")
                .Returns(false);
            yield return new TestCaseData(new Uri("http://tempuri.org/?test=one", UriKind.Absolute), new Uri("http://tempuri.org?test=ONE", UriKind.Absolute))
                .SetName("AuthorityCaseInsensitiveEqualsTest: Alternating query case")
                .Returns(false);
            yield return new TestCaseData(new Uri("http://tempuri.org/?test=one#six", UriKind.Absolute), new Uri("http://tempuri.org?test=one#two", UriKind.Absolute))
                .SetName("AuthorityCaseInsensitiveEqualsTest: Alternating fragment")
                .Returns(false);
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource("GetAuthorityCaseInsensitiveEqualsTestCases")]
        public bool AuthorityCaseInsensitiveEqualsTest(Uri x, Uri y) => x.AuthorityCaseInsensitiveEquals(y);

        public static IEnumerable<TestCaseData> GetAsRelativeUriStringTestCases()
        {
            yield return new TestCaseData("")
                .SetName("AsRelativeUriStringTest: 2 Empty Uri values")
                .Returns("");
            yield return new TestCaseData(@"\\SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET\testazureshare")
                .SetName("AsRelativeUriStringTest: UNC path")
                .Returns("%5C%5CSERVICENOWDIAG479.FILE.CORE.WINDOWS.NET%5Ctestazureshare");
            yield return new TestCaseData("http://tempuri.org/TEST Path")
                .SetName("AsRelativeUriStringTest: Absolute URI")
                .Returns("/TEST%20Path");
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource("GetAsRelativeUriStringTestCases")]
        public string AsRelativeUriStringTest(string text) => text.AsRelativeUriString();

        public static IEnumerable<TestCaseData> GetAsUserNameComponentEncodedTestCases()
        {
            yield return new TestCaseData("")
                .SetName("AsUserNameComponentEncodedTest: 2 Empty Uri values")
                .Returns("");
            yield return new TestCaseData("!")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("!");
            yield return new TestCaseData("@")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%40");
            yield return new TestCaseData("#")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%23");
            yield return new TestCaseData("$")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("$");
            yield return new TestCaseData("%")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%25");
            yield return new TestCaseData("^")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%5E");
            yield return new TestCaseData("&")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("&");
            yield return new TestCaseData("*")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("*");
            yield return new TestCaseData("(")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("(");
            yield return new TestCaseData(")")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns(")");
            yield return new TestCaseData("_")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("_");
            yield return new TestCaseData("+")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("+");
            yield return new TestCaseData("-")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("-");
            yield return new TestCaseData("=")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("=");
            yield return new TestCaseData("[")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("[");
            yield return new TestCaseData("]")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("]");
            yield return new TestCaseData("\\")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%5C");
            yield return new TestCaseData(";")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns(";");
            yield return new TestCaseData("'")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("'");
            yield return new TestCaseData(".")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns(".");
            yield return new TestCaseData("/")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%2F");
            yield return new TestCaseData(">")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%3E");
            yield return new TestCaseData("?")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%3F");
            yield return new TestCaseData(":")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%3A");
            yield return new TestCaseData("\"")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%22");
            yield return new TestCaseData("{")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%7B");
            yield return new TestCaseData("}")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%7D");
            yield return new TestCaseData("|")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("%7C");
            yield return new TestCaseData(@"!@#$%^&*()_+-=[]\;'./>?:""{}|")
                .SetName("AsUserNameComponentEncodedTest: UNC path")
                .Returns("!%40%23$%25%5E&*()_+-=[]%5C;'.%2F%3E%3F%3A%22%7B%7D%7C");
            yield return new TestCaseData("http://tempuri.org/TEST Path")
                .SetName("AsUserNameComponentEncodedTest: Absolute URI")
                .Returns("http%3A%2F%2Ftempuri.org%2FTEST%20Path");
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource("GetAsUserNameComponentEncodedTestCases")]
        public string AsUserNameComponentEncodedTest(string text) => text.AsUserNameComponentEncoded();

        public static IEnumerable<TestCaseData> GetAsPasswordComponentEncodedTestCases()
        {
            yield return new TestCaseData("")
                .SetName("AsPasswordComponentEncodedTest: 2 Empty Uri values")
                .Returns("");
            yield return new TestCaseData("!")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("!");
            yield return new TestCaseData("@")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("%40");
            yield return new TestCaseData("#")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("%23");
            yield return new TestCaseData("$")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("$");
            yield return new TestCaseData("%")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("%25");
            yield return new TestCaseData("^")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("%5E");
            yield return new TestCaseData("&")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("&");
            yield return new TestCaseData("*")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("*");
            yield return new TestCaseData("(")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("(");
            yield return new TestCaseData(")")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns(")");
            yield return new TestCaseData("_")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("_");
            yield return new TestCaseData("+")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("+");
            yield return new TestCaseData("-")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("-");
            yield return new TestCaseData("=")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("=");
            yield return new TestCaseData("[")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("[");
            yield return new TestCaseData("]")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("]");
            yield return new TestCaseData("\\")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("%5C");
            yield return new TestCaseData(";")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns(";");
            yield return new TestCaseData("'")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("'");
            yield return new TestCaseData(".")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns(".");
            yield return new TestCaseData("/")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("%2F");
            yield return new TestCaseData(">")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("%3E");
            yield return new TestCaseData("?")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("%3F");
            yield return new TestCaseData(":")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns(":");
            yield return new TestCaseData("\"")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("%22");
            yield return new TestCaseData("{")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("%7B");
            yield return new TestCaseData("}")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("%7D");
            yield return new TestCaseData("|")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("%7C");
            yield return new TestCaseData(@"!@#$%^&*()_+-=[]\;'./>?:""{}|")
                .SetName("AsPasswordComponentEncodedTest: UNC path")
                .Returns("!%40%23$%25%5E&*()_+-=[]%5C;'.%2F%3E%3F:%22%7B%7D%7C");
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource("GetAsPasswordComponentEncodedTestCases")]
        public string AsPasswordComponentEncodedTest(string text) => text.AsPasswordComponentEncoded();

        public static IEnumerable<TestCaseData> GetGetUserNameAndPasswordTestTestCases()
        {
            yield return new TestCaseData(new Uri("", UriKind.Relative))
                .SetName("GetUserNameAndPasswordTest: Empty Uri")
                .Returns(new Tuple<string, string>(null, null));
            yield return new TestCaseData(new Uri("http://me:thepw@host.com", UriKind.Absolute))
                .SetName("GetUserNameAndPasswordTest: User and password")
                .Returns(new Tuple<string, string>("me", "thepw"));
            yield return new TestCaseData(new Uri("http://justuser@host.com", UriKind.Absolute))
                .SetName("GetUserNameAndPasswordTest: Only user")
                .Returns(new Tuple<string, string>("justuser", null));
            yield return new TestCaseData(new Uri("http://:justpw@host.com", UriKind.Absolute))
                .SetName("GetUserNameAndPasswordTest: Password with empty user")
                .Returns(new Tuple<string, string>("", "justpw"));
            yield return new TestCaseData(new Uri("http://host.com", UriKind.Absolute))
                .SetName("GetUserNameAndPasswordTest: No user or pw")
                .Returns(new Tuple<string, string>(null, null));
            yield return new TestCaseData(null)
                .SetName("GetUserNameAndPasswordTest: null value")
                .Returns(new Tuple<string, string>(null, null));
        }

        [Test, Property("Priority", 2)]
        [TestCaseSource("GetGetUserNameAndPasswordTestTestCases")]
        public Tuple<string, string> GetUserNameAndPasswordTest(Uri uri)
        {
            string userName = uri.GetUserNameAndPassword(out string password);
            return new Tuple<string, string>(userName, password);
        }

        public static IEnumerable<TestCaseData> GetTrySetUserInfoComponentTestTestCases()
        {
            yield return new TestCaseData(new Uri("http://me:thepw@host.com", UriKind.Absolute), "new", "pw")
                .SetName("TrySetUserInfoComponentTest: Replace username password")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://new:pw@host.com", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("http://justuser@host.com", UriKind.Absolute), "xyz", "pdq")
                .SetName("TrySetUserInfoComponentTest: Add username and password")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://xyz:pdq@host.com", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("http://:justpw@host.com", UriKind.Absolute), "xyz", "justpw")
                .SetName("TrySetUserInfoComponentTest: Set username")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://xyz:justpw@host.com", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("http://me:thepw@host.com", UriKind.Absolute), "me", "thepw")
                .SetName("TrySetUserInfoComponentTest: Setting same password")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://new:pw@host.com", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("http://me:thepw@host.com", UriKind.Absolute), null, null)
                .SetName("TrySetUserInfoComponentTest: Remove")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://host.com", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("http://me:thepw@host.com", UriKind.Absolute), null, "thepw")
                .SetName("TrySetUserInfoComponentTest: Remove by setting user to null")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://host.com", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("http://me:thepw@host.com", UriKind.Absolute), "me", null)
                .SetName("TrySetUserInfoComponentTest: Remove just pw")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://me@host.com", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("http://host.com", UriKind.Absolute), null, null)
                .SetName("TrySetUserInfoComponentTest: Remove nothing")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://host.com", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("thepw@host.com", UriKind.Relative), "new", "pw")
                .SetName("TrySetUserInfoComponentTest: Replace username password")
                .Returns(new Tuple<bool, Uri>(false, new Uri("thepw@host.com", UriKind.Relative)));
            yield return new TestCaseData(new Uri("file:///NoHost", UriKind.Absolute), "new", "pw")
                .SetName("TrySetUserInfoComponentTest: Replace username password")
                .Returns(new Tuple<bool, Uri>(false, new Uri("file:///NoHost", UriKind.Absolute)));
        }

        [Test, Property("Priority", 2)]
        [TestCaseSource("GetTrySetUserInfoComponentTestTestCases")]
        public Tuple<bool, Uri> TrySetUserInfoComponentTest(Uri uri, string userName, string password)
        {
            bool result = uri.TrySetUserInfoComponent(userName, password, out uri);
            return new Tuple<bool, Uri>(result, uri);
        }

        public static IEnumerable<TestCaseData> GetTrySetHostComponentTestTestCases()
        {
            yield return new TestCaseData(new Uri("http://me:thepw@host.com:8080/test?qry=true#mark", UriKind.Absolute), "new", (int?)null)
                .SetName("TrySetHostComponentTest: Replace between user info and path")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://me:thepw@new/test?qry=true#mark", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("file://mysite/mypath", UriKind.Absolute), null, (int?)null)
                .SetName("TrySetHostComponentTest: Remove")
                .Returns(new Tuple<bool, Uri>(true, new Uri("file:///mypath", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("file:///mypath", UriKind.Absolute), "mysite", (int?)null)
                .SetName("TrySetHostComponentTest: Insert")
                .Returns(new Tuple<bool, Uri>(true, new Uri("file://mysite/mypath", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("http://me:thepw@host.com:8080", UriKind.Absolute), "new", 9090)
                .SetName("TrySetHostComponentTest: Replace after user info and no path, with port")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://me:thepw@new:9090", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("http://me:thepw@host.com:8080", UriKind.Absolute), "new", (int?)null)
                .SetName("TrySetHostComponentTest: Replace after user info and no path")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://me:thepw@new", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("http://host.com:8080/test?qry=true#mark", UriKind.Absolute), "new", 9000)
                .SetName("TrySetHostComponentTest: Replace between scheme and path, with port")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://new:9000/test?qry=true#mark", UriKind.Absolute)));
            yield return new TestCaseData(new Uri("http://host.com:8080/test?qry=true#mark", UriKind.Absolute), "new", (int?)null)
                .SetName("TrySetHostComponentTest: Replace between scheme and path")
                .Returns(new Tuple<bool, Uri>(true, new Uri("http://new/test?qry=true#mark", UriKind.Absolute)));
        }

        [Test, Property("Priority", 2)]
        [TestCaseSource("GetTrySetHostComponentTestTestCases")]
        public Tuple<bool, Uri> TrySetHostComponentTest(Uri uri, string hostName, int? port)
        {
            bool result = uri.TrySetHostComponent(hostName, port, out uri);
            return new Tuple<bool, Uri>(result, uri);
        }

        public static IEnumerable<TestCaseData> GetAsNormalizedTestCases()
        {
            yield return new TestCaseData(new Uri("", UriKind.Relative))
                .SetName("AsNormalizedTest: Empty Uri")
                .Returns(new Uri("", UriKind.Relative));
            yield return new TestCaseData(new Uri("?", UriKind.Relative))
                .SetName("AsNormalizedTest: Relative with empty query")
                .Returns(new Uri("", UriKind.Relative));
            yield return new TestCaseData(new Uri("#", UriKind.Relative))
                .SetName("AsNormalizedTest: Relative with empty fragment")
                .Returns(new Uri("", UriKind.Relative));
            yield return new TestCaseData(new Uri("?#", UriKind.Relative))
                .SetName("AsNormalizedTest: Relative with empty query and fragment")
                .Returns(new Uri("", UriKind.Relative));
            yield return new TestCaseData(new Uri(@"\\SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET\testazureshare", UriKind.Absolute))
                .SetName("AsNormalizedTest: Upper case UNC host file URI")
                .Returns(new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute));
            yield return new TestCaseData(new Uri("FILE://SERVICENOWDIAG479.FILE.CORE.WINDOWS.NET/TESTAZURESHARE?#", UriKind.Absolute))
                .SetName("AsNormalizedTest: URL totally upper case")
                .Returns(new Uri("file://servicenowdiag479.file.core.windows.net/TESTAZURESHARE", UriKind.Absolute));
            foreach (ValueAndExpectedResult<Uri, Uri> testData in GetFileUriValues())
                yield return new TestCaseData(testData.Value)
                    .SetName(testData.Description)
                    .Returns(testData.ExpectedResult);
        }

        [Test, Property("Priority", 3)]
        [TestCaseSource("GetAsNormalizedTestCases")]
        public Uri AsNormalizedTest(Uri uri) => uri.AsNormalized();

        public class ValueAndDescription<T>
        {
            public ValueAndDescription(T value, string description)
            {
                Value = value;
                Description = description;
            }

            public T Value { get; }
            public string Description { get; }
        }

        public class ValueAndExpectedResult<TValue, TResult> : ValueAndDescription<TValue>
        {
            public ValueAndExpectedResult(TValue value, TResult expectedResult, string description) : base(value, description)
            {
                ExpectedResult = expectedResult;
            }

            public TResult ExpectedResult { get; }
        }

        public class SerialNumberValueAndDescription<T> : ValueAndDescription<T>
        {
            public SerialNumberValueAndDescription(uint serialNumber, byte? ordinal, T value, string description)
                : base(value, description)
            {
                SerialNumber = serialNumber;
                Ordinal = ordinal;
            }

            public uint SerialNumber { get; }
            public byte? Ordinal { get; }
        }

        public class UuidValueAndDescription<T> : ValueAndDescription<T>
        {
            public UuidValueAndDescription(Guid uuid, T value, string description)
                : base(value, description)
            {
                UUID = uuid;
            }

            public Guid UUID { get; }
        }

        public class SerialNumberUrlAndDescription : SerialNumberValueAndDescription<Uri>
        {
            public SerialNumberUrlAndDescription(uint serialNumber, byte? ordinal, Uri value, string description) : base(serialNumber, ordinal, value, description)
            {
            }

            public static SerialNumberUrlAndDescription Create(ValueAndDescription<uint> serialNumber, ValueAndDescription<byte> ordinal)
            {
                if (serialNumber is null)
                    throw new ArgumentNullException(nameof(serialNumber));
                if (ordinal is null)
                {
                    string id = $"{(serialNumber.Value << 8).ToString("x4")}-{(serialNumber.Value & 0xffff).ToString("x4")}";
                    return new SerialNumberUrlAndDescription(serialNumber.Value, null,
                        new Uri($"urn:volume:id:{id}", UriKind.Absolute),
                        $"serialNumber = {id} ({serialNumber.Description})");
                }
                string snId = serialNumber.Value.ToString("x8");
                string ordId = ordinal.Value.ToString("x2");
                    return new SerialNumberUrlAndDescription(serialNumber.Value, ordinal.Value,
                        new Uri($"urn:volume:id:{snId}-{ordId}", UriKind.Absolute),
                        $"serialNumber = {snId} ({serialNumber.Description}), ordinal = {ordId} ({ordinal.Description})");
            }
        }

        public static IEnumerable<ValueAndDescription<Guid>> GetUuidValues()
        {
            yield return new ValueAndDescription<Guid>(new Guid("39adc116-682d-11eb-ae93-0242ac130002"), "Version 1 UUID");
            yield return new ValueAndDescription<Guid>(new Guid("77e419fa-2146-35f0-92a3-721b6b2536a9"), "Version 3 UUID");
            yield return new ValueAndDescription<Guid>(new Guid("552841f8-3407-4608-b1d5-c129039539e2"), "Version 4 UUID");
            yield return new ValueAndDescription<Guid>(Guid.Empty, "Emtpy UUID");
        }

        public static IEnumerable<ValueAndDescription<uint>> GetSerialNumberValues()
        {
            yield return new ValueAndDescription<uint>(0U, "Zero value");
            yield return new ValueAndDescription<uint>((uint)int.MaxValue, "Bit-wise equivalent of Int32.MaxValue");
            yield return new ValueAndDescription<uint>(uint.MaxValue, "Bit-wise equivalent of -1");
            yield return new ValueAndDescription<uint>(0xe5b42303U, "Random value > Int32.MaxValue");
        }

        public static IEnumerable<ValueAndDescription<byte>> GetOrdinalValues()
        {
            yield return new ValueAndDescription<byte>(0x00, "Zero value");
            yield return new ValueAndDescription<byte>(0x07, "Value 7");
            yield return new ValueAndDescription<byte>(0xA0, "Value 10");
            yield return new ValueAndDescription<byte>(0xFF, "Max value");
        }

        public static IEnumerable<ValueAndExpectedResult<Uri, Uri>> GetFileUriValues()
        {
            yield return new ValueAndExpectedResult<Uri, Uri>(
                new Uri(@"\\servicenowdiag479.file.core.windows.net\testazureshare", UriKind.Absolute),
                new Uri("file://servicenowdiag479.file.core.windows.net/testazureshare", UriKind.Absolute),
                "From UNC path");
            yield return new ValueAndExpectedResult<Uri, Uri>(
                new Uri(@"C:\Users\lerwi\AppData\Roaming\Microsoft\Windows\Start Menu\Programs\Administrative Tools", UriKind.Absolute),
                new Uri(@"file:///C:/Users/lerwi/AppData/Roaming/Microsoft/Windows/Start%20Menu/Programs/Administrative%20Tools", UriKind.Absolute),
                "From program files folder");
            yield return new ValueAndExpectedResult<Uri, Uri>(
                new Uri("file:///my/folder", UriKind.Absolute),
                new Uri("file:///my/folder", UriKind.Absolute),
                "Local without explicit host name");
            yield return new ValueAndExpectedResult<Uri, Uri>(
                new Uri("file://192.168.1.1/my/folder", UriKind.Absolute),
                new Uri("file://192.168.1.1/my/folder", UriKind.Absolute),
                "IPV2 address");
            yield return new ValueAndExpectedResult<Uri, Uri>(
                new Uri("file://[fe80::1dee:91b0:4872:1f9]/my/folder", UriKind.Absolute),
                new Uri("file://[fe80::1dee:91b0:4872:1f9]/my/folder", UriKind.Absolute),
                "IPV6 address");
        }

        public static IEnumerable<ValueAndDescription<Uri>> GetHttpUriValues()
        {
            yield return new ValueAndDescription<Uri>(new Uri("https://docs.microsoft.com/en-us/aspnet/core/data/ef-rp/complex-data-model?view=aspnetcore-5.0&tabs=visual-studio#many-to-many-relationships", UriKind.Absolute),
                "Path, query and fragment");
            yield return new ValueAndDescription<Uri>(new Uri("http://tempuri.org", UriKind.Absolute), "Host only");
            yield return new ValueAndDescription<Uri>(new Uri("http://user:pw@erwinefamily.net/feed/v2", UriKind.Absolute), "Username and PW");
            yield return new ValueAndDescription<Uri>(new Uri("file://test@myhost.com", UriKind.Absolute), "Username only");
        }

        public static IEnumerable<ValueAndDescription<Uri>> GetRelativeUriValues()
        {
            yield return new ValueAndDescription<Uri>(new Uri("", UriKind.Absolute), "Empty");
            yield return new ValueAndDescription<Uri>(new Uri("/", UriKind.Absolute), "Root");
            yield return new ValueAndDescription<Uri>(new Uri("MyFolder/", UriKind.Absolute), "No root");
            yield return new ValueAndDescription<Uri>(new Uri("/one/two/three?key=X&value=7#mark", UriKind.Absolute), "Multi-segment path with query and fragment");
        }

        public static IEnumerable<SerialNumberValueAndDescription<Uri>> GetVolumeSerialNumberUriValues() => GetSerialNumberValues().Select(sn =>
        {
            string id = $"{(sn.Value << 8).ToString("x4")}-{(sn.Value & 0xffff).ToString("x4")}";
            return new SerialNumberValueAndDescription<Uri>(sn.Value, null, new Uri($"urn:volume:id:{id}", UriKind.Absolute), sn.Description);
        });

        public static IEnumerable<SerialNumberUrlAndDescription> GetVolumeSerialNumberAndOrdinalUriValues() => GetSerialNumberValues().SelectMany(sn =>
            GetOrdinalValues().Select(o => SerialNumberUrlAndDescription.Create(sn, o))
        );

        public static IEnumerable<UuidValueAndDescription<Uri>> GetVolumeUUIDUriValues() => GetUuidValues().Select(uuid =>
        {
            return new UuidValueAndDescription<Uri>(uuid.Value, new Uri($"urn:uuid:{uuid.Value.ToString("d").ToLower()}", UriKind.Absolute), uuid.Description);
        });
    }
}
