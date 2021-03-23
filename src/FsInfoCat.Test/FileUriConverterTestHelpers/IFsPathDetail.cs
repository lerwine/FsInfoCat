namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public interface IFsPathDetail : IHasPathSegmentInfo
    {
        bool IsAbsolute { get; set; }
        UncHostInfo Host { get; set; }
    }
}
