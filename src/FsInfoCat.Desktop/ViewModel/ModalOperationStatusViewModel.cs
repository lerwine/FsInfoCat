using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel
{
    public class ModalOperationStatusViewModel : DependencyObject
    {
        private static readonly DependencyPropertyKey IsIndeterminatePropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsIndeterminate), typeof(bool),
            typeof(ModalOperationStatusViewModel), new PropertyMetadata(true));

        public static readonly DependencyProperty IsIndeterminateProperty = IsIndeterminatePropertyKey.DependencyProperty;

        public bool IsIndeterminate
        {
            get { return (bool)GetValue(IsIndeterminateProperty); }
            private set { SetValue(IsIndeterminatePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey ProgressPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Progress), typeof(int), typeof(ModalOperationStatusViewModel),
                new PropertyMetadata(0));

        public static readonly DependencyProperty ProgressProperty = ProgressPropertyKey.DependencyProperty;

        public int Progress
        {
            get { return (int)GetValue(ProgressProperty); }
            private set { SetValue(ProgressPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey HeadingPropertyKey =
            DependencyProperty.RegisterReadOnly(nameof(Heading), typeof(string), typeof(ModalOperationStatusViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty HeadingProperty = HeadingPropertyKey.DependencyProperty;

        public string Heading
        {
            get { return GetValue(HeadingProperty) as string; }
            private set { SetValue(HeadingPropertyKey, value); }
        }

        private static readonly DependencyPropertyKey MessagePropertyKey = DependencyProperty.RegisterReadOnly(nameof(Message), typeof(string), typeof(ModalOperationStatusViewModel),
                new PropertyMetadata(""));

        public static readonly DependencyProperty MessageProperty = MessagePropertyKey.DependencyProperty;

        public string Message
        {
            get { return GetValue(MessageProperty) as string; }
            private set { SetValue(MessagePropertyKey, value); }
        }

        private static readonly DependencyPropertyKey StatusPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Status), typeof(TaskStatus), typeof(ModalOperationStatusViewModel),
                new PropertyMetadata(TaskStatus.Created));

        public static readonly DependencyProperty StatusProperty = StatusPropertyKey.DependencyProperty;

        public TaskStatus Status
        {
            get { return (TaskStatus)GetValue(StatusProperty); }
            private set { SetValue(StatusPropertyKey, value); }
        }

        public event EventHandler Cancel;

        private static readonly DependencyPropertyKey CancelCommandPropertyKey = DependencyProperty.RegisterReadOnly(nameof(CancelCommand),
            typeof(Commands.RelayCommand), typeof(ModalOperationStatusViewModel), new PropertyMetadata(null, null, (DependencyObject d, object baseValue) =>
                (baseValue is Commands.RelayCommand rc) ? rc : new Commands.RelayCommand(() => ((ModalOperationStatusViewModel)d).Cancel?.Invoke(d, EventArgs.Empty))));

        public static readonly DependencyProperty CancelCommandProperty = CancelCommandPropertyKey.DependencyProperty;

        public Commands.RelayCommand CancelCommand => (Commands.RelayCommand)GetValue(CancelCommandProperty);

        public class Controller
        {
            private readonly ModalOperationStatusViewModel _target;

            internal Controller(ModalOperationStatusViewModel target)
            {
                _target = target ?? throw new ArgumentNullException(nameof(target));
            }

            public void SetStatus(string heading, string message, int progress, TaskStatus status) =>
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.IsIndeterminate = false;
                    _target.Heading = heading;
                    _target.Message = message;
                    _target.Progress = progress;
                    _target.Status = status;
                }));

            public void SetStatus(string message, int progress, TaskStatus status) =>
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.IsIndeterminate = false;
                    _target.Message = message;
                    _target.Progress = progress;
                    _target.Status = status;
                }));

            public void SetStatus(string heading, string message, TaskStatus status) =>
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.Heading = heading;
                    _target.Message = message;
                    _target.Status = status;
                }));

            public void SetStatus(string heading, string message, int progress) =>
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.IsIndeterminate = false;
                    _target.Heading = heading;
                    _target.Message = message;
                    _target.Progress = progress;
                }));

            public void SetStatus(string message, TaskStatus status) =>
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.Message = message;
                    _target.Status = status;
                }));

            public void SetStatus(string message, int progress) =>
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.IsIndeterminate = false;
                    _target.Message = message;
                    _target.Progress = progress;
                }));

            public void SetStatus(int progress, TaskStatus status) =>
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.IsIndeterminate = false;
                    _target.Progress = progress;
                    _target.Status = status;
                }));

            public void SetStatus(int progress)
            {
                _target.IsIndeterminate = false;
                _target.Dispatcher.Invoke(new Action(() => _target.Progress = progress));
            }

            public void SetStatus(TaskStatus status) =>
                _target.Dispatcher.Invoke(new Action(() => _target.Status = status));

            public void SetStatus(string heading, string message) =>
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.Heading = heading;
                    _target.Message = message;
                }));

            public void SetStatus(string message) =>
                _target.Dispatcher.Invoke(new Action(() => _target.Message = message));

            public void SetIndeterminateStatus(string heading, string message, TaskStatus status) =>
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.Heading = heading;
                    _target.Message = message;
                    _target.Status = status;
                }));

            public void SetIndeterminateStatus(string message, TaskStatus status) =>
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.Message = message;
                    _target.Status = status;
                }));

            public void SetIndeterminateStatus(string heading, string message) =>
                _target.Dispatcher.Invoke(new Action(() =>
                {
                    _target.Heading = heading;
                    _target.Message = message;
                }));

            public void SetIndeterminateStatus(TaskStatus status) =>
                _target.Dispatcher.Invoke(new Action(() => _target.Status = status));

            public void SetIndeterminateStatus(string message) =>
                _target.Dispatcher.Invoke(new Action(() => _target.Message = message));

            public DispatcherOperation BeginSetStatus(string heading, string message, int progress, TaskStatus status) =>
                _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _target.IsIndeterminate = false;
                    _target.Heading = heading;
                    _target.Message = message;
                    _target.Progress = progress;
                    _target.Status = status;
                }));

            public DispatcherOperation BeginSetStatus(string message, int progress, TaskStatus status) =>
                _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _target.IsIndeterminate = false;
                    _target.Message = message;
                    _target.Progress = progress;
                    _target.Status = status;
                }));

            public DispatcherOperation BeginSetStatus(string heading, string message, TaskStatus status) =>
                _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _target.Heading = heading;
                    _target.Message = message;
                    _target.Status = status;
                }));

            public DispatcherOperation BeginSetStatus(string heading, string message, int progress) =>
                _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _target.IsIndeterminate = false;
                    _target.Heading = heading;
                    _target.Message = message;
                    _target.Progress = progress;
                }));

            public DispatcherOperation BeginSetStatus(string message, TaskStatus status) =>
                _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _target.Message = message;
                    _target.Status = status;
                }));

            public DispatcherOperation BeginSetStatus(string message, int progress) =>
                _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _target.IsIndeterminate = false;
                    _target.Message = message;
                    _target.Progress = progress;
                }));

            public DispatcherOperation BeginSetStatus(int progress, TaskStatus status) =>
                _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _target.IsIndeterminate = false;
                    _target.Progress = progress;
                    _target.Status = status;
                }));

            public DispatcherOperation BeginSetStatus(string heading, string message) =>
                _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _target.Heading = heading;
                    _target.Message = message;
                }));

            public DispatcherOperation BeginSetStatus(int progress)
            {
                _target.IsIndeterminate = false;
                return _target.Dispatcher.BeginInvoke(new Action(() => _target.Progress = progress));
            }
            
            public DispatcherOperation BeginSetStatus(TaskStatus status) =>
                _target.Dispatcher.BeginInvoke(new Action(() => _target.Status = status));

            public DispatcherOperation BeginSetStatus(string message) =>
                _target.Dispatcher.BeginInvoke(new Action(() => _target.Message = message));

            public DispatcherOperation BeginSetIndeterminateStatus(string heading, string message, TaskStatus status) =>
                _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _target.Heading = heading;
                    _target.IsIndeterminate = true;
                    _target.Message = message;
                    _target.Status = status;
                }));

            public DispatcherOperation BeginSetIndeterminateStatus(string message, TaskStatus status) =>
                _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _target.IsIndeterminate = true;
                    _target.Message = message;
                    _target.Status = status;
                }));

            public DispatcherOperation BeginSetIndeterminateStatus(string heading, string message) =>
                _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _target.Heading = heading;
                    _target.IsIndeterminate = true;
                    _target.Message = message;
                }));

            public DispatcherOperation BeginSetIndeterminateStatus(TaskStatus status) =>
                _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _target.IsIndeterminate = true;
                    _target.Status = status;
                }));

            public DispatcherOperation BeginSetIndeterminateStatus(string message) =>
                _target.Dispatcher.BeginInvoke(new Action(() =>
                {
                    _target.IsIndeterminate = true;
                    _target.Message = message;
                }));
        }
    }
}
