using System;

namespace DapperUnitOfWork.UnitOfWorks
{
    /// <inheritdoc />
    /// <summary>
    /// Base Unit of Work for multiple DbContexts.
    /// </summary>
    public interface IBaseMultiDbContextUnitOfWork : IDisposable
    {
        /// <summary>
        /// Flag indicating if this instance has been disposed.
        /// </summary>
        bool IsDisposed { get; }
    }
}