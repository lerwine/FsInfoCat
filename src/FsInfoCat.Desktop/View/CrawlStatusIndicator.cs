using FsInfoCat.Desktop.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FsInfoCat.Desktop.View
{
    /// <summary>
    /// Follow steps 1a or 1b and then 2 to use this custom control in a XAML file.
    ///
    /// Step 1a) Using this custom control in a XAML file that exists in the current project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:FsInfoCat.Desktop.View"
    ///
    ///
    /// Step 1b) Using this custom control in a XAML file that exists in a different project.
    /// Add this XmlNamespace attribute to the root element of the markup file where it is
    /// to be used:
    ///
    ///     xmlns:MyNamespace="clr-namespace:FsInfoCat.Desktop.View;assembly=FsInfoCat.Desktop.View"
    ///
    /// You will also need to add a project reference from the project where the XAML file lives
    /// to this project and Rebuild to avoid compilation errors:
    ///
    ///     Right click on the target project in the Solution Explorer and
    ///     "Add Reference"->"Projects"->[Browse to and select this project]
    ///
    ///
    /// Step 2)
    /// Go ahead and use your control in the XAML file.
    ///
    ///     <MyNamespace:CrawlStatusIndicator/>
    ///
    /// </summary>
    public class CrawlStatusIndicator : Control
    {
        #region Status Property Members

        /// <summary>
        /// Identifies the <see cref="Status"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty StatusProperty = DependencyPropertyBuilder<CrawlStatusIndicator, Model.CrawlStatus>
            .Register(nameof(Status))
            .DefaultValue(Model.CrawlStatus.NotRunning)
            .OnChanged((d, oldValue, newValue) => (d as CrawlStatusIndicator)?.OnStatusPropertyChanged(oldValue, newValue))
            .AsReadWrite();

        public Model.CrawlStatus Status { get => (Model.CrawlStatus)GetValue(StatusProperty); set => SetValue(StatusProperty, value); }

        /// <summary>
        /// Called when the value of the <see cref="Status"/> dependency property has changed.
        /// </summary>
        /// <param name="oldValue">The previous value of the <see cref="Status"/> property.</param>
        /// <param name="newValue">The new value of the <see cref="Status"/> property.</param>
        protected virtual void OnStatusPropertyChanged(Model.CrawlStatus oldValue, Model.CrawlStatus newValue)
        {
            // TODO: Implement OnStatusPropertyChanged Logic
        }

        #endregion

        static CrawlStatusIndicator()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(CrawlStatusIndicator), new FrameworkPropertyMetadata(typeof(CrawlStatusIndicator)));
        }
    }
}
