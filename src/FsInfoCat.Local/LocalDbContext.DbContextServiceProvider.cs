using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Local
{
    public partial class LocalDbContext
    {
        private class DbContextServiceProvider : ProxyServiceProvider
        {
            private readonly object _entity;
            private readonly LocalDbContext _dbContext;
            private readonly Type _entityType;

            internal DbContextServiceProvider([DisallowNull] LocalDbContext dbContext, [DisallowNull] object entity)
                : base(Services.ServiceProvider)
            {
                _dbContext = dbContext;
                _entityType = (_entity = entity).GetType();
            }

            protected override bool TryGetService(Type serviceType, out object service)
            {
                if (serviceType is null)
                {
                    service = null;
                    return false;
                }
                if (serviceType.IsInstanceOfType(_entity))
                    service = _entity;
                else if (serviceType.IsInstanceOfType(_dbContext))
                    service = _dbContext;
                else if (serviceType.Equals(typeof(EntityEntry)))
                    service = _dbContext.Entry(_entity);
                else
                {
                    service = null;
                    return false;
                }
                return true;
            }
        }
    }
}
