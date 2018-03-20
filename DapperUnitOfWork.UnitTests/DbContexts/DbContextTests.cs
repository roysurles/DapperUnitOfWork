using FluentAssertions;
using System;
using System.Data;
using Xunit;
using Xunit.Abstractions;

namespace DapperUnitOfWork.UnitTests.DbContexts
{
    public class DbContextTests : BaseTest
    {
        public DbContextTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact(DisplayName = "Ctor_DbConnectionStringIsEmptyString_ExceptionThrown")]
        [Trait("Category", "Unit")]
        public void Ctor_DbConnectionStringIsEmptyString_ExceptionThrown()
        {
            // Arrange
            var metadata = new MissingDbConnectionStringDbContextMetadata();
            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => new MissingDbConnectionStringDbContext(metadata);

            // Act

            // Assert            
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact(DisplayName = "Connection_AfterDbContextInstantiation_ConnectionIsNull")]
        [Trait("Category", "Unit")]
        public void Connection_AfterDbContextInstantiation_ConnectionIsNull()
        {
            // Arrange
            using (var dbcontext = Container.GetInstance<ISampleDatabaseDbContext>())
            {
                // Act

                // Assert
                dbcontext.IsConnectionNull.Should().Be(true);
            }
        }

        [Fact(DisplayName = "Dispose_AllDisposablesAreDisposed")]
        [Trait("Category", "Unit")]
        public void Dispose_AllDisposablesAreDisposed()
        {
            // Arrange
            var dbcontext = Container.GetInstance<ISampleDatabaseDbContext>();
            // ReSharper disable once AccessToDisposedClosure
            Action action = () => dbcontext.Connection.Close();

            // Act
            dbcontext.Dispose();

            // Assert
            dbcontext.IsDisposed.Should().Be(true);
            action.Should().Throw<InvalidOperationException>();
            dbcontext.IsTransactionNull.Should().Be(true);
        }

        [Fact(DisplayName = "OpenConnection_ConnectionIsClosed_ConnectionIsOpen")]
        [Trait("Category", "Unit")]
        public void OpenConnection_ConnectionIsClosed_ConnectionIsOpen()
        {
            // Arrange
            using (var dbcontext = Container.GetInstance<ISampleDatabaseDbContext>())
            {
                // Act
                dbcontext.OpenConnection();

                // Assert
                dbcontext.Connection.State.Should().Be(ConnectionState.Open);
            }
        }

        [Fact(DisplayName = "OpenConnection_ConnectionIsOpen_ConnectionStateIsOpen")]
        [Trait("Category", "Unit")]
        public void OpenConnection_ConnectionIsOpen_ConnectionStateIsOpen()
        {
            // Arrange
            using (var dbcontext = Container.GetInstance<ISampleDatabaseDbContext>())
            {
                // Act
                dbcontext.OpenConnection();
                dbcontext.OpenConnection();

                // Assert
                dbcontext.Connection.State.Should().Be(ConnectionState.Open);
            }
        }

        [Fact(DisplayName = "CloseConnection_ConnectionIsOpen_ConnectionIsClosed")]
        [Trait("Category", "Unit")]
        public void CloseConnection_ConnectionIsOpen_ConnectionIsClosed()
        {
            // Arrange
            using (var dbcontext = Container.GetInstance<ISampleDatabaseDbContext>())
            {
                // Act
                dbcontext.OpenConnection();
                dbcontext.CloseConnection();

                // Assert
                dbcontext.Connection.State.Should().Be(ConnectionState.Closed);
            }
        }

        [Fact(DisplayName = "CloseConnection_ConnectionIsClosed_ConnectionIsClosed")]
        [Trait("Category", "Unit")]
        public void CloseConnection_ConnectionIsClosed_ConnectionIsClosed()
        {
            // Arrange
            using (var dbcontext = Container.GetInstance<ISampleDatabaseDbContext>())
            {
                // Act
                dbcontext.CloseConnection();

                // Assert
                dbcontext.Connection.State.Should().Be(ConnectionState.Closed);
            }
        }

        [Fact(DisplayName = "BeginTransaction_NotInTransactionAndConnectionIsClosed_ConnectionIsOpenAndTransactionIsNotNull")]
        [Trait("Category", "Unit")]
        public void BeginTransaction_NotInTransactionAndConnectionIsClosed_ConnectionIsOpenAndTransactionIsNotNull()
        {
            // Arrange
            using (var dbcontext = Container.GetInstance<ISampleDatabaseDbContext>())
            {
                // Act
                dbcontext.BeginTransaction();

                // Assert
                dbcontext.Connection.State.Should().Be(ConnectionState.Open);
                dbcontext.Transaction.Should().NotBeNull();
            }
        }

        [Fact(DisplayName = "BeginTransaction_NotInTransactionAndConnectionIsOpen_ConnectionIsOpenAndTransactionIsNotNull")]
        [Trait("Category", "Unit")]
        public void BeginTransaction_NotInTransactionAndConnectionIsOpen_ConnectionIsOpenAndTransactionIsNotNull()
        {
            // Arrange
            using (var dbcontext = Container.GetInstance<ISampleDatabaseDbContext>())
            {
                // Act
                dbcontext.OpenConnection();
                dbcontext.BeginTransaction();

                // Assert
                dbcontext.Connection.State.Should().Be(ConnectionState.Open);
                dbcontext.Transaction.Should().NotBeNull();
            }
        }

