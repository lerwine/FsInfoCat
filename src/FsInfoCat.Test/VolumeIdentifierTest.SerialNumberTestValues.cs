using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FsInfoCat.Test
{
    public partial class VolumeIdentifierTest
    {
        class SerialNumberTestValues
        {
            private readonly ReadOnlyCollection<SerialNumberTestValues> _caseVariants;
            private readonly ReadOnlyCollection<SerialNumberTestValues> _dashCaseVariants;
            private readonly SerialNumberTestValues _dashVariant;
            internal uint SerialNumber { get; }
            internal string StringParam { get; }
            internal string StringValue { get; }
            internal string Description { get; }
            internal string UrnParam { get; }
            internal Uri Url { get; }
            protected SerialNumberTestValues(uint serialNumber, string description)
            {
                SerialNumber = serialNumber;
                StringParam = $"{(serialNumber >> 16).ToString("x4")}-{(serialNumber & 0xFFFFU).ToString("x4")}";
                StringValue = $"volume:id:{StringParam}";
                Description = description;
                Url = new Uri($"urn:{StringValue}", UriKind.Absolute);
                UrnParam = Url.AbsoluteUri;
                Collection<SerialNumberTestValues> caseVariants = new Collection<SerialNumberTestValues>();
                Collection<SerialNumberTestValues> dashCaseVariants = new Collection<SerialNumberTestValues>();
                _caseVariants = new ReadOnlyCollection<SerialNumberTestValues>(caseVariants);
                _dashCaseVariants = new ReadOnlyCollection<SerialNumberTestValues>(dashCaseVariants);
                _dashVariant = new SerialNumberTestValues(this, serialNumber.ToString("x8"), $"{description} w/o dash",
                    $"urn:volume:id:{serialNumber.ToString("x8")}");
                caseVariants.Add(this);
                dashCaseVariants.Add(this);
                dashCaseVariants.Add(_dashVariant);
                if (StringParam.ToUpper() != StringParam)
                {
                    caseVariants.Add(new SerialNumberTestValues(this, StringParam.ToUpper(),
                        $"Upper case {Description.Substring(0, 1).ToLower()}{Description.Substring(1)}",
                        $"urn:VOLUME:ID:{StringParam.ToUpper()}"));
                    caseVariants.Add(new SerialNumberTestValues(this, _dashVariant.StringParam.ToUpper(),
                        $"Upper case {_dashVariant.Description.Substring(0, 1).ToLower()}{_dashVariant.Description.Substring(1)}",
                        $"urn:VOLUME:ID:{_dashVariant.StringParam.ToUpper()}"));
                }
            }
            protected SerialNumberTestValues(SerialNumberTestValues dashVariant, string stringParam, string description, string urn)
            {
                _dashVariant = dashVariant;
                _caseVariants = dashVariant._caseVariants;
                _dashCaseVariants = dashVariant._dashCaseVariants;
                SerialNumber = dashVariant.SerialNumber;
                StringParam = stringParam;
                StringValue = dashVariant.StringValue;
                Description = description;
                Url = dashVariant.Url;
                UrnParam = urn;
            }

            private static IEnumerable<SerialNumberTestValues> _GetTestValues()
            {
                yield return new SerialNumberTestValues(0x3B518D4BU, "Value of 995,200,331");
                yield return new SerialNumberTestValues(0x9E497DE8U, "Bit-wise equivalent to signed integer -1,639,350,808");
                yield return new SerialNumberTestValues(0U, "Zero value");
                yield return new SerialNumberTestValues(1U, "Value of 1");
                yield return new SerialNumberTestValues(0x80000000U, "Bit-wise equivalent to Int32.MinValue");
                yield return new SerialNumberTestValues(0x7FFFFFFFU, "Bit-wise equivalent to Int32.MaxValue");
                yield return new SerialNumberTestValues(0xFFFFFFFFU, "Bit-wise equivalent to signed integer -1");
            }

            public static IEnumerable<SerialNumberTestValues> GetTestValues(bool includeCaseVariants = false, bool includeDashVariants = false)
            {
                if (includeCaseVariants)
                {
                    if (includeDashVariants)
                        return _GetTestValues().SelectMany(v => v._dashCaseVariants);
                    return _GetTestValues().SelectMany(v => v._caseVariants);
                }
                if (includeDashVariants)
                    return _GetTestValues().SelectMany(v => new SerialNumberTestValues[] { v, v._dashVariant });
                return _GetTestValues();
            }
        }
    }
}
