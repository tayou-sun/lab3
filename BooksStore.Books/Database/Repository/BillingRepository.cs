using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BooksStore.Books.Database.Model;

namespace BooksStore.Books.Database.Repository
{
    public class BillingRepository : IRepository<Billing>
    {
        private readonly DatabaseContext _databaseContext;

        public BillingRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public ICollection<Billing> GetAll()
        {
            return _databaseContext.Billigs.ToList();
        }

        public Billing Get(int id)
        {
            return _databaseContext.Billigs.Find(id);
        }

        public void Create(Billing item)
        {
            _databaseContext.Billigs.Add(item);
            _databaseContext.SaveChanges();
        }

        public void Update(Billing item)
        {
            _databaseContext.Billigs.Attach(item);
            _databaseContext.Entry(item).State = EntityState.Modified;
            _databaseContext.SaveChanges();
        }

        public void Delete(Billing item)
        {
            _databaseContext.Billigs.Remove(item);
            _databaseContext.SaveChanges();
        }
    }
}