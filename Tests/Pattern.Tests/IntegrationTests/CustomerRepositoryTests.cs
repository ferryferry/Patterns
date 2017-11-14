using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Pattern.Tests.Data;
using Repository.Pattern.EfCore;
using Repository.Pattern.Repositories;
using Repository.Pattern.UnitOfWork;
using System;
using System.Linq;
using TrackableEntities.Common.Core;

namespace Pattern.Tests.IntegrationTests
{
	[TestClass]
	public class CustomerRepositoryTests
	{
		[TestMethod]
		public void CreateCustomerTest()
		{
			string[] contextFactoryArgs = { "CreateCustomerTest" };
			using (var context = new InMemoryApplicationDbContextFactory().CreateDbContext(contextFactoryArgs))
			{
				IUnitOfWorkAsync unitOfWork = new UnitOfWork(context);
				IRepositoryAsync<Customer> customerRepository = new Repository<Customer>(context, unitOfWork);

				var customer = new Customer
				{
					CustomerID = "LLE37",
					CompanyName = "CBRE",
					ContactName = "Long Le",
					ContactTitle = "App/Dev Architect",
					Address = "11111 Sky Ranch",
					City = "Dallas",
					PostalCode = "75042",
					Country = "USA",
					Phone = "(222) 222-2222",
					Fax = "(333) 333-3333",
					TrackingState = TrackingState.Added,
				};

				customerRepository.Insert(customer);
				unitOfWork.SaveChanges();
			}

			//  Query for newly created customer by ID from a new context, to ensure it's not pulling from cache
			using (var context = new InMemoryApplicationDbContextFactory().CreateDbContext(contextFactoryArgs))
			{
				IUnitOfWorkAsync unitOfWork = new UnitOfWork(context);
				IRepositoryAsync<Customer> customerRepository = new Repository<Customer>(context, unitOfWork);
				var customer = customerRepository.Find("LLE37");
				Assert.AreEqual(customer.CustomerID, "LLE37");
			}
		}

		[TestMethod]
		public void CreateAndUpdateAndDeleteCustomerGraphTest()
		{
			string[] contextFactoryArgs = { "CreateAndUpdateAndDeleteCustomerGraphTest" };
			// Create new customer
			using (var context = new InMemoryApplicationDbContextFactory().CreateDbContext(contextFactoryArgs))
			{
				IUnitOfWorkAsync unitOfWork = new UnitOfWork(context);
				IRepositoryAsync<Customer> customerRepository = new Repository<Customer>(context, unitOfWork);

				var customer = new Customer
				{
					CustomerID = "LLE38",
					CompanyName = "CBRE",
					ContactName = "Long Le",
					ContactTitle = "App/Dev Architect",
					Address = "11111 Sky Ranch",
					City = "Dallas",
					PostalCode = "75042",
					Country = "USA",
					Phone = "(222) 222-2222",
					Fax = "(333) 333-3333",
					TrackingState = TrackingState.Added,
					Orders = new[]
					{
						new Order()
						{
							CustomerID = "LLE38",
							EmployeeID = 1,
							OrderDate = DateTime.Now,
							TrackingState = TrackingState.Added,
						},
						new Order()
						{
							CustomerID = "LLE39",
							EmployeeID = 1,
							OrderDate = DateTime.Now,
							TrackingState = TrackingState.Added
						},
					}
				};

				unitOfWork.Repository<Customer>().ApplyChanges(customer);
				unitOfWork.SaveChanges();
			}

			Customer customerForUpdateDeleteGraphTest = null;

			//  Query for newly created customer by ID from a new context, to ensure it's not pulling from cache
			using (var context = new InMemoryApplicationDbContextFactory().CreateDbContext(contextFactoryArgs))
			{
				IUnitOfWorkAsync unitOfWork = new UnitOfWork(context);
				IRepositoryAsync<Customer> customerRepository = new Repository<Customer>(context, unitOfWork);

				customerForUpdateDeleteGraphTest = customerRepository
					.Query(x => x.CustomerID == "LLE38")
					.Include(x => x.Orders)
					.Select()
					.SingleOrDefault();

				// Testing that customer was created
				Assert.AreEqual(customerForUpdateDeleteGraphTest?.CustomerID, "LLE38");

				// Testing that orders in customer graph were created
				Assert.IsTrue(customerForUpdateDeleteGraphTest?.Orders.Count == 2);

				// Make changes to the object graph while in this context, will save these 
				// changes in another context, testing managing states between and/or while disconnected
				// from the orginal DataContext

				// Updating the customer in the graph
				customerForUpdateDeleteGraphTest.City = "Houston";
				customerForUpdateDeleteGraphTest.TrackingState = TrackingState.Modified;

				// Updating the order in the graph
				var firstOrder = customerForUpdateDeleteGraphTest.Orders.Take(1).Single();
				firstOrder.ShipCity = "Houston";
				firstOrder.TrackingState = TrackingState.Modified;

				// Deleting one of the orders from the graph
				var secondOrder = customerForUpdateDeleteGraphTest.Orders.Skip(1).Take(1).Single();
				secondOrder.TrackingState = TrackingState.Deleted;
			}

			//  Query for newly created customer by ID from a new context, to ensure it's not pulling from cache
			using (var context = new InMemoryApplicationDbContextFactory().CreateDbContext(contextFactoryArgs))
			{
				IUnitOfWorkAsync unitOfWork = new UnitOfWork(context);
				IRepositoryAsync<Customer> customerRepository = new Repository<Customer>(context, unitOfWork);

				// Testing changes to graph while disconncted from it's orginal DataContext
				// Saving changes while graph was previous DataContext that was already disposed
				customerRepository.ApplyChanges(customerForUpdateDeleteGraphTest);
				unitOfWork.SaveChanges();

				customerForUpdateDeleteGraphTest = customerRepository
					.Query(x => x.CustomerID == "LLE38")
					.Include(x => x.Orders)
					.Select()
					.SingleOrDefault();

				Assert.AreEqual(customerForUpdateDeleteGraphTest?.CustomerID, "LLE38");

				// Testing for order(2) was deleted from the graph
				Assert.IsTrue(customerForUpdateDeleteGraphTest?.Orders.Count == 1);

				// Testing that customer was updated in the graph
				Assert.IsTrue(customerForUpdateDeleteGraphTest.City == "Houston");

				// Testing that order was updated in the graph.
				Assert.IsTrue(customerForUpdateDeleteGraphTest.Orders.ToArray()[0].ShipCity == "Houston");
			}
		}
	}
}
