using System;
using System.Collections.Generic;
using System.Text;
using DapperUnitOfWork.DbContexts;

namespace DapperUnitOfWork.UnitTests.DbContexts
{
    public class MissingDbConnectionStringDbContextMetadata : IDbContextMetadata
    {
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public string DbConnectionString { get; }
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        public bool OpenConnectionOnConnectionCreation { get; }
    }
}
