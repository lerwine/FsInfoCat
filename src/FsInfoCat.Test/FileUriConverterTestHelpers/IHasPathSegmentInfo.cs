namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public interface IHasPathSegmentInfo : IOwnable
    {
        PathSegmentInfo Path { get; set; }
    }
}
