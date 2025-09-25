import React from "react";
import { Container, Spinner } from "react-bootstrap";

interface LoaderProps {
  message?: string;
  variant?:
    | "primary"
    | "secondary"
    | "success"
    | "danger"
    | "warning"
    | "info"
    | "light"
    | "dark";
  fullScreen?: boolean;
}

const Loader: React.FC<LoaderProps> = ({
  message = "Loading...",
  variant = "primary",
  fullScreen = true,
}) => {
  const containerStyle = fullScreen
    ? { minHeight: "100vh" }
    : { minHeight: "200px" };

  return (
    <Container
      className="d-flex justify-content-center align-items-center"
      style={containerStyle}
    >
      <div className="text-center">
        <Spinner animation="border" variant={variant} />
        <p className="mt-3">{message}</p>
      </div>
    </Container>
  );
};

export default Loader;
