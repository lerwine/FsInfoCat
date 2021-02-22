using System;
using NUnit.Framework;
using FsInfoCat.Util;
using System.Collections.Generic;
using System.Reflection;
using FsInfoCat.Models.Crawl;

namespace FsInfoCat.Test
{
    [TestFixture]
    public class UtilExtensionMethodsTest
    {
        [SetUp]
        public void Setup()
        {
        }

        public static IEnumerable<TestCaseData> GetIsEqualToTestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetIsEqualToTestCases))]
        public bool IsEqualToTest(string s, char c)
        {
            return s.IsEqualTo(c);
        }

        public static IEnumerable<TestCaseData> GetEndsWithTestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetEndsWithTestCases))]
        public bool EndsWithTest(string s, char c)
        {
            return s.EndsWith(c);
        }

        public static IEnumerable<TestCaseData> GetSplit1TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetSplit1TestCases))]
        public string[] Split1Test(string s, char c)
        {
            return s.Split(c);
        }

        public static IEnumerable<TestCaseData> GetSplit2TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetSplit2TestCases))]
        public string[] Split2Test(string s, char c, int count)
        {
            return s.Split(c, count);
        }

        public static IEnumerable<TestCaseData> GetTryDequeueTestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryDequeueTestCases))]
        public Tuple<bool, string> TryDequeueTest(Queue<string> source)
        {
            return new Tuple<bool, string>(source.TryDequeue(out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryGetDescription1TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryGetDescription1TestCases))]
        public Tuple<bool, string> TryGetDescription1Test(MemberInfo memberInfo)
        {
            return new Tuple<bool, string>(memberInfo.TryGetDescription(out string result), result);
        }

        public static IEnumerable<TestCaseData> GetTryGetDescription2TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryGetDescription2TestCases))]
        public Tuple<bool, string, string> TryGetDescription2Test(MessageId messageId)
        {
            return new Tuple<bool, string, string>(messageId.TryGetDescription(out string result, out string name), result, name);
        }

        public static IEnumerable<TestCaseData> GetTryGetDescription3TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryGetDescription3TestCases))]
        public Tuple<bool, string> TryGetDescription3Test(MessageId messageId)
        {
            return new Tuple<bool, string>(messageId.TryGetDescription(out string result), result);
        }

        public static IEnumerable<TestCaseData> GetGetDescription1TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetGetDescription1TestCases))]
        public string GetDescription1Test(MemberInfo memberInfo)
        {
            return memberInfo.GetDescription();
        }

        public static IEnumerable<TestCaseData> GetGetDescription2TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetGetDescription2TestCases))]
        public string GetDescription2Test(MessageId messageId)
        {
            return messageId.GetDescription();
        }

        public static IEnumerable<TestCaseData> GetGetDescription3TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetGetDescription3TestCases))]
        public string GetDescription3Test(MessageId messageId, Func<string, string> getDefaultDescription)
        {
            return messageId.GetDescription(getDefaultDescription);
        }

        public static IEnumerable<TestCaseData> GetGetDescription4TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetGetDescription4TestCases))]
        public Tuple<string, string> GetDescription4Test(MessageId messageId)
        {
            return new Tuple<string, string>(messageId.GetDescription(out string name), name);
        }

        public static IEnumerable<TestCaseData> GetGetDescriptionTestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetGetDescriptionTestCases))]
        public Tuple<string, string> GetDescription5Test(MessageId value, Func<string, string> getDefaultDescription)
        {
            return new Tuple<string, string>(value.GetDescription(getDefaultDescription, out string name), name);
        }

        public static IEnumerable<TestCaseData> GetGetDescription6TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetGetDescription6TestCases))]
        public string GetDescription6Test(MessageId value, Func<MessageId, string> getDefaultDescription)
        {
            return value.GetDescription(getDefaultDescription);
        }

        public static IEnumerable<TestCaseData> GetGetDescription7TestCases() => throw new InconclusiveException("Test data not implemented");

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetGetDescription7TestCases))]
        public Tuple<string, string> GetDescription7Test(MessageId value, Func<MessageId, string> getDefaultDescription)
        {
            return new Tuple<string, string>(value.GetDescription(getDefaultDescription, out string name), name);
        }
    }
}
