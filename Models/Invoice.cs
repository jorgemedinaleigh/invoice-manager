using System;
using System.Collections.Generic;

namespace InvoiceManager.Models
{
    public class Invoice
    {
        public int Id { get; set; }
        public int InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime PaymentDueDate { get; set; }
        public string PaymentStatus { get; set; } // Pending, Paid, Overdue
        public string InvoiceStatus { get; set; } // Issued, Partial, Cancelled
        public bool IsConsistent { get; set; }

        public Customer Customer { get; set; }
        public ICollection<InvoiceDetail> Details { get; set; }
        public ICollection<CreditNote> CreditNotes { get; set; }
        public Payment Payment { get; set; }
    }
}
