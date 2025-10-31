import React from "react";
import { Navigate } from "react-router-dom";
import { hasToken } from "../../utils/auth";

interface ProtectedRouteProps {
  children: React.ReactNode;
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({ children }) => {
  if (!hasToken()) {
    // You can replace this with a custom error page/component if you want
    return <Navigate to="/unauthorized" replace />;
  }
  return <>{children}</>;
};

export default ProtectedRoute;
