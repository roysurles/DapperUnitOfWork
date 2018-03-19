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

        [Fact(DisplayName = "DbConnectionString_EmptyString_ExceptionThrown")]
        [Trait("Category", "Unit")]
        public void DbConnectionString_EmptyString_ExceptionThrown()
        {
            // Arrange
            var metadata = new MissingDbConnectionStringDbContextMetadata();
            // ReSharper disable once ObjectCreationAsStatement
            Action action = () => new MissingDbConnectionStringDbContext(metadata);

            // Act

            // Assert            
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact(DisplayName = "Connection_AfterInstantiation_IsNull")]
        [Trait("Category", "Unit")]
        public void Connection_AfterInstantiation_IsNull()
        {
            // Arrange
            var metadata = new SampleDatabaseDbContextMetadata();

            // Act
            using (var dbcontext = new SampleDatabaseDbContext(metadata))
            {
                // Assert
                dbcontext.IsConnectionNull.Should().Be(true);
            }
        }

        [Fact(DisplayName = "Dispose_AllDisposablesAreDisposed")]
        [Trait("Category", "Unit")]
        public void Dispose_AllDisposablesAreDisposed()
        {
            // Arrange
            var metadata = new SampleDatabaseDbContextMetadata();
            var dbcontext = new SampleDatabaseDbContext(metadata);
            // ReSharper disable once AccessToDisposedClosure
            Action action = () => dbcontext.Connection.Close();

            // Act
            dbcontext.Dispose();

            // Assert
            dbcontext.IsDisposed.Should().Be(true);
            action.Should().Throw<InvalidOperationException>();
            dbcontext.IsTransactionNull.Should().Be(true);
        }

        [Fact(DisplayName = "OpenConnection_ClosedState_ConnectionStateIsOpen")]
        [Trait("Category", "Unit")]
        public void OpenConnection_ClosedState_ConnectionStateIsOpen()
        {
            // Arrange
            var metadata = new SampleDatabaseDbContextMetadata();
            using (var dbcontext = new SampleDatabaseDbContext(metadata))
            {
                // Act
                dbcontext.OpenConnection();

                // Assert
                dbcontext.Connection.State.Should().Be(ConnectionState.Open);
            }
        }

        [Fact(DisplayName = "OpenConnection_OpenState_ConnectionStateIsOpen")]
        [Trait("Category", "Unit")]
        public void OpenConnection_OpenState_ConnectionStateIsOpen()
        {
            // Arrange
            var metadata = new SampleDatabaseDbContextMetadata();
            using (var dbcontext = new SampleDatabaseDbContext(metadata))
            {
                // Act
                dbcontext.OpenConnection();
                dbcontext.OpenConnection();

                // Assert
                dbcontext.Connection.State.Should().Be(ConnectionState.Open);
            }
        }

        [Fact(DisplayName = "CloseConnection_OpenState_ConnectionStateIsClosed")]
        [Trait("Category", "Unit")]
        public void CloseConnection_OpenState_ConnectionStateIsClosed()
        {
            // Arrange
            var metadata = new SampleDatabaseDbContextMetadata();
            using (var dbcontext = new SampleDatabaseDbContext(metadata))
            {
                // Act
                dbcontext.OpenConnection();
                dbcontext.CloseConnection();

                // Assert
                dbcontext.Connection.State.Should().Be(ConnectionState.Closed);
            }
        }

        [Fact(DisplayName = "CloseConnection_ClosedState_ConnectionStateIsClosed")]
        [Trait("Category", "Unit")]
        public void CloseConnection_ClosedState_ConnectionStateIsClosed()
        {
            // Arrange
            var metadata = new SampleDatabaseDbContextMetadata();
            using (var dbcontext = new SampleDatabaseDbContext(metadata))
            {
                // Act
                dbcontext.CloseConnection();

                // Assert
                dbcontext.Connection.State.Should().Be(ConnectionState.Closed);
            }
        }

        [Fact(DisplayName = "BeginTransaction_NotInTransactionAndConnectionClosed_ConnectionStateIsOpenAndTransactionIsNotNull")]
        [Trait("Category", "Unit")]
        public void BeginTransaction_NotInTransactionAndConnectionClosed_ConnectionStateIsOpenAndTransactionIsNotNull()
        {
            // Arrange
            var metadata = new SampleDatabaseDbContextMetadata();
            using (var dbcontext = new SampleDatabaseDbContext(metadata))
            {
                // Act
                dbcontext.BeginTransaction();

                // Assert
                dbcontext.Connection.State.Should().Be(ConnectionState.Open);
                dbcontext.Transaction.Should().NotBeNull();
            }
        }

        [Fact(DisplayName = "BeginTransaction_NotInTransactionAndConnectionOpen_ConnectionStateIsOpenAndTransactionIsNotNull")]
        [Trait("Category", "Unit")]
        public void BeginTransaction_NotInTransactionAndConnectionOpen_ConnectionStateIsOpenAndTransactionIsNotNull()
        {
            // Arrange
            var metadata = new SampleDatabaseDbContextMetadata();
            using (var dbcontext = new SampleDatabaseDbContext(metadata))
            {
                // Act
                dbcontext.OpenConnection();
                dbcontext.BeginTransaction();

                // Assert
                dbcontext.Connection.State.Should().Be(ConnectionState.Open);
                dbcontext.Transaction.Should().NotBeNull();
            }
        }

        [Fact(DisplayName = "BeginTransaction_NotInTransactionAndConnectionOpen_ConnectionStateIsOpenAndTransactionIsNotNull")]
        [Trait("Category", "Unit")]
        public void BeginTransaction_AlreadyInTransaction_ExceptionThrown()
        {
            // Arrange
            var metadata = new SampleDatabaseDbContextMetadata();
            using (var dbcontext = new SampleDatabaseDbContext(metadata))
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
            var metadata = new SampleDatabaseDbContextMetadata();
            using (var dbcontext = new SampleDatabaseDbContext(metadata))
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
            var metadata = new SampleDatabaseDbContextMetadata();
            using (var dbcontext = new SampleDatabaseDbContext(metadata))
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
            var metadata = new SampleDatabaseDbContextMetadata();
            using (var dbcontext = new SampleDatabaseDbContext(metadata))
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

        [Fact(DisplayName = "RollbackTransaction_NotInTransactionAndConnectionWasClosed_ConnectionIsClosedAndTransactionIsNull")]
        [Trait("Category", "Unit")]
        public void RollbackTransaction_NotInTransactionAndConnectionWasClosed_ConnectionIsClosedAndTransactionIsNull()
        {
            // Arrange
            var metadata = new SampleDatabaseDbContextMetadata();
            using (var dbcontext = new SampleDatabaseDbContext(metadata))
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
            var metadata = new SampleDatabaseDbContextMetadata();
            using (var dbcontext = new SampleDatabaseDbContext(metadata))
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
            var metadata = new SampleDatabaseDbContextMetadata();
            using (var dbcontext = new SampleDatabaseDbContext(metadata))
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
