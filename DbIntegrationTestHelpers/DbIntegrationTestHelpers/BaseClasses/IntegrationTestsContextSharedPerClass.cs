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
    public abstract class IntegrationTestsContextSharedPerClass<T> : HelpersBase<T> where T : DbContext
    {

        /// <summary>
        /// The DbContext
        /// </summary>
        public override T Context { get; protected set; }

        /// <summary>
        /// Resets the change tracker
        /// </summary>
        [OneTimeTearDown]
        public virtual void TearDown()
        {
            DbContextService.ResetChangeTracker(this.Context);
        }

        /// <summary>
        /// Performs the test setup
        /// </summary>
        [OneTimeSetUp]
        public virtual void SetUp()
        {
            this.Context = this.DbContextProvider.GetContextInstance();
            Database.SetInitializer<T>(new DropCreateDatabaseAlwaysWithConnectionClose());
            this.Context.Database.Initialize(true);
            this.SeedAction?.Invoke();
        }
    }
}