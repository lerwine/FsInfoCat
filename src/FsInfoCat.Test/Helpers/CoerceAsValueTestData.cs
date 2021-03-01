using FsInfoCat.PS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;

namespace FsInfoCat.Test.Helpers
{
    public static class CoerceAsValueTestData
    {
        internal static IEnumerable<CoerceAsValueTestData<int>> GetCoerceAsIntTestData() => (new CoerceAsValueTestData<int>[] { new CoerceAsValueTestData<int>(null, false, default(int)) })
            .Concat(
                (new int[] { int.MinValue, 0, int.MaxValue }).Select(inputObj => new CoerceAsValueTestData<int>(inputObj, true, inputObj))
                .Concat(
                    (new object[] { ((long)int.MinValue) - 1L, (long)int.MinValue, (long)int.MaxValue, ((long)int.MaxValue) + 1L, true, "12", "", "Test" }).Select(inputObj =>
                    {
                        bool returnValue = LanguagePrimitives.TryConvertTo(inputObj, out int i);
                        return new CoerceAsValueTestData<int>(inputObj, returnValue, (returnValue) ? i : default(int));
                    }
                    )
                )
                .SelectMany(testData => new CoerceAsValueTestData<int>[]
                {
                        testData,
                        new CoerceAsValueTestData<int>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result)
                })
            );

        internal static IEnumerable<CoerceAsValueTestData<Uri>> GetCoerceAsUriTestData() => (new CoerceAsValueTestData<Uri>[] { new CoerceAsValueTestData<Uri>(null, false, null) })
            .Concat(
                (new Uri[] { new Uri("http://tempuri.org", UriKind.Absolute), new Uri("", UriKind.Relative) }).Select(inputObj => new CoerceAsValueTestData<Uri>(inputObj, true, inputObj))
                .Concat(
                    (new object[] { "/test/path", "\\\\myhost\\MyShare", "http://:", DateTime.Parse("2/27/2021 9:00:00 PM"), Guid.Parse("0108bc51-dbe5-4a3d-9fb7-518963fb18c0") }).Select(inputObj =>
                    {
                        bool returnValue = LanguagePrimitives.TryConvertTo(inputObj, out Uri uri);
                        return new CoerceAsValueTestData<Uri>(inputObj, returnValue, (returnValue) ? uri : null);
                    }
                    )
                )
                .SelectMany(testData => new CoerceAsValueTestData<Uri>[]
                {
                        testData,
                        new CoerceAsValueTestData<Uri>(PSObject.AsPSObject(testData.InputObj), testData.ReturnValue, testData.Result)
                })
            );

        internal static bool TryCoerceAsInt(object inputObj, out int result)
        {
            if (inputObj is DateTime dateTime)
            {
                result = dateTime.Year;
                return true;
            }
            result = default;
            return false;
        }

        internal static IEnumerable<CoerceAsValueTestDataF<int>> GetCoerceAsIntTestDataF() => (new CoerceAsValueTestDataF<int>[]
        {
            new CoerceAsValueTestDataF<int>(null, false, default(int), TryCoerceAsInt, new FuncInvocationResult<int, bool>())
        }).Concat(
            GetCoerceAsIntTestData().Where(testData => !(testData.InputObj is null)).Select(testData =>
            {
                if (testData.ReturnValue)
                    return new CoerceAsValueTestDataF<int>(testData, TryCoerceAsInt, new FuncInvocationResult<int, bool>());
                bool returnValue = TryCoerceAsInt(testData.InputObj, out int result);
                return new CoerceAsValueTestDataF<int>(testData.InputObj, returnValue, result, TryCoerceAsInt, new FuncInvocationResult<int, bool>(returnValue, result));
            })
        );

        internal static bool TryCoerceAsUri(object inputObj, out Uri uri)
        {
            if (inputObj is Guid guid)
            {
                uri = new Uri($"urn:uuid:{guid.ToString("D").ToLower()}");
                return true;
            }
            uri = null;
            return false;
        }

        internal static IEnumerable<CoerceAsValueTestDataF<Uri>> GetCoerceAsUriTestDataF() => (new CoerceAsValueTestDataF<Uri>[]
        {
            new CoerceAsValueTestDataF<Uri>(null, false, null, TryCoerceAsUri, new FuncInvocationResult<Uri, bool>())
        }).Concat(
            GetCoerceAsUriTestData().Where(testData => !(testData.InputObj is null)).Select(testData =>
            {
                if (testData.ReturnValue)
                    return new CoerceAsValueTestDataF<Uri>(testData, TryCoerceAsUri, new FuncInvocationResult<Uri, bool>());
                bool returnValue = TryCoerceAsUri(testData.InputObj, out Uri result);
                return new CoerceAsValueTestDataF<Uri>(testData.InputObj, returnValue, result, TryCoerceAsUri, new FuncInvocationResult<Uri, bool>(returnValue, result));
            })
        );

