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

    /// <summary>
    /// This is a base class for unit test classes where the context is a STATIC instance, shared for all tests classes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class StaticContextIntegrationsTestsBase<T> where T : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        protected StaticContextIntegrationsTestsBase()
        {
        }

        private const string ConnectionString =
            @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=IntegrationTestHelpersDb;Integrated Security=SSPI;MultipleActiveResultSets=true;";

        private static readonly T ContextBackingField = StaticContextIntegrationsTestsBase<T>.GetContextInstance<T>();

        /// <summary>
        /// The DbContext
        /// </summary>
        public T Context => StaticContextIntegrationsTestsBase<T>.ContextBackingField;

        /// <summary>
        /// Constructs an instance of the context using Activator
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
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

        /// <summary>
        /// Resets the change tracker
        /// </summary>
        [TearDown]
        public virtual void ResetChangeTracker()
        {
            IEnumerable<DbEntityEntry> changedEntriesCopy = StaticContextIntegrationsTestsBase<T>.ContextBackingField.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);
            foreach (DbEntityEntry entity in changedEntriesCopy)
            {
                StaticContextIntegrationsTestsBase<T>.ContextBackingField.Entry(entity.Entity).State = EntityState.Detached;
            }
        }

        /// <summary>
        /// Performs the test setup
        /// </summary>
        [SetUp]
        public virtual void Setup()
        {
            Database.SetInitializer(new DropCreateDatabaseAlwaysAndSeed());
            StaticContextIntegrationsTestsBase<T>.ContextBackingField.Database.Initialize(false);
        }
    }
}