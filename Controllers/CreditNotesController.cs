using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using InvoiceManager.Data;
using InvoiceManager.Models;

namespace InvoiceManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CreditNotesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CreditNotesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("{invoiceId}")]
        public async Task<IActionResult> AddCreditNote(int invoiceId, [FromBody] CreditNote creditNote)
        {
            var invoice = await _context.Invoices
                .Include(i => i.CreditNotes)
                .FirstOrDefaultAsync(i => i.Id == invoiceId);

            if (invoice == null)
                return NotFound();

            var totalCredit = invoice.CreditNotes.Sum(cn => cn.CreditNoteAmount);
            var saldoPendiente = invoice.TotalAmount - totalCredit;

            if (creditNote.CreditNoteAmount > saldoPendiente)
                return BadRequest("Credit note amount exceeds remaining balance.");

            creditNote.CreditNoteDate = DateTime.Now;
            invoice.CreditNotes.Add(creditNote);

            await _context.SaveChangesAsync();
            return Ok(creditNote);
        }
    }
}
