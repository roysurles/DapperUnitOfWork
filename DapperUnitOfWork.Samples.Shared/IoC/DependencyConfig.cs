﻿using DapperUnitOfWork.Samples.Shared.DbContexts;
using DapperUnitOfWork.Samples.Shared.Repositories;
using DapperUnitOfWork.Samples.Shared.UnitOfWorks;
using SimpleInjector;

namespace DapperUnitOfWork.Samples.Shared.IoC
{
    public static class DependencyConfig
    {
        public static Container Container;

        public static void BuildContainer()
        {
            Container = new Container();

            // Metadata
            Container.Register<INorthwindDbContextMetadata, NorthwindDbContextMetadata>(Lifestyle.Transient);

            // DbContexts
            Container.RegisterDisposableTransient<INorthwindDbContext, NorthwindDbContext>();

            // Repositories
            Container.RegisterDisposableTransient<ICategoryNorthwindRepository, CategoryNorthwindRepository>();

            // Unit of Works
            Container.RegisterDisposableTransient<INorthwindSingleDbContextUnitOfWork, NorthwindSingleDbContextUnitOfWork>();

            // Optionally verify the container.
            Container.Verify();
        }
    }
}
