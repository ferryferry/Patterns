using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Pattern.Tests.Data;
using Repository.Pattern.EfCore;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using TrackableEntities.Common.Core;

namespace Pattern.Tests.IntegrationTests
{
	[TestClass]
	public class ChangeTrackerEntriesTest
	{
		[TestMethod]
		public void AddProducts()
		{
			string[] contextFactoryArgs = { "AddProducts" };

			for (var x = 0; x < 2; x++)
			{
				var products = new List<Product>();

				for (var i = 0; i < 100; i++)
				{
					products.Add(new Product
					{
						ProductName = Guid.NewGuid().ToString(),
						Discontinued = false,
						TrackingState = TrackingState.Added
					});
				}

				using (var context = new InMemoryApplicationDbContextFactory().CreateDbContext(contextFactoryArgs))
				{
					IUnitOfWorkAsync unitOfWork = new UnitOfWork(context);
					var northwindContext = context;
					Assert.IsFalse(northwindContext.ChangeTracker.Entries().Any());

					IRepositoryAsync<Product> productRepository =
						new Repository<Product>(context, unitOfWork);

					productRepository.InsertRange(products);
					products.Clear();
					unitOfWork.SaveChanges();

					Assert.IsTrue(northwindContext.ChangeTracker.Entries().Count() == 100);
				}

				System.Threading.Thread.Sleep(5000);
			}
		}
	}
}
