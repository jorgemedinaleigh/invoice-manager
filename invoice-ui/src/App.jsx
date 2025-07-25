import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";
import InvoicesPage from "./pages/InvoicesPage";
import ReportsPage from "./pages/ReportsPage";
import Dashboard from "./pages/Dashboard";

export default function App() {
  return (
    <div className="container">
      <Router>
        <nav className="navbar navbar-expand-lg bg-body-tertiary">
          <div className="container-fluid">
            <div class="collapse navbar-collapse">
              <ul className="navbar-nav">
                <li className="nav-item"><Link className="nav-link" to="/">Dashboard</Link></li>
                <li className="nav-item"><Link className="nav-link" to="/invoices">Facturas</Link></li>
                <li className="nav-item"><Link className="nav-link" to="/reports">Reportes</Link></li>
              </ul>
            </div>
          </div>
        </nav>
        <div className="p-6">
          <Routes>
            <Route path="/" element={<Dashboard />} />
            <Route path="/invoices" element={<InvoicesPage />} />
            <Route path="/reports" element={<ReportsPage />} />
          </Routes>
        </div>
      </Router>
    </div>
  );
}
