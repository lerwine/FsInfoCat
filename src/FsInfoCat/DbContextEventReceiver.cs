using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace FsInfoCat
{
    public class DbContextEventReceiver : IDisposable
    {
        private bool disposedValue;
        private readonly DbContext _dbContext;

        public Queue<SaveChangesEventArgs> EventData { get; } = new();

        public bool SaveChangesFailedOcurred { get; set; }

        public bool SavedChangesOccurred { get; set; }

        public DbContextEventReceiver(DbContext dbContext)
        {
            _dbContext = dbContext;
            dbContext.SaveChangesFailed += DbContext_SaveChangesFailed;
            dbContext.SavedChanges += DbContext_SavedChanges;
        }

        private void DbContext_SavedChanges(object sender, SavedChangesEventArgs e)
        {
            EventData.Enqueue(e);
            SavedChangesOccurred = true;
        }

        private void DbContext_SaveChangesFailed(object sender, SaveChangesFailedEventArgs e)
        {
            EventData.Enqueue(e);
            SaveChangesFailedOcurred = true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _dbContext.SaveChangesFailed += DbContext_SaveChangesFailed;
                    _dbContext.SavedChanges += DbContext_SavedChanges;
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
