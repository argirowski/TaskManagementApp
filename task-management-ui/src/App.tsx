import React from "react";
import { BrowserRouter } from "react-router-dom";
import Navigation from "./components/layout/Navigation";
import AppRoutes from "./routes/AppRoutes";
import "bootstrap/dist/css/bootstrap.min.css";
import "./index.css";

const App: React.FC = () => {
  return (
    <BrowserRouter>
      <Navigation />
      <div className="task-management-app">
        <AppRoutes />
      </div>
    </BrowserRouter>
  );
};

export default App;
