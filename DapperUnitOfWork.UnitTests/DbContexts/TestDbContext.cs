using DapperUnitOfWork.DbContexts;
using System.Data.SqlClient;

namespace DapperUnitOfWork.UnitTests.DbContexts
{
    public class TestDbContext : BaseDbContext<SqlConnection, SqlTransaction>, ITestDbContext
    {
        public TestDbContext(ITestDbContextMetadata metadata) : base(metadata)
        {
        }
    }

    public interface ITestDbContext : IBaseDbContext<SqlConnection, SqlTransaction>
    {
    }
}
