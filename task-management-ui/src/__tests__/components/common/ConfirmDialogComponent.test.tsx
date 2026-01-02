import React from "react";
import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import ConfirmDialogComponent from "../../../components/common/ConfirmDialogComponent";
import { ConfirmDialogComponentProps } from "../../../types/types";

describe("ConfirmDialogComponent", () => {
  const defaultProps: ConfirmDialogComponentProps = {
    show: true,
    title: "Confirm Action",
    message: "Are you sure you want to proceed?",
    onConfirm: jest.fn(),
    onCancel: jest.fn(),
  };

  beforeEach(() => {
    jest.clearAllMocks();
  });

  describe("Rendering", () => {
    it("should render when show is true", () => {
      render(<ConfirmDialogComponent {...defaultProps} />);

      expect(screen.getByText("Confirm Action")).toBeInTheDocument();
      expect(
        screen.getByText("Are you sure you want to proceed?")
      ).toBeInTheDocument();
    });

    it("should not render when show is false", () => {
      render(<ConfirmDialogComponent {...defaultProps} show={false} />);

      expect(screen.queryByText("Confirm Action")).not.toBeInTheDocument();
      expect(
        screen.queryByText("Are you sure you want to proceed?")
      ).not.toBeInTheDocument();
    });

    it("should render the title correctly", () => {
      const title = "Delete Item";
      render(<ConfirmDialogComponent {...defaultProps} title={title} />);

      expect(screen.getByText(title)).toBeInTheDocument();
    });

    it("should render the message correctly", () => {
      const message = "This action cannot be undone.";
      render(<ConfirmDialogComponent {...defaultProps} message={message} />);

      expect(screen.getByText(message)).toBeInTheDocument();
    });
  });

  describe("Button Labels", () => {
    it("should use default confirm text when not provided", () => {
      render(<ConfirmDialogComponent {...defaultProps} />);

      expect(
        screen.getByRole("button", { name: "Confirm" })
      ).toBeInTheDocument();
    });

    it("should use default cancel text when not provided", () => {
      render(<ConfirmDialogComponent {...defaultProps} />);

      expect(
        screen.getByRole("button", { name: "Cancel" })
      ).toBeInTheDocument();
    });

    it("should use custom confirm text when provided", () => {
      const confirmText = "Yes, Delete";
      render(
        <ConfirmDialogComponent {...defaultProps} confirmText={confirmText} />
      );

      expect(
        screen.getByRole("button", { name: confirmText })
      ).toBeInTheDocument();
    });

    it("should use custom cancel text when provided", () => {
      const cancelText = "No, Keep It";
      render(
        <ConfirmDialogComponent {...defaultProps} cancelText={cancelText} />
      );

      expect(
        screen.getByRole("button", { name: cancelText })
      ).toBeInTheDocument();
    });
  });

  describe("Variants", () => {
    it("should use danger variant by default", () => {
      render(<ConfirmDialogComponent {...defaultProps} />);

      const confirmButton = screen.getByRole("button", { name: "Confirm" });
      expect(confirmButton).toHaveClass("btn-danger");
    });

    it("should use danger variant when specified", () => {
      render(<ConfirmDialogComponent {...defaultProps} variant="danger" />);

      const confirmButton = screen.getByRole("button", { name: "Confirm" });
      expect(confirmButton).toHaveClass("btn-danger");
    });

    it("should use primary variant when specified", () => {
      render(<ConfirmDialogComponent {...defaultProps} variant="primary" />);

      const confirmButton = screen.getByRole("button", { name: "Confirm" });
      expect(confirmButton).toHaveClass("btn-primary");
    });

    it("should use warning variant when specified", () => {
      render(<ConfirmDialogComponent {...defaultProps} variant="warning" />);

      const confirmButton = screen.getByRole("button", { name: "Confirm" });
      expect(confirmButton).toHaveClass("btn-warning");
    });
  });

  describe("Button Actions", () => {
    it("should call onConfirm when confirm button is clicked", async () => {
      const onConfirmMock = jest.fn();
      render(
        <ConfirmDialogComponent {...defaultProps} onConfirm={onConfirmMock} />
      );

      const confirmButton = screen.getByRole("button", { name: "Confirm" });
      await userEvent.click(confirmButton);

      expect(onConfirmMock).toHaveBeenCalledTimes(1);
    });

    it("should call onCancel when cancel button is clicked", async () => {
      const onCancelMock = jest.fn();
      render(
        <ConfirmDialogComponent {...defaultProps} onCancel={onCancelMock} />
      );

      const cancelButton = screen.getByRole("button", { name: "Cancel" });
      await userEvent.click(cancelButton);

      expect(onCancelMock).toHaveBeenCalledTimes(1);
    });

    it("should call onCancel when close button is clicked", async () => {
      const onCancelMock = jest.fn();
      render(
        <ConfirmDialogComponent {...defaultProps} onCancel={onCancelMock} />
      );

      // Find the close button (X button in modal header)
      const closeButton = screen.getByRole("button", { name: /close/i });
      await userEvent.click(closeButton);

      expect(onCancelMock).toHaveBeenCalledTimes(1);
    });

    it("should call onCancel when Escape key is pressed", async () => {
      const onCancelMock = jest.fn();
      render(
        <ConfirmDialogComponent {...defaultProps} onCancel={onCancelMock} />
      );

      // Modal backdrop click and Escape key both trigger onHide (onCancel)
      // Testing Escape key as it's more reliable in tests
      await userEvent.keyboard("{Escape}");

      expect(onCancelMock).toHaveBeenCalledTimes(1);
    });
  });

  describe("Modal Structure", () => {
    it("should render modal header with title", () => {
      render(<ConfirmDialogComponent {...defaultProps} />);

      const title = screen.getByText("Confirm Action");
      expect(title).toBeInTheDocument();
      // Modal.Title can render as different elements depending on react-bootstrap version
      // Just verify the title text is present
      expect(title).toHaveTextContent("Confirm Action");
    });

    it("should render modal body with message", () => {
      render(<ConfirmDialogComponent {...defaultProps} />);

      const message = screen.getByText("Are you sure you want to proceed?");
      expect(message).toBeInTheDocument();
    });

    it("should render modal footer with buttons", () => {
      render(<ConfirmDialogComponent {...defaultProps} />);

      const cancelButton = screen.getByRole("button", { name: "Cancel" });
      const confirmButton = screen.getByRole("button", { name: "Confirm" });

      expect(cancelButton).toBeInTheDocument();
      expect(confirmButton).toBeInTheDocument();
    });

    it("should have close button in header", () => {
      render(<ConfirmDialogComponent {...defaultProps} />);

      const closeButton = screen.getByRole("button", { name: /close/i });
      expect(closeButton).toBeInTheDocument();
    });
  });

  describe("Edge cases", () => {
    it("should handle empty title", () => {
      render(<ConfirmDialogComponent {...defaultProps} title="" />);

      const modal = screen.getByRole("dialog");
      expect(modal).toBeInTheDocument();
    });

    it("should handle empty message", () => {
      render(<ConfirmDialogComponent {...defaultProps} message="" />);

      const modal = screen.getByRole("dialog");
      expect(modal).toBeInTheDocument();
    });

    it("should handle long messages", () => {
      const longMessage = "A".repeat(1000);
      render(
        <ConfirmDialogComponent {...defaultProps} message={longMessage} />
      );

      expect(screen.getByText(longMessage)).toBeInTheDocument();
    });

    it("should handle special characters in title and message", () => {
      const specialTitle = "Delete <script>alert('XSS')</script>?";
      const specialMessage = "This will delete & remove the item!";

      render(
        <ConfirmDialogComponent
          {...defaultProps}
          title={specialTitle}
          message={specialMessage}
        />
      );

      expect(screen.getByText(specialTitle)).toBeInTheDocument();
      expect(screen.getByText(specialMessage)).toBeInTheDocument();
    });

    it("should handle multiple rapid clicks on confirm button", async () => {
      const onConfirmMock = jest.fn();
      render(
        <ConfirmDialogComponent {...defaultProps} onConfirm={onConfirmMock} />
      );

      const confirmButton = screen.getByRole("button", { name: "Confirm" });
      await userEvent.click(confirmButton);
      await userEvent.click(confirmButton);
      await userEvent.click(confirmButton);

      expect(onConfirmMock).toHaveBeenCalledTimes(3);
    });

    it("should handle multiple rapid clicks on cancel button", async () => {
      const onCancelMock = jest.fn();
      render(
        <ConfirmDialogComponent {...defaultProps} onCancel={onCancelMock} />
      );

      const cancelButton = screen.getByRole("button", { name: "Cancel" });
      await userEvent.click(cancelButton);
      await userEvent.click(cancelButton);

      expect(onCancelMock).toHaveBeenCalledTimes(2);
    });
  });
});
