using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
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
using FsInfoCat.Models;

namespace FsInfoCat.Desktop.ViewModels
{
    public class AppSettingsViewModel : DependencyObject
    {
        private Task _saveSettingsTask = null;
        private bool _noSaveOnChange = true;
        private object _syncRoot = new object();
        private CancellationTokenSource _tokenSource = null;

        #region "Raw base web API URI" (RawBaseWebApiUri) Property Members

        /// <summary>
        /// Defines the name for the <see cref="RawBaseWebApiUri" /> dependency property.
        /// </summary>
        public const string PropertyName_RawBaseWebApiUri = "RawBaseWebApiUri";

        /// <summary>
        /// Identifies the <see cref="RawBaseWebApiUri" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty RawBaseWebApiUriProperty = DependencyProperty.Register(PropertyName_RawBaseWebApiUri, typeof(string), typeof(AppSettingsViewModel),
                new PropertyMetadata("",
                    (d, e) => (d as AppSettingsViewModel).OnRawBaseWebApiUriPropertyChanged(e.OldValue as string, e.NewValue as string),
                    (d, baseValue) => CoerceRawBaseWebApiUriValue(baseValue as string)
            )
        );

        private void OnRawBaseWebApiUriPropertyChanged(string oldValue, string newValue)
        {
            if (string.IsNullOrWhiteSpace(newValue))
            {
                ValidatedBaseWebApiUri = "";
                BaseWebApiUriErrorMessage = "Not Provided";
                RegisterCommand.IsEnabled = false;
                HostIdRegistrationMessage = "";
            }
            else if (Uri.TryCreate(newValue, UriKind.RelativeOrAbsolute, out Uri uri))
            {
                if (uri.IsAbsoluteUri)
                {
                    ValidatedBaseWebApiUri = uri.AbsoluteUri;
                    BaseWebApiUriErrorMessage = "";
                    if (RegisteredHostId.Length > 0 && ValidatedBaseWebApiUri == RegisteredBaseWebApiUri)
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
                    ValidatedBaseWebApiUri = "";
                    HostIdRegistrationMessage = "";
                    BaseWebApiUriErrorMessage = "URI not absolute";
                    RegisterCommand.IsEnabled = false;
                }
            }
            else
            {
                ValidatedBaseWebApiUri = "";
                BaseWebApiUriErrorMessage = "Invalid URI";
                RegisterCommand.IsEnabled = false;
                HostIdRegistrationMessage = "";
            }
        }

        public static string CoerceRawBaseWebApiUriValue(string value)
        {
            if (null == value)
                return "";
            return value;
        }

        /// <summary>
        /// Raw base web API URI
        /// </summary>
        public string RawBaseWebApiUri
        {
            get
            {
                if (CheckAccess())
                    return (string)(GetValue(RawBaseWebApiUriProperty));
                return Dispatcher.Invoke(() => (string)(GetValue(RawBaseWebApiUriProperty)));
            }
            set
            {
                if (CheckAccess())
                    SetValue(RawBaseWebApiUriProperty, value);
                else
                    Dispatcher.Invoke(() => SetValue(RawBaseWebApiUriProperty, value));
            }
        }

        #endregion

        #region "Base web API URI error message" (BaseWebApiUriErrorMessage) Property Members

        /// <summary>
        /// Defines the name for the <see cref="BaseWebApiUriErrorMessage" /> dependency property.
        /// </summary>
        public const string PropertyName_BaseWebApiUriErrorMessage = "BaseWebApiUriErrorMessage";

        private static readonly DependencyPropertyKey BaseWebApiUriErrorMessagePropertyKey = DependencyProperty.RegisterReadOnly(PropertyName_BaseWebApiUriErrorMessage, typeof(string), typeof(AppSettingsViewModel),
                new PropertyMetadata("", null,
                    (d, baseValue) => CoerceBaseWebApiUriErrorMessageValue(baseValue as string)
            )
        );

        /// <summary>
        /// Identifies the <see cref="BaseWebApiUriErrorMessage" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty BaseWebApiUriErrorMessageProperty = BaseWebApiUriErrorMessagePropertyKey.DependencyProperty;

        public static string CoerceBaseWebApiUriErrorMessageValue(string value)
        {
            if (null == value)
                return "";
            return value;
        }

        /// <summary>
        /// Base web API URI error message
        /// </summary>
        public string BaseWebApiUriErrorMessage
        {
            get
            {
                if (CheckAccess())
                    return (string)(GetValue(BaseWebApiUriErrorMessageProperty));
                return Dispatcher.Invoke(() => (string)(GetValue(BaseWebApiUriErrorMessageProperty)));
            }
            private set
            {
                if (CheckAccess())
                    SetValue(BaseWebApiUriErrorMessagePropertyKey, value);
                else
                    Dispatcher.Invoke(() => SetValue(BaseWebApiUriErrorMessagePropertyKey, value));
            }
        }

