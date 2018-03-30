using DapperUnitOfWork.DbContexts;
using System.Data.SqlClient;

namespace DapperUnitOfWork.UnitTests.DbContexts
{
    public class MissingDbConnectionStringDbContext : BaseDbContext<SqlConnection, SqlTransaction>, IMissingDbConnectionStringDbContext
    {
        public MissingDbConnectionStringDbContext(IMissingDbConnectionStringDbContextMetadata metadata) : base(metadata)
        {
        }
    }

    public interface IMissingDbConnectionStringDbContext : IBaseDbContext<SqlConnection, SqlTransaction>
    {
    }
}
