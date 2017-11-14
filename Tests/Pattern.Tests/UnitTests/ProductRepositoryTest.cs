using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Pattern.Tests.Data;
using Repository.Pattern.EfCore;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrackableEntities.Common.Core;

namespace Pattern.Tests.UnitTests
{
	[TestClass]
	public class ProductRepositoryTest
	{
		[TestMethod]
		public void DeleteProductById()
		{
			string[] contextFactoryArgs = { "DeleteProductById" };
			using (var context = new InMemoryApplicationDbContextFactory().CreateDbContext(contextFactoryArgs))
			{
				IUnitOfWorkAsync unitOfWork = new UnitOfWork(context);
				unitOfWork.Repository<Product>().Insert(new Product
				{
					ProductID = 2,
					Discontinued = true,
					TrackingState = TrackingState.Added
				});

				unitOfWork.SaveChanges();

				unitOfWork.Repository<Product>().Delete(2);

				unitOfWork.SaveChanges();

				var product = unitOfWork.Repository<Product>().Find(2);

				Assert.IsNull(product);
			}
		}

		[TestMethod]
		public void DeepLoadProductWithSupplier()
		{
			string[] contextFactoryArgs = { "DeepLoadProductWithSupplier" };
			using (var context = new InMemoryApplicationDbContextFactory().CreateDbContext(contextFactoryArgs))
			{
				IUnitOfWork unitOfWork = new UnitOfWork(context);
				unitOfWork.Repository<Supplier>().Insert(new Supplier
				{
					SupplierID = 1,
					CompanyName = "Nokia",
					City = "Tampere",
					Country = "Finland",
					ContactName = "Stephen Elop",
					ContactTitle = "CEO",
					TrackingState = TrackingState.Added
				});
				unitOfWork.Repository<Product>().Insert(new Product
				{
					ProductID = 2,
					Discontinued = true,
					ProductName = "Nokia Lumia 1520",
					SupplierID = 1,
					TrackingState = TrackingState.Added
				});

				unitOfWork.SaveChanges();

				var product = unitOfWork.Repository<Product>().Find(2).Supplier;

				Assert.IsNotNull(product);
			}
		}

		[TestMethod]
		public void DeleteProductByProduct()
		{
			string[] contextFactoryArgs = { "DeleteProductByProduct" };
			using (var context = new InMemoryApplicationDbContextFactory().CreateDbContext(contextFactoryArgs))
			{
				IUnitOfWork unitOfWork = new UnitOfWork(context);
				unitOfWork.Repository<Product>().Insert(new Product
				{
					ProductID = 2,
					Discontinued = true,
					TrackingState = TrackingState.Added
				});

				unitOfWork.SaveChanges();

				var product = unitOfWork.Repository<Product>().Find(2);

				product.TrackingState = TrackingState.Deleted;

				unitOfWork.Repository<Product>().Delete(product);

				unitOfWork.SaveChanges();

				var productDeleted = unitOfWork.Repository<Product>().Find(2);

				Assert.IsNull(productDeleted);
			}
		}

		[TestMethod]
		public void FindProductById()
		{
			string[] contextFactoryArgs = { "FindProductById" };
			using (var context = new InMemoryApplicationDbContextFactory().CreateDbContext(contextFactoryArgs))
			{
				IUnitOfWork unitOfWork = new UnitOfWork(context);
				unitOfWork.Repository<Product>().Insert(new Product
				{
					ProductID = 1,
					Discontinued = false,
					TrackingState = TrackingState.Added
				});
				unitOfWork.Repository<Product>().Insert(new Product
				{
					ProductID = 2,
					Discontinued = true,
					TrackingState = TrackingState.Added
				});
				unitOfWork.Repository<Product>().Insert(new Product
				{
					ProductID = 3,
					Discontinued = true,
					TrackingState = TrackingState.Added
				});

				unitOfWork.SaveChanges();

				var product = unitOfWork.Repository<Product>().Find(2);

				Assert.IsNotNull(product);
				Assert.AreEqual(2, product.ProductID);
			}
		}

		[TestMethod]
		public void GetProductsExecutesQuery()
		{
			string[] contextFactoryArgs = { "GetProductsExecutesQuery" };
			using (var context = new InMemoryApplicationDbContextFactory().CreateDbContext(contextFactoryArgs))
			{
				IUnitOfWork unitOfWork = new UnitOfWork(context);
				var products = unitOfWork.Repository<Product>().Query().Select().ToList();
				Assert.IsInstanceOfType(products, typeof(List<Product>));
			}
		}

