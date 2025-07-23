using System;

namespace InvoiceManager.Models
{
    public class CreditNote
    {
        public int Id { get; set; }
        public int InvoiceId { get; set; }
        public int CreditNoteNumber { get; set; }
        public DateTime CreditNoteDate { get; set; }
        public decimal CreditNoteAmount { get; set; }
    }
}
