using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace FsInfoCat.Test.Helpers
{
    public class FuncTestData<TResult> : IFuncTestData<TResult>, IEquatable<FuncTestData<TResult>>
    {
        private static readonly IEqualityComparer<TResult> _comparer = EqualityComparer<TResult>.Default;
        public TResult ReturnValue { get; }
        object IFuncTestData.ReturnValue { get; }
        public FuncTestData(TResult returnValue) { ReturnValue = returnValue; }
        public static FuncTestData<TResult> FromInvocation(Func<TResult> func) => new FuncTestData<TResult>(func());
        protected bool PropertiesAreEqual([NotNull] FuncTestData<TResult> other) => _comparer.Equals(ReturnValue, other.ReturnValue);
        public virtual bool Equals([AllowNull] FuncTestData<TResult> other) => !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals(object obj) => Equals(obj as FuncTestData<TResult>);
        public override int GetHashCode() => _comparer.GetHashCode(ReturnValue);
        public override string ToString() => $"{GetType().Name}{{ {PropertiesToString(new StringBuilder())} }}";
        protected virtual StringBuilder PropertiesToString(StringBuilder sb) => sb.Append(nameof(ReturnValue)).Append(" = ").Append(ReturnValue);
    }

    public class FuncTestData<TParam, TResult> : FuncTestData<TResult>, IFuncTestData<TParam, TResult>, IEquatable<FuncTestData<TParam, TResult>>
    {
        private static readonly IEqualityComparer<TParam> _comparer = EqualityComparer<TParam>.Default;
        public TParam Input1 { get; }
        public FuncTestData(TParam arg1, TResult returnValue) : base(returnValue) { Input1 = arg1; }
        public static FuncTestData<TParam, TResult> FromInvocation(Func<TParam, TResult> func, TParam arg1) => new FuncTestData<TParam, TResult>(arg1, func(arg1));
        protected bool PropertiesAreEqual([NotNull] FuncTestData<TParam, TResult> other) => base.PropertiesAreEqual(other) && _comparer.Equals(Input1, other.Input1);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Input1);
        public virtual bool Equals([AllowNull] FuncTestData<TParam, TResult> other) => !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData<TResult> other) => Equals(other as FuncTestData<TParam, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData<TParam, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input1)).Append(" = ").Append(Input1);
    }

    public class FuncTestData<TParam1, TParam2, TResult> : FuncTestData<TParam1, TResult>, IFuncTestData<TParam1, TParam2, TResult>, IEquatable<FuncTestData<TParam1, TParam2, TResult>>
    {
        private static readonly IEqualityComparer<TParam2> _comparer = EqualityComparer<TParam2>.Default;
        public TParam2 Input2 { get; }
        public FuncTestData(TParam1 arg1, TParam2 arg2, TResult returnValue) : base(arg1, returnValue) { Input2 = arg2; }
        public static FuncTestData<TParam1, TParam2, TResult> FromInvocation(Func<TParam1, TParam2, TResult> func, TParam1 arg1, TParam2 arg2)
            => new FuncTestData<TParam1, TParam2, TResult>(arg1, arg2, func(arg1, arg2));
        protected bool PropertiesAreEqual([NotNull] FuncTestData<TParam1, TParam2, TResult> other) => base.PropertiesAreEqual(other) && _comparer.Equals(Input2, other.Input2);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Input1, Input2);
        public virtual bool Equals([AllowNull] FuncTestData<TParam1, TParam2, TResult> other) => !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData<TParam1, TResult> other) => Equals(other as FuncTestData<TParam1, TParam2, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData<TParam1, TParam2, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input2)).Append(" = ").Append(Input2);
    }

    public class FuncTestData<TParam1, TParam2, TParam3, TResult> : FuncTestData<TParam1, TParam2, TResult>, IFuncTestData<TParam1, TParam2, TParam3, TResult>,
        IEquatable<FuncTestData<TParam1, TParam2, TParam3, TResult>>
    {
        private static readonly IEqualityComparer<TParam3> _comparer = EqualityComparer<TParam3>.Default;
        public TParam3 Input3 { get; }
        public FuncTestData(TParam1 arg1, TParam2 arg2, TParam3 arg3, TResult returnValue) : base(arg1, arg2, returnValue) { Input3 = arg3; }
        public static FuncTestData<TParam1, TParam2, TParam3, TResult> FromInvocation(Func<TParam1, TParam2, TParam3, TResult> func, TParam1 arg1, TParam2 arg2, TParam3 arg3)
            => new FuncTestData<TParam1, TParam2, TParam3, TResult>(arg1, arg2, arg3, func(arg1, arg2, arg3));
        protected bool PropertiesAreEqual([NotNull] FuncTestData<TParam1, TParam2, TParam3, TResult> other) => base.PropertiesAreEqual(other) && _comparer.Equals(Input3, other.Input3);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Input1, Input2, Input3);
        public virtual bool Equals([AllowNull] FuncTestData<TParam1, TParam2, TParam3, TResult> other) => !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData<TParam1, TParam2, TResult> other) => Equals(other as FuncTestData<TParam1, TParam2, TParam3, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData<TParam1, TParam2, TParam3, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input3)).Append(" = ").Append(Input3);
    }

    public class FuncTestData<TParam1, TParam2, TParam3, TParam4, TResult> : FuncTestData<TParam1, TParam2, TParam3, TResult>,
        IFuncTestData<TParam1, TParam2, TParam3, TParam4, TResult>, IEquatable<FuncTestData<TParam1, TParam2, TParam3, TParam4, TResult>>
    {
        private static readonly IEqualityComparer<TParam4> _comparer = EqualityComparer<TParam4>.Default;
        public TParam4 Input4 { get; }
        public FuncTestData(TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TResult returnValue) : base(arg1, arg2, arg3, returnValue) { Input4 = arg4; }
        public static FuncTestData<TParam1, TParam2, TParam3, TParam4, TResult> FromInvocation(Func<TParam1, TParam2, TParam3, TParam4, TResult> func, TParam1 arg1, TParam2 arg2,
            TParam3 arg3, TParam4 arg4) => new FuncTestData<TParam1, TParam2, TParam3, TParam4, TResult>(arg1, arg2, arg3, arg4, func(arg1, arg2, arg3, arg4));
        protected bool PropertiesAreEqual([NotNull] FuncTestData<TParam1, TParam2, TParam3, TParam4, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Input4, other.Input4);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Input1, Input2, Input3, Input4);
        public virtual bool Equals([AllowNull] FuncTestData<TParam1, TParam2, TParam3, TParam4, TResult> other) => !(other is null) &&
            (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData<TParam1, TParam2, TParam3, TResult> other) => Equals(other as FuncTestData<TParam1, TParam2, TParam3, TParam4, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData<TParam1, TParam2, TParam3, TParam4, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input4)).Append(" = ").Append(Input4);
    }

    public class FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TResult> : FuncTestData<TParam1, TParam2, TParam3, TParam4, TResult>,
        IFuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TResult>, IEquatable<FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TResult>>
    {
        private static readonly IEqualityComparer<TParam5> _comparer = EqualityComparer<TParam5>.Default;
        public TParam5 Input5 { get; }
        public FuncTestData(TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TParam5 arg5, TResult returnValue) : base(arg1, arg2, arg3, arg4, returnValue)
        {
            Input5 = arg5;
        }
        public static FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TResult> FromInvocation(Func<TParam1, TParam2, TParam3, TParam4, TParam5, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TParam5 arg5)
            => new FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TResult>(arg1, arg2, arg3, arg4, arg5, func(arg1, arg2, arg3, arg4, arg5));
        protected bool PropertiesAreEqual([NotNull] FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Input5, other.Input5);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Input1, Input2, Input3, Input4, Input5);
        public virtual bool Equals([AllowNull] FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TResult> other) => !(other is null) &&
            (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData<TParam1, TParam2, TParam3, TParam4, TResult> other) =>
            Equals(other as FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input5)).Append(" = ").Append(Input5);
    }

    public class FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult> : FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TResult>,
        IFuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult>, IEquatable<FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult>>
    {
        private static readonly IEqualityComparer<TParam6> _comparer = EqualityComparer<TParam6>.Default;
        public TParam6 Input6 { get; }
        public FuncTestData(TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TParam5 arg5, TParam6 arg6, TResult returnValue) : base(arg1, arg2, arg3, arg4, arg5, returnValue)
        {
            Input6 = arg6;
        }
        public static FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult> FromInvocation(Func<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TParam5 arg5, TParam6 arg6)
            => new FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult>(arg1, arg2, arg3, arg4, arg5, arg6, func(arg1, arg2, arg3, arg4, arg5, arg6));
        protected bool PropertiesAreEqual([NotNull] FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Input6, other.Input6);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Input1, Input2, Input3, Input4, Input5, Input6);
        public virtual bool Equals([AllowNull] FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult> other) => !(other is null) &&
            (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TResult> other) =>
            Equals(other as FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input6)).Append(" = ").Append(Input6);
    }

    public class FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TResult> :
        FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult>, IFuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TResult>,
        IEquatable<FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TResult>>
    {
        private static readonly IEqualityComparer<TParam7> _comparer = EqualityComparer<TParam7>.Default;
        public TParam7 Input7 { get; }
        public FuncTestData(TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TParam5 arg5, TParam6 arg6, TParam7 arg7, TResult returnValue)
            : base(arg1, arg2, arg3, arg4, arg5, arg6, returnValue)
        {
            Input7 = arg7;
        }
        public static FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TResult> FromInvocation(Func<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TParam5 arg5, TParam6 arg6, TParam7 arg7)
            => new FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TResult>(arg1, arg2, arg3, arg4, arg5, arg6, arg7, func(arg1, arg2, arg3, arg4, arg5, arg6,
                arg7));
        protected bool PropertiesAreEqual([NotNull] FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Input7, other.Input7);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Input1, Input2, Input3, Input4, Input5, Input6, Input7);
        public virtual bool Equals([AllowNull] FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TResult> other) => !(other is null) &&
            (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TResult> other) =>
            Equals(other as FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TParam7, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input7)).Append(" = ").Append(Input7);
    }

    public class FuncTestData1<TOut, TResult> : FuncTestData<TResult>, IFuncTestData1<TOut, TResult>, IEquatable<FuncTestData1<TOut, TResult>>
    {
        private static readonly IEqualityComparer<TOut> _comparer = EqualityComparer<TOut>.Default;
        public TOut Output1 { get; }
        public FuncTestData1(TResult returnValue, TOut o) : base(returnValue) { Output1 = o; }
        public static FuncTestData1<TOut, TResult> FromInvocation(FuncWithOutput<TOut, TResult> func) => new FuncTestData1<TOut, TResult>(func(out TOut o), o);
        protected bool PropertiesAreEqual([NotNull] FuncTestData1<TOut, TResult> other) => base.PropertiesAreEqual(other) && _comparer.Equals(Output1, other.Output1);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1);
        public virtual bool Equals([AllowNull] FuncTestData1<TOut, TResult> other) => !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData<TResult> other) => Equals(other as FuncTestData1<TOut, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData1<TOut, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Output1)).Append(" = ").Append(Output1);
    }

    public class FuncTestData1<TParam, TOut, TResult> : FuncTestData1<TOut, TResult>, IFuncTestData1<TParam, TOut, TResult>, IEquatable<FuncTestData1<TParam, TOut, TResult>>
    {
        private static readonly IEqualityComparer<TParam> _comparer = EqualityComparer<TParam>.Default;
        public TParam Input1 { get; }
        public FuncTestData1(TParam arg1, TResult returnValue, TOut o) : base(returnValue, o) { Input1 = arg1; }
        public static FuncTestData1<TParam, TOut, TResult> FromInvocation(FuncWithOutput<TParam, TOut, TResult> func, TParam arg1)
            => new FuncTestData1<TParam, TOut, TResult>(arg1, func(arg1, out TOut o), o);
        protected bool PropertiesAreEqual([NotNull] FuncTestData1<TParam, TOut, TResult> other) => base.PropertiesAreEqual(other) && _comparer.Equals(Input1, other.Input1);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Input1);
        public virtual bool Equals([AllowNull] FuncTestData1<TParam, TOut, TResult> other) => !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData1<TOut, TResult> other) => Equals(other as FuncTestData1<TParam, TOut, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData1<TParam, TOut, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input1)).Append(" = ").Append(Input1);
    }

    public class FuncTestData1<TParam1, TParam2, TOut, TResult> : FuncTestData1<TParam1, TOut, TResult>, IFuncTestData1<TParam1, TParam2, TOut, TResult>,
        IEquatable<FuncTestData1<TParam1, TParam2, TOut, TResult>>
    {
        private static readonly IEqualityComparer<TParam2> _comparer = EqualityComparer<TParam2>.Default;
        public TParam2 Input2 { get; }
        public FuncTestData1(TParam1 arg1, TParam2 arg2, TResult returnValue, TOut o) : base(arg1, returnValue, o) { Input2 = arg2; }
        public static FuncTestData1<TParam1, TParam2, TOut, TResult> FromInvocation(FuncWithOutput<TParam1, TParam2, TOut, TResult> func, TParam1 arg1, TParam2 arg2)
            => new FuncTestData1<TParam1, TParam2, TOut, TResult>(arg1, arg2, func(arg1, arg2, out TOut o), o);
        protected bool PropertiesAreEqual([NotNull] FuncTestData1<TParam1, TParam2, TOut, TResult> other) => base.PropertiesAreEqual(other) && _comparer.Equals(Input2, other.Input2);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Input1, Input2);
        public virtual bool Equals([AllowNull] FuncTestData1<TParam1, TParam2, TOut, TResult> other) => !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData1<TParam1, TOut, TResult> other) => Equals(other as FuncTestData1<TParam1, TParam2, TOut, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData1<TParam1, TParam2, TOut, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input2)).Append(" = ").Append(Input2);
    }

    public class FuncTestData1<TParam1, TParam2, TParam3, TOut, TResult> : FuncTestData1<TParam1, TParam2, TOut, TResult>, IFuncTestData1<TParam1, TParam2, TParam3, TOut, TResult>,
        IEquatable<FuncTestData1<TParam1, TParam2, TParam3, TOut, TResult>>
    {
        private static readonly IEqualityComparer<TParam3> _comparer = EqualityComparer<TParam3>.Default;
        public TParam3 Input3 { get; }
        public FuncTestData1(TParam1 arg1, TParam2 arg2, TParam3 arg3, TResult returnValue, TOut o) : base(arg1, arg2, returnValue, o) { Input3 = arg3; }
        public static FuncTestData1<TParam1, TParam2, TParam3, TOut, TResult> FromInvocation(FuncWithOutput<TParam1, TParam2, TParam3, TOut, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3)
            => new FuncTestData1<TParam1, TParam2, TParam3, TOut, TResult>(arg1, arg2, arg3, func(arg1, arg2, arg3, out TOut o), o);
        protected bool PropertiesAreEqual([NotNull] FuncTestData1<TParam1, TParam2, TParam3, TOut, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Input3, other.Input3);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Input1, Input2, Input3);
        public virtual bool Equals([AllowNull] FuncTestData1<TParam1, TParam2, TParam3, TOut, TResult> other) => !(other is null) &&
            (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData1<TParam1, TParam2, TOut, TResult> other) => Equals(other as FuncTestData1<TParam1, TParam2, TParam3, TOut, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData1<TParam1, TParam2, TParam3, TOut, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input3)).Append(" = ").Append(Input3);
    }

    public class FuncTestData1<TParam1, TParam2, TParam3, TParam4, TOut, TResult> : FuncTestData1<TParam1, TParam2, TParam3, TOut, TResult>,
        IFuncTestData1<TParam1, TParam2, TParam3, TParam4, TOut, TResult>, IEquatable<FuncTestData1<TParam1, TParam2, TParam3, TParam4, TOut, TResult>>
    {
        private static readonly IEqualityComparer<TParam4> _comparer = EqualityComparer<TParam4>.Default;
        public TParam4 Input4 { get; }
        public FuncTestData1(TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TResult returnValue, TOut o) : base(arg1, arg2, arg3, returnValue, o) { Input4 = arg4; }
        public static FuncTestData1<TParam1, TParam2, TParam3, TParam4, TOut, TResult> FromInvocation(FuncWithOutput<TParam1, TParam2, TParam3, TParam4, TOut, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4)
            => new FuncTestData1<TParam1, TParam2, TParam3, TParam4, TOut, TResult>(arg1, arg2, arg3, arg4, func(arg1, arg2, arg3, arg4, out TOut o), o);
        protected bool PropertiesAreEqual([NotNull] FuncTestData1<TParam1, TParam2, TParam3, TParam4, TOut, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Input4, other.Input4);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Input1, Input2, Input3, Input4);
        public virtual bool Equals([AllowNull] FuncTestData1<TParam1, TParam2, TParam3, TParam4, TOut, TResult> other) => !(other is null) &&
            (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData1<TParam1, TParam2, TParam3, TOut, TResult> other) =>
            Equals(other as FuncTestData1<TParam1, TParam2, TParam3, TParam4, TOut, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData1<TParam1, TParam2, TParam3, TParam4, TOut, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input4)).Append(" = ").Append(Input4);
    }

    public class FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult> : FuncTestData1<TParam1, TParam2, TParam3, TParam4, TOut, TResult>,
        IFuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult>, IEquatable<FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult>>
    {
        private static readonly IEqualityComparer<TParam5> _comparer = EqualityComparer<TParam5>.Default;
        public TParam5 Input5 { get; }
        public FuncTestData1(TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TParam5 arg5, TResult returnValue, TOut o) : base(arg1, arg2, arg3, arg4, returnValue, o)
        {
            Input5 = arg5;
        }
        public static FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult> FromInvocation(FuncWithOutput<TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TParam5 arg5)
            => new FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult>(arg1, arg2, arg3, arg4, arg5, func(arg1, arg2, arg3, arg4, arg5, out TOut o), o);
        protected bool PropertiesAreEqual([NotNull] FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Input5, other.Input5);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Input1, Input2, Input3, Input4, Input5);
        public virtual bool Equals([AllowNull] FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult> other) => !(other is null) &&
            (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData1<TParam1, TParam2, TParam3, TParam4, TOut, TResult> other) =>
            Equals(other as FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input5)).Append(" = ").Append(Input5);
    }

    public class FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut, TResult> : FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult>,
        IFuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut, TResult>, IEquatable<FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut, TResult>>
    {
        private static readonly IEqualityComparer<TParam6> _comparer = EqualityComparer<TParam6>.Default;
        public TParam6 Input6 { get; }
        public FuncTestData1(TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TParam5 arg5, TParam6 arg6, TResult returnValue, TOut o)
            : base(arg1, arg2, arg3, arg4, arg5, returnValue, o)
        {
            Input6 = arg6;
        }
        public static FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut, TResult> FromInvocation(FuncWithOutput<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TParam5 arg5, TParam6 arg6)
            => new FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut, TResult>(arg1, arg2, arg3, arg4, arg5, arg6, func(arg1, arg2, arg3, arg4, arg5, arg6,
                out TOut o), o);
        protected bool PropertiesAreEqual([NotNull] FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Input6, other.Input6);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Input1, Input2, Input3, Input4, Input5, Input6);
        public virtual bool Equals([AllowNull] FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut, TResult> other) => !(other is null) &&
            (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TOut, TResult> other) =>
            Equals(other as FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData1<TParam1, TParam2, TParam3, TParam4, TParam5, TParam6, TOut, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input6)).Append(" = ").Append(Input6);
    }

    public class FuncTestData2<TOut1, TOut2, TResult> : FuncTestData1<TOut1, TResult>, IFuncTestData2<TOut1, TOut2, TResult>, IEquatable<FuncTestData2<TOut1, TOut2, TResult>>
    {
        private static readonly IEqualityComparer<TOut2> _comparer = EqualityComparer<TOut2>.Default;
        public TOut2 Output2 { get; }
        public FuncTestData2(TResult returnValue, TOut1 o1, TOut2 o2) : base(returnValue, o1) { Output2 = o2; }
        public static FuncTestData2<TOut1, TOut2, TResult> FromInvocation(FuncWithOutput2<TOut1, TOut2, TResult> func) =>
            new FuncTestData2<TOut1, TOut2, TResult>(func(out TOut1 o1, out TOut2 o2), o1, o2);
        protected bool PropertiesAreEqual([NotNull] FuncTestData2<TOut1, TOut2, TResult> other) => base.PropertiesAreEqual(other) && _comparer.Equals(Output2, other.Output2);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Output2);
        public virtual bool Equals([AllowNull] FuncTestData2<TOut1, TOut2, TResult> other) => !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData1<TOut1, TResult> other) => Equals(other as FuncTestData2<TOut1, TOut2, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData2<TOut1, TOut2, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Output2)).Append(" = ").Append(Output2);
    }

    public class FuncTestData2<TParam, TOut1, TOut2, TResult> : FuncTestData2<TOut1, TOut2, TResult>, IFuncTestData2<TParam, TOut1, TOut2, TResult>,
        IEquatable<FuncTestData2<TParam, TOut1, TOut2, TResult>>
    {
        private static readonly IEqualityComparer<TParam> _comparer = EqualityComparer<TParam>.Default;
        public TParam Input1 { get; }
        public FuncTestData2(TParam arg1, TResult returnValue, TOut1 o1, TOut2 o2) : base(returnValue, o1, o2) { Input1 = arg1; }
        public static FuncTestData2<TParam, TOut1, TOut2, TResult> FromInvocation(FuncWithOutput2<TParam, TOut1, TOut2, TResult> func, TParam arg1)
            => new FuncTestData2<TParam, TOut1, TOut2, TResult>(arg1, func(arg1, out TOut1 o1, out TOut2 o2), o1, o2);
        protected bool PropertiesAreEqual([NotNull] FuncTestData2<TParam, TOut1, TOut2, TResult> other) => base.PropertiesAreEqual(other) && _comparer.Equals(Input1, other.Input1);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Output2, Input1);
        public virtual bool Equals([AllowNull] FuncTestData2<TParam, TOut1, TOut2, TResult> other) => !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData2<TOut1, TOut2, TResult> other) => Equals(other as FuncTestData2<TParam, TOut1, TOut2, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData2<TParam, TOut1, TOut2, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input1)).Append(" = ").Append(Input1);
    }

    public class FuncTestData2<TParam1, TParam2, TOut1, TOut2, TResult> : FuncTestData2<TParam1, TOut1, TOut2, TResult>, IFuncTestData2<TParam1, TParam2, TOut1, TOut2, TResult>,
        IEquatable<FuncTestData2<TParam1, TParam2, TOut1, TOut2, TResult>>
    {
        private static readonly IEqualityComparer<TParam2> _comparer = EqualityComparer<TParam2>.Default;
        public TParam2 Input2 { get; }
        public FuncTestData2(TParam1 arg1, TParam2 arg2, TResult returnValue, TOut1 o1, TOut2 o2) : base(arg1, returnValue, o1, o2) { Input2 = arg2; }
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Output2, Input1, Input2);
        public static FuncTestData2<TParam1, TParam2, TOut1, TOut2, TResult> FromInvocation(FuncWithOutput2<TParam1, TParam2, TOut1, TOut2, TResult> func, TParam1 arg1, TParam2 arg2)
            => new FuncTestData2<TParam1, TParam2, TOut1, TOut2, TResult>(arg1, arg2, func(arg1, arg2, out TOut1 o1, out TOut2 o2), o1, o2);
        protected bool PropertiesAreEqual([NotNull] FuncTestData2<TParam1, TParam2, TOut1, TOut2, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Input2, other.Input2);
        public virtual bool Equals([AllowNull] FuncTestData2<TParam1, TParam2, TOut1, TOut2, TResult> other) => !(other is null) &&
            (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData2<TParam1, TOut1, TOut2, TResult> other) => Equals(other as FuncTestData2<TParam1, TParam2, TOut1, TOut2, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData2<TParam1, TParam2, TOut1, TOut2, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input2)).Append(" = ").Append(Input2);
    }

    public class FuncTestData2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult> : FuncTestData2<TParam1, TParam2, TOut1, TOut2, TResult>,
        IFuncTestData2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult>, IEquatable<FuncTestData2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult>>
    {
        private static readonly IEqualityComparer<TParam3> _comparer = EqualityComparer<TParam3>.Default;
        public TParam3 Input3 { get; }
        public FuncTestData2(TParam1 arg1, TParam2 arg2, TParam3 arg3, TResult returnValue, TOut1 o1, TOut2 o2) : base(arg1, arg2, returnValue, o1, o2) { Input3 = arg3; }
        public static FuncTestData2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult> FromInvocation(FuncWithOutput2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3)
            => new FuncTestData2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult>(arg1, arg2, arg3, func(arg1, arg2, arg3, out TOut1 o1, out TOut2 o2), o1, o2);
        protected bool PropertiesAreEqual([NotNull] FuncTestData2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Input3, other.Input3);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Output2, Input1, Input2, Input3);
        public virtual bool Equals([AllowNull] FuncTestData2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult> other) => !(other is null) &&
            (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData2<TParam1, TParam2, TOut1, TOut2, TResult> other) =>
            Equals(other as FuncTestData2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input3)).Append(" = ").Append(Input3);
    }

    public class FuncTestData2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult> : FuncTestData2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult>,
        IFuncTestData2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult>, IEquatable<FuncTestData2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult>>
    {
        private static readonly IEqualityComparer<TParam4> _comparer = EqualityComparer<TParam4>.Default;
        public TParam4 Input4 { get; }
        public FuncTestData2(TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TResult returnValue, TOut1 o1, TOut2 o2) : base(arg1, arg2, arg3, returnValue, o1, o2)
        {
            Input4 = arg4;
        }
        public static FuncTestData2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult> FromInvocation(FuncWithOutput2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4)
            => new FuncTestData2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult>(arg1, arg2, arg3, arg4, func(arg1, arg2, arg3, arg4,
                out TOut1 o1, out TOut2 o2), o1, o2);
        protected bool PropertiesAreEqual([NotNull] FuncTestData2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Input4, other.Input4);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Output2, Input1, Input2, Input3, Input4);
        public virtual bool Equals([AllowNull] FuncTestData2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult> other) => !(other is null) &&
            (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData2<TParam1, TParam2, TParam3, TOut1, TOut2, TResult> other) =>
            Equals(other as FuncTestData2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input4)).Append(" = ").Append(Input4);
    }

    public class FuncTestData2<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2, TResult> :
        FuncTestData2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult>, IFuncTestData2<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2, TResult>,
        IEquatable<FuncTestData2<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2, TResult>>
    {
        private static readonly IEqualityComparer<TParam5> _comparer = EqualityComparer<TParam5>.Default;
        public TParam5 Input5 { get; }
        public FuncTestData2(TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TParam5 arg5, TResult returnValue, TOut1 o1, TOut2 o2)
            : base(arg1, arg2, arg3, arg4, returnValue, o1, o2)
        {
            Input5 = arg5;
        }
        public static FuncTestData2<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2, TResult> FromInvocation(FuncWithOutput2<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TParam5 arg5)
            => new FuncTestData2<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2, TResult>(arg1, arg2, arg3, arg4, arg5, func(arg1, arg2, arg3, arg4, arg5,
                out TOut1 o1, out TOut2 o2), o1, o2);
        protected bool PropertiesAreEqual([NotNull] FuncTestData2<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Input5, other.Input5);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Output2, Input1, Input2, Input3, Input4, Input5);
        public virtual bool Equals([AllowNull] FuncTestData2<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2, TResult> other) => !(other is null) &&
            (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData2<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TResult> other) =>
            Equals(other as FuncTestData2<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData2<TParam1, TParam2, TParam3, TParam4, TParam5, TOut1, TOut2, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input5)).Append(" = ").Append(Input5);
    }

    public class FuncTestData3<TOut1, TOut2, TOut3, TResult> : FuncTestData2<TOut1, TOut2, TResult>, IFuncTestData3<TOut1, TOut2, TOut3, TResult>,
        IEquatable<FuncTestData3<TOut1, TOut2, TOut3, TResult>>
    {
        private static readonly IEqualityComparer<TOut3> _comparer = EqualityComparer<TOut3>.Default;
        public TOut3 Output3 { get; }
        public FuncTestData3(TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3) : base(returnValue, o1, o2) { Output3 = o3; }
        public static FuncTestData3<TOut1, TOut2, TOut3, TResult> FromInvocation(FuncWithOutput3<TOut1, TOut2, TOut3, TResult> func)
            => new FuncTestData3<TOut1, TOut2, TOut3, TResult>(func(out TOut1 o1, out TOut2 o2, out TOut3 o3), o1, o2, o3);
        protected bool PropertiesAreEqual([NotNull] FuncTestData3<TOut1, TOut2, TOut3, TResult> other) => base.PropertiesAreEqual(other) && _comparer.Equals(Output3, other.Output3);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Output2, Output3);
        public virtual bool Equals([AllowNull] FuncTestData3<TOut1, TOut2, TOut3, TResult> other) => !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData2<TOut1, TOut2, TResult> other) => Equals(other as FuncTestData3<TOut1, TOut2, TOut3, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData3<TOut1, TOut2, TOut3, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Output3)).Append(" = ").Append(Output3);
    }

    public class FuncTestData3<TParam, TOut1, TOut2, TOut3, TResult> : FuncTestData3<TOut1, TOut2, TOut3, TResult>, IFuncTestData3<TParam, TOut1, TOut2, TOut3, TResult>,
        IEquatable<FuncTestData3<TParam, TOut1, TOut2, TOut3, TResult>>
    {
        private static readonly IEqualityComparer<TParam> _comparer = EqualityComparer<TParam>.Default;
        public TParam Input1 { get; }
        public FuncTestData3(TParam arg1, TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3) : base(returnValue, o1, o2, o3) { Input1 = arg1; }
        public static FuncTestData3<TParam, TOut1, TOut2, TOut3, TResult> FromInvocation(FuncWithOutput3<TParam, TOut1, TOut2, TOut3, TResult> func, TParam arg1)
            => new FuncTestData3<TParam, TOut1, TOut2, TOut3, TResult>(arg1, func(arg1, out TOut1 o1, out TOut2 o2, out TOut3 o3), o1, o2, o3);
        protected bool PropertiesAreEqual([NotNull] FuncTestData3<TParam, TOut1, TOut2, TOut3, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Input1, other.Input1);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Output2, Output3, Input1);
        public virtual bool Equals([AllowNull] FuncTestData3<TParam, TOut1, TOut2, TOut3, TResult> other) => !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData3<TOut1, TOut2, TOut3, TResult> other) => Equals(other as FuncTestData3<TParam, TOut1, TOut2, TOut3, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData3<TParam, TOut1, TOut2, TOut3, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input1)).Append(" = ").Append(Input1);
    }

    public class FuncTestData3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult> : FuncTestData3<TParam1, TOut1, TOut2, TOut3, TResult>,
        IFuncTestData3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult>, IEquatable<FuncTestData3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult>>
    {
        private static readonly IEqualityComparer<TParam2> _comparer = EqualityComparer<TParam2>.Default;
        public TParam2 Input2 { get; }
        public FuncTestData3(TParam1 arg1, TParam2 arg2, TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3) : base(arg1, returnValue, o1, o2, o3) { Input2 = arg2; }
        public static FuncTestData3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult> FromInvocation(FuncWithOutput3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult> func,
                TParam1 arg1, TParam2 arg2)
            => new FuncTestData3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult>(arg1, arg2, func(arg1, arg2, out TOut1 o1, out TOut2 o2, out TOut3 o3), o1, o2, o3);
        protected bool PropertiesAreEqual([NotNull] FuncTestData3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Input2, other.Input2);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Output2, Output3, Input1, Input2);
        public virtual bool Equals([AllowNull] FuncTestData3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult> other) => !(other is null) &&
            (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData3<TParam1, TOut1, TOut2, TOut3, TResult> other) =>
            Equals(other as FuncTestData3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input2)).Append(" = ").Append(Input2);
    }

    public class FuncTestData3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult> : FuncTestData3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult>,
        IFuncTestData3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult>, IEquatable<FuncTestData3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult>>
    {
        private static readonly IEqualityComparer<TParam3> _comparer = EqualityComparer<TParam3>.Default;
        public TParam3 Input3 { get; }
        public FuncTestData3(TParam1 arg1, TParam2 arg2, TParam3 arg3, TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3) : base(arg1, arg2, returnValue, o1, o2, o3)
        {
            Input3 = arg3;
        }
        public static FuncTestData3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult> FromInvocation(FuncWithOutput3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3)
            => new FuncTestData3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult>(arg1, arg2, arg3, func(arg1, arg2, arg3, out TOut1 o1, out TOut2 o2, out TOut3 o3), o1, o2, o3);
        protected bool PropertiesAreEqual([NotNull] FuncTestData3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Input3, other.Input3);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Output2, Output3, Input1, Input2, Input3);
        public virtual bool Equals([AllowNull] FuncTestData3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult> other) => !(other is null) &&
            (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData3<TParam1, TParam2, TOut1, TOut2, TOut3, TResult> other) =>
            Equals(other as FuncTestData3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input3)).Append(" = ").Append(Input3);
    }

    public class FuncTestData3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3, TResult> :
        FuncTestData3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult>, IFuncTestData3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3, TResult>,
        IEquatable<FuncTestData3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3, TResult>>
    {
        private static readonly IEqualityComparer<TParam4> _comparer = EqualityComparer<TParam4>.Default;
        public TParam4 Input4 { get; }
        public FuncTestData3(TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4, TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3)
            : base(arg1, arg2, arg3, returnValue, o1, o2, o3)
        {
            Input4 = arg4;
        }
        public static FuncTestData3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3, TResult> FromInvocation(FuncWithOutput3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3, TParam4 arg4)
            => new FuncTestData3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3, TResult>(arg1, arg2, arg3, arg4, func(arg1, arg2, arg3, arg4, out TOut1 o1, out TOut2 o2,
                out TOut3 o3), o1, o2, o3);
        protected bool PropertiesAreEqual([NotNull] FuncTestData3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Input4, other.Input4);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Output2, Output3, Input1, Input2, Input3, Input4);
        public virtual bool Equals([AllowNull] FuncTestData3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3, TResult> other) => !(other is null) &&
            (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData3<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TResult> other) =>
            Equals(other as FuncTestData3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData3<TParam1, TParam2, TParam3, TParam4, TOut1, TOut2, TOut3, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input4)).Append(" = ").Append(Input4);
    }

    public class FuncTestData4<TOut1, TOut2, TOut3, TOut4, TResult> : FuncTestData3<TOut1, TOut2, TOut3, TResult>, IFuncTestData4<TOut1, TOut2, TOut3, TOut4, TResult>,
        IEquatable<FuncTestData4<TOut1, TOut2, TOut3, TOut4, TResult>>
    {
        private static readonly IEqualityComparer<TOut4> _comparer = EqualityComparer<TOut4>.Default;
        public TOut4 Output4 { get; }
        public FuncTestData4(TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4) : base(returnValue, o1, o2, o3) { Output4 = o4; }
        public static FuncTestData4<TOut1, TOut2, TOut3, TOut4, TResult> FromInvocation(FuncWithOutput4<TOut1, TOut2, TOut3, TOut4, TResult> func)
            => new FuncTestData4<TOut1, TOut2, TOut3, TOut4, TResult>(func(out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4), o1, o2, o3, o4);
        protected bool PropertiesAreEqual([NotNull] FuncTestData4<TOut1, TOut2, TOut3, TOut4, TResult> other) => base.PropertiesAreEqual(other) && _comparer.Equals(Output4, other.Output4);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Output2, Output3, Output4);
        public virtual bool Equals([AllowNull] FuncTestData4<TOut1, TOut2, TOut3, TOut4, TResult> other) => !(other is null) && (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData3<TOut1, TOut2, TOut3, TResult> other) => Equals(other as FuncTestData4<TOut1, TOut2, TOut3, TOut4, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData4<TOut1, TOut2, TOut3, TOut4, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Output4)).Append(" = ").Append(Output4);
    }

    public class FuncTestData4<TParam, TOut1, TOut2, TOut3, TOut4, TResult> : FuncTestData4<TOut1, TOut2, TOut3, TOut4, TResult>,
        IFuncTestData4<TParam, TOut1, TOut2, TOut3, TOut4, TResult>, IEquatable<FuncTestData4<TParam, TOut1, TOut2, TOut3, TOut4, TResult>>
    {
        private static readonly IEqualityComparer<TParam> _comparer = EqualityComparer<TParam>.Default;
        public TParam Input1 { get; }
        public FuncTestData4(TParam arg1, TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4) : base(returnValue, o1, o2, o3, o4)
        {
            Input1 = arg1;
        }
        public static FuncTestData4<TParam, TOut1, TOut2, TOut3, TOut4, TResult> FromInvocation(FuncWithOutput4<TParam, TOut1, TOut2, TOut3, TOut4, TResult> func, TParam arg1)
            => new FuncTestData4<TParam, TOut1, TOut2, TOut3, TOut4, TResult>(arg1, func(arg1, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4),
                o1, o2, o3, o4);
        protected bool PropertiesAreEqual([NotNull] FuncTestData4<TParam, TOut1, TOut2, TOut3, TOut4, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Input1, other.Input1);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Output2, Output3, Output4, Input1);
        public virtual bool Equals([AllowNull] FuncTestData4<TParam, TOut1, TOut2, TOut3, TOut4, TResult> other) => !(other is null) &&
            (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData4<TOut1, TOut2, TOut3, TOut4, TResult> other) => Equals(other as FuncTestData4<TParam, TOut1, TOut2, TOut3, TOut4, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData4<TParam, TOut1, TOut2, TOut3, TOut4, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input1)).Append(" = ").Append(Input1);
    }

    public class FuncTestData4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult> : FuncTestData4<TParam1, TOut1, TOut2, TOut3, TOut4, TResult>,
        IFuncTestData4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult>, IEquatable<FuncTestData4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult>>
    {
        private static readonly IEqualityComparer<TParam2> _comparer = EqualityComparer<TParam2>.Default;
        public TParam2 Input2 { get; }
        public FuncTestData4(TParam1 arg1, TParam2 arg2, TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4) : base(arg1, returnValue, o1, o2, o3, o4)
        {
            Input2 = arg2;
        }
        public static FuncTestData4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult> FromInvocation(FuncWithOutput4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult> func,
                TParam1 arg1, TParam2 arg2)
            => new FuncTestData4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult>(arg1, arg2, func(arg1, arg2, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4), o1, o2, o3, o4);
        protected bool PropertiesAreEqual([NotNull] FuncTestData4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Input2, other.Input2);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Output2, Output3, Output4, Input1, Input2);
        public virtual bool Equals([AllowNull] FuncTestData4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult> other) => !(other is null) &&
            (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData4<TParam1, TOut1, TOut2, TOut3, TOut4, TResult> other) =>
            Equals(other as FuncTestData4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input2)).Append(" = ").Append(Input2);
    }

    public class FuncTestData4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4, TResult> : FuncTestData4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult>,
        IFuncTestData4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4, TResult>, IEquatable<FuncTestData4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4, TResult>>
    {
        private static readonly IEqualityComparer<TParam3> _comparer = EqualityComparer<TParam3>.Default;
        public TParam3 Input3 { get; }
        public FuncTestData4(TParam1 arg1, TParam2 arg2, TParam3 arg3, TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4) : base(arg1, arg2, returnValue, o1, o2, o3, o4)
        {
            Input3 = arg3;
        }
        public static FuncTestData4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4, TResult> FromInvocation(FuncWithOutput4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4, TResult> func,
                TParam1 arg1, TParam2 arg2, TParam3 arg3)
            => new FuncTestData4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4, TResult>(arg1, arg2, arg3, func(arg1, arg2, arg3, out TOut1 o1, out TOut2 o2, out TOut3 o3,
                out TOut4 o4), o1, o2, o3, o4);
        protected bool PropertiesAreEqual([NotNull] FuncTestData4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Input3, other.Input3);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Output2, Output3, Output4, Input1, Input2, Input3);
        public virtual bool Equals([AllowNull] FuncTestData4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4, TResult> other) => !(other is null) &&
            (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData4<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TResult> other) =>
            Equals(other as FuncTestData4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData4<TParam1, TParam2, TParam3, TOut1, TOut2, TOut3, TOut4, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input3)).Append(" = ").Append(Input3);
    }

    public class FuncTestData5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> : FuncTestData4<TOut1, TOut2, TOut3, TOut4, TResult>,
        IFuncTestData5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>, IEquatable<FuncTestData5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>>
    {
        private static readonly IEqualityComparer<TOut5> _comparer = EqualityComparer<TOut5>.Default;
        public TOut5 Output5 { get; }
        public FuncTestData5(TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5) : base(returnValue, o1, o2, o3, o4) { Output5 = o5; }
        public static FuncTestData5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> FromInvocation(FuncWithOutput5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> func)
            => new FuncTestData5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>(func(out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5),
                o1, o2, o3, o4, o5);
        protected bool PropertiesAreEqual([NotNull] FuncTestData5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Output5, other.Output5);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Output2, Output3, Output4, Output5);
        public virtual bool Equals([AllowNull] FuncTestData5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> other) => !(other is null) &&
            (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData4<TOut1, TOut2, TOut3, TOut4, TResult> other) => Equals(other as FuncTestData5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Output5)).Append(" = ").Append(Output5);
    }

    public class FuncTestData5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TResult> : FuncTestData5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>,
        IFuncTestData5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TResult>, IEquatable<FuncTestData5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TResult>>
    {
        private static readonly IEqualityComparer<TParam> _comparer = EqualityComparer<TParam>.Default;
        public TParam Input1 { get; }
        public FuncTestData5(TParam arg1, TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5) : base(returnValue, o1, o2, o3, o4, o5)
        {
            Input1 = arg1;
        }
        public static FuncTestData5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TResult> FromInvocation(FuncWithOutput5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TResult> func, TParam arg1)
            => new FuncTestData5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TResult>(arg1, func(arg1, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5),
                o1, o2, o3, o4, o5);
        protected bool PropertiesAreEqual([NotNull] FuncTestData5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Input1, other.Input1);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Output2, Output3, Output4, Output5, Input1);
        public virtual bool Equals([AllowNull] FuncTestData5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TResult> other) => !(other is null) &&
            (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> other) =>
            Equals(other as FuncTestData5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData5<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input1)).Append(" = ").Append(Input1);
    }

    public class FuncTestData5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5, TResult> : FuncTestData5<TParam1, TOut1, TOut2, TOut3, TOut4, TOut5, TResult>,
        IFuncTestData5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5, TResult>, IEquatable<FuncTestData5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5, TResult>>
    {
        private static readonly IEqualityComparer<TParam2> _comparer = EqualityComparer<TParam2>.Default;
        public TParam2 Input2 { get; }
        public FuncTestData5(TParam1 arg1, TParam2 arg2, TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5) : base(arg1, returnValue, o1, o2, o3, o4, o5)
        {
            Input2 = arg2;
        }
        public static FuncTestData5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5, TResult> FromInvocation(FuncWithOutput5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5, TResult> func,
                TParam1 arg1, TParam2 arg2)
            => new FuncTestData5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5, TResult>(arg1, arg2, func(arg1, arg2, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4,
                out TOut5 o5), o1, o2, o3, o4, o5);
        protected bool PropertiesAreEqual([NotNull] FuncTestData5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Input2, other.Input2);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Output2, Output3, Output4, Output5, Input1, Input2);
        public virtual bool Equals([AllowNull] FuncTestData5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5, TResult> other) => !(other is null) &&
            (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData5<TParam1, TOut1, TOut2, TOut3, TOut4, TOut5, TResult> other) =>
            Equals(other as FuncTestData5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData5<TParam1, TParam2, TOut1, TOut2, TOut3, TOut4, TOut5, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input2)).Append(" = ").Append(Input2);
    }

    public class FuncTestData6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> : FuncTestData5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult>,
        IFuncTestData6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>, IEquatable<FuncTestData6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>>
    {
        private static readonly IEqualityComparer<TOut6> _comparer = EqualityComparer<TOut6>.Default;
        public TOut6 Output6 { get; }
        public FuncTestData6(TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5, TOut6 o6)
            : base(returnValue, o1, o2, o3, o4, o5)
        {
            Output6 = o6;
        }
        public static FuncTestData6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> FromInvocation(FuncWithOutput6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> func)
            => new FuncTestData6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>(func(out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5, out TOut6 o6),
                o1, o2, o3, o4, o5, o6);
        protected bool PropertiesAreEqual([NotNull] FuncTestData6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Output6, other.Output6);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Output2, Output3, Output4, Output5, Output6);
        public virtual bool Equals([AllowNull] FuncTestData6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> other) => !(other is null) &&
            (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData5<TOut1, TOut2, TOut3, TOut4, TOut5, TResult> other) =>
            Equals(other as FuncTestData6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Output6)).Append(" = ").Append(Output6);
    }

    public class FuncTestData6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> : FuncTestData6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>,
        IFuncTestData6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>, IEquatable<FuncTestData6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>>
    {
        private static readonly IEqualityComparer<TParam> _comparer = EqualityComparer<TParam>.Default;
        public TParam Input1 { get; }
        public FuncTestData6(TParam arg1, TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5, TOut6 o6) : base(returnValue, o1, o2, o3, o4, o5, o6)
        {
            Input1 = arg1;
        }
        public static FuncTestData6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> FromInvocation(FuncWithOutput6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> func, TParam p)
            => new FuncTestData6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>(p, func(p, out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5, out TOut6 o6),
                o1, o2, o3, o4, o5, o6);
        protected bool PropertiesAreEqual([NotNull] FuncTestData6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Input1, other.Input1);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Output2, Output3, Output4, Output5, Output6, Input1);
        public virtual bool Equals([AllowNull] FuncTestData6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> other) => !(other is null) &&
            (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> other) =>
            Equals(other as FuncTestData6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData6<TParam, TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Input1)).Append(" = ").Append(Input1);
    }

    public class FuncTestData7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult> : FuncTestData6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult>,
        IFuncTestData7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult>, IEquatable<FuncTestData7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult>>
    {
        private static readonly IEqualityComparer<TOut7> _comparer = EqualityComparer<TOut7>.Default;
        public TOut7 Output7 { get; }
        public FuncTestData7(TResult returnValue, TOut1 o1, TOut2 o2, TOut3 o3, TOut4 o4, TOut5 o5, TOut6 o6, TOut7 o7) : base(returnValue, o1, o2, o3, o4, o5, o6)
        {
            Output7 = o7;
        }
        public static FuncTestData7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult> FromInvocation(FuncWithOutput7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult> func)
            => new FuncTestData7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult>(func(out TOut1 o1, out TOut2 o2, out TOut3 o3, out TOut4 o4, out TOut5 o5, out TOut6 o6,
                out TOut7 o7), o1, o2, o3, o4, o5, o6, o7);
        protected bool PropertiesAreEqual([NotNull] FuncTestData7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult> other) =>
            base.PropertiesAreEqual(other) && _comparer.Equals(Output7, other.Output7);
        public override int GetHashCode() => HashCode.Combine(ReturnValue, Output1, Output2, Output3, Output4, Output5, Output6, Output7);
        public virtual bool Equals([AllowNull] FuncTestData7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult> other) => !(other is null) &&
            (ReferenceEquals(this, other) || PropertiesAreEqual(other));
        public override bool Equals([AllowNull] FuncTestData6<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TResult> other) =>
            Equals(other as FuncTestData7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult>);
        public override bool Equals(object obj) => Equals(obj as FuncTestData7<TOut1, TOut2, TOut3, TOut4, TOut5, TOut6, TOut7, TResult>);
        protected override StringBuilder PropertiesToString(StringBuilder sb) => base.PropertiesToString(sb).Append(", ").Append(nameof(Output7)).Append(" = ").Append(Output7);
    }
}
