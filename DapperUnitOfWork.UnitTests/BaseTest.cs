using DapperUnitOfWork.UnitTests.IoC;
using SimpleInjector;
using System;
using Xunit.Abstractions;

namespace DapperUnitOfWork.UnitTests
{
    public abstract class BaseTest : IDisposable
    {
        protected BaseTest(ITestOutputHelper output)
        {
            Output = output;
            Container = DependencyConfig.BuildContainer();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed)
                return;

            if (disposing)
            {
                Container?.Dispose();
            }

            IsDisposed = true;
        }

        public bool IsDisposed { get; private set; }

        public Container Container { get; }

        public ITestOutputHelper Output { get; }
    }
}
