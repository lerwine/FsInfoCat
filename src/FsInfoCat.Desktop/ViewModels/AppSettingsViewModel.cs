using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Principal;
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
using FsInfoCat.Models.DB;
using FsInfoCat.Models.HostDevices;
using FsInfoCat.Desktop.Commands;

namespace FsInfoCat.Desktop.ViewModels
{
    public class AppSettingsViewModel : DependencyObject
    {
        private Task _saveSettingsTask = null;
        private bool _saveOnChange = false;
        private object _syncRoot = new object();
        private CancellationTokenSource _tokenSource = null;

        #region "Is registered with web API" (IsRegisteredWithWebApi) Property Members

        /// <summary>
        /// Defines the name for the <see cref="IsRegisteredWithWebApi" /> dependency property.
        /// </summary>
        public const string PropertyName_IsRegisteredWithWebApi = "IsRegisteredWithWebApi";

        private static readonly DependencyPropertyKey IsRegisteredWithWebApiPropertyKey = DependencyProperty.RegisterReadOnly(PropertyName_IsRegisteredWithWebApi, typeof(bool), typeof(AppSettingsViewModel),
                new PropertyMetadata(false, null, ViewModelHelper.CoerceAsBoolean));

        /// <summary>
        /// Identifies the <see cref="IsRegisteredWithWebApi" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsRegisteredWithWebApiProperty = IsRegisteredWithWebApiPropertyKey.DependencyProperty;

        public event EventHandler<VmPropertyChangedEventArgs<bool, Uri, Guid>> IsRegisteredWithWebApiChanged;

        private void OnWebApiValidationChanged(Uri validatedUri, Uri registeredUri, Guid registeredHostId, bool isBusy)
        {
            bool isEmptyMessage;
            bool raiseEvent;
            bool isRegistered;
            bool wasRegistered = IsRegisteredWithWebApi;

            if (null == registeredUri)
            {
                raiseEvent = wasRegistered;
                isRegistered = false;
                isEmptyMessage = null == validatedUri;
                RegisterCommand.IsEnabled = !(isEmptyMessage || isBusy);
            }
            else
            {
                isRegistered = isEmptyMessage = !registeredHostId.Equals(Guid.Empty);
                if (null != validatedUri && validatedUri.AbsoluteUri != registeredUri.AbsoluteUri)
                {
                    RegisterCommand.IsEnabled = !isBusy;
                    raiseEvent = true;
                }
                else
                {
                    raiseEvent = isRegistered != wasRegistered;
                    RegisterCommand.IsEnabled = false;
                }
            }
            IsRegisteredWithWebApi = isRegistered;
            HostIdRegistrationMessage = (isEmptyMessage) ? "" : "Not registered";
            if (raiseEvent)
            {
                EventHandler<VmPropertyChangedEventArgs<bool, Uri, Guid>> eventHandler = IsRegisteredWithWebApiChanged;
                if (null != eventHandler)
                    eventHandler.Invoke(this, new VmPropertyChangedEventArgs<bool, Uri, Guid>(IsRegisteredWithWebApiProperty, wasRegistered, isRegistered, RegisteredBaseWebApiUri, RegisteredHostId));
            }

        }

        /// <summary>
        /// Is registered with web API
        /// </summary>
        public bool IsRegisteredWithWebApi
        {
            get
            {
                if (CheckAccess())
                    return (bool)(GetValue(IsRegisteredWithWebApiProperty));
                return Dispatcher.Invoke(() => (bool)(GetValue(IsRegisteredWithWebApiProperty)));
            }
            private set
            {
                if (CheckAccess())
                    SetValue(IsRegisteredWithWebApiPropertyKey, value);
                else
                    Dispatcher.Invoke(() => SetValue(IsRegisteredWithWebApiPropertyKey, value));
            }
        }

