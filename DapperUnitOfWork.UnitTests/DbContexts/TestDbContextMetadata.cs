using DapperUnitOfWork.DbContexts;

namespace DapperUnitOfWork.UnitTests.DbContexts
{
    public class TestDbContextMetadata : ITestDbContextMetadata
    {
        public string DbConnectionString { get; }
        public bool OpenConnectionOnConnectionCreation { get; }
    }

    public interface ITestDbContextMetadata : IDbContextMetadata
    {
    }
}
