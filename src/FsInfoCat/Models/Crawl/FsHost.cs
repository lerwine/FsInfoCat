using System.Collections.ObjectModel;

namespace FsInfoCat.Models.Crawl
{
    public sealed class FsHost : IFsNode
    {
        private Collection<FsRoot> _roots = null;
        public Collection<FsRoot> Roots
        {
            get
            {
                Collection<FsRoot> roots = _roots;
                if (null == roots)
                    _roots = roots = new Collection<FsRoot>();
                return roots;
            }
            set { _roots = value; }
        }

        private Collection<CrawlError> _errors = null;
        public Collection<CrawlError> Errors
        {
            get
            {
                Collection<CrawlError> errors = _errors;
                if (null == errors)
                    _errors = errors = new Collection<CrawlError>();
                return errors;
            }
            set { _errors = value; }
        }

        public string MachineName { get; set; }

        public string MachineIdentifier { get; set; }
    }
}
