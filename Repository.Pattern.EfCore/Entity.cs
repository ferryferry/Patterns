using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;
using TrackableEntities.Common.Core;

namespace Repository.Pattern.EfCore
{
	public abstract class Entity : ITrackable
	{
		[NotMapped]
		public TrackingState TrackingState { get; set; }

		[NotMapped]
		public ICollection<string> ModifiedProperties { get; set; }
	}
}