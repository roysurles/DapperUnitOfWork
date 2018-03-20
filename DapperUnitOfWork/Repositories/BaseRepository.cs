using DapperUnitOfWork.DbContexts;
using System;
using System.Data.Common;

namespace DapperUnitOfWork.Repositories
{
    public abstract class BaseRepository<TDbContext> : IBaseRepository
        where TDbContext : class, IBaseDbContext<DbConnection, DbTransaction>
    {
        private readonly TDbContext _localDbContext;
        private TDbContext _overrideDbContext;

        protected BaseRepository(TDbContext dbContext) =>
            _localDbContext = dbContext;

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
                _localDbContext?.Dispose();
                _overrideDbContext?.Dispose();
            }

            IsDisposed = true;
        }

        public bool IsDisposed { get; private set; }

        public bool IsLocalDbContextDisposed => _localDbContext.IsDisposed;

        public bool? IsOverrideDbContextDisposed => _overrideDbContext?.IsDisposed;

        public bool IsOverrideDbContextNull => _overrideDbContext == null;

        public Guid LocalDbContextId => _localDbContext.Id;

        public Guid? OverrideDbContextId => _overrideDbContext?.Id;

        public void OverrideDbContext(object dbContext)
        {
            if (_overrideDbContext != null)
                throw new InvalidOperationException($"{nameof(_overrideDbContext)} may only be set once per instance for this repository: {GetType().Name}.");
            if (IsDisposed)
                throw new InvalidOperationException($"{nameof(_overrideDbContext)} cannot be set after this repository ({GetType().Name}) has been disposed.");

            _overrideDbContext = (TDbContext)dbContext;
        }

        public Type DbContextConcreteType =>
            DbContext.GetType();

        protected TDbContext DbContext => _overrideDbContext ?? _localDbContext;
    }
}
