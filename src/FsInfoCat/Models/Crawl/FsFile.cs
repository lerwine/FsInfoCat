using System;
using System.Collections.ObjectModel;
using System.IO;

namespace FsInfoCat.Models.Crawl
{
    public class FsFile : IFsChildNode
    {
        public string Name { get; set; }

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

        public long Length { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime LastWriteTime { get; set; }
        public int Attributes { get; set; }

        public static void Import(FileInfo file, int i, IFsDirectory container)
        {
            string name;
            try
            {
                name = file.Name;
            }
            catch (Exception exc)
            {
                container.Errors.Add(new CrawlError(exc, "Enumerating file #" + (i + 1).ToString()));
                return;
            }
            FsFile item = new FsFile { Name = name };
            try { item.Length = file.Length; }
            catch (Exception exc)
            {
                item.Length = -1L;
                item.Errors.Add(new CrawlError(exc, "Getting file length"));
            }
            try { item.CreationTime = file.CreationTimeUtc; }
            catch (Exception exc) { item.Errors.Add(new CrawlError(exc, "Getting file creation time")); }
            try { item.LastWriteTime = file.LastWriteTimeUtc; }
            catch (Exception exc) { item.Errors.Add(new CrawlError(exc, "Getting file last write time")); }
            try { item.Attributes = (int)file.Attributes; }
            catch (Exception exc) { item.Errors.Add(new CrawlError(exc, "Getting file system attributes")); }
            container.ChildNodes.Add(item);
        }
    }
}
