using DapperUnitOfWork.DbContexts;
using System.Data.SqlClient;

namespace DapperUnitOfWork.UnitTests.DbContexts
{
    public class SampleDatabaseDbContext : BaseDbContext<SqlConnection, SqlTransaction>, ISampleDatabaseDbContext
    {
        public SampleDatabaseDbContext(ISampleDatabaseDbContextMetadata metadata) : base(metadata)
        {
        }
    }

    public interface ISampleDatabaseDbContext : IBaseDbContext<SqlConnection, SqlTransaction>
    {
    }
}
