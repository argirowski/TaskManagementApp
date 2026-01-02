import React from "react";
import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import AlertComponent from "../../../components/common/AlertComponent";
import { AlertComponentProps } from "../../../types/types";

describe("AlertComponent", () => {
  const defaultProps: AlertComponentProps = {
    show: true,
    variant: "success",
    message: "Test message",
  };

  beforeEach(() => {
    jest.clearAllMocks();
  });

  describe("Rendering", () => {
    it("should render when show is true", () => {
      render(<AlertComponent {...defaultProps} />);

      expect(screen.getByText("Test message")).toBeInTheDocument();
      expect(screen.getByRole("alert")).toBeInTheDocument();
    });

    it("should not render when show is false", () => {
      render(<AlertComponent {...defaultProps} show={false} />);

      expect(screen.queryByText("Test message")).not.toBeInTheDocument();
      expect(screen.queryByRole("alert")).not.toBeInTheDocument();
    });

    it("should render the message correctly", () => {
      const message = "Custom alert message";
      render(<AlertComponent {...defaultProps} message={message} />);

      expect(screen.getByText(message)).toBeInTheDocument();
    });
  });

  describe("Variants", () => {
    it("should render with success variant", () => {
      render(<AlertComponent {...defaultProps} variant="success" />);

      const alert = screen.getByRole("alert");
      expect(alert).toBeInTheDocument();
      expect(alert).toHaveClass("alert-success");
    });

    it("should render with danger variant", () => {
      render(<AlertComponent {...defaultProps} variant="danger" />);

      const alert = screen.getByRole("alert");
      expect(alert).toBeInTheDocument();
      expect(alert).toHaveClass("alert-danger");
    });

    it("should render with warning variant", () => {
      render(<AlertComponent {...defaultProps} variant="warning" />);

      const alert = screen.getByRole("alert");
      expect(alert).toBeInTheDocument();
      expect(alert).toHaveClass("alert-warning");
    });

    it("should render with info variant", () => {
      render(<AlertComponent {...defaultProps} variant="info" />);

      const alert = screen.getByRole("alert");
      expect(alert).toBeInTheDocument();
      expect(alert).toHaveClass("alert-info");
    });

    it("should render with primary variant", () => {
      render(<AlertComponent {...defaultProps} variant="primary" />);

      const alert = screen.getByRole("alert");
      expect(alert).toBeInTheDocument();
      expect(alert).toHaveClass("alert-primary");
    });

    it("should render with secondary variant", () => {
      render(<AlertComponent {...defaultProps} variant="secondary" />);

      const alert = screen.getByRole("alert");
      expect(alert).toBeInTheDocument();
      expect(alert).toHaveClass("alert-secondary");
    });

    it("should render with light variant", () => {
      render(<AlertComponent {...defaultProps} variant="light" />);

      const alert = screen.getByRole("alert");
      expect(alert).toBeInTheDocument();
      expect(alert).toHaveClass("alert-light");
    });

    it("should render with dark variant", () => {
      render(<AlertComponent {...defaultProps} variant="dark" />);

      const alert = screen.getByRole("alert");
      expect(alert).toBeInTheDocument();
      expect(alert).toHaveClass("alert-dark");
    });
  });

  describe("Dismissible", () => {
    it("should be dismissible by default", () => {
      render(<AlertComponent {...defaultProps} />);

      const alert = screen.getByRole("alert");
      expect(alert).toBeInTheDocument();
      // Check for close button when dismissible
      const closeButton = screen.queryByRole("button", { name: /close/i });
      expect(closeButton).toBeInTheDocument();
    });

    it("should be dismissible when dismissible is true", () => {
      render(<AlertComponent {...defaultProps} dismissible={true} />);

      const alert = screen.getByRole("alert");
      expect(alert).toBeInTheDocument();
      const closeButton = screen.queryByRole("button", { name: /close/i });
      expect(closeButton).toBeInTheDocument();
    });

    it("should not be dismissible when dismissible is false", () => {
      render(<AlertComponent {...defaultProps} dismissible={false} />);

      const alert = screen.getByRole("alert");
      expect(alert).toBeInTheDocument();
      const closeButton = screen.queryByRole("button", { name: /close/i });
      expect(closeButton).not.toBeInTheDocument();
    });
  });

  describe("onClose callback", () => {
    it("should call onClose when close button is clicked", async () => {
      const onCloseMock = jest.fn();

      render(
        <AlertComponent
          {...defaultProps}
          dismissible={true}
          onClose={onCloseMock}
        />
      );

      const closeButton = screen.getByRole("button", { name: /close/i });
      await userEvent.click(closeButton);

      expect(onCloseMock).toHaveBeenCalledTimes(1);
    });

    it("should show close button when dismissible is true and onClose is provided", () => {
      const onCloseMock = jest.fn();
      render(
        <AlertComponent
          {...defaultProps}
          dismissible={true}
          onClose={onCloseMock}
        />
      );

      const closeButton = screen.queryByRole("button", { name: /close/i });
      expect(closeButton).toBeInTheDocument();
    });

    it("should not call onClose when dismissible is false", () => {
      const onCloseMock = jest.fn();
      render(
        <AlertComponent
          {...defaultProps}
          dismissible={false}
          onClose={onCloseMock}
        />
      );

      const closeButton = screen.queryByRole("button", { name: /close/i });
      expect(closeButton).not.toBeInTheDocument();
      expect(onCloseMock).not.toHaveBeenCalled();
    });
  });

  describe("Edge cases", () => {
    it("should handle empty message", () => {
      render(<AlertComponent {...defaultProps} message="" />);

      const alert = screen.getByRole("alert");
      expect(alert).toBeInTheDocument();
      expect(alert).toHaveTextContent("");
    });

    it("should handle long messages", () => {
      const longMessage = "A".repeat(1000);
      render(<AlertComponent {...defaultProps} message={longMessage} />);

      expect(screen.getByText(longMessage)).toBeInTheDocument();
    });

    it("should handle message with special characters", () => {
      const specialMessage =
        "Alert with <script>alert('XSS')</script> & special chars!";
      render(<AlertComponent {...defaultProps} message={specialMessage} />);

      expect(screen.getByText(specialMessage)).toBeInTheDocument();
    });
  });
});
