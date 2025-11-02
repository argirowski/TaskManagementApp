import React, { Fragment } from "react";
import { Navigate, Outlet } from "react-router-dom";
import { hasToken } from "../../utils/auth";

const ProtectedRoute: React.FC = () => {
  if (!hasToken()) {
    // Redirect to login page if not authenticated
    return <Navigate to="/login" replace />;
  }
  return (
    <Fragment>
      <Outlet />
    </Fragment>
  );
};

export default ProtectedRoute;
