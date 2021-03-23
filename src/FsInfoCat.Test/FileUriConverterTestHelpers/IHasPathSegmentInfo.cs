namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public interface IHasPathSegmentInfo : ISynchronized
    {
        PathSegmentInfo Path { get; set; }
    }
}
