namespace DbIntegrationTestHelpers
{
    using System;
    using System.Data.Entity;

    /// <summary>
    /// Base class for all ingratation test helper bases classes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class HelpersBase<T> where T : DbContext
    {
        /// <summary>
        /// The DbContext
        /// </summary>
        public abstract T Context { get; protected set; }

        private DbContextProvider<T> _dbContextProvider;

        /// <summary>
        /// Your seeding action for the database
        /// </summary>
        // ReSharper disable once UnassignedGetOnlyAutoProperty
        protected virtual Action SeedAction { get; }
        /// <summary>
        /// Component responsible for providing the database context
        /// </summary>
        protected virtual DbContextProvider<T> DbContextProvider
        {
            get
            {
                if (this._dbContextProvider == null)
                {
                    this._dbContextProvider = new DbContextProvider<T>();
                }
                return this._dbContextProvider;
            }
        }
    }
}