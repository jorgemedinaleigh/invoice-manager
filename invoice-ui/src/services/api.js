import axios from "axios";

// Configuración de Axios para conectarse al backend .NET
const api = axios.create({
  baseURL: "http://localhost:5000/api", // Ajusta el puerto si tu backend usa otro
  headers: {
    "Content-Type": "application/json"
  }
});

export default api;
