import React from "react";
import { Alert as BootstrapAlert } from "react-bootstrap";
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
    <BootstrapAlert
      variant={variant}
      dismissible={dismissible}
      onClose={onClose}
    >
      {message}
    </BootstrapAlert>
  );
};

export default AlertComponent;
