namespace EntityFrameworkSample
{
    using System.Data.Entity;

    public class BookStoreDbContext : DbContext
    {
        public BookStoreDbContext(string nameOrConnectionString) : base(nameOrConnectionString){}

        public DbSet<Book> Books { get; set; }
        public DbSet<Author> Authors { get; set; }

        
    }
}