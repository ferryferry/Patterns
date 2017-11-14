﻿using Repository.Pattern.EfCore;
using System.Collections.Generic;

namespace Models
{
	public partial class Customer : Entity
	{
		public Customer()
		{
			Orders = new List<Order>();
		}

		public string CustomerID { get; set; }
		public string CompanyName { get; set; }
		public string ContactName { get; set; }
		public string ContactTitle { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string Region { get; set; }
		public string PostalCode { get; set; }
		public string Country { get; set; }
		public string Phone { get; set; }
		public string Fax { get; set; }
		public virtual ICollection<Order> Orders { get; set; }
	}
}