        [Fact(DisplayName = "BeginTransaction_InTransaction_ExceptionThrown")]
        [Trait("Category", "Unit")]
        public void BeginTransaction_InTransaction_ExceptionThrown()
        {
            // Arrange
            using (var dbcontext = Container.GetInstance<ISampleDatabaseDbContext>())
            {
                // ReSharper disable once AccessToDisposedClosure
                Action action = () => dbcontext.BeginTransaction();

                // Act
                dbcontext.BeginTransaction();

                // Assert
                action.Should().Throw<InvalidOperationException>();
            }
        }

        [Fact(DisplayName = "CommitTransaction_NotInTransaction_ExceptionThrown")]
        [Trait("Category", "Unit")]
        public void CommitTransaction_NotInTransaction_ExceptionThrown()
        {
            // Arrange
            using (var dbcontext = Container.GetInstance<ISampleDatabaseDbContext>())
            {
                // ReSharper disable once AccessToDisposedClosure
                Action action = () => dbcontext.CommitTransaction();

                // Act

                // Assert
                action.Should().Throw<InvalidOperationException>();
            }
        }

        [Fact(DisplayName = "CommitTransaction_InTransactionAndConnectionWasClosedPriorToBeginTransaction_ConnectionIsClosedAndTransactionIsNull")]
        [Trait("Category", "Unit")]
        public void CommitTransaction_InTransactionAndConnectionWasClosedPriorToBeginTransaction_ConnectionIsClosedAndTransactionIsNull()
        {
            // Arrange
            using (var dbcontext = Container.GetInstance<ISampleDatabaseDbContext>())
            {
                // Act
                dbcontext.BeginTransaction();
                dbcontext.CommitTransaction();

                // Assert
                dbcontext.Connection.State.Should().Be(ConnectionState.Closed);
                dbcontext.IsTransactionNull.Should().Be(true);
            }
        }

        [Fact(DisplayName = "CommitTransaction_InTransactionAndConnectionWasOpenPriorToBeginTransaction_ConnectionIsOpenAndTransactionIsNull")]
        [Trait("Category", "Unit")]
        public void CommitTransaction_InTransactionAndConnectionWasOpenPriorToBeginTransaction_ConnectionIsOpenAndTransactionIsNull()
        {
            // Arrange
            using (var dbcontext = Container.GetInstance<ISampleDatabaseDbContext>())
            {
                // Act
                dbcontext.OpenConnection();
                dbcontext.BeginTransaction();
                dbcontext.CommitTransaction();

                // Assert
                dbcontext.Connection.State.Should().Be(ConnectionState.Open);
                dbcontext.IsTransactionNull.Should().Be(true);
            }
        }

        [Fact(DisplayName = "RollbackTransaction_NotInTransactionAndConnectionIsClosed_ConnectionIsClosedAndTransactionIsNull")]
        [Trait("Category", "Unit")]
        public void RollbackTransaction_NotInTransactionAndConnectionIsClosed_ConnectionIsClosedAndTransactionIsNull()
        {
            // Arrange
            using (var dbcontext = Container.GetInstance<ISampleDatabaseDbContext>())
            {
                // Act
                dbcontext.RollbackTransaction();

                // Assert
                dbcontext.Connection.State.Should().Be(ConnectionState.Closed);
                dbcontext.IsTransactionNull.Should().Be(true);
            }
        }

        [Fact(DisplayName = "RollbackTransaction_InTransactionAndConnectionWasClosedPriorToBeginTransaction_ConnectionIsClosedAndTransactionIsNull")]
        [Trait("Category", "Unit")]
        public void RollbackTransaction_InTransactionAndConnectionWasClosedPriorToBeginTransaction_ConnectionIsClosedAndTransactionIsNull()
        {
            // Arrange
            using (var dbcontext = Container.GetInstance<ISampleDatabaseDbContext>())
            {
                // Act
                dbcontext.BeginTransaction();
                dbcontext.RollbackTransaction();

                // Assert
                dbcontext.Connection.State.Should().Be(ConnectionState.Closed);
                dbcontext.IsTransactionNull.Should().Be(true);
            }
        }

        [Fact(DisplayName = "RollbackTransaction_InTransactionAndConnectionWasOpenPriorToBeginTransaction_ConnectionIsOpenAndTransactionIsNull")]
        [Trait("Category", "Unit")]
        public void RollbackTransaction_InTransactionAndConnectionWasOpenPriorToBeginTransaction_ConnectionIsOpenAndTransactionIsNull()
        {
            // Arrange
            using (var dbcontext = Container.GetInstance<ISampleDatabaseDbContext>())
            {
                // Act
                dbcontext.OpenConnection();
                dbcontext.BeginTransaction();
                dbcontext.RollbackTransaction();

                // Assert
                dbcontext.Connection.State.Should().Be(ConnectionState.Open);
                dbcontext.IsTransactionNull.Should().Be(true);
            }
        }
    }
}
