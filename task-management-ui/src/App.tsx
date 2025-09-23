import React from "react";
import { BrowserRouter } from "react-router-dom";
import AppRoutes from "./routes/AppRoutes";
import "bootstrap/dist/css/bootstrap.min.css";
import "./index.css";

const App: React.FC = () => {
  return (
    <BrowserRouter>
      <div className="task-management-app">
        <AppRoutes />
      </div>
    </BrowserRouter>
  );
};

export default App;
