import { useState } from "react";
import api from "../services/api";

export default function CreditNoteModal({ invoice, onClose, onAdded }) {
  const [amount, setAmount] = useState("");

  const addCreditNote = async () => {
    try {
      await api.post(`/creditnotes/${invoice.id}`, {
        creditNoteNumber: Date.now(), // Número ficticio
        creditNoteAmount: parseFloat(amount)
      });
      onAdded();
      onClose();
    } catch (error) {
      alert("Error al agregar nota de crédito");
    }
  };

  return (
    <div className="fixed inset-0 bg-black bg-opacity-30 flex justify-center items-center">
      <div className="bg-white p-4 rounded w-96">
        <h2 className="text-lg font-bold mb-2">Agregar Nota de Crédito</h2>
        <p>Factura #{invoice.invoiceNumber}</p>
        <input
          type="number"
          placeholder="Monto"
          value={amount}
          onChange={(e) => setAmount(e.target.value)}
          className="border w-full p-2 mb-4"
        />
        <div className="flex justify-end gap-2">
          <button onClick={onClose} className="bg-gray-300 px-3 py-1 rounded">Cancelar</button>
          <button onClick={addCreditNote} className="bg-green-500 text-white px-3 py-1 rounded">
            Guardar
          </button>
        </div>
      </div>
    </div>
  );
}
