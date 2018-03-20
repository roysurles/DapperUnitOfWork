using DapperUnitOfWork.UnitTests.DbContexts;
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

        [Fact(DisplayName = "Ctor_OverrideDbContextIsNull")]
        [Trait("Category", "Unit")]
        public void Ctor_OverrideDbContextIsNull()
        {
            // Arrange
            using (var repository = Container.GetInstance<ISampleDatabaseRepository>())
            {
                // Act

                // Assert
                repository.IsOverrideDbContextNull.Should().BeTrue();
            }
        }

        [Fact(DisplayName = "Dispose_AllDisposablesAreDisposed")]
        [Trait("Category", "Unit")]
        public void Dispose_AllDisposablesAreDisposed()
        {
            // Arrange
            var repository = Container.GetInstance<ISampleDatabaseRepository>();
            repository.OverrideDbContext(Container.GetInstance<ISampleDatabaseDbContext>());

            // Act
            repository.Dispose();

            // Assert
            repository.IsDisposed.Should().BeTrue();
            repository.IsLocalDbContextDisposed.Should().BeTrue();
            repository.IsOverrideDbContextDisposed.Should().BeTrue();
        }

        [Fact(DisplayName = "OverrideDbContext_OverrideDbContextIsNotNull")]
        [Trait("Category", "Unit")]
        public void OverrideDbContext_OverrideDbContextIsNotNull()
        {
            // Arrange
            using (var repository = Container.GetInstance<ISampleDatabaseRepository>())
            {
                var dbContext = Container.GetInstance<ISampleDatabaseDbContext>();

                // Act
                repository.OverrideDbContext(dbContext);

                // Assert
                repository.IsOverrideDbContextNull.Should().BeFalse();
                // ReSharper disable once PossibleInvalidOperationException
                repository.LocalDbContextId.Should().NotBe(repository.OverrideDbContextId.Value, "because IoC for testing is creating Transient(s)");
                repository.DbContextConcreteType.Should().Be(dbContext.GetType());
            }
        }
    }
}
