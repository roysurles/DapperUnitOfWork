using DapperUnitOfWork.DbContexts;
using DapperUnitOfWork.Repositories;
using System;
using System.Data.Common;
using System.Linq;

namespace DapperUnitOfWork.UnitOfWorks
{
    public abstract class BaseMultiDbContextUnitOfWork : IBaseMultiDbContextUnitOfWork
    {
        private IBaseDbContext<DbConnection, DbTransaction>[] _dbContexts;
        private IBaseRepository[] _repositories;

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
                if (_dbContexts?.Any() == true)
                {
                    foreach (var dbContext in _dbContexts)
                        dbContext?.Dispose();
                }
            }

            IsDisposed = true;
        }

        public bool IsDisposed { get; private set; }

        /// <summary>
        /// Keeps an internal array of DbContexts for IDisposable.Dispose() and to override in corresponding repositories.
        /// </summary>
        /// <param name="dbContexts">array of IBaseDbContext&lt;DbConnection, DbTransaction&gt;</param>
        protected void RegisterDbContexts(params IBaseDbContext<DbConnection, DbTransaction>[] dbContexts)
        {
            _dbContexts = dbContexts;
            OverrideRespoitoriesLocalDbContexts();
        }
        /// <summary>
        /// Keeps an internal array of Repositories for IDisposable.Dispose() and to be overriden with the corresponding DbContext.
        /// </summary>
        /// <param name="repositories">array of IBaseRepository</param>
        protected void RegisterRepositories(params IBaseRepository[] repositories)
        {
            _repositories = repositories;
            OverrideRespoitoriesLocalDbContexts();
        }

        /// <summary>
        /// Open connection for all DbContexts provided in RegisterDbContexts.
        /// </summary>
        protected void OpenAllConnections() =>
            InvokeDbContextAction(dbContext => true, dbContext => dbContext.OpenConnection());
        /// <summary>
        /// Close connection for all DbContexts provided in RegisterDbContexts where !DbContext.IsConnectionNull
        /// </summary>
        protected void CloseAllConnections() =>
            InvokeDbContextAction(dbContext => !dbContext.IsConnectionNull, dbContext => dbContext.CloseConnection());
        /// <summary>
        /// Begin transaction for all DbContexts provided in RegisterDbContexts.
        /// </summary>
        protected void BeginAllTransactions() =>
            InvokeDbContextAction(dbContext => true, dbContext => dbContext.BeginTransaction());
        /// <summary>
        /// Commit transaction for all DbContexts provided in RegisterDbContexts where !dbContext.IsTransactionNull.
        /// </summary>
        protected void CommitAllTransactions() =>
            InvokeDbContextAction(dbContext => !dbContext.IsTransactionNull, dbContext => dbContext.CommitTransaction());
        /// <summary>
        /// Rollback transaction for all DbContexts provided in RegisterDbContexts where !dbContext.IsTransactionNull.
        /// </summary>
        protected void RollbackAllTransactions() =>
            InvokeDbContextAction(dbContext => !dbContext.IsTransactionNull, dbContext => dbContext.BeginTransaction());

        /// <summary>
        /// Open connection for all DbContexts provided.
        /// </summary>
        /// <param name="dbContexts">IBaseDbContext&lt;DbConnection, DbTransaction&gt;</param>
        protected static void OpenConnectionFor(params IBaseDbContext<DbConnection, DbTransaction>[] dbContexts) =>
            InvokeDbContextAction(dbContext => dbContext.OpenConnection(), dbContexts);
        /// <summary>
        /// Close connection for all DbContexts provided.
        /// </summary>
        /// <param name="dbContexts">IBaseDbContext&lt;DbConnection, DbTransaction&gt;</param>
        protected static void CloseConnectionFor(params IBaseDbContext<DbConnection, DbTransaction>[] dbContexts) =>
            InvokeDbContextAction(dbContext => dbContext.CloseConnection(), dbContexts);
        /// <summary>
        /// Begin transaction for all DbContexts provided.
        /// </summary>
        /// <param name="dbContexts">IBaseDbContext&lt;DbConnection, DbTransaction&gt;</param>
        protected static void BeginTransactionFor(params IBaseDbContext<DbConnection, DbTransaction>[] dbContexts) =>
            InvokeDbContextAction(dbContext => dbContext.BeginTransaction(), dbContexts);
        /// <summary>
        /// Commit transaction for all DbContexts provided.
        /// </summary>
        /// <param name="dbContexts">IBaseDbContext&lt;DbConnection, DbTransaction&gt;</param>
        protected static void CommitTransactionFor(params IBaseDbContext<DbConnection, DbTransaction>[] dbContexts) =>
            InvokeDbContextAction(dbContext => dbContext.CommitTransaction(), dbContexts);
        /// <summary>
        /// Rollback transaction for all DbContexts provided.
        /// </summary>
        /// <param name="dbContexts">IBaseDbContext&lt;DbConnection, DbTransaction&gt;</param>
        protected static void RollbackTransactionFor(params IBaseDbContext<DbConnection, DbTransaction>[] dbContexts) =>
            InvokeDbContextAction(dbContext => dbContext.RollbackTransaction(), dbContexts);

        private void OverrideRespoitoriesLocalDbContexts()
        {
            if (_dbContexts == null || !_dbContexts.Any())
                return;
            if (_repositories == null || !_repositories.Any())
                return;

            foreach (var dbContext in _dbContexts)
            {
                foreach (var repository in _repositories.Where(repository => repository.DbContextConcreteType == dbContext.GetType()))
                {
                    repository.OverrideLocalDbContext(dbContext);
                }
            }
        }
        private void InvokeDbContextAction(Func<IBaseDbContext<DbConnection, DbTransaction>, bool> condition
            , Action<IBaseDbContext<DbConnection, DbTransaction>> action)
        {
            if (_dbContexts == null || !_dbContexts.Any())
                return;

            foreach (var dbContext in _dbContexts)
            {
                if (condition(dbContext))
                    action(dbContext);
            }
        }
        private static void InvokeDbContextAction(Action<IBaseDbContext<DbConnection, DbTransaction>> action
            , params IBaseDbContext<DbConnection, DbTransaction>[] dbContexts)
        {
            if (dbContexts == null || !dbContexts.Any())
                return;

            foreach (var dbContext in dbContexts)
            {
                action(dbContext);
            }
        }
    }
}
