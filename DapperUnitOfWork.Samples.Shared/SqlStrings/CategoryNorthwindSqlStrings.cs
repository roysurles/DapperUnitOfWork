namespace DapperUnitOfWork.Samples.Shared.SqlStrings
{
    internal static class CategoryNorthwindSqlStrings
    {
        internal static string SelectAll { get; } =
            @"select CategoryId
              , CategoryName
              , Description
              from Categories";
        internal static string SelectById { get; } =
            @"select CategoryId
              , CategoryName
              , Description
              from Categories
              where CategoryID = @categoryId";

        internal static string Insert { get; } =
            @"insert into Categories (CategoryName, Description)
                values (@CategoryName, @Description)
              select SCOPE_IDENTITY()";

        internal static string Update { get; } =
            @"update Categories 
                set CategoryName = @CategoryName
                , Description = @Description
              where CategoryID = @CategoryId";

        internal static string DeleteById { get; } =
            @"delete
              from Categories
              where CategoryID = @categoryId";
    }
}
