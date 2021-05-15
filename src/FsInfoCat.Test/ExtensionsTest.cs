using NUnit.Framework;
using System;

namespace FsInfoCat.Test
{
    public class ExtensionsTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void IsNullableTypeTest()
        {
            Type type = typeof(int?);
            bool actual = type.IsNullableType();
            Assert.That(actual, Is.True);

            type = typeof(int);
            actual = type.IsNullableType();
            Assert.That(actual, Is.False);

            type = typeof(Uri);
            actual = type.IsNullableType();
            Assert.That(actual, Is.False);

            type = null;
            Assert.Throws<ArgumentNullException>(() => type.IsNullableType());
        }

        [Test]
        public void IsSameTypeDefinitionTest()
        {
            Type x = typeof(IEquatable<int>);
            Type y = typeof(IEquatable<int>);
            bool actual = x.IsSameTypeDefinition(y);
            Assert.That(actual, Is.True);

            y = typeof(IEquatable<string>);
            actual = x.IsSameTypeDefinition(y);
            Assert.That(actual, Is.True);

            x = typeof(IEquatable<>);
            actual = x.IsSameTypeDefinition(y);
            Assert.That(actual, Is.True);

            y = typeof(IEquatable<>);
            actual = x.IsSameTypeDefinition(y);
            Assert.That(actual, Is.True);

            y = typeof(IComparable<int>);
            actual = x.IsSameTypeDefinition(y);
            Assert.That(actual, Is.False);

            y = typeof(IComparable<>);
            actual = x.IsSameTypeDefinition(y);
            Assert.That(actual, Is.False);

            x = typeof(IEquatable<string>);
            actual = x.IsSameTypeDefinition(y);
            Assert.That(actual, Is.False);

            y = typeof(IComparable<string>);
            actual = x.IsSameTypeDefinition(y);
            Assert.That(actual, Is.False);

            Assert.Throws<ArgumentNullException>(() => x.IsSameTypeDefinition(null));
            x = null;
            Assert.Throws<ArgumentNullException>(() => x.IsSameTypeDefinition(y));
        }

