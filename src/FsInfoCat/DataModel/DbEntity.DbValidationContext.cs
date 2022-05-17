using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat
{
    public abstract partial class DbEntity
    {
        // TODO: Document DbEntity.DbValidationContext classes
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        public class DbValidationContext<T>
            where T : DbContext
        {
            public ValidationContext ValidationContext { get; }

            public T DbContext { get; }

            public EntityEntry Entry { get; }

            internal DbValidationContext([DisallowNull] EntityEntry entry, [DisallowNull] ValidationContext validationContext, [DisallowNull] T dbContext)
            {
                if (!ReferenceEquals(entry.Context, dbContext))
                    throw new ArgumentOutOfRangeException(nameof(entry));
                ValidationContext = validationContext;
                DbContext = dbContext;
                Entry = entry;
            }

            public DbValidationContext(ValidationContext validationContext, T dbContext, object entity)
            {
                if (entity is null)
                    throw new ArgumentNullException(nameof(entity));
                ValidationContext = validationContext ?? throw new ArgumentNullException(nameof(validationContext));
                DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
                Entry = dbContext.Entry(entity);
            }
        }

        public class DbValidationContext : DbValidationContext<DbContext>
        {
            public DbValidationContext(ValidationContext validationContext, DbContext dbContext, object entity) : base(validationContext, dbContext, entity) { }

            public bool TryConvert<T>(out DbValidationContext<T> result)
                where T : DbContext
            {
                if (DbContext is T dbContext)
                {
                    result = new DbValidationContext<T>(Entry, ValidationContext, dbContext);
                    return true;
                }
                result = null;
                return false;
            }

        }
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}
