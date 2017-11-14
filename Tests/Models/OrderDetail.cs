using Repository.Pattern.EfCore;

namespace Models
{
	public partial class OrderDetail : Entity
	{
		public int Id { get; set; }
		public int OrderID { get; set; }
		public int ProductID { get; set; }
		public decimal UnitPrice { get; set; }
		public short Quantity { get; set; }
		public float Discount { get; set; }
		public virtual Order Order { get; set; }
		public virtual Product Product { get; set; }
	}
}
