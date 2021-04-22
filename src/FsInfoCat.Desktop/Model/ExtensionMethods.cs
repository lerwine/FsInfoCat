using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FsInfoCat.Desktop.Model
{
    public static class ExtensionMethods
    {
        public static UserRole AsNormalized(this UserRole userRole)
        {
            if (userRole.HasFlag(UserRole.Administrator))
            {
                foreach (UserRole r in Enum.GetValues(typeof(UserRole)))
                    userRole |= r;
            }
            else if (userRole != UserRole.None)
                return userRole | UserRole.Viewer;
            return userRole;
        }

        public static UserRole GetEffectiveRoles(this UserAccount user)
        {
            if (user is null)
                return UserRole.None;
            if (user.Memberships is null)
                return user.ExplicitRoles;
            return AsNormalized(user.Memberships.Select(m => (m is null || m.Group is null) ? UserRole.None : m.Group.Roles).Aggregate(user.ExplicitRoles, (a, r) => a | r));
        }

        public static UserAccount GetUserById(this DbModel dbContext, Guid id) => (dbContext is null) ? null : GetUserById(dbContext.UserAccounts, id);

        public static UserAccount GetUserById(this DbSet<UserAccount> dbSet, Guid id)
        {
            if (dbSet is null)
                return null;
            return (from u in dbSet where u.Id == id select u).FirstOrDefault();
        }

        public static UserAccount GetSystemAccount(this DbModel dbContext)
        {
            Guid id = Guid.Empty;
            UserAccount userAccount = GetUserById(dbContext, id);
            if (userAccount is null)
                throw new InvalidOperationException("Could not find system account");
            return userAccount;
        }
    }

    public partial class DbModel
    {
        internal DbModel(string connectionString) : base(connectionString) { }
    }
}
