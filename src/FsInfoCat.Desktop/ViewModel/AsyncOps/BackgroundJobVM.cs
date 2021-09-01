using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using System.Windows;

namespace FsInfoCat.Desktop.ViewModel.AsyncOps
{
    public partial class BackgroundJobVM : DependencyObject
    {
        private readonly ILogger<BackgroundJobVM> _logger;

        #region Title Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="Title"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler TitlePropertyChanged;

        /// <summary>
        /// Identifies the <see cref="Title"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.Register(nameof(Title), typeof(string), typeof(BackgroundJobVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as BackgroundJobVM).TitlePropertyChanged?.Invoke(d, e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Title { get => GetValue(TitleProperty) as string; set => SetValue(TitleProperty, value); }

        #endregion
        #region Message Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="Message"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler MessagePropertyChanged;

        /// <summary>
        /// Identifies the <see cref="Message"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MessageProperty = DependencyProperty.Register(nameof(Message), typeof(string), typeof(BackgroundJobVM),
                new PropertyMetadata("", (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as BackgroundJobVM).MessagePropertyChanged?.Invoke(d, e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public string Message { get => GetValue(MessageProperty) as string; set => SetValue(MessageProperty, value); }

        #endregion
        #region MessageLevel Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="MessageLevel"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler MessageLevelPropertyChanged;

        /// <summary>
        /// Identifies the <see cref="MessageLevel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty MessageLevelProperty = DependencyProperty.Register(nameof(MessageLevel), typeof(StatusMessageLevel),
            typeof(BackgroundJobVM), new PropertyMetadata(StatusMessageLevel.Information,
                (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as BackgroundJobVM)?.MessageLevelPropertyChanged?.Invoke(d, e)));

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public StatusMessageLevel MessageLevel { get => (StatusMessageLevel)GetValue(MessageLevelProperty); set => SetValue(MessageLevelProperty, value); }

        #endregion
        #region Cancel Property Members

        /// <summary>
        /// Occurs when the <see cref="Cancel">Cancel Command</see> is invoked.
        /// </summary>
        public event EventHandler<Commands.CommandEventArgs> CancelInvoked;

        private static readonly DependencyPropertyKey CancelPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Cancel),
            typeof(Commands.RelayCommand), typeof(BackgroundJobVM), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="Cancel"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CancelProperty = CancelPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the $name$ command object.
        /// </summary>
        /// <value>The <see cref="System.Windows.Input.ICommand"/> that implements the $command$ command.</value>
        public Commands.RelayCommand Cancel => (Commands.RelayCommand)GetValue(CancelProperty);

        #endregion
        #region IsCompleted Property Members

        private static readonly DependencyPropertyKey IsCompletedPropertyKey = DependencyProperty.RegisterReadOnly(nameof(IsCompleted), typeof(bool),
            typeof(BackgroundJobVM), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the <see cref="IsCompleted"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsCompletedProperty = IsCompletedPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public bool IsCompleted { get => (bool)GetValue(IsCompletedProperty); private set => SetValue(IsCompletedPropertyKey, value); }

        #endregion
        #region JobStatus Property Members

        /// <summary>
        /// Occurs when the value of the <see cref="JobStatus"/> dependency property has changed.
        /// </summary>
        public event DependencyPropertyChangedEventHandler JobStatusPropertyChanged;

        private static readonly DependencyPropertyKey JobStatusPropertyKey = DependencyProperty.RegisterReadOnly(nameof(JobStatus), typeof(AsyncJobStatus),
            typeof(BackgroundJobVM), new PropertyMetadata(AsyncJobStatus.WaitingToRun, (DependencyObject d, DependencyPropertyChangedEventArgs e) =>
            (d as BackgroundJobVM)?.JobStatusPropertyChanged?.Invoke(d, e)));

        /// <summary>
        /// Identifies the <see cref="JobStatus"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty JobStatusProperty = JobStatusPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets or sets .
        /// </summary>
        /// <value>The .</value>
        public AsyncJobStatus JobStatus { get => (AsyncJobStatus)GetValue(JobStatusProperty); private set => SetValue(JobStatusPropertyKey, value); }

        #endregion
        #region Duration Property Members

        private static readonly DependencyPropertyKey DurationPropertyKey = DependencyProperty.RegisterReadOnly(nameof(Duration), typeof(TimeSpan), typeof(BackgroundJobVM),
                new PropertyMetadata(TimeSpan.Zero));

        /// <summary>
        /// Identifies the <see cref="Duration"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DurationProperty = DurationPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets .
        /// </summary>
        /// <value>The .</value>
        public TimeSpan Duration { get => (TimeSpan)GetValue(DurationProperty); private set => SetValue(DurationPropertyKey, value); }

        #endregion
        public BackgroundJobVM()
        {
            _logger = App.GetLogger(this);
            SetValue(CancelPropertyKey, new Commands.RelayCommand(parameter => CancelInvoked?.Invoke(this, new(parameter))));
        }

        public static BackgroundJobVM Create([DisallowNull] string title, [DisallowNull] string initialMessage,
            [DisallowNull] Func<StatusListener, IBackgroundJob, Task> createTask, out IAsyncJob job)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException($"'{nameof(title)}' cannot be null or whitespace.", nameof(title));
            if (string.IsNullOrWhiteSpace(initialMessage))
                throw new ArgumentException($"'{nameof(initialMessage)}' cannot be null or whitespace.", nameof(initialMessage));
            if (createTask is null)
                throw new ArgumentNullException(nameof(createTask));

            BackgroundJobVM viewModel = new();
            job = new Job(title, initialMessage, viewModel, createTask);
            viewModel.Message = job.Message;
            viewModel.Title = job.Title;
            viewModel.MessageLevel = job.MessageLevel;
            return viewModel;
        }

        public static BackgroundJobVM Create<TResult>([DisallowNull] string title, [DisallowNull] string initialMessage,
            [DisallowNull] Func<StatusListener, IBackgroundJob, Task<TResult>> createTask, out IAsyncJob<TResult> job)
        {
            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException($"'{nameof(title)}' cannot be null or whitespace.", nameof(title));
            if (string.IsNullOrWhiteSpace(initialMessage))
                throw new ArgumentException($"'{nameof(initialMessage)}' cannot be null or whitespace.", nameof(initialMessage));
            if (createTask is null)
                throw new ArgumentNullException(nameof(createTask));

            BackgroundJobVM viewModel = new() { Title = title, Message = initialMessage };
            job = new Job<TResult>(title, initialMessage, viewModel, createTask);
            viewModel.Message = job.Message;
            viewModel.Title = job.Title;
            viewModel.MessageLevel = job.MessageLevel;
            return viewModel;
        }
    }
}
