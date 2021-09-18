using FsInfoCat.Desktop.ViewModel;
using System.Windows;

namespace FsInfoCat.Desktop.LocalData.CrawlConfigurations
{
    public class ReportItemFilter : ViewModel.Filter.CrawlConfigFilter
    {
        #region DisplayText Property Members

        /// <summary>
        /// Identifies the <see cref="DisplayText"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DisplayTextProperty = DependencyPropertyBuilder<ReportItemFilter, string>
            .Register(nameof(DisplayText))
            .DefaultValue("")
            .CoerseWith(NonWhiteSpaceOrEmptyStringCoersion.Default)
            .AsReadWrite();

        public string DisplayText { get => GetValue(DisplayTextProperty) as string; set => SetValue(DisplayTextProperty, value); }

        #endregion
    }
}
