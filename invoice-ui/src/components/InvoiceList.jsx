export default function InvoiceList({ invoices, onAddCreditNote }) {
  return (
    <table className="table-auto w-full border">
      <thead>
        <tr className="bg-gray-200">
          <th className="p-2">Número</th>
          <th>Cliente</th>
          <th>Total</th>
          <th>Estado</th>
          <th>Pago</th>
          <th>Acción</th>
        </tr>
      </thead>
      <tbody>
        {invoices.map(inv => (
          <tr key={inv.id} className="border-b">
            <td className="p-2">{inv.invoiceNumber}</td>
            <td>{inv.customerName}</td>
            <td>${inv.totalAmount}</td>
            <td>{inv.invoiceStatus}</td>
            <td>{inv.paymentStatus}</td>
            <td>
              <button
                className="bg-blue-500 text-white px-2 py-1 rounded"
                onClick={() => onAddCreditNote(inv)}
              >
                Agregar NC
              </button>
            </td>
          </tr>
        ))}
      </tbody>
    </table>
  );
}
