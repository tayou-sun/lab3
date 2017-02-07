using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BooksStore.Account.Database.Model;

namespace BooksStore.Account.Database.Repository
{
    /// <summary>
    ///     Реализация репозитория для модели User
    /// </summary>
    public class UserRepository : IRepository<User>
    {
        private readonly DatabaseContext _databaseContext;

        public UserRepository(DatabaseContext databaseContext)
        {
            _databaseContext = databaseContext;
        }

        public ICollection<User> GetAll()
        {
            return _databaseContext.Users.ToList();
        }

        public User Get(int id)
        {
            return _databaseContext.Users.Find(id);
        }

        public void Create(User item)
        {
            _databaseContext.Users.Add(item);
            _databaseContext.SaveChanges();
        }

        public void Update(User item)
        {
            _databaseContext.Users.Attach(item);
            _databaseContext.Entry(item).State = EntityState.Modified;
            _databaseContext.SaveChanges();
        }

        public void Delete(User item)
        {
            _databaseContext.Users.Remove(item);
            _databaseContext.SaveChanges();
        }
    }
}