        internal static IEnumerable<CoerceAsValueTestDataR<int>> GetCoerceAsIntTestDataR() => GetCoerceAsIntTestData().SelectMany(testData => new CoerceAsValueTestDataR<int>[]
        {
            new CoerceAsValueTestDataR<int>(testData, false),
            new CoerceAsValueTestDataR<int>(testData, true)
        });

        internal static IEnumerable<CoerceAsValueTestDataR<Uri>> GetCoerceAsUriTestDataR() => GetCoerceAsUriTestData().SelectMany(testData => new CoerceAsValueTestDataR<Uri>[]
        {
            new CoerceAsValueTestDataR<Uri>(testData, false),
            new CoerceAsValueTestDataR<Uri>(testData, true)
        });

        internal static IEnumerable<CoerceAsValueTestDataFR<int>> GetCoerceAsIntTestDataFR() => GetCoerceAsIntTestDataF().SelectMany(testData => new CoerceAsValueTestDataFR<int>[]
        {
            new CoerceAsValueTestDataFR<int>(testData, false),
            new CoerceAsValueTestDataFR<int>(testData, true)
        });

        internal static IEnumerable<CoerceAsValueTestDataFR<Uri>> GetCoerceAsUriTestDataFR() => GetCoerceAsUriTestDataF().SelectMany(testData => new CoerceAsValueTestDataFR<Uri>[]
        {
            new CoerceAsValueTestDataFR<Uri>(testData, false),
            new CoerceAsValueTestDataFR<Uri>(testData, true)
        });

        internal const int CoerceAsIntNullValue = 7;

        internal static IEnumerable<CoerceAsValueTestDataN<int>> GetCoerceAsIntTestDataN() => GetCoerceAsIntTestData().Select(testData => new CoerceAsValueTestDataN<int>(testData, () => CoerceAsIntNullValue));

        internal static readonly Uri CoerceAsUriNullValue = new Uri("https://www.erwinefamily.net", UriKind.Absolute);

        internal static IEnumerable<CoerceAsValueTestDataN<Uri>> GetCoerceAsUriTestDataN() => GetCoerceAsUriTestData().Select(testData => new CoerceAsValueTestDataN<Uri>(testData, () => CoerceAsUriNullValue));

        internal static IEnumerable<CoerceAsValueTestDataFN<int>> GetCoerceAsIntTestDataFN() => GetCoerceAsIntTestDataF().Select(testData => new CoerceAsValueTestDataFN<int>(testData, () => CoerceAsIntNullValue));

        internal static IEnumerable<CoerceAsValueTestDataFN<Uri>> GetCoerceAsUriTestDataFN() => GetCoerceAsUriTestDataF().Select(testData => new CoerceAsValueTestDataFN<Uri>(testData, () => CoerceAsUriNullValue));

        internal static IEnumerable<CoerceAsValueTestDataNR<int>> GetCoerceAsIntTestDataNR() => GetCoerceAsIntTestDataN().SelectMany(testData => new CoerceAsValueTestDataNR<int>[]
        {
            new CoerceAsValueTestDataNR<int>(testData, false),
            new CoerceAsValueTestDataNR<int>(testData, true)
        });

        internal static IEnumerable<CoerceAsValueTestDataNR<Uri>> GetCoerceAsUriTestDataNR() => GetCoerceAsUriTestDataN().SelectMany(testData => new CoerceAsValueTestDataNR<Uri>[]
        {
            new CoerceAsValueTestDataNR<Uri>(testData, false),
            new CoerceAsValueTestDataNR<Uri>(testData, true)
        });

        internal static IEnumerable<CoerceAsValueTestDataFNR<int>> GetCoerceAsIntTestDataFNR() => GetCoerceAsIntTestDataFN().SelectMany(testData => new CoerceAsValueTestDataFNR<int>[]
        {
            new CoerceAsValueTestDataFNR<int>(testData, false),
            new CoerceAsValueTestDataFNR<int>(testData, true)
        });

