using FluentAssertions;
using System;
using System.Data;
using Xunit;
using Xunit.Abstractions;

namespace DapperUnitOfWork.UnitTests.UnitOfWorks
{
    public class SingleDbContextUnitOfWorkTests : BaseTest
    {
        public SingleDbContextUnitOfWorkTests(ITestOutputHelper output) : base(output)
        {
        }

        [Fact(DisplayName = "Ctor_AllRepositoriesHaveSameDbContext")]
        [Trait("Category", "Unit")]
        public void Ctor_AllRepositoriesHaveSameDbContext()
        {
            // Arrange
            using (var uow = Container.GetInstance<ISampleSingleDbContextUnitOfWork>())
            {
                // Act

                // Assert                
                uow.DbContext.IsConnectionNull.Should().BeTrue();
                uow.SampleRepository.DbContext.Should().Be(uow.DbContext);
                uow.SampleRepository2.DbContext.Should().Be(uow.DbContext);
            }
        }

        [Fact(DisplayName = "Dispose_AllDisposablesAreDisposed")]
        [Trait("Category", "Unit")]
        public void Dispose_AllDisposablesAreDisposed()
        {
            // Arrange
            var uow = Container.GetInstance<ISampleSingleDbContextUnitOfWork>();

            // Act
            uow.Dispose();

            // Assert
            uow.DbContext.IsDisposed.Should().BeTrue();
            uow.SampleRepository.IsDisposed.Should().BeTrue();
            uow.SampleRepository2.IsDisposed.Should().BeTrue();
        }

        [Fact(DisplayName = "OpenConnection_ConnectionIsClosed_ConnectionIsOpen")]
        [Trait("Category", "Unit")]
        public void OpenConnection_ConnectionIsClosed_ConnectionIsOpen()
        {
            // Arrange
            using (var uow = Container.GetInstance<ISampleSingleDbContextUnitOfWork>())
            {
                // Act
                uow.DbContext.OpenConnection();

                // Assert
                uow.DbContext.Connection.State.Should().Be(ConnectionState.Open);
            }
        }

        [Fact(DisplayName = "OpenConnection_ConnectionIsOpen_ConnectionIsOpen")]
        [Trait("Category", "Unit")]
        public void OpenConnection_ConnectionIsOpen_ConnectionIsOpen()
        {
            // Arrange
            using (var uow = Container.GetInstance<ISampleSingleDbContextUnitOfWork>())
            {
                // Act
                uow.DbContext.OpenConnection();
                uow.DbContext.OpenConnection();

                // Assert
                uow.DbContext.Connection.State.Should().Be(ConnectionState.Open);
            }
        }

        [Fact(DisplayName = "CloseConnection_ConnectionIsOpen_ConnectionIsClosed")]
        [Trait("Category", "Unit")]
        public void CloseConnection_ConnectionIsOpen_ConnectionIsClosed()
        {
            // Arrange
            using (var uow = Container.GetInstance<ISampleSingleDbContextUnitOfWork>())
            {
                // Act
                uow.DbContext.OpenConnection();
                uow.DbContext.CloseConnection();

                // Assert
                uow.DbContext.Connection.State.Should().Be(ConnectionState.Closed);
            }
        }

        [Fact(DisplayName = "CloseConnection_ConnectionIsClosed_ConnectionIsClosed")]
        [Trait("Category", "Unit")]
        public void CloseConnection_ConnectionIsClosed_ConnectionIsClosed()
        {
            // Arrange
            using (var uow = Container.GetInstance<ISampleSingleDbContextUnitOfWork>())
            {
                // Act
                uow.DbContext.CloseConnection();

                // Assert
                uow.DbContext.Connection.State.Should().Be(ConnectionState.Closed);
            }
        }

        [Fact(DisplayName = "BeginTransaction_NotInTransactionAndConnectionIsClosed_ConnectionIsOpenAndTransactionIsNotNull")]
        [Trait("Category", "Unit")]
        public void BeginTransaction_NotInTransactionAndConnectionIsClosed_ConnectionIsOpenAndTransactionIsNotNull()
        {
            // Arrange
            using (var uow = Container.GetInstance<ISampleSingleDbContextUnitOfWork>())
            {
                // Act
                uow.DbContext.BeginTransaction();

                // Assert
                uow.DbContext.Connection.State.Should().Be(ConnectionState.Open);
                uow.DbContext.Transaction.Should().NotBeNull();
            }
        }

        [Theory(DisplayName = "BeginTransactionWithIsolationLevel_NotInTransactionAndConnectionIsClosed_ConnectionIsOpenAndTransactionIsNotNullWithIsolationLevel")]
        [Trait("Category", "Unit")]
        // [InlineData(IsolationLevel.Chaos, IsolationLevel.Chaos)] -- The IsolationLevel enumeration value, 16, is not supported by the .Net Framework SqlClient Data Provider.
        [InlineData(IsolationLevel.ReadCommitted, IsolationLevel.ReadCommitted)]
        [InlineData(IsolationLevel.ReadUncommitted, IsolationLevel.ReadUncommitted)]
        [InlineData(IsolationLevel.RepeatableRead, IsolationLevel.RepeatableRead)]
        [InlineData(IsolationLevel.Serializable, IsolationLevel.Serializable)]
        [InlineData(IsolationLevel.Snapshot, IsolationLevel.Snapshot)]
        [InlineData(IsolationLevel.Unspecified, IsolationLevel.ReadCommitted)]
        public void BeginTransactionWithIsolationLevel_NotInTransactionAndConnectionIsClosed_ConnectionIsOpenAndTransactionIsNotNullWithIsolationLevel(IsolationLevel expectedIsolationLevel, IsolationLevel actualIsolationLevel)
        {
            // Arrange
            using (var uow = Container.GetInstance<ISampleSingleDbContextUnitOfWork>())
            {
                // Act
                uow.DbContext.BeginTransaction(expectedIsolationLevel);

                // Assert
                uow.DbContext.Connection.State.Should().Be(ConnectionState.Open);
                uow.DbContext.Transaction.Should().NotBeNull();
                uow.DbContext.Transaction.IsolationLevel.Should().Be(actualIsolationLevel);
            }
        }

