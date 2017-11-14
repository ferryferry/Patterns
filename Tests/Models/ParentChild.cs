namespace Models
{
	public class ParentChild
	{
		public int Id { get; set; }
		public Parent Parent { get; set; }
		public int ParentId { get; set; }

		public Child Child { get; set; }
		public int ChildId { get; set; }
	}
}