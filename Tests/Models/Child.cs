using Repository.Pattern.EfCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
	public class Child : Entity
	{
		public Child()
		{
			ParentChildren = new List<ParentChild>();
		}
		public Child(string name)
		{
			ParentChildren = new List<ParentChild>();
			Name = name;
		}

		public int Id { get; set; }
		public string Name { get; set; }
		public List<ParentChild> ParentChildren { get; set; }

		public void AddParent(Parent parent)
		{
			var parentChild = new ParentChild() { Child = this, Parent = parent };
			ParentChildren.Add(parentChild);
			parent.ParentChildren.Add(parentChild);
		}
	}
}
