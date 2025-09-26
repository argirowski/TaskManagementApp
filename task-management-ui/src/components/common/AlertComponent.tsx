import React from "react";
import { Alert } from "react-bootstrap";
import { AlertComponentProps } from "../../types/types";

const AlertComponent: React.FC<AlertComponentProps> = ({
  show,
  variant,
  message,
  dismissible = true,
  onClose,
}) => {
  if (!show) return null;

  return (
    <Alert variant={variant} dismissible={dismissible} onClose={onClose}>
      {message}
    </Alert>
  );
};

export default AlertComponent;
