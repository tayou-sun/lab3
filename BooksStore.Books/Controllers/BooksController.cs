using System;
using System.Linq;
using System.Web.Mvc;
using BooksStore.Books.Database;
using BooksStore.Books.Database.Model;

namespace BooksStore.Books.Controllers
{
    public class BooksController : Controller
    {
        private const int BooksPerPage = 5;

        private readonly DatabaseController _databaseController;

        public BooksController()
        {
            _databaseController = new DatabaseController();
        }
         
        public string CreateBook(string name, string author, string price)
        {
            var book = new Book {Name = name, Author = author, Price = int.Parse(price)};
            _databaseController.Books.Create(book);

            return "OK";
        }

        #region Работа со списком книг

        public string GetBooksInfo()
        {
            var books = _databaseController.Books.GetAll().ToList();
            var result = "{ \"books_count\":" + books.Count + ", \"pages_count\":" + (books.Count/BooksPerPage + 1) + " }";

            return result;
        }

        public string GetBooksFromPage(int pageNumber)
        {
            var books = _databaseController.Books.GetAll().ToList();

            if (books.Count == 0)
                return "{ \"error\":\"no_books\", \"error_description\":\"No books were found!\"";

            if (pageNumber <= 0 || pageNumber > (books.Count / BooksPerPage + 1))
                return "{ \"error\":\"wrong_page\", \"error_description\":\"Unable to get books from this page!\"";

            var result = "[ ";

            for (var i = (pageNumber - 1)*BooksPerPage; i < (pageNumber - 1)*BooksPerPage + BooksPerPage; ++i)
            {
                if (i >= books.Count)
                    break;

                result += "{ \"id\":" + books[i].BookId + ", \"name\":\"" + books[i].Name + "\", \"author\":\"" + books[i].Author + "\", \"price\":" + books[i].Price + " }";

                if (i < books.Count - 1 && i != (pageNumber - 1)*BooksPerPage + BooksPerPage - 1)
                    result += ", ";
            }

            return result + " ]";
        }

        #endregion

        #region Работа с покупками

        [HttpGet]
        public string ProcessPurchase(int id, string username)
        {
            var book = _databaseController.Books.Get(id);

            if (book == null)
                return "{ \"error\":\"wrong_book\", \"error_description\":\"Wrong book id!\"";

            var purchase = new Purchase { Book = book, Username = username };
            _databaseController.Purchases.Create(purchase);

            return "{ \"purchase\":" + _databaseController.Purchases.GetAll().Last().PurchaseId + ", \"book\":" + id + ", \"username\":\"" + username + "\" }";
        }

        #endregion

        #region Работа с биллингом

        [HttpGet]
        public string ProcessBilling(int id)
        {
            var purchase = _databaseController.Purchases.Get(id);

            if (purchase == null)
                return "{ \"error\":\"wrong_purchase\", \"error_description\":\"Wrong purchase id!\"";

            var billing = new Billing { Date = DateTime.Now, Purchase = purchase };
            _databaseController.Billigs.Create(billing);

            return "{ \"billing\":" + _databaseController.Billigs.GetAll().Last().BillingId + ", \"date\":" + billing.Date + ", \"purchase\":" + purchase.PurchaseId + " }";
        }

        #endregion
    }
}