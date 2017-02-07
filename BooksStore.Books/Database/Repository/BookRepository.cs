using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BooksStore.Books.Database.Model;

namespace BooksStore.Books.Database.Repository
{
    public class BookRepository : IRepository<Book>
    {
        private readonly DatabaseContext _databaseContext;

        public BookRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public ICollection<Book> GetAll()
        {
            return _databaseContext.Books.ToList();
        }

        public Book Get(int id)
        {
            return _databaseContext.Books.Find(id);
        }

        public void Create(Book item)
        {
            _databaseContext.Books.Add(item);
            _databaseContext.SaveChanges();
        }

        public void Update(Book item)
        {
            _databaseContext.Books.Attach(item);
            _databaseContext.Entry(item).State = EntityState.Modified;
            _databaseContext.SaveChanges();
        }

        public void Delete(Book item)
        {
            _databaseContext.Books.Remove(item);
            _databaseContext.SaveChanges();
        }
    }
}