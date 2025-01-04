import './App.sass'
import OwnerOverview from "./pages/Owner/OwnerOverview.tsx";
import {BrowserRouter as Router, Routes, Route} from "react-router-dom";
import ReservationPlatform from "./pages/ReservationPlatform/ReservationPlatform.tsx";
function App() {
  return (
    <Router>
      <Routes>
        <Route path="/" element={<OwnerOverview />} />
        <Route path="/reservations" element={<ReservationPlatform />} />
      </Routes>
    </Router>
  )
}

export default App
