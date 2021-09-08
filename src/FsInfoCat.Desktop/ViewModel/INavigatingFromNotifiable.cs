using System.ComponentModel;

namespace FsInfoCat.Desktop.ViewModel
{
    interface INavigatingFromNotifiable
    {
        void OnNavigatingFrom(CancelEventArgs e);
    }
}
