using FluentAssertions;
using Xunit;
using Xunit.Abstractions;

namespace DapperUnitOfWork.UnitTests.Repositories
{
    public class RepositoryTests : BaseTest
    {
        public RepositoryTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact(DisplayName = "Dispose_AllDisposablesAreDisposed")]
        [Trait("Category", "Unit")]
        public void Dispose_AllDisposablesAreDisposed()
        {
            // Arrange
            var repository = Container.GetInstance<ISampleDatabaseRepository>();

            // Act
            repository.Dispose();

            // Assert
            repository.IsDisposed.Should().Be(true);
        }


        /*
         *  Dispose
         *  Override
         *  Type
         *  DbCOntext (override vs local)
         */
    }
}
