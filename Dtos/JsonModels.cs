using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace InvoiceManager.Dtos
{
    public class Root
    {
        [JsonPropertyName("invoices")]
        public List<InvoiceJson> Invoices { get; set; }
    }

    public class InvoiceJson
    {
        [JsonPropertyName("invoice_number")]
        public int InvoiceNumber { get; set; }

        [JsonPropertyName("invoice_date")]
        public string InvoiceDate { get; set; }

        [JsonPropertyName("invoice_status")]
        public string InvoiceStatus { get; set; }

        [JsonPropertyName("total_amount")]
        public decimal TotalAmount { get; set; }

        [JsonPropertyName("days_to_due")]
        public int DaysToDue { get; set; }

        [JsonPropertyName("payment_due_date")]
        public string PaymentDueDate { get; set; }

        [JsonPropertyName("payment_status")]
        public string PaymentStatus { get; set; }

        [JsonPropertyName("invoice_detail")]
        public List<InvoiceDetailJson> InvoiceDetail { get; set; }

        [JsonPropertyName("invoice_payment")]
        public InvoicePaymentJson InvoicePayment { get; set; }

        [JsonPropertyName("invoice_credit_note")]
        public List<CreditNoteJson> InvoiceCreditNote { get; set; }

        [JsonPropertyName("customer")]
        public CustomerJson Customer { get; set; }
    }

    public class InvoiceDetailJson
    {
        [JsonPropertyName("product_name")]
        public string ProductName { get; set; }

        [JsonPropertyName("unit_price")]
        public decimal UnitPrice { get; set; }

        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }

        [JsonPropertyName("subtotal")]
        public decimal Subtotal { get; set; }
    }

    public class InvoicePaymentJson
    {
        [JsonPropertyName("payment_method")]
        public string PaymentMethod { get; set; }

        [JsonPropertyName("payment_date")]
        public string PaymentDate { get; set; }
    }

    public class CreditNoteJson
    {
        [JsonPropertyName("credit_note_number")]
        public int CreditNoteNumber { get; set; }

        [JsonPropertyName("credit_note_date")]
        public string CreditNoteDate { get; set; }

        [JsonPropertyName("credit_note_amount")]
        public decimal CreditNoteAmount { get; set; }
    }

    public class CustomerJson
    {
        [JsonPropertyName("customer_run")]
        public string CustomerRun { get; set; }

        [JsonPropertyName("customer_name")]
        public string CustomerName { get; set; }

        [JsonPropertyName("customer_email")]
        public string CustomerEmail { get; set; }
    }
}
