using Xunit.Abstractions;

namespace DapperUnitOfWork.UnitTests.UnitOfWorks
{
    public class MultiDbContextUnitOfWorkTests : BaseTest
    {
        public MultiDbContextUnitOfWorkTests(ITestOutputHelper output) : base(output)
        {
        }

        /*
         *  ctor
         *  dispose
         *  RegisterDbContexts
         *  RegisterRepositories
         *  OpenAllConnections
         *  CloseAllConnections
         *  BeginAllTransactions
         *  CommitAllTransactions
         *  RollbackAllTransactions
         *  OpenConnectionFor
         *  CloseConnectionFor
         *  BeginTransactionFor
         *  CommitTransactionFor
         *  RollbackTransactionFor
         *  OverrideRespoitoriesLocalDbContexts
         */
    }
}
