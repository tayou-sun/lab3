using BooksStore.Books.Database.Repository;

namespace BooksStore.Books.Database
{
    public class DatabaseController
    {
        private readonly DatabaseContext _databaseContext;

        public DatabaseController()
        {
            _databaseContext = new DatabaseContext();
            //Тут дергать, чтобы перестроить базу
            _databaseContext.Database.Initialize(false);

            Books = new BookRepository(_databaseContext);
            Purchases = new PurchaseRepository(_databaseContext);
            Billigs = new BillingRepository(_databaseContext);
        }

        public BookRepository Books { get; private set; }
        public PurchaseRepository Purchases { get; private set; }
        public BillingRepository Billigs { get; private set; }

        public void Save()
        {
            _databaseContext.SaveChanges();
        }
    }
}