using DapperUnitOfWork.DbContexts;

namespace DapperUnitOfWork.Samples.Shared.DataAccess.DbContexts
{
    public class NorthwindDbContextMetadata : INorthwindDbContextMetadata
    {
        // TODO:  change DbConnectionString to your local instance of Northwind
        public string DbConnectionString { get; } =
            @"Persist Security Info=False;Integrated Security=true;Initial Catalog=Northwind;server=I7-970\SQLSERVER2012";
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public bool OpenConnectionOnConnectionCreation { get; }
    }

    public interface INorthwindDbContextMetadata : IDbContextMetadata
    {
    }
}
