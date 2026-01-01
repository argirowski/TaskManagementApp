import React, { useState, useEffect } from "react";
import { useLocation } from "react-router-dom";
import NavigationComponent from "./components/layout/NavigationComponent";
import { hasToken } from "./utils/auth";
import AppRoutes from "./routes/AppRoutes";
import "bootstrap/dist/css/bootstrap.min.css";
import "./index.css";

const AppContent: React.FC = () => {
  const [isAuthenticated, setIsAuthenticated] = useState(hasToken());
  const location = useLocation();

  // Check authentication status on mount and when route changes
  useEffect(() => {
    setIsAuthenticated(hasToken());
  }, [location]);

  return (
    <>
      {isAuthenticated && <NavigationComponent />}
      <div className="task-management-app">
        <AppRoutes />
      </div>
    </>
  );
};

export default AppContent;
