using DapperUnitOfWork.DbContexts;

namespace DapperUnitOfWork.UnitTests.DbContexts
{
    public class SampleDatabaseDbContextMetadata : ISampleDatabaseDbContextMetadata
    {
        // TODO:  get relative path working |DataDirectory|
        public string DbConnectionString { get; } =
            @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\MSDevelopment\DataAccess\DapperUnitOfWork\DapperUnitOfWork.UnitTests\Data\SampleDatabase.mdf;Integrated Security=True;MultipleActiveResultSets=True;Connect Timeout=30";
        // @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\SampleDatabase.mdf;Integrated Security=True;MultipleActiveResultSets=True;";
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public bool OpenConnectionOnConnectionCreation { get; }
    }

    public interface ISampleDatabaseDbContextMetadata : IDbContextMetadata
    {
    }
}
