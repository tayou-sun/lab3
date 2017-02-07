using BooksStore.Account.Database.Repository;

namespace BooksStore.Account.Database
{
    /// <summary>
    ///     Контроллер базы данных
    /// </summary>
    public class DatabaseController
    {
        private readonly DatabaseContext _databaseContext;

        public DatabaseController()
        {
            _databaseContext = new DatabaseContext();
            //Тут дергать, чтобы перестроить базу
            _databaseContext.Database.Initialize(false);

            Users = new UserRepository(_databaseContext);
        }

        public UserRepository Users { get; private set; }

        public void Save()
        {
            _databaseContext.SaveChanges();
        }
    }
}