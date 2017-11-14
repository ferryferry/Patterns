using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Models;

namespace Pattern.Tests.Data
{
	public class LocalDbApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
	{
		public ApplicationDbContext CreateDbContext(string[] args)
		{
			var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
			optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=aspnet-WebApplication1-31AED7D4-EFC3-4C6C-9DA0-8AF6CBF75DE1;Trusted_Connection=True;MultipleActiveResultSets=true");

			return new ApplicationDbContext(optionsBuilder.Options);
		}
	}
}
