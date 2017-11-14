using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using TrackableEntities.Common.Core;
using TrackableEntities.EF.Core;
using Repository.Pattern.DataContext;

namespace Repository.Pattern.EfCore
{
    public class DataContext : DbContext, IDataContextAsync
    {
        private readonly Guid _instanceId;

        public DataContext(DbContextOptions options) : base(options)
        {
            _instanceId = Guid.NewGuid();
        }

        public Guid InstanceId { get { return _instanceId; } }

        /// <summary>
        ///     Saves all changes made in this context to the underlying database.
        /// </summary>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.</exception>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually
        ///     indicates an optimistic concurrency violation; that is, a row has been changed
        ///     in the database since it was queried.</exception>
        /// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.</exception>
        /// <exception cref="System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple
        ///     asynchronous commands concurrently on the same context instance.</exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     The context or connection have been disposed.</exception>
        /// <exception cref="System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either
        ///     before or after sending commands to the database.</exception>
        /// <seealso cref="DbContext.SaveChanges"/>
        /// <returns>The number of objects written to the underlying database.</returns>
        public override int SaveChanges()
        {
            SyncObjectsStatePreCommit();
            var changes = base.SaveChanges();
            SyncObjectsStatePostCommit();
            return changes;
        }

        /// <summary>
        ///     Asynchronously saves all changes made in this context to the underlying database.
        /// </summary>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">
        ///     An error occurred sending updates to the database.</exception>
        /// <exception cref="System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
        ///     A database command did not affect the expected number of rows. This usually
        ///     indicates an optimistic concurrency violation; that is, a row has been changed
        ///     in the database since it was queried.</exception>
        /// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">
        ///     The save was aborted because validation of entity property values failed.</exception>
        /// <exception cref="System.NotSupportedException">
        ///     An attempt was made to use unsupported behavior such as executing multiple
        ///     asynchronous commands concurrently on the same context instance.</exception>
        /// <exception cref="System.ObjectDisposedException">
        ///     The context or connection have been disposed.</exception>
        /// <exception cref="System.InvalidOperationException">
        ///     Some error occurred attempting to process entities in the context either
        ///     before or after sending commands to the database.</exception>
        /// <seealso cref="DbContext.SaveChangesAsync"/>
        /// <returns>A task that represents the asynchronous save operation.  The 
        ///     <see cref="Task.Result">Task.Result</see> contains the number of 
        ///     objects written to the underlying database.</returns>
		public override async Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
		{
			return await SaveChangesAsync(cancellationToken);
		}
		/// <summary>
		///     Asynchronously saves all changes made in this context to the underlying database.
		/// </summary>
		/// <exception cref="System.Data.Entity.Infrastructure.DbUpdateException">
		///     An error occurred sending updates to the database.</exception>
		/// <exception cref="System.Data.Entity.Infrastructure.DbUpdateConcurrencyException">
		///     A database command did not affect the expected number of rows. This usually
		///     indicates an optimistic concurrency violation; that is, a row has been changed
		///     in the database since it was queried.</exception>
		/// <exception cref="System.Data.Entity.Validation.DbEntityValidationException">
		///     The save was aborted because validation of entity property values failed.</exception>
		/// <exception cref="System.NotSupportedException">
		///     An attempt was made to use unsupported behavior such as executing multiple
		///     asynchronous commands concurrently on the same context instance.</exception>
		/// <exception cref="System.ObjectDisposedException">
		///     The context or connection have been disposed.</exception>
		/// <exception cref="System.InvalidOperationException">
		///     Some error occurred attempting to process entities in the context either
		///     before or after sending commands to the database.</exception>
		/// <seealso cref="DbContext.SaveChangesAsync"/>
		/// <returns>A task that represents the asynchronous save operation.  The 
		///     <see cref="Task.Result">Task.Result</see> contains the number of 
		///     objects written to the underlying database.</returns>
		public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
        {
            SyncObjectsStatePreCommit();
            var changesAsync = await base.SaveChangesAsync(cancellationToken);
            SyncObjectsStatePostCommit();
            return changesAsync;
        }

		public async Task<int> SaveChangesAsync()
		{
			return await base.SaveChangesAsync();
		}

		public void SyncObjectState<TEntity>(TEntity entity) where TEntity : class, ITrackable
		{
			this.ApplyChanges(entity);
		}

		private void SyncObjectsStatePreCommit()
		{
			var entities = ChangeTracker.Entries().Select(x => x.Entity).OfType<ITrackable>();
			this.UpdateRange(entities);
		}

		public void SyncObjectsStatePostCommit()
		{
			foreach (var dbEntityEntry in ChangeTracker.Entries())
			{
				((ITrackable)dbEntityEntry.Entity).TrackingState = StateHelper.ConvertState(dbEntityEntry.State);
			}
		}
	}
}