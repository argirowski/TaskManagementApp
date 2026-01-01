import React from "react";
import { BrowserRouter } from "react-router-dom";
import AppContent from "./AppContent";
import "bootstrap/dist/css/bootstrap.min.css";
import "./index.css";

const App: React.FC = () => {
  return (
    <BrowserRouter>
      <AppContent />
    </BrowserRouter>
  );
};

export default App;
