using FsInfoCat.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace FsInfoCat.Test
{
    public class SuspendableTests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ServiceTest()
        {
            ISuspendable x = Extensions.NewSuspendable();
            Assert.That(x, Is.Not.Null);
            Assert.That(x.IsSuspended, Is.False);
            Assert.That(x.SyncRoot, Is.Not.Null);
            ISuspendable y = Extensions.NewSuspendable();
            Assert.That(y, Is.Not.Null);
            Assert.That(x, Is.Not.SameAs(y));
            Assert.That(y.IsSuspended, Is.False);
            Assert.That(y.SyncRoot, Is.Not.Null);
        }

        [Test]
        public void SuspendTest()
        {
            ISuspendable target = Extensions.NewSuspendable();
            Assert.That(target.IsSuspended, Is.False);
            ISuspension x, y;
            using (x = target.Suspend())
            {
                Assert.That(target.IsSuspended, Is.True);
                Assert.That(x, Is.Not.Null);
                Assert.That(x.ConcurrencyToken, Is.Not.Null);
                using (y = target.Suspend())
                {
                    Assert.That(y, Is.Not.Null);
                    Assert.That(x, Is.Not.SameAs(y));
                    Assert.That(target.IsSuspended, Is.True);
                    Assert.That(y.ConcurrencyToken, Is.Not.Null);
                    Assert.That(x.ConcurrencyToken, Is.SameAs(y.ConcurrencyToken));
                }
                Assert.That(target.IsSuspended, Is.True);
            }
            Assert.That(target.IsSuspended, Is.False);

            x = target.Suspend();
            Assert.That(target.IsSuspended, Is.True);
            Assert.That(x, Is.Not.Null);
            Assert.That(x.ConcurrencyToken, Is.Not.Null);
            using (y = target.Suspend())
            {
                Assert.That(y, Is.Not.Null);
                Assert.That(x, Is.Not.SameAs(y));
                Assert.That(target.IsSuspended, Is.True);
                Assert.That(y.ConcurrencyToken, Is.Not.Null);
                Assert.That(x.ConcurrencyToken, Is.SameAs(y.ConcurrencyToken));
                x.Dispose();
                Assert.That(target.IsSuspended, Is.True);
            }
            Assert.That(target.IsSuspended, Is.False);

            x = target.Suspend();
            Assert.That(target.IsSuspended, Is.True);
            Assert.That(x, Is.Not.Null);
            Assert.That(x.ConcurrencyToken, Is.Not.Null);
            using (ISuspension z = target.Suspend())
            {
                Assert.That(z, Is.Not.Null);
                Assert.That(x, Is.Not.SameAs(z));
                Assert.That(target.IsSuspended, Is.True);
                Assert.That(z.ConcurrencyToken, Is.Not.Null);
                Assert.That(x.ConcurrencyToken, Is.SameAs(z.ConcurrencyToken));
                y = target.Suspend();
                Assert.That(y, Is.Not.Null);
                Assert.That(x, Is.Not.SameAs(y));
                Assert.That(target.IsSuspended, Is.True);
                Assert.That(y.ConcurrencyToken, Is.Not.Null);
                Assert.That(x.ConcurrencyToken, Is.SameAs(y.ConcurrencyToken));
                x.Dispose();
                Assert.That(target.IsSuspended, Is.True);
            }
            Assert.That(target.IsSuspended, Is.True);
            y.Dispose();
            Assert.That(target.IsSuspended, Is.False);
        }

        class SuspensionEventTargetListener
        {
            public int Count;
            internal void IncrementAction() => Count++;
            internal void DecrementAction() => Count--;
            internal int CountPlusTwo()
            {
                Count += 2;
                return Count;
            }
            internal int CountMinusTwo()
            {
                Count -= 2;
                return Count;
            }
            internal List<Tuple<object, EventArgs>> BeginSuspensionInvocations { get; } = new List<Tuple<object, EventArgs>>();
            internal List<Tuple<object, EventArgs>> EndSuspensionInvocations { get; } = new List<Tuple<object, EventArgs>>();
            internal void OnBeginSuspension(object sender, EventArgs e)
            {
                BeginSuspensionInvocations.Add(new Tuple<object, EventArgs>(sender, e));
            }

            internal void OnEndSuspension(object sender, EventArgs e)
            {
                EndSuspensionInvocations.Add(new Tuple<object, EventArgs>(sender, e));
            }
        }

        [Test]
        public void SuspensionEventsTest()
        {
            SuspensionEventTargetListener eventTarget = new SuspensionEventTargetListener();
            ISuspendable target = Extensions.NewSuspendable();
            target.BeginSuspension += eventTarget.OnBeginSuspension;
            target.EndSuspension += eventTarget.OnEndSuspension;
            Assert.That(target.IsSuspended, Is.False);
            Assert.That(eventTarget.BeginSuspensionInvocations.Count, Is.EqualTo(0));
            Assert.That(eventTarget.EndSuspensionInvocations.Count, Is.EqualTo(0));
            Tuple<object, EventArgs> invocationData;
            ISuspension x, y;
            using (x = target.Suspend())
            {
                Assert.That(target.IsSuspended, Is.True);
                Assert.That(eventTarget.BeginSuspensionInvocations.Count, Is.EqualTo(1));
                Assert.That(eventTarget.EndSuspensionInvocations.Count, Is.EqualTo(0));
                invocationData = eventTarget.BeginSuspensionInvocations[0];
                Assert.That(invocationData.Item1, Is.Not.Null);
                Assert.That(invocationData.Item2, Is.Not.Null);
                Assert.That(invocationData.Item1, Is.SameAs(target));
                eventTarget.BeginSuspensionInvocations.Clear();
                using (y = target.Suspend())
                {
                    Assert.That(eventTarget.BeginSuspensionInvocations.Count, Is.EqualTo(0));
                    Assert.That(eventTarget.EndSuspensionInvocations.Count, Is.EqualTo(0));
                    Assert.That(target.IsSuspended, Is.True);
                }
                Assert.That(eventTarget.BeginSuspensionInvocations.Count, Is.EqualTo(0));
                Assert.That(eventTarget.EndSuspensionInvocations.Count, Is.EqualTo(0));
                Assert.That(target.IsSuspended, Is.True);
            }
            Assert.That(eventTarget.BeginSuspensionInvocations.Count, Is.EqualTo(0));
            Assert.That(eventTarget.EndSuspensionInvocations.Count, Is.EqualTo(1));
            invocationData = eventTarget.EndSuspensionInvocations[0];
            Assert.That(invocationData.Item1, Is.Not.Null);
            Assert.That(invocationData.Item2, Is.Not.Null);
            Assert.That(invocationData.Item1, Is.SameAs(target));
            Assert.That(target.IsSuspended, Is.False);
            eventTarget.EndSuspensionInvocations.Clear();

            x = target.Suspend();
            Assert.That(target.IsSuspended, Is.True);
            Assert.That(eventTarget.BeginSuspensionInvocations.Count, Is.EqualTo(1));
            Assert.That(eventTarget.EndSuspensionInvocations.Count, Is.EqualTo(0));
            invocationData = eventTarget.BeginSuspensionInvocations[0];
            Assert.That(invocationData.Item1, Is.Not.Null);
            Assert.That(invocationData.Item2, Is.Not.Null);
            Assert.That(invocationData.Item1, Is.SameAs(target));
            eventTarget.BeginSuspensionInvocations.Clear();
            using (y = target.Suspend())
            {
                Assert.That(eventTarget.BeginSuspensionInvocations.Count, Is.EqualTo(0));
                Assert.That(eventTarget.EndSuspensionInvocations.Count, Is.EqualTo(0));
                Assert.That(target.IsSuspended, Is.True);
                x.Dispose();
                Assert.That(eventTarget.BeginSuspensionInvocations.Count, Is.EqualTo(0));
                Assert.That(eventTarget.EndSuspensionInvocations.Count, Is.EqualTo(0));
                Assert.That(target.IsSuspended, Is.True);
            }
            Assert.That(eventTarget.BeginSuspensionInvocations.Count, Is.EqualTo(0));
            Assert.That(eventTarget.EndSuspensionInvocations.Count, Is.EqualTo(1));
            invocationData = eventTarget.EndSuspensionInvocations[0];
            Assert.That(invocationData.Item1, Is.Not.Null);
            Assert.That(invocationData.Item2, Is.Not.Null);
            Assert.That(invocationData.Item1, Is.SameAs(target));
            Assert.That(target.IsSuspended, Is.False);
            eventTarget.EndSuspensionInvocations.Clear();

            x = target.Suspend();
            Assert.That(target.IsSuspended, Is.True);
            Assert.That(eventTarget.BeginSuspensionInvocations.Count, Is.EqualTo(1));
            Assert.That(eventTarget.EndSuspensionInvocations.Count, Is.EqualTo(0));
            invocationData = eventTarget.BeginSuspensionInvocations[0];
            Assert.That(invocationData.Item1, Is.Not.Null);
            Assert.That(invocationData.Item2, Is.Not.Null);
            Assert.That(invocationData.Item1, Is.SameAs(target));
            eventTarget.BeginSuspensionInvocations.Clear();
            using (ISuspension z = target.Suspend())
            {
                Assert.That(eventTarget.BeginSuspensionInvocations.Count, Is.EqualTo(0));
                Assert.That(eventTarget.EndSuspensionInvocations.Count, Is.EqualTo(0));
                Assert.That(target.IsSuspended, Is.True);
                y = target.Suspend();
                Assert.That(eventTarget.BeginSuspensionInvocations.Count, Is.EqualTo(0));
                Assert.That(eventTarget.EndSuspensionInvocations.Count, Is.EqualTo(0));
                Assert.That(target.IsSuspended, Is.True);
                x.Dispose();
                Assert.That(eventTarget.BeginSuspensionInvocations.Count, Is.EqualTo(0));
                Assert.That(eventTarget.EndSuspensionInvocations.Count, Is.EqualTo(0));
                Assert.That(target.IsSuspended, Is.True);
            }
            Assert.That(eventTarget.BeginSuspensionInvocations.Count, Is.EqualTo(0));
            Assert.That(eventTarget.EndSuspensionInvocations.Count, Is.EqualTo(0));
            Assert.That(target.IsSuspended, Is.True);
            y.Dispose();
            Assert.That(eventTarget.BeginSuspensionInvocations.Count, Is.EqualTo(0));
            Assert.That(eventTarget.EndSuspensionInvocations.Count, Is.EqualTo(1));
            invocationData = eventTarget.EndSuspensionInvocations[0];
            Assert.That(invocationData.Item1, Is.Not.Null);
            Assert.That(invocationData.Item2, Is.Not.Null);
            Assert.That(invocationData.Item1, Is.SameAs(target));
            Assert.That(target.IsSuspended, Is.False);
            eventTarget.EndSuspensionInvocations.Clear();
        }

        [Test]
        public void AssertNotSuspendedTest()
        {
            SuspensionEventTargetListener listener = new SuspensionEventTargetListener();
            ISuspendable target = Extensions.NewSuspendable();
            Assert.That(target.IsSuspended, Is.False);
            target.AssertNotSuspended();
            target.AssertNotSuspended(listener.IncrementAction);
            Assert.That(listener.Count, Is.EqualTo(1));
            int actual = target.AssertNotSuspended(listener.CountPlusTwo);
            Assert.That(actual, Is.EqualTo(3));
            Assert.That(listener.Count, Is.EqualTo(3));

            ISuspension x, y;
            using (x = target.Suspend())
            {
                Assert.That(target.IsSuspended, Is.True);
                Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended());
                Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended(listener.IncrementAction));
                Assert.That(listener.Count, Is.EqualTo(3));
                Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended(listener.CountPlusTwo));
                Assert.That(listener.Count, Is.EqualTo(3));
                using (y = target.Suspend())
                {
                    Assert.That(target.IsSuspended, Is.True);
                    Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended());
                    Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended(listener.IncrementAction));
                    Assert.That(listener.Count, Is.EqualTo(3));
                    Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended(listener.CountPlusTwo));
                    Assert.That(listener.Count, Is.EqualTo(3));
                }
                Assert.That(target.IsSuspended, Is.True);
                Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended());
                Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended(listener.IncrementAction));
                Assert.That(listener.Count, Is.EqualTo(3));
                Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended(listener.CountPlusTwo));
                Assert.That(listener.Count, Is.EqualTo(3));
            }
            Assert.That(target.IsSuspended, Is.False);
            target.AssertNotSuspended();
            target.AssertNotSuspended(listener.DecrementAction);
            Assert.That(listener.Count, Is.EqualTo(2));
            actual = target.AssertNotSuspended(listener.CountMinusTwo);
            Assert.That(actual, Is.EqualTo(0));
            Assert.That(listener.Count, Is.EqualTo(0));

            x = target.Suspend();
            Assert.That(target.IsSuspended, Is.True);
            Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended());
            Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended(listener.IncrementAction));
            Assert.That(listener.Count, Is.EqualTo(0));
            Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended(listener.CountPlusTwo));
            Assert.That(listener.Count, Is.EqualTo(0));
            using (y = target.Suspend())
            {
                Assert.That(target.IsSuspended, Is.True);
                Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended());
                Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended(listener.IncrementAction));
                Assert.That(listener.Count, Is.EqualTo(0));
                Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended(listener.CountPlusTwo));
                Assert.That(listener.Count, Is.EqualTo(0));
                x.Dispose();
                Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended());
                Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended(listener.IncrementAction));
                Assert.That(listener.Count, Is.EqualTo(0));
                Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended(listener.CountPlusTwo));
                Assert.That(listener.Count, Is.EqualTo(0));
            }
            Assert.That(target.IsSuspended, Is.False);
            target.AssertNotSuspended();
            target.AssertNotSuspended(listener.IncrementAction);
            Assert.That(listener.Count, Is.EqualTo(1));
            actual = target.AssertNotSuspended(listener.CountPlusTwo);
            Assert.That(actual, Is.EqualTo(3));
            Assert.That(listener.Count, Is.EqualTo(3));

            x = target.Suspend();
            Assert.That(target.IsSuspended, Is.True);
            Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended());
            Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended(listener.IncrementAction));
            Assert.That(listener.Count, Is.EqualTo(3));
            Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended(listener.CountPlusTwo));
            Assert.That(listener.Count, Is.EqualTo(3));
            using (ISuspension z = target.Suspend())
            {
                Assert.That(target.IsSuspended, Is.True);
                Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended());
                Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended(listener.IncrementAction));
                Assert.That(listener.Count, Is.EqualTo(3));
                Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended(listener.CountPlusTwo));
                Assert.That(listener.Count, Is.EqualTo(3));
                y = target.Suspend();
                Assert.That(target.IsSuspended, Is.True);
                Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended());
                Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended(listener.IncrementAction));
                Assert.That(listener.Count, Is.EqualTo(3));
                Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended(listener.CountPlusTwo));
                Assert.That(listener.Count, Is.EqualTo(3));
                x.Dispose();
                Assert.That(target.IsSuspended, Is.True);
                Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended());
                Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended(listener.IncrementAction));
                Assert.That(listener.Count, Is.EqualTo(3));
                Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended(listener.CountPlusTwo));
                Assert.That(listener.Count, Is.EqualTo(3));
            }
            Assert.That(target.IsSuspended, Is.True);
            Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended());
            Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended(listener.IncrementAction));
            Assert.That(listener.Count, Is.EqualTo(3));
            Assert.Throws<InvalidOperationException>(() => target.AssertNotSuspended(listener.CountPlusTwo));
            Assert.That(listener.Count, Is.EqualTo(3));
            y.Dispose();
            Assert.That(target.IsSuspended, Is.False);
            target.AssertNotSuspended();
            target.AssertNotSuspended(listener.DecrementAction);
            Assert.That(listener.Count, Is.EqualTo(2));
            actual = target.AssertNotSuspended(listener.CountMinusTwo);
            Assert.That(actual, Is.EqualTo(0));
            Assert.That(listener.Count, Is.EqualTo(0));
        }

        [Test]
        public void IfNotSuspendedTest()
        {
            SuspensionEventTargetListener eventTarget = new SuspensionEventTargetListener();
            ISuspendable target = Extensions.NewSuspendable();
            Assert.That(target.IsSuspended, Is.False);
            target.IfNotSuspended(eventTarget.DecrementAction);
            Assert.That(eventTarget.Count, Is.EqualTo(-1));
            target.IfNotSuspended(eventTarget.DecrementAction, eventTarget.IncrementAction);
            Assert.That(eventTarget.Count, Is.EqualTo(-2));
            int actual = target.IfNotSuspended(eventTarget.CountMinusTwo, eventTarget.CountPlusTwo);
            Assert.That(actual, Is.EqualTo(-4));
            Assert.That(eventTarget.Count, Is.EqualTo(-4));
            actual = target.IfNotSuspended(eventTarget.CountMinusTwo, 5);
            Assert.That(actual, Is.EqualTo(-6));
            Assert.That(eventTarget.Count, Is.EqualTo(-6));

            ISuspension x, y;
            using (x = target.Suspend())
            {
                Assert.That(target.IsSuspended, Is.True);
                target.IfNotSuspended(eventTarget.DecrementAction);
                Assert.That(eventTarget.Count, Is.EqualTo(-6));
                target.IfNotSuspended(eventTarget.DecrementAction, eventTarget.IncrementAction);
                Assert.That(eventTarget.Count, Is.EqualTo(-5));
                actual = target.IfNotSuspended(eventTarget.CountMinusTwo, eventTarget.CountPlusTwo);
                Assert.That(actual, Is.EqualTo(-3));
                Assert.That(eventTarget.Count, Is.EqualTo(-3));
                actual = target.IfNotSuspended(eventTarget.CountMinusTwo, 2);
                Assert.That(actual, Is.EqualTo(2));
                Assert.That(eventTarget.Count, Is.EqualTo(-3));
                using (y = target.Suspend())
                {
                    Assert.That(target.IsSuspended, Is.True);
                    target.IfNotSuspended(eventTarget.DecrementAction);
                    Assert.That(eventTarget.Count, Is.EqualTo(-3));
                    target.IfNotSuspended(eventTarget.DecrementAction, eventTarget.IncrementAction);
                    Assert.That(eventTarget.Count, Is.EqualTo(-2));
                    actual = target.IfNotSuspended(eventTarget.CountMinusTwo, eventTarget.CountPlusTwo);
                    Assert.That(actual, Is.EqualTo(0));
                    Assert.That(eventTarget.Count, Is.EqualTo(0));
                    actual = target.IfNotSuspended(eventTarget.CountMinusTwo, 3);
                    Assert.That(actual, Is.EqualTo(3));
                    Assert.That(eventTarget.Count, Is.EqualTo(0));
                }
                Assert.That(target.IsSuspended, Is.True);
                target.IfNotSuspended(eventTarget.DecrementAction);
                Assert.That(eventTarget.Count, Is.EqualTo(0));
                target.IfNotSuspended(eventTarget.DecrementAction, eventTarget.IncrementAction);
                Assert.That(eventTarget.Count, Is.EqualTo(1));
                actual = target.IfNotSuspended(eventTarget.CountMinusTwo, eventTarget.CountPlusTwo);
                Assert.That(actual, Is.EqualTo(3));
                Assert.That(eventTarget.Count, Is.EqualTo(3));
                actual = target.IfNotSuspended(eventTarget.CountMinusTwo, 5);
                Assert.That(actual, Is.EqualTo(5));
                Assert.That(eventTarget.Count, Is.EqualTo(3));
            }
            Assert.That(target.IsSuspended, Is.False);
            target.IfNotSuspended(eventTarget.DecrementAction);
            Assert.That(eventTarget.Count, Is.EqualTo(2));
            target.IfNotSuspended(eventTarget.DecrementAction, eventTarget.IncrementAction);
            Assert.That(eventTarget.Count, Is.EqualTo(1));
            actual = target.IfNotSuspended(eventTarget.CountMinusTwo, eventTarget.CountPlusTwo);
            Assert.That(actual, Is.EqualTo(-1));
            Assert.That(eventTarget.Count, Is.EqualTo(-1));
            actual = target.IfNotSuspended(eventTarget.CountMinusTwo, 4);
            Assert.That(actual, Is.EqualTo(-3));
            Assert.That(eventTarget.Count, Is.EqualTo(-3));

            x = target.Suspend();
            Assert.That(target.IsSuspended, Is.True);
            target.IfNotSuspended(eventTarget.DecrementAction);
            Assert.That(eventTarget.Count, Is.EqualTo(-3));
            target.IfNotSuspended(eventTarget.DecrementAction, eventTarget.IncrementAction);
            Assert.That(eventTarget.Count, Is.EqualTo(-2));
            actual = target.IfNotSuspended(eventTarget.CountMinusTwo, eventTarget.CountPlusTwo);
            Assert.That(actual, Is.EqualTo(0));
            Assert.That(eventTarget.Count, Is.EqualTo(0));
            actual = target.IfNotSuspended(eventTarget.CountMinusTwo, -5);
            Assert.That(actual, Is.EqualTo(-5));
            Assert.That(eventTarget.Count, Is.EqualTo(0));
            using (y = target.Suspend())
            {
                Assert.That(target.IsSuspended, Is.True);
                target.IfNotSuspended(eventTarget.DecrementAction);
                Assert.That(eventTarget.Count, Is.EqualTo(0));
                target.IfNotSuspended(eventTarget.DecrementAction, eventTarget.IncrementAction);
                Assert.That(eventTarget.Count, Is.EqualTo(1));
                actual = target.IfNotSuspended(eventTarget.CountMinusTwo, eventTarget.CountPlusTwo);
                Assert.That(actual, Is.EqualTo(3));
                Assert.That(eventTarget.Count, Is.EqualTo(3));
                actual = target.IfNotSuspended(eventTarget.CountMinusTwo, 12);
                Assert.That(actual, Is.EqualTo(12));
                Assert.That(eventTarget.Count, Is.EqualTo(3));
                x.Dispose();
                target.IfNotSuspended(eventTarget.DecrementAction);
                Assert.That(eventTarget.Count, Is.EqualTo(3));
                target.IfNotSuspended(eventTarget.DecrementAction, eventTarget.IncrementAction);
                Assert.That(eventTarget.Count, Is.EqualTo(4));
                actual = target.IfNotSuspended(eventTarget.CountMinusTwo, eventTarget.CountPlusTwo);
                Assert.That(actual, Is.EqualTo(6));
                Assert.That(eventTarget.Count, Is.EqualTo(6));
                actual = target.IfNotSuspended(eventTarget.CountMinusTwo, 10);
                Assert.That(actual, Is.EqualTo(10));
                Assert.That(eventTarget.Count, Is.EqualTo(6));
            }
            Assert.That(target.IsSuspended, Is.False);
            target.IfNotSuspended(eventTarget.DecrementAction);
            Assert.That(eventTarget.Count, Is.EqualTo(5));
            target.IfNotSuspended(eventTarget.DecrementAction, eventTarget.IncrementAction);
            Assert.That(eventTarget.Count, Is.EqualTo(4));
            actual = target.IfNotSuspended(eventTarget.CountMinusTwo, eventTarget.CountPlusTwo);
            Assert.That(actual, Is.EqualTo(2));
            Assert.That(eventTarget.Count, Is.EqualTo(2));
            actual = target.IfNotSuspended(eventTarget.CountMinusTwo, 4);
            Assert.That(actual, Is.EqualTo(0));
            Assert.That(eventTarget.Count, Is.EqualTo(0));

            x = target.Suspend();
            Assert.That(target.IsSuspended, Is.True);
            target.IfNotSuspended(eventTarget.DecrementAction);
            Assert.That(eventTarget.Count, Is.EqualTo(0));
            target.IfNotSuspended(eventTarget.DecrementAction, eventTarget.IncrementAction);
            Assert.That(eventTarget.Count, Is.EqualTo(1));
            actual = target.IfNotSuspended(eventTarget.CountMinusTwo, eventTarget.CountPlusTwo);
            Assert.That(actual, Is.EqualTo(3));
            Assert.That(eventTarget.Count, Is.EqualTo(3));
            actual = target.IfNotSuspended(eventTarget.CountMinusTwo, 2);
            Assert.That(actual, Is.EqualTo(2));
            Assert.That(eventTarget.Count, Is.EqualTo(3));
            using (ISuspension z = target.Suspend())
            {
                Assert.That(target.IsSuspended, Is.True);
                target.IfNotSuspended(eventTarget.DecrementAction);
                Assert.That(eventTarget.Count, Is.EqualTo(3));
                target.IfNotSuspended(eventTarget.DecrementAction, eventTarget.IncrementAction);
                Assert.That(eventTarget.Count, Is.EqualTo(4));
                actual = target.IfNotSuspended(eventTarget.CountMinusTwo, eventTarget.CountPlusTwo);
                Assert.That(actual, Is.EqualTo(6));
                Assert.That(eventTarget.Count, Is.EqualTo(6));
                actual = target.IfNotSuspended(eventTarget.CountMinusTwo, 3);
                Assert.That(actual, Is.EqualTo(3));
                Assert.That(eventTarget.Count, Is.EqualTo(6));
                y = target.Suspend();
                Assert.That(target.IsSuspended, Is.True);
                target.IfNotSuspended(eventTarget.DecrementAction);
                Assert.That(eventTarget.Count, Is.EqualTo(6));
                target.IfNotSuspended(eventTarget.DecrementAction, eventTarget.IncrementAction);
                Assert.That(eventTarget.Count, Is.EqualTo(7));
                actual = target.IfNotSuspended(eventTarget.CountMinusTwo, eventTarget.CountPlusTwo);
                Assert.That(actual, Is.EqualTo(9));
                Assert.That(eventTarget.Count, Is.EqualTo(9));
                actual = target.IfNotSuspended(eventTarget.CountMinusTwo, 4);
                Assert.That(actual, Is.EqualTo(4));
                Assert.That(eventTarget.Count, Is.EqualTo(9));
                x.Dispose();
                Assert.That(target.IsSuspended, Is.True);
                target.IfNotSuspended(eventTarget.DecrementAction);
                Assert.That(eventTarget.Count, Is.EqualTo(9));
                target.IfNotSuspended(eventTarget.DecrementAction, eventTarget.IncrementAction);
                Assert.That(eventTarget.Count, Is.EqualTo(10));
                actual = target.IfNotSuspended(eventTarget.CountMinusTwo, eventTarget.CountPlusTwo);
                Assert.That(actual, Is.EqualTo(12));
                Assert.That(eventTarget.Count, Is.EqualTo(12));
                actual = target.IfNotSuspended(eventTarget.CountMinusTwo, 5);
                Assert.That(actual, Is.EqualTo(5));
                Assert.That(eventTarget.Count, Is.EqualTo(12));
            }
            target.IfNotSuspended(eventTarget.DecrementAction);
            Assert.That(eventTarget.Count, Is.EqualTo(12));
            target.IfNotSuspended(eventTarget.DecrementAction, eventTarget.IncrementAction);
            Assert.That(eventTarget.Count, Is.EqualTo(13));
            actual = target.IfNotSuspended(eventTarget.CountMinusTwo, eventTarget.CountPlusTwo);
            Assert.That(actual, Is.EqualTo(15));
            Assert.That(eventTarget.Count, Is.EqualTo(15));
            actual = target.IfNotSuspended(eventTarget.CountMinusTwo, 6);
            Assert.That(actual, Is.EqualTo(6));
            Assert.That(eventTarget.Count, Is.EqualTo(15));
            y.Dispose();
            Assert.That(target.IsSuspended, Is.False);
            target.IfNotSuspended(eventTarget.DecrementAction);
            Assert.That(eventTarget.Count, Is.EqualTo(14));
            target.IfNotSuspended(eventTarget.DecrementAction, eventTarget.IncrementAction);
            Assert.That(eventTarget.Count, Is.EqualTo(13));
            actual = target.IfNotSuspended(eventTarget.CountMinusTwo, eventTarget.CountPlusTwo);
            Assert.That(eventTarget.Count, Is.EqualTo(11));
            actual = target.IfNotSuspended(eventTarget.CountMinusTwo, 10);
            Assert.That(actual, Is.EqualTo(9));
            Assert.That(eventTarget.Count, Is.EqualTo(9));
        }

        class BlockingTestHelper
        {
            internal int Value { get; private set; }
            internal readonly ISuspendable Target = Extensions.NewSuspendable();
            internal void Increment()
            {
                using (ISuspension suspension = Target.Suspend())
                {
                    Value++;
                }
            }
        }

        [Test]
        public void BlockingTest()
        {
            BlockingTestHelper helper = new BlockingTestHelper();
            using (AutoResetEvent resetA = new AutoResetEvent(false))
            {
                using (AutoResetEvent resetB = new AutoResetEvent(false))
                {
                    using (AutoResetEvent resetC = new AutoResetEvent(false))
                    {
                        Task task = Task.Factory.StartNew(() =>
                        {
                            helper.Increment();
                            resetA.Set();
                            resetB.WaitOne();
                            helper.Increment();
                            resetC.Set();
                        });
                        Thread.Sleep(100);
                        resetA.WaitOne();
                        Assert.That(helper.Value, Is.EqualTo(1));
                        using (ISuspension suspension = helper.Target.Suspend())
                        {
                            resetB.Set();
                            Thread.Sleep(100);
                        }
                        Thread.Sleep(100);
                        resetC.WaitOne();
                        task.Wait(1000);
                        Assert.That(task.Status, Is.EqualTo(TaskStatus.RanToCompletion));
                        Assert.That(helper.Value, Is.EqualTo(2));
                    }
                }
            }
            using (AutoResetEvent resetA = new AutoResetEvent(false))
            {
                using (AutoResetEvent resetB = new AutoResetEvent(false))
                {
                    using (AutoResetEvent resetC = new AutoResetEvent(false))
                    {
                        Task task = Task.Factory.StartNew(() =>
                        {
                            helper.Increment();
                            resetA.Set();
                            resetB.WaitOne();
                            helper.Increment();
                            resetC.Set();
                        });
                        Thread.Sleep(100);
                        resetA.WaitOne();
                        Assert.That(helper.Value, Is.EqualTo(3));
                        using (ISuspension suspension = helper.Target.Suspend(false))
                        {
                            resetB.Set();
                            Thread.Sleep(100);
                        }
                        Thread.Sleep(100);
                        resetC.WaitOne();
                        task.Wait(1000);
                        Assert.That(task.Status, Is.EqualTo(TaskStatus.RanToCompletion));
                        Assert.That(helper.Value, Is.EqualTo(4));
                    }
                }
            }
            using (AutoResetEvent resetA = new AutoResetEvent(false))
            {
                using (AutoResetEvent resetB = new AutoResetEvent(false))
                {
                    using (AutoResetEvent resetC = new AutoResetEvent(false))
                    {
                        Task task = Task.Factory.StartNew(() =>
                        {
                            helper.Increment();
                            resetA.Set();
                            resetB.WaitOne();
                            helper.Increment();
                            resetC.Set();
                        });
                        Thread.Sleep(100);
                        resetA.WaitOne();
                        Assert.That(helper.Value, Is.EqualTo(5));
                        using (ISuspension suspension = helper.Target.Suspend(true))
                        {
                            resetB.Set();
                            resetC.WaitOne(1000);
                            Assert.That(helper.Value, Is.EqualTo(6));
                        }
                        task.Wait(100);
                        Assert.That(task.Status, Is.EqualTo(TaskStatus.RanToCompletion));
                    }
                }
            }
        }
    }
}
