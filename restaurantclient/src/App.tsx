import './App.sass'
import {BrowserRouter as Router, Routes, Route} from "react-router-dom";
import ReservationPlatform from "./pages/Client/ReservationPlatform/ReservationPlatform.tsx";
import OwnerSetup from "./pages/Owner/Setup/OwnerSetup.tsx";
import Tables from "./components/FormObjects/TableCreator/tablesForm.tsx";
function App() {
  return (
    <Router>
      <Routes>

        <Route path="/setup" element={<OwnerSetup />} />
        <Route path="/reservations" element={<ReservationPlatform />} />
        <Route path="/tables" element={<Tables />} />

      </Routes>
    </Router>
  )
}

export default App
