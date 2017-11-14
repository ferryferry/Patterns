using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Models;
using System;

namespace Pattern.Tests.Data
{
	public class InMemoryApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
	{
		public ApplicationDbContext CreateDbContext(string[] args)
		{
			string databaseName = Guid.NewGuid().ToString();
			if (args != null)
				databaseName = args[0];

			var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
			optionsBuilder.UseInMemoryDatabase(databaseName);

			return new ApplicationDbContext(optionsBuilder.Options);
		}
	}
}
