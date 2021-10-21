using FsInfoCat.Desktop.ViewModel;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.UnitTests
{
    [TestClass]
    public class MainVMTestClass
    {
        [TestMethod]
        public async Task ConstructorTestMethod()
        {
            Thread.Sleep(1000);
            IApplicationNavigation nav = await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Thread.Sleep(1000);
                return Hosting.ServiceProvider.GetRequiredService<IApplicationNavigation>();
            });
            Assert.IsNotNull(nav);
            Assert.AreSame(nav, Hosting.ServiceProvider.GetRequiredService<IApplicationNavigation>());
            await Application.Current.Dispatcher.InvokeAsync(() => Assert.IsFalse(nav.CanGoBack));
            await Application.Current.Dispatcher.InvokeAsync(() => Assert.IsNull(nav.Content));
            MainVM target = nav as MainVM;
            Assert.IsNotNull(target);
            await Application.Current.Dispatcher.InvokeAsync(() => Assert.IsNull(target.NavigatedContent));
            await Application.Current.Dispatcher.InvokeAsync(() => Assert.IsNotNull(target.NewCrawl));
            await Application.Current.Dispatcher.InvokeAsync(() => Assert.IsNotNull(target.ViewAudioPropertySets));
            await Application.Current.Dispatcher.InvokeAsync(() => Assert.IsNotNull(target.ViewCrawlConfigurations));
            await Application.Current.Dispatcher.InvokeAsync(() => Assert.IsNotNull(target.ViewCrawlLogs));
            await Application.Current.Dispatcher.InvokeAsync(() => Assert.IsNotNull(target.ViewDocumentPropertySets));
            await Application.Current.Dispatcher.InvokeAsync(() => Assert.IsNotNull(target.ViewDRMPropertySets));
            await Application.Current.Dispatcher.InvokeAsync(() => Assert.IsNotNull(target.ViewFileSystems));
            await Application.Current.Dispatcher.InvokeAsync(() => Assert.IsNotNull(target.ViewGPSPropertySets));
            await Application.Current.Dispatcher.InvokeAsync(() => Assert.IsNotNull(target.ViewImagePropertySets));
            await Application.Current.Dispatcher.InvokeAsync(() => Assert.IsNotNull(target.ViewMediaPropertySets));
            await Application.Current.Dispatcher.InvokeAsync(() => Assert.IsNotNull(target.ViewMusicPropertySets));
            await Application.Current.Dispatcher.InvokeAsync(() => Assert.IsNotNull(target.ViewPersonalTagDefinitions));
            await Application.Current.Dispatcher.InvokeAsync(() => Assert.IsNotNull(target.ViewPhotoPropertySets));
            await Application.Current.Dispatcher.InvokeAsync(() => Assert.IsNotNull(target.ViewRecordedTVPropertySets));
            await Application.Current.Dispatcher.InvokeAsync(() => Assert.IsNotNull(target.ViewRedundancySets));
            await Application.Current.Dispatcher.InvokeAsync(() => Assert.IsNotNull(target.ViewSharedTagDefinitions));
            await Application.Current.Dispatcher.InvokeAsync(() => Assert.IsNotNull(target.ViewSummaryPropertySets));
            await Application.Current.Dispatcher.InvokeAsync(() => Assert.IsNotNull(target.ViewSymbolicNames));
            await Application.Current.Dispatcher.InvokeAsync(() => Assert.IsNotNull(target.ViewVideoPropertySets));
            await Application.Current.Dispatcher.InvokeAsync(() => Assert.IsNotNull(target.ViewVolumes));
        }
    }
}
