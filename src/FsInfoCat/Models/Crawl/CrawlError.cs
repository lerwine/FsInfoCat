using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace FsInfoCat.Models.Crawl
{
    public class CrawlError : CrawlMessage
    {
        private string _activity = "";
        private string _category = "";
        private string _reason = "";
        private string _targetName = "";
        private string _targetType = "";
        private string _recommendedAction = "";

        public string Activity
        {
            get => _activity;
            set => _activity = ModelHelper.CoerceAsWsNormalized(value);
        }
        public string Category
        {
            get => _category;
            set => _category = ModelHelper.CoerceAsWsNormalized(value);
        }
        public string Reason
        {
            get => _reason;
            set => _reason = ModelHelper.CoerceAsTrimmed(value);
        }
        public string TargetName
        {
            get => _targetName;
            set => _targetName = ModelHelper.CoerceAsWsNormalized(value);
        }
        public string TargetType
        {
            get => _targetType;
            set => _targetType = ModelHelper.CoerceAsWsNormalized(value);
        }

        public string RecommendedAction
        {
            get => _recommendedAction;
            set => _recommendedAction = ModelHelper.CoerceAsTrimmed(value);
        }

        public Collection<CrawlError> InnerErrors { get; set; }

        CrawlError() { }

        /// <summary>
        /// Create new CrawlError.
        /// </summary>
        /// <param name="exception"><seealso cref="Exception" /> that is the cause of the error.</param>
        /// <exception cref="ArgumentNullException"><paramref name="exception" /> was null.</exception>
        public CrawlError(Exception exception, MessageId id)
            : base((exception is null) ? "" : (string.IsNullOrWhiteSpace(exception.Message)) ? exception.GetType().Name : exception.Message, id)
        {
            if (exception is null)
                throw new ArgumentNullException(nameof(exception));
            List<CrawlError> innerErrors;
            if (exception is AggregateException)
                innerErrors = ((AggregateException)exception).InnerExceptions.Select(e => new CrawlError(e, id)).ToList();
            else
            {
                innerErrors = new List<CrawlError>();
                if (null != exception.InnerException)
                    innerErrors.Add(new CrawlError(exception.InnerException, id));
            }
            InnerErrors = new Collection<CrawlError>(innerErrors);
        }
    }
}
