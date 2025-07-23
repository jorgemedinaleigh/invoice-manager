using System.IO;
using System.Text.Json;
using System.Linq;
using System.Threading.Tasks;
using InvoiceManager.Data;
using InvoiceManager.Models;
using InvoiceManager.Dtos;
using Microsoft.EntityFrameworkCore;

namespace InvoiceManager.Services
{
    public class DataImporter
    {
        private readonly AppDbContext _context;

        public DataImporter(AppDbContext context)
        {
            _context = context;
        }

        public async Task ImportDataAsync(string jsonPath)
        {
            try
            {
                // Leer archivo
                var json = await File.ReadAllTextAsync(jsonPath);
                if (string.IsNullOrWhiteSpace(json))
                {
                    Console.WriteLine("❌ El archivo JSON está vacío.");
                    return;
                }
                Console.WriteLine($"Contenido del JSON (primeros 200 chars): {json.Substring(0, Math.Min(200, json.Length))}");


                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var data = JsonSerializer.Deserialize<Root>(json, options);

                if (data?.Invoices == null)
                {
                    Console.WriteLine("❌ No se pudo deserializar el JSON o no tiene datos.");
                    return;
                }

                Console.WriteLine($"✅ Se encontraron {data.Invoices.Count} facturas para importar.");

                foreach (var inv in data.Invoices)
                {
                    // Evitar duplicados
                    var existing = await _context.Invoices
                        .FirstOrDefaultAsync(x => x.InvoiceNumber == inv.InvoiceNumber);

                    if (existing != null)
                        continue;

                    // Validar consistencia
                    var subtotalSum = (inv.InvoiceDetail?.Sum(d => d.Subtotal) ?? 0);
                    bool isConsistent = subtotalSum == inv.TotalAmount;

                    // Crear cliente
                    var customer = new Customer
                    {
                        CustomerRun = inv.Customer?.CustomerRun ?? string.Empty,
                        CustomerName = inv.Customer?.CustomerName ?? "Desconocido",
                        CustomerEmail = inv.Customer?.CustomerEmail ?? string.Empty
                    };

                    // Crear factura
                    var invoice = new Invoice
                    {
                        InvoiceNumber = inv.InvoiceNumber,
                        InvoiceDate = ParseDate(inv.InvoiceDate),
                        TotalAmount = inv.TotalAmount,
                        PaymentDueDate = ParseDate(inv.PaymentDueDate),
                        PaymentStatus = CalculatePaymentStatus(inv), // Aplicamos lógica automática
                        InvoiceStatus = CalculateInvoiceStatus(inv),
                        IsConsistent = isConsistent,
                        Customer = customer
                    };

                    // Detalles de productos
                    if (inv.InvoiceDetail != null)
                    {
                        foreach (var d in inv.InvoiceDetail)
                        {
                            invoice.Details.Add(new InvoiceDetail
                            {
                                ProductName = d.ProductName ?? "Producto sin nombre",
                                UnitPrice = d.UnitPrice,
                                Quantity = d.Quantity,
                                Subtotal = d.Subtotal
                            });
                        }
                    }

                    // Notas de crédito
                    if (inv.InvoiceCreditNote != null)
                    {
                        foreach (var cn in inv.InvoiceCreditNote)
                        {
                            invoice.CreditNotes.Add(new CreditNote
                            {
                                CreditNoteNumber = cn.CreditNoteNumber,
                                CreditNoteDate = ParseDate(cn.CreditNoteDate),
                                CreditNoteAmount = cn.CreditNoteAmount
                            });
                        }
                    }

                    // Pago (si existe)
                    if (inv.InvoicePayment != null)
                    {
                        invoice.Payment = new Payment
                        {
                            PaymentMethod = inv.InvoicePayment.PaymentMethod,
                            PaymentDate = ParseNullableDate(inv.InvoicePayment.PaymentDate)
                        };
                    }

                    _context.Invoices.Add(invoice);
                }

                await _context.SaveChangesAsync();
                Console.WriteLine("✅ Datos importados correctamente.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error al importar datos: {ex.InnerException?.Message ?? ex.Message}");
            }
        }

        // ✅ Reglas automáticas de negocio
        private string CalculatePaymentStatus(InvoiceJson inv)
        {
            if (inv.InvoicePayment != null && !string.IsNullOrEmpty(inv.InvoicePayment.PaymentDate))
                return "Paid";

            var dueDate = ParseDate(inv.PaymentDueDate);
            if (dueDate < DateTime.Now)
                return "Overdue";

            return "Pending";
        }

        private string CalculateInvoiceStatus(InvoiceJson inv)
        {
            var creditSum = inv.InvoiceCreditNote?.Sum(cn => cn.CreditNoteAmount) ?? 0;

            if (creditSum == 0)
                return "Issued";

            if (creditSum >= inv.TotalAmount)
                return "Cancelled";

            return "Partial";
        }

        // ✅ Métodos auxiliares seguros
        private DateTime ParseDate(string? dateString)
        {
            return DateTime.TryParse(dateString, out var date)
                ? date
                : DateTime.Now;
        }

        private DateTime? ParseNullableDate(string? dateString)
        {
            return DateTime.TryParse(dateString, out var date)
                ? date
                : (DateTime?)null;
        }
    }
}
