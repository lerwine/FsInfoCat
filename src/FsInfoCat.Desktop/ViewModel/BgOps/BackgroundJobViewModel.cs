using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace FsInfoCat.Desktop.ViewModel.BgOps
{
    public partial class BackgroundJobViewModel : DependencyObject
    {
        public event DependencyPropertyChangedEventHandler StatusPropertyChanged;
        public event DependencyPropertyChangedEventHandler StatusMessagePropertyChanged;
        public event DependencyPropertyChangedEventHandler MessageLevelPropertyChanged;
        public event DependencyPropertyChangedEventHandler StatusDetailPropertyChanged;

        private static readonly DependencyPropertyKey StatusPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Status), typeof(JobStatus), typeof(BackgroundJobViewModel),
                new PropertyMetadata(JobStatus.NotRunning, (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as BackgroundJobViewModel).OnStatusPropertyChanged(e)));

        public static readonly DependencyProperty StatusProperty = StatusPropertyKey.DependencyProperty;

        public JobStatus Status
        {
            get => (JobStatus)GetValue(StatusProperty);
            private set => SetValue(StatusPropertyKey, value);
        }

        protected virtual void OnStatusPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnStatusPropertyChanged((JobStatus)args.OldValue, (JobStatus)args.NewValue); }
            finally { StatusPropertyChanged?.Invoke(this, args); }
        }

        protected virtual void OnStatusPropertyChanged(JobStatus oldValue, JobStatus newValue)
        {
            // TODO: Implement OnStatusPropertyChanged Logic
        }

        private static readonly DependencyPropertyKey StatusMessagePropertyKey = DependencyProperty.RegisterReadOnly(nameof(StatusMessage), typeof(string),
            typeof(BackgroundJobViewModel), new PropertyMetadata("",
                (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as BackgroundJobViewModel).OnStatusMessagePropertyChanged(e)));

        public static readonly DependencyProperty StatusMessageProperty = StatusMessagePropertyKey.DependencyProperty;

        public string StatusMessage
        {
            get => GetValue(StatusMessageProperty) as string;
            private set => SetValue(StatusMessagePropertyKey, value);
        }

        protected virtual void OnStatusMessagePropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnStatusMessagePropertyChanged(args.OldValue as string, args.NewValue as string); }
            finally { StatusMessagePropertyChanged?.Invoke(this, args); }
        }

        protected virtual void OnStatusMessagePropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnStatusMessagePropertyChanged Logic
        }

        private static readonly DependencyPropertyKey MessageLevelPropertyKey = DependencyProperty.RegisterReadOnly(nameof(MessageLevel), typeof(StatusMessageLevel),
            typeof(BackgroundJobViewModel), new PropertyMetadata(StatusMessageLevel.Information,
                (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as BackgroundJobViewModel).OnMessageLevelPropertyChanged(e)));

        public static readonly DependencyProperty MessageLevelProperty = MessageLevelPropertyKey.DependencyProperty;

        public StatusMessageLevel MessageLevel
        {
            get => (StatusMessageLevel)GetValue(MessageLevelProperty);
            private set => SetValue(MessageLevelPropertyKey, value);
        }

        protected virtual void OnMessageLevelPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnMessageLevelPropertyChanged((StatusMessageLevel)args.OldValue, (StatusMessageLevel)args.NewValue); }
            finally { MessageLevelPropertyChanged?.Invoke(this, args); }
        }

        protected virtual void OnMessageLevelPropertyChanged(StatusMessageLevel oldValue, StatusMessageLevel newValue)
        {
            // TODO: Implement OnMessageLevelPropertyChanged Logic
        }

        private static readonly DependencyPropertyKey StatusDetailPropertyKey = DependencyProperty.RegisterReadOnly(nameof(StatusDetail), typeof(string), typeof(BackgroundJobViewModel),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as BackgroundJobViewModel).OnStatusDetailPropertyChanged(e)));

        public static readonly DependencyProperty StatusDetailProperty = StatusDetailPropertyKey.DependencyProperty;

        public string StatusDetail
        {
            get => GetValue(StatusDetailProperty) as string;
            private set => SetValue(StatusDetailPropertyKey, value);
        }

        protected virtual void OnStatusDetailPropertyChanged(DependencyPropertyChangedEventArgs args)
        {
            try { OnStatusDetailPropertyChanged(args.OldValue as string, args.NewValue as string); }
            finally { StatusDetailPropertyChanged?.Invoke(this, args); }
        }

        protected virtual void OnStatusDetailPropertyChanged(string oldValue, string newValue)
        {
            // TODO: Implement OnStatusDetailPropertyChanged Logic
        }

        public IStateJob<TState, Task<TResult>> StartNew<TState, TResult>(TState state, Func<IContext<TState>, TResult> doWork, Action<JobProgressEventArgs<TState>> onProgressChanged = null)
        {
            return Job<Task<TResult>, TState>.Create(state, this, ctx =>
            {
                return Task.Factory.StartNew(s =>
                {
                    Dispatcher.Invoke(() => Status = JobStatus.Running, DispatcherPriority.Background, ctx.Token);
                    return doWork(ctx);
                }, ctx.State, ctx.Token);
            }, onProgressChanged);
        }

        public IJob<Task<TResult>> StartNew<TResult>(Func<IContext, TResult> doWork, Action<JobProgressEventArgs> onProgressChanged = null)
        {
            return Job<Task<TResult>, object>.Create(null, this, ctx =>
            {
                return Task.Factory.StartNew(s =>
                {
                    Dispatcher.Invoke(() => Status = JobStatus.Running, DispatcherPriority.Background, ctx.Token);
                    return doWork(ctx);
                }, ctx.State, ctx.Token);
            }, onProgressChanged);
        }

        public IStateJob<TState, Task> StartNew<TState>(TState state, Action<IContext<TState>> doWork, Action<JobProgressEventArgs<TState>> onProgressChanged = null)
        {
            return Job<Task, TState>.Create(state, this, ctx =>
            {
                return Task.Factory.StartNew(s =>
                {
                    Dispatcher.Invoke(() => Status = JobStatus.Running, DispatcherPriority.Background, ctx.Token);
                    doWork(ctx);
                }, ctx.State, ctx.Token);
            }, onProgressChanged);
        }

        public IJob<Task> StartNew(Action<IContext> doWork, Action<JobProgressEventArgs> onProgressChanged = null)
        {
            return Job<Task, object>.Create(null, this, ctx =>
            {
                return Task.Factory.StartNew(s =>
                {
                    Dispatcher.Invoke(() => Status = JobStatus.Running, DispatcherPriority.Background, ctx.Token);
                    doWork(ctx);
                }, ctx.State, ctx.Token);
            }, onProgressChanged);
        }

        public IStateJob<TState, Task<TResult>> FromAsync<TState, TResult>(TState state, Func<IContext<TState>, Task<TResult>> factory,
            Action<JobProgressEventArgs<TState>> onProgressChanged = null)
        {
            return Job<Task<TResult>, TState>.Create(state, this, ctx =>
            {
                return Task.Run(async () =>
                {
                    Dispatcher.Invoke(() => Status = JobStatus.Running, DispatcherPriority.Background, ctx.Token);
                    return await factory(ctx);
                }, ctx.Token);
            }, onProgressChanged);
        }

        public IJob<Task<TResult>> FromAsync<TResult>(Func<IContext, Task<TResult>> factory, Action<JobProgressEventArgs> onProgressChanged = null)
        {
            return Job<Task<TResult>, object>.Create(null, this, ctx =>
            {
                return Task.Run(async () =>
                {
                    Dispatcher.Invoke(() => Status = JobStatus.Running, DispatcherPriority.Background, ctx.Token);
                    return await factory(ctx);
                }, ctx.Token);
            }, onProgressChanged);
        }

        public IStateJob<TState, Task> FromAsync<TState>(TState state, Func<IContext<TState>, Task> factory, Action<JobProgressEventArgs<TState>> onProgressChanged = null)
        {
            return Job<Task, TState>.Create(state, this, ctx =>
            {
                return Task.Run(async () =>
                {
                    Dispatcher.Invoke(() => Status = JobStatus.Running, DispatcherPriority.Background, ctx.Token);
                    await factory(ctx);
                }, ctx.Token);
            }, onProgressChanged);
        }

        public IJob<Task> FromAsync(Func<IContext, Task> factory, Action<JobProgressEventArgs> onProgressChanged = null)
        {
            return Job<Task, object>.Create(null, this, ctx =>
            {
                return Task.Run(async () =>
                {
                    Dispatcher.Invoke(() => Status = JobStatus.Running, DispatcherPriority.Background, ctx.Token);
                    await factory(ctx);
                }, ctx.Token);
            }, onProgressChanged);
        }
    }
}
