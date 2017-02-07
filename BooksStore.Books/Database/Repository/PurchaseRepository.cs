using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BooksStore.Books.Database.Model;

namespace BooksStore.Books.Database.Repository
{
    public class PurchaseRepository : IRepository<Purchase>
    {
        private readonly DatabaseContext _databaseContext;

        public PurchaseRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public ICollection<Purchase> GetAll()
        {
            return _databaseContext.Purchases.ToList();
        }

        public Purchase Get(int id)
        {
            return _databaseContext.Purchases.Find(id);
        }

        public void Create(Purchase item)
        {
            _databaseContext.Purchases.Add(item);
            _databaseContext.SaveChanges();
        }

        public void Update(Purchase item)
        {
            _databaseContext.Purchases.Attach(item);
            _databaseContext.Entry(item).State = EntityState.Modified;
            _databaseContext.SaveChanges();
        }

        public void Delete(Purchase item)
        {
            _databaseContext.Purchases.Remove(item);
            _databaseContext.SaveChanges();
        }
    }
}