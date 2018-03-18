using DapperUnitOfWork.DbContexts;
using System.Data.SqlClient;

namespace DapperUnitOfWork.Samples.Shared.DataAccess.DbContexts
{
    public class NorthwindDbContext : BaseDbContext<SqlConnection, SqlTransaction>, INorthwindDbContext
    {
        public NorthwindDbContext(INorthwindDbContextMetadata metadata) : base(metadata)
        {
        }
    }

    public interface INorthwindDbContext : IBaseDbContext<SqlConnection, SqlTransaction>
    {

    }
}
