namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public interface IRelativeUrl : IHasPathSegmentInfo
    {
        string Query { get; set; }
        string Fragment { get; set; }
        FsPathInfo LocalPath { get; set; }
    }
}
