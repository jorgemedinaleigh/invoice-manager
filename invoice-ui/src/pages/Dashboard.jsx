import { useEffect, useState } from "react";
import api from "../services/api";

export default function Dashboard() {
  const [summary, setSummary] = useState([]);
  const [loading, setLoading] = useState(true);

  const fetchSummary = async () => {
    try {
      const response = await api.get("/reports/payment-summary");
      setSummary(response.data);
    } catch (error) {
      console.error("Error al obtener resumen", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchSummary();
  }, []);

  return (
    <div>
      <h1 className="text-2xl font-bold mb-4">Dashboard</h1>
      {loading ? (
        <p>Cargando...</p>
      ) : (
        <div className="grid grid-cols-3 gap-4">
          {summary.map((item, idx) => (
            <div key={idx} className="bg-white shadow p-4 rounded">
              <h2 className="text-lg font-semibold">{item.status}</h2>
              <p className="text-2xl font-bold">{item.count}</p>
              <p className="text-gray-500">{item.percentage.toFixed(2)}%</p>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
