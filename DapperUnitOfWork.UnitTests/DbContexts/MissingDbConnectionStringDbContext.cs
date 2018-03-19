using DapperUnitOfWork.DbContexts;
using System.Data.SqlClient;

namespace DapperUnitOfWork.UnitTests.DbContexts
{
    public class MissingDbConnectionStringDbContext : BaseDbContext<SqlConnection, SqlTransaction>
    {
        public MissingDbConnectionStringDbContext(IDbContextMetadata metadata) : base(metadata)
        {
        }
    }
}
