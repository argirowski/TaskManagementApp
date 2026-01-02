import React from "react";
import { render, screen } from "@testing-library/react";
import userEvent from "@testing-library/user-event";
import EmptyStateComponent from "../../../components/common/EmptyStateComponent";
import { EmptyStateComponentProps } from "../../../types/types";

describe("EmptyStateComponent", () => {
  const defaultProps: EmptyStateComponentProps = {
    title: "No Items Found",
    message: "There are no items to display.",
  };

  beforeEach(() => {
    jest.clearAllMocks();
  });

  describe("Rendering", () => {
    it("should render the title", () => {
      render(<EmptyStateComponent {...defaultProps} />);

      expect(screen.getByText("No Items Found")).toBeInTheDocument();
      expect(screen.getByRole("heading", { level: 1 })).toHaveTextContent(
        "No Items Found"
      );
    });

    it("should render the message when provided", () => {
      render(<EmptyStateComponent {...defaultProps} />);

      expect(
        screen.getByText("There are no items to display.")
      ).toBeInTheDocument();
    });

    it("should not render message when message is not provided", () => {
      render(<EmptyStateComponent title="No Items" />);

      expect(
        screen.queryByText("There are no items to display.")
      ).not.toBeInTheDocument();
    });

    it("should render empty message when message is empty string", () => {
      render(<EmptyStateComponent {...defaultProps} message="" />);

      // Empty string message should not render the message paragraph
      expect(
        screen.queryByText("There are no items to display.")
      ).not.toBeInTheDocument();
    });
  });

  describe("Action Button", () => {
    it("should render action button when actionText and onAction are provided", () => {
      const onActionMock = jest.fn();
      render(
        <EmptyStateComponent
          {...defaultProps}
          actionText="Go Home"
          onAction={onActionMock}
        />
      );

      const button = screen.getByRole("button", { name: "Go Home" });
      expect(button).toBeInTheDocument();
    });

    it("should not render action button when actionText is not provided", () => {
      const onActionMock = jest.fn();
      render(<EmptyStateComponent {...defaultProps} onAction={onActionMock} />);

      expect(screen.queryByRole("button")).not.toBeInTheDocument();
    });

    it("should not render action button when onAction is not provided", () => {
      render(<EmptyStateComponent {...defaultProps} actionText="Go Home" />);

      expect(screen.queryByRole("button")).not.toBeInTheDocument();
    });

    it("should not render action button when both actionText and onAction are not provided", () => {
      render(<EmptyStateComponent {...defaultProps} />);

      expect(screen.queryByRole("button")).not.toBeInTheDocument();
    });

    it("should call onAction when action button is clicked", async () => {
      const onActionMock = jest.fn();
      render(
        <EmptyStateComponent
          {...defaultProps}
          actionText="Go Home"
          onAction={onActionMock}
        />
      );

      const button = screen.getByRole("button", { name: "Go Home" });
      await userEvent.click(button);

      expect(onActionMock).toHaveBeenCalledTimes(1);
    });

    it("should render button with correct text", () => {
      const onActionMock = jest.fn();
      const actionText = "Create New Item";
      render(
        <EmptyStateComponent
          {...defaultProps}
          actionText={actionText}
          onAction={onActionMock}
        />
      );

      expect(
        screen.getByRole("button", { name: actionText })
      ).toBeInTheDocument();
    });

    it("should render button with large size class", () => {
      const onActionMock = jest.fn();
      render(
        <EmptyStateComponent
          {...defaultProps}
          actionText="Go Home"
          onAction={onActionMock}
        />
      );

      const button = screen.getByRole("button", { name: "Go Home" });
      expect(button).toHaveClass("btn-lg");
    });

    it("should render button with custom class", () => {
      const onActionMock = jest.fn();
      render(
        <EmptyStateComponent
          {...defaultProps}
          actionText="Go Home"
          onAction={onActionMock}
        />
      );

      const button = screen.getByRole("button", { name: "Go Home" });
      expect(button).toHaveClass("btn-back-home");
    });
  });

  describe("Layout and Styling", () => {
    it("should render title in a heading element", () => {
      render(<EmptyStateComponent {...defaultProps} />);

      const heading = screen.getByRole("heading", { level: 1 });
      expect(heading).toBeInTheDocument();
      expect(heading).toHaveTextContent("No Items Found");
    });

    it("should render message in a paragraph when provided", () => {
      render(<EmptyStateComponent {...defaultProps} />);

      const message = screen.getByText("There are no items to display.");
      expect(message).toBeInTheDocument();
      expect(message.tagName).toBe("P");
    });

    it("should render all content elements", () => {
      const onActionMock = jest.fn();
      render(
        <EmptyStateComponent
          {...defaultProps}
          actionText="Go Home"
          onAction={onActionMock}
        />
      );

      // Verify all main elements are rendered
      expect(screen.getByRole("heading", { level: 1 })).toBeInTheDocument();
      expect(
        screen.getByText("There are no items to display.")
      ).toBeInTheDocument();
      expect(
        screen.getByRole("button", { name: "Go Home" })
      ).toBeInTheDocument();
    });
  });

  describe("Edge cases", () => {
    it("should handle empty title", () => {
      render(<EmptyStateComponent title="" />);

      const heading = screen.getByRole("heading", { level: 1 });
      expect(heading).toBeInTheDocument();
      expect(heading).toHaveTextContent("");
    });

    it("should handle long title", () => {
      const longTitle = "A".repeat(200);
      render(<EmptyStateComponent title={longTitle} />);

      expect(screen.getByText(longTitle)).toBeInTheDocument();
    });

    it("should handle long message", () => {
      const longMessage = "A".repeat(500);
      render(<EmptyStateComponent {...defaultProps} message={longMessage} />);

      expect(screen.getByText(longMessage)).toBeInTheDocument();
    });

    it("should handle special characters in title", () => {
      const specialTitle = "No Items <script>alert('XSS')</script> Found!";
      render(<EmptyStateComponent title={specialTitle} />);

      expect(screen.getByText(specialTitle)).toBeInTheDocument();
    });

    it("should handle special characters in message", () => {
      const specialMessage = "There are & no items <b>to display</b>!";
      render(
        <EmptyStateComponent {...defaultProps} message={specialMessage} />
      );

      expect(screen.getByText(specialMessage)).toBeInTheDocument();
    });

    it("should handle special characters in actionText", () => {
      const onActionMock = jest.fn();
      const specialActionText = "Go & Create <New> Item!";
      render(
        <EmptyStateComponent
          {...defaultProps}
          actionText={specialActionText}
          onAction={onActionMock}
        />
      );

      expect(
        screen.getByRole("button", { name: specialActionText })
      ).toBeInTheDocument();
    });

    it("should handle multiple rapid clicks on action button", async () => {
      const onActionMock = jest.fn();
      render(
        <EmptyStateComponent
          {...defaultProps}
          actionText="Go Home"
          onAction={onActionMock}
        />
      );

      const button = screen.getByRole("button", { name: "Go Home" });
      await userEvent.click(button);
      await userEvent.click(button);
      await userEvent.click(button);

      expect(onActionMock).toHaveBeenCalledTimes(3);
    });
  });
});
