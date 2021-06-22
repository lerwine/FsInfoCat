using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.ObjectModel;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class CollectionExtensionsTests
    {
        private static readonly ReadOnlyCollection<int> PrimeNumbers = new(new int[] { 1, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 37, 41, 43, 47, 53, 59, 61, 67, 71, 73, 79,
            83, 89, 97, 101, 103, 107, 109, 113, 127 });

        [TestMethod]
        [Ignore]
        public void IsPrimeNumberTestMethod()
        {
            int start = 0;
            foreach (int p in PrimeNumbers)
            {
                bool actual;
                for (int i = start; i < p; i++)
                {
                    actual = Collections.CollectionExtensions.IsPrimeNumber(i);
                    Assert.IsFalse(actual);
                }
                actual = Collections.CollectionExtensions.IsPrimeNumber(p);
                Assert.IsTrue(actual);
                start = p + 1;
            }
        }

        [TestMethod]
        public void FindPrimeNumberTestMethod()
        {
            int start = 0;
            foreach (int p in PrimeNumbers)
            {
                int actual;
                for (int i = start; i <= p; i++)
                {
                    actual = Collections.CollectionExtensions.FindPrimeNumber(i);
                    Assert.AreEqual(actual, p);
                }
                start = p + 1;
            }
        }
    }
}
