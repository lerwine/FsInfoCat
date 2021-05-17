using FsInfoCat.ComponentSupport;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace FsInfoCat.Test
{
    public class ValidationResultCollectionTest
    {
        [Test]
        public void AddTest()
        {
            // 0*: { ErrorMessage = "Error 1", MemberNames = [] }
            ValidationResultCollection target = new ValidationResultCollection();
            string errorMessage0 = "Error 1";
            string[] memberNames0 = Array.Empty<string>();
            ValidationResult newItem = new ValidationResult(errorMessage0);
            target.Add(newItem);
            Assert.That(target.Count, Is.EqualTo(1));
            ValidationResult item0 = target[0];
            Assert.That(item0.ErrorMessage, Is.EqualTo(errorMessage0));
            Assert.That(item0.MemberNames.SequenceEqual(memberNames0), Is.True);
            Assert.That(item0, Is.Not.SameAs(newItem));

            // 0:  { ErrorMessage = "Error 1", MemberNames = [] }
            // 1*: { ErrorMessage = "Error 2", MemberNames = [] }
            string errorMessage1 = "Error 2";
            string[] memberNames1 = Array.Empty<string>();
            newItem = new ValidationResult(errorMessage1);
            target.Add(newItem);
            Assert.That(target.Count, Is.EqualTo(2));
            ValidationResult item1 = target[1];
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(item1.ErrorMessage, Is.EqualTo(errorMessage1));
            Assert.That(item1.MemberNames.SequenceEqual(memberNames1), Is.True);
            Assert.That(item1, Is.Not.SameAs(newItem));

            // 0<=1: { ErrorMessage = "Error 2", MemberNames = [] }: Delete: { ErrorMessage = "Error 1", MemberNames = [] }
            // 1*:   { ErrorMessage = "Error 1", MemberNames = [] }
            newItem = new ValidationResult(errorMessage0);
            target.Add(newItem);
            Assert.That(target.Count, Is.EqualTo(2));
            Assert.That(target[0], Is.SameAs(item1));
            Assert.That(target[1].ErrorMessage, Is.EqualTo(errorMessage0));
            Assert.That(target[1].MemberNames.SequenceEqual(memberNames0), Is.True);
            Assert.That(target[1], Is.Not.SameAs(newItem));
            Assert.That(target[1], Is.Not.SameAs(item0));
            item0 = target[1];

            // 0<=1: { ErrorMessage = "Error 1", MemberNames = [] }; Delete: { ErrorMessage = "Error 1", MemberNames = [] }
            // 1*:   { ErrorMessage = "Error 2", MemberNames = [] }
            newItem = new ValidationResult(errorMessage1, new string[] { "", null, "\n" });
            target.Add(newItem);
            Assert.That(target.Count, Is.EqualTo(2));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(target[1].ErrorMessage, Is.EqualTo(errorMessage1));
            Assert.That(target[1].MemberNames.SequenceEqual(memberNames1), Is.True);
            Assert.That(target[1], Is.Not.SameAs(newItem));
            Assert.That(target[1], Is.Not.SameAs(item1));
            item1 = target[1];

            Assert.Throws<ArgumentNullException>(() => target.Add(null));
            Assert.That(target.Count, Is.EqualTo(2));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(target[1], Is.SameAs(item1));

            Assert.Throws<ArgumentOutOfRangeException>(() => target.Add(new ValidationResult("")));
            Assert.That(target.Count, Is.EqualTo(2));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(target[1], Is.SameAs(item1));

            Assert.Throws<ArgumentOutOfRangeException>(() => target.Add(new ValidationResult(" ")));
            Assert.That(target.Count, Is.EqualTo(2));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(target[1], Is.SameAs(item1));

            string errorMessage2 = errorMessage1;
            string[] memberNames2 = new string[] { "Property1", "Property2" };
            newItem = new ValidationResult(errorMessage2, memberNames2);
            target.Add(newItem);
            Assert.That(target.Count, Is.EqualTo(3));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(target[1], Is.SameAs(item1));
            ValidationResult item2 = target[2];
            Assert.That(item2.ErrorMessage, Is.EqualTo(errorMessage2));
            Assert.That(item2.MemberNames.SequenceEqual(memberNames2), Is.True);
            Assert.That(item2, Is.Not.SameAs(newItem));
            Assert.That(item2.MemberNames, Is.Not.SameAs(memberNames2));

            // 0:  { ErrorMessage = "Error 1", MemberNames = [] }
            // 1:  { ErrorMessage = "Error 2", MemberNames = [] }
            // 2:  { ErrorMessage = "Error 2", MemberNames = ["Property1", "Property2"] }
            // 3*: { ErrorMessage = "Error 3", MemberNames = ["Property1", "Property2"] }
            string errorMessage3 = "Error 3";
            string[] memberMames3 = memberNames2;
            newItem = new ValidationResult(errorMessage3, memberMames3);
            target.Add(newItem);
            Assert.That(target.Count, Is.EqualTo(4));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(target[1], Is.SameAs(item1));
            Assert.That(target[2], Is.SameAs(item2));
            ValidationResult item3 = target[3];
            Assert.That(item3.ErrorMessage, Is.EqualTo(errorMessage3));
            Assert.That(item3.MemberNames.SequenceEqual(memberMames3), Is.True);
            Assert.That(item3, Is.Not.SameAs(newItem));
            Assert.That(item3.MemberNames, Is.Not.SameAs(memberMames3));

            // 0:    { ErrorMessage = "Error 1", MemberNames = [] }
            // 1:    { ErrorMessage = "Error 2", MemberNames = [] }
            // 2<=3: { ErrorMessage = "Error 3", MemberNames = ["Property1", "Property2"] }; Delete: { ErrorMessage = "Error 2", MemberNames = ["Property1", "Property2"] }
            // 3*:   { ErrorMessage = "Error 2", MemberNames = ["Property1", "Property2"] }
            newItem = new ValidationResult(errorMessage2, memberNames2);
            target.Add(newItem);
            Assert.That(target.Count, Is.EqualTo(4));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(target[1], Is.SameAs(item1));
            Assert.That(target[2], Is.SameAs(item3));
            Assert.That(target[3].ErrorMessage, Is.EqualTo(errorMessage2));
            Assert.That(target[3].MemberNames.SequenceEqual(memberNames2), Is.True);
            Assert.That(target[3], Is.Not.SameAs(newItem));
            Assert.That(target[3], Is.Not.SameAs(item2));
            Assert.That(target[3].MemberNames, Is.Not.SameAs(memberNames2));
        }

        [Test]
        public void InsertTest()
        {
            // 1*: { ErrorMessage = "Error 2", MemberNames = [] }
            ValidationResultCollection target = new ValidationResultCollection();
            string errorMessage1 = "Error 2";
            string[] memberNames1 = Array.Empty<string>();
            ValidationResult newItemA = new ValidationResult(errorMessage1);
            target.Add(newItemA);
            Assert.That(target.Count, Is.EqualTo(1));

            // 0*:   { ErrorMessage = "Error 1", MemberNames = [] }
            // 1<=0: { ErrorMessage = "Error 2", MemberNames = [] }
            string errorMessage0 = "Error 1";
            string[] memberNames0 = Array.Empty<string>();
            ValidationResult newItemB = new ValidationResult(errorMessage0);
            target.Insert(0, newItemB);
            Assert.That(target.Count, Is.EqualTo(2));
            ValidationResult item0 = target[0];
            ValidationResult item1 = target[1];
            Assert.That(item0.ErrorMessage, Is.EqualTo(errorMessage0));
            Assert.That(item0.MemberNames.SequenceEqual(memberNames0), Is.True);
            Assert.That(item0, Is.Not.SameAs(newItemB));
            Assert.That(item1.ErrorMessage, Is.EqualTo(errorMessage1));
            Assert.That(item1.MemberNames.SequenceEqual(memberNames1), Is.True);
            Assert.That(item1, Is.Not.SameAs(newItemA));

            // 0:    { ErrorMessage = "Error 1", MemberNames = [] }
            // 1*:   { ErrorMessage = "Error 2", MemberNames = ["Test"] }
            // 2<=1: { ErrorMessage = "Error 2", MemberNames = [] }
            memberNames1 = new string[] { "Test" };
            newItemA = new ValidationResult(errorMessage1, memberNames1);
            target.Insert(1, newItemA);
            Assert.That(target.Count, Is.EqualTo(3));
            ValidationResult item2 = item1;
            item1 = target[1];
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(target[2], Is.SameAs(item2));
            Assert.That(item1.ErrorMessage, Is.EqualTo(errorMessage1));
            Assert.That(item1.MemberNames.SequenceEqual(memberNames1), Is.True);
            Assert.That(item1, Is.Not.SameAs(newItemA));

            // 0*: { ErrorMessage = "Error 1", MemberNames = [] }; Delete: { ErrorMessage = "Error 1", MemberNames = [] }
            // 1:  { ErrorMessage = "Error 2", MemberNames = ["Test"] }
            // 2:  { ErrorMessage = "Error 2", MemberNames = [] }
            newItemB = new ValidationResult(errorMessage0);
            target.Insert(0, newItemB);
            Assert.That(target.Count, Is.EqualTo(3));
            Assert.That(target[1], Is.SameAs(item1));
            Assert.That(target[2], Is.SameAs(item2));
            Assert.That(target[0].ErrorMessage, Is.EqualTo(item0.ErrorMessage));
            Assert.That(target[0].MemberNames.SequenceEqual(item0.MemberNames), Is.True);
            Assert.That(target[0], Is.Not.SameAs(newItemB));
            Assert.That(target[0], Is.Not.SameAs(item0));
            item0 = target[0];

            Assert.Throws<ArgumentNullException>(() => target.Add(null));
            Assert.That(target.Count, Is.EqualTo(3));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(target[1], Is.SameAs(item1));
            Assert.That(target[2], Is.SameAs(item2));

            Assert.Throws<ArgumentOutOfRangeException>(() => target.Add(new ValidationResult("")));
            Assert.That(target.Count, Is.EqualTo(3));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(target[1], Is.SameAs(item1));
            Assert.That(target[2], Is.SameAs(item2));

            Assert.Throws<ArgumentOutOfRangeException>(() => target.Add(new ValidationResult(" ")));
            Assert.That(target.Count, Is.EqualTo(3));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(target[1], Is.SameAs(item1));
            Assert.That(target[2], Is.SameAs(item2));

            // 0<=1: { ErrorMessage = "Error 2", MemberNames = ["Test"] }; Delete: { ErrorMessage = "Error 1", MemberNames = [] }
            // 1*:   { ErrorMessage = "Error 1", MemberNames = [] }
            // 2:    { ErrorMessage = "Error 2", MemberNames = [] }
            newItemB = new ValidationResult(errorMessage0);
            target.Insert(2, newItemB);
            Assert.That(target.Count, Is.EqualTo(3));
            Assert.That(target[0], Is.SameAs(item1));
            Assert.That(target[2], Is.SameAs(item2));
            Assert.That(target[1].ErrorMessage, Is.EqualTo(item0.ErrorMessage));
            Assert.That(target[1].ErrorMessage, Is.EqualTo(errorMessage0));
            Assert.That(target[1].MemberNames.SequenceEqual(item0.MemberNames), Is.True);
            Assert.That(target[1].MemberNames.SequenceEqual(memberNames0), Is.True);
            Assert.That(target[1], Is.Not.SameAs(newItemB));
            Assert.That(target[1], Is.Not.SameAs(item0));
            item0 = item1;
            item1 = target[1];

            // 0*:   { ErrorMessage = "Error 2", MemberNames = [] }
            // 1<=0: { ErrorMessage = "Error 2", MemberNames = ["Test"] };
            // 2<=1: { ErrorMessage = "Error 1", MemberNames = [] }; Deleted: { ErrorMessage = "Error 2", MemberNames = [] }
            newItemB = new ValidationResult(errorMessage1, memberNames0);
            target.Insert(0, newItemB);
            Assert.That(target.Count, Is.EqualTo(3));
            Assert.That(target[1], Is.SameAs(item0));
            Assert.That(target[2], Is.SameAs(item1));
            Assert.That(target[0].ErrorMessage, Is.EqualTo(item2.ErrorMessage));
            Assert.That(target[0].ErrorMessage, Is.EqualTo(errorMessage1));
            Assert.That(target[0].MemberNames.SequenceEqual(item2.MemberNames), Is.True);
            Assert.That(target[0].MemberNames.SequenceEqual(memberNames0), Is.True);
            Assert.That(target[0], Is.Not.SameAs(newItemB));
            Assert.That(target[0], Is.Not.SameAs(item2));
        }

        [Test]
        public void SetTest()
        {
            ValidationResultCollection target = new ValidationResultCollection();

            // 0*: { ErrorMessage = "Error 1", MemberNames = [] }
            string errorMessage0 = "Error 1";
            string[] memberNames0 = Array.Empty<string>();
            ValidationResult newItemB = new ValidationResult(errorMessage0);
            target.Add(newItemB);

            // 0*: { ErrorMessage = "Error 1", MemberNames = [] }; Delete: { ErrorMessage = "Error 1", MemberNames = [] }
            ValidationResult item1 = target[0];
            ValidationResult newItemA = new ValidationResult(errorMessage0);
            ((IList<ValidationResult>)target)[0] = newItemA;
            Assert.That(target.Count, Is.EqualTo(1));
            ValidationResult item0 = target[0];
            Assert.That(item0.ErrorMessage, Is.EqualTo(errorMessage0));
            Assert.That(item0.ErrorMessage, Is.EqualTo(item1.ErrorMessage));
            Assert.That(item0.MemberNames.SequenceEqual(memberNames0), Is.True);
            Assert.That(item0.MemberNames.SequenceEqual(item1.MemberNames), Is.True);
            Assert.That(item0, Is.Not.SameAs(newItemA));
            Assert.That(item0, Is.Not.SameAs(newItemB));
            Assert.That(item0, Is.Not.SameAs(item1));

            // 0:  { ErrorMessage = "Error 1", MemberNames = [] }
            // 1*: { ErrorMessage = "Error 2", MemberNames = ["."] }
            string errorMessage1 = "Error 2";
            string[] memberNames1 = new string[] { "." };
            target.Add(new ValidationResult(errorMessage1));
            // 0:  { ErrorMessage = "Error 1", MemberNames = [] }
            // 1:  { ErrorMessage = "Error 2", MemberNames = ["."] }
            // 2*: { ErrorMessage = "Error 4", MemberNames = [] }
            string errorMessage3 = "Error 4";
            string[] memberNames3 = Array.Empty<string>();
            target.Add(new ValidationResult(errorMessage3, memberNames3));
            item1 = target[1];
            ValidationResult item2 = target[2];

            // 0:  { ErrorMessage = "Error 1", MemberNames = [] }
            // 1:  { ErrorMessage = "Error 2", MemberNames = ["."] }
            // 2*: { ErrorMessage = "Error 3", MemberNames = ["Test"] }; Replace: { ErrorMessage = "Error 4", MemberNames = [] }
            string errorMessage2 = "Error 3";
            string[] memberNames2 = new string[] { "Test" };
            newItemA = new ValidationResult(errorMessage2, memberNames2);
            ((IList<ValidationResult>)target)[2] = newItemA;
            Assert.That(target.Count, Is.EqualTo(3));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(target[1], Is.SameAs(item1));
            Assert.That(target[2].ErrorMessage, Is.EqualTo(errorMessage2));
            Assert.That(target[2].MemberNames.SequenceEqual(memberNames2), Is.True);
            Assert.That(target[2], Is.Not.SameAs(newItemA));
            Assert.That(target[2], Is.Not.SameAs(item2));
            item2 = target[2];

            // 0<=1:  { ErrorMessage = "Error 2", MemberNames = ["."] }; Delete: { ErrorMessage = "Error 1", MemberNames = [] }
            // 1*:  { ErrorMessage = "Error 1", MemberNames = [] }; Replace: { ErrorMessage = "Error 3", MemberNames = ["Test"] }
            newItemA = new ValidationResult(errorMessage0);
            ((IList<ValidationResult>)target)[2] = newItemA;
            Assert.That(target.Count, Is.EqualTo(2));
            Assert.That(target[0], Is.SameAs(item1));
            Assert.That(target[1].ErrorMessage, Is.EqualTo(errorMessage0));
            Assert.That(target[1].MemberNames.SequenceEqual(memberNames0), Is.True);
            Assert.That(target[1], Is.Not.SameAs(newItemA));
            Assert.That(target[1], Is.Not.SameAs(item0));
            item0 = item1;
            item1 = target[1];

            // 0:  { ErrorMessage = "Error 2", MemberNames = ["."] }
            // 1:  { ErrorMessage = "Error 1", MemberNames = [] }
            // 2*: { ErrorMessage = "Error 3", MemberNames = ["Test"] }
            target.Add(new ValidationResult(errorMessage2, memberNames2));
            item2 = target[2];
            // 0*: { ErrorMessage = "Error 3", MemberNames = ["Test"] }; Replace: { ErrorMessage = "Error 2", MemberNames = ["."] }
            // 1:  { ErrorMessage = "Error 1", MemberNames = [] }
            // 2:  Delete: { ErrorMessage = "Error 3", MemberNames = ["Test"] }
            newItemA = new ValidationResult(errorMessage2, memberNames2);
            ((IList<ValidationResult>)target)[0] = newItemA;
            Assert.That(target.Count, Is.EqualTo(2));
            Assert.That(target[1], Is.SameAs(item1));
            Assert.That(target[0].ErrorMessage, Is.EqualTo(errorMessage2));
            Assert.That(target[0].MemberNames.SequenceEqual(memberNames2), Is.True);
            Assert.That(target[0], Is.Not.SameAs(newItemA));
            Assert.That(target[0], Is.Not.SameAs(item2));
        }

        [Test]
        public void ContainsTest()
        {
            ValidationResultCollection target = new ValidationResultCollection();

            // 0:  { ErrorMessage = "Error 1", MemberNames = ["."] }
            // 1:  { ErrorMessage = "Error 2", MemberNames = [] }
            // 2*: { ErrorMessage = "Error 3", MemberNames = ["Test"] }
            target.Add(new ValidationResult("Error 1", new string[] { "." }));
            target.Add(new ValidationResult("Error 2"));
            target.Add(new ValidationResult("Error 3", new string[] { "Test" }));

            bool actual = target.Contains(new ValidationResult("Error 1", new string[] { "." }));
            Assert.That(actual, Is.True);
            actual = target.Contains(target[0]);
            Assert.That(actual, Is.True);
            actual = target.Contains(new ValidationResult("Error 2"));
            Assert.That(actual, Is.True);
            actual = target.Contains(target[1]);
            Assert.That(actual, Is.True);
            actual = target.Contains(new ValidationResult("Error 3", new string[] { "Test" }));
            Assert.That(actual, Is.True);
            actual = target.Contains(target[2]);
            Assert.That(actual, Is.True);

            actual = target.Contains(new ValidationResult("Error 1", new string[] { ".", "", " " }));
            Assert.That(actual, Is.True);
            actual = target.Contains(new ValidationResult("Error 2", new string[] { null, "" }));
            Assert.That(actual, Is.True);
            actual = target.Contains(new ValidationResult("Error 2", null));
            Assert.That(actual, Is.True);
            actual = target.Contains(new ValidationResult("Error 3", new string[] { null, "Test" }));
            Assert.That(actual, Is.True);

            actual = target.Contains(new ValidationResult("Error 1", new string[] { ".", "2" }));
            Assert.That(actual, Is.False);
            actual = target.Contains(new ValidationResult("Error 1"));
            Assert.That(actual, Is.False);
            actual = target.Contains(new ValidationResult("Error 2", new string[] { "." }));
            Assert.That(actual, Is.False);
            actual = target.Contains(new ValidationResult("Error 3", new string[] { "." }));
            Assert.That(actual, Is.False);
            actual = target.Contains(new ValidationResult("Error 3", null));
            Assert.That(actual, Is.False);
            actual = target.Contains(new ValidationResult(null));
            Assert.That(actual, Is.False);
            actual = target.Contains(null);
            Assert.That(actual, Is.False);
        }

        [Test]
        public void IndexOfTest()
        {
            ValidationResultCollection target = new ValidationResultCollection();

            // 0:  { ErrorMessage = "Error 1", MemberNames = ["."] }
            // 1:  { ErrorMessage = "Error 2", MemberNames = [] }
            // 2*: { ErrorMessage = "Error 3", MemberNames = ["Test"] }
            target.Add(new ValidationResult("Error 1", new string[] { "." }));
            target.Add(new ValidationResult("Error 2"));
            target.Add(new ValidationResult("Error 3", new string[] { "Test" }));

            int actual = target.IndexOf(new ValidationResult("Error 1", new string[] { "." }));
            Assert.That(actual, Is.EqualTo(0));
            actual = target.IndexOf(target[0]);
            Assert.That(actual, Is.EqualTo(0));
            actual = target.IndexOf(new ValidationResult("Error 2"));
            Assert.That(actual, Is.EqualTo(1));
            actual = target.IndexOf(target[1]);
            Assert.That(actual, Is.EqualTo(1));
            actual = target.IndexOf(new ValidationResult("Error 3", new string[] { "Test" }));
            Assert.That(actual, Is.EqualTo(2));
            actual = target.IndexOf(target[2]);
            Assert.That(actual, Is.EqualTo(2));

            actual = target.IndexOf(new ValidationResult("Error 1", new string[] { ".", "", " " }));
            Assert.That(actual, Is.EqualTo(0));
            actual = target.IndexOf(new ValidationResult("Error 2", new string[] { null, "" }));
            Assert.That(actual, Is.EqualTo(1));
            actual = target.IndexOf(new ValidationResult("Error 2", null));
            Assert.That(actual, Is.EqualTo(1));
            actual = target.IndexOf(new ValidationResult("Error 3", new string[] { null, "Test" }));
            Assert.That(actual, Is.EqualTo(2));

            actual = target.IndexOf(new ValidationResult("Error 1", new string[] { ".", "2" }));
            Assert.That(actual, Is.EqualTo(-1));
            actual = target.IndexOf(new ValidationResult("Error 1"));
            Assert.That(actual, Is.EqualTo(-1));
            actual = target.IndexOf(new ValidationResult("Error 2", new string[] { "." }));
            Assert.That(actual, Is.EqualTo(-1));
            actual = target.IndexOf(new ValidationResult("Error 3", new string[] { "." }));
            Assert.That(actual, Is.EqualTo(-1));
            actual = target.IndexOf(new ValidationResult("Error 3", null));
            Assert.That(actual, Is.EqualTo(-1));
            actual = target.IndexOf(new ValidationResult(null));
            Assert.That(actual, Is.EqualTo(-1));
            actual = target.IndexOf(null);
            Assert.That(actual, Is.EqualTo(-1));
        }

        [Test]
        public void RemoveTest()
        {
            ValidationResultCollection target = new ValidationResultCollection();

            // 0: { ErrorMessage = "Error 1", MemberNames = ["."] }
            // 1: { ErrorMessage = "Error 2", MemberNames = [] }
            // 2: { ErrorMessage = "Error 3", MemberNames = ["Test"] }
            target.Add(new ValidationResult("Error 1", new string[] { "." }));
            target.Add(new ValidationResult("Error 2"));
            target.Add(new ValidationResult("Error 3", new string[] { "Test" }));
            ValidationResult item0 = target[0];
            ValidationResult item1 = target[1];
            ValidationResult item2 = target[2];

            bool actual = target.Remove(new ValidationResult("Error 1", new string[] { ".", "2" }));
            Assert.That(actual, Is.False);
            Assert.That(target.Count, Is.EqualTo(3));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(target[1], Is.SameAs(item1));
            Assert.That(target[2], Is.SameAs(item2));

            actual = target.Remove(new ValidationResult("Error 1"));
            Assert.That(actual, Is.False);
            Assert.That(target.Count, Is.EqualTo(3));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(target[1], Is.SameAs(item1));
            Assert.That(target[2], Is.SameAs(item2));

            actual = target.Remove(new ValidationResult("Error 2", new string[] { "." }));
            Assert.That(actual, Is.False);
            Assert.That(target.Count, Is.EqualTo(3));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(target[1], Is.SameAs(item1));
            Assert.That(target[2], Is.SameAs(item2));

            actual = target.Remove(new ValidationResult("Error 3", new string[] { "." }));
            Assert.That(actual, Is.False);
            Assert.That(target.Count, Is.EqualTo(3));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(target[1], Is.SameAs(item1));
            Assert.That(target[2], Is.SameAs(item2));

            actual = target.Remove(new ValidationResult("Error 3", null));
            Assert.That(actual, Is.False);
            Assert.That(target.Count, Is.EqualTo(3));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(target[1], Is.SameAs(item1));
            Assert.That(target[2], Is.SameAs(item2));

            actual = target.Remove(new ValidationResult(null));
            Assert.That(actual, Is.False);
            Assert.That(target.Count, Is.EqualTo(3));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(target[1], Is.SameAs(item1));
            Assert.That(target[2], Is.SameAs(item2));

            actual = target.Remove(null);
            Assert.That(actual, Is.False);
            Assert.That(target.Count, Is.EqualTo(3));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(target[1], Is.SameAs(item1));
            Assert.That(target[2], Is.SameAs(item2));

            // 0:    { ErrorMessage = "Error 1", MemberNames = ["."] }
            // 1<=2: { ErrorMessage = "Error 3", MemberNames = ["Test"] }; Remove: { ErrorMessage = "Error 2", MemberNames = [] }
            actual = target.Remove(new ValidationResult("Error 2"));
            item1 = item2;
            Assert.That(actual, Is.True);
            Assert.That(target.Count, Is.EqualTo(2));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(target[1], Is.SameAs(item1));

            // 0:  { ErrorMessage = "Error 1", MemberNames = ["."] }
            // 1:  { ErrorMessage = "Error 3", MemberNames = ["Test"] }
            // 2*: { ErrorMessage = "Error 2", MemberNames = [] }
            target.Add(new ValidationResult("Error 2"));
            item2 = target[2];
            // 0<=1: { ErrorMessage = "Error 3", MemberNames = ["Test"] }; Remove: { ErrorMessage = "Error 1", MemberNames = ["."] }
            // 1<=2: { ErrorMessage = "Error 2", MemberNames = [] }
            actual = target.Remove(target[0]);
            item0 = item1;
            item1 = item2;
            Assert.That(actual, Is.True);
            Assert.That(target.Count, Is.EqualTo(2));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(target[1], Is.SameAs(item1));

            // 0:    { ErrorMessage = "Error 3", MemberNames = ["Test"] }
            // 1*:   { ErrorMessage = "Error 2", MemberNames = ["."] }
            // 2<=1: { ErrorMessage = "Error 2", MemberNames = [] }
            target.Insert(1, new ValidationResult("Error 2", new string[] { "." }));
            item1 = target[1];
            // 0: { ErrorMessage = "Error 3", MemberNames = ["Test"] }
            // 1: { ErrorMessage = "Error 2", MemberNames = ["."] }
            //    Remove: { ErrorMessage = "Error 2", MemberNames = [] }
            actual = target.Remove(new ValidationResult("Error 2", new string[] { "" }));
            Assert.That(actual, Is.True);
            Assert.That(target.Count, Is.EqualTo(2));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(target[1], Is.SameAs(item1));

            // 0:  { ErrorMessage = "Error 3", MemberNames = ["Test"] }
            // 1:  { ErrorMessage = "Error 2", MemberNames = ["."] }
            // 2*: { ErrorMessage = "Error 2", MemberNames = [] }
            target.Add(new ValidationResult("Error 2"));
            item2 = target[2];
            // 0: { ErrorMessage = "Error 3", MemberNames = ["Test"] }
            // 1: { ErrorMessage = "Error 2", MemberNames = [] }; Removed: { ErrorMessage = "Error 2", MemberNames = ["."] }
            actual = target.Remove(new ValidationResult("Error 2", new string[] { "", ".", null }));
            item1 = item2;
            Assert.That(actual, Is.True);
            Assert.That(target.Count, Is.EqualTo(2));
            Assert.That(target[0], Is.SameAs(item0));
            Assert.That(target[1], Is.SameAs(item1));

            actual = target.Remove(new ValidationResult("Error 3", new string[] { "Test" }));
            Assert.That(actual, Is.True);
            Assert.That(target.Count, Is.EqualTo(1));
            Assert.That(target[0], Is.SameAs(item1));

            actual = target.Remove(new ValidationResult("Error 2", Array.Empty<string>()));
            Assert.That(actual, Is.True);
            Assert.That(target.Count, Is.EqualTo(0));
        }
    }
}
