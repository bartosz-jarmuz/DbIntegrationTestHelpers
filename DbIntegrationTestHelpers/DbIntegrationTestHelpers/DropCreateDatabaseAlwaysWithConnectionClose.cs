namespace DbIntegrationTestHelpers
{
    using System.Data.Entity;

    internal sealed class DropCreateDatabaseAlwaysWithConnectionClose : DropCreateDatabaseAlways<DbContext>
    {
        public override void InitializeDatabase(DbContext context)
        {
            // Close all connections before drop
            if (context.Database.Exists())
            {
                context.Database.ExecuteSqlCommand(
                    TransactionalBehavior.DoNotEnsureTransaction,
                    $"ALTER DATABASE [{context.Database.Connection.Database}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE");
            }

            base.InitializeDatabase(context);
        }
    }
}