        [Test]
        public void IsSelfEquatableTest()
        {
            Type type = typeof(ComponentSupport.SelfComparableExample);
            bool actual = type.IsSelfEquatable();
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.SelfComparableExample);
            actual = type.IsSelfEquatable(false);
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.SelfComparableExample);
            actual = type.IsSelfEquatable(true);
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.NotComparableExample<>).GetGenericArguments()[0];
            actual = type.IsSelfEquatable();
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.NotComparableExample<>).GetGenericArguments()[0];
            actual = type.IsSelfEquatable(false);
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.NotComparableExample<>).GetGenericArguments()[0];
            actual = type.IsSelfEquatable(true);
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.BaseComparableExample);
            actual = type.IsSelfEquatable();
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.BaseComparableExample);
            actual = type.IsSelfEquatable(false);
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.BaseComparableExample);
            bool expected = typeof(IEquatable<ComponentSupport.BaseComparableExample>).IsAssignableFrom(typeof(ComponentSupport.BaseComparableExample));
            actual = type.IsSelfEquatable(true);
            Assert.That(actual, Is.EqualTo(expected));

            type = typeof(ComponentSupport.OtherComparableExample);
            actual = type.IsSelfEquatable();
            Assert.That(actual, Is.False);

            type = typeof(ComponentSupport.SelfComparableExample<ComponentSupport.SelfComparableExample>);
            actual = type.IsSelfEquatable();
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.SelfComparableExample<ComponentSupport.SelfComparableExample>);
            actual = type.IsSelfEquatable(false);
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.SelfComparableExample<ComponentSupport.SelfComparableExample>);
            expected = typeof(IEquatable<ComponentSupport.SelfComparableExample<ComponentSupport.SelfComparableExample>>).IsAssignableFrom(typeof(ComponentSupport.SelfComparableExample<ComponentSupport.SelfComparableExample>));
            actual = type.IsSelfEquatable(true);
            Assert.That(actual, Is.EqualTo(expected));

            type = typeof(ComponentSupport.InheritedComparableExample);
            actual = type.IsSelfEquatable();
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.InheritedComparableExample);
            actual = type.IsSelfEquatable(false);
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.InheritedComparableExample);
            expected = typeof(IEquatable<ComponentSupport.InheritedComparableExample>).IsAssignableFrom(typeof(ComponentSupport.InheritedComparableExample));
            actual = type.IsSelfEquatable(true);
            Assert.That(actual, Is.EqualTo(expected));

            type = typeof(ComponentSupport.NotComparableExample);
            actual = type.IsSelfEquatable();
            Assert.That(actual, Is.False);

            type = null;
            Assert.Throws<ArgumentNullException>(() => type.IsSelfEquatable());
        }

        [Test]
        public void IsSelfComparableTest()
        {
            Type type = typeof(ComponentSupport.SelfComparableExample);
            bool actual = type.IsSelfComparable();
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.SelfComparableExample);
            actual = type.IsSelfComparable(false);
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.SelfComparableExample);
            actual = type.IsSelfComparable(true);
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.NotComparableExample<>).GetGenericArguments()[0];
            actual = type.IsSelfComparable();
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.NotComparableExample<>).GetGenericArguments()[0];
            actual = type.IsSelfComparable(false);
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.NotComparableExample<>).GetGenericArguments()[0];
            actual = type.IsSelfComparable(true);
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.BaseComparableExample);
            actual = type.IsSelfComparable();
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.BaseComparableExample);
            actual = type.IsSelfComparable(false);
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.BaseComparableExample);
            bool expected = typeof(IComparable<ComponentSupport.BaseComparableExample>).IsAssignableFrom(typeof(ComponentSupport.BaseComparableExample));
            actual = type.IsSelfComparable(true);
            Assert.That(actual, Is.EqualTo(expected));

            type = typeof(ComponentSupport.OtherComparableExample);
            actual = type.IsSelfComparable();
            Assert.That(actual, Is.False);

            type = typeof(ComponentSupport.SelfComparableExample<ComponentSupport.SelfComparableExample>);
            actual = type.IsSelfComparable();
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.SelfComparableExample<ComponentSupport.SelfComparableExample>);
            actual = type.IsSelfComparable(false);
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.SelfComparableExample<ComponentSupport.SelfComparableExample>);
            expected = typeof(IComparable<ComponentSupport.SelfComparableExample<ComponentSupport.SelfComparableExample>>).IsAssignableFrom(typeof(ComponentSupport.SelfComparableExample<ComponentSupport.SelfComparableExample>));
            actual = type.IsSelfComparable(true);
            Assert.That(actual, Is.EqualTo(expected));

            type = typeof(ComponentSupport.InheritedComparableExample);
            actual = type.IsSelfComparable();
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.InheritedComparableExample);
            actual = type.IsSelfComparable(false);
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.InheritedComparableExample);
            expected = typeof(IComparable<ComponentSupport.InheritedComparableExample>).IsAssignableFrom(typeof(ComponentSupport.InheritedComparableExample));
            actual = type.IsSelfComparable(true);
            Assert.That(actual, Is.EqualTo(expected));

            type = typeof(ComponentSupport.NotComparableExample);
            actual = type.IsSelfComparable();
            Assert.That(actual, Is.False);

            type = null;
            Assert.Throws<ArgumentNullException>(() => type.IsSelfComparable());
        }

        [Test]
        public void HasTypeConverterTest()
        {
            Type type = typeof(Uri);

            bool actual = type.HasTypeConverter();
            Assert.That(actual, Is.True);

            type = typeof(ComponentSupport.NotComparableExample);
            actual = type.HasTypeConverter();
            Assert.That(actual, Is.False);

            type = null;
            Assert.Throws<ArgumentNullException>(() => type.HasTypeConverter());
        }
    }
}
