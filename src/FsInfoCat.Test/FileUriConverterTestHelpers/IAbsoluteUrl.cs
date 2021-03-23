namespace FsInfoCat.Test.FileUriConverterTestHelpers
{
    public interface IAbsoluteUrl : IRelativeUrl
    {
        bool IsWellFormed { get; set; }
        UriAuthority Authority { get; set; }
    }
}
