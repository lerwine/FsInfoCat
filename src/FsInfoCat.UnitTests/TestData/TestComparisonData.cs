using System;
using System.Collections.ObjectModel;

namespace FsInfoCat.UnitTests.TestData
{
    public record TestComparisonData
    {
        public Guid? UpstreamId => throw new NotImplementedException();

        public DateTime? LastSynchronizedOn => throw new NotImplementedException();

        public bool AreEqual => throw new NotImplementedException();

        public DateTime ComparedOn => throw new NotImplementedException();

        public Guid BaselineId => throw new NotImplementedException();

        public Guid CorrelativeId => throw new NotImplementedException();

        public DateTime CreatedOn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime ModifiedOn { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public static readonly Collection<TestRedundancyData> Data = new();
    }
}
