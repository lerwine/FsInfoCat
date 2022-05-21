using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class StringExtensionMethodsTests
    {
        private static TestContext _testContext;

        [ClassInitialize]
        public static void OnClassInitialize(TestContext testContext)
        {
            _testContext = testContext;
        }

        [DataTestMethod]
        [DataRow("Test $ With, Spaces.", "Test $ With, Spaces.", 21, DisplayName = "input = \"Test $ With, Spaces.\", maxLength = 21")]
        [DataRow("Test $ With, Spaces.", "Test $ With, Spaces.", 20, DisplayName = "input = \"Test $ With, Spaces.\", maxLength = 20")]
        [DataRow("Test $ With, Spaces.", "Test $ With,…", 19, DisplayName = "input = \"Test $ With, Spaces.\", maxLength = 19")]
        [DataRow("Test $ With, Spaces.", "Test $ With,…", 13, DisplayName = "input = \"Test $ With, Spaces.\", maxLength = 13")]
        [DataRow("Test $ With, Spaces.", "Test $…", 12, DisplayName = "input = \"Test $ With, Spaces.\", maxLength = 12")]
        [DataRow("Test $ With, Spaces.", "Test $…", 7, DisplayName = "input = \"Test $ With, Spaces.\", maxLength = 7")]
        [DataRow("Test $ With, Spaces.", "Test…", 6, DisplayName = "input = \"Test $ With, Spaces.\", maxLength = 6")]
        [DataRow("Test $ With, Spaces.", "Test…", 5, DisplayName = "input = \"Test $ With, Spaces.\", maxLength = 5")]
        [DataRow("Test $ With, Spaces.", "Tes…", 4, DisplayName = "input = \"Test $ With, Spaces.\", maxLength = 4")]
        [DataRow("Test $ With, Spaces.", "Te…", 3, DisplayName = "input = \"Test $ With, Spaces.\", maxLength = 3")]
        [DataRow("Test;With!,Punctuation...", "Test;With!,Punctuation...", 26, DisplayName = "input = \"Test;With!,Punctuation...\", maxLength = 26")]
        [DataRow("Test;With!,Punctuation...", "Test;With!,Punctuation...", 25, DisplayName = "input = \"Test;With!,Punctuation...\", maxLength = 25")]
        [DataRow("Test;With!,Punctuation...", "Test;With!,Punctuation.…", 24, DisplayName = "input = \"Test;With!,Punctuation...\", maxLength = 24")]
        [DataRow("Test;With!,Punctuation...", "Test;With!…", 23, DisplayName = "input = \"Test;With!,Punctuation...\", maxLength = 23")]
        [DataRow("Test;With!,Punctuation...", "Test;With!…", 13, DisplayName = "input = \"Test;With!,Punctuation...\", maxLength = 13")]
        [DataRow("Test;With!,Punctuation...", "Test;With!…", 12, DisplayName = "input = \"Test;With!,Punctuation...\", maxLength = 12")]
        [DataRow("Test;With!,Punctuation...", "Test;With!…", 11, DisplayName = "input = \"Test;With!,Punctuation...\", maxLength = 11")]
        [DataRow("Test;With!,Punctuation...", "Test;…", 10, DisplayName = "input = \"Test;With!,Punctuation...\", maxLength = 10")]
        [DataRow("Test$With<=Symbols>", "Test$With<=Symbols>", 20, DisplayName = "input = \"Test$With<=Symbols>\", maxLength = 20")]
        [DataRow("Test$With<=Symbols>", "Test$With<=Symbols>", 19, DisplayName = "input = \"Test$With<=Symbols>\", maxLength = 19")]
        [DataRow("Test$With<=Symbols>", "Test$With…", 18, DisplayName = "input = \"Test$With<=Symbols>\", maxLength = 18")]
        [DataRow("Test$With<=Symbols>", "Test$With…", 17, DisplayName = "input = \"Test$With<=Symbols>\", maxLength = 17")]
        [DataRow("Test$With<=Symbols>", "Test$With…", 12, DisplayName = "input = \"Test$With<=Symbols>\", maxLength = 12")]
        [DataRow("Test$With<=Symbols>", "Test$With…", 11, DisplayName = "input = \"Test$With<=Symbols>\", maxLength = 11")]
        [DataRow("Test$With<=Symbols>", "Test$With…", 10, DisplayName = "input = \"Test$With<=Symbols>\", maxLength = 10")]
        [DataRow("Test$With<=Symbols>", "Test…", 9, DisplayName = "input = \"Test$With<=Symbols>\", maxLength = 9")]
        [DataRow(" Mi><ed Test$With<=Space.First and last", " Mi><ed Test$With<=Space.First and last", 40, DisplayName = "input = \" Mi><ed Test$With<=Space.First and last\", maxLength = 40")]
        [DataRow(" Mi><ed Test$With<=Space.First and last", " Mi><ed Test$With<=Space.First and last", 39, DisplayName = "input = \" Mi><ed Test$With<=Space.First and last\", maxLength = 39")]
        [DataRow(" Mi><ed Test$With<=Space.First and last", " Mi><ed Test$With<=Space.First…", 31, DisplayName = "input = \" Mi><ed Test$With<=Space.First and last\", maxLength = 31")]
        [DataRow(" Mi><ed Test$With<=Space.First and last", " Mi><ed…", 30, DisplayName = "input = \" Mi><ed Test$With<=Space.First and last\", maxLength = 30")]
        [DataRow(" Mi><ed Test$With<=Space.First and last", " Mi…", 7, DisplayName = "input = \" Mi><ed Test$With<=Space.First and last\", maxLength = 7")]
        [DataRow(" Mi><ed Test$With<=Space.First and last", " Mi…", 6, DisplayName = "input = \" Mi><ed Test$With<=Space.First and last\", maxLength = 6")]
        [DataRow(" Mi><ed.Test$With<=Punction First and last!", " Mi><ed.Test$With<=Punction First and last!", 44, DisplayName = "input = \" Mi><ed.Test$With<=Punction First and last!\", maxLength = 44")]
        [DataRow(" Mi><ed.Test$With<=Punction First and last!", " Mi><ed.Test$With<=Punction First and last!", 43, DisplayName = "input = \" Mi><ed.Test$With<=Punction First and last!\", maxLength = 43")]
        [DataRow(" Mi><ed.Test$With<=Punction First and last!", " Mi><ed.Test$With<=Punction…", 28, DisplayName = "input = \" Mi><ed.Test$With<=Punction First and last!\", maxLength = 28")]
        [DataRow(" Mi><ed.Test$With<=Punction First and last!", " Mi><ed.…", 27, DisplayName = "input = \" Mi><ed.Test$With<=Punction First and last!\", maxLength = 27")]
        [DataRow(" Mi><ed.Test$With<=Punction First and last!", " Mi><ed.…", 9, DisplayName = "input = \" Mi><ed.Test$With<=Punction First and last!\", maxLength = 9")]
        [DataRow(" Mi><ed.Test$With<=Punction First and last!", " Mi…", 8, DisplayName = "input = \" Mi><ed.Test$With<=Punction First and last!\", maxLength = 8")]
        [DataRow(" Mi><ed.Test$With<=Punction First and last!", " Mi…", 7, DisplayName = "input = \" Mi><ed.Test$With<=Punction First and last!\", maxLength = 7")]
        public void TruncateWithElipsesTestMethod(string input, string expected, int maxLength)
        {
            string actual = input.TruncateWithElipses(maxLength);
            Assert.AreEqual(expected, actual);
        }
    }
}
