using FsInfoCat.Util;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FsInfoCat.Test
{
    [TestFixture]
    public class UtilExtensionMethodsTest
    {
        [SetUp]
        public void Setup()
        {
        }

        public static IEnumerable<TestCaseData> GetIsEqualTo1TestCases()
        {
            var cd = (new char[] { ' ', 'x', 'y' }).Select(c => new { C = c, D = $"{c}" })
                .Concat(new[] { new { C = '\n', D = "\\n" }, new { C = '\x00', D = "\\x00" } });
            return cd.Select(a =>
                new TestCaseData(null, a.C)
                    .SetDescription($"s: null, c: '{a.D}'")
                    .Returns(false)
            ).Concat(cd.Select(a =>
                new TestCaseData("", a.C)
                    .SetDescription($"s: \"\", c: '{a.D}'")
                    .Returns(false)
            )).Concat(cd.SelectMany(a => cd.SelectMany(b => cd.Select(c =>
                new TestCaseData($"{a.C}{b.C}", c.C)
                    .SetDescription($"s: \"{a.D}{b.D}\", c: '{c.D}'")
                    .Returns(false)
            )))).Concat(cd.SelectMany(a => cd.Select(b =>
                new TestCaseData($"{a.C}", b.C)
                    .SetDescription($"s: \"{a.D}\", c: '{b.D}'")
                    .Returns(a.C.Equals(b.C))
            )));
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetIsEqualTo1TestCases))]
        public bool IsEqualToTest1(string s, char c)
        {
            return s.IsEqualTo(c);
        }

        public static IEnumerable<TestCaseData> GetIsEqualTo2TestCases()
        {
            Func<string, string> toDescription = t => (t is null) ? "null" : $"\"{t.Replace("\n", "\\n").Replace("\x0000", "\\x0000")}\"";

            return (new string[] { null, "", "\n", "\x0000", "a", "BC", "1234" }).SelectMany(t => (new int[] { 0, 1, 2, 4, 5 }).Select(length =>
                    new { Length = length, Target = t, Other = t }
                ))
                .Concat(
                    (new string[] { "", "\n", "\x0000", "a", "BC", "1234" }).SelectMany(t => (new string[] { "", "\n", "\x0000", "a", "BC", "1234" }).Select(o =>
                       new { Length = 0, Target = t, Other = o }
                    ))
                )
                .Concat(
                    (new string[] { "x\n", "x\x0000", "xa", "xBC", "x1234" }).SelectMany(t => (new string[] { "x\n", "x\x0000", "xa", "xBC", "x1234" }).Select(o =>
                       new { Length = 1, Target = t, Other = o }
                    ))
                )
                .Concat(
                    (new string[] { "\x0000\n", "\x0000a", "\x00001234" }).SelectMany(t => (new string[] { "\x0000\n", "\x0000a", "\x00001234" }).Select(o =>
                       new { Length = 1, Target = t, Other = o }
                    ))
                )
                .Concat(
                    (new string[] { "BC", "BCa", "BC1234" }).SelectMany(t => (new string[] { "BC\n", "BC", "BC1" }).Select(o =>
                       new { Length = 1, Target = t, Other = o }
                    ))
                )
                .Concat(
                    (new string[] { "\nC", "\nCa", "\nC1234" }).SelectMany(t => (new string[] { "\nC\n", "\nC", "\nC1" }).Select(o =>
                       new { Length = 1, Target = t, Other = o }
                    ))
                )
                .Concat(
                    (new string[] { "x\nC3", "x\nC3a", "x\nC31234" }).SelectMany(t => (new string[] { "x\nC3", "x\nC3C", "x\nC31" }).Select(o =>
                       new { Length = 4, Target = t, Other = o }
                    ))
                )
                .Select(a => new TestCaseData(a.Target, a.Other, a.Length)
                        .SetDescription($"target = {toDescription(a.Target)}, other = {toDescription(a.Other)}, startIndex = {a.Length}")
                        .Returns(true)).Concat(
                    (new string[] { "", "a", "1234" }).SelectMany(t => (new string[] { null, "\n", "\x0000", "BC" }).SelectMany(o => new[]
                        {
                            new { Length = 1, Target = t, Other = o },
                            new { Length = 1, Target = o, Other = t }
                        }
                    ))
                    .Concat(
                        (new string[] { "x\n", "xa", "x1234" }).SelectMany(t => (new string[] { "x\x0000", "xBC", "y1234" }).SelectMany(o => new[]
                            {
                                new { Length = 2, Target = t, Other = o },
                                new { Length = 2, Target = o, Other = t }
                            }
                        ))
                    )
                    .Concat(
                        (new string[] { "x\n", "xa", "x12 " }).SelectMany(t => (new string[] { "x\x0000", "xBC", "x1234" }).SelectMany(o => new[]
                            {
                                new { Length = 4, Target = t, Other = o },
                                new { Length = 4, Target = o, Other = t }
                            }
                        ))
                    )
                    .Select(a => new TestCaseData(a.Target, a.Other, a.Length)
                            .SetDescription($"target = {toDescription(a.Target)}, other = {toDescription(a.Other)}, startIndex = {a.Length}")
                            .Returns(false))
                );
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetIsEqualTo2TestCases))]
        public bool IsEqualToTest2(string target, string other, int length)
        {
            return target.IsEqualTo(other, length);
        }

        public static IEnumerable<TestCaseData> GetEndsWithTestCases()
        {
            yield return new TestCaseData("t", 't')
                .SetDescription($"s: \"t\", c: 't'")
                .Returns(true);
            yield return new TestCaseData("tc", 't')
                .SetDescription($"s: \"tc\", c: 't'")
                .Returns(false);
            yield return new TestCaseData("tc", 'c')
                .SetDescription($"s: \"tc\", c: 'c'")
                .Returns(true);
            yield return new TestCaseData("T", 't')
                .SetDescription($"s: \"T\", c: 't'")
                .Returns(false);
            yield return new TestCaseData("t", 'T')
                .SetDescription($"s: \"t\", c: 'T'")
                .Returns(false);
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetEndsWithTestCases))]
        public bool EndsWithTest(string s, char c)
        {
            return s.EndsWith(c);
        }

        public static IEnumerable<TestCaseData> GetSplit1TestCases()
        {
            yield return new TestCaseData("t", 't')
                .SetDescription($"s: \"t\", c: 't'")
                .Returns(new string[] { "", "" });
            yield return new TestCaseData("tc", 't')
                .SetDescription($"s: \"tc\", c: 't'")
                .Returns(new string[] { "", "c" });
            yield return new TestCaseData("tc", 'c')
                .SetDescription($"s: \"tc\", c: 'c'")
                .Returns(new string[] { "t", "" });
            yield return new TestCaseData("T", 't')
                .SetDescription($"s: \"T\", c: 't'")
                .Returns(new string[] { "T" });
            yield return new TestCaseData("t", 'T')
                .SetDescription($"s: \"t\", c: 'T'")
                .Returns(new string[] { "t" });
            yield return new TestCaseData("one,two, three ,  four  ,five", ',')
                .SetDescription($"s: \"one,two, three ,  four  ,five\", c: ','")
                .Returns(new string[] { "one", "two", " three ", "  four  ", "five" });
            yield return new TestCaseData("one,two, three ,  four  ,five,", ',')
                .SetDescription($"s: \"one,two, three ,  four  ,five,\", c: ','")
                .Returns(new string[] { "one", "two", " three ", "  four  ", "five", "" });
            yield return new TestCaseData(",one,two, three ,  four  ,five", ',')
                .SetDescription($"s: \",one,two, three ,  four  ,five\", c: ','")
                .Returns(new string[] { "", "one", "two", " three ", "  four  ", "five" });
            yield return new TestCaseData(",one,two, three ,  four  ,five,", ',')
                .SetDescription($"s: \",one,two, three ,  four  ,five,\", c: ','")
                .Returns(new string[] { "", "one", "two", " three ", "  four  ", "five", "" });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetSplit1TestCases))]
        public string[] Split1Test(string s, char c)
        {
            return s.Split(c);
        }

        public static IEnumerable<TestCaseData> GetSplit2TestCases()
        {
            yield return new TestCaseData("t", 't', 1)
                .SetDescription($"s: \"t\", c: 't', count: 1")
                .Returns(new string[] { "t" });
            yield return new TestCaseData("t", 't', 2)
                .SetDescription($"s: \"t\", c: 't', count: 2")
                .Returns(new string[] { "", "" });
            yield return new TestCaseData("t", 't', 3)
                .SetDescription($"s: \"t\", c: 't', count: 3")
                .Returns(new string[] { "", "" });

            yield return new TestCaseData("tc", 't', 1)
                .SetDescription($"s: \"tc\", c: 't', count: 1")
                .Returns(new string[] { "tc" });
            yield return new TestCaseData("tc", 't', 2)
                .SetDescription($"s: \"tc\", c: 't', count: 2")
                .Returns(new string[] { "", "c" });
            yield return new TestCaseData("tc", 't', 3)
                .SetDescription($"s: \"tc\", c: 't', count: 3")
                .Returns(new string[] { "", "c" });

            yield return new TestCaseData("tc", 'c', 1)
                .SetDescription($"s: \"tc\", c: 'c', count: 1")
                .Returns(new string[] { "tc" });
            yield return new TestCaseData("tc", 'c', 2)
                .SetDescription($"s: \"tc\", c: 'c', count: 2")
                .Returns(new string[] { "t", "" });
            yield return new TestCaseData("tc", 'c', 3)
                .SetDescription($"s: \"tc\", c: 'c', count: 3")
                .Returns(new string[] { "t", "" });

            yield return new TestCaseData("T", 't', 1)
                .SetDescription($"s: \"T\", c: 't', count: 1")
                .Returns(new string[] { "T" });
            yield return new TestCaseData("T", 't', 2)
                .SetDescription($"s: \"T\", c: 't', count: 2")
                .Returns(new string[] { "T" });
            yield return new TestCaseData("T", 't', 3)
                .SetDescription($"s: \"T\", c: 't', count: 3")
                .Returns(new string[] { "T" });

            yield return new TestCaseData("t", 'T', 1)
                .SetDescription($"s: \"t\", c: 'T', count: 1")
                .Returns(new string[] { "t" });
            yield return new TestCaseData("t", 'T', 2)
                .SetDescription($"s: \"t\", c: 'T', count: 2")
                .Returns(new string[] { "t" });
            yield return new TestCaseData("t", 'T', 3)
                .SetDescription($"s: \"t\", c: 'T', count: 3")
                .Returns(new string[] { "t" });

            yield return new TestCaseData("one,two, three ,  four  ,five", ',', 1)
                .SetDescription($"s: \"one,two, three ,  four  ,five\", c: ',', count: 1")
                .Returns(new string[] { "one,two, three ,  four  ,five" });
            yield return new TestCaseData("one,two, three ,  four  ,five", ',', 4)
                .SetDescription($"s: \"one,two, three ,  four  ,five\", c: ',', count: 4")
                .Returns(new string[] { "one", "two", " three ", "  four  ,five" });
            yield return new TestCaseData("one,two, three ,  four  ,five", ',', 6)
                .SetDescription($"s: \"one,two, three ,  four  ,five\", c: ',', count: 6")
                .Returns(new string[] { "one", "two", " three ", "  four  ", "five" });

            yield return new TestCaseData("one,two, three ,  four  ,five,", ',', 1)
                .SetDescription($"s: \"one,two, three ,  four  ,five,\", c: ',', count: 1")
                .Returns(new string[] { "one,two, three ,  four  ,five," });
            yield return new TestCaseData("one,two, three ,  four  ,five,", ',', 4)
                .SetDescription($"s: \"one,two, three ,  four  ,five,\", c: ',', count: 4")
                .Returns(new string[] { "one", "two", " three ", "  four  ,five," });
            yield return new TestCaseData("one,two, three ,  four  ,five,", ',', 6)
                .SetDescription($"s: \"one,two, three ,  four  ,five,\", c: ',', count: 6")
                .Returns(new string[] { "one", "two", " three ", "  four  ", "five", "" });

            yield return new TestCaseData(",one,two, three ,  four  ,five", ',', 1)
                .SetDescription($"s: \",one,two, three ,  four  ,five\", c: ',', count: 1")
                .Returns(new string[] { ",one,two, three ,  four  ,five" });
            yield return new TestCaseData(",one,two, three ,  four  ,five", ',', 4)
                .SetDescription($"s: \",one,two, three ,  four  ,five\", c: ',', count: 4")
                .Returns(new string[] { "", "one", "two", " three ,  four  ,five" });
            yield return new TestCaseData(",one,two, three ,  four  ,five", ',', 6)
                .SetDescription($"s: \",one,two, three ,  four  ,five\", c: ',', count: 6")
                .Returns(new string[] { "", "one", "two", " three ", "  four  ", "five" });

            yield return new TestCaseData(",one,two, three ,  four  ,five,", ',', 1)
                .SetDescription($"s: \",one,two, three ,  four  ,five,\", c: ',', count: 1")
                .Returns(new string[] { ",one,two, three ,  four  ,five," });
            yield return new TestCaseData(",one,two, three ,  four  ,five,", ',', 4)
                .SetDescription($"s: \",one,two, three ,  four  ,five,\", c: ',', count: 4")
                .Returns(new string[] { "", "one", "two", " three ,  four  ,five," });
            yield return new TestCaseData(",one,two, three ,  four  ,five,", ',', 6)
                .SetDescription($"s: \",one,two, three ,  four  ,five,\", c: ',', count: 6")
                .Returns(new string[] { "", "one", "two", " three ", "  four  ", "five," });
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetSplit2TestCases))]
        public string[] Split2Test(string s, char c, int count)
        {
            return s.Split(c, count);
        }

        public static IEnumerable<TestCaseData> GetTryDequeueTestCases()
        {
            yield return new TestCaseData(new Queue<string>())
                .SetDescription("{}")
                .Returns(new Tuple<bool, string>(false, null));
            yield return new TestCaseData(new Queue<string>(new string[] { "First" }))
                .SetDescription("{ \"First\" }")
                .Returns(new Tuple<bool, string>(true, "First"));
            yield return new TestCaseData(new Queue<string>(new string[] { "A", "B" }))
                .SetDescription("{ \"A\", \"B\" }")
                .Returns(new Tuple<bool, string>(true, "A"));
            yield return new TestCaseData(new Queue<string>(new string[] { "", "C" }))
                .SetDescription("{ \"\", \"C\" }")
                .Returns(new Tuple<bool, string>(true, ""));
            yield return new TestCaseData(new Queue<string>(new string[] { null, "C" }))
                .SetDescription("{ null, \"C\" }")
                .Returns(new Tuple<bool, string>(true, null));
        }

        [Test, Property("Priority", 1)]
        [TestCaseSource(nameof(GetTryDequeueTestCases))]
        public Tuple<bool, string> TryDequeueTest(Queue<string> source)
        {
            return new Tuple<bool, string>(source.TryDequeue(out string result), result);
        }

        /*
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
        */
    }
}
