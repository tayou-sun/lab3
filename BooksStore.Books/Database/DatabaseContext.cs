using System.Data.Entity;
using BooksStore.Books.Database.Model;

namespace BooksStore.Books.Database
{
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("DatabaseContext")
        {
        }

        public DbSet<Book> Books { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<Billing> Billigs { get; set; }
    }
}