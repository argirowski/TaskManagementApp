import React from "react";
import { Navigate } from "react-router-dom";
import { useAppSelector } from "../../store/hooks";
import LoaderComponent from "../common/LoaderComponent";

interface ProtectedRouteProps {
  children: React.ReactNode;
  redirectTo?: string;
}

const ProtectedRoute: React.FC<ProtectedRouteProps> = ({
  children,
  redirectTo = "/login",
}) => {
  const { isAuthenticated, loading } = useAppSelector((state) => state.auth);

  // Show loading while checking authentication
  if (loading) {
    return <LoaderComponent message="Checking authentication..." />;
  }

  // Redirect to login if not authenticated
  if (!isAuthenticated) {
    return <Navigate to={redirectTo} replace />;
  }

  // Render protected content if authenticated
  return <>{children}</>;
};

export default ProtectedRoute;
