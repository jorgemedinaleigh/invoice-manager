using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvoiceManager.Data;
using InvoiceManager.Models;

namespace InvoiceManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoicesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public InvoicesController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ GET /api/invoices?number=&status=&paymentStatus=
        [HttpGet]
        public async Task<IActionResult> GetInvoices([FromQuery] int? number, [FromQuery] string? status, [FromQuery] string? paymentStatus)
        {
            var query = _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Details)
                .Include(i => i.CreditNotes)
                .Include(i => i.Payment)
                .AsQueryable();

            if (number.HasValue)
                query = query.Where(i => i.InvoiceNumber == number);

            if (!string.IsNullOrEmpty(status))
                query = query.Where(i => i.InvoiceStatus.ToLower() == status.ToLower());

            if (!string.IsNullOrEmpty(paymentStatus))
                query = query.Where(i => i.PaymentStatus.ToLower() == paymentStatus.ToLower());

            var result = await query.ToListAsync();
            return Ok(result);
        }

        // ✅ GET /api/invoices/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetInvoice(int id)
        {
            var invoice = await _context.Invoices
                .Include(i => i.Customer)
                .Include(i => i.Details)
                .Include(i => i.CreditNotes)
                .Include(i => i.Payment)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
                return NotFound();

            return Ok(invoice);
        }

        // ✅ POST /api/invoices
        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] Invoice invoice)
        {
            if (_context.Invoices.Any(i => i.InvoiceNumber == invoice.InvoiceNumber))
                return BadRequest("Invoice number must be unique");

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetInvoice), new { id = invoice.Id }, invoice);
        }

        // ✅ PUT /api/invoices/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateInvoice(int id, [FromBody] Invoice invoice)
        {
            var existing = await _context.Invoices.FindAsync(id);
            if (existing == null)
                return NotFound();

            existing.PaymentStatus = invoice.PaymentStatus;
            existing.InvoiceStatus = invoice.InvoiceStatus;

            await _context.SaveChangesAsync();
            return Ok(existing);
        }

        // ✅ DELETE /api/invoices/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
                return NotFound();

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
