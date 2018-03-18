using System;

namespace DapperUnitOfWork.Repositories
{
    /// <inheritdoc cref="IDisposable" />
    /// <summary>
    /// One Repository per Domain per DbContext
    /// </summary>
    /// <remarks>
    /// Naming Convention: [Domain][Connection]Repository
    /// </remarks>
    /// <typeparam name="TDbContext">Desired DbContext</typeparam>
    public interface IBaseRepository : IDisposable
    {
        /// <summary>
        /// Flag indicating if this instance has been disposed.
        /// </summary>
        bool IsDisposed { get; }

        /// <summary>
        /// Returns the concrete type of the DbContext.
        /// </summary>
        Type DbContextConcreteType { get; }

        /// <summary>
        /// Ovveride the local DbContext.
        /// </summary>
        /// <param name="dbContext"></param>
        void OverrideDbContext(object dbContext);
    }
}
