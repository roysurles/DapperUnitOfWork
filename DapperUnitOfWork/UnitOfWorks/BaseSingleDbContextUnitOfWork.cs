using DapperUnitOfWork.DbContexts;
using DapperUnitOfWork.Repositories;
using System;
using System.Data;
using System.Data.Common;
using System.Linq;

namespace DapperUnitOfWork.UnitOfWorks
{
    public abstract class BaseSingleDbContextUnitOfWork<TDbContext> : IBaseSingleDbContextUnitOfWork
        where TDbContext : class, IBaseDbContext<DbConnection, DbTransaction>
    {
        private readonly IBaseRepository[] _repositories;
        private readonly TDbContext _dbContext;

        /// <summary>
        /// Pass in dbContext and each repository, so that every repository can use and share the dbContext.
        /// </summary>
        /// <param name="dbContext">IBaseDbContext&lt;DbConnection, DbTransaction&gt;</param>
        /// <param name="repositories">array of IBaseRepository&lt;TDbContext&gt;</param>
        protected BaseSingleDbContextUnitOfWork(TDbContext dbContext, params IBaseRepository[] repositories)
        {
            _dbContext = dbContext;

            if (repositories == null || !repositories.Any())
                return;

            _repositories = repositories;
            foreach (var repository in _repositories)
                repository.OverrideLocalDbContext(_dbContext);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            if (disposing)
            {
                if (_repositories?.Any() == true)
                {
                    foreach (var repository in _repositories)
                        repository?.Dispose();
                }

                _dbContext?.Dispose();
            }

            IsDisposed = true;
        }

        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Safely opens a database connection with the settings specified by the DbContext.Connection.ConnectionString if it is not already opened.
        /// </summary>
        protected virtual void OpenConnection() =>
            _dbContext.OpenConnection();
        /// <summary>
        /// Safely closes the connection to the database if it is not already closed.
        /// </summary>
        protected virtual void CloseConnection() =>
            _dbContext.CloseConnection();
        /// <summary>
        /// Safely opens Connection if not already opened, then starts a database transaction. 
        /// Nested database transaction(s) are not allowed. 
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when currently in a database Transaction.</exception>
        protected virtual void BeginTransaction() =>
            _dbContext.BeginTransaction();
        /// <summary>
        /// Safely opens Connection if not already opened, then starts a database transaction with the specified isolation level. 
        /// Nested database transaction(s) are not allowed.
        /// </summary>
        /// <param name="isolationLevel">The isolation level under which the database transaction should run.</param>
        /// <exception cref="InvalidOperationException">Thrown when currently in a database Transaction.</exception>
        protected virtual void BeginTransaction(IsolationLevel isolationLevel) =>
            _dbContext.BeginTransaction(isolationLevel);
        /// <summary>
        /// Commits the database transaction.         
        /// On success, the transaction will be disposed and dereferenced and the connection will be closed if it was closed prior to BeginTransaction.
        /// </summary>
        protected virtual void CommitTransaction() =>
            _dbContext.CommitTransaction();
        /// <summary>
        /// Safely (checks null transaction) rolls back the entire database transaction.  
        /// On success, the database transaction will be disposed and dereferenced and the connection will be closed if it was closed prior to BeginTransaction.
        /// </summary>
        protected virtual void RollbackTransaction() =>
            _dbContext.RollbackTransaction();
    }
}
