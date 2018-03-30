using SimpleInjector;
using SimpleInjector.Diagnostics;
using System;

namespace DapperUnitOfWork.UnitTests.IoC
{
    public static class SimpleInjectorExtensions
    {
        public static void RegisterDisposableTransient<TService, TImplementation>(
            this Container c)
            where TImplementation : class, IDisposable, TService
            where TService : class
        {
            var r = Lifestyle.Transient.CreateRegistration<TImplementation>(c);
            r.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "ignore");
            c.AddRegistration(typeof(TService), r);
        }
    }
}
