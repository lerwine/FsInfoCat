using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace FsInfoCat.Test.Helpers
{
    public class FuncInvocationResult<TResult> : IFuncInvocationResult<TResult>, IEquatable<FuncInvocationResult<TResult>>
    {
        private static readonly IEqualityComparer<TResult> _comparer = EqualityComparer<TResult>.Default;
        public TResult ReturnValue { get; }
        object IFuncTestData.ReturnValue => ReturnValue;
        public bool WasInvoked { get; }
        public FuncInvocationResult() { WasInvoked = false; }
        public FuncInvocationResult(TResult returnValue)
        {
            ReturnValue = returnValue;
            WasInvoked = true;
        }
        public virtual bool Equals([AllowNull] FuncInvocationResult<TResult> other) => !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        bool IEquatable<IInvocationResult>.Equals(IInvocationResult other) => Equals(other as FuncInvocationResult<TResult>);
        bool IEquatable<IFuncInvocationResult<TResult>>.Equals(IFuncInvocationResult<TResult> other) => Equals(other as FuncInvocationResult<TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncInvocationResult<TResult>);
        public override int GetHashCode() => HashCode.Combine(WasInvoked, ReturnValue);
        protected bool PropertiesAreEqual([DisallowNull] FuncInvocationResult<TResult> other) => WasInvoked == other.WasInvoked && _comparer.Equals(ReturnValue, other.ReturnValue);
        public override string ToString() => $"{GetType().Name}{{ {PropertiesToString(new StringBuilder())} }}";
        protected virtual StringBuilder PropertiesToString(StringBuilder sb) => sb.Append(nameof(ReturnValue)).Append(" = ").Append(ReturnValue)
            .Append(", ").Append(nameof(WasInvoked)).Append(" = ").Append(WasInvoked);
    }

    public class FuncInvocationResult<TOut, TResult> : FuncInvocationResult<TResult>, IFuncInvocationResult<TOut, TResult>, IEquatable<FuncInvocationResult<TOut, TResult>>
    {
        private static readonly IEqualityComparer<TOut> _comparer = EqualityComparer<TOut>.Default;
        public TOut Output1 { get; }
        public FuncInvocationResult() { }
        public FuncInvocationResult(TResult returnValue, TOut o) : base(returnValue) { Output1 = o; }
        public virtual bool Equals([AllowNull] FuncInvocationResult<TOut, TResult> other) => !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncInvocationResult<TResult> other) => Equals(other as FuncInvocationResult<TOut, TResult>);
        bool IEquatable<IInvocationResult<TOut>>.Equals(IInvocationResult<TOut> other) => Equals(other as FuncInvocationResult<TOut, TResult>);
        bool IEquatable<IFuncInvocationResult<TOut, TResult>>.Equals(IFuncInvocationResult<TOut, TResult> other) => Equals(other as FuncInvocationResult<TOut, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncInvocationResult<TOut, TResult>);
        public override int GetHashCode() => HashCode.Combine(WasInvoked, Output1, ReturnValue);
        protected bool PropertiesAreEqual([DisallowNull] FuncInvocationResult<TOut, TResult> other) => base.PropertiesAreEqual(other) && _comparer.Equals(Output1, other.Output1);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Output1)).Append(" = ").Append(Output1);
    }

    public class FuncInvocationResult<TOut1, TOut2, TResult> : FuncInvocationResult<TOut1, TResult>, IFuncInvocationResult<TOut1, TOut2, TResult>,
        IEquatable<FuncInvocationResult<TOut1, TOut2, TResult>>
    {
        private static readonly IEqualityComparer<TOut2> _comparer = EqualityComparer<TOut2>.Default;
        public TOut2 Output2 { get; }
        public FuncInvocationResult() { }
        public FuncInvocationResult(TResult returnValue, TOut1 o1, TOut2 o2) : base(returnValue, o1) { Output2 = o2; }
        public virtual bool Equals([AllowNull] FuncInvocationResult<TOut1, TOut2, TResult> other) => !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncInvocationResult<TOut1, TResult> other) => Equals(other as FuncInvocationResult<TOut1, TOut2, TResult>);
        bool IEquatable<IInvocationResult<TOut1, TOut2>>.Equals(IInvocationResult<TOut1, TOut2> other) => Equals(other as FuncInvocationResult<TOut1, TOut2, TResult>);
        bool IEquatable<IFuncInvocationResult<TOut1, TOut2, TResult>>.Equals(IFuncInvocationResult<TOut1, TOut2, TResult> other) =>
            Equals(other as FuncInvocationResult<TOut1, TOut2, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncInvocationResult<TOut1, TOut2, TResult>);
        public override int GetHashCode() => HashCode.Combine(WasInvoked, Output1, Output2, ReturnValue);
        protected bool PropertiesAreEqual([DisallowNull] FuncInvocationResult<TOut1, TOut2, TResult> other) => base.PropertiesAreEqual(other) && _comparer.Equals(Output2, other.Output2);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Output2)).Append(" = ").Append(Output2);
    }

    public class FuncInvocationResult<TOut1, TOut2, TOut3, TResult> : FuncInvocationResult<TOut1, TOut2, TResult>, IFuncInvocationResult<TOut1, TOut2, TOut3, TResult>,
        IEquatable<FuncInvocationResult<TOut1, TOut2, TOut3, TResult>>
    {
        private static readonly IEqualityComparer<TOut3> _comparer = EqualityComparer<TOut3>.Default;
        public TOut3 Output3 { get; }
        public FuncInvocationResult() { }
        public FuncInvocationResult(TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3) : base(returnValue, o1, o2) { Output3 = o3; }
        public virtual bool Equals([AllowNull] FuncInvocationResult<TOut1, TOut2, TOut3, TResult> other) => !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncInvocationResult<TOut1, TOut2, TResult> other) => Equals(other as FuncInvocationResult<TOut1, TOut2, TOut3, TResult>);
        bool IEquatable<IInvocationResult<TOut1, TOut2, TOut3>>.Equals(IInvocationResult<TOut1, TOut2, TOut3> other) => Equals(other as FuncInvocationResult<TOut1, TOut2, TOut3, TResult>);
        bool IEquatable<IFuncInvocationResult<TOut1, TOut2, TOut3, TResult>>.Equals(IFuncInvocationResult<TOut1, TOut2, TOut3, TResult> other) =>
            Equals(other as FuncInvocationResult<TOut1, TOut2, TOut3, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncInvocationResult<TOut1, TOut2, TOut3, TResult>);
        public override int GetHashCode() => HashCode.Combine(WasInvoked, Output1, Output2, Output3, ReturnValue);
        protected bool PropertiesAreEqual([DisallowNull] FuncInvocationResult<TOut1, TOut2, TOut3, TResult> other) => base.PropertiesAreEqual(other) &&
                _comparer.Equals(Output3, other.Output3);
            protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Output3)).Append(" = ").Append(Output3);
    }

    public class FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TResult> : FuncInvocationResult<TOut1, TOut2, TOut3, TResult>, IFuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TResult>,
        IEquatable<FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TResult>>
    {
        private static readonly IEqualityComparer<TOut4> _comparer = EqualityComparer<TOut4>.Default;
        public TOut4 Output4 { get; }
        public FuncInvocationResult() { }
        public FuncInvocationResult(TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4) : base(returnValue, o1, o2, o3) { Output4 = o4; }
        public virtual bool Equals([AllowNull] FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TResult> other) => !(other is null) && (ReferenceEquals(this, other) ||
            PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncInvocationResult<TOut1, TOut2, TOut3, TResult> other) => Equals(other as FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TResult>);
        bool IEquatable<IInvocationResult<TOut1, TOut2, TOut3, TOut4>>.Equals(IInvocationResult<TOut1, TOut2, TOut3, TOut4> other) =>
            Equals(other as FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TResult>);
        bool IEquatable<IFuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TResult>>.Equals(IFuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TResult> other) =>
            Equals(other as FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TResult>);
        public override int GetHashCode() => HashCode.Combine(WasInvoked, Output1, Output2, Output3, Output4, ReturnValue);
        protected bool PropertiesAreEqual([DisallowNull] FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TResult> other) => base.PropertiesAreEqual(other) &&
            _comparer.Equals(Output4, other.Output4);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Output4)).Append(" = ").Append(Output4);
    }

    public class FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> : FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TResult>,
        IFuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>, IEquatable<FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>>
    {
        private static readonly IEqualityComparer<TOut5> _comparer = EqualityComparer<TOut5>.Default;
        public TOut5 Output5 { get; }
        public FuncInvocationResult() { }
        public FuncInvocationResult(TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5) : base(returnValue, o1, o2, o3, o4) { Output5 = o5; }
        public virtual bool Equals([AllowNull] FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> other) => !(other is null) && (ReferenceEquals(this, other) ||
            PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TResult> other) =>
            Equals(other as FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>);
        bool IEquatable<IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5>>.Equals(IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5> other) =>
            Equals(other as FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>);
        bool IEquatable<IFuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>>.Equals(IFuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> other) =>
            Equals(other as FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>);
        public override int GetHashCode() => HashCode.Combine(WasInvoked, Output1, Output2, Output3, Output4, Output5, ReturnValue);
        protected bool PropertiesAreEqual([DisallowNull] FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> other) => base.PropertiesAreEqual(other) &&
            _comparer.Equals(Output5, other.Output5);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Output5)).Append(" = ").Append(Output5);
    }

    public class FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> : FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>,
        IFuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>, IEquatable<FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>>
    {
        private static readonly IEqualityComparer<TOut6> _comparer = EqualityComparer<TOut6>.Default;
        public TOut6 Output6 { get; }
        public FuncInvocationResult() { }
        public FuncInvocationResult(TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5, TOut6 o6) : base(returnValue, o1, o2, o3, o4, o5) { Output6 = o6; }
        public virtual bool Equals([AllowNull] FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> other) => !(other is null) && (ReferenceEquals(this, other) ||
            PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> other) =>
            Equals(other as FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>);
        bool IEquatable<IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6>>.Equals(IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6> other) =>
            Equals(other as FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>);
        bool IEquatable<IFuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>>.Equals(IFuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> other) =>
            Equals(other as FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>);
        public override int GetHashCode() => HashCode.Combine(WasInvoked, Output1, Output2, Output3, Output4, Output5, Output6, ReturnValue);
        protected bool PropertiesAreEqual([DisallowNull] FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> other) => base.PropertiesAreEqual(other) &&
            _comparer.Equals(Output6, other.Output6);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Output6)).Append(" = ").Append(Output6);
    }

    public class FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult> : FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>,
        IFuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult>, IEquatable<FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult>>
    {
        private static readonly IEqualityComparer<TOut7> _comparer = EqualityComparer<TOut7>.Default;
        public TOut7 Output7 { get; }
        public FuncInvocationResult() { }
        public FuncInvocationResult(TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5, TOut6 o6, TOut7 o7) : base(returnValue, o1, o2, o3, o4, o5, o6) { Output7 = o7; }
        public virtual bool Equals([AllowNull] FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult> other) =>
            !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> other) =>
            Equals(other as FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult>);
        bool IEquatable<IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7>>.Equals(IInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7> other) =>
            Equals(other as FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult>);
        bool IEquatable<IFuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult>>.Equals(IFuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult> other) =>
            Equals(other as FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult>);
        public override int GetHashCode() => HashCode.Combine(Output1, Output2, Output3, Output4, Output5, Output6, Output7, HashCode.Combine(WasInvoked, ReturnValue));
        protected bool PropertiesAreEqual([DisallowNull] FuncInvocationResult<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult> other) => base.PropertiesAreEqual(other) &&
            _comparer.Equals(Output7, other.Output7);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Output7)).Append(" = ").Append(Output7);
    }
}
