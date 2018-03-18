using System;
using System.Data;
using System.Data.Common;

namespace DapperUnitOfWork.DbContexts
{
    public abstract class BaseDbContext<TDbConnection, TDbTransaction> : IBaseDbContext<TDbConnection, TDbTransaction>
        where TDbConnection : DbConnection
        where TDbTransaction : DbTransaction
    {
        private readonly IDbContextMetadata _metadata;
        private TDbConnection _connection;
        private bool _wasConnectionClosed;

        protected BaseDbContext(IDbContextMetadata metadata)
        {
            if (string.IsNullOrWhiteSpace(metadata.DbConnectionString))
                throw new ArgumentNullException(nameof(metadata.DbConnectionString), $"{nameof(metadata.DbConnectionString)} from {metadata.GetType().Name} cannot be null or empty space.");

            Id = Guid.NewGuid();
            _metadata = metadata;
            _wasConnectionClosed = !_metadata.OpenConnectionOnConnectionCreation;
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
                Transaction?.Dispose();
                _connection?.Dispose();
            }

            IsDisposed = true;
        }

        public Guid Id { get; }
        public bool IsDisposed { get; private set; }
        public bool IsConnectionNull => _connection == null;
        public bool IsTransactionNull => Transaction == null;

        public TDbConnection Connection
        {
            get
            {
                if (_connection != null)
                    return _connection;
                if (IsDisposed)
                    throw new InvalidOperationException($"{GetType().Name}.{nameof(Connection)} has been disposed.");

                _connection = CreateDbConnection(_metadata.DbConnectionString, _metadata.OpenConnectionOnConnectionCreation);

                return _connection;
            }
        }
        public TDbTransaction Transaction { get; protected set; }

        public void OpenConnection()
        {
            if (Connection.State != ConnectionState.Open)
                Connection.Open();
        }
        public void CloseConnection()
        {
            if (Connection.State != ConnectionState.Closed)
                Connection.Close();
        }

        public void BeginTransaction()
        {
            InitBeforeBeginTransaction();
            Transaction = (TDbTransaction)Connection.BeginTransaction();
        }
        public void BeginTransaction(IsolationLevel isolationLevel)
        {
            InitBeforeBeginTransaction();
            Transaction = (TDbTransaction)Connection.BeginTransaction(isolationLevel);
        }

        public void CommitTransaction()
        {
            if (Transaction == null)
                throw new InvalidOperationException($"Cannot {nameof(CommitTransaction)} when not currently in a transaction.  {nameof(BeginTransaction)} before attempting to {nameof(CommitTransaction)}.");

            Transaction.Commit();
            CleanUpAfterCommmitOrRollback();
        }
        public void RollbackTransaction()
        {
            Transaction?.Rollback();
            CleanUpAfterCommmitOrRollback();
        }

        private void InitBeforeBeginTransaction()
        {
            if (Transaction != null)
                throw new InvalidOperationException($"Cannot {nameof(BeginTransaction)} while this DbContext ({GetType().Name}) is currently in a transaction.  Either Commit or Rollback, then {nameof(BeginTransaction)}.");

            _wasConnectionClosed = Connection.State == ConnectionState.Closed;
            OpenConnection();
        }
        private void CleanUpAfterCommmitOrRollback()
        {
            Transaction?.Dispose();
            Transaction = null;
            if (_wasConnectionClosed)
                CloseConnection();
        }

        private static TDbConnection CreateDbConnection(string connectionString, bool openConnection = false)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentNullException(nameof(connectionString), $"{nameof(connectionString)} cannot be null or empty space for {nameof(CreateDbConnection)}.");

            var dbConnection = (TDbConnection)Activator.CreateInstance(typeof(TDbConnection));

            if (dbConnection == null)
                throw new NullReferenceException($"Cannot create a new connection for: {typeof(TDbConnection).Name}.  Check {nameof(connectionString)} and try again.");

            dbConnection.ConnectionString = connectionString;

            if (openConnection)
                dbConnection.Open();

            return dbConnection;
        }
    }
}