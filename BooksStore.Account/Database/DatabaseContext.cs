using System.Data.Entity;
using BooksStore.Account.Database.Model;

namespace BooksStore.Account.Database
{
    /// <summary>
    ///     Контекст базы данных (подключение через EntityFramework)
    /// </summary>
    public class DatabaseContext : DbContext
    {
        public DatabaseContext() : base("DatabaseContext")
        {
        }

        public DbSet<User> Users { get; set; }
    }
}