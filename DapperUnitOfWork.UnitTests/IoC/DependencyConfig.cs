using DapperUnitOfWork.UnitTests.DbContexts;
using DapperUnitOfWork.UnitTests.Repositories;
using DapperUnitOfWork.UnitTests.UnitOfWorks;
using SimpleInjector;
using SimpleInjector.Lifestyles;

namespace DapperUnitOfWork.UnitTests.IoC
{
    public static class DependencyConfig
    {
        public static Container BuildContainer()
        {
            var container = new Container();
            container.Options.SuppressLifestyleMismatchVerification = true;
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            // Metadata
            container.Register<ISampleDatabaseDbContextMetadata, SampleDatabaseDbContextMetadata>(Lifestyle.Transient);

            // DbContexts
            container.RegisterDisposableTransient<ISampleDatabaseDbContext, SampleDatabaseDbContext>();

            // Repositories            
            container.RegisterDisposableTransient<ISampleRepository, SampleRepository>();
            container.RegisterDisposableTransient<ISampleRepository2, SampleRepository2>();

            // Unit of Works
            container.RegisterDisposableTransient<ISampleSingleDbContextUnitOfWork, SampleSingleDbContextUnitOfWork>();

            // Optionally verify the container.
            container.Verify();

            return container;
        }
    }
}
