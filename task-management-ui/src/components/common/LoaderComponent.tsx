import React from "react";
import { Container, Spinner } from "react-bootstrap";
import { LoaderComponentProps } from "../../types/types";

const LoaderComponent: React.FC<LoaderComponentProps> = ({
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

export default LoaderComponent;
