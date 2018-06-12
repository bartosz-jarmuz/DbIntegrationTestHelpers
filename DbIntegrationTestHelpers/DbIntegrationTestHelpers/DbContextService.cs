using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbIntegrationTestHelpers
{
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;

    internal static class DbContextService
    {

        public static void DropDatabase(DbContext context)
        {
            ResetChangeTracker(context);
            context.Database.Delete();
            context.Dispose();
        }

        public static void ResetChangeTracker(DbContext context)
        {
            IEnumerable<DbEntityEntry> changedEntriesCopy = context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);
            foreach (DbEntityEntry entity in changedEntriesCopy)
            {
                context.Entry(entity.Entity).State = EntityState.Detached;
            }
        }

        public static T CreateInstance<T>(string connectionString)
        {
            try
            {
                var type = typeof(T);
                var ctx = (T)Activator.CreateInstance(type, connectionString);

                return ctx;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Your DB context has to have a constructor which allows setting a connection string", ex);
            }
        }
    }
}
