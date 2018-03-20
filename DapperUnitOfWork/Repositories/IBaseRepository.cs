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
        /// Flag indicating if LocalDbContext is disposed.  Used for testing purposes.
        /// </summary>
        bool IsLocalDbContextDisposed { get; }

        /// <summary>
        /// Flag indicating if OverrideDbContext is disposed.  Used for testing purposes.
        /// </summary>
        bool? IsOverrideDbContextDisposed { get; }

        /// <summary>
        /// Flag indicating if OverrideDbContext is null.  Used for testing purposes.
        /// </summary>
        bool IsOverrideDbContextNull { get; }

        /// <summary>
        /// Id of LocalDbContext.  Used for testing purposes.
        /// </summary>
        Guid LocalDbContextId { get; }

        /// <summary>
        /// Id of OverrideDbContext.  Used for testing purposes.
        /// </summary>
        Guid? OverrideDbContextId { get; }

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
