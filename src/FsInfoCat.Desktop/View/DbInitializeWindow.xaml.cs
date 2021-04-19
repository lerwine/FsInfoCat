using FsInfoCat.Desktop.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FsInfoCat.Desktop.View
{
    /// <summary>
    /// Interaction logic for DbInitializeWindow.xaml
    /// </summary>
    public partial class DbInitializeWindow : Window
    {
        private bool _autoUpdateDisplayName = true;

        public DbInitializeWindow()
        {
            InitializeComponent();
            WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
            SecurityIdentifier sid = windowsIdentity.User;
            string accountName = windowsIdentity.Name;
            int index = accountName.IndexOf('\\');  
            if (index < 0)
            {
                DomainTextBox.Text = sid.IsAccountSid() ? sid.AccountDomainSid.ToString() : "";
                AccountNameTextBox.Text = accountName;
            }
            else
            {
                DomainTextBox.Text = accountName.Substring(0, index);
                AccountNameTextBox.Text = accountName.Substring(index + 1);
            }
            SIDTextBox.Text = sid.ToString();
        }

        protected override void OnActivated(EventArgs e)
        {

        }

        internal static bool CheckConfiguration(Func<bool> ifNoSystemAccount, Action<Exception, string> onException)
        {
            DbModel dbContext;
            try { dbContext = new DbModel(); }
            catch (Exception exception)
            {
                onException(exception, $"Error connecting to database: {(string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message)}");
                return false;
            }
            using (dbContext)
            {
                try
                {
                    Guid id = Guid.Empty;
                    if ((from u in dbContext.UserAccounts where u.Id == id select u).Any())
                        return true;
                }
                catch (Exception exception)
                {
                    onException(exception, $"Error reading from to database: {(string.IsNullOrWhiteSpace(exception.Message) ? exception.ToString() : exception.Message)}");
                    return false;
                }
            }
            return ifNoSystemAccount();
        }

        private void WindowsAuthRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            UserNameErrorTextBlock.Visibility = PasswordErrorTextBlock.Visibility = Visibility.Collapsed;
            UserNameTextBox.IsEnabled = PasswordTextBox.IsEnabled = ConfirmTextBox.IsEnabled = false;
            _passwordValid = true;
            ContinueButton.IsEnabled = _lastNameValid;
        }

        private void UserNameRadioButton_Checked(object sender, RoutedEventArgs e)
        {
            UserNameTextBox.IsEnabled = PasswordTextBox.IsEnabled = ConfirmTextBox.IsEnabled = true;
            UserNameErrorTextBlock.Visibility = string.IsNullOrWhiteSpace(UserNameTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
            ValidatePassword();
        }

        private void UserNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UserNameErrorTextBlock.Visibility = string.IsNullOrWhiteSpace(UserNameTextBox.Text) ? Visibility.Visible : Visibility.Collapsed;
        }

        private void PasswordTextBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ValidatePassword();
        }

        private void ValidatePassword()
        {
            SecureString securePassword = PasswordTextBox.SecurePassword;
            if (securePassword.Length == 0)
                PasswordErrorTextBlock.Text = "Password cannot be empty.";
            else if (securePassword.Equals(ConfirmTextBox.SecurePassword))
            {
                IntPtr ptr = Marshal.SecureStringToBSTR(securePassword);
                try
                {
                    if (PwCheckRegex.IsMatch(Marshal.PtrToStringBSTR(ptr)))
                    {
                        _passwordValid = true;
                        ContinueButton.IsEnabled = _lastNameValid;
                        PasswordErrorTextBlock.Visibility = Visibility.Collapsed;
                        return;
                    }
                    PasswordErrorTextBlock.Text = "Password does not meet minimum length and complexity requirements (must contains at least 8 characters and at least 1 upper-case letter, 1 lower-case letter, 1 digit, and 1 symbol).";
                }
                finally { Marshal.FreeBSTR(ptr); }
            }
            else
                PasswordErrorTextBlock.Text = "Password and confirmation do not match.";
            ContinueButton.IsEnabled = _passwordValid = false;
            PasswordErrorTextBlock.Visibility = Visibility.Visible;
        }

        private void TitleTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            AutoUpdateDisplayName();
        }

        private static readonly Regex PwCheckRegex = new Regex(@"^(?=[^A-Z]*[A-Z])(?=[^a-z]*[a-z])(?=\D*\d)(?=[^\p{P}\p{S}]*[\p{P}\p{S}]).{8}", RegexOptions.Compiled);
        private static readonly Regex WsRegex = new Regex(@"( \s+|(?! )\s+)", RegexOptions.Compiled);
        private static bool IsWsNormalizedEmpty(string text, out string wsNormalized)
        {
            if (text is null)
                wsNormalized = "";
            else if ((wsNormalized = text.Trim()).Length > 0)
            {
                if (WsRegex.IsMatch(wsNormalized))
                    wsNormalized = WsRegex.Replace(wsNormalized, " ");
                return false;
            }
            return true;
        }
        private static string AsWsNormalized(string text)
        {
            if (text is null)
                return "";
            return ((text = text.Trim()).Length > 0 && WsRegex.IsMatch(text)) ? WsRegex.Replace(text, " ") : text;
        }
        private bool _lastNameValid = false;
        private bool _passwordValid = false;
        private string _expectedDisplayName = "";
        private void AutoUpdateDisplayName()
        {
            _lastNameValid = !IsWsNormalizedEmpty(LastNameTextBox.Text, out string lastName);
            ContinueButton.IsEnabled = _lastNameValid && _passwordValid;
            if (IsWsNormalizedEmpty(TitleTextBox.Text, out string title))
            {
                if (IsWsNormalizedEmpty(SuffixTextBox.Text, out string suffix))
                {
                    if (IsWsNormalizedEmpty(FirstNameTextBox.Text, out string firstName))
                        _expectedDisplayName = IsWsNormalizedEmpty(MITextBox.Text, out string mi) ? lastName : $"{lastName}, {mi}.";
                    else
                        _expectedDisplayName = IsWsNormalizedEmpty(MITextBox.Text, out string mi) ? $"{lastName}, {firstName}" : $"{lastName}, {firstName} {mi}.";
                }
                if (IsWsNormalizedEmpty(FirstNameTextBox.Text, out string f))
                    _expectedDisplayName = IsWsNormalizedEmpty(MITextBox.Text, out string mi) ? $"{lastName}, {suffix}" : $"{lastName}, {mi}., {suffix}";
                else
                    _expectedDisplayName = IsWsNormalizedEmpty(MITextBox.Text, out string mi) ? $"{lastName}, {f}, {suffix}" : $"{lastName}, {f} {mi}., {suffix}";
            }
            if (IsWsNormalizedEmpty(SuffixTextBox.Text, out string s))
            {
                if (IsWsNormalizedEmpty(FirstNameTextBox.Text, out string firstName))
                    _expectedDisplayName = IsWsNormalizedEmpty(MITextBox.Text, out string mi) ? $"{title} {lastName}" : $"{title} {lastName}, {mi}.";
                else
                    _expectedDisplayName = IsWsNormalizedEmpty(MITextBox.Text, out string mi) ? $"{title} {lastName}, {firstName}" : $"{title} {lastName}, {firstName} {mi}.";
            }
            if (IsWsNormalizedEmpty(FirstNameTextBox.Text, out string n))
                _expectedDisplayName = IsWsNormalizedEmpty(MITextBox.Text, out string mi) ? $"{title} {lastName}, {s}" : $"{title} {lastName}, {mi}., {s}";
            else
                _expectedDisplayName = IsWsNormalizedEmpty(MITextBox.Text, out string mi) ? $"{title} {lastName}, {n}, {s}" : $"{title} {lastName}, {n} {mi}., {s}";
            if (_autoUpdateDisplayName)
                DisplayNameTextBox.Text = _expectedDisplayName;
            else if (string.Equals(_expectedDisplayName, AsWsNormalized(DisplayNameTextBox.Text), StringComparison.InvariantCultureIgnoreCase))
                _autoUpdateDisplayName = true;
        }

        private void LastNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            AutoUpdateDisplayName();
        }

        private void FirstNameTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            AutoUpdateDisplayName();
        }

        private void MITextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            AutoUpdateDisplayName();
        }

        private void SuffixTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            AutoUpdateDisplayName();
        }

        private void DisplayNameTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            _autoUpdateDisplayName = false;
        }

        private void DisplayNameTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (IsWsNormalizedEmpty(DisplayNameTextBox.Text, out string displayName))
            {
                _autoUpdateDisplayName = true;
                AutoUpdateDisplayName();
            }
            else
                _autoUpdateDisplayName = string.Equals(_expectedDisplayName, displayName, StringComparison.InvariantCultureIgnoreCase);
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            bool isWindowsAuth = WindowsAuthRadioButton.IsChecked ?? false;
            string lastName = AsWsNormalized(LastNameTextBox.Text);
            if (!IsWsNormalizedEmpty(FirstNameTextBox.Text, out string firstName))
                firstName = null;
            if (!IsWsNormalizedEmpty(MITextBox.Text, out string mi))
                mi = null;
            if (!IsWsNormalizedEmpty(TitleTextBox.Text, out string title))
                title = null;
            if (!IsWsNormalizedEmpty(SuffixTextBox.Text, out string suffix))
                suffix = null;
            string userName = AsWsNormalized(UserNameTextBox.Text);
            SecureString pw = PasswordTextBox.SecurePassword;
            string displayName = AsWsNormalized(DisplayNameTextBox.Text);
            if (displayName.Length == 0)
            {
                _autoUpdateDisplayName = true;
                AutoUpdateDisplayName();
                displayName = AsWsNormalized(DisplayNameTextBox.Text);
            }
            Task.Factory.StartNew(new Func<bool>(() =>
            {
                using (DbModel dbContext = new DbModel())
                {
                    using (System.Data.Entity.DbContextTransaction transaction = dbContext.Database.BeginTransaction())
                    {
                        try
                        {
                            UserAccount sysAccount = new UserAccount
                            {
                                Id = Guid.Empty,
                                LastName = "Account",
                                FirstName = "System",
                                DisplayName = "System Account",
                                CreatedById = Guid.Empty,
                                CreatedOn = DateTime.Now,
                                ModifiedById = Guid.Empty,
                                ExplicitRoles = UserRole.Administrator
                            };
                            sysAccount.ModifiedOn = sysAccount.CreatedOn;
                            dbContext.UserAccounts.Add(sysAccount);
                            dbContext.SaveChanges();
                            UserAccount userAccount = new UserAccount
                            {
                                Id = Guid.NewGuid(),
                                LastName = lastName,
                                FirstName = firstName,
                                DisplayName = displayName,
                                Title = title,
                                Suffix = suffix,
                                CreatedById = sysAccount.Id,
                                CreatedBy = sysAccount,
                                CreatedOn = DateTime.Now,
                                ModifiedById = sysAccount.Id,
                                ModifiedBy = sysAccount,
                                ExplicitRoles = UserRole.Administrator
                            };
                            userAccount.ModifiedOn = userAccount.CreatedOn;
                            dbContext.UserAccounts.Add(userAccount);
                            dbContext.SaveChanges();
                            if (isWindowsAuth)
                            {
                                WindowsIdentity windowsIdentity = WindowsIdentity.GetCurrent();
                                SecurityIdentifier sid = windowsIdentity.User;
                                string sddl = sid.IsAccountSid() ? sid.AccountDomainSid.ToString() : "";
                                string accountName = windowsIdentity.Name;
                                int index = accountName.IndexOf('\\');
                                string domain;
                                if (index < 0)
                                    domain = sddl;
                                else
                                {
                                    domain = accountName.Substring(0, index);
                                    accountName = accountName.Substring(index + 1);
                                }
                                WindowsAuthDomain authDomain = new WindowsAuthDomain
                                {
                                    Id = Guid.NewGuid(),
                                    Name = domain,
                                    SID = sddl,
                                    CreatedById = sysAccount.Id,
                                    CreatedBy = sysAccount,
                                    CreatedOn = DateTime.Now,
                                    ModifiedById = sysAccount.Id,
                                    ModifiedBy = sysAccount
                                };
                                authDomain.ModifiedOn = authDomain.CreatedOn;
                                dbContext.WindowsAuthDomains.Add(authDomain);
                                dbContext.SaveChanges();
                                WindowsIdentityLogin windowsIdentityLogin = new WindowsIdentityLogin
                                {
                                    Id = Guid.NewGuid(),
                                    AccountName = accountName,
                                    DomainId = authDomain.Id,
                                    Domain = authDomain,
                                    SID = sid.ToString(),
                                    UserId = userAccount.Id,
                                    UserAccount = userAccount,
                                    CreatedById = sysAccount.Id,
                                    CreatedBy = sysAccount,
                                    CreatedOn = DateTime.Now,
                                    ModifiedById = sysAccount.Id,
                                    ModifiedBy = sysAccount
                                };
                                authDomain.ModifiedOn = authDomain.CreatedOn;
                                userAccount.WindowsIdentityLogins.Add(windowsIdentityLogin);
                                dbContext.SaveChanges();
                            }
                            else
                            {
                                BasicLogin basicLogin = new BasicLogin
                                {
                                    Id = Guid.NewGuid(),
                                    LoginName = userName,
                                    PwHash = (PwHash.TryCreate(pw, out PwHash? h) && h.HasValue) ? h.Value.ToString() : "",
                                    UserId = userAccount.Id,
                                    UserAccount = userAccount,
                                    CreatedById = sysAccount.Id,
                                    CreatedBy = sysAccount,
                                    CreatedOn = DateTime.Now,
                                    ModifiedById = sysAccount.Id,
                                    ModifiedBy = sysAccount
                                };
                                basicLogin.ModifiedOn = basicLogin.CreatedOn;
                                userAccount.BasicLogins.Add(basicLogin);
                                dbContext.SaveChanges();
                            }
                            transaction.Commit();
                        }
                        catch
                        {
                            transaction.Rollback();
                            throw;
                        }
                    }
                }
                return true;
            })).ContinueWith(t => Dispatcher.BeginInvoke(new Action(() => OnInitFinished(t))));
        }

        private void OnInitFinished(Task<bool> task)
        {
            if (task.IsCanceled)
                DialogResult = false;
            else if (task.IsFaulted)
            {
                MessageBox.Show(this, $"Error intializing database: {(string.IsNullOrWhiteSpace(task.Exception.Message) ? task.Exception.ToString() : task.Exception.Message)}",
                    "Innitializatiln Error", MessageBoxButton.OK, MessageBoxImage.Error);
                DialogResult = false;
            }
            else
                DialogResult = task.Result;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void DbInitializeViewModel_InitializationSuccess(object sender, EventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void DbInitializeViewModel_InitializationCancelled(object sender, EventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
