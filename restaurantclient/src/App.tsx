import './App.sass'
import OwnerOverview from "./pages/Owner/Overview/OwnerOverview.tsx";
import {BrowserRouter as Router, Routes, Route} from "react-router-dom";
import ReservationPlatform from "./pages/ReservationPlatform/ReservationPlatform.tsx";
import OwnerSetup from "./pages/Owner/Setup/OwnerSetup.tsx";
import Tables from "./components/CreateTables/tables.tsx";
function App() {
  return (
    <Router>
      <Routes>

        <Route path="/" element={<OwnerOverview />} />
        <Route path="/setup" element={<OwnerSetup />} />
        <Route path="/reservations" element={<ReservationPlatform />} />
        <Route path="/tables" element={<Tables />} />

      </Routes>
    </Router>
  )
}

export default App
