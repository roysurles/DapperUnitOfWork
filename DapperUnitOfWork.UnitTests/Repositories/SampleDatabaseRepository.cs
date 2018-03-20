using DapperUnitOfWork.Repositories;
using DapperUnitOfWork.UnitTests.DbContexts;

namespace DapperUnitOfWork.UnitTests.Repositories
{
    public class SampleDatabaseRepository : BaseRepository<ISampleDatabaseDbContext>, ISampleDatabaseRepository
    {
        public SampleDatabaseRepository(ISampleDatabaseDbContext dbContext) : base(dbContext)
        {
        }
    }

    public interface ISampleDatabaseRepository : IBaseRepository
    {

    }
}
