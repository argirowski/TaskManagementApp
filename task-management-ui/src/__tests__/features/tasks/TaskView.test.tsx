// Mock the task service
jest.mock("../../../services/taskService", () => ({
  fetchTask: jest.fn(),
}));

// Mock LoaderComponent
jest.mock("../../../components/common/LoaderComponent", () => {
  return function MockLoaderComponent({ message }: { message: string }) {
    return <div data-testid="loader">{message}</div>;
  };
});

// Mock EmptyStateComponent
jest.mock("../../../components/common/EmptyStateComponent", () => {
  return function MockEmptyStateComponent({
    title,
    message,
    actionText,
    onAction,
  }: {
    title: string;
    message: string;
    actionText: string;
    onAction: () => void;
  }) {
    return (
      <div data-testid="empty-state">
        <h2>{title}</h2>
        <p>{message}</p>
        <button onClick={onAction}>{actionText}</button>
      </div>
    );
  };
});

// Mock react-router-dom - Jest will automatically use __mocks__/react-router-dom.tsx
jest.mock("react-router-dom");

// eslint-disable-next-line import/first
import React from "react";
// eslint-disable-next-line import/first
import { render, screen, waitFor } from "@testing-library/react";
// eslint-disable-next-line import/first
import userEvent from "@testing-library/user-event";
// eslint-disable-next-line import/first
import { MemoryRouter } from "react-router-dom";
// eslint-disable-next-line import/first
import TaskView from "../../../features/tasks/TaskView";
// eslint-disable-next-line import/first
import { fetchTask } from "../../../services/taskService";
// eslint-disable-next-line import/first
import { SingleTaskDTO } from "../../../types/types";
// eslint-disable-next-line import/first
import { AxiosError } from "axios";
// eslint-disable-next-line import/first
import * as reactRouterDom from "react-router-dom";

// Access mock functions from the mocked module
const mockNavigate = (reactRouterDom as any).mockNavigate || jest.fn();
const mockUseParams = (reactRouterDom as any).mockUseParams || jest.fn();

const mockedFetchTask = fetchTask as jest.MockedFunction<typeof fetchTask>;

