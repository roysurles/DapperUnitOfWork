using System;
using Xunit.Abstractions;

namespace DapperUnitOfWork.UnitTests
{
    public abstract class BaseTest : IDisposable
    {
        protected BaseTest(ITestOutputHelper output)
        {
            Output = output;
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
                // dispose IDisposables here
            }

            IsDisposed = true;
        }

        public bool IsDisposed { get; private set; }

        public ITestOutputHelper Output { get; }
    }
}
