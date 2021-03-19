using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace FsInfoCat.Test.Helpers
{
    public class InvocationResult : IInvocationResult, IEquatable<InvocationResult>
    {
        public InvocationResult(bool wasInvoked) { WasInvoked = wasInvoked; }
        public bool WasInvoked { get; }
        public virtual bool Equals([AllowNull] InvocationResult other) => !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        bool IEquatable<IInvocationResult>.Equals(IInvocationResult other) => Equals(other as InvocationResult);
        public override bool Equals(object obj) => Equals(obj as InvocationResult);
        protected bool PropertiesAreEqual(InvocationResult other) => WasInvoked == other.WasInvoked;
        public override int GetHashCode() => WasInvoked.GetHashCode();
        public override string ToString() => $"{GetType().Name}{{ {PropertiesToString(new StringBuilder())} }}";
        protected virtual StringBuilder PropertiesToString(StringBuilder sb) => sb.Append(nameof(WasInvoked)).Append(" = ").Append(WasInvoked);
    }

    public class InvocationResult<TOut> : InvocationResult, IInvocationResult<TOut>, IEquatable<InvocationResult<TOut>>
    {
        private static readonly IEqualityComparer<TOut> _comparer = EqualityComparer<TOut>.Default;
        public TOut Output1 { get; }
        public InvocationResult() : base(false) { }
        public InvocationResult(TOut o1) : base(true) { Output1 = o1; }
        public virtual bool Equals([AllowNull] InvocationResult<TOut> other) => !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] InvocationResult other) => Equals(other as InvocationResult<TOut>);
        bool IEquatable<IInvocationResult<TOut>>.Equals(IInvocationResult<TOut> other) => Equals(other as InvocationResult<TOut>);
        public override bool Equals(object obj) => Equals(obj as InvocationResult<TOut>);
        public override int GetHashCode() => HashCode.Combine(Output1, WasInvoked);
        protected bool PropertiesAreEqual([DisallowNull] InvocationResult<TOut> other) => base.PropertiesAreEqual(other) && _comparer.Equals(Output1, other.Output1);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Output1)).Append(" = ").Append(Output1);
    }

    public class InvocationResult<TOut1, TOut2> : InvocationResult<TOut1>, IInvocationResult<TOut1, TOut2>, IEquatable<InvocationResult<TOut1, TOut2>>
    {
        private static readonly IEqualityComparer<TOut2> _comparer = EqualityComparer<TOut2>.Default;
        public TOut2 Output2 { get; }
        public InvocationResult() { }
        public InvocationResult(TOut1 o1, TOut2 o2) : base(o1) { Output2 = o2; }
        public virtual bool Equals([AllowNull] InvocationResult<TOut1, TOut2> other) => !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] InvocationResult<TOut1> other) => Equals(other as InvocationResult<TOut1, TOut2>);
        bool IEquatable<IInvocationResult<TOut1, TOut2>>.Equals(IInvocationResult<TOut1, TOut2> other) => Equals(other as InvocationResult<TOut1, TOut2>);
        public override bool Equals(object obj) => Equals(obj as InvocationResult<TOut1, TOut2>);
        public override int GetHashCode() => HashCode.Combine(Output1, Output2, WasInvoked);
        protected bool PropertiesAreEqual([DisallowNull] InvocationResult<TOut1, TOut2> other) => base.PropertiesAreEqual(other) && _comparer.Equals(Output2, other.Output2);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Output1)).Append(" = ").Append(Output1);
    }

    public class InvocationResult<TOut1, TOut2, TOut3> : InvocationResult<TOut1, TOut2>, IInvocationResult<TOut1, TOut2, TOut3>, IEquatable<InvocationResult<TOut1, TOut2, TOut3>>
    {
        private static readonly IEqualityComparer<TOut3> _comparer = EqualityComparer<TOut3>.Default;
        public TOut3 Output3 { get; }
        public InvocationResult() { }
        public InvocationResult(TOut1 o1, TOut2 o2, TOut3 o3) : base(o1, o2) { Output3 = o3; }
        public virtual bool Equals([AllowNull] InvocationResult<TOut1, TOut2, TOut3> other) => !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] InvocationResult<TOut1, TOut2> other) => Equals(other as InvocationResult<TOut1, TOut2, TOut3>);
        bool IEquatable<IInvocationResult<TOut1, TOut2, TOut3>>.Equals(IInvocationResult<TOut1, TOut2, TOut3> other) => Equals(other as InvocationResult<TOut1, TOut2, TOut3>);
        public override bool Equals(object obj) => Equals(obj as InvocationResult<TOut1, TOut2, TOut3>);
        public override int GetHashCode() => HashCode.Combine(Output1, Output2, Output3, WasInvoked);
        protected bool PropertiesAreEqual([DisallowNull] InvocationResult<TOut1, TOut2, TOut3> other) => base.PropertiesAreEqual(other) && _comparer.Equals(Output3, other.Output3);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Output3)).Append(" = ").Append(Output3);
    }

    public class InvocationResult<TOut1, TOut2, TOut3, TOut4> : InvocationResult<TOut1, TOut2, TOut3>, IInvocationResult<TOut1, TOut2, TOut3, TOut4>,
        IEquatable<InvocationResult<TOut1, TOut2, TOut3, TOut4>>
    {
        private static readonly IEqualityComparer<TOut4> _comparer = EqualityComparer<TOut4>.Default;
        public TOut4 Output4 { get; }
        public InvocationResult() { }
        public InvocationResult(TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4) : base(o1, o2, o3) { Output4 = o4; }
        public virtual bool Equals([AllowNull] InvocationResult<TOut1, TOut2, TOut3, TOut4> other) => !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] InvocationResult<TOut1, TOut2, TOut3> other) => Equals(other as InvocationResult<TOut1, TOut2, TOut3, TOut4>);
        bool IEquatable<IInvocationResult<TOut1, TOut2, TOut3, TOut4>>.Equals(IInvocationResult<TOut1, TOut2, TOut3, TOut4> other) =>
            Equals(other as InvocationResult<TOut1, TOut2, TOut3, TOut4>);
        public override bool Equals(object obj) => Equals(obj as InvocationResult<TOut1, TOut2, TOut3, TOut4>);
        public override int GetHashCode() => HashCode.Combine(Output1, Output2, Output3, Output4, WasInvoked);
        protected bool PropertiesAreEqual([DisallowNull] InvocationResult<TOut1, TOut2, TOut3, TOut4> other) => base.PropertiesAreEqual(other) && _comparer.Equals(Output4, other.Output4);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Output4)).Append(" = ").Append(Output4);
    }

    public class InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5> : InvocationResult<TOut1, TOut2, TOut3, TOut4>, IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5>,
        IEquatable<InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5>>
    {
        private static readonly IEqualityComparer<TOut5> _comparer = EqualityComparer<TOut5>.Default;
        public TOut5 Output5 { get; }
        public InvocationResult() { }
        public InvocationResult(TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5) : base(o1, o2, o3, o4) { Output5 = o5; }
        public virtual bool Equals([AllowNull] InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5> other) => !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] InvocationResult<TOut1, TOut2, TOut3, TOut4> other) => Equals(other as InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5>);
        bool IEquatable<IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5>>.Equals(IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5> other) =>
            Equals(other as InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5>);
        public override bool Equals(object obj) => Equals(obj as InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5>);
        public override int GetHashCode() => HashCode.Combine(Output1, Output2, Output3, Output4, Output5, WasInvoked);
        protected bool PropertiesAreEqual([DisallowNull] InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5> other) => base.PropertiesAreEqual(other) &&
            _comparer.Equals(Output5, other.Output5);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Output5)).Append(" = ").Append(Output5);
    }

    public class InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> : InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5>,
        IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>, IEquatable<InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>
    {
        private static readonly IEqualityComparer<TOut6> _comparer = EqualityComparer<TOut6>.Default;
        public TOut6 Output6 { get; }
        public InvocationResult() { }
        public InvocationResult(TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5, TOut6 o6) : base(o1, o2, o3, o4, o5) { Output6 = o6; }
        public virtual bool Equals([AllowNull] InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> other) =>
            !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5> other) => Equals(other as InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>);
        bool IEquatable<IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>.Equals(IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> other) =>
            Equals(other as InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>);
        public override bool Equals(object obj) => Equals(obj as InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>);
        public override int GetHashCode() => HashCode.Combine(Output1, Output2, Output3, Output4, Output5, Output6, WasInvoked);
        protected bool PropertiesAreEqual([DisallowNull] InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> other) => base.PropertiesAreEqual(other) &&
            _comparer.Equals(Output6, other.Output6);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Output6)).Append(" = ").Append(Output6);
    }

    public class InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> : InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>,
        IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>, IEquatable<InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>
    {
        private static readonly IEqualityComparer<TOut7> _comparer = EqualityComparer<TOut7>.Default;
        public TOut7 Output7 { get; }
        public InvocationResult() { }
        public InvocationResult(TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5, TOut6 o6, TOut7 o7) : base(o1, o2, o3, o4, o5, o6) { Output7 = o7; }
        public virtual bool Equals([AllowNull] InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> other) =>
            !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> other) =>
            Equals(other as InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>);
        bool IEquatable<IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>.Equals(IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> other) =>
            Equals(other as InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>);
        public override bool Equals(object obj) => Equals(obj as InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>);
        public override int GetHashCode() => HashCode.Combine(Output1, Output2, Output3, Output4, Output5, Output6, Output7, WasInvoked);
        protected bool PropertiesAreEqual([DisallowNull] InvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> other) => base.PropertiesAreEqual(other) &&
            _comparer.Equals(Output7, other.Output7);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Output7)).Append(" = ").Append(Output7);
    }
}
