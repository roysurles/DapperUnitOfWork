using System;

namespace DapperUnitOfWork.UnitOfWorks
{
    /// <inheritdoc />
    /// <summary>
    /// Base Unit of Work for a single DbContext.
    /// </summary>
    public interface IBaseSingleDbContextUnitOfWork : IDisposable
    {
        /// <summary>
        /// Flag indicating if this instance has been disposed.
        /// </summary>
        bool IsDisposed { get; }
    }
}
