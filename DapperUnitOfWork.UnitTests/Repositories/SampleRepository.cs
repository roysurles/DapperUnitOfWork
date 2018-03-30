using DapperUnitOfWork.Repositories;
using DapperUnitOfWork.UnitTests.DbContexts;

namespace DapperUnitOfWork.UnitTests.Repositories
{
    public class SampleRepository : BaseRepository<ISampleDatabaseDbContext>, ISampleRepository
    {
        public SampleRepository(ISampleDatabaseDbContext dbContext) : base(dbContext)
        {
            LocalDbContext = dbContext;
        }

        public ISampleDatabaseDbContext LocalDbContext { get; }

        public ISampleDatabaseDbContext OverrideDbContext { get; private set; }

        public new ISampleDatabaseDbContext DbContext => base.DbContext;

        public new void OverrideLocalDbContext(object dbContext)
        {
            OverrideDbContext = dbContext as ISampleDatabaseDbContext;
            base.OverrideLocalDbContext(dbContext);
        }
    }

    public interface ISampleRepository : IBaseRepository
    {
        ISampleDatabaseDbContext LocalDbContext { get; }
        ISampleDatabaseDbContext OverrideDbContext { get; }
        ISampleDatabaseDbContext DbContext { get; }
    }
}
