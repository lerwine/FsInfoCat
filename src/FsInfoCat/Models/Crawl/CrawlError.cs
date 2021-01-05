using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FsInfoCat.Models.Crawl
{
    public class CrawlError
    {
        public string Message { get; set; }

        public string Activity { get; set; }

        public Collection<CrawlError> InnerErrors { get; set; }

        public CrawlError(Exception exception, string activity)
        {
            Activity = (null == activity) ? "" : activity;
            Message = (string.IsNullOrWhiteSpace(exception.Message)) ? exception.GetType().Name : exception.Message;
            List<CrawlError> innerErrors;
            if (exception is AggregateException)
                innerErrors = ((AggregateException)exception).InnerExceptions.Select(e => new CrawlError(e)).ToList();
            else
            {
                innerErrors = new List<CrawlError>();
                if (null != exception.InnerException)
                    innerErrors.Add(new CrawlError(exception.InnerException));
            }
            InnerErrors = new Collection<CrawlError>(innerErrors);
        }
        public CrawlError(Exception exception) : this(exception, null) { }
    }
}
