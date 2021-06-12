using Shell32;

namespace FsInfoCat.Desktop.COM
{
    public class ShellAdapter : ComObjectAdapter, Shell
    {
        public ShellAdapter() : base("Shell.Application") { }

        public Folder NameSpace(object vDir) => (Folder)InvokeMethod(nameof(NameSpace), vDir);

        public Folder BrowseForFolder(int Hwnd, string Title, int Options, object RootFolder) => (Folder)InvokeMethod(nameof(BrowseForFolder), Hwnd, Title, Options, RootFolder);

        public dynamic Windows() => InvokeMethod(nameof(Windows));

        public void Open(object vDir) => InvokeMethod(nameof(Open), vDir);

        public void Explore(object vDir) => InvokeMethod(nameof(Explore), vDir);

        public void MinimizeAll() => InvokeMethod(nameof(MinimizeAll));

        public void UndoMinimizeALL() => InvokeMethod(nameof(UndoMinimizeALL));

        public void FileRun() => InvokeMethod(nameof(FileRun));

        public void CascadeWindows() => InvokeMethod(nameof(CascadeWindows));

        public void TileVertically() => InvokeMethod(nameof(TileVertically));

        public void TileHorizontally() => InvokeMethod(nameof(TileHorizontally));

        public void ShutdownWindows() => InvokeMethod(nameof(ShutdownWindows));

        public void Suspend() => InvokeMethod(nameof(Suspend));

        public void EjectPC() => InvokeMethod(nameof(EjectPC));

        public void SetTime() => InvokeMethod(nameof(SetTime));

        public void TrayProperties() => InvokeMethod(nameof(TrayProperties));

        public void Help() => InvokeMethod(nameof(Help));

        public void FindFiles() => InvokeMethod(nameof(FindFiles));

        public void FindComputer() => InvokeMethod(nameof(FindComputer));

        public void RefreshMenu() => InvokeMethod(nameof(RefreshMenu));

        public void ControlPanelItem(string bstrDir) => InvokeMethod(nameof(ControlPanelItem), bstrDir);

        public int IsRestricted(string Group, string Restriction) => (int)InvokeMethod(nameof(IsRestricted), Group, Restriction);

        public void ShellExecute(string File, object vArgs, object vDir, object vOperation, object vShow) =>
            InvokeMethod(nameof(ShellExecute), File, vArgs, vDir, vOperation, vShow);

        public void FindPrinter(string Name, string location, string model) => InvokeMethod(nameof(FindPrinter), Name, location, model);

        public dynamic GetSystemInformation(string Name) => InvokeMethod(nameof(GetSystemInformation), Name);

        public dynamic ServiceStart(string ServiceName, object Persistent) => InvokeMethod(nameof(ServiceStart), ServiceName, Persistent);

        public dynamic ServiceStop(string ServiceName, object Persistent) => InvokeMethod(nameof(ServiceStop), ServiceName, Persistent);

        public dynamic IsServiceRunning(string ServiceName) => InvokeMethod(nameof(IsServiceRunning), ServiceName);

        public dynamic CanStartStopService(string ServiceName) => InvokeMethod(nameof(CanStartStopService), ServiceName);

        public dynamic ShowBrowserBar(string bstrClsid, object bShow) => InvokeMethod(nameof(ShowBrowserBar), bstrClsid, bShow);

        public void AddToRecent(object varFile, string bstrCategory) => InvokeMethod(nameof(AddToRecent), varFile, bstrCategory);

        public void WindowsSecurity() => InvokeMethod(nameof(WindowsSecurity));

        public void ToggleDesktop() => InvokeMethod(nameof(ToggleDesktop));

        public dynamic ExplorerPolicy(string bstrPolicyName) => InvokeMethod(nameof(ExplorerPolicy), bstrPolicyName);

        public bool GetSetting(int lSetting) => (bool)InvokeMethod(nameof(ExplorerPolicy), lSetting);

        public void WindowSwitcher() => InvokeMethod(nameof(WindowSwitcher));

        public void SearchCommand() => InvokeMethod(nameof(SearchCommand));

        public dynamic Application => GetProperty(nameof(Application));

        public dynamic Parent => GetProperty(nameof(Parent));
    }
}