        internal static IEnumerable<CoerceAsValueTestDataFNR<Uri>> GetCoerceAsUriTestDataFNR() => GetCoerceAsUriTestDataFN().SelectMany(testData => new CoerceAsValueTestDataFNR<Uri>[]
        {
            new CoerceAsValueTestDataFNR<Uri>(testData, false),
            new CoerceAsValueTestDataFNR<Uri>(testData, true)
        });

    }

    public class CoerceAsValueTestData<TResult>
    {
        public CoerceAsValueTestData(object inputObj, bool returnValue, TResult result)
        {
            InputObj = inputObj;
            ReturnValue = returnValue;
            IfSuccessResult = (returnValue) ? new InvocationResult<TResult>(result) : new InvocationResult<TResult>();
            Result = result;
        }
        public CoerceAsValueTestData(CoerceAsValueTestData<TResult> source)
        {
            InputObj = source.InputObj;
            ReturnValue = source.ReturnValue;
            IfSuccessResult = (source.ReturnValue) ? new InvocationResult<TResult>(source.Result) : new InvocationResult<TResult>();
            Result = source.Result;
        }
        public object InputObj { get; }
        public bool ReturnValue { get; }
        public InvocationResult<TResult> IfSuccessResult { get; }
        public TResult Result { get; }
    }

    public class CoerceAsValueTestDataF<TResult> : CoerceAsValueTestData<TResult>
    {
        public CoerceAsValueTestDataF(CoerceAsValueTestDataF<TResult> source)
            : base(source)
        {
            FallbackHandler = source.FallbackHandler;
            FallbackResult = source.FallbackResult;
        }

        public CoerceAsValueTestDataF(CoerceAsValueTestData<TResult> source, TryCoerceHandler<object, TResult> fallbackHandler, IFuncInvocationResult<TResult, bool> fallbackResult)
            : base(source)
        {
            FallbackHandler = fallbackHandler ?? throw new ArgumentNullException(nameof(fallbackHandler));
            FallbackResult = fallbackResult ?? throw new ArgumentNullException(nameof(fallbackResult));
        }

        public CoerceAsValueTestDataF(object inputObj, bool returnValue, TResult result, TryCoerceHandler<object, TResult> fallbackHandler, IFuncInvocationResult<TResult, bool> fallbackResult)
            : base(inputObj, returnValue, result)
        {
            FallbackHandler = fallbackHandler ?? throw new ArgumentNullException(nameof(fallbackHandler));
            FallbackResult = fallbackResult ?? throw new ArgumentNullException(nameof(fallbackResult));
        }

        public TryCoerceHandler<object, TResult> FallbackHandler { get; }
        public IFuncInvocationResult<TResult, bool> FallbackResult { get; }
    }

    public class CoerceAsValueTestDataR<TResult> : CoerceAsValueTestData<TResult>
    {
        public CoerceAsValueTestDataR(CoerceAsValueTestDataR<TResult> source)
            : base(source)
        {
            ReturnValueIfNull = source.ReturnValueIfNull;
        }

        public CoerceAsValueTestDataR(CoerceAsValueTestData<TResult> source, bool returnValueIfNull)
            : base(source.InputObj, (source.InputObj is null) ? returnValueIfNull : source.ReturnValue, source.Result)
        {
            ReturnValueIfNull = returnValueIfNull;
        }

        public CoerceAsValueTestDataR(object inputObj, bool returnValue, TResult result, bool returnValueIfNull)
            : base(inputObj, (result is null) ? returnValueIfNull : returnValue, result)
        {
            ReturnValueIfNull = returnValueIfNull;
        }

        public bool ReturnValueIfNull { get; }
    }

    public class CoerceAsValueTestDataFR<TResult> : CoerceAsValueTestDataF<TResult>
    {
        public CoerceAsValueTestDataFR(CoerceAsValueTestDataFR<TResult> source)
            : base(source)
        {
            ReturnValueIfNull = source.ReturnValueIfNull;
        }

        public CoerceAsValueTestDataFR(CoerceAsValueTestDataF<TResult> source, bool returnValueIfNull)
            : base(source.InputObj, (source.InputObj is null) ? returnValueIfNull : source.ReturnValue, source.Result, source.FallbackHandler, source.FallbackResult)
        {
            ReturnValueIfNull = returnValueIfNull;
        }

        public CoerceAsValueTestDataFR(object inputObj, bool returnValue, TResult result, TryCoerceHandler<object, TResult> fallbackHandler, IFuncInvocationResult<TResult, bool> fallbackResult, bool returnValueIfNull)
            : base(inputObj, (result is null) ? returnValueIfNull : returnValue, result, fallbackHandler, fallbackResult)
        {
            ReturnValueIfNull = returnValueIfNull;
        }

        public bool ReturnValueIfNull { get; }
    }

    public class CoerceAsValueTestDataN<TResult> : CoerceAsValueTestData<TResult>
    {
        public CoerceAsValueTestDataN(CoerceAsValueTestDataN<TResult> source)
            : base(source)
        {
            IfNullCallback = source.IfNullCallback;
            IfNullResult = source.IfNullResult;
        }

        public CoerceAsValueTestDataN(CoerceAsValueTestData<TResult> source, Func<TResult> ifNullCallback)
            : base(source.InputObj, source.ReturnValue, (source.InputObj is null) ? ifNullCallback() : source.Result)
        {
            IfNullCallback = ifNullCallback;
            IfNullResult = (source.InputObj is null) ? new FuncInvocationResult<TResult>(ifNullCallback()) : new FuncInvocationResult<TResult>();
        }

        public CoerceAsValueTestDataN(object inputObj, bool returnValue, TResult result, Func<TResult> ifNullCallback)
            : base(inputObj, returnValue, (inputObj is null) ? ifNullCallback() : result)
        {
            IfNullCallback = ifNullCallback;
            IfNullResult = (inputObj is null) ? new FuncInvocationResult<TResult>(ifNullCallback()) : new FuncInvocationResult<TResult>();
        }

        public Func<TResult> IfNullCallback { get; }

        public IFuncInvocationResult<TResult> IfNullResult { get; }
    }

    public class CoerceAsValueTestDataFN<TResult> : CoerceAsValueTestDataF<TResult>
    {
        public CoerceAsValueTestDataFN(CoerceAsValueTestDataFN<TResult> source)
            : base(source)
        {
            IfNullCallback = source.IfNullCallback;
            IfNullResult = source.IfNullResult;
        }

        public CoerceAsValueTestDataFN(CoerceAsValueTestDataF<TResult> source, Func<TResult> ifNullCallback)
            : base(source.InputObj, source.ReturnValue, (source.InputObj is null) ? ifNullCallback() : source.Result, source.FallbackHandler, source.FallbackResult)
        {
            IfNullCallback = ifNullCallback;
            IfNullResult = (source.InputObj is null) ? new FuncInvocationResult<TResult>(ifNullCallback()) : new FuncInvocationResult<TResult>();
        }

        public CoerceAsValueTestDataFN(object inputObj, bool returnValue, TResult result, TryCoerceHandler<object, TResult> fallbackHandler, IFuncInvocationResult<TResult, bool> fallbackResult, Func<TResult> ifNullCallback)
            : base(inputObj, returnValue, (inputObj is null) ? ifNullCallback() : result, fallbackHandler, fallbackResult)
        {
            IfNullCallback = ifNullCallback;
            IfNullResult = (inputObj is null) ? new FuncInvocationResult<TResult>(ifNullCallback()) : new FuncInvocationResult<TResult>();
        }

        public Func<TResult> IfNullCallback { get; }

        public IFuncInvocationResult<TResult> IfNullResult { get; }
    }

    public class CoerceAsValueTestDataNR<TResult> : CoerceAsValueTestDataN<TResult>
    {
        public CoerceAsValueTestDataNR(CoerceAsValueTestDataNR<TResult> source)
            : base(source)
        {
            ReturnValueIfNull = source.ReturnValueIfNull;
        }

        public CoerceAsValueTestDataNR(CoerceAsValueTestDataN<TResult> source, bool returnValueIfNull)
            : base(source.InputObj, (source.InputObj is null) ? returnValueIfNull : source.ReturnValue, source.Result, source.IfNullCallback)
        {
            ReturnValueIfNull = returnValueIfNull;
        }

        public CoerceAsValueTestDataNR(object inputObj, bool returnValue, TResult result, Func<TResult> ifNullCallback, bool returnValueIfNull)
            : base(inputObj, (result is null) ? returnValueIfNull : returnValue, result, ifNullCallback)
        {
            ReturnValueIfNull = returnValueIfNull;
        }

        public bool ReturnValueIfNull { get; }
    }

    public class CoerceAsValueTestDataFNR<TResult> : CoerceAsValueTestDataFN<TResult>
    {
        public CoerceAsValueTestDataFNR(CoerceAsValueTestDataFNR<TResult> source)
            : base(source)
        {
            ReturnValueIfNull = source.ReturnValueIfNull;
        }

        public CoerceAsValueTestDataFNR(CoerceAsValueTestDataFN<TResult> source, bool returnValueIfNull)
            : base(source.InputObj, (source.InputObj is null) ? returnValueIfNull : source.ReturnValue, source.Result, source.FallbackHandler, source.FallbackResult, source.IfNullCallback)
        {
            ReturnValueIfNull = returnValueIfNull;
        }

        public CoerceAsValueTestDataFNR(object inputObj, bool returnValue, TResult result, TryCoerceHandler<object, TResult> fallbackHandler, IFuncInvocationResult<TResult, bool> fallbackResult,
            Func<TResult> ifNullCallback, bool returnValueIfNull)
            : base(inputObj, (inputObj is null) ? returnValueIfNull : returnValue, result, fallbackHandler, fallbackResult, ifNullCallback)
        {
            ReturnValueIfNull = returnValueIfNull;
        }

        public bool ReturnValueIfNull { get; }
    }
}
