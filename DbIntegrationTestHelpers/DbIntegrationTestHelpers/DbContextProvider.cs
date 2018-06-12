namespace DbIntegrationTestHelpers
{
    using System;

    /// <summary>
    /// Provides the test database context
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DbContextProvider<T>
    {
        /// <summary>
        /// The connection string for test database. <para/>
        /// Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=IntegrationTestHelpersDb;Integrated Security=SSPI;MultipleActiveResultSets=true;"
        /// </summary>
        protected virtual string ConnectionString { get; } =
            @"Data Source=(LocalDb)\MSSQLLocalDB;Initial Catalog=IntegrationTestHelpersDb;Integrated Security=SSPI;MultipleActiveResultSets=true;";

        /// <summary>
        /// Constructs an instance of the context using Activator
        /// </summary>
        /// <returns></returns>
        public virtual T GetContextInstance()
        {
            return DbContextService.CreateInstance<T>(this.ConnectionString);
        }
    }
}