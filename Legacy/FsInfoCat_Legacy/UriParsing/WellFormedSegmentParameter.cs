using System;

namespace FsInfoCat.UriParsing
{
    public class WellFormedSegmentParameter : IUriParameterElement
    {
        private WellFormedSegmentParameter(string key, string value)
        {
            Key = key;
            Value = value;
        }

        public string Key { get; }

        public string Value { get; }

        bool IUriComponent.IsWellFormed => true;

        public static bool TryParse(string source, out SegmentParameterComponentList<WellFormedSegmentParameter> result)
        {
            // TODO: Implement TryParse(string, out SegmentParameterComponentList<WellFormedSegmentParameter>)
            throw new NotImplementedException();
        }
    }
}