        [Fact(DisplayName = "BeginTransaction_NotInTransactionAndConnectionIsOpen_ConnectionIsOpenAndTransactionIsNotNull")]
        [Trait("Category", "Unit")]
        public void BeginTransaction_NotInTransactionAndConnectionIsOpen_ConnectionIsOpenAndTransactionIsNotNull()
        {
            // Arrange
            using (var uow = Container.GetInstance<ISampleSingleDbContextUnitOfWork>())
            {
                // Act
                uow.DbContext.OpenConnection();
                uow.DbContext.BeginTransaction();

                // Assert
                uow.DbContext.Connection.State.Should().Be(ConnectionState.Open);
                uow.DbContext.Transaction.Should().NotBeNull();
            }
        }

        [Fact(DisplayName = "BeginTransaction_InTransaction_ExceptionThrown")]
        [Trait("Category", "Unit")]
        public void BeginTransaction_InTransaction_ExceptionThrown()
        {
            // Arrange
            using (var uow = Container.GetInstance<ISampleSingleDbContextUnitOfWork>())
            {
                // ReSharper disable once AccessToDisposedClosure
                Action action = () => uow.DbContext.BeginTransaction();

                // Act
                uow.DbContext.BeginTransaction();

                // Assert
                action.Should().Throw<InvalidOperationException>();
            }
        }

        [Fact(DisplayName = "CommitTransaction_NotInTransaction_ExceptionThrown")]
        [Trait("Category", "Unit")]
        public void CommitTransaction_NotInTransaction_ExceptionThrown()
        {
            // Arrange
            using (var uow = Container.GetInstance<ISampleSingleDbContextUnitOfWork>())
            {
                // ReSharper disable once AccessToDisposedClosure
                Action action = () => uow.DbContext.CommitTransaction();

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
            using (var uow = Container.GetInstance<ISampleSingleDbContextUnitOfWork>())
            {
                // Act
                uow.DbContext.BeginTransaction();
                uow.DbContext.CommitTransaction();

                // Assert
                uow.DbContext.Connection.State.Should().Be(ConnectionState.Closed);
                uow.DbContext.IsTransactionNull.Should().BeTrue();
            }
        }

        [Fact(DisplayName = "CommitTransaction_InTransactionAndConnectionWasOpenPriorToBeginTransaction_ConnectionIsOpenAndTransactionIsNull")]
        [Trait("Category", "Unit")]
        public void CommitTransaction_InTransactionAndConnectionWasOpenPriorToBeginTransaction_ConnectionIsOpenAndTransactionIsNull()
        {
            // Arrange
            using (var uow = Container.GetInstance<ISampleSingleDbContextUnitOfWork>())
            {
                // Act
                uow.DbContext.OpenConnection();
                uow.DbContext.BeginTransaction();
                uow.DbContext.CommitTransaction();

                // Assert
                uow.DbContext.Connection.State.Should().Be(ConnectionState.Open);
                uow.DbContext.IsTransactionNull.Should().BeTrue();
            }
        }

        [Fact(DisplayName = "RollbackTransaction_NotInTransactionAndConnectionIsClosed_ConnectionIsClosedAndTransactionIsNull")]
        [Trait("Category", "Unit")]
        public void RollbackTransaction_NotInTransactionAndConnectionIsClosed_ConnectionIsClosedAndTransactionIsNull()
        {
            // Arrange
            using (var uow = Container.GetInstance<ISampleSingleDbContextUnitOfWork>())
            {
                // Act
                uow.DbContext.RollbackTransaction();

                // Assert
                uow.DbContext.Connection.State.Should().Be(ConnectionState.Closed);
                uow.DbContext.IsTransactionNull.Should().BeTrue();
            }
        }

        [Fact(DisplayName = "RollbackTransaction_InTransactionAndConnectionWasClosedPriorToBeginTransaction_ConnectionIsClosedAndTransactionIsNull")]
        [Trait("Category", "Unit")]
        public void RollbackTransaction_InTransactionAndConnectionWasClosedPriorToBeginTransaction_ConnectionIsClosedAndTransactionIsNull()
        {
            // Arrange
            using (var uow = Container.GetInstance<ISampleSingleDbContextUnitOfWork>())
            {
                // Act
                uow.DbContext.BeginTransaction();
                uow.DbContext.RollbackTransaction();

                // Assert
                uow.DbContext.Connection.State.Should().Be(ConnectionState.Closed);
                uow.DbContext.IsTransactionNull.Should().BeTrue();
            }
        }

        [Fact(DisplayName = "RollbackTransaction_InTransactionAndConnectionWasOpenPriorToBeginTransaction_ConnectionIsOpenAndTransactionIsNull")]
        [Trait("Category", "Unit")]
        public void RollbackTransaction_InTransactionAndConnectionWasOpenPriorToBeginTransaction_ConnectionIsOpenAndTransactionIsNull()
        {
            // Arrange
            using (var uow = Container.GetInstance<ISampleSingleDbContextUnitOfWork>())
            {
                // Act
                uow.DbContext.OpenConnection();
                uow.DbContext.BeginTransaction();
                uow.DbContext.RollbackTransaction();

                // Assert
                uow.DbContext.Connection.State.Should().Be(ConnectionState.Open);
                uow.DbContext.IsTransactionNull.Should().BeTrue();
            }
        }
    }
}
