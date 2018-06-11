// -----------------------------------------------------------------------
//  <copyright file="IntegrationsTestsBase.cs" company="SDL plc">
//   Copyright (c) SDL plc. All rights reserved.
//  </copyright>
// -----------------------------------------------------------------------

namespace DbIntegrationTestHelpers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Linq;
    using NUnit.Framework;

    public class StaticContextIntegrationsTestsBase<T> where T : DbContext
    {
        protected StaticContextIntegrationsTestsBase()
        {
        }

        private const string ConnectionString =
            @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=telimena-AutomatedTestsDb;Integrated Security=SSPI;MultipleActiveResultSets=true;";

        private static readonly T ContextBackingField = StaticContextIntegrationsTestsBase<T>.GetContextInstance<T>();
        public T Context => StaticContextIntegrationsTestsBase<T>.ContextBackingField;

        public static T GetContextInstance<T>() where T : DbContext
        {
            try
            {
                var type = typeof(T);
                var ctx = (T) Activator.CreateInstance(type, StaticContextIntegrationsTestsBase<T>.ConnectionString);

                return ctx;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Your DB context has to have a constructor which allows setting a connection string", ex);
            }
        }

        [TearDown]
        public void ResetChangeTracker()
        {
            IEnumerable<DbEntityEntry> changedEntriesCopy = StaticContextIntegrationsTestsBase<T>.ContextBackingField.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added ||
                            e.State == EntityState.Modified ||
                            e.State == EntityState.Deleted
                );
            foreach (DbEntityEntry entity in changedEntriesCopy)
            {
                StaticContextIntegrationsTestsBase<T>.ContextBackingField.Entry(entity.Entity).State = EntityState.Detached;
            }
        }

        [SetUp]
        public void Setup()
        {
            Database.SetInitializer(new DropCreateDatabaseAlwaysAndSeed());
            StaticContextIntegrationsTestsBase<T>.ContextBackingField.Database.Initialize(false);
        }
    }
}