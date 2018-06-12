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
    using System.Diagnostics;
    using System.Linq;
    using NUnit.Framework;

    /// <summary>
    /// This is a base class for unit test classes where the context is a STATIC instance, shared for all tests classes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [TestFixture]
    public class IntegrationsTestsBase<T> where T : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        protected IntegrationsTestsBase()
        {
        }

        private DbContextProvider<T> dbContextProvider;
        /// <summary>
        /// Component responsible for providing the database context
        /// </summary>
        protected virtual DbContextProvider<T> DbContextProvider
        {
            get
            {
                if (this.dbContextProvider == null)
                {
                    this.dbContextProvider = new DbContextProvider<T>();
                }
                return this.dbContextProvider;
            }
        }
        private T context;

        /// <summary>
        /// The DbContext
        /// </summary>
        public T Context
        {
            get
            {
                if (this.context == null)
                {
                    this.context = this.DbContextProvider.GetContextInstance();
                }
                return this.context;
            }
            set => this.context = value;
        }

        /// <summary>
        /// Your seeding action for the database
        /// </summary>
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        protected virtual Action SeedAction { get; }
        /// <summary>
        /// Specify whether the DB should be dropped and restored with each test run
        /// </summary>
        protected virtual bool ForceDatabaseResetWithEachTest { get; set; } = true;

        /// <summary>
        /// Resets the change tracker
        /// </summary>
        [TearDown]
        public virtual void ResetChangeTracker()
        {
            IEnumerable<DbEntityEntry> changedEntriesCopy = this.Context.ChangeTracker.Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified || e.State == EntityState.Deleted);
            foreach (DbEntityEntry entity in changedEntriesCopy)
            {
                this.Context.Entry(entity.Entity).State = EntityState.Detached;
            }
        }

        /// <summary>
        /// Performs the test setup
        /// </summary>
        [SetUp]
        public virtual void Setup()
        {
            Database.SetInitializer<T>(new DropCreateDatabaseAlwaysWithConnectionClose());
            if (this.ForceDatabaseResetWithEachTest)
            {
                this.ResetChangeTracker();
                this.Context.Database.Delete();
                this.Context.Dispose();
                this.Context = this.DbContextProvider.GetContextInstance();
            }
            this.Context.Database.Initialize(this.ForceDatabaseResetWithEachTest);
            this.SeedAction?.Invoke();
        }
    }
}