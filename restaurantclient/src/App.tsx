import './App.sass'
import {BrowserRouter as Router, Routes, Route} from "react-router-dom";
import ReservationPlatform from "./pages/Client/ReservationPlatform/ReservationPlatform.tsx";
import OwnerSetup from "./pages/Owner/Setup/OwnerSetup.tsx";

function App() {
  return (
    <Router>
      <Routes>
        <Route path="/setup" element={<OwnerSetup />} />
        <Route path="/reservations" element={<ReservationPlatform />} />
      </Routes>
    </Router>
  )
}

export default App
