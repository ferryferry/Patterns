﻿using System;
using TrackableEntities.Common.Core;

namespace Repository.Pattern.DataContext
{
    public interface IDataContext : IDisposable
    {
        int SaveChanges();
		void SyncObjectState<TEntity>(TEntity entity) where TEntity : class, ITrackable;
		void SyncObjectsStatePostCommit();
    }
}
