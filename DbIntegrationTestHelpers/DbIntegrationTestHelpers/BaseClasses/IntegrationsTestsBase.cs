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
    /// This is a base class for unit test classes where the context is an instance, reset with each test <para/>
    /// This approach is slightly slower than the StaticIntegrationsTestBase, however it means a fresh db is created and seeded for each test method
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [TestFixture]
    public abstract class IntegrationsTestsBase<T> : HelpersBase<T> where T : DbContext
    {

        private T _context;

        /// <summary>
        /// The DbContext
        /// </summary>
        public override T Context
        {
            get
            {
                if (this._context == null)
                {
                    this._context = this.DbContextProvider.GetContextInstance();
                }
                return this._context;
            }
            protected set => this._context = value;
        }

        /// <summary>
        /// Resets the change tracker
        /// </summary>
        [TearDown]
        public virtual void TearDown()
        {
            DbContextService.ResetChangeTracker(this.Context);
        }

        /// <summary>
        /// Performs the test setup
        /// </summary>
        [SetUp]
        public virtual void SetUp()
        {
            Database.SetInitializer<T>(new DropCreateDatabaseAlwaysWithConnectionClose());
            DbContextService.DropDatabase(this.Context);
            this.Context = this.DbContextProvider.GetContextInstance();
            this.Context.Database.Initialize(true);
            this.SeedAction?.Invoke();
        }
    }
}