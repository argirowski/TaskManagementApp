import React from "react";
import { Button } from "react-bootstrap";

interface EmptyStateProps {
  title: string;
  message?: string;
  actionText?: string;
  onAction?: () => void;
  icon?: React.ReactNode;
}

const EmptyState: React.FC<EmptyStateProps> = ({
  title,
  message,
  actionText,
  onAction,
  icon,
}) => {
  return (
    <div className="text-center py-5">
      {icon && <div className="mb-3">{icon}</div>}
      <h5>{title}</h5>
      {message && <p className="text-muted">{message}</p>}
      {actionText && onAction && (
        <Button variant="primary" onClick={onAction}>
          {actionText}
        </Button>
      )}
    </div>
  );
};

export default EmptyState;
