import React from "react";
import { Button } from "react-bootstrap";
import { EmptyStateComponentProps } from "../../types/types";

const EmptyStateComponent: React.FC<EmptyStateComponentProps> = ({
  title,
  message,
  actionText,
  onAction,
}) => {
  return (
    <div
      className="d-flex align-items-center justify-content-center"
      style={{ minHeight: "100vh" }}
    >
      <div className="text-center py-5">
        <h1>{title}</h1>
        {message && (
          <p className="text-muted" style={{ fontSize: "1.25rem" }}>
            {message}
          </p>
        )}
        {actionText && onAction && (
          <Button onClick={onAction} size="lg" className="btn-back-home">
            {actionText}
          </Button>
        )}
      </div>
    </div>
  );
};

export default EmptyStateComponent;
