using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FsInfoCat.Desktop.ViewModels
{
    public class AppSettingsViewModel : DependencyObject
    {
        private Task _saveSettingsTask = null;
        private bool _noSaveOnChange = true;
        private object _syncRoot = new object();
        private CancellationTokenSource _tokenSource = null;

        #region RawBaseWebApiURI Property Members

        /// <summary>
        /// Defines the name for the <see cref="RawBaseWebApiURI"/> dependency property.
        /// </summary>
        public const string DependencyPropertyName_RawBaseWebApiURI = "RawBaseWebApiURI";

        /// <summary>
        /// Identifies the <see cref="RawBaseWebApiURI"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RawBaseWebApiURIProperty = DependencyProperty.Register(DependencyPropertyName_RawBaseWebApiURI, typeof(string), typeof(AppSettingsViewModel),
                new PropertyMetadata("",
                    (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as AppSettingsViewModel).RawBaseWebApiURI_PropertyChanged(e.OldValue as string, e.NewValue as string),
                    (DependencyObject d, object baseValue) => CoerceNonNull(baseValue as string)));

        /// <summary>
        /// Raw Base URI for web API calls.
        /// </summary>
        public string RawBaseWebApiURI
        {
            get { return GetValue(RawBaseWebApiURIProperty) as string; }
            set { SetValue(RawBaseWebApiURIProperty, value); }
        }

        private void RawBaseWebApiURI_PropertyChanged(string oldValue, string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue))
            {
                ValidatedBaseWebApiURI = "";
                BaseWebApiURIErrorMessage = "Not Provided";
                RegisterCommand.IsEnabled = false;
                HostIdRegistrationMessage = "";
            }
            else if (Uri.TryCreate(newValue, UriKind.RelativeOrAbsolute, out Uri uri))
            {
                if (uri.IsAbsoluteUri)
                {
                    ValidatedBaseWebApiURI = uri.AbsoluteUri;
                    BaseWebApiURIErrorMessage = "";
                    if (RegisteredHostId.Length > 0 && ValidatedBaseWebApiURI == RegisteredBaseWebApiURI)
                    {
                        HostIdRegistrationMessage = "";
                        RegisterCommand.IsEnabled = false;
                    }
                    else
                    {
                        HostIdRegistrationMessage = "Not registered";
                        RegisterCommand.IsEnabled = true;
                    }
                }
                else
                {
                    ValidatedBaseWebApiURI = "";
                    HostIdRegistrationMessage = "";
                    BaseWebApiURIErrorMessage = "URI not absolute";
                    RegisterCommand.IsEnabled = false;
                }
            }
            else
            {
                ValidatedBaseWebApiURI = "";
                BaseWebApiURIErrorMessage = "Invalid URI";
                RegisterCommand.IsEnabled = false;
                HostIdRegistrationMessage = "";
            }
        }

        /// <summary>
        /// Converts null strings to empty string
        /// </summary>
        /// <param name="baseValue">The source value.</param>
        /// <returns>The coerced value.</returns>
        public static string CoerceNonNull(string value)
        {
            return (null == value) ? "" : value;
        }

        #endregion

        #region BaseWebApiURIErrorMessage Property Members

        /// <summary>
        /// Defines the name for the <see cref="BaseWebApiURIErrorMessage"/> dependency property.
        /// </summary>
        public const string PropertyName_BaseWebApiURIErrorMessage = "BaseWebApiURIErrorMessage";

        private static readonly DependencyPropertyKey BaseWebApiURIErrorMessagePropertyKey = DependencyProperty.RegisterReadOnly(PropertyName_BaseWebApiURIErrorMessage, typeof(string),
            typeof(AppSettingsViewModel), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="BaseWebApiURIErrorMessage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty BaseWebApiURIErrorMessageProperty = BaseWebApiURIErrorMessagePropertyKey.DependencyProperty;

        /// <summary>
        /// Path to folder which contains filesystem resources
        /// </summary>
        public string BaseWebApiURIErrorMessage
        {
            get { return GetValue(BaseWebApiURIErrorMessageProperty) as string; }
            private set { SetValue(BaseWebApiURIErrorMessagePropertyKey, value); }
        }

        #endregion

        #region ValidatedBaseWebApiURI Property Members

        /// <summary>
        /// Defines the name for the <see cref="ValidatedBaseWebApiURI"/> dependency property.
        /// </summary>
        public const string PropertyName_ValidatedBaseWebApiURI = "ValidatedBaseWebApiURI";

        private static readonly DependencyPropertyKey ValidatedBaseWebApiURIPropertyKey = DependencyProperty.RegisterReadOnly(PropertyName_ValidatedBaseWebApiURI, typeof(string),
            typeof(AppSettingsViewModel), new PropertyMetadata("",
                    (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as AppSettingsViewModel).ValidatedBaseWebApiURI_PropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Identifies the <see cref="ValidatedBaseWebApiURI"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValidatedBaseWebApiURIProperty = ValidatedBaseWebApiURIPropertyKey.DependencyProperty;

        /// <summary>
        /// Path to folder which contains filesystem resources
        /// </summary>
        public string ValidatedBaseWebApiURI
        {
            get { return GetValue(ValidatedBaseWebApiURIProperty) as string; }
            private set { SetValue(ValidatedBaseWebApiURIPropertyKey, value); }
        }

        private void ValidatedBaseWebApiURI_PropertyChanged(string oldValue, string newValue)
        {
            if (_noSaveOnChange)
                return;
            Properties.Settings.Default.BaseWebApiURI = newValue;
            SavePropertiesAsync();
        }

        #endregion

        #region HostIdRegistrationMessage Property Members

        /// <summary>
        /// Defines the name for the <see cref="HostIdRegistrationMessage"/> dependency property.
        /// </summary>
        public const string PropertyName_HostIdRegistrationMessage = "HostIdRegistrationMessage";

        private static readonly DependencyPropertyKey HostIdRegistrationMessagePropertyKey = DependencyProperty.RegisterReadOnly(PropertyName_HostIdRegistrationMessage, typeof(string),
            typeof(AppSettingsViewModel), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="HostIdRegistrationMessage"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HostIdRegistrationMessageProperty = HostIdRegistrationMessagePropertyKey.DependencyProperty;

        /// <summary>
        /// Path to folder which contains filesystem resources
        /// </summary>
        public string HostIdRegistrationMessage
        {
            get { return GetValue(HostIdRegistrationMessageProperty) as string; }
            private set { SetValue(HostIdRegistrationMessagePropertyKey, value); }
        }

        #endregion

        #region RegisteredBaseWebApiURI Property Members

        /// <summary>
        /// Defines the name for the <see cref="RegisteredBaseWebApiURI"/> dependency property.
        /// </summary>
        public const string PropertyName_RegisteredBaseWebApiURI = "RegisteredBaseWebApiURI";

        private static readonly DependencyPropertyKey RegisteredBaseWebApiURIPropertyKey = DependencyProperty.RegisterReadOnly(PropertyName_RegisteredBaseWebApiURI, typeof(string),
            typeof(AppSettingsViewModel), new PropertyMetadata(""));

        /// <summary>
        /// Identifies the <see cref="RegisteredBaseWebApiURI"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RegisteredBaseWebApiURIProperty = RegisteredBaseWebApiURIPropertyKey.DependencyProperty;

        /// <summary>
        /// Path to folder which contains filesystem resources
        /// </summary>
        public string RegisteredBaseWebApiURI
        {
            get { return GetValue(RegisteredBaseWebApiURIProperty) as string; }
            private set { SetValue(RegisteredBaseWebApiURIPropertyKey, value); }
        }

        #endregion

        #region RegisteredHostId Property Members

        /// <summary>
        /// Defines the name for the <see cref="RegisteredHostId"/> dependency property.
        /// </summary>
        public const string PropertyName_RegisteredHostId = "RegisteredHostId";

        private static readonly DependencyPropertyKey RegisteredHostIdPropertyKey = DependencyProperty.RegisterReadOnly(PropertyName_RegisteredHostId, typeof(string),
            typeof(AppSettingsViewModel), new PropertyMetadata("",
                    (DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as AppSettingsViewModel).RegisteredHostId_PropertyChanged(e.OldValue as string, e.NewValue as string)));

        /// <summary>
        /// Identifies the <see cref="RegisteredHostId"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty RegisteredHostIdProperty = RegisteredHostIdPropertyKey.DependencyProperty;

        /// <summary>
        /// Path to folder which contains filesystem resources
        /// </summary>
        public string RegisteredHostId
        {
            get { return GetValue(RegisteredHostIdProperty) as string; }
            private set { SetValue(RegisteredHostIdPropertyKey, value); }
        }

        private void RegisteredHostId_PropertyChanged(string oldValue, string newValue)
        {
            if (_noSaveOnChange)
                return;
            Properties.Settings.Default.RegisteredHostId = newValue;
            SavePropertiesAsync();
        }

        #endregion

        #region Register Command Property Members

        public Commands.RelayCommand RegisterCommand { get; }

        protected virtual void OnRegister(object parameter)
        {
            // TODO: Invoke web API to register
            HostIdRegistrationMessage = "Registration not implemented";
        }

        #endregion

        public AppSettingsViewModel()
        {
            RegisterCommand = new Commands.RelayCommand(OnRegister, false, true);
            RawBaseWebApiURI = Properties.Settings.Default.BaseWebApiURI;
            RegisteredHostId = Properties.Settings.Default.RegisteredHostId;
            _noSaveOnChange = false;
        }

        private void SavePropertiesAsync()
        {
            lock (_syncRoot)
            {
                if (_tokenSource != null && !_tokenSource.IsCancellationRequested && !_saveSettingsTask.IsCompleted)
                    _tokenSource.Cancel();
                _tokenSource = new CancellationTokenSource();
                CancellationToken token = _tokenSource.Token;
                _saveSettingsTask = Task.Factory.StartNew(o =>
                {
                    CancellationToken t = (CancellationToken)o;
                    if (t.WaitHandle.WaitOne(new TimeSpan(0, 0, 5)))
                        return;

                    lock (_syncRoot)
                    {
                        if (!t.IsCancellationRequested)
                            Properties.Settings.Default.Save();
                    }
                }, token, token);
                _saveSettingsTask.ContinueWith((t, o) =>
                {
                    lock (_syncRoot)
                    {
                        if (_tokenSource != null && ReferenceEquals(o, _tokenSource))
                            _tokenSource = null;
                    }
                    (o as CancellationTokenSource).Dispose();
                }, _tokenSource);
            }
        }

    }
}
