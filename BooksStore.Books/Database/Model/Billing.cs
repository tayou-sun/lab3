using System;

namespace BooksStore.Books.Database.Model
{
    public class Billing
    {
        public int BillingId { get; set; }
        public DateTime Date { get; set; }

        public int PurchaseId { get; set; }
        public virtual Purchase Purchase { get; set; }
    }
}