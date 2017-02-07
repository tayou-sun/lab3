namespace BooksStore.Books.Database.Model
{
    public class Purchase
    {
        public int PurchaseId { get; set; }

        public int BookId { get; set; }
        public virtual Book Book { get; set; }

        public string Username { get; set; }
    }
}