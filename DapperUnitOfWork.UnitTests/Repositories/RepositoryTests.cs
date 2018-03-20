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
            using (var repository = Container.GetInstance<ISampleRepository>())
            {
                // Act

                // Assert
                repository.OverrideDbContext.Should().BeNull();
            }
        }

        [Fact(DisplayName = "Dispose_AllDisposablesAreDisposed")]
        [Trait("Category", "Unit")]
        public void Dispose_AllDisposablesAreDisposed()
        {
            // Arrange
            var repository = Container.GetInstance<ISampleRepository>();
            repository.OverrideLocalDbContext(Container.GetInstance<ISampleDatabaseDbContext>());

            // Act
            repository.Dispose();

            // Assert
            repository.IsDisposed.Should().BeTrue();
            repository.LocalDbContext.IsDisposed.Should().BeTrue();
            repository.OverrideDbContext.IsDisposed.Should().BeTrue();
        }

        [Fact(DisplayName = "OverrideDbContext_OverrideDbContextIsNotNull")]
        [Trait("Category", "Unit")]
        public void OverrideDbContext_OverrideDbContextIsNotNull()
        {
            // Arrange
            using (var repository = Container.GetInstance<ISampleRepository>())
            {
                var dbContext = Container.GetInstance<ISampleDatabaseDbContext>();

                // Act
                repository.OverrideLocalDbContext(dbContext);

                // Assert
                repository.OverrideDbContext.Should().NotBeNull();
                // ReSharper disable once PossibleInvalidOperationException
                repository.LocalDbContext.Should().NotBe(repository.OverrideDbContext, "because IoC for testing is creating Transient(s)");
                repository.DbContext.Should().Be(repository.OverrideDbContext, "because its overriden");
                repository.DbContextConcreteType.Should().Be(dbContext.GetType());
            }
        }
    }
}
