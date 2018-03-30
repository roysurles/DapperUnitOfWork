using DapperUnitOfWork.UnitTests.DbContexts;

namespace DapperUnitOfWork.UnitTests.Repositories
{
    public class SampleRepository2 : SampleRepository, ISampleRepository2
    {
        public SampleRepository2(ISampleDatabaseDbContext dbContext) : base(dbContext)
        {
        }
    }

    public interface ISampleRepository2 : ISampleRepository
    {
    }
}
