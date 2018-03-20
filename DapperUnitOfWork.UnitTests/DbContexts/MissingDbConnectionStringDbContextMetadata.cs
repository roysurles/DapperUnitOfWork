using DapperUnitOfWork.DbContexts;

namespace DapperUnitOfWork.UnitTests.DbContexts
{
    public class MissingDbConnectionStringDbContextMetadata : IMissingDbConnectionStringDbContextMetadata
    {
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public string DbConnectionString { get; }
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public bool OpenConnectionOnConnectionCreation { get; }
    }

    public interface IMissingDbConnectionStringDbContextMetadata : IDbContextMetadata
    {
    }
}
