using Dapper;
using DapperUnitOfWork.Repositories;
using DapperUnitOfWork.Samples.Shared.DbContexts;
using DapperUnitOfWork.Samples.Shared.Models;
using DapperUnitOfWork.Samples.Shared.SqlStrings;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DapperUnitOfWork.Samples.Shared.Repositories
{
    /// <summary>
    /// Sample repository for the 'Categories' table / domain in the Northwind database
    /// </summary>
    /// <remarks>
    /// Pass the incoming dbContext into the base ctor to set Base.DbContext and allow the base to handle disposing of the dbContext.
    /// Make sure to include the DbContext.Transaction in every method so each method can participate in transaction(s).
    /// </remarks>
    public class CategoryNorthwindRepository : BaseRepository<INorthwindDbContext>, ICategoryNorthwindRepository
    {
        public CategoryNorthwindRepository(INorthwindDbContext dbContext) : base(dbContext)
        {
        }

        public Task<IEnumerable<CategoryModel>> GetAllAsync() =>
            DbContext.Connection.QueryAsync<CategoryModel>(CategoryNorthwindSqlStrings.SelectAll
                , null, DbContext.Transaction);

        public Task<CategoryModel> GetByIdAsync(int categoryId) =>
            DbContext.Connection.QueryFirstOrDefaultAsync<CategoryModel>(CategoryNorthwindSqlStrings.SelectById
                , new { categoryId }, DbContext.Transaction);

        public async Task<CategoryModel> InsertAsync(CategoryModel categoryModel)
        {
            categoryModel.CategoryId =
                await DbContext.Connection.ExecuteScalarAsync<int>(CategoryNorthwindSqlStrings.Insert
                    , new { categoryModel.CategoryName, categoryModel.Description }
                    , DbContext.Transaction).ConfigureAwait(false);

            return categoryModel;
        }

        public Task<int> UpdateAsync(CategoryModel categoryModel)
        {
            return DbContext.Connection.ExecuteAsync(CategoryNorthwindSqlStrings.Update
                , new { categoryModel.CategoryName, categoryModel.Description, categoryModel.CategoryId }
                , DbContext.Transaction);
        }

        public Task<int> DeleteAsync(int categoryId) =>
            DbContext.Connection.ExecuteAsync(CategoryNorthwindSqlStrings.DeleteById
                , new { categoryId }, DbContext.Transaction);
    }

    public interface ICategoryNorthwindRepository : IBaseRepository
    {
        Task<IEnumerable<CategoryModel>> GetAllAsync();
        Task<CategoryModel> GetByIdAsync(int categoryId);
        Task<CategoryModel> InsertAsync(CategoryModel categoryModel);
        Task<int> UpdateAsync(CategoryModel categoryModel);
        Task<int> DeleteAsync(int categoryId);
    }
}
