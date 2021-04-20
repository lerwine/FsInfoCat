using FsInfoCat.Desktop.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        public DbInitializeWindow()
        {
            InitializeComponent();
        }

        protected override void OnActivated(EventArgs e)
        {

        }

        internal static async Task<bool> CheckConfiguration(Func<bool> ifNoSystemAccount, Action<Exception, string> onException)
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
                    if (await dbContext.UserAccounts.AnyAsync(u => u.Id == id))
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

        private void DbInitializeViewModel_InitializationSuccess(object sender, EventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void DbInitializeViewModel_InitializationCancelled(object sender, EventArgs e)
        {
            MessageBox.Show(this, "Initialization was not completed. This app will now close", "Innitialization Incomplete", MessageBoxButton.OK, MessageBoxImage.Error);
            DialogResult = false;
            Close();
        }
    }
}
