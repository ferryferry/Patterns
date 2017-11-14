using Repository.Pattern.EfCore;
using System;
namespace Models
{
	public class CustomerOrder : Entity
	{
		public int Id { get; set; }
		public string CustomerId { get; set; }
		public string ContactName { get; set; }
		public int OrderId { get; set; }
		public DateTime? OrderDate { get; set; }
	}
}
