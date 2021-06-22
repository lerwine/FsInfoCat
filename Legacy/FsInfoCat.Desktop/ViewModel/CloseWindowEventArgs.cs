using System;

namespace FsInfoCat.Desktop.ViewModel
{
    public class CloseWindowEventArgs : EventArgs
    {
        public CloseWindowEventArgs(bool? dialogResult)
        {
            DialogResult = dialogResult;
        }

        public bool? DialogResult { get; }
    }
}
