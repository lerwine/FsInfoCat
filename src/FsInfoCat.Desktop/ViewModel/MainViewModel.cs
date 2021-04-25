using FsInfoCat.Desktop.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel
{
    public class MainViewModel : DependencyObject
    {
        #region Properties

        #region LocalVolumes Property Members

        private static readonly DependencyPropertyKey InnerLocalVolumesPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(InnerLocalVolumes), typeof(ObservableCollection<LocalVolumeVM>), typeof(MainViewModel),
                new PropertyMetadata(new ObservableCollection<LocalVolumeVM>()));

        private static readonly DependencyPropertyKey LocalVolumesPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(LocalVolumes), typeof(ReadOnlyObservableCollection<LocalVolumeVM>), typeof(MainViewModel),
                new PropertyMetadata(null));

        protected static readonly DependencyProperty InnerLocalVolumesProperty = InnerLocalVolumesPropertyKey.DependencyProperty;

        public static readonly DependencyProperty LocalVolumesProperty = LocalVolumesPropertyKey.DependencyProperty;

        protected ObservableCollection<LocalVolumeVM> InnerLocalVolumes
        {
            get { return (ObservableCollection<LocalVolumeVM>)GetValue(InnerLocalVolumesProperty); }
            private set { SetValue(InnerLocalVolumesPropertyKey, value); }
        }

        public ReadOnlyObservableCollection<LocalVolumeVM> LocalVolumes
        {
            get
            {
                ReadOnlyObservableCollection<LocalVolumeVM> value = (ReadOnlyObservableCollection<LocalVolumeVM>)GetValue(LocalVolumesProperty);

                if (value == null)
                {
                    value = new ReadOnlyObservableCollection<LocalVolumeVM>(InnerLocalVolumes);
                    SetValue(LocalVolumesPropertyKey, value);
                }

                return value;
            }
            private set { SetValue(LocalVolumesPropertyKey, value); }
        }

        #endregion

        private static readonly DependencyPropertyKey RefreshCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(RefreshCommand), typeof(Commands.RelayCommand),
            typeof(MainViewModel), new PropertyMetadata(null, null, (DependencyObject d, object baseValue) =>
                (baseValue is Commands.RelayCommand rc) ? rc : new Commands.RelayCommand(((MainViewModel)d).OnRefreshExecute)));

        public static readonly DependencyProperty RefreshCommandProperty = RefreshCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand RefreshCommand => (Commands.RelayCommand)GetValue(RefreshCommandProperty);

        private static readonly DependencyPropertyKey ModalStatusPropertyKey = DependencyProperty.RegisterReadOnly(nameof(ModalStatus), typeof(ModalOperationStatusViewModel),
                typeof(MainViewModel), new PropertyMetadata(null, null,
                    (DependencyObject d, object baseValue) => (baseValue is ModalOperationStatusViewModel vm) ? vm : new ModalOperationStatusViewModel()));

        public static readonly DependencyProperty ModalStatusProperty = ModalStatusPropertyKey.DependencyProperty;

        public ModalOperationStatusViewModel ModalStatus
        {
            get { return (ModalOperationStatusViewModel)GetValue(ModalStatusProperty); }
            private set { SetValue(ModalStatusPropertyKey, value); }
        }

        #endregion

        public static async Task<List<LocalVolume>> GetDbVolumesAsync()
        {
            using (LocalDbContext dbContext = new LocalDbContext())
                return await dbContext.Volumes.ToListAsync();
        }

        public MainViewModel()
        {
            ModalStatus.StartNew("Getting local drive listing", controller =>
            {
                List<Win32_LogicalDiskRootDirectory> sysVolumes = Win32_LogicalDiskRootDirectory.GetLogicalDiskRootDirectories(controller);
                Task<List<LocalVolume>> task = GetDbVolumesAsync();
                task.Wait();
                return new Tuple<List<LocalVolume>, List<Win32_LogicalDiskRootDirectory>>(task.Result, sysVolumes);
            }).ContinueWith(task =>
            {
                if (!(task.IsCanceled || task.IsFaulted))
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        foreach (LocalVolumeVM vm in LocalVolumeVM.GetAllLocalVolumes(task.Result.Item1, task.Result.Item2))
                            InnerLocalVolumes.Add(vm);
                    }));
            });
        }

        private void OnRefreshExecute()
        {
            ModalStatus.StartNew("Refreshing logical drive listing", Win32_LogicalDiskRootDirectory.GetLogicalDiskRootDirectories).ContinueWith(t =>
                Dispatcher.BeginInvoke(new Action<Task<List<Win32_LogicalDiskRootDirectory>>>(OnGetLogicalDiskRootDirectoriesCompleted), t));
        }

        private void OnGetLogicalDiskRootDirectoriesCompleted(Task<List<Win32_LogicalDiskRootDirectory>> task)
        {
            if (!(task.IsCanceled || task.IsFaulted))
                LocalVolumeVM.Refresh(LocalVolumes, task.Result);
        }
    }
}
