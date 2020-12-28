using System;

namespace FsInfoCat.Desktop
{
    public class UrlAndID
    {
        public Uri Url { get; }
        public Guid ID { get; }
        public UrlAndID(Uri url, Guid id)
        {
            Url = url;
            ID = id;
        }
    }
}
