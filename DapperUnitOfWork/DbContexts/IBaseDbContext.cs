using System;
using System.Data;
using System.Data.Common;

namespace DapperUnitOfWork.DbContexts
{
    public interface IBaseDbContext<out TDbConnection, out TDbTransaction> : IDisposable
        where TDbConnection : DbConnection
        where TDbTransaction : DbTransaction
    {
        /// <summary>
        /// Unique Id for this instance.
        /// </summary>
        Guid Id { get; }
        /// <summary>
        /// Flag indicating if this instance has been disposed.
        /// </summary>
        bool IsDisposed { get; }
        /// <summary>
        /// Flag indicating if connection is null.  Used for testing purposes.
        /// </summary>
        bool IsConnectionNull { get; }
        /// <summary>
        /// Flag indicating if transaction is null.  Used for testing purposes.
        /// </summary>
        bool IsTransactionNull { get; }

        /// <summary>
        /// Represents a connection to a dabatase.
        /// </summary>
        TDbConnection Connection { get; }
        /// <summary>
        /// Represents a database transaction.
        /// </summary>
        TDbTransaction Transaction { get; }

        /// <summary>
        /// Safely opens a database connection with the settings specified by the DbConnection.ConnectionString if it is not already opened.
        /// </summary>
        void OpenConnection();
        /// <summary>
        /// Safely closes the connection to the database if it is not already closed.
        /// </summary>
        void CloseConnection();

        /// <summary>
        /// Safely opens Connection if not already opened, then starts a database transaction. 
        /// Nested database transaction(s) are not allowed.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown when currently in a database Transaction.</exception>
        void BeginTransaction();
        /// <summary>
        /// Safely opens Connection if not already opened, then starts a database transaction with the specified isolation level. 
        /// Nested database transaction(s) are not allowed.
        /// </summary>
        /// <param name="isolationLevel">The isolation level under which the database transaction should run.</param>
        /// <exception cref="InvalidOperationException">Thrown when currently in a database Transaction.</exception>
        void BeginTransaction(IsolationLevel isolationLevel);

        /// <summary>
        /// Commits the database transaction.         
        /// On success, the transaction will be disposed and dereferenced and the connection will be closed if it was closed prior to BeginTransaction.
        /// </summary>
        void CommitTransaction();

        /// <summary>
        /// Safely (checks null transaction) rolls back the entire database transaction.  
        /// On success, the database transaction will be Disposed and dereferenced and the Connection will be closed if it was closed prior to BeginTransaction.
        /// </summary>
        void RollbackTransaction();
    }
}
