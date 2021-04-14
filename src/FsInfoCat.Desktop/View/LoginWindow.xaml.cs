using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
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
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            ViewModel.SettingsViewModel settingsViewModel = App.GetSettingsVM();
            settingsViewModel.GetMachineIdentifierAsync().ContinueWith(OnGetMachineIdentifierFinished);
        }

        private void OnGetMachineIdentifierFinished(Task<SecurityIdentifier> task)
        {
            if (task.IsCanceled)
                Dispatcher.Invoke(() => OnGetMachineIdentifierFailed(null));
            else if (task.IsFaulted)
                Dispatcher.Invoke(() => OnGetMachineIdentifierFailed(task.Exception));
            else
            {
                SecurityIdentifier sid = task.Result;
                if (sid is null)
                    Dispatcher.Invoke(() => OnGetMachineIdentifierFailed(null));
                else
                {
                    string userName = Dispatcher.Invoke(() => UserNameTextBox.Text);
                    using (FsInfoCatEntities dbContext = new FsInfoCatEntities())
                    {
                        Account account = (from c in dbContext.Accounts where c.LoginName == userName select c).FirstOrDefault();
                        if (account is null)
                            Dispatcher.Invoke(OnLoginFailed);
                        else
                        {
                            UserCredential userCredential = (from u in dbContext.UserCredentials where u.AccountID == account.AccountID select u).FirstOrDefault();
                            if (userCredential is null || string.IsNullOrWhiteSpace(userCredential.PwHash))
                                Dispatcher.Invoke(OnLoginFailed);
                            else
                            {
                                // TODO: Compare hashes                                
                            }
                        }
                    }
                }
            }
            throw new NotImplementedException();
        }

        private void OnLoginFailed()
        {
            throw new NotImplementedException();
        }

        private void OnGetMachineIdentifierFailed(AggregateException exception)
        {
            throw new NotImplementedException();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
