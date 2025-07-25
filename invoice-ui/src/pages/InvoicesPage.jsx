import { useEffect, useState } from "react";
import api from "../services/api";
import InvoiceFilters from "../components/InvoiceFilters";
import InvoiceList from "../components/InvoiceList";
import CreditNoteModal from "../components/CreditNoteModal";

export default function InvoicesPage() {
  const [invoices, setInvoices] = useState([]);
  const [loading, setLoading] = useState(true);
  const [filters, setFilters] = useState({ number: "", status: "", paymentStatus: "" });
  const [selectedInvoice, setSelectedInvoice] = useState(null);

  const fetchInvoices = async () => {
    setLoading(true);
    try {
      const params = {};
      if (filters.number) params.number = filters.number;
      if (filters.status) params.status = filters.status;
      if (filters.paymentStatus) params.paymentStatus = filters.paymentStatus;

      const response = await api.get("/invoices", { params });
      setInvoices(response.data);
    } catch (error) {
      console.error("Error fetching invoices", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchInvoices();
  }, [filters]);

  return (
    <div>
      <h1 className="text-2xl font-bold mb-4">Facturas</h1>
      <InvoiceFilters filters={filters} setFilters={setFilters} />

      {loading ? (
        <p>Cargando...</p>
      ) : (
        <InvoiceList invoices={invoices} onAddCreditNote={(inv) => setSelectedInvoice(inv)} />
      )}

      {selectedInvoice && (
        <CreditNoteModal
          invoice={selectedInvoice}
          onClose={() => setSelectedInvoice(null)}
          onAdded={fetchInvoices}
        />
      )}
    </div>
  );
}