        #endregion

        #region "Validated base Web API URI" (ValidatedBaseWebApiUri) Property Members

        /// <summary>
        /// Defines the name for the <see cref="ValidatedBaseWebApiUri" /> dependency property.
        /// </summary>
        public const string PropertyName_ValidatedBaseWebApiUri = "ValidatedBaseWebApiUri";

        private static readonly DependencyPropertyKey ValidatedBaseWebApiUriPropertyKey = DependencyProperty.RegisterReadOnly(PropertyName_ValidatedBaseWebApiUri, typeof(string), typeof(AppSettingsViewModel),
                new PropertyMetadata("",
                    (d, e) => (d as AppSettingsViewModel).OnValidatedBaseWebApiUriPropertyChanged(e.OldValue as string, e.NewValue as string),
                    (d, baseValue) => CoerceValidatedBaseWebApiUriValue(baseValue as string)
            )
        );

        /// <summary>
        /// Identifies the <see cref="ValidatedBaseWebApiUri" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValidatedBaseWebApiUriProperty = ValidatedBaseWebApiUriPropertyKey.DependencyProperty;

        private void OnValidatedBaseWebApiUriPropertyChanged(string oldValue, string newValue)
        {
            if (_noSaveOnChange)
                return;
            Properties.Settings.Default.BaseWebApiURI = newValue;
            SavePropertiesAsync();
        }

        public static string CoerceValidatedBaseWebApiUriValue(string value)
        {
            if (null == value)
                return "";
            return value;
        }

        /// <summary>
        /// Validated base Web API URI
        /// </summary>
        public string ValidatedBaseWebApiUri
        {
            get
            {
                if (CheckAccess())
                    return (string)(GetValue(ValidatedBaseWebApiUriProperty));
                return Dispatcher.Invoke(() => (string)(GetValue(ValidatedBaseWebApiUriProperty)));
            }
            private set
            {
                if (CheckAccess())
                    SetValue(ValidatedBaseWebApiUriPropertyKey, value);
                else
                    Dispatcher.Invoke(() => SetValue(ValidatedBaseWebApiUriPropertyKey, value));
            }
        }

        #endregion

        #region "Host ID registration message" (HostIdRegistrationMessage) Property Members

        /// <summary>
        /// Defines the name for the <see cref="HostIdRegistrationMessage" /> dependency property.
        /// </summary>
        public const string PropertyName_HostIdRegistrationMessage = "HostIdRegistrationMessage";

        private static readonly DependencyPropertyKey HostIdRegistrationMessagePropertyKey = DependencyProperty.RegisterReadOnly(PropertyName_HostIdRegistrationMessage, typeof(string), typeof(AppSettingsViewModel),
                new PropertyMetadata("", null,
                    (d, baseValue) => CoerceHostIdRegistrationMessageValue(baseValue as string)
            )
        );

        /// <summary>
        /// Identifies the <see cref="HostIdRegistrationMessage" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty HostIdRegistrationMessageProperty = HostIdRegistrationMessagePropertyKey.DependencyProperty;

        public static string CoerceHostIdRegistrationMessageValue(string value)
        {
            if (null == value)
                return "";
            return value;
        }

        /// <summary>
        /// Host ID registration message
        /// </summary>
        public string HostIdRegistrationMessage
        {
            get
            {
                if (CheckAccess())
                    return (string)(GetValue(HostIdRegistrationMessageProperty));
                return Dispatcher.Invoke(() => (string)(GetValue(HostIdRegistrationMessageProperty)));
            }
            private set
            {
                if (CheckAccess())
                    SetValue(HostIdRegistrationMessagePropertyKey, value);
                else
                    Dispatcher.Invoke(() => SetValue(HostIdRegistrationMessagePropertyKey, value));
            }
        }

        #endregion

        #region "Registered base web API URI" (RegisteredBaseWebApiUri) Property Members

        /// <summary>
        /// Defines the name for the <see cref="RegisteredBaseWebApiUri" /> dependency property.
        /// </summary>
        public const string PropertyName_RegisteredBaseWebApiUri = "RegisteredBaseWebApiUri";

        private static readonly DependencyPropertyKey RegisteredBaseWebApiUriPropertyKey = DependencyProperty.RegisterReadOnly(PropertyName_RegisteredBaseWebApiUri, typeof(string), typeof(AppSettingsViewModel),
                new PropertyMetadata("", null,
                    (d, baseValue) => CoerceRegisteredBaseWebApiUriValue(baseValue as string)
            )
        );