		[TestMethod]
		public void GetProductsThatHaveBeenDiscontinued()
		{
			string[] contextFactoryArgs = { "GetProductsThatHaveBeenDiscontinued" };
			using (var context = new InMemoryApplicationDbContextFactory().CreateDbContext(contextFactoryArgs))
			{
				IUnitOfWork unitOfWork = new UnitOfWork(context);
				unitOfWork.Repository<Product>().Insert(new Product
				{
					ProductID = 1,
					Discontinued = false,
					TrackingState = TrackingState.Added
				});
				unitOfWork.Repository<Product>().Insert(new Product
				{
					ProductID = 2,
					Discontinued = true,
					TrackingState = TrackingState.Added
				});
				unitOfWork.Repository<Product>().Insert(new Product
				{
					ProductID = 3,
					Discontinued = true,
					TrackingState = TrackingState.Added
				});

				unitOfWork.SaveChanges();

				var discontinuedProducts = unitOfWork.Repository<Product>().Query(t => t.Discontinued).Select();

				Assert.AreEqual(2, discontinuedProducts.Count());
			}
		}

		[TestMethod]
		public void InsertProduct()
		{
			string[] contextFactoryArgs = { "InsertProduct" };
			using (var context = new InMemoryApplicationDbContextFactory().CreateDbContext(contextFactoryArgs))
			{
				IUnitOfWork unitOfWork = new UnitOfWork(context);
				unitOfWork.Repository<Product>().Insert(new Product
				{
					ProductID = 1,
					Discontinued = false,
					TrackingState = TrackingState.Added
				});
				unitOfWork.Repository<Product>().Insert(new Product
				{
					ProductID = 2,
					Discontinued = true,
					TrackingState = TrackingState.Added
				});
				unitOfWork.Repository<Product>().Insert(new Product
				{
					ProductID = 3,
					Discontinued = true,
					TrackingState = TrackingState.Added
				});

				unitOfWork.SaveChanges();

				var product = unitOfWork.Repository<Product>().Find(2);

				Assert.IsNotNull(product);
				Assert.AreEqual(2, product.ProductID);
			}
		}

		[TestMethod]
		public void InsertRangeOfProducts()
		{
			string[] contextFactoryArgs = { "InsertRangeOfProducts" };
			using (var context = new InMemoryApplicationDbContextFactory().CreateDbContext(contextFactoryArgs))
			{
				IUnitOfWork unitOfWork = new UnitOfWork(context);
				var newProducts = new[]
				{
				new Product {ProductID = 1, Discontinued = false, TrackingState = TrackingState.Added},
				new Product {ProductID = 2, Discontinued = true, TrackingState = TrackingState.Added},
				new Product {ProductID = 3, Discontinued = true, TrackingState = TrackingState.Added}
			};

				unitOfWork.Repository<Product>().InsertRange(newProducts);
				unitOfWork.SaveChanges();

				var savedProducts = unitOfWork.Repository<Product>().Query().Select();

				Assert.AreEqual(savedProducts.Count(), newProducts.Length);
			}
		}

		[TestMethod]
		public void UpdateProduct()
		{
			string[] contextFactoryArgs = { "UpdateProduct" };
			using (var context = new InMemoryApplicationDbContextFactory().CreateDbContext(contextFactoryArgs))
			{
				IUnitOfWork unitOfWork = new UnitOfWork(context);
				unitOfWork.Repository<Product>().Insert(new Product
				{
					ProductID = 2,
					Discontinued = true,
					TrackingState = TrackingState.Added
				});

				unitOfWork.SaveChanges();

				var product = unitOfWork.Repository<Product>().Find(2);

				Assert.AreEqual(product.Discontinued, true, "Assert we are able to find the inserted Product.");

				product.Discontinued = false;
				product.TrackingState = TrackingState.Modified;

				unitOfWork.Repository<Product>().Update(product);
				unitOfWork.SaveChanges();

				Assert.AreEqual(product.Discontinued, false, "Assert that our changes were saved.");
			}
		}

		[TestMethod]
		public async Task FindProductKeyAsync()
		{
			string[] contextFactoryArgs = { "FindProductKeyAsync" };
			using (var context = new InMemoryApplicationDbContextFactory().CreateDbContext(contextFactoryArgs))
			{
				IUnitOfWorkAsync unitOfWork = new UnitOfWork(context);
				unitOfWork.Repository<Product>().Insert(new Product { ProductID = 2, Discontinued = true });

				unitOfWork.SaveChanges();

				var product = await unitOfWork.RepositoryAsync<Product>().FindAsync(2);

				Assert.AreEqual(product.ProductID, 2);
			}
		}
	}
}