describe("TaskView", () => {
  const projectId = "project-123";
  const taskId = "task-456";

  beforeEach(() => {
    jest.clearAllMocks();
    // Set up mockUseParams to return the test values
    if (jest.isMockFunction(mockUseParams)) {
      mockUseParams.mockReturnValue({ projectId, taskId });
    } else {
      // Access from the mocked module if needed
      const mockedModule = reactRouterDom as any;
      if (
        mockedModule.mockUseParams &&
        jest.isMockFunction(mockedModule.mockUseParams)
      ) {
        mockedModule.mockUseParams.mockReturnValue({ projectId, taskId });
      }
    }
  });

  const renderComponent = () => {
    return render(
      <MemoryRouter>
        <TaskView />
      </MemoryRouter>
    );
  };

  describe("Loading state", () => {
    it("should show loader while fetching task", async () => {
      // Mock a delayed response
      mockedFetchTask.mockImplementation(
        () =>
          new Promise((resolve) => {
            setTimeout(() => {
              resolve({
                projectTaskTitle: "Test Task",
                projectTaskDescription: "Test Description",
              } as SingleTaskDTO);
            }, 100);
          })
      );

      renderComponent();

      expect(screen.getByTestId("loader")).toBeInTheDocument();
      expect(screen.getByText("Loading task details...")).toBeInTheDocument();
    });

    it("should hide loader after task is loaded", async () => {
      const mockTask: SingleTaskDTO = {
        projectTaskTitle: "Test Task",
        projectTaskDescription: "Test Description",
      };

      mockedFetchTask.mockResolvedValueOnce(mockTask);

      renderComponent();

      await waitFor(() => {
        expect(screen.queryByTestId("loader")).not.toBeInTheDocument();
      });

      expect(screen.getByText("Test Task")).toBeInTheDocument();
    });
  });

  describe("Task display", () => {
    it("should display task details when task is loaded", async () => {
      const mockTask: SingleTaskDTO = {
        projectTaskTitle: "My Task",
        projectTaskDescription: "Task description here",
      };

      mockedFetchTask.mockResolvedValueOnce(mockTask);

      renderComponent();

      await waitFor(() => {
        expect(screen.getByText("My Task")).toBeInTheDocument();
      });

      expect(screen.getByText("Task description here")).toBeInTheDocument();
      expect(screen.getByText("Task Details")).toBeInTheDocument();
      expect(screen.getByText("Title:")).toBeInTheDocument();
      expect(screen.getByText("Description:")).toBeInTheDocument();
    });

    it("should display 'No description provided' when task has no description", async () => {
      const mockTask: SingleTaskDTO = {
        projectTaskTitle: "Task Without Description",
        projectTaskDescription: "",
      };

      mockedFetchTask.mockResolvedValueOnce(mockTask);

      renderComponent();

      await waitFor(() => {
        expect(screen.getByText("No description provided")).toBeInTheDocument();
      });
    });

    it("should display task with null description as 'No description provided'", async () => {
      const mockTask: SingleTaskDTO = {
        projectTaskTitle: "Task With Null Description",
        projectTaskDescription: null as unknown as string,
      };

      mockedFetchTask.mockResolvedValueOnce(mockTask);

      renderComponent();

      await waitFor(() => {
        expect(screen.getByText("No description provided")).toBeInTheDocument();
      });
    });
  });

  describe("Navigation", () => {
    it("should navigate to edit page when Edit Task button is clicked", async () => {
      const mockTask: SingleTaskDTO = {
        projectTaskTitle: "Test Task",
        projectTaskDescription: "Test Description",
      };

      mockedFetchTask.mockResolvedValueOnce(mockTask);

      renderComponent();

      await waitFor(() => {
        expect(screen.getByText("Test Task")).toBeInTheDocument();
      });

      const editButton = screen.getByRole("button", { name: "Edit Task" });
      await userEvent.click(editButton);

      expect(mockNavigate).toHaveBeenCalledWith(
        `/projects/${projectId}/tasks/${taskId}/edit`
      );
    });

    it("should navigate back to project when Back to Project button is clicked", async () => {
      const mockTask: SingleTaskDTO = {
        projectTaskTitle: "Test Task",
        projectTaskDescription: "Test Description",
      };

      mockedFetchTask.mockResolvedValueOnce(mockTask);

      renderComponent();

      await waitFor(() => {
        expect(screen.getByText("Test Task")).toBeInTheDocument();
      });

      const backButton = screen.getByRole("button", {
        name: "Back to Project",
      });
      await userEvent.click(backButton);

      expect(mockNavigate).toHaveBeenCalledWith(`/projects/${projectId}`);
    });
  });

  describe("Error handling", () => {
    it("should show empty state when task is not found (404 error)", async () => {
      const error = new AxiosError("Not Found");
      error.response = {
        status: 404,
        statusText: "Not Found",
        data: { error: "Task not found" },
        headers: {},
        config: {} as any,
      };

      mockedFetchTask.mockRejectedValueOnce(error);

      renderComponent();

      await waitFor(() => {
        expect(screen.getByTestId("empty-state")).toBeInTheDocument();
      });

      expect(screen.getByText("Task Not Found")).toBeInTheDocument();
      expect(
        screen.getByText("The task you're looking for could not be found.")
      ).toBeInTheDocument();
    });

    it("should show empty state when fetchTask throws network error", async () => {
      const error = new Error("Network Error");
      mockedFetchTask.mockRejectedValueOnce(error);

      renderComponent();

      await waitFor(() => {
        expect(screen.getByTestId("empty-state")).toBeInTheDocument();
      });

      expect(screen.getByText("Task Not Found")).toBeInTheDocument();
    });

    it("should navigate back to project when clicking action button in empty state", async () => {
      const error = new AxiosError("Not Found");
      error.response = {
        status: 404,
        statusText: "Not Found",
        data: { error: "Task not found" },
        headers: {},
        config: {} as any,
      };

      mockedFetchTask.mockRejectedValueOnce(error);

      renderComponent();

      await waitFor(() => {
        expect(screen.getByTestId("empty-state")).toBeInTheDocument();
      });

      const backButton = screen.getByRole("button", {
        name: "Back to Project",
      });
      await userEvent.click(backButton);

      expect(mockNavigate).toHaveBeenCalledWith(`/projects/${projectId}`);
    });
  });

  describe("URL parameters", () => {
    it("should call fetchTask with correct projectId and taskId from URL params", async () => {
      const mockTask: SingleTaskDTO = {
        projectTaskTitle: "Test Task",
        projectTaskDescription: "Test Description",
      };

      mockedFetchTask.mockResolvedValueOnce(mockTask);

      renderComponent();

      await waitFor(() => {
        expect(mockedFetchTask).toHaveBeenCalledWith(projectId, taskId);
      });
    });

    it("should not call fetchTask when projectId is missing", () => {
      mockUseParams.mockReturnValue({ projectId: undefined, taskId });

      renderComponent();

      expect(mockedFetchTask).not.toHaveBeenCalled();
    });

    it("should not call fetchTask when taskId is missing", () => {
      mockUseParams.mockReturnValue({ projectId, taskId: undefined });

      renderComponent();

      expect(mockedFetchTask).not.toHaveBeenCalled();
    });

    it("should not call fetchTask when both params are missing", () => {
      mockUseParams.mockReturnValue({
        projectId: undefined,
        taskId: undefined,
      });

      renderComponent();

      expect(mockedFetchTask).not.toHaveBeenCalled();
    });
  });

  describe("Component structure", () => {
    it("should render task details in a card", async () => {
      const mockTask: SingleTaskDTO = {
        projectTaskTitle: "Test Task",
        projectTaskDescription: "Test Description",
      };

      mockedFetchTask.mockResolvedValueOnce(mockTask);

      renderComponent();

      await waitFor(() => {
        expect(screen.getByText("Task Details")).toBeInTheDocument();
      });

      // Check for card structure
      expect(screen.getByText("Test Task")).toBeInTheDocument();
    });

    it("should render both action buttons", async () => {
      const mockTask: SingleTaskDTO = {
        projectTaskTitle: "Test Task",
        projectTaskDescription: "Test Description",
      };

      mockedFetchTask.mockResolvedValueOnce(mockTask);

      renderComponent();

      await waitFor(() => {
        expect(
          screen.getByRole("button", { name: "Edit Task" })
        ).toBeInTheDocument();
      });

      expect(
        screen.getByRole("button", { name: "Back to Project" })
      ).toBeInTheDocument();
    });
  });
});
