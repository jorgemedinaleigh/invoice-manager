using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvoiceManager.Data;

namespace InvoiceManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReportsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ReportsController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Facturas inconsistentes
        [HttpGet("inconsistent")]
        public async Task<IActionResult> GetInconsistentInvoices()
        {
            var result = await _context.Invoices
                .Where(i => !i.IsConsistent)
                .ToListAsync();

            return Ok(result);
        }

        // ✅ Facturas vencidas +30 días sin pago ni NC
        [HttpGet("overdue-30")]
        public async Task<IActionResult> GetOverdueInvoices()
        {
            var cutoffDate = DateTime.Now.AddDays(-30);
            var result = await _context.Invoices
                .Include(i => i.CreditNotes)
                .Where(i => i.PaymentStatus == "Pending"
                            && i.PaymentDueDate < cutoffDate
                            && i.CreditNotes.Count == 0)
                .ToListAsync();

            return Ok(result);
        }

        // ✅ Resumen por estado de pago
        [HttpGet("payment-summary")]
        public async Task<IActionResult> GetPaymentSummary()
        {
            var total = await _context.Invoices.CountAsync();
            var summary = await _context.Invoices
                .GroupBy(i => i.PaymentStatus)
                .Select(g => new
                {
                    Status = g.Key,
                    Count = g.Count(),
                    Percentage = (g.Count() * 100.0) / total
                }).ToListAsync();

            return Ok(summary);
        }
    }
}
