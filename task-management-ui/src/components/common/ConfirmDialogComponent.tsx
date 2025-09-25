import React from "react";
import { Modal, Button } from "react-bootstrap";
import { ConfirmDialogComponentProps } from "../../types/types";

const ConfirmDialogComponent: React.FC<ConfirmDialogComponentProps> = ({
  show,
  title,
  message,
  confirmText = "Confirm",
  cancelText = "Cancel",
  onConfirm,
  onCancel,
  variant = "danger",
}) => {
  return (
    <Modal show={show} onHide={onCancel}>
      <Modal.Header closeButton>
        <Modal.Title>{title}</Modal.Title>
      </Modal.Header>
      <Modal.Body>
        {message}
        <br />
        <small className="text-muted">This action cannot be undone.</small>
      </Modal.Body>
      <Modal.Footer>
        <Button variant="secondary" onClick={onCancel}>
          {cancelText}
        </Button>
        <Button variant={variant} onClick={onConfirm}>
          {confirmText}
        </Button>
      </Modal.Footer>
    </Modal>
  );
};

export default ConfirmDialogComponent;
