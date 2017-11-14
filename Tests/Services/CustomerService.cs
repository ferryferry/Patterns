using Models;
using Repositories;
using Repository.Pattern.Repositories;
using Service.Pattern;
using System.Collections.Generic;

namespace Services
{
	public interface ICustomerService : IService<Customer>
	{
		decimal CustomerOrderTotalByYear(string customerId, int year);
		IEnumerable<Customer> CustomersByCompany(string companyName);
		IEnumerable<CustomerOrder> GetCustomerOrder(string country);
	}

	public class CustomerService : Service<Customer>, ICustomerService
	{
		private readonly IRepositoryAsync<Customer> _repository;

		public CustomerService(IRepositoryAsync<Customer> repository) : base(repository)
		{
			_repository = repository;
		}

		public decimal CustomerOrderTotalByYear(string customerId, int year)
		{
			// add business logic here
			return _repository.GetCustomerOrderTotalByYear(customerId, year);
		}

		public IEnumerable<Customer> CustomersByCompany(string companyName)
		{
			// add business logic here
			return _repository.CustomersByCompany(companyName);
		}

		public IEnumerable<CustomerOrder> GetCustomerOrder(string country)
		{
			// add business logic here
			return _repository.GetCustomerOrder(country);
		}

		public override void Insert(Customer entity)
		{
			// e.g. add business logic here before inserting
			base.Insert(entity);
		}

		public override void Delete(object id)
		{
			// e.g. add business logic here before deleting
			base.Delete(id);
		}
	}
}
