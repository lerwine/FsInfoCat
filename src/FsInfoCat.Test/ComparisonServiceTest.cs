using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace FsInfoCat.Test
{
    public class ComparisonServiceTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ServiceTest()
        {
            IComparisonService x = Services.GetComparisonService();
            Assert.That(x, Is.Not.Null);
            IComparisonService y = Services.GetComparisonService();
            Assert.That(y, Is.Not.Null);
            Assert.That(x, Is.SameAs(y));
        }

        [Test]
        public void GetEqualityComparerTest()
        {
            IComparisonService comparisonService = Services.GetComparisonService();
            Assert.That(comparisonService, Is.Not.Null);

            IEqualityComparer equalityComparerA = comparisonService.GetEqualityComparer(typeof(int));
            Assert.That(equalityComparerA, Is.Not.Null);
            Assert.That(equalityComparerA, Is.InstanceOf<IEqualityComparer<int>>());
            IEqualityComparer<int> intComparer = comparisonService.GetEqualityComparer<int>();
            Assert.That(intComparer, Is.Not.Null);
            Assert.That(intComparer, Is.SameAs(equalityComparerA));

            equalityComparerA = comparisonService.GetEqualityComparer(typeof(decimal?));
            Assert.That(equalityComparerA, Is.Not.Null);
            Assert.That(equalityComparerA, Is.InstanceOf<IEqualityComparer<decimal?>>());
            IEqualityComparer<decimal?> decimalComparer = comparisonService.GetEqualityComparer<decimal?>();
            Assert.That(decimalComparer, Is.Not.Null);
            Assert.That(decimalComparer, Is.SameAs(equalityComparerA));

            equalityComparerA = comparisonService.GetEqualityComparer(typeof(Uri));
            Assert.That(equalityComparerA, Is.Not.Null);
            Assert.That(equalityComparerA, Is.InstanceOf<IEqualityComparer<Uri>>());
            IEqualityComparer<Uri> uriComparer = comparisonService.GetEqualityComparer<Uri>();
            Assert.That(uriComparer, Is.Not.Null);
            Assert.That(uriComparer, Is.SameAs(equalityComparerA));

            equalityComparerA = comparisonService.GetEqualityComparer(typeof(SqlConnectionStringBuilder));
            Assert.That(equalityComparerA, Is.Not.Null);
            Assert.That(equalityComparerA, Is.InstanceOf<IEqualityComparer<SqlConnectionStringBuilder>>());
            IEqualityComparer<SqlConnectionStringBuilder> connectionStringComparer = comparisonService.GetEqualityComparer<SqlConnectionStringBuilder>();
            Assert.That(connectionStringComparer, Is.Not.Null);
            Assert.That(connectionStringComparer, Is.SameAs(equalityComparerA));

            equalityComparerA = comparisonService.GetEqualityComparer(typeof(string));
            Assert.That(equalityComparerA, Is.Not.Null);
            Assert.That(equalityComparerA, Is.InstanceOf<IEqualityComparer<string>>());
            IEqualityComparer<string> stringComparer = comparisonService.GetEqualityComparer<string>();
            Assert.That(stringComparer, Is.Not.Null);
            Assert.That(stringComparer, Is.SameAs(equalityComparerA));
        }

        [Test]
        public void GetEqualityComparer_ValueTest()
        {
            IComparisonService comparisonService = Services.GetComparisonService();
            Assert.That(comparisonService, Is.Not.Null);
            IEqualityComparer<int> equalityComparer = comparisonService.GetEqualityComparer<int>();
            Assert.That(equalityComparer, Is.Not.Null);

            bool actual = equalityComparer.Equals(1, 1);
            Assert.That(actual, Is.True);

            actual = equalityComparer.Equals(1, 0);
            Assert.That(actual, Is.False);

            actual = equalityComparer.Equals(int.MaxValue, int.MinValue);
            Assert.That(actual, Is.False);
        }

        [Test]
        public void GetEqualityComparer_NullableTest()
        {
            IComparisonService comparisonService = Services.GetComparisonService();
            Assert.That(comparisonService, Is.Not.Null);
            IEqualityComparer<decimal?> equalityComparer = comparisonService.GetEqualityComparer<decimal?>();
            Assert.That(equalityComparer, Is.Not.Null);

            bool actual = equalityComparer.Equals(1m, 1m);
            Assert.That(actual, Is.True);

            actual = equalityComparer.Equals(1m, 0m);
            Assert.That(actual, Is.False);

            actual = equalityComparer.Equals(decimal.MaxValue, decimal.MinValue);
            Assert.That(actual, Is.False);

            actual = equalityComparer.Equals(null, null);
            Assert.That(actual, Is.True);

            actual = equalityComparer.Equals(null, 0m);
            Assert.That(actual, Is.False);

            actual = equalityComparer.Equals(0m, null);
            Assert.That(actual, Is.False);
        }

        [Test]
        public void GetEqualityComparer_AnnotatedPropertyTest()
        {
            IComparisonService comparisonService = Services.GetComparisonService();
            Assert.That(comparisonService, Is.Not.Null);
            IEqualityComparer<SqlConnectionStringBuilder> equalityComparer = comparisonService.GetEqualityComparer<SqlConnectionStringBuilder>();
            Assert.That(equalityComparer, Is.Not.Null);

            string connectionString = @"data source=(localdb)\ProjectsV13;initial catalog=FsInfoCatLocal;integrated security=True";
            SqlConnectionStringBuilder x = new SqlConnectionStringBuilder(connectionString);

            bool actual = equalityComparer.Equals(x, x);
            Assert.That(actual, Is.True);

            SqlConnectionStringBuilder y = new SqlConnectionStringBuilder(connectionString);
            actual = equalityComparer.Equals(x, y);
            Assert.That(actual, Is.True);

            connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=FsInfoCatLocal;Integrated Security=True";
            y = new SqlConnectionStringBuilder(connectionString);
            actual = equalityComparer.Equals(x, y);
            Assert.That(actual, Is.True);

            connectionString = @"Data Source=(localdb)\ProjectsV13;Initial Catalog=FsInfoCatLocal";
            y = new SqlConnectionStringBuilder(connectionString);
            actual = equalityComparer.Equals(x, y);
            Assert.That(actual, Is.False);

            actual = equalityComparer.Equals(null, null);
            Assert.That(actual, Is.True);

            actual = equalityComparer.Equals(x, null);
            Assert.That(actual, Is.False);

            actual = equalityComparer.Equals(null, y);
            Assert.That(actual, Is.False);
        }

        [Test]
        public void GetEqualityComparer_ConvertingTest()
        {
            IComparisonService comparisonService = Services.GetComparisonService();
            Assert.That(comparisonService, Is.Not.Null);
            IEqualityComparer<Uri> equalityComparer = comparisonService.GetEqualityComparer<Uri>();
            Assert.That(equalityComparer, Is.Not.Null);

            string uriString = "http://www.erwinefamily.net";
            Uri x = new Uri(uriString, UriKind.Absolute);
            bool actual = equalityComparer.Equals(x, x);
            Assert.That(actual, Is.True);

            Uri y = new Uri(uriString, UriKind.Absolute);
            actual = equalityComparer.Equals(x, y);
            Assert.That(actual, Is.True);

            uriString = "http://www.erwinefamily.net/mypage.html";
            y = new Uri(uriString, UriKind.Absolute);
            actual = equalityComparer.Equals(x, y);
            Assert.That(actual, Is.False);

            actual = equalityComparer.Equals(null, null);
            Assert.That(actual, Is.True);

            actual = equalityComparer.Equals(x, null);
            Assert.That(actual, Is.False);

            actual = equalityComparer.Equals(null, y);
            Assert.That(actual, Is.False);
        }

        [Test]
        public void GetEqualityComparer_StringTest()
        {
            IComparisonService comparisonService = Services.GetComparisonService();
            Assert.That(comparisonService, Is.Not.Null);
            IEqualityComparer<string> equalityComparer = comparisonService.GetEqualityComparer<string>();
            Assert.That(equalityComparer, Is.Not.Null);

            bool actual = equalityComparer.Equals("", "");
            Assert.That(actual, Is.True);

            actual = equalityComparer.Equals("Test", "Test");
            Assert.That(actual, Is.True);

            actual = equalityComparer.Equals("TEST", "Test");
            Assert.That(actual, Is.False);

            actual = equalityComparer.Equals(" ", "");
            Assert.That(actual, Is.False);

            actual = equalityComparer.Equals(null, null);
            Assert.That(actual, Is.True);

            actual = equalityComparer.Equals("", null);
            Assert.That(actual, Is.False);

            actual = equalityComparer.Equals(null, "");
            Assert.That(actual, Is.False);
        }

        [Test]
        public void GetComparerTest()
        {
            IComparisonService comparisonService = Services.GetComparisonService();
            Assert.That(comparisonService, Is.Not.Null);

            IComparer comparerA = comparisonService.GetComparer(typeof(int));
            Assert.That(comparerA, Is.Not.Null);
            Assert.That(comparerA, Is.InstanceOf<IComparer<int>>());
            IComparer<int> intComparer = comparisonService.GetComparer<int>();
            Assert.That(intComparer, Is.Not.Null);
            Assert.That(intComparer, Is.SameAs(comparerA));

            comparerA = comparisonService.GetComparer(typeof(decimal?));
            Assert.That(comparerA, Is.Not.Null);
            Assert.That(comparerA, Is.InstanceOf<IComparer<decimal?>>());
            IComparer<decimal?> decimalComparer = comparisonService.GetComparer<decimal?>();
            Assert.That(decimalComparer, Is.Not.Null);
            Assert.That(decimalComparer, Is.SameAs(comparerA));

            comparerA = comparisonService.GetComparer(typeof(Uri));
            Assert.That(comparerA, Is.Not.Null);
            Assert.That(comparerA, Is.InstanceOf<IComparer<Uri>>());
            IComparer<Uri> uriComparer = comparisonService.GetComparer<Uri>();
            Assert.That(uriComparer, Is.Not.Null);
            Assert.That(uriComparer, Is.SameAs(comparerA));

            comparerA = comparisonService.GetComparer(typeof(SqlConnectionStringBuilder));
            Assert.That(comparerA, Is.Not.Null);
            Assert.That(comparerA, Is.InstanceOf<IComparer<SqlConnectionStringBuilder>>());
            IComparer<SqlConnectionStringBuilder> connectionStringComparer = comparisonService.GetComparer<SqlConnectionStringBuilder>();
            Assert.That(connectionStringComparer, Is.Not.Null);
            Assert.That(connectionStringComparer, Is.SameAs(comparerA));

            comparerA = comparisonService.GetComparer(typeof(string));
            Assert.That(comparerA, Is.Not.Null);
            Assert.That(comparerA, Is.InstanceOf<IComparer<string>>());
            IComparer<string> stringComparer = comparisonService.GetComparer<string>();
            Assert.That(stringComparer, Is.Not.Null);
            Assert.That(stringComparer, Is.SameAs(comparerA));
        }

        [Test]
        public void GetComparer_ValueTest()
        {
            IComparisonService comparisonService = Services.GetComparisonService();
            Assert.That(comparisonService, Is.Not.Null);
            IComparer<int> comparer = comparisonService.GetComparer<int>();
            Assert.That(comparer, Is.Not.Null);

            int actual = comparer.Compare(1, 1);
            Assert.That(actual, Is.EqualTo(0));

            actual = comparer.Compare(1, 0);
            Assert.That(actual, Is.GreaterThan(0));

            actual = comparer.Compare(0, 1);
            Assert.That(actual, Is.LessThan(0));

            actual = comparer.Compare(int.MaxValue, int.MinValue);
            Assert.That(actual, Is.GreaterThan(0));

            actual = comparer.Compare(int.MinValue, int.MaxValue);
            Assert.That(actual, Is.LessThan(0));
        }

        [Test]
        public void GetComparer_NullableTest()
        {
            IComparisonService comparisonService = Services.GetComparisonService();
            Assert.That(comparisonService, Is.Not.Null);
            IComparer<decimal?> comparer = comparisonService.GetComparer<decimal?>();
            Assert.That(comparer, Is.Not.Null);

            int actual = comparer.Compare(1m, 1m);
            Assert.That(actual, Is.EqualTo(0));

            actual = comparer.Compare(1m, 0m);
            Assert.That(actual, Is.GreaterThan(0));

            actual = comparer.Compare(0m, 1m);
            Assert.That(actual, Is.LessThan(0));

            actual = comparer.Compare(decimal.MaxValue, decimal.MinValue);
            Assert.That(actual, Is.GreaterThan(0));

            actual = comparer.Compare(decimal.MinValue, decimal.MaxValue);
            Assert.That(actual, Is.LessThan(0));

            actual = comparer.Compare(null, null);
            Assert.That(actual, Is.EqualTo(0));

            actual = comparer.Compare(null, 0m);
            Assert.That(actual, Is.LessThan(0));

            actual = comparer.Compare(0m, null);
            Assert.That(actual, Is.GreaterThan(0));
        }

        [Test]
        public void GetComparer_ConvertingTest()
        {
            IComparisonService comparisonService = Services.GetComparisonService();
            Assert.That(comparisonService, Is.Not.Null);
            IComparer<Uri> comparer = comparisonService.GetComparer<Uri>();
            Assert.That(comparer, Is.Not.Null);

            Uri x = new Uri("", UriKind.Relative);
            int actual = comparer.Compare(x, x);
            Assert.That(actual, Is.EqualTo(0));

            Uri y = new Uri("", UriKind.Relative);
            actual = comparer.Compare(x, y);
            Assert.That(actual, Is.EqualTo(0));

            x = new Uri("https://www.tempuri.org", UriKind.Absolute);
            actual = comparer.Compare(x, y);
            Assert.That(actual, Is.GreaterThan(0));

            y = new Uri("https://www.TEMPURI.org", UriKind.Absolute);
            actual = comparer.Compare(x, y);
            Assert.That(actual, Is.LessThan(0));

            actual = comparer.Compare(null, null);
            Assert.That(actual, Is.EqualTo(0));

            x = new Uri("", UriKind.Relative);
            actual = comparer.Compare(x, null);
            Assert.That(actual, Is.EqualTo(0));

            actual = comparer.Compare(null, x);
            Assert.That(actual, Is.EqualTo(0));
        }

        [Test]
        public void GetComparer_StringTest()
        {
            IComparisonService comparisonService = Services.GetComparisonService();
            Assert.That(comparisonService, Is.Not.Null);
            IComparer<string> comparer = comparisonService.GetComparer<string>();
            Assert.That(comparer, Is.Not.Null);

            int actual = comparer.Compare("", "");
            Assert.That(actual, Is.EqualTo(0));

            actual = comparer.Compare("Test", "Test");
            Assert.That(actual, Is.EqualTo(0));

            actual = comparer.Compare("TEST", "Test");
            Assert.That(actual, Is.GreaterThan(0));

            actual = comparer.Compare("Test", "TEST");
            Assert.That(actual, Is.LessThan(0));

            actual = comparer.Compare(" ", "");
            Assert.That(actual, Is.GreaterThan(0));

            actual = comparer.Compare("", " ");
            Assert.That(actual, Is.LessThan(0));

            actual = comparer.Compare(null, null);
            Assert.That(actual, Is.EqualTo(0));

            actual = comparer.Compare("", null);
            Assert.That(actual, Is.GreaterThan(0));

            actual = comparer.Compare(null, "");
            Assert.That(actual, Is.LessThan(0));
        }

        [Test]
        public void GetDefaultEqualityComparerTest()
        {
            IComparisonService comparisonService = Services.GetComparisonService();
            Assert.That(comparisonService, Is.Not.Null);

        }
    }
}
