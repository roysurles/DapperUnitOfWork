namespace DapperUnitOfWork.DbContexts
{
    /// <summary>
    /// Metadata used by DbContext
    /// </summary>
    public interface IDbContextMetadata
    {
        /// <summary>
        /// Used by DbConnection
        /// </summary>
        string DbConnectionString { get; }
        /// <summary>
        /// Indicates if the connection should be opened immediately after creating the connection
        /// </summary>
        bool OpenConnectionOnConnectionCreation { get; }
    }
}
