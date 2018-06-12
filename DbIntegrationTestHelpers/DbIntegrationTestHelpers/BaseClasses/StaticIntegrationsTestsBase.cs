namespace DbIntegrationTestHelpers
{
    using System.Data.Entity;
    using NUnit.Framework;


    /// <summary>
    /// This is a base class for unit test classes where the db context is STATIC, shared for all tests classes
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [TestFixture]
    public abstract class StaticContextIntegrationsTestsBase<T> : HelpersBase<T> where T : DbContext
    {
       
        private static T context = new DbContextProvider<T>().GetContextInstance();

        /// <summary>
        /// The DB Context
        /// </summary>
        public override T Context
        {
            get => context;
            protected set => context = value;
        }


        /// <summary>
        /// Resets the change tracker
        /// </summary>
        [OneTimeTearDown]
        public virtual void OneTimeTearDown()
        {
            DbContextService.ResetChangeTracker(this.Context);
        }

        /// <summary>
        /// Performs the test setup
        /// </summary>
        [OneTimeSetUp]
        public virtual void OneTimeSetUp()
        {
            Database.SetInitializer<T>(new DropCreateDatabaseAlwaysWithConnectionClose());
            this.Context.Database.Initialize(false);
            this.SeedAction?.Invoke();
        }
    }
}