using DapperUnitOfWork.DbContexts;

namespace DapperUnitOfWork.Shared.DbContexts
{
	public class NorthwindDbContextMetadata : INorthwindDbContextMetadata
	{
		public string DbConnectionString { get; } = @"";
		public bool OpenConnectionOnConnectionCreation { get; }
	}

	public interface INorthwindDbContextMetadata : IDbContextMetadata
	{
	}
}
