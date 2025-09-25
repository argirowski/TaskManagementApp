import React from "react";
import { Alert as BootstrapAlert } from "react-bootstrap";

interface AlertProps {
  show: boolean;
  variant:
    | "success"
    | "danger"
    | "warning"
    | "info"
    | "primary"
    | "secondary"
    | "light"
    | "dark";
  message: string;
  dismissible?: boolean;
  onClose?: () => void;
}

const Alert: React.FC<AlertProps> = ({
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

export default Alert;
