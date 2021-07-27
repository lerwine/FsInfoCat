using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Linq;

namespace CodeGeneration
{
    public sealed class ComparisonGroup : CheckConstraint, IEquatable<ComparisonGroup>
    {
        public ComparisonGroup(bool isOr, params CheckConstraint[] constraints)
        {
            if (constraints is null || (constraints = constraints.Where(c => c is not null)
                    .SelectMany(c => (c is ComparisonGroup g && g.IsOr == isOr) ? g.Constraints : Enumerable.Repeat(c, 1)).ToArray()).Length == 0)
                throw new ArgumentOutOfRangeException(nameof(constraints));
            using IEnumerator<CheckConstraint> enumerator = (constraints ?? Array.Empty<CheckConstraint>()).Where(c => c is not null).GetEnumerator();
            if (!enumerator.MoveNext())
                throw new ArgumentOutOfRangeException(nameof(constraints));
            IsOr = isOr;
            List<CheckConstraint> checkConstraints = new();
            do
            {
                if (!checkConstraints.Contains(enumerator.Current))
                    checkConstraints.Add(enumerator.Current);
            }
            while (enumerator.MoveNext());
            Constraints = new ReadOnlyCollection<CheckConstraint>(checkConstraints);
        }
        public bool IsOr { get; }
        public ReadOnlyCollection<CheckConstraint> Constraints { get; }
        public override bool IsCompound => Constraints.Count > 1;
        public override CheckConstraint And(CheckConstraint cc)
        {
            if (cc is null || Equals(cc) || Constraints.Contains(cc))
                return this;
            if (IsOr)
                return base.And(cc);
            return new ComparisonGroup(false, Constraints.Concat((cc is ComparisonGroup cg && !cg.IsOr) ? cg.Constraints : Enumerable.Repeat(cc, 1)).ToArray());
        }
        public override CheckConstraint Or(CheckConstraint cc)
        {
            if (cc is null || Equals(cc) || Constraints.Contains(cc))
                return this;
            if (!IsOr)
                return base.Or(cc);
            return new ComparisonGroup(true, Constraints.Concat((cc is ComparisonGroup cg && cg.IsOr) ? cg.Constraints : Enumerable.Repeat(cc, 1)).ToArray());
        }
        public bool Equals(ComparisonGroup other)
        {
            if (other is null)
                return false;
            if (ReferenceEquals(this, other))
                return true;
            if (IsOr != other.IsOr || Constraints.Count != other.Constraints.Count)
                return false;
            return Constraints.All(c => other.Constraints.Contains(c));
        }
        public override bool Equals(CheckConstraint other) => other is ComparisonGroup g && Equals(g);
        public override bool Equals(object obj) => obj is ComparisonGroup other && Equals(other);
        public override int GetHashCode()
        {
            if (Constraints.Count == 0)
                return IsOr ? 1 : 0;
            int seed = Constraints.Count + 1;
            int prime = (seed & 0xffff).FindPrimeNumber();
            seed = (prime + 1).FindPrimeNumber();
            return new int[] { IsOr ? 1 : 0 }.Concat(Constraints.Select(c => c.GetHashCode())).Aggregate(seed, (a, i) =>
            {
                unchecked { return (a * prime) ^ i; }
            });
        }
        public override string ToString() => ToSqlString();
        public override string ToSqlString()
        {
            if (Constraints.Count == 0)
                return "";
            if (Constraints.Count == 1)
                return Constraints[0].ToSqlString();
            return string.Join(IsOr ? " OR " : " AND ", Constraints.Select(c => c.IsCompound ? $"({c.ToSqlString()})" : c.ToSqlString()));
        }
        public override string ToCsString()
        {
            if (Constraints.Count == 0)
                return "";
            if (Constraints.Count == 1)
                return Constraints[0].ToCsString();
            return string.Join(IsOr ? " || " : " && ", Constraints.Select(c => c.IsCompound ? $"({c.ToCsString()})" : c.ToCsString()));
        }
    }
}
