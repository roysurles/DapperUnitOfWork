using DapperUnitOfWork.Samples.Shared.DataAccess.Models;
using DapperUnitOfWork.Samples.Shared.DataAccess.UnitOfWorks;
using DapperUnitOfWork.Samples.Shared.IoC;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DapperUnitOfWork.NetCore.SingleDbContextUnitOfWork.ConsoleClient
{
    internal static class Program
    {
        private static void Main()
        {
            try
            {
                Console.WriteLine($"Starting {Assembly.GetEntryAssembly().GetName().Name}...{Environment.NewLine}");

                DependencyConfig.BuildContainer();

                var categoryModels = new List<CategoryModel>
                {
                    new CategoryModel {CategoryName = "CategoryName", Description = "Description"},
                    new CategoryModel {CategoryName = "CategoryName", Description = "Description"},
                    new CategoryModel {CategoryName = "CategoryName", Description = "Description"}
                };

                var northwindUoW = DependencyConfig.Container.GetInstance<INorthwindSingleDbContextUnitOfWork>();

                Console.WriteLine($"Inserting {categoryModels.Count} Categories into Northwind.Categories...{Environment.NewLine}");

                var results = northwindUoW.InsertCategoriesAsync(categoryModels).GetAwaiter().GetResult();

                Console.WriteLine($"New IDs: {string.Join(',', results.Select(x => x.CategoryId))}{Environment.NewLine}");
                Console.WriteLine($"Query Northwind.Categories for new records...{Environment.NewLine}");

                Console.WriteLine("Press ENTER to exit.");
                Console.ReadLine();
            }
            finally
            {
                DependencyConfig.Container.Dispose();
            }
        }
    }
}
