using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Models;
using Pattern.Tests.Data;
using Repository.Pattern.EfCore;
using Repository.Pattern.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Pattern.Tests.UnitTests
{
	[TestClass]
	public class ParentChildrenTests : Entity
	{
		[TestMethod]
		public void InsertChildrenTest()
		{
			string[] contextFactoryArgs = { "InsertChildrenTest" };
			using (var context = new InMemoryApplicationDbContextFactory().CreateDbContext(contextFactoryArgs))
			{
				var father = new Parent("Father");
				var mother = new Parent("Mother");
				var child1 = new Child("Child 1");
				var child2 = new Child("Child 2");
				var child3 = new Child("Child 3");

				IUnitOfWorkAsync unitOfWork = new UnitOfWork(context);
				unitOfWork.Repository<Parent>().Insert(father);
				unitOfWork.Repository<Parent>().Insert(mother);
				unitOfWork.SaveChanges();

				Assert.AreEqual(2, context.Parents.ToList().Count());

				father.AddChild(child1);
				Assert.AreEqual(child1, father.ParentChildren[0].Child);
				Assert.AreEqual(father, child1.ParentChildren[0].Parent);

				unitOfWork.Repository<Parent>().Update(father);
				unitOfWork.SaveChanges();

				var child = context.Parents.SingleOrDefault(x => x.Name == father.Name).ParentChildren.SingleOrDefault(x => x.Child.Name == child1.Name).Child;
				Assert.AreEqual(child1, child);

				unitOfWork.Repository<Child>().Delete(child1);
				unitOfWork.SaveChanges();

				Assert.IsFalse(context.Children.Any());

				father.AddChild(child2);
				unitOfWork.Repository<Parent>().Update(father);

				var fatherFromDb = context.Parents.Include(parent => parent.ParentChildren).SingleOrDefault(x => x.Name == father.Name);
				Assert.AreEqual(1, fatherFromDb.ParentChildren.Count());
			}
		}
	}
}
