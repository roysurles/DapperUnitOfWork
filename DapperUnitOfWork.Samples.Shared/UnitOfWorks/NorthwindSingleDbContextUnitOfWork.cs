using DapperUnitOfWork.Samples.Shared.DbContexts;
using DapperUnitOfWork.Samples.Shared.Models;
using DapperUnitOfWork.Samples.Shared.Repositories;
using DapperUnitOfWork.UnitOfWorks;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DapperUnitOfWork.Samples.Shared.UnitOfWorks
{
    /// <summary>
    /// Sample single DbContext unit of work
    /// </summary>
    /// <remarks>
    /// Pass the incoming dbContext into the base ctor to:
    ///     1) set Base.DbContext;
    ///     2) override repositories DbContext for shared use and participation in transactions;
    ///     3) allow the base to handle disposing of the dbContext;
    /// Pass the incoming repositories into the base ctor to:
    ///     1) override repositories DbContext for shared use and participation in transactions;
    ///     2) allow the base to handle disposing of the repositories;
    /// 
    /// Explore methods of base class.
    /// </remarks>
    public class NorthwindSingleDbContextUnitOfWork : BaseSingleDbContextUnitOfWork<INorthwindDbContext>, INorthwindSingleDbContextUnitOfWork
    {
        private readonly ICategoryNorthwindRepository _categoryNorthwindRepository;

        public NorthwindSingleDbContextUnitOfWork(INorthwindDbContext dbContext, ICategoryNorthwindRepository categoryNorthwindRepository)
            : base(dbContext, categoryNorthwindRepository) =>
            _categoryNorthwindRepository = categoryNorthwindRepository;


        public async Task<IEnumerable<CategoryModel>> InsertCategoriesAsync(IEnumerable<CategoryModel> categoryModels)
        {
            var results = new List<CategoryModel>();

            try
            {
                BeginTransaction();

                foreach (var categoryModel in categoryModels as CategoryModel[] ?? categoryModels.ToArray())
                {
                    var result = await _categoryNorthwindRepository.InsertAsync(categoryModel).ConfigureAwait(false);
                    results.Add(result);
                }

                CommitTransaction();
            }
            finally
            {
                CloseConnection();
            }

            return results;
        }
    }

    public interface INorthwindSingleDbContextUnitOfWork : IBaseSingleDbContextUnitOfWork
    {
        Task<IEnumerable<CategoryModel>> InsertCategoriesAsync(IEnumerable<CategoryModel> categoryModels);
    }
}
