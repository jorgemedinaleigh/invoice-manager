export default function InvoiceFilters({ filters, setFilters }) {
  return (
    <div className="flex gap-4 mb-4">
      <input
        type="text"
        placeholder="Número de factura"
        value={filters.number}
        onChange={(e) => setFilters({ ...filters, number: e.target.value })}
        className="border p-2"
      />
      <select
        value={filters.status}
        onChange={(e) => setFilters({ ...filters, status: e.target.value })}
        className="border p-2"
      >
        <option value="">Estado factura</option>
        <option value="Issued">Issued</option>
        <option value="Partial">Partial</option>
        <option value="Cancelled">Cancelled</option>
      </select>
      <select
        value={filters.paymentStatus}
        onChange={(e) => setFilters({ ...filters, paymentStatus: e.target.value })}
        className="border p-2"
      >
        <option value="">Estado pago</option>
        <option value="Pending">Pending</option>
        <option value="Overdue">Overdue</option>
        <option value="Paid">Paid</option>
      </select>
    </div>
  );
}
