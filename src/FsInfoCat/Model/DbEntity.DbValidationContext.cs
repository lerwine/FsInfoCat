using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FsInfoCat.Model;

public abstract partial class DbEntity
{
    /// <summary>
    /// Context object for database entity validation.
    /// </summary>
    /// <typeparam name="T">The database context type.</typeparam>
    public class DbValidationContext<T>
        where T : DbContext
    {
        /// <summary>
        /// Gets the validation context object.
        /// </summary>
        public ValidationContext ValidationContext { get; }

        /// <summary>
        /// Gets the database context object.
        /// </summary>
        public T DbContext { get; }

        /// <summary>
        /// Gets the entry of the entity being validated.
        /// </summary>
        public EntityEntry Entry { get; }

        internal DbValidationContext([DisallowNull] EntityEntry entry, [DisallowNull] ValidationContext validationContext, [DisallowNull] T dbContext)
        {
            if (!ReferenceEquals(entry.Context, dbContext))
                throw new ArgumentOutOfRangeException(nameof(entry));
            ValidationContext = validationContext;
            DbContext = dbContext;
            Entry = entry;
        }

        /// <summary>
        /// Initializes a new <c>DbValidationContext</c>.
        /// </summary>
        /// <param name="validationContext">The validation context object.</param>
        /// <param name="dbContext">The database context object.</param>
        /// <param name="entity">The entry of the entity being validated.</param>
        public DbValidationContext([DisallowNull] ValidationContext validationContext, [DisallowNull] T dbContext, [DisallowNull] object entity)
        {
            ArgumentNullException.ThrowIfNull(entity);
            ValidationContext = validationContext ?? throw new ArgumentNullException(nameof(validationContext));
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            Entry = dbContext.Entry(entity);
        }
    }

    /// <summary>
    /// Context object for database entity validation.
    /// </summary>
    public class DbValidationContext([DisallowNull] ValidationContext validationContext, [DisallowNull] DbContext dbContext, [DisallowNull] object entity) : DbValidationContext<DbContext>(validationContext, dbContext, entity)
    {
        /// <summary>
        /// Attempts to convert the current DB validation context object into an object with more strongly typed <see cref="DbValidationContext{T}.DbContext"/>
        /// </summary>
        /// <param name="result"></param>
        /// <returns><see langword="true"/> if the <see cref="DbValidationContext{T}.DbContext"/> of the current object is of type <typeparamref name="T"/> and could therefore be converted;
        /// otherwise, <see langword="false"/>.</returns>
        public bool TryConvert<T>([MaybeNullWhen(true)] out DbValidationContext<T> result)
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
}
