import { useEffect, useState } from "react";
import api from "../services/api";
import ReportSummary from "../components/ReportSummary";

export default function ReportsPage() {
  const [summary, setSummary] = useState([]);
  const [loading, setLoading] = useState(true);

  const fetchSummary = async () => {
    try {
      const response = await api.get("/reports/payment-summary");
      setSummary(response.data);
    } catch (error) {
      console.error("Error fetching summary", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchSummary();
  }, []);

  return (
    <div>
      <h1 className="text-2xl font-bold mb-4">Reportes</h1>
      {loading ? (
        <p>Cargando...</p>
      ) : summary.length > 0 ? (
        <ReportSummary data={summary} />
      ) : (
        <p>No hay datos disponibles</p>
      )}
    </div>
  );
}