        #endregion

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
                    ViewModelHelper.CoerceAsString
            )
        );

        private void OnRawBaseWebApiUriPropertyChanged(string oldValue, string newValue)
        {
            Uri newBaseWebApiUri;
            string errorMessage = "";
            if (string.IsNullOrWhiteSpace(newValue))
            {
                errorMessage = "Not Provided";
                newBaseWebApiUri = null;
            }
            else if (Uri.TryCreate(newValue, UriKind.RelativeOrAbsolute, out newBaseWebApiUri))
            {
                if (newBaseWebApiUri.IsAbsoluteUri)
                {
                    if (!string.IsNullOrEmpty(newBaseWebApiUri.Fragment))
                    {
                        if (newBaseWebApiUri.Fragment == "#")
                        {
                            UriBuilder uriBuilder = new UriBuilder(newBaseWebApiUri);
                            uriBuilder.Fragment = null;
                            newBaseWebApiUri = uriBuilder.Uri;
                        }
                        else
                        {
                            errorMessage = "URI cannot have a fragment (" + newBaseWebApiUri.Fragment + ")";
                            newBaseWebApiUri = null;
                        }
                    }
                    if (null != newBaseWebApiUri)
                    {
                        if (string.IsNullOrEmpty(newBaseWebApiUri.PathAndQuery))
                        {
                            UriBuilder uriBuilder = new UriBuilder(newBaseWebApiUri);
                            uriBuilder.Path = "/";
                            newBaseWebApiUri = uriBuilder.Uri;
                        }
                        else
                        {
                            if (newBaseWebApiUri.Query == "?")
                            {
                                UriBuilder uriBuilder = new UriBuilder(newBaseWebApiUri);
                                uriBuilder.Query = null;
                                newBaseWebApiUri = uriBuilder.Uri;
                            }
                            else if (!string.IsNullOrEmpty(newBaseWebApiUri.Query))
                            {
                                errorMessage = "URI cannot have a fragment (" + newBaseWebApiUri.Fragment + ")";
                                newBaseWebApiUri = null;
                            }
                            if (null != newBaseWebApiUri && newBaseWebApiUri.PathAndQuery != "/")
                            {
                                errorMessage = "URI must not contain a relative path (" + newBaseWebApiUri.PathAndQuery + ")";
                                newBaseWebApiUri = null;
                            }
                        }
                        if (null != newBaseWebApiUri)
                        {
                            string scheme = newBaseWebApiUri.Scheme;
                            if (scheme != Uri.UriSchemeHttps && scheme != Uri.UriSchemeHttp && scheme != Uri.UriSchemeNetPipe && scheme != Uri.UriSchemeNetTcp)
                            {
                                if (String.Equals(scheme, Uri.UriSchemeHttps, StringComparison.InvariantCultureIgnoreCase))
                                    scheme = Uri.UriSchemeHttps;
                                else if (String.Equals(scheme, Uri.UriSchemeHttp, StringComparison.InvariantCultureIgnoreCase))
                                    scheme = Uri.UriSchemeHttp;
                                else if (String.Equals(scheme, Uri.UriSchemeNetPipe, StringComparison.InvariantCultureIgnoreCase))
                                    scheme = Uri.UriSchemeNetPipe;
                                else if (String.Equals(scheme, Uri.UriSchemeNetTcp, StringComparison.InvariantCultureIgnoreCase))
                                    scheme = Uri.UriSchemeNetTcp;
                                else
                                {
                                    errorMessage = "Unsupported scheme \"" + scheme + "\"";
                                    newBaseWebApiUri = null;
                                }
                                if (null != newBaseWebApiUri)
                                {
                                    UriBuilder uriBuilder = new UriBuilder(newBaseWebApiUri);
                                    uriBuilder.Scheme = scheme;
                                    newBaseWebApiUri = uriBuilder.Uri;
                                }
                            }
                        }
                    }
                }
                else
                {
                    errorMessage = "URI not absolute";
                    newBaseWebApiUri = null;
                }
            }
            else
            {
                errorMessage = "Invalid URI";
                newBaseWebApiUri = null;
            }

            ValidatedBaseWebApiUri = newBaseWebApiUri;
            BaseWebApiUriErrorMessage = errorMessage;

            if (null != newBaseWebApiUri)
            {
                Uri oldValidatedBaseWebApiUri = ValidatedBaseWebApiUri;
                if (null == oldValidatedBaseWebApiUri || oldValidatedBaseWebApiUri.AbsoluteUri != newBaseWebApiUri.AbsoluteUri || RegisteredHostId.Equals(Guid.Empty))
                {
                    RegisterCommand.IsEnabled = true;
                    HostIdRegistrationMessage = "Not Registered";
                    return;
                }
            }
            RegisterCommand.IsEnabled = false;
            HostIdRegistrationMessage = "";
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
                new PropertyMetadata("", null, ViewModelHelper.CoerceAsString));

        /// <summary>
        /// Identifies the <see cref="BaseWebApiUriErrorMessage" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty BaseWebApiUriErrorMessageProperty = BaseWebApiUriErrorMessagePropertyKey.DependencyProperty;

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

        private static readonly DependencyPropertyKey ValidatedBaseWebApiUriPropertyKey = DependencyProperty.RegisterReadOnly(PropertyName_ValidatedBaseWebApiUri, typeof(Uri), typeof(AppSettingsViewModel),
                new PropertyMetadata("",
                    (d, e) => (d as AppSettingsViewModel).OnValidatedBaseWebApiUriPropertyChanged(e.OldValue as Uri, e.NewValue as Uri)
            )
        );

        /// <summary>
        /// Identifies the <see cref="ValidatedBaseWebApiUri" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty ValidatedBaseWebApiUriProperty = ValidatedBaseWebApiUriPropertyKey.DependencyProperty;

        private void OnValidatedBaseWebApiUriPropertyChanged(Uri oldValue, Uri newValue)
        {
            string uriString = Properties.Settings.Default.BaseWebApiURI;
            if (string.IsNullOrWhiteSpace(uriString) || !Uri.TryCreate(uriString.Trim(), UriKind.Absolute, out Uri settingsUri))
                settingsUri = null;
            if (null == newValue)
            {
                if (null == settingsUri)
                    return;
                Properties.Settings.Default.BaseWebApiURI = "";
            }
            else
            {
                if (null != settingsUri && settingsUri.AbsoluteUri == newValue.AbsoluteUri)
                    return;
                Properties.Settings.Default.BaseWebApiURI = newValue.AbsoluteUri;
            }
            if (_saveOnChange)
                SavePropertiesAsync();
            OnWebApiValidationChanged(newValue, RegisteredBaseWebApiUri, RegisteredHostId, IsBusy);
        }

        /// <summary>
        /// Validated base Web API URI
        /// </summary>
        public Uri ValidatedBaseWebApiUri
        {
            get
            {
                if (CheckAccess())
                    return (Uri)(GetValue(ValidatedBaseWebApiUriProperty));
                return Dispatcher.Invoke(() => (Uri)(GetValue(ValidatedBaseWebApiUriProperty)));
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
                new PropertyMetadata("", null, ViewModelHelper.CoerceAsString));

        /// <summary>
        /// Identifies the <see cref="HostIdRegistrationMessage" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty HostIdRegistrationMessageProperty = HostIdRegistrationMessagePropertyKey.DependencyProperty;

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

        private static readonly DependencyPropertyKey RegisteredBaseWebApiUriPropertyKey = DependencyProperty.RegisterReadOnly(PropertyName_RegisteredBaseWebApiUri, typeof(Uri), typeof(AppSettingsViewModel),
                new PropertyMetadata(null,
                    (d, e) => (d as AppSettingsViewModel).OnRegisteredBaseWebApiUriPropertyChanged(e.OldValue as Uri, e.NewValue as Uri)
                )
            );

        public event EventHandler<VmPropertyChangedEventArgs<Uri>> RegisteredBaseWebApiUriChanged;

        private void OnRegisteredBaseWebApiUriPropertyChanged(Uri oldValue, Uri newValue)
        {
            string uriString = Properties.Settings.Default.BaseWebApiURI;
            if (string.IsNullOrWhiteSpace(uriString) || !Uri.TryCreate(uriString, UriKind.Absolute, out Uri settingsUri))
                settingsUri = null;
            if (null == newValue)
            {
                if (null == settingsUri)
                    return;
                Properties.Settings.Default.BaseWebApiURI = "";
            }
            else if (null == settingsUri || settingsUri.AbsoluteUri != newValue.AbsoluteUri)
                Properties.Settings.Default.BaseWebApiURI = newValue.AbsoluteUri;
            else
                return;
            if (_saveOnChange)
                SavePropertiesAsync();

            OnWebApiValidationChanged(ValidatedBaseWebApiUri, newValue, RegisteredHostId, IsBusy);
            OnWebApiValidationChanged(ValidatedBaseWebApiUri, RegisteredBaseWebApiUri, RegisteredHostId, IsBusy);

            EventHandler<VmPropertyChangedEventArgs<Uri>> eventHandler = RegisteredBaseWebApiUriChanged;
            if (null != eventHandler)
                eventHandler.Invoke(this, new VmPropertyChangedEventArgs<Uri>(RegisteredBaseWebApiUriProperty, settingsUri, newValue));
        }

        /// <summary>
        /// Identifies the <see cref="RegisteredBaseWebApiUri" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty RegisteredBaseWebApiUriProperty = RegisteredBaseWebApiUriPropertyKey.DependencyProperty;

        /// <summary>
        /// Registered base web API URI
        /// </summary>
        public Uri RegisteredBaseWebApiUri
        {
            get
            {
                if (CheckAccess())
                    return GetValue(RegisteredBaseWebApiUriProperty) as Uri;
                return Dispatcher.Invoke(() => GetValue(RegisteredBaseWebApiUriProperty) as Uri);
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

        private static readonly DependencyPropertyKey RegisteredHostIdPropertyKey = DependencyProperty.RegisterReadOnly(PropertyName_RegisteredHostId, typeof(Guid), typeof(AppSettingsViewModel),
                new PropertyMetadata(Guid.Empty,
                    (d, e) => (d as AppSettingsViewModel).OnRegisteredHostIdPropertyChanged(e.OldValue as Guid?, (Guid)e.NewValue),
                    ViewModelHelper.CoerceAsGuid
            )
        );

        /// <summary>
        /// Identifies the <see cref="RegisteredHostId" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty RegisteredHostIdProperty = RegisteredHostIdPropertyKey.DependencyProperty;

        public event EventHandler<VmPropertyChangedEventArgs<Guid>> RegisteredHostIdPropertyChanged;

        private void OnRegisteredHostIdPropertyChanged(Guid? oldValue, Guid newValue)
        {
            string guidString = Properties.Settings.Default.RegisteredHostId;
            if (string.IsNullOrWhiteSpace(guidString) || !Guid.TryParse(guidString.Trim(), out Guid settingsGuid))
                settingsGuid = Guid.Empty;
            if (newValue.Equals(settingsGuid))
                return;
            Properties.Settings.Default.RegisteredHostId = (newValue.Equals(Guid.Empty)) ? "" : newValue.ToString("d");
            if (_saveOnChange)
                SavePropertiesAsync();
            EventHandler<VmPropertyChangedEventArgs<Guid>> eventHandler = RegisteredHostIdPropertyChanged;
            if (null != eventHandler)
                eventHandler.Invoke(this, new VmPropertyChangedEventArgs<Guid>(RegisteredHostIdProperty, settingsGuid, newValue));
            OnWebApiValidationChanged(ValidatedBaseWebApiUri, RegisteredBaseWebApiUri, newValue, IsBusy);
        }

        /// <summary>
        /// Registered host ID
        /// </summary>
        public Guid RegisteredHostId
        {
            get
            {
                if (CheckAccess())
                    return (Guid)(GetValue(RegisteredHostIdProperty));
                return Dispatcher.Invoke(() => (Guid)(GetValue(RegisteredHostIdProperty)));
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

        #region "Is busy" (IsBusy) Property Members

        /// <summary>
        /// Defines the name for the <see cref="IsBusy" /> dependency property.
        /// </summary>
        public const string PropertyName_IsBusy = "IsBusy";

        private static readonly DependencyPropertyKey IsBusyPropertyKey = DependencyProperty.RegisterReadOnly(PropertyName_IsBusy, typeof(bool), typeof(AppSettingsViewModel),
                new PropertyMetadata(false,
                    (d, e) => (d as AppSettingsViewModel).OnIsBusyPropertyChanged(e.OldValue as bool?, (bool)(e.NewValue)),
                    ViewModelHelper.CoerceAsBoolean
            )
        );

        /// <summary>
        /// Identifies the <see cref="IsBusy" /> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsBusyProperty = IsBusyPropertyKey.DependencyProperty;

        private void OnIsBusyPropertyChanged(bool? oldValue, bool newValue)
        {
            OnWebApiValidationChanged(ValidatedBaseWebApiUri, RegisteredBaseWebApiUri, RegisteredHostId, newValue);
        }

        /// <summary>
        /// Is busy
        /// </summary>
        public bool IsBusy
        {
            get
            {
                if (CheckAccess())
                    return (bool)(GetValue(IsBusyProperty));
                return Dispatcher.Invoke(() => (bool)(GetValue(IsBusyProperty)));
            }
            private set
            {
                if (CheckAccess())
                    SetValue(IsBusyPropertyKey, value);
                else
                    Dispatcher.Invoke(() => SetValue(IsBusyPropertyKey, value));
            }
        }

        #endregion

        #region Register Command Property Members

        public Commands.RelayCommand RegisterCommand { get; }

        private Task<UrlAndID> RegisterAsync()
        {
            IsBusy = true;
            Task<UrlAndID> t = Task.Factory.StartNew<UrlAndID>(() =>
            {
                Uri uri = ValidatedBaseWebApiUri;
                if (null == uri)
                    return null;

                HostDeviceRegRequest request = HostDeviceRegRequest.CreateForLocal();
                UriBuilder builder = new UriBuilder(uri);
                builder.Path = "/api/HostDevice/Register";
                JsonSerializerOptions options = new JsonSerializerOptions();
                JsonContent content = JsonContent.Create<HostDeviceRegRequest>(request);
                HttpResponseMessage response = ((App)App.Current).HttpClient.PostAsJsonAsync(builder.Uri, content).Result;
                RequestResponse<HostDevice> result = response.Content.ReadFromJsonAsync<RequestResponse<HostDevice>>().Result;
                if (null == result)
                    HostIdRegistrationMessage = "Registration failed";
                else
                {
                    if (result.Success)
                    {
                        RegisteredBaseWebApiUri = uri;
                        RegisteredHostId = result.Result.HostDeviceID;
                        HostIdRegistrationMessage = (string.IsNullOrWhiteSpace(result.Message)) ? "Registration successful" : result.Message;
                        return new UrlAndID(uri, result.Result.HostDeviceID);
                    }
                    HostIdRegistrationMessage = (string.IsNullOrWhiteSpace(result.Message)) ? "Registration failed" : result.Message;
                }
                return null;
            });
            t.ContinueWith(task =>
            {
                if (task.IsFaulted)
                    HostIdRegistrationMessage = "Registration failed: " + ((string.IsNullOrWhiteSpace(task.Exception.Message)) ? task.Exception.GetType().Name : task.Exception.Message);
                else if (!task.IsCompletedSuccessfully)
                    HostIdRegistrationMessage = "Registration canceled";
                IsBusy = false;
            });
            return t;
        }

        protected virtual void OnRegister(object parameter)
        {
            RegisterAsync();
        }

        #endregion

        public AppSettingsViewModel()
        {
            RegisterCommand = new Commands.RelayCommand(OnRegister, false, true);
            string s = Properties.Settings.Default.RegisteredHostId;
            RegisteredHostId = (string.IsNullOrWhiteSpace(s) || !Guid.TryParse(s.Trim(), out Guid id)) ? Guid.Empty : id;
            RawBaseWebApiUri = Properties.Settings.Default.BaseWebApiURI;
            RegisteredBaseWebApiUri = ValidatedBaseWebApiUri;
            _saveOnChange = true;
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

        private Task<UrlAndID> GetRemoteHostUri()
        {
            Uri uri = RegisteredBaseWebApiUri;
            Guid id = RegisteredHostId;
            if (null == uri || id.Equals(Guid.Empty))
            {
                if (null != (uri = ValidatedBaseWebApiUri))
                    return RegisterAsync();
                return Task.FromResult<UrlAndID>(null);
            }
            return Task.FromResult(new UrlAndID(uri, id));
        }

    }
}
