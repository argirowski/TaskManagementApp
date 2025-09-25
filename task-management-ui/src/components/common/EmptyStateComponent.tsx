import React from "react";
import { Button } from "react-bootstrap";
import { EmptyStateComponentProps } from "../../types/types";

const EmptyStateComponent: React.FC<EmptyStateComponentProps> = ({
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

export default EmptyStateComponent;