        /// <summary>
        /// Identifies the <see cref="RegisteredBaseWebApiUri" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty RegisteredBaseWebApiUriProperty = RegisteredBaseWebApiUriPropertyKey.DependencyProperty;

        public static string CoerceRegisteredBaseWebApiUriValue(string value)
        {
            if (null == value)
                return "";
            return value;
        }

        /// <summary>
        /// Registered base web API URI
        /// </summary>
        public string RegisteredBaseWebApiUri
        {
            get
            {
                if (CheckAccess())
                    return (string)(GetValue(RegisteredBaseWebApiUriProperty));
                return Dispatcher.Invoke(() => (string)(GetValue(RegisteredBaseWebApiUriProperty)));
            }
            private set
            {
                if (CheckAccess())
                    SetValue(RegisteredBaseWebApiUriPropertyKey, value);
                else
                    Dispatcher.Invoke(() => SetValue(RegisteredBaseWebApiUriPropertyKey, value));
            }
        }

        #endregion

        #region "Registered host ID" (RegisteredHostId) Property Members

        /// <summary>
        /// Defines the name for the <see cref="RegisteredHostId" /> dependency property.
        /// </summary>
        public const string PropertyName_RegisteredHostId = "RegisteredHostId";

        private static readonly DependencyPropertyKey RegisteredHostIdPropertyKey = DependencyProperty.RegisterReadOnly(PropertyName_RegisteredHostId, typeof(string), typeof(AppSettingsViewModel),
                new PropertyMetadata("",
                    (d, e) => (d as AppSettingsViewModel).OnRegisteredHostIdPropertyChanged(e.OldValue as string, e.NewValue as string),
                    (d, baseValue) => CoerceRegisteredHostIdValue(baseValue as string)
            )
        );

        /// <summary>
        /// Identifies the <see cref="RegisteredHostId" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty RegisteredHostIdProperty = RegisteredHostIdPropertyKey.DependencyProperty;

        private void OnRegisteredHostIdPropertyChanged(string oldValue, string newValue)
        {
            if (_noSaveOnChange)
                return;
            Properties.Settings.Default.RegisteredHostId = newValue;
            SavePropertiesAsync();
        }

        public static string CoerceRegisteredHostIdValue(string value)
        {
            if (null == value)
                return "";
            return value;
        }

        /// <summary>
        /// Registered host ID
        /// </summary>
        public string RegisteredHostId
        {
            get
            {
                if (CheckAccess())
                    return (string)(GetValue(RegisteredHostIdProperty));
                return Dispatcher.Invoke(() => (string)(GetValue(RegisteredHostIdProperty)));
            }
            private set
            {
                if (CheckAccess())
                    SetValue(RegisteredHostIdPropertyKey, value);
                else
                    Dispatcher.Invoke(() => SetValue(RegisteredHostIdPropertyKey, value));
            }
        }

        #endregion

        #region Register Command Property Members

        public Commands.RelayCommand RegisterCommand { get; }

        protected virtual void OnRegister(object parameter)
        {
            Task.Factory.StartNew(() =>
            {
                string uri = ValidatedBaseWebApiUri;
                UriBuilder builder = new UriBuilder(uri);
                builder.Path = "api/MediaHost";
                MediaHostRegRequest hostNameRef = new MediaHostRegRequest();
                hostNameRef.IsWindows = Environment.OSVersion.Platform == PlatformID.Win32NT;
                hostNameRef.MachineName = Environment.MachineName;
                JsonSerializerOptions options = new JsonSerializerOptions();
                JsonContent content = JsonContent.Create<MediaHostRegRequest>(hostNameRef);
                HttpResponseMessage response = ((App)(App.Current)).HttpClient.PostAsJsonAsync(builder.Uri, content).Result;
                MediaHost result = response.Content.ReadFromJsonAsync<MediaHost>().Result;
                if (null != result)
                {
                    RegisteredBaseWebApiUri = uri;
                    RegisteredHostId = result.HostID.ToString("n");
                    HostIdRegistrationMessage = "Registration successful";
                }
                else
                    HostIdRegistrationMessage = "Registration failed";
            }).ContinueWith(task =>
            {
                if (task.IsFaulted)
                    HostIdRegistrationMessage = "Registration failed: " + ((string.IsNullOrWhiteSpace(task.Exception.Message)) ? task.Exception.GetType().Name : task.Exception.Message);
                else if (!task.IsCompletedSuccessfully)
                    HostIdRegistrationMessage = "Registration canceled";
            });
        }

        #endregion

        public AppSettingsViewModel()
        {
            RegisterCommand = new Commands.RelayCommand(OnRegister, false, true);
            RawBaseWebApiUri = Properties.Settings.Default.BaseWebApiURI;
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
