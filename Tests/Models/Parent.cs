using Repository.Pattern.EfCore;
using System.Collections.Generic;

namespace Models
{
	public class Parent : Entity
	{
		public Parent()
		{
			ParentChildren = new List<ParentChild>();
		}

		public Parent(string name)
		{
			ParentChildren = new List<ParentChild>();
			Name = name;
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public List<ParentChild> ParentChildren { get; set; }

		public void AddChild(Child child)
		{
			var parentChild = new ParentChild() { Child = child, Parent = this };
			ParentChildren.Add(parentChild);
			child.ParentChildren.Add(parentChild);
		}
	}
}