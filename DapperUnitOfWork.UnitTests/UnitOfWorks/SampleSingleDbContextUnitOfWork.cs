using DapperUnitOfWork.UnitOfWorks;
using DapperUnitOfWork.UnitTests.DbContexts;
using DapperUnitOfWork.UnitTests.Repositories;

namespace DapperUnitOfWork.UnitTests.UnitOfWorks
{
    public class SampleSingleDbContextUnitOfWork : BaseSingleDbContextUnitOfWork<ISampleDatabaseDbContext>, ISampleSingleDbContextUnitOfWork
    {
        public SampleSingleDbContextUnitOfWork(ISampleDatabaseDbContext dbContext
            , ISampleRepository sampleRepository, ISampleRepository2 sampleRepository2)
            : base(dbContext, sampleRepository, sampleRepository2)
        {
            DbContext = dbContext;
            SampleRepository = sampleRepository;
            SampleRepository2 = sampleRepository2;
        }

        public ISampleDatabaseDbContext DbContext { get; }
        public ISampleRepository SampleRepository { get; }
        public ISampleRepository2 SampleRepository2 { get; }
    }

    public interface ISampleSingleDbContextUnitOfWork : IBaseSingleDbContextUnitOfWork
    {
        ISampleDatabaseDbContext DbContext { get; }
        ISampleRepository SampleRepository { get; }
        ISampleRepository2 SampleRepository2 { get; }
    }
